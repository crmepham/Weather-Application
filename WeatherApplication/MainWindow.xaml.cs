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
        // define class variables
        List<String> searchCriteria;
        Day day;
        Location loc;
        WeatherApplicationClassLibrary.Settings set;

        public MainWindow()
        {
            InitializeComponent();
            


            // instantiate class variables
            searchCriteria = new List<String>();
            set = new WeatherApplicationClassLibrary.Settings();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // attempt to load data from .txt and .xml files
            try
            {

            // read settings from file
            set.readSettingsFile();

            // load saved postcode
            searchCriteria.Add(set.Postcode);

            // get woeid using saved postcode
            set.updateWOEID(searchCriteria);

            // load todays info from saved postcode
            day = new Day();
            day.updateDay(set.WOEID);

            // load location info from settings
            loc = new Location();
            loc.updateLocation(set.WOEID);

            // set the data context for the various labels to display the correct data
            dayInfo.DataContext = day.Weather;
            lastBuildDateLabel.DataContext = day;
            townLabel.DataContext = loc;
            Forecast1.DataContext = day.Weather.ForecastList[0];
            Forecast2.DataContext = day.Weather.ForecastList[1];
            Forecast3.DataContext = day.Weather.ForecastList[2];
            Forecast4.DataContext = day.Weather.ForecastList[3];
            Forecast5.DataContext = day.Weather.ForecastList[4];

            // set default location of  map
            Microsoft.Maps.MapControl.WPF.Location l = new Microsoft.Maps.MapControl.WPF.Location(Convert.ToDouble(loc.Latitude), Convert.ToDouble(loc.Longitude));
            Map.SetView(l, 11);
                
            }

            catch (Exception ex)
            {
            // display any errors in the error window
            Error errorWindow = new Error();
            errorWindow.Show();
            errorWindow.errorMessage.Text = ex.Message + "\n" + ex.StackTrace;

            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            // close program
            Environment.Exit(0);
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Settings settingsWindow = new Settings(loc, set, day);
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
            searchCriteria.Clear();
            searchCriteria.Add(townSearchTextbox.Text);
            searchCriteria.Add(countySearchTextbox.Text);
            searchCriteria.Add(postcodeSearchTextbox.Text);

            if (isValidSearchCriteria(searchCriteria))
            {
                formatSearchCriteria(searchCriteria);
                set.updateWOEID(searchCriteria);
                if (set.WOEID.Length > 0)
                {
                    // update location
                    loc.updateLocation(set.WOEID);

                    // update day
                    day.updateDay(set.WOEID);

                    // update map location
                    Microsoft.Maps.MapControl.WPF.Location l = new Microsoft.Maps.MapControl.WPF.Location(Convert.ToDouble(loc.Latitude), Convert.ToDouble(loc.Longitude));
                    Map.SetView(l, 11);

                }
                else
                {
                    displayError("Error: Could not retrieve WOEID, please try again.");
                }
            }
            else
            {
                displayError("Error: Please provide one or more search criteria.");
            }
            
        }

        private void displayError(string errorMessage)
        {
            // display any errors in the error window
            Error errorWindow = new Error();
            errorWindow.Show();
            errorWindow.errorMessage.Text = errorMessage;
        }

        private List<String> formatSearchCriteria(List<String> searchCriteria)
        {
            List<String> result = new List<string>();

            for (int i = 0; i < searchCriteria.Count; i++)
            {
                String searchTerm = searchCriteria.ElementAt(i);
                searchTerm = searchTerm.Replace(" ", "");
                searchTerm = searchTerm.Trim();

                result.Add(searchCriteria.ElementAt(i));
            }

            return result;
        }

        private Boolean isValidSearchCriteria(List<String> searchCriteria)
        {
            Boolean isValid = false;


            for (int i = 0; i < searchCriteria.Count; i++ )
            {
                String searchTerm = searchCriteria.ElementAt(i);
                searchTerm = searchTerm.Replace(" ", "");
                searchTerm = searchTerm.Trim();

                if (searchTerm.Length > 0)
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Map.ZoomLevel = e.NewValue;
        }

        private void getWeatherButton_Click(object sender, RoutedEventArgs e)
        {
            // clear search list
            searchCriteria.Clear();
            
            // add lat, long to search list
            searchCriteria.Add(Map.Center.Latitude.ToString());
            searchCriteria.Add(Map.Center.Longitude.ToString());

            // get new woeid
            formatSearchCriteria(searchCriteria);
            set.updateWOEID(searchCriteria);

            // update location
            loc.updateLocation(set.WOEID);

            // update day
            day.updateDay(set.WOEID);

        }

        private void Map_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            slider1.Value = Map.ZoomLevel;
        }

        private void Map_MouseDown(object sender, MouseButtonEventArgs e)
        {
            slider1.Value = Map.ZoomLevel;
        }
    }
}
