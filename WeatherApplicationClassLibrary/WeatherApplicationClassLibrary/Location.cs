using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.ComponentModel;

namespace WeatherApplicationClassLibrary
{
    public class Location : INotifyPropertyChanged
    {
        #region class fields
        private String town;
        private String postcode;
        private String county;
        private String latitude;
        private String longitude;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region class properties
        public String Town
        {
            get { return town; }
            set 
            {
                town = value;
                OnPropertyChanged("Town");
            }
        }

        public String Postcode
        {
            get { return postcode; }
            set { postcode = value; }
        }

        public String County
        {
            get { return county; }
            set { county = value; OnPropertyChanged("County"); }
        }
        
        public String Latitude
        {
            get { return latitude; }
            set { latitude = value; OnPropertyChanged("Latitude"); }
        }
        
        public String Longitude
        {
            get { return longitude; }
            set { longitude = value; OnPropertyChanged("Longitude"); }
        }
        #endregion


        public Location()
        {
            Town = "";
            County = "";
            Latitude = "";
            Longitude = "";
        }

        // notify interface of changes to property values
        private void OnPropertyChanged(string Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }

        /// <summary>
        /// <para>Will update the Day properties. Run this when searching for a new locations weather data.</para>
        /// </summary>
        /// <param name="woeid">The id of the location</param>
        public void updateLocation(String woeid)
        {
            // clear existing location data incase not all data is present to replace existing data
            resetLocationFields();

            // build the search uri query
            String query = String.Format("http://weather.yahooapis.com/forecastrss?w={0}", woeid);

            // Instantiate a new XML document retrieved from the uri
            XmlDocument weatherData = new XmlDocument();
            weatherData.Load(query);

            // traverse the node list in the XML document to where the relevant data is located
            XmlNode channel = weatherData.SelectSingleNode("rss").SelectSingleNode("channel");
            XmlNamespaceManager man = new XmlNamespaceManager(weatherData.NameTable);

            // establish the two namespaced to access the nodes that have prefixes
            man.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");
            man.AddNamespace("geo", "http://www.w3.org/2003/01/geo/wgs84_pos#");

            // assign the Locations attributes the relevant node content
            Town = checkIfNull(channel.SelectSingleNode("yweather:location", man).Attributes["city"].Value);
            County = checkIfNull(channel.SelectSingleNode("yweather:location", man).Attributes["region"].Value);
            Latitude = checkIfNull(channel.SelectSingleNode("item").SelectSingleNode("geo:lat", man).InnerText);
            Longitude = checkIfNull(channel.SelectSingleNode("item").SelectSingleNode("geo:long", man).InnerText);
            
        }

        /// <summary>
        /// Used to set field value in cases where this data could not be retrieved from the XML document
        /// </summary>
        /// <param name="arg">XML document node.</param>
        /// <returns>Value of XML node or an empty string</returns>
        private String checkIfNull(String arg)
        {
            String result = " ";

            if (arg == null)
            {
                return result;
            }
            else
            {
                return arg;
            }
        }

        // resets the class fields incase this data is not retrieved from the XML document
        private void resetLocationFields()
        {
            town = "";
            county = "";
            latitude = "";
            longitude = "";
        }
    }
}
