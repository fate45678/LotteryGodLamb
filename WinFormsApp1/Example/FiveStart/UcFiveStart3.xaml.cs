using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppTest.AP;

namespace WpfAppTest
{
    /// <summary>
    /// UcFiveStart3.xaml 的互動邏輯
    /// </summary>
    public partial class UcFiveStart3 : UserControl
    {
        public UcFiveStart3()
        {
            InitializeComponent();
        }

        bool IsFirstTime = true;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsFirstTime)
            {
                cbl012.ItemsSource = DB.CombinationAllNumber(5, 0, 2).OrderBy(x=>x.Code);
                IsFirstTime = false;
            }
        }
    }
}
