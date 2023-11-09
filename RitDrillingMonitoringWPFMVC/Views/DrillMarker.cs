using GMap.NET.WindowsPresentation;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using GMap.NET;

namespace RitDrillingMonitoringWPFMVC.Views
{
    internal class DrillMarker: GMapMarker
    {
        private bool _isSelected;
        public int Id { get; set; }
        public string Title { get; set; }
        public string PositionTitle { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                _pinSrcImage = null;
                Shape = IsSelected ? CreatePinImage(this, new Uri("pack://application:,,,/Resources/drill selected.png", UriKind.Absolute)) : CreatePinImage(this, new Uri("pack://application:,,,/Resources/drill.png", UriKind.Absolute));
            }
        }

        private BitmapImage? _pinSrcImage;
        public DrillMarker(PointLatLng pos, int id, string title, string positionTitle) : base(pos)
        {
            Id = id;
            Title = title;
            PositionTitle = positionTitle;
            IsSelected = false;

        }

        private Image CreatePinImage(GMapMarker marker, Uri pathUri)
        {
            var img = new Image
            {
                Tag = marker,
                Width = 32,
                Height = 32
            };

            if (_pinSrcImage == null)
            {
                _pinSrcImage = new BitmapImage(pathUri);
                _pinSrcImage.Freeze();
            }
            img.Source = _pinSrcImage;
            marker.Offset = new Point(-img.Width / 2, -img.Height / 2);
            return img;
        }

        
    }

}
