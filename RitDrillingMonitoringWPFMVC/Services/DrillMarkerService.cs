using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using RitDrillingMonitoringWPFMVC.Models;
using RitDrillingMonitoringWPFMVC.Views;

namespace RitDrillingMonitoringWPFMVC.Services
{
    internal class DrillMarkerService
    {
        public static List<DrillMarker> GetDrillMarkers()
        {
            var list = new List<DrillMarker>();
            using var db = new RitnavSystemForDrillMachinesContext();
            var listDrill = db.DrillMachines.ToList();
            foreach (var drill in listDrill)
            {
                list.Add(new DrillMarker(new PointLatLng(drill.Latitude, drill.Longitude),
                    drill.IddrillingMachine, drill.Title, drill.PositionTag));
            }
            return list;
        }

        public  DrillMarker UpdateDrillMarker(DrillMachine drillMachine)
        {
            
            using var db = new RitnavSystemForDrillMachinesContext();
            var oldDrillMarker = db.DrillMachines.FirstOrDefault(d => d.IddrillingMachine == drillMachine.IddrillingMachine);
            var newDrillMarker = new DrillMarker(new PointLatLng(drillMachine.Latitude, drillMachine.Longitude), drillMachine.IddrillingMachine, drillMachine.Title, drillMachine.PositionTag);
            if (oldDrillMarker != null)
            {
                db.PositionsDrillMachines.Add(new PositionsDrillMachine()
                {
                    Date = DateTime.Now,
                    IddrillMachine = newDrillMarker.Id,
                    Latitude = oldDrillMarker.Latitude,
                    Longitude = oldDrillMarker.Longitude,
                    PositionTag = oldDrillMarker.PositionTag
                });
                db.DrillPolygons.Load();
                oldDrillMarker.PositionTag = String.Empty;
                var holes = HoleMarkerService.GetDrillMarkers();
                foreach (var hole in holes)
                {
                    if (hole.IsInRange5(newDrillMarker.Position.Lat, newDrillMarker.Position.Lng))
                    {
                        
                        var holeInDb = db.DrillHoles.FirstOrDefault(h => h.IddrillHole == hole.Id);
                        oldDrillMarker.PositionTag = $"{holeInDb.IddrillPolygonNavigation.DesignatingTag} {holeInDb.DesignatingTag}";
                    }
                }
                db.CoordinatesDrillPolygons.Load();
                if (oldDrillMarker.PositionTag == String.Empty)
                {
                    var polygons = db.DrillPolygons.ToList();
                    foreach (var polygon in polygons)
                    {
                        var poygonDrill = new DrillingPolygon(polygon.DrillingPolygonCoordinatesLatLngs,
                            polygon.IddrillPolygon);
                        if (poygonDrill.IsInside(newDrillMarker.Position))
                        {
                            oldDrillMarker.PositionTag = $"{polygon.DesignatingTag}";
                        }
                    }
                }
                if (oldDrillMarker.PositionTag == String.Empty)
                {
                    oldDrillMarker.PositionTag = $"Вне блока";
                }

                oldDrillMarker.Latitude = (float)newDrillMarker.Position.Lat;
                oldDrillMarker.Longitude = (float)newDrillMarker.Position.Lng;
                db.DrillMachines.Update(oldDrillMarker);
                db.SaveChanges();
            }
            return newDrillMarker;
        }
    }
}
