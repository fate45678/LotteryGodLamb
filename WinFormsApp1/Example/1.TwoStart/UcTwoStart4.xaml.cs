using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp.Custom;
using Wpf.Base;

namespace WpfAppTest
{
    /// <summary>
    /// UcTwoStart.xaml 的互動邏輯
    /// </summary>
    public partial class UcTwoStart4 : UserControl
    {
        public UcTwoStart4()
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
            cblFilter.ItemsSource = Calculation.CreateContinueNumber(0, 18, 2);
            cblOption1.ItemsSource = Calculation.ZeroOneCombination(1, '奇', '偶').OrderByDescending(x => x.Code);
            cblOption2.ItemsSource = Calculation.CreateContinueNumber(0, 2);
            cblType1.ItemsSource = CustomOption.TwoStartSumType1();
            cblType2.ItemsSource = CustomOption.TwoStartSumType2();
            cblType3.ItemsSource = CustomOption.TwoStartSumType3();
            cblType4.ItemsSource = CustomOption.TwoStartSumType4();

            /*RadioButtonList*/
            Data2 = Calculation.CreateOption(1, 2, new string[2] { "保留", "排除" });
            rblOption.ItemsSource = Data2;

            /*預設值*/
            SetDefaultValue();
        }

        bool IsSetting = false;
        /// <summary>
        /// 設定預設值
        /// </summary>
        public void SetDefaultValue()
        {
            IsSetting = true;
            /*CheckBoxList*/
            cblFilter.Clear();
            cblOption1.Clear();
            cblOption2.Clear();
            cblType1.Clear();
            cblType2.Clear();
            cblType3.Clear();
            cblType4.Clear();

            /*RadioButtonList*/
            rblOption.SelectedValue = 1;
            IsSetting = false;
        }

        /// <summary>
        /// 選取全部選項
        /// </summary>
        public void SelectAll()
        {
            IsSetting = true;
            /*CheckBoxList*/
            cblFilter.SelectedAll();
            //cblOption1.SelectedAll();
            //cblOption2.SelectedAll();
            //cblType1.SelectedAll();
            //cblType2.SelectedAll();
            //cblType3.SelectedAll();
            //cblType4.SelectedAll();
            IsSetting = false;
        }

