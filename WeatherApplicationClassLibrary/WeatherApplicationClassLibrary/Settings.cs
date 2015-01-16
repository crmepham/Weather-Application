using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WeatherApplicationClassLibrary
{
    /// <summary>
    /// <para>The Settings class is used to load and write settings data. It is written in a dynamic way so that any number of settings can be
    /// added or removed to the settings.txt file.</para>
    /// </summary>
    class Settings
    {
        // class variables
        private String line;
        private String file;
        private String[] settings;
        public String Postcode
        {
            get { return settings[0]; }
            set { settings[0] = value; }
        }

        public Settings()
        {
            file = "settings.txt";

            try
            {
                using(StreamReader sReader = new StreamReader(file))
                {
                    readSettingsFile(sReader);
                }
                
            }
            catch (IOException e) { Console.Write(e); }

        }

        /// <summary>
        /// This method is used to retrieve the settings data from the settings.txt file.
        /// </summary>
        /// <param name="sr">StreamReader is used to read in the settings.txt file and store the data the line variable.</param>
        /// <returns></returns>
        private String[] readSettingsFile(StreamReader sr)
        {

            try
            {
                using(StreamReader sReader = new StreamReader(file))
                {
                    // read the first line of text
                    while ((line = sr.ReadLine()) != null)
                    {
                        // remove any whitespace
                        line = line.Replace(" ", "");

                        // split the string into an array of settings by any comas found
                        settings = line.Split(',');
                    }
                }
            }
            catch (IOException e) { Console.Write(e); }

            return settings;
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
