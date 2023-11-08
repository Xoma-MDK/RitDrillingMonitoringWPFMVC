using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RitDrillingMonitoringWPFMVC.Models
{
    public class GPGGA
    {
        public TimeSpan time;
        public PointLatLng point;
        public int solutionType;
        public int usedSatellitesCount;
        public double geometricFactor;
        public double altitudeAboveSea;
        public double heightOfGeoidAboveEllipsoid;
        public bool isEmpty = true;

        public GPGGA() { }

        public GPGGA(TimeSpan time, PointLatLng point, int solutionType, int usedSatellitesCount, double geometricFactor, double altitudeAboveSea, double heightOfGeoidAboveEllipsoid)
        {
            this.time = time;
            this.point = point;
            this.solutionType = solutionType;
            this.usedSatellitesCount = usedSatellitesCount;
            this.geometricFactor = geometricFactor;
            this.altitudeAboveSea = altitudeAboveSea;
            this.heightOfGeoidAboveEllipsoid = heightOfGeoidAboveEllipsoid;
            isEmpty = false;
        }
    }
}
