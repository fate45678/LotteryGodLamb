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

namespace WinFormsApp1
{
    /// <summary>
    /// bigbuttom.xaml 的互動邏輯
    /// </summary>
    public partial class bigbuttom : UserControl
    {
        public bigbuttom()
        {
            InitializeComponent();
            UpdateProgressBar();
        }

        private void GDMaster_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateProgressBar()
        {
            double Sum = 90.0;
            int count = 0;
            pg.Value = Sum / count;
            lbpercent.Content = (Sum / count) + " %";
        }
    }
}
