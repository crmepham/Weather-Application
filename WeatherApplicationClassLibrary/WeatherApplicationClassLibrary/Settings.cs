using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace WeatherApplicationClassLibrary
{
    /// <summary>
    /// <para>The Settings class is used to load and write settings data. It is written in a dynamic way so that any number of settings can be
    /// added or removed to the settings.txt file.</para>
    /// </summary>
    public class Settings
    {
        // class variables
        private String line;
        private String file;
        private String[] settings;
        private String woeid;
        private String devKey;

        public String WOEID 
        {
            get { return woeid; }
        }

        public String Postcode
        {
            get { return settings[0]; }
            set { settings[0] = value; }
        }

        public Settings()
        {
            file = "settings.txt";
            devKey = "dj0yJmk9TmFteGt6WDFTWFBkJmQ9WVdrOVpIaG1iVGhrTTJNbWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD1kZA";
        }

        public void updateWOEID(List<String> searchCriteria)
        {
            String query = buildWOEIDQuery(searchCriteria);

            XmlDocument woeidData = new XmlDocument();
            woeidData.Load(query);

            XmlNodeList node = woeidData.GetElementsByTagName("woeid");

            woeid = node[0].InnerText;
        }


        private String buildWOEIDQuery(List<String> searchCriteria)
        {
            String query = "http://where.yahooapis.com/v1/places.q('";
            //for (int i = 0; i < searchCriteria.Count; i++)
            //{
            //    query += (String)searchCriteria[i] + ",";
            //}

            //query = query.TrimEnd(',');
            //query += "')?appid=" + devKey;


            // refactored
            query += string.Join(",", searchCriteria);
            query += "')?appid=" + devKey;

            return query;
        }

        /// <summary>
        /// This method is used to retrieve the settings data from the settings.txt file.
        /// </summary>
        /// <param name="sr">StreamReader is used to read in the settings.txt file and store the data the line variable.</param>
        /// <returns></returns>
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
                }
            }
            catch (IOException e) { Console.Write(e); }
        }

        /// <summary>
        /// The writeSettingsFile method if used to write a dynamic number of settings to the settings.txt file.
        /// </summary>
        /// <param name="settings">settings is a String array used to store the various settings that will be written to the setting.txt file.</param>
        public void writeSettingsFile(String[] settings)
        {
            try
            {
                using(StreamWriter sWriter = new StreamWriter(file))
                {
                    line = "";
                    // dynamically build the String of text that will be saved to the file. It is written in a dynamic way incase more settings are added in future
                    for (int i = 0; i < settings.Length; i++ )
                    {
                        // don't add a coma after the last setting or any empty element will exist in the string array when retrieveing the settings
                        if (i != settings.Length)
                        {
                            line += settings[i] + ",";
                        }
                        else
                        {
                            line += settings[i];
                        }
                    }

                    // write the string of settings to the settings.txt file
                    sWriter.WriteLine(line);
                }
            }
            catch (IOException e) { Console.Write(e); }
            
        }
    }
}