        /// <summary>
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            tmp = Calculation.SumNumber(tmp, cblFilter, ((int)rblOption.SelectedValue == 1));
            return tmp;
        }

        /// <summary>
        /// 切換Filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbl_SelectedValueChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!IsFirstTime && !IsSetting)
            {
                if (cblOption1.IsSelectAll || cblOption2.IsSelectAll || 
                    cblType1.IsSelectAll || cblType2.IsSelectAll || cblType3.IsSelectAll || cblType4.IsSelectAll)
                    cblFilter.SelectedAll();
                else if (cblOption1.IsClear || cblOption2.IsClear ||
                         cblType1.IsClear || cblType2.IsClear || cblType3.IsClear || cblType4.IsClear)
                    cblFilter.Clear();
                else
                {
                    var tmp = cblFilter.SelectedValue.ToArray();
                    switch (((CheckBoxList)sender).Name)
                    {
                        case "cblOption1": //奇偶
                            if (cblOption1.CheckItem.Tag != null)
                            {
                                if ((int)cblOption1.CheckItem.Tag == 1)
                                    tmp[1] = tmp[3] = tmp[5] = tmp[7] = tmp[9] = tmp[11] = tmp[13] = tmp[15] = tmp[17] = ((bool)cblOption1.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblOption1.CheckItem.Tag == 2)
                                    tmp[0] = tmp[2] = tmp[4] = tmp[6] = tmp[8] = tmp[10] = tmp[12] = tmp[14] = tmp[16] = tmp[18] = ((bool)cblOption1.CheckItem.IsChecked ? '1' : '0');
                            }
                            break;
                        case "cblOption2": //012路
                            if (cblOption2.CheckItem.Tag != null)
                            {
                                if ((int)cblOption2.CheckItem.Tag == 1)
                                    tmp[0] = tmp[3] = tmp[6] = tmp[9] = tmp[12] = tmp[15] = tmp[18] = ((bool)cblOption2.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblOption2.CheckItem.Tag == 2)
                                    tmp[1] = tmp[4] = tmp[7] = tmp[10] = tmp[13] = tmp[16] = ((bool)cblOption2.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblOption2.CheckItem.Tag == 3)
                                    tmp[2] = tmp[5] = tmp[8] = tmp[11] = tmp[14] = tmp[17] = ((bool)cblOption2.CheckItem.IsChecked ? '1' : '0');
                            }
                            break;
                        case "cblType1": //和值　分類一
                            if (cblType1.CheckItem.Tag != null)
                            {
                                if ((int)cblType1.CheckItem.Tag == 1)
                                    tmp[0] = tmp[1] = tmp[2] = tmp[3] = tmp[4] = tmp[5] = tmp[6] = tmp[7] = ((bool)cblType1.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType1.CheckItem.Tag == 2)
                                    tmp[7] = tmp[8] = tmp[9] = tmp[10] = tmp[11] = ((bool)cblType1.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType1.CheckItem.Tag == 3)
                                    tmp[11] = tmp[12] = tmp[13] = tmp[14] = tmp[15] = tmp[16] = tmp[17] = tmp[18] = ((bool)cblType1.CheckItem.IsChecked ? '1' : '0');
                            }
                            break;
                        case "cblType2": //和值　分類二
                            if (cblType2.CheckItem.Tag != null)
                            {
                                if ((int)cblType2.CheckItem.Tag == 1)
                                    tmp[7] = tmp[8] = tmp[9] = tmp[10] = tmp[11] = ((bool)cblType2.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType2.CheckItem.Tag == 2)
                                    tmp[6] = tmp[7] = tmp[8] = tmp[9] = tmp[10] = tmp[11] = tmp[12] = ((bool)cblType2.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType2.CheckItem.Tag == 3)
                                    tmp[0] = tmp[1] = tmp[2] = tmp[3] = tmp[4] = tmp[5] = tmp[13] = tmp[14] = tmp[15] = tmp[16] = tmp[17] = tmp[18] = ((bool)cblType2.CheckItem.IsChecked ? '1' : '0');
                            }
                            break;
                        case "cblType3": //和值　分類三
                            if (cblType3.CheckItem.Tag != null)
                            {
                                if ((int)cblType3.CheckItem.Tag == 1)
                                    tmp[0] = tmp[1] = tmp[2] = tmp[3] = tmp[4] = tmp[5] = tmp[6] = ((bool)cblType3.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType3.CheckItem.Tag == 2)
                                    tmp[7] = tmp[8] = tmp[9] = tmp[10] = tmp[11] = ((bool)cblType3.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType3.CheckItem.Tag == 3)
                                    tmp[12] = tmp[13] = tmp[14] = tmp[15] = tmp[16] = tmp[17] = tmp[18] = ((bool)cblType3.CheckItem.IsChecked ? '1' : '0');
                            }
                            break;
                        case "cblType4": //和值　分類四
                            if (cblType4.CheckItem.Tag != null)
                            {
                                if ((int)cblType4.CheckItem.Tag == 1)
                                    tmp[0] = tmp[1] = tmp[2] = tmp[3] = ((bool)cblType4.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType4.CheckItem.Tag == 2)
                                    tmp[4] = tmp[5] = tmp[6] = tmp[7] = tmp[8] = ((bool)cblType4.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType4.CheckItem.Tag == 3)
                                    tmp[9] = tmp[10] = tmp[11] = tmp[12] = tmp[13] = ((bool)cblType4.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType4.CheckItem.Tag == 4)
                                    tmp[14] = tmp[15] = tmp[16] = tmp[17] = tmp[18] = ((bool)cblType4.CheckItem.IsChecked ? '1' : '0');
                            }
                            break;
                    }
                    cblFilter.SelectedValue = new string(tmp);
                }
            }
        }

    }
}
