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
    public partial class UcTwoStart3 : UserControl
    {
        public UcTwoStart3()
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

        List<BaseOptions> Data2;
        void SetData()
        {
            /*CheckBoxList*/
            cblOption1.ItemsSource = DB.CombinationNumber(2, 0, 2).OrderBy(x => x.Code);
            cblOption2.ItemsSource = DB.CombinationNumber(2, 0, 2, new string[3] { "小", "中", "大" }).OrderByDescending(x => x.Code);
            cblOption3.ItemsSource = DB.TwoStartOption1().OrderBy(x => x.ID);

            /*RadioButtonList*/
            Data2 = DB.CreateOption(1, 2, new string[2] { "保留", "排除" });
            rblOption1.ItemsSource = Data2;
            rblOption2.ItemsSource = Data2;
            rblOption3.ItemsSource = Data2;

            /*預設值*/
            SetDefaultValue();
        }

        /// <summary>
        /// 設定預設值
        /// </summary>
        public void SetDefaultValue()
        {
            IsSetting = true;
            /*CheckBoxList*/
            cblOption1.Clear();
            cblOption2.Clear();
            cblOption3.Clear();

            /*RadioButtonList*/
            rblOption1.SelectedValue = 1;
            rblOption2.SelectedValue = 1;
            rblOption3.SelectedValue = 1;

            cblOption1.VisibleItems = "100000000";
            IsSetting = false;
        }

        /// <summary>
        /// 選取全部選項
        /// </summary>
        public void SelectAll()
        {
            IsSetting = true;
            /*CheckBoxList*/
            cblOption1.SelectedAll();
            cblOption2.SelectedAll();
            cblOption3.SelectedAll();
            IsSetting = false;
        }

        /// <summary>
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            tmp = Calculation.SumNumber(tmp, cblOption1, ((int)rblOption1.SelectedValue == 1));
            tmp = Calculation.CheckValueNumber(tmp, cblOption2, 2, ((int)rblOption2.SelectedValue == 1));
            return tmp;
        }

        bool IsSetting = false;
        private void cbl_SelectedValueChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!IsSetting)
            {
                var cbl = sender as CheckBoxList;
                if (cbl != null)
                {
                    if (cbl.Name == "cblOption1")
                    {
                    }
                }
            }
        }
    }
}
