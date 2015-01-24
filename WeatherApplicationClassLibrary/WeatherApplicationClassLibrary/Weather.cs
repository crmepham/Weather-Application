using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;

namespace WeatherApplicationClassLibrary
{
    public class Weather
    {
        private String temperature;
        private String condition;
        private String temperatureHigh;
        private String temperatureLow;
        private String windSpeed;
        private String humidy;
        private String sunrise;
        private String sunset;

        public Weather()
        {

        }

        

        // get the weather
        public String updateWeather(String woeid)
        {
            String query = String.Format("http://weather.yahooapis.com/forecastrss?w={0}", woeid);

            XmlDocument weatherData = new XmlDocument();
            weatherData.Load(query);

            XmlNode channel = weatherData.SelectSingleNode("rss").SelectSingleNode("channel");
            XmlNamespaceManager man = new XmlNamespaceManager(weatherData.NameTable);
            man.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            
            temperature     = channel.SelectSingleNode("yweather:condition", man).Attributes["temp"].Value;
            condition       = channel.SelectSingleNode("yweather:condition", man).Attributes["text"].Value;
            temperatureHigh = channel.SelectSingleNode("yweather:location", man).Attributes["region"].Value;

            return "";
        }

        // make sure WOEID is a number

        // load weather
    }
}
