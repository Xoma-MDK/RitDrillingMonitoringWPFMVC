using RitDrillingMonitoringWPFMVC.Models;
using RitDrillingMonitoringWPFMVC.Views;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GMap.NET.WindowsPresentation;

namespace RitDrillingMonitoringWPFMVC.Services
{
    internal class DrillPolygonService
    {
        public static List<DrillingPolygon> GetDrillPolygons(GMapControl map)
        {
            var list = new List<DrillingPolygon>();
            using var db = new RitnavSystemForDrillMachinesContext();
            var listPotygons = db.DrillPolygons.ToList();
            db.CoordinatesDrillPolygons.Load();
            foreach (var polygon in listPotygons)
            {
                list.Add(new DrillingPolygon(polygon.DrillingPolygonCoordinatesLatLngs, polygon.IddrillPolygon, map));
            }
            return list;
        }
    }
}
