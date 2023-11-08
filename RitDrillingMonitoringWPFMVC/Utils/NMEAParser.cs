using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RitDrillingMonitoringWPFMVC.Models;

namespace RitDrillingMonitoringWPFMVC.Utils
{
    public static class NMEAParser
    {
        static readonly Regex GpggaRegex = new(@"\$GPGGA,\d+\.*\d*,\d+\.\d+,(N|S),\d+\.\d+,(E|W),\d,\d+,\d+\.*\d*,\d+\.*\d*,M,\d+\.*\d*,M,\d*,.*");

        public static GPGGA ParseGPGGA(string data)
        {
            Match match = GpggaRegex.Match(data);
            if (!match.Success) return new GPGGA();
            data = match.Value;
            string[] parts = data.Split(',');
            TimeSpan UTCTime = ParseUTCTime(parts[1]);
            double latitude = ParseLatitude(parts[2], parts[3]);
            double longtitude = ParseLongtitude(parts[4], parts[5]);
            PointLatLng point = new(latitude, longtitude);
            int solutionType = int.Parse(parts[6]);
            int satellitesUsedCount = int.Parse(parts[7]);
            double geometricFactor = double.Parse(parts[8]);
            double altitudeAboveSea = double.Parse(parts[9]);
            double heightOfGeoidAboveEllipsoid = double.Parse(parts[11]);

            return new GPGGA(UTCTime, point, solutionType, satellitesUsedCount, geometricFactor, altitudeAboveSea, heightOfGeoidAboveEllipsoid);
        }

        private static TimeSpan ParseUTCTime(string part)
        {
            int hours = int.Parse(part.Substring(0, 2));
            int minutes = int.Parse(part.Substring(2, 2));
            int seconds = int.Parse(part.Substring(4, 2));
            TimeSpan UTCtime = new(hours, minutes, seconds);
            return UTCtime;
        }

        private static double ParseLatitude(string part, string side)
        {
            part = part.Replace(".", "");
            part = part.Insert(2, ",");
            double latitude = double.Parse(part);
            if (side == "S")
                latitude = -latitude;
            return latitude;
        }

        private static double ParseLongtitude(string part, string side)
        {
            part = part.Replace(".", "");
            part = part.Insert(3, ",");
            double longtitude = double.Parse(part);
            if (side == "W")
                longtitude = -longtitude;
            return longtitude;
        }
    }
}
