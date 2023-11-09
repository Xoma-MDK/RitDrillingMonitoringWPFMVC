using GMap.NET.WindowsPresentation;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using GMap.NET;

namespace RitDrillingMonitoringWPFMVC.Views
{
    internal class HoleMarker : GMapMarker
    {
        public int Id { get; set; }
        private BitmapImage? _pinSrcImage = null;

        public HoleMarker(PointLatLng pos, int id) : base(pos)
        {
            Id = id;
            Shape = CreatePinImage(this);
        }
        private Image CreatePinImage(GMapMarker marker)
        {
            var img = new Image
            {
                Tag = marker,
                Width = 8,
                Height = 8
            };

            if (_pinSrcImage == null)
            {
                _pinSrcImage = new BitmapImage(new Uri("pack://application:,,,/Resources/Hole.png", UriKind.Absolute));
                _pinSrcImage.Freeze();
            }
            img.Source = _pinSrcImage;
            marker.Offset = new Point(-img.Width / 2, -img.Height / 2);
            return img;
        }
        public bool IsInRange5(double otherLatitude, double otherLongitude)
        {
            double a = 6378137;
            double phi1 = ToRadians(Position.Lat);
            double phi2 = ToRadians(otherLatitude);
            double deltaPhi = ToRadians(otherLatitude - Position.Lat);
            double deltaLambda = ToRadians(otherLongitude - Position.Lng);

            double aSinAlpha = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                               Math.Cos(phi1) * Math.Cos(phi2) *
                               Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(aSinAlpha), Math.Sqrt(1 - aSinAlpha));

            double u1 = Math.Atan2(Math.Cos(phi2) * Math.Sin(deltaLambda), Math.Cos(phi1) * Math.Sin(phi2) - Math.Sin(phi1) * Math.Cos(phi2) * Math.Cos(deltaLambda));
            double u2 = Math.Atan2(Math.Cos(phi1) * Math.Sin(deltaLambda), -Math.Sin(phi1) * Math.Cos(phi2) + Math.Cos(phi1) * Math.Sin(phi2) * Math.Cos(deltaLambda));

            double sinSigma = Math.Sqrt((Math.Cos(u2) * Math.Cos(u2)) * Math.Sin(c) * Math.Sin(c) + (Math.Cos(u1) * Math.Sin(u2) - Math.Sin(u1) * Math.Cos(u2) * Math.Cos(c)) * (Math.Cos(u1) * Math.Sin(u2) - Math.Sin(u1) * Math.Cos(u2) * Math.Cos(c)));
            double cosSigma = Math.Sin(u1) * Math.Sin(u2) + Math.Cos(u1) * Math.Cos(u2) * Math.Cos(c);

            double sigma = Math.Atan2(sinSigma, cosSigma);

            double distance = a * sigma;

            return distance <= 5;
        }

        private static double ToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}
