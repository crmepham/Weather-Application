using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WeatherApplicationClassLibrary
{
    /// <summary>
    /// <para>Used as a data store for a single days weather forecast data</para>
    /// </summary>
    public class Forecast : INotifyPropertyChanged
    {
        #region class variables
        private String day;
        private String date;
        private String hiLow;
        private String condition;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region class properties
        public String Day
        {
            get { return day; }
            set { day = value; OnPropertyChanged("Day"); }
        }
        
        public String Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged("Date"); }
        }

        public String HiLow
        {
            get { return hiLow; }
            set { hiLow = value; OnPropertyChanged("HiLow"); }
        }

        public String ForecastCondition
        {
            get { return condition; }
            set { condition = value; OnPropertyChanged("ForecastCondition"); }
        }
        #endregion

        // notify interface of changes to property values
        private void OnPropertyChanged(string Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
    }
}
