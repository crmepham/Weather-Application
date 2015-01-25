using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace WeatherApplicationClassLibrary
{
    public class Location
    {
        private String town;

        public String Town
        {
            get { return town; }
        }
        private String county;

        public String County
        {
            get { return county; }
        }
        private String postcode;

        public String Postcode
        {
            get { return postcode; }
            set { postcode = value; }
        }
        private String latitude;

        public String Latitude
        {
            get { return latitude; }
        }
        private String longitude;

        public String Longitude
        {
            get { return longitude; }
        }


        public void updateLocation(String woeid)
        {
            String query = String.Format("http://weather.yahooapis.com/forecastrss?w={0}", woeid);

            XmlDocument weatherData = new XmlDocument();

            weatherData.Load(query);

            XmlNode channel = weatherData.SelectSingleNode("rss").SelectSingleNode("channel");
            XmlNamespaceManager man = new XmlNamespaceManager(weatherData.NameTable);
            man.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");
            man.AddNamespace("geo", "http://www.w3.org/2003/01/geo/wgs84_pos#");

            town = channel.SelectSingleNode("yweather:location", man).Attributes["city"].Value;
            county = channel.SelectSingleNode("yweather:location", man).Attributes["region"].Value;

            latitude = channel.SelectSingleNode("item").SelectSingleNode("geo:lat", man).InnerText;
            longitude = channel.SelectSingleNode("item").SelectSingleNode("geo:long", man).InnerText;
            
        }

    }
}
