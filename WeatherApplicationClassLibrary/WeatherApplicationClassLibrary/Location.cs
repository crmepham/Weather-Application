using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WeatherApplicationClassLibrary
{
    public class Location
    {
        private String town;
        private String county;
        private String postcode;
        private String latitude;
        private String longitude;
        
        
        public Location(){
            
        }

        private void updateLocation(String woeid)
        {

            String query = String.Format("http://weather.yahooapis.com/forecastrss?w={0}", woeid);

            XmlDocument weatherData = new XmlDocument();
            weatherData.Load(query);

            XmlNode channel = weatherData.SelectSingleNode("rss").SelectSingleNode("channel");
            XmlNamespaceManager man = new XmlNamespaceManager(weatherData.NameTable);
            man.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            town = channel.SelectSingleNode("yweather:location", man).Attributes["city"].Value;
            county = channel.SelectSingleNode("yweather:location", man).Attributes["region"].Value;
            latitude = channel.SelectSingleNode("item").SelectSingleNode("geo:lat").Value;
            longitude = channel.SelectSingleNode("item").SelectSingleNode("geo:long").Value;

        }

    }
}
