using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeatherApplicationClassLibrary;

namespace WeatherApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        List<String> searchCriteria = new List<String>();
        Day day;
        Location loc;
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // attempt to instantiate methods and load data from .txt and .xml files
            try
            {
                // load settings
                WeatherApplicationClassLibrary.Settings settings = new WeatherApplicationClassLibrary.Settings();

                // read settings from file
                settings.readSettingsFile();

                // load saved postcode
                searchCriteria.Add(settings.Postcode);

                // get woeid using saved postcode
                settings.updateWOEID(searchCriteria);

                // load todays info from saved postcode
                day = new Day();
                day.updateDay(settings.WOEID);

                // load location info from settings
                loc = new Location();
                loc.updateLocation(settings.WOEID);
                loc.Postcode = settings.Postcode;

                dayInfo.DataContext = day.Weather;
                lastBuildDateLabel.DataContext = day;
                townLabel.DataContext = loc;
                Forecast1.DataContext = day.Weather.ForecastList[0];
                Forecast2.DataContext = day.Weather.ForecastList[1];
                Forecast3.DataContext = day.Weather.ForecastList[2];
                Forecast4.DataContext = day.Weather.ForecastList[3];
                Forecast5.DataContext = day.Weather.ForecastList[4];

                // assign values to user interface labels
                
            }

            catch (Exception ex)
            {
                // display any errors in the error window
                Error errorWindow = new Error();
                errorWindow.Show();
                errorWindow.errorMessage.Text = ex.Message + "\n" + ex.StackTrace;

            }

           


        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            // close program
            Environment.Exit(0);
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Settings settingsWindow = new Settings();
            settingsWindow.Show();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            Guide guideWindow = new Guide();
            guideWindow.Show();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            About aboutWindow = new About();
            aboutWindow.Show();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
