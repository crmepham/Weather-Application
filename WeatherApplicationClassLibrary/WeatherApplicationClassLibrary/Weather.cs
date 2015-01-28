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
        public String Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }
        private String condition;
        public String Condition
        {
            get { return condition; }
            set { condition = value; }
        }
        private String windSpeed;

        public String WindSpeed
        {
            get { return windSpeed; }
            set { windSpeed = value; }
        }
        private String humidity;

        public String Humidity
        {
            get { return humidity; }
            set { humidity = value; }
        }
        private String sunrise;

        public String Sunrise
        {
            get { return sunrise; }
            set { sunrise = value; }
        }
        private String sunset;

        public String Sunset
        {
            get { return sunset; }
            set { sunset = value; }
        }
        private String windChill;

        public String WindChill
        {
            get { return windChill; }
            set { windChill = value; }
        }
        private String windDirection;

        public String WindDirection
        {
            get { return windDirection; }
            set { windDirection = value; }
        }

        private List<Forecast> forecastList = new List<Forecast>();

        public List<Forecast> ForecastList
        {
            get { return forecastList; }
            set { forecastList = value; }
        }

        public Weather()
        {

        }

        public Forecast getForecastInList(int id)
        {
            Forecast forecast = ForecastList.ElementAt(id);
            return forecast;
        }

        public void updateForecastList(String woeid)
        {
            
            String query = String.Format("http://weather.yahooapis.com/forecastrss?w={0}", woeid);

            XmlDocument weatherData = new XmlDocument();
            weatherData.Load(query);
            XmlNamespaceManager man = new XmlNamespaceManager(weatherData.NameTable);
            man.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            XmlNode channel = weatherData.SelectSingleNode("rss").SelectSingleNode("channel").SelectSingleNode("item");
            XmlNodeList nodeList =  channel.SelectNodes("yweather:forecast", man);

            int i = 0;
            foreach(XmlNode forecastNode in nodeList)
            {
                
                Forecast forecast = new Forecast();
                forecast.Day = (i == 0) ? "Today" : forecastNode.Attributes["day"].Value;
                forecast.Date = forecastNode.Attributes["date"].Value;
                forecast.HiLow = "Low: " + forecastNode.Attributes["low"].Value + "\u00b0 / Hi: " + forecastNode.Attributes["high"].Value + "\u00b0";
                forecast.ForecastCondition = forecastNode.Attributes["text"].Value;
                this.forecastList.Add(forecast);
                i++;
            }
        }
    }
}
