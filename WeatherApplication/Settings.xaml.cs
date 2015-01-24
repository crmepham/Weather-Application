using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WeatherApplicationClassLibrary;

namespace WeatherApplication
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void SettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // load settings
            WeatherApplicationClassLibrary.Settings settings = new WeatherApplicationClassLibrary.Settings();
            settings.readSettingsFile();

            postCode.Text = settings.Postcode;
        }
    }
}
