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
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // load settings
            WeatherApplicationClassLibrary.Settings settings = new WeatherApplicationClassLibrary.Settings();
            
            // read settings from file
            settings.readSettingsFile();

            // load saved postcode
            searchCriteria.Add(settings.Postcode);

            // get woeid using saved postcode
            settings.updateWOEID(searchCriteria);

            // load todays info
            Day day = new Day(settings.WOEID);
            day.updateDay(settings.WOEID);


            weatherTodayDate.Content = day.Date;
            // --- test
            temperature.Content = day.Name;//settings.WOEID;

            


            

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
    }
}
