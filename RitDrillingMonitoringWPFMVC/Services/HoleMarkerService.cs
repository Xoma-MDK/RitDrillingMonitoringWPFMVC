using GMap.NET;
using RitDrillingMonitoringWPFMVC.Models;
using RitDrillingMonitoringWPFMVC.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RitDrillingMonitoringWPFMVC.Services
{
    internal class HoleMarkerService
    {
        public static List<HoleMarker> GetDrillHoles()
        {
            var list = new List<HoleMarker>();
            using var db = new RitnavSystemForDrillMachinesContext();
            List<DrillHole> listHoles = db.DrillHoles.ToList();
            foreach (DrillHole hole in listHoles)
            {
                list.Add(new HoleMarker(new PointLatLng(hole.Latitude, hole.Longitude), hole.IddrillHole));
            }
            return list;
        }
    }
}
