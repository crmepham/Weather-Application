using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherApplicationClassLibrary
{
    public class Forecast : Weather
    {
        private String day;

        public String Day
        {
            get { return day; }
            set { day = value; }
        }
        private String date;

        public String Date
        {
            get { return date; }
            set { date = value; }
        }

        private String hiLow;

        public String HiLow
        {
            get { return hiLow; }
            set { hiLow = value; }
        }

        private String condition;

        public String ForecastCondition
        {
            get { return condition; }
            set { condition = value; }
        }

        public void updateForecast()
        {

        }
    }

    
}
