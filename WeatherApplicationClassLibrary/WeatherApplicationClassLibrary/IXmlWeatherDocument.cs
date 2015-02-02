using System;

namespace WeatherApplicationClassLibrary
{
    public interface IXmlWeatherDocument
    {
        void updateXmlDocument(String locationId);
        void addNameSpace(String prefix, String uri);
        void updateXmlNodeList(String prefix);
    }
}
