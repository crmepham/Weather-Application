using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WeatherApplicationClassLibrary
{
    public class Forecast : INotifyPropertyChanged
    {
        private String day;

        public String Day
        {
            get { return day; }
            set { day = value; OnPropertyChanged("Day"); }
        }
        private String date;

        public String Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged("Date"); }
        }

        private String hiLow;

        public String HiLow
        {
            get { return hiLow; }
            set { hiLow = value; OnPropertyChanged("HiLow"); }
        }

        private String condition;

        public String ForecastCondition
        {
            get { return condition; }
            set { condition = value; OnPropertyChanged("ForecastCondition"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
    }

    
}
