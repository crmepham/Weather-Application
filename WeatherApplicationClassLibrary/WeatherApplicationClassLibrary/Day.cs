using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.ComponentModel;

namespace WeatherApplicationClassLibrary 
{
    /// <summary>
    /// <para>Represents a single day and will store information about the day including, the weather for the day and for the next four days following</para>
    /// </summary>
    public class Day : INotifyPropertyChanged
    {
        #region Class variable
        private String name;
        private String date;
        private String lastBuildDate;
        private Weather weather;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Class Properties
        public Weather Weather
        {
            get { return weather; }
            set { weather = value; }
        }
        public String LastBuildDate
        {
            get { return lastBuildDate; }
            set { lastBuildDate = value; OnPropertyChanged("LastBuildDate"); }
        }
        public String Name
        {
            get { return name; }
            set 
            { 
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public String Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged("Date"); }
        }
        public Day(String woeid)
        {
            updateDay(woeid);
        }
        #endregion

        public Day()
        {
            weather = new Weather();
        }

        // notify App of changes to Property values 
        private void OnPropertyChanged(string Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }

        /// <summary>
        /// Will update the Day properties. Run this when searching for a new locations weather data.
        /// </summary>
        /// <param name="woeid">The id of the location.</param>
        public void updateDay(String woeid)
        {
            // build the search uri query
            String query = String.Format("http://weather.yahooapis.com/forecastrss?w={0}", woeid);

            // Instantiate a new XML document retrieved from the uri
            XmlDocument weatherData = new XmlDocument();
            weatherData.Load(query);

            // traverse the node list in the XML document to where the relevant data is located
            XmlNode channel = weatherData.SelectSingleNode("rss").SelectSingleNode("channel");

            // establish the two namespaced to access the nodes that have prefixes
            XmlNamespaceManager man = new XmlNamespaceManager(weatherData.NameTable);
            man.AddNamespace("lastBuildDate", "http://xml.weather.yahoo.com/ns/rss/1.0");
            man.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            // get the users choice of temperature (celcius/Fahrenheit)
            Settings settings = new Settings();
            settings.readSettingsFile();
            String tempChoice = settings.TempChoice;

            // assign the Day's attribute the relevant node content
            name = channel.SelectSingleNode("lastBuildDate", man).InnerText.Substring(0,3);
            date = channel.SelectSingleNode("lastBuildDate", man).InnerText.Substring(5, 11);
            lastBuildDate = channel.SelectSingleNode("lastBuildDate", man).InnerText;

            // the days weather attributes
            weather.Condition = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", man).Attributes["text"].Value;
            weather.WindChill = convertTemperature(tempChoice, channel.SelectSingleNode("yweather:wind", man).Attributes["chill"].Value) + "\u00b0";
            weather.WindDirection = channel.SelectSingleNode("yweather:wind", man).Attributes["direction"].Value + "\u00b0";
            weather.WindSpeed = channel.SelectSingleNode("yweather:wind", man).Attributes["speed"].Value + "mph";
            weather.Humidity = convertTemperature(tempChoice, channel.SelectSingleNode("yweather:atmosphere", man).Attributes["humidity"].Value) + "\u00b0";
            weather.Sunrise = channel.SelectSingleNode("yweather:astronomy", man).Attributes["sunrise"].Value;
            weather.Sunset = channel.SelectSingleNode("yweather:astronomy", man).Attributes["sunset"].Value;
            Weather.Temperature = convertTemperature(tempChoice, channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", man).Attributes["temp"].Value) + "\u00b0";
            
            // the forecast for the next 5 days is contained in the weather object
            weather.updateForecastList(woeid);
        }

        /// <summary>
        /// <para>Used to convert the xml temperatures to the users desired temperature.</para>
        /// </summary>
        /// <param name="tempChoice">The users desired temperature.</param>
        /// <param name="temp">The XML temperature in Fahrenheit.</param>
        /// <returns>The converted temperature if different from the XML temperature.</returns>
        private String convertTemperature(String tempChoice, String temp)
        {
            String resultString = null;
            if (tempChoice.Equals("c"))
            {
                // convert fahrenheit temperature to celcius
                int temperature = Convert.ToInt32(temp);

                // conversion formula
                int result = (temperature - 32) * 5 / 9;

                // return converted temperature as a string
                resultString = result.ToString();

            }
            else
            {
                // no conversion required
                resultString = temp;
            }

            return resultString;
        }
    }
}
