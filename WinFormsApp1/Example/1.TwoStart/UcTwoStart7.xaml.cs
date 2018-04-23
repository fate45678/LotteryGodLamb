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
    public partial class UcTwoStart7 : UserControl
    {
        public UcTwoStart7()
        {
            InitializeComponent();
        }

        List<BaseOptions> Data2;
        void SetData()
        {
            /*RadioButtonList*/
            Data2 = DB.CreateOption(1, 2, new string[2] { "保留", "排除" });
            rblOption1.ItemsSource = Data2;
            rblOption2.ItemsSource = Data2;
            rblOpiton3.ItemsSource = Data2;

            /*預設值*/
            SetDefaultValue();
        }

        /// <summary>
        /// 設定預設值
        /// </summary>
        public void SetDefaultValue()
        {
            /*RadioButtonList*/
            rblOption1.SelectedValue = 1;
            rblOption2.SelectedValue = 1;
            rblOpiton3.SelectedValue = 1;

            teSum1.Text = "";
            teSum2.Text = "";
            teSum3.Text = "";
            teSum4.Text = "";
        }
    }
}
