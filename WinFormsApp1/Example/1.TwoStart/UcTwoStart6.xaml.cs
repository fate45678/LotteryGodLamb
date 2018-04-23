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
    public partial class UcTwoStart6 : UserControl
    {
        public UcTwoStart6()
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
            cblFilter.ItemsSource = DB.CreateContinueNumber();
            cblType1.ItemsSource = DB.CreateOption(0, 1, new string[2] { "奇", "偶" });
            cblType2.ItemsSource = DB.CreateOption(0, 1, new string[2] { "大", "小" });
            cblType3.ItemsSource = DB.CreateOption(0, 1, new string[2] { "質", "合" });
            cblType4.ItemsSource = DB.CreateOption(0, 2, new string[3] { "大", "中", "小" });
            cblType5.ItemsSource = DB.CreateContinueNumber(0, 2);
            cblType6.ItemsSource = DB.CreateContinueNumber(0, 4);
            cblSumLastType1.ItemsSource = DB.TwoStart_SumLastType1();
            cblSumLastType2.ItemsSource = DB.TwoStart_SumLastType2();

            /*RadioButtonList*/
            Data2 = DB.CreateOption(1, 2, new string[2] { "保留", "排除" });
            rblFilter.ItemsSource = Data2;

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
            cblFilter.Clear();
            cblType1.Clear();
            cblType2.Clear();
            cblType3.Clear();
            cblType4.Clear();
            cblType5.Clear();
            cblType6.Clear();
            cblSumLastType1.Clear();
            cblSumLastType2.Clear();

            /*RadioButtonList*/
            rblFilter.SelectedValue = 1;
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
            //cblType1.SelectedAll();
            //cblType2.SelectedAll();
            //cblType3.SelectedAll();
            //cblType4.SelectedAll();
            //cblType5.SelectedAll();
            //cblType6.SelectedAll();
            //cblSumLastType1.SelectedAll();
            //cblSumLastType2.SelectedAll();
            IsSetting = false;
        }

        /// <summary>
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            tmp = Calculation.CrossNumber(tmp, cblFilter, ((int)rblFilter.SelectedValue == 1));
            return tmp;
        }

        /// <summary>
        /// 是否設值中
        /// </summary>
        bool IsSetting = false;

        /// <summary>
        /// 切換Filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbl_SelectedValueChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!IsFirstTime && !IsSetting)
            {
                if (cblType1.IsSelectAll || cblType2.IsSelectAll || cblType3.IsSelectAll || cblType4.IsSelectAll || 
                    cblType5.IsSelectAll || cblType6.IsSelectAll || cblSumLastType1.IsSelectAll || cblSumLastType2.IsSelectAll)
                    cblFilter.SelectedAll();
                else if (cblType1.IsClear || cblType2.IsClear || cblType3.IsClear || cblType4.IsClear ||
                         cblType5.IsClear || cblType6.IsClear || cblSumLastType1.IsClear || cblSumLastType2.IsClear)
                    cblFilter.Clear();
                else
                {
                    var tmp = cblFilter.SelectedValue.ToArray();
                    switch (((CheckBoxList)sender).Name)
                    {
                        case "cblType1": //奇偶
                            if (cblType1.CheckItem.Tag != null)
                            {
                                if ((int)cblType1.CheckItem.Tag == 1)
                                    tmp[1] = tmp[3] = tmp[5] = tmp[7] = tmp[9] = ((bool)cblType1.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType1.CheckItem.Tag == 2)
                                    tmp[0] = tmp[2] = tmp[4] = tmp[6] = tmp[8] = ((bool)cblType1.CheckItem.IsChecked ? '1' : '0');
                            }
                            break;
                        case "cblType2": //大小
                            if (cblType2.CheckItem.Tag != null)
                            {
                                if ((int)cblType2.CheckItem.Tag == 1)
                                    tmp[5] = tmp[6] = tmp[7] = tmp[8] = tmp[9] = ((bool)cblType2.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType2.CheckItem.Tag == 2)
                                    tmp[0] = tmp[1] = tmp[2] = tmp[3] = tmp[4] = ((bool)cblType2.CheckItem.IsChecked ? '1' : '0');
                            }
                            break;
                        case "cblType3": //質合
                            if (cblType3.CheckItem.Tag != null)
                            {
                                if ((int)cblType3.CheckItem.Tag == 1)
                                    tmp[1] = tmp[2] = tmp[3] = tmp[5] = tmp[7] = ((bool)cblType3.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType3.CheckItem.Tag == 2)
                                    tmp[0] = tmp[4] = tmp[6] = tmp[8] = tmp[9] = ((bool)cblType3.CheckItem.IsChecked ? '1' : '0');
                            }
                            break;
                        case "cblType4": //大中小
                            if (cblType4.CheckItem.Tag != null)
                            {
                                if ((int)cblType4.CheckItem.Tag == 3)
                                    tmp[0] = tmp[1] = tmp[2] = tmp[3] = ((bool)cblType4.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType4.CheckItem.Tag == 2)
                                    tmp[4] = tmp[5] = tmp[6] = ((bool)cblType4.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType4.CheckItem.Tag == 1)
                                    tmp[7] = tmp[8] = tmp[9] = ((bool)cblType4.CheckItem.IsChecked ? '1' : '0');
                            }
                            break;
                        case "cblType5": //012路
                            if (cblType5.CheckItem.Tag != null)
                            {
                                if ((int)cblType5.CheckItem.Tag == 1)
                                    tmp[0] = tmp[3] = tmp[6] = tmp[9] = ((bool)cblType5.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType5.CheckItem.Tag == 2)
                                    tmp[1] = tmp[4] = tmp[7] = ((bool)cblType5.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType5.CheckItem.Tag == 3)
                                    tmp[2] = tmp[5] = tmp[8] = ((bool)cblType5.CheckItem.IsChecked ? '1' : '0');
                            }
                            break;
                        case "cblType6": //???
                            if (cblType6.CheckItem.Tag != null)
                            {
                                if ((int)cblType6.CheckItem.Tag == 1)
                                    tmp[0] = tmp[2] = tmp[5] = tmp[8] = ((bool)cblType6.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType6.CheckItem.Tag == 2)
                                    tmp[1] = tmp[6] = ((bool)cblType6.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType6.CheckItem.Tag == 3)
                                    tmp[2] = tmp[7] = ((bool)cblType6.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType6.CheckItem.Tag == 4)
                                    tmp[3] = tmp[8] = ((bool)cblType6.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblType6.CheckItem.Tag == 5)
                                    tmp[4] = tmp[9] = ((bool)cblType6.CheckItem.IsChecked ? '1' : '0');
                            }
                            break;
                        case "cblSumLastType1": //跨度　分類一
                            if (cblSumLastType1.CheckItem.Tag != null)
                            {
                                if ((int)cblSumLastType1.CheckItem.Tag == 1)
                                    tmp[0] = tmp[3] = tmp[4] = tmp[8] = ((bool)cblSumLastType1.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblSumLastType1.CheckItem.Tag == 2)
                                    tmp[2] = tmp[6] = tmp[7] = ((bool)cblSumLastType1.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblSumLastType1.CheckItem.Tag == 3)
                                    tmp[1] = tmp[5] = tmp[9] = ((bool)cblSumLastType1.CheckItem.IsChecked ? '1' : '0');
                            }
                            break;
                        case "cblSumLastType2": //跨度　分類二
                            if (cblSumLastType2.CheckItem.Tag != null)
                            {
                                if ((int)cblSumLastType2.CheckItem.Tag == 1)
                                    tmp[2] = tmp[4] = tmp[9] = ((bool)cblSumLastType2.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblSumLastType2.CheckItem.Tag == 2)
                                    tmp[0] = tmp[3] = tmp[5] = tmp[7] = ((bool)cblSumLastType2.CheckItem.IsChecked ? '1' : '0');
                                else if ((int)cblSumLastType2.CheckItem.Tag == 3)
                                    tmp[1] = tmp[6] = tmp[8] = ((bool)cblSumLastType2.CheckItem.IsChecked ? '1' : '0');
                            }
                            break;
                    }
                    cblFilter.SelectedValue = new string(tmp);
                }
            }
        }
    }
}
