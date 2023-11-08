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
    internal class DrillMachineService
    {
        public static List<DrillMachine> GetDrillMachines()
        {
            using var db = new RitnavSystemForDrillMachinesContext();
            return db.DrillMachines.ToList();
        }

        public static DrillMachine CreateDrillMachine(string title, double Latitude,double Longitude)
        {
            var drillMachine = new DrillMachine()
            {
                Title = title,
                PositionTag = "Вне блока",
                Longitude = (float)Longitude,
                Latitude = (float)Latitude,
            };
            using var db = new RitnavSystemForDrillMachinesContext();
            db.DrillMachines.Add(drillMachine);
            db.SaveChanges();
            return drillMachine;
        }
    }
}
