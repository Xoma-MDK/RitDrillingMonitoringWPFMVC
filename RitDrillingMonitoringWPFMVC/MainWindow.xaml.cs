using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using RitDrillingMonitoringWPFMVC.Models;
using RitDrillingMonitoringWPFMVC.Services;
using RitDrillingMonitoringWPFMVC.Views;
using WebSocketSharp;

namespace RitDrillingMonitoringWPFMVC
{
    public partial class MainWindow : Window
    {
        private DrillMarker? _currentElement;
        public MainWindow()
        {
            InitializeComponent();
            InitMap();
            Dispatcher.Invoke(() =>
            {
                new API((o)=> UpdateMap(o)).Init();
            });
            LoadDrillMarkers();
            LoadDrillingPolygons();
            LoadHoleMarkers();
        }

        private void InitMap()
        {
            MapControl.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            MapControl.Zoom = 11;
            MapControl.Position = new PointLatLng(55.008111, 82.921768);
            MapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;
            MapControl.DragButton = MouseButton.Right;
            MapControl.ShowCenter = false;
        }

        private void UpdateMap(object drill)
        {
            Dispatcher.Invoke(() =>
            {
                new DrillMarkerService().UpdateDrillMarker((DrillMachine)drill);
                MapControl.Markers.Clear();
                LoadDrillMarkers();
                LoadDrillingPolygons();
                LoadHoleMarkers();
            });
            
        }
        public void LoadDrillMarkers()
        {
            try
            {

                foreach (var drillMarker in DrillMarkerService.GetDrillMarkers())
                {
                    MapControl.Markers.Add(drillMarker);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки буровых установок!" + ex.Message, "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
      
        public void LoadDrillingPolygons()
        {
            try
            {
                using var db = new RitnavSystemForDrillMachinesContext();
                var listPotygons = db.DrillPolygons.ToList();
                db.CoordinatesDrillPolygons.Load();
                foreach (var polygon in listPotygons)
                {
                    MapControl.Markers.Add(new Views.DrillingPolygon(polygon.DrillingPolygonCoordinatesLatLngs, polygon.IddrillPolygon));
                }
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки буровых установок!", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void LoadHoleMarkers()
        {
            try
            {
                using var db = new RitnavSystemForDrillMachinesContext();
                var listHoles = db.DrillHoles.ToList();
                foreach (var hole in listHoles)
                {
                    MapControl.Markers.Add(new HoleMarker(new PointLatLng(hole.Latitude, hole.Longitude), hole.IddrillHole));
                }
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки скважин!", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        private void MarkerClick(DrillMarker marker)
        {
            Debug.Print(marker.Id.ToString());
            marker.Map.Position = marker.Position;
            marker.Map.Zoom = 16;
            MapControl.Markers.Remove(marker);
            marker.IsSelected = true;
            MapControl.Markers.Add(marker);
            TbLatitude.Text = marker.Position.Lat.ToString(CultureInfo.InvariantCulture);
            TbLongtitude.Text = marker.Position.Lng.ToString(CultureInfo.InvariantCulture);
            TbStatus.Text = marker.PositionTitle;
            Panel.Visibility = Visibility.Visible;

        }

        private HitTestResultBehavior HitTestCallback(HitTestResult result)
        {
            if (result.VisualHit is Image image)
            {
                _currentElement = image.Tag as DrillMarker;
                if (_currentElement != null) MarkerClick(_currentElement);
                return HitTestResultBehavior.Stop;
            }
            return HitTestResultBehavior.Continue;
        }

        private void MapControlDrill_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_currentElement != null)
            {
                ((DrillMarker)MapControl.Markers[MapControl.Markers.IndexOf(_currentElement)]).IsSelected = false;
                TbLatitude.Text = string.Empty;
                TbLongtitude.Text = string.Empty;
                TbStatus.Text = string.Empty;
                Panel.Visibility = Visibility.Collapsed;
                _currentElement = null;
            }
            if (_currentElement == null)
            {
                Point pt = e.GetPosition(MapControl);
                var parameters = new PointHitTestParameters(pt);
                VisualTreeHelper.HitTest(MapControl, null, HitTestCallback, parameters);
            }
        }
        #region Custom Top Panel
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = (this.WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        #endregion
    }
}
