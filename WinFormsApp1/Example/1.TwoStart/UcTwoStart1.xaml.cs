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
using WpfAppTest.Base;

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

        /// <summary>
        /// 設定控件
        /// </summary>
        void SetData()
        {
            /*CheckBoxList*/
            var Data = DB.CreateContinueNumber();
            cblFixTen.ItemsSource = Data;
            cblFixUnit.ItemsSource = Data;
            cblAnyOne.ItemsSource = Data;

            /*RadioButtonList*/
            var Data2 = DB.CreateOption(1, 2, new string[2] { "保留", "排除" });
            rblFixTen.ItemsSource = Data2;
            rblFixUnit.ItemsSource = Data2;
            rblAnyOne.ItemsSource = Data2;

            /*預設值*/
            SetDefaultValue();
        }

        /// <summary>
        /// 設定預設值
        /// </summary>
        public void SetDefaultValue()
        {
            /*CheckBoxList*/
            cblFixTen.Clear();
            cblFixUnit.Clear();
            cblAnyOne.Clear();

            /*RadioButtonList*/
            rblFixTen.SelectedValue = 1;
            rblFixUnit.SelectedValue = 1;
            rblAnyOne.SelectedValue = 1;

            /*TextBox*/
            teLastFiveStart.Text = "";
        }

        /// <summary>
        /// 選取全部選項
        /// </summary>
        public void SelectAll()
        {
            /*CheckBoxList*/
            cblFixTen.SelectedAll();
            cblFixUnit.SelectedAll();
            cblAnyOne.SelectedAll();
        }

        /// <summary>
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            tmp = Base.Calculation.PosNumber(tmp, cblFixTen,  "0" , (int)rblFixTen.SelectedValue == 1);
            tmp = Base.Calculation.PosNumber(tmp, cblFixUnit,  "1" , (int)rblFixUnit.SelectedValue == 1);
            tmp = Base.Calculation.PosNumber(tmp, cblAnyOne, "*" , (int)rblAnyOne.SelectedValue == 1);
            return tmp;
        }
    }
}
