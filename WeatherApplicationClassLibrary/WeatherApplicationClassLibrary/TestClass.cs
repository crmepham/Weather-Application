using System;
using System.Text;
using System.ComponentModel;
using System.Xml;

namespace WeatherApplicationClassLibrary
{
    public class TestClass : INotifyPropertyChanged
    {
        private String name;

        public String Name
        {
            get { return name; }
            set 
            { 
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        public TestClass()
        {
            name = "John";
        }

        public void updateName()
        {
            name = "";

            name = "test";
            //String query = String.Format("http://weather.yahooapis.com/forecastrss?w={0}", "26489883");

            //XmlDocument weatherData = new XmlDocument();

            //weatherData.Load(query);

            //XmlNode channel = weatherData.SelectSingleNode("rss").SelectSingleNode("channel");
            //XmlNamespaceManager man = new XmlNamespaceManager(weatherData.NameTable);
            //man.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");
            //man.AddNamespace("geo", "http://www.w3.org/2003/01/geo/wgs84_pos#");

            //name = channel.SelectSingleNode("yweather:location", man).Attributes["city"].Value;
        }
    }
}
