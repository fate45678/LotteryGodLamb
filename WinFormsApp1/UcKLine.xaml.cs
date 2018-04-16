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
    /// UcKLine.xaml 的互動邏輯
    /// </summary>
    public partial class UcKLine : UserControl
    {
        public UcKLine()
        {
            InitializeComponent();
        }
        private void Cv_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public class DrawPic
        {
            Line x_axis = new Line();
            Line y_axis = new Line();
            int global_x0 = 0;
            int global_y0 = 0;
            int global_xLength = 0;
            int global_yLength = 0;
            Canvas LocalCv;
            public DrawPic() { }
        }
    }
}
