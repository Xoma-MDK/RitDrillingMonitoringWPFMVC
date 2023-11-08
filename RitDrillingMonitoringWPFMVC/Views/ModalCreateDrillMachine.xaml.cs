using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RitDrillingMonitoringWPFMVC.Views
{
    /// <summary>
    /// Логика взаимодействия для ModalGreateDrillMachine.xaml
    /// </summary>
    public partial class ModalCreateDrillMachine : Window
    {
        public string Title => TbTitle.Text;

        public ModalCreateDrillMachine()
        {
            InitializeComponent();
        }
        #region Custom Top Panel
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(TbTitle.Text != String.Empty) DialogResult = true;
        }
    }
}
