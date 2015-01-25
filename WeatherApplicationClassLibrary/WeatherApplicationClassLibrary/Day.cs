using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;

namespace WeatherApplicationClassLibrary
{
    public class Day
    {
        private String name;
        private String date;
        private String lastBuildDate;
        

        public String LastBuildDate
        {
            get { return lastBuildDate; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        public String Date
        {
            get { return date; }
            set { date = value; }
        }
        public Day(String woeid)
        {
            updateDay(woeid);
        }

        public Day()
        {
            
        }

        public void updateDay(String woeid)
        {
           String query = String.Format("http://weather.yahooapis.com/forecastrss?w={0}", woeid);

            XmlDocument weatherData = new XmlDocument();
            weatherData.Load(query);

            XmlNode channel = weatherData.SelectSingleNode("rss").SelectSingleNode("channel");
            XmlNamespaceManager man = new XmlNamespaceManager(weatherData.NameTable);
            man.AddNamespace("lastBuildDate", "http://xml.weather.yahoo.com/ns/rss/1.0");

            name = channel.SelectSingleNode("lastBuildDate", man).InnerText.Substring(0,3);
            date = channel.SelectSingleNode("lastBuildDate", man).InnerText.Substring(5, 11);
            lastBuildDate = channel.SelectSingleNode("lastBuildDate", man).InnerText;
        }

        public String todaysDate()
        {
            String result = name + " " + date;
            return result;
        }
    }
}
