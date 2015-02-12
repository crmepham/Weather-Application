using System;
namespace WeatherApplicationClassLibrary
{
interface IXmlAccessManager
{
void addNameSpace(string prefix, string uri);
System.Xml.XmlNode Channel {get; }
System.Xml.XmlNamespaceManager NamespaceManager {get; set; }
void updateXmlDocument(string woeid);
void updateXmlNodeList(string prefix);
System.Xml.XmlNodeList XmlNodeList {get; set; }
}
}
