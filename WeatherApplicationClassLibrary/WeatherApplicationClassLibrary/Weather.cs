using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel;

namespace WeatherApplicationClassLibrary
{
    /// <summary>
    /// <para>Contains the full weather information for the day today</para>
    /// </summary>
    public class Weather : INotifyPropertyChanged
    {
        #region class fields
        private String temperature;
        private String condition;
        private String windSpeed;
        private String humidity;
        private String sunrise;
        private String sunset;
        private String windChill;
        private String windDirection;
        private List<Forecast> forecastList;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region class properties
        public String Temperature
        {
            get { return temperature; }
            set { temperature = value; OnPropertyChanged("Temperature"); }
        }
        
        public String Condition
        {
            get { return condition; }
            set { condition = value; OnPropertyChanged("Condition"); }
        }
        
        public String WindSpeed
        {
            get { return windSpeed; }
            set { windSpeed = value; OnPropertyChanged("WindSpeed"); }
        }
        
        public String Humidity
        {
            get { return humidity; }
            set { humidity = value; OnPropertyChanged("Humidity"); }
        }
        
        public String Sunrise
        {
            get { return sunrise; }
            set { sunrise = value; OnPropertyChanged("Sunrise"); }
        }
        
        public String Sunset
        {
            get { return sunset; }
            set { sunset = value; OnPropertyChanged("Sunset"); }
        }
        
        public String WindChill
        {
            get { return windChill; }
            set { windChill = value; OnPropertyChanged("WindChill"); }
        }
        
        public String WindDirection
        {
            get { return windDirection; }
            set { windDirection = value; OnPropertyChanged("WindDirection"); }
        }

        public List<Forecast> ForecastList
        {
            get { return forecastList; }
            set { forecastList = value; OnPropertyChanged("ForecastList"); }
        }
        #endregion

        // notify App of changes to Property values 
        private void OnPropertyChanged(string Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }

        public Weather()
        {
            // instantiate the 5 day forecast list
            forecastList = new List<Forecast>();

            // add the forecast objects to the list of forecasts
            for (int i = 0; i < 5; i++)
            {
                Forecast forecast = new Forecast();
                ForecastList.Add(forecast);
            }
        }

        /// <summary>
        /// Used to update the list of forecast objects containing basic weather data for the next 5 days
        /// </summary>
        /// <param name="woeid">The id of the location</param>
        public void updateForecastList(XmlAccessManager xm)
        {

            // update xm nodelist
            xm.updateXmlNodeList("yweather:forecast");

            // get the users temperature choice (celcius/fahrenheit)
            Settings settings = new Settings();
            settings.readSettingsFile();

            int i = 0;

            // iterate through each of the nodes in the node list
            foreach(XmlNode forecastNode in xm.XmlNodeList)
            {
                // assign the nodes attribute data to the equivalant forecast object fields
                ForecastList.ElementAt(i).Day = (i == 0) ? "Today" : forecastNode.Attributes["day"].Value;
                ForecastList.ElementAt(i).Date = forecastNode.Attributes["date"].Value;
                ForecastList.ElementAt(i).HiLow = "Low: " + convertTemperature(settings.TempChoice, forecastNode.Attributes["low"].Value) + "\u00b0 / Hi: " + convertTemperature(settings.TempChoice, forecastNode.Attributes["high"].Value) + "\u00b0";
                ForecastList.ElementAt(i).ForecastCondition = forecastNode.Attributes["text"].Value;
                i++;
            }
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
