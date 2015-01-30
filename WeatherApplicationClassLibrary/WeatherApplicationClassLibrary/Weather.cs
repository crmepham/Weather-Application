using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel;

namespace WeatherApplicationClassLibrary
{
    public class Weather : INotifyPropertyChanged
    {
        private String temperature;
        public String Temperature
        {
            get { return temperature; }
            set { temperature = value; OnPropertyChanged("Temperature"); }
        }
        private String condition;
        public String Condition
        {
            get { return condition; }
            set { condition = value; OnPropertyChanged("Condition"); }
        }
        private String windSpeed;

        public String WindSpeed
        {
            get { return windSpeed; }
            set { windSpeed = value; OnPropertyChanged("WindSpeed"); }
        }
        private String humidity;

        public String Humidity
        {
            get { return humidity; }
            set { humidity = value; OnPropertyChanged("Humidity"); }
        }
        private String sunrise;

        public String Sunrise
        {
            get { return sunrise; }
            set { sunrise = value; OnPropertyChanged("Sunrise"); }
        }
        private String sunset;

        public String Sunset
        {
            get { return sunset; }
            set { sunset = value; OnPropertyChanged("Sunset"); }
        }
        private String windChill;

        public String WindChill
        {
            get { return windChill; }
            set { windChill = value; OnPropertyChanged("WindChill"); }
        }
        private String windDirection;

        public String WindDirection
        {
            get { return windDirection; }
            set { windDirection = value; OnPropertyChanged("WindDirection"); }
        }

        private List<Forecast> forecastList = new List<Forecast>();

        public List<Forecast> ForecastList
        {
            get { return forecastList; }
            set { forecastList = value; OnPropertyChanged("ForecastList"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }

        public Weather()
        {
            for (int i = 0; i < 5; i++)
            {
                Forecast forecast = new Forecast();
                ForecastList.Add(forecast);
            }
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

                Settings settings = new Settings();
                settings.readSettingsFile();
                String tempChoice = settings.TempChoice;

                ForecastList.ElementAt(i).Day = (i == 0) ? "Today" : forecastNode.Attributes["day"].Value;
                ForecastList.ElementAt(i).Date = forecastNode.Attributes["date"].Value;
                ForecastList.ElementAt(i).HiLow = "Low: " + convertTemperature(tempChoice, forecastNode.Attributes["low"].Value) + "\u00b0 / Hi: " + convertTemperature(tempChoice, forecastNode.Attributes["high"].Value) + "\u00b0";
                ForecastList.ElementAt(i).ForecastCondition = forecastNode.Attributes["text"].Value;
                i++;
            }
        }

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
