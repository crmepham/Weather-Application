using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.ComponentModel;

namespace WeatherApplicationClassLibrary 
{
    /// <summary>
    /// <para>The Settings class is used to load and write settings data. 
    /// It is written in a dynamic way so that any number of settings can be
    /// added or removed to the settings.txt file.</para>
    /// </summary>
    public class Settings : INotifyPropertyChanged
    {
        #region class variables
        private String line;
        private String file;
        private String[] settings;
        private String woeid;
        private String devKey;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region class properties
        public String WOEID 
        {
            get { return woeid; }
            set { woeid = value; }
        }

        public String Postcode
        {
            get { return settings[0]; }
            set 
            { 
                settings[0] = value;
                OnPropertyChanged("Postcode");
            }
        }

        public String TempChoice
        {
            get { return settings[1]; }
            set { settings[1] = value; }
        }
        #endregion

        public Settings()
        {
            file = "settings.txt";
            devKey = "dj0yJmk9TmFteGt6WDFTWFBkJmQ9WVdrOVpIaG1iVGhrTTJNbWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD1kZA";
        }

        // notify interface of changes to property values
        private void OnPropertyChanged(string Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Postcode));
        }

        /// <summary>
        /// Get the woeid of the location based on the search terms in the search criteria list
        /// </summary>
        /// <param name="searchCriteria">Provides a list of search terms.</param>
        public void updateWOEID(List<String> searchCriteria)
        {
            // build the uri query based on the search terms in list
            String query = buildWOEIDQuery(searchCriteria);

            // Instantiate a new XML document retrieved from the uri
            XmlDocument woeidData = new XmlDocument();
            woeidData.Load(query);

            // attempt to get the woeid from the XML document
            try
            {
                XmlNodeList node = woeidData.GetElementsByTagName("woeid");

                woeid = node[0].InnerText;
            }
            catch (Exception e)
            {
                // if no woeid is present assign an empty string to woeid
                woeid = "";

                Console.Write(e.Message);
            }
           

        }

        /// <summary>
        /// dynamically builds a uri query using a list of search terms
        /// </summary>
        /// <param name="searchCriteria">Provides a list of search terms</param>
        /// <returns>A full uri query to retrieve a woeid</returns>
        private String buildWOEIDQuery(List<String> searchCriteria)
        {
            String query = "http://where.yahooapis.com/v1/places.q('";
            query += string.Join(",", searchCriteria);
            query += "')?appid=" + devKey;

            return query;
        }

        /// <summary>
        /// Used to set field value in cases where this data could not be retrieved from the XML document
        /// </summary>
        /// <param name="arg">XML document node.</param>
        /// <returns>Value of XML node or an empty string</returns>
        private String checkIfNull(String arg)
        {
            String result = "";

            if (arg == null)
            {
                return result;
            }
            else
            {
                return arg;
            }
        }

        /// <summary>
        /// This method is used to retrieve the settings data from the settings.txt file.
        /// </summary>
        /// <param name="sr">StreamReader is used to read in the settings.txt 
        /// file and store the data the line variable.</param>
        public void readSettingsFile()
        {
            try
            {
                using(StreamReader sReader = new StreamReader(file))
                {
                    // read the first line of text
                    while ((line = sReader.ReadLine()) != null)
                    {
                        // remove any whitespace
                        line = line.Replace(" ", "");

                        // split the string into an array of settings by any comas found
                        settings = line.Split(',');

                    }

                    // if there is no existing postcode, provide one
                    if(settings[0].Equals(""))
                    {
                        settings[0] = "ip333rl";
                    }
                }
            }
            catch (IOException e) { Console.Write(e); }
            
        }

        /// <summary>
        /// The writeSettingsFile method if used to write a dynamic number of settings to the settings.txt file.
        /// </summary>
        /// <param name="settings">settings is a String array used to store the various 
        /// settings that will be written to the setting.txt file.</param>
        public void writeSettingsFile()
        {
            try
            {
                using(StreamWriter sWriter = new StreamWriter(file))
                {
                    // create a comma separated string using the settings fields
                    line = "";
                    line += string.Join(",", settings);

                    // write the string of settings to the settings.txt file
                    sWriter.WriteLine(line);
                }
            }
            catch (IOException e) { Console.Write(e); }
            
        }
    }
}
