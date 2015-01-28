﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.ComponentModel;

namespace WeatherApplicationClassLibrary 
{
    public class Day
    {
        private String name;
        private String date;
        private String lastBuildDate;
        private Weather weather;

        public Weather Weather
        {
            get { return weather; }
            set { weather = value; }
        }

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
            weather = new Weather();
        }

        public void updateDay(String woeid)
        {
            String query = String.Format("http://weather.yahooapis.com/forecastrss?w={0}", woeid);

            XmlDocument weatherData = new XmlDocument();
            weatherData.Load(query);

            XmlNode channel = weatherData.SelectSingleNode("rss").SelectSingleNode("channel");
            XmlNamespaceManager man = new XmlNamespaceManager(weatherData.NameTable);
            man.AddNamespace("lastBuildDate", "http://xml.weather.yahoo.com/ns/rss/1.0");
            man.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            name = channel.SelectSingleNode("lastBuildDate", man).InnerText.Substring(0,3);
            date = channel.SelectSingleNode("lastBuildDate", man).InnerText.Substring(5, 11);
            lastBuildDate = channel.SelectSingleNode("lastBuildDate", man).InnerText;

            weather.Temperature = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", man).Attributes["temp"].Value + "\u00b0";
            weather.Condition = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", man).Attributes["text"].Value;
            weather.WindChill = channel.SelectSingleNode("yweather:wind", man).Attributes["chill"].Value + "\u00b0";
            weather.WindDirection = channel.SelectSingleNode("yweather:wind", man).Attributes["direction"].Value + "\u00b0";
            weather.WindSpeed = channel.SelectSingleNode("yweather:wind", man).Attributes["speed"].Value + "mph";
            weather.Humidity = channel.SelectSingleNode("yweather:atmosphere", man).Attributes["humidity"].Value + "\u00b0";
            weather.Sunrise = channel.SelectSingleNode("yweather:astronomy", man).Attributes["sunrise"].Value;
            weather.Sunset = channel.SelectSingleNode("yweather:astronomy", man).Attributes["sunset"].Value;

            weather.updateForecastList(woeid);


        }

        public String todaysDate()
        {
            String result = name + " " + date;
            return result;
        }
    }
}
