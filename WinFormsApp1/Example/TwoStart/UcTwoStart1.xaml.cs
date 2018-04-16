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
    /// UcTwoStart.xaml 的互動邏輯
    /// </summary>
    public partial class UcTwoStart1 : UserControl
    {
        public UcTwoStart1()
        {
            InitializeComponent();
        }

        bool IsFirstTime = true;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsFirstTime)
            {
                SetData();
                IsFirstTime = false;
            }
        }

        List<BaseOptions> Data;

        void SetData()
        {
            Data = DB.ZeroToNine();
            cblFixTen.ItemsSource = Data;
            cblFixUnit.ItemsSource = Data;
            cblAnyOne.ItemsSource = Data;
            
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
