using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace RitDrillingMonitoringWPFMVC.Views
{
    internal class DrillingPolygon : GMapPolygon
    {
        public int Id { get; set; }
        public DrillingPolygon(IEnumerable<PointLatLng> points, int id) : base(points)
        {
            Id = id;
            Shape = Shape = new Path
            {
                Stroke = Brushes.DarkBlue,
                StrokeThickness = 1.5,
                Effect = null,
                Fill = Brushes.DarkBlue,
                Opacity = 0.25

            };
            Points = points.ToList();
        }
        public bool IsInside(PointLatLng p)
        {
            int count = Points.Count;
            if (count < 3)
            {
                return false;
            }

            bool flag = false;
            int i = 0;
            checked
            {
                int index = count - 1;
                for (; i < count; i++)
                {
                    PointLatLng pointLatLng = Points[i];
                    PointLatLng pointLatLng2 = Points[index];
                    if (((pointLatLng.Lat < p.Lat && pointLatLng2.Lat >= p.Lat) || (pointLatLng2.Lat < p.Lat && pointLatLng.Lat >= p.Lat)) && pointLatLng.Lng + (p.Lat - pointLatLng.Lat) / (pointLatLng2.Lat - pointLatLng.Lat) * (pointLatLng2.Lng - pointLatLng.Lng) < p.Lng)
                    {
                        flag = !flag;
                    }

                    index = i;
                }

                return flag;
            }
        }
    }
}
