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
    /// <para>Represents a single day and will store information about the day including, 
    /// the weather for the day and for the next four days following</para>
    /// </summary>
    public class Day : INotifyPropertyChanged
    {
        #region Class variables
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
        public void updateDay(XmlAccessManager xm)
        {

            xm.addNameSpace("lastBuildDate", "http://xml.weather.yahoo.com/ns/rss/1.0");
            xm.addNameSpace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            // get the users choice of temperature (celcius/Fahrenheit)
            Settings settings = new Settings();
            settings.readSettingsFile();
            String tempChoice = settings.TempChoice;

            // assign the Day's attribute the relevant node content
            name = xm.Channel.SelectSingleNode("lastBuildDate", xm.NamespaceManager).InnerText.Substring(0,3);
            date = xm.Channel.SelectSingleNode("lastBuildDate", xm.NamespaceManager).InnerText.Substring(5, 11);
            lastBuildDate = xm.Channel.SelectSingleNode("lastBuildDate", xm.NamespaceManager).InnerText;

            // the days weather attributes
            weather.Condition = xm.Channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", xm.NamespaceManager).Attributes["text"].Value;
            weather.WindChill = convertTemperature(tempChoice, xm.Channel.SelectSingleNode("yweather:wind", xm.NamespaceManager).Attributes["chill"].Value) + "\u00b0";
            weather.WindDirection = xm.Channel.SelectSingleNode("yweather:wind", xm.NamespaceManager).Attributes["direction"].Value + "\u00b0";
            weather.WindSpeed = xm.Channel.SelectSingleNode("yweather:wind", xm.NamespaceManager).Attributes["speed"].Value + "mph";
            weather.Humidity = convertTemperature(tempChoice, xm.Channel.SelectSingleNode("yweather:atmosphere", xm.NamespaceManager).Attributes["humidity"].Value) + "\u00b0";
            weather.Sunrise = xm.Channel.SelectSingleNode("yweather:astronomy", xm.NamespaceManager).Attributes["sunrise"].Value;
            weather.Sunset = xm.Channel.SelectSingleNode("yweather:astronomy", xm.NamespaceManager).Attributes["sunset"].Value;
            Weather.Temperature = convertTemperature(tempChoice, xm.Channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", xm.NamespaceManager).Attributes["temp"].Value) + "\u00b0";
            
            // the forecast for the next 5 days is contained in the weather object
            weather.updateForecastList(xm);
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
