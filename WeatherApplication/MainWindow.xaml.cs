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
        #region class variables
        List<String> searchCriteria;
        Day day;
        Location loc;
        WeatherApplicationClassLibrary.Settings set;

        #endregion

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

        #region The various menu options
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
        #endregion

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            // add the search terms to a list
            searchCriteria.Clear();
            searchCriteria.Add(townSearchTextbox.Text);
            searchCriteria.Add(countySearchTextbox.Text);
            searchCriteria.Add(postcodeSearchTextbox.Text);

            // check if that there is at least one search term 
            if (!isValidSearchCriteria())
            {
                displayError("Please provide one or more search criteria.");
                return;
            }

            // get new woeid based on the new search terms
            set.updateWOEID(searchCriteria);

            // check if a woeid was found
            if (set.WOEID.Length <= 0)
            {
                displayError("Could not retrieve WOEID, please try again.");
                return;
            }

            // update location
            loc.updateLocation(set.WOEID);

            // update day
            day.updateDay(set.WOEID);

            // update map location
            Microsoft.Maps.MapControl.WPF.Location l = new Microsoft.Maps.MapControl.WPF.Location(Convert.ToDouble(loc.Latitude), Convert.ToDouble(loc.Longitude));
            Map.SetView(l, 11);

        }

        /// <summary>
        /// <para>Opens a separate window to display the error message provided to it.</para>
        /// </summary>
        /// <param name="errorMessage">This is the String error message to display in the new error window.</param>
        private void displayError(string errorMessage)
        {
            // display any errors in the error window
            Error errorWindow = new Error();
            errorWindow.Show();
            errorWindow.errorMessage.Text = errorMessage;
        }

        /// <summary>
        /// <para>Check to make sure the list of search terms contains at least one term to search by.</para>
        /// </summary>
        /// <returns>Returns true if at least one search term exists in the search terms list.</returns>
        private Boolean isValidSearchCriteria()
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
            // map the Map zoom level to the slider value
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
            set.updateWOEID(searchCriteria);

            // if no woeid could be found reset it to default
            if (set.WOEID.Equals(""))
            {
                // clear search term list
                searchCriteria.Clear();

                // read settings file to get default postcode 
                set.readSettingsFile();

                // add postcode to search term list
                searchCriteria.Add(set.Postcode);

                // get default location
                set.updateWOEID(searchCriteria);

                // update location
                loc.updateLocation(set.WOEID);

                // update day
                day.updateDay(set.WOEID);

                // update map location
                Microsoft.Maps.MapControl.WPF.Location l = new Microsoft.Maps.MapControl.WPF.Location(Convert.ToDouble(loc.Latitude), Convert.ToDouble(loc.Longitude));
                Map.SetView(l, 11);

                displayError("Could not retrieve location information. Returned to default location.");
                return;
            }

            // update location
            loc.updateLocation(set.WOEID);

            // update day
            day.updateDay(set.WOEID);

        }

        private void Map_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // map the slider to the current map zoom level
            slider1.Value = Map.ZoomLevel;
        }

        private void Map_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // map the slider to the current map zoom level
            slider1.Value = Map.ZoomLevel;
        }
    }
}
