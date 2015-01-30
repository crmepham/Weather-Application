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

        private Location loc;
        private WeatherApplicationClassLibrary.Settings settings;
        private Day day;

        public Settings(Location locIn, WeatherApplicationClassLibrary.Settings settingsIn, Day dayIn)
        {
            InitializeComponent();

            loc = locIn;
            settings = settingsIn;
            day = dayIn;
        }

        private void SettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {

            // load settings
            settings.readSettingsFile();

            postCode.DataContext = settings;

            if (settings.TempChoice.Equals("c"))
            {
                celciusRadioButton.IsChecked = true;
            }
            else
            {
                fahrenheitRadioButton.IsChecked = true;
            }
        }

        private void updateSettingsButton_Click(object sender, RoutedEventArgs e)
        {

            if (celciusRadioButton.IsChecked == true)
            {

                settings.TempChoice = "c";
            }
            else
            {
                settings.TempChoice = "f";
            }

            settings.Postcode = postCode.Text;

            settings.writeSettingsFile();

            //List<String> searchCriteria = new List<string>();
            //searchCriteria.Add(settings.Postcode);
            //settings.updateWOEID(searchCriteria);
            loc.updateLocation(settings.WOEID);

            day.updateDay(settings.WOEID);
            day.Weather.updateForecastList(settings.WOEID);

            this.Close();
        }
    }
}
