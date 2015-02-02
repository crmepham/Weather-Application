using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WeatherApplicationClassLibrary
{
    /// <summary>
    /// <para>Encapsulates all XML document controls.</para>
    /// </summary>
    public class XmlAccessManager
    {
        #region class variables
        private String query;
        private XmlDocument xmlDoc;
        private XmlNode channel;
        private XmlNodeList xmlNodeList;
        private XmlNamespaceManager namespaceManager;
        #endregion

        #region class properties
        public XmlNodeList XmlNodeList
        {
            get { return xmlNodeList; }
            set { xmlNodeList = value; }
        }

        public XmlNode Channel
        {
            get { return channel; }
        }
        

        public XmlNamespaceManager NamespaceManager
        {
            get { return namespaceManager; }
            set { namespaceManager = value; }
        }
        #endregion

        public XmlAccessManager(String woeid)
        {
            updateXmlDocument(woeid);
        }

        /// <summary>
        /// <para>Updates the objects XML document and related variables.</para>
        /// </summary>
        /// <param name="woeid">location ID</param>
        public void updateXmlDocument(String woeid)
        {
            // build the query uri to retrieve xml document
            query = String.Format("http://weather.yahooapis.com/forecastrss?w={0}", woeid);

            // create a new xml document and load it using the query uri
            xmlDoc = new XmlDocument();
            xmlDoc.Load(query);

            // select the base node level
            channel = xmlDoc.SelectSingleNode("rss").SelectSingleNode("channel");

            // establish a namespace manager for any document prefixes
            namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
        }

        /// <summary>
        /// <para>Add a namespace for a node prefix.</para>
        /// </summary>
        /// <param name="prefix">Name of node prefix.</param>
        /// <param name="uri">XML document uri for specified prefix.</param>
        public void addNameSpace(String prefix, String uri)
        {
            // if the prefix hasnt already been added to the namespace manager add it
            if (!namespaceManager.HasNamespace(prefix))
            {
                namespaceManager.AddNamespace(prefix, uri);
            }
        }

        /// <summary>
        /// <para>Create a new node list.</para>
        /// </summary>
        /// <param name="prefix">Node prefix.</param>
        public void updateXmlNodeList(String prefix)
        {
            xmlNodeList = channel.SelectSingleNode("item").SelectNodes(prefix, namespaceManager);
        }
    }
}
