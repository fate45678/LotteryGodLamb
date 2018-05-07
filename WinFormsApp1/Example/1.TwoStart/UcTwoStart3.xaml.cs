using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        void SetData()
        {
            /*CheckBoxList*/
            cblOption1.ItemsSource = DB.CombinationNumber(2, 0, 2).OrderBy(x => x.Code);
            cblOption2.ItemsSource = DB.CombinationNumber(2, 0, 2, new string[3] { "小", "中", "大" }).OrderByDescending(x => x.Code);
            cblOption3.ItemsSource = DB.CreateOption(1, 4, new string[4] { "对子", "连号", "杂号", "假对" }).OrderBy(x => x.ID); //, "假连"

            /*RadioButtonList*/
            var data = DB.CreateOption(1, 2, new string[2] { "保留", "排除" });
            rblOption1.ItemsSource = data;
            rblOption2.ItemsSource = data;
            rblOption3.ItemsSource = data;

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
            //012路
            tmp = Calculation.DivThreeRemainder(tmp, cblOption1, ((int)rblOption1.SelectedValue == 1));

            //大中小
            tmp = Calculation.CheckValueNumber(tmp, cblOption2, 2, ((int)rblOption2.SelectedValue == 1));

            //類型
            tmp = Calculation.TwoStartType(tmp, cblOption3.SelectedValue, ((int)rblOption3.SelectedValue == 1));
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
