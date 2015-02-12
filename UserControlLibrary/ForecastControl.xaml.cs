using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserControlLibrary
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ForecastControl : UserControl
    {
        public ForecastControl()
        {
            InitializeComponent();
        }

        public object Forecast1DC
        {
            get { return Forecast1.DataContext; }
            set { Forecast1.DataContext = value; }
        }

        public object Forecast2DC
        {
            get { return Forecast2.DataContext; }
            set { Forecast2.DataContext = value; }
        }

        public object Forecast3DC
        {
            get { return Forecast3.DataContext; }
            set { Forecast3.DataContext = value; }
        }

        public object Forecast4DC
        {
            get { return Forecast4.DataContext; }
            set { Forecast4.DataContext = value; }
        }

        public object Forecast5DC
        {
            get { return Forecast5.DataContext; }
            set { Forecast5.DataContext = value; }
        }

    }
}
