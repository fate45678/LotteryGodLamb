using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Controls = System.Windows.Controls;
using Forms = System.Windows.Forms;
using System.IO;
using WpfApp.Custom;
using System.Collections;
using Wpf.Base;

namespace WpfAppTest
{
    /// <summary>
    /// UcFiveStart1.xaml 的互動邏輯
    /// </summary>
    public partial class UcFiveStart1 : Controls.UserControl
    {
        public UcFiveStart1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 是否為第一次載入
        /// </summary>
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
        /// 設定資料
        /// </summary>
        void SetData()
        {
            cblData1.ItemsSource = Calculation.ZeroOneCombination(5, '大', '小').OrderByDescending(x => x.Code).ToList();
            cblData2.ItemsSource = Calculation.ZeroOneCombination(5, '奇', '偶').OrderByDescending(x => x.Code).ToList();
            cblData3.ItemsSource = Calculation.ZeroOneCombination(5, '质', '合').OrderByDescending(x => x.Code).ToList();

            var Data = Calculation.CreateContinueNumber().OrderBy(x => x.ID).ToList();
            cblTenThousands.ItemsSource = Data;
            cblThousands.ItemsSource = Data;
            cblHundreds.ItemsSource = Data;
            cblTens.ItemsSource = Data;
            cblUnits.ItemsSource = Data;

            cblSumLast.ItemsSource = Data;
            cblCross.ItemsSource = Data;
            cblComm.ItemsSource = Data;

            //膽碼
            cblNumber1.ItemsSource = Data;
            cblNumber2.ItemsSource = Data;
            cblNumber3.ItemsSource = Data;
            cblNumber4.ItemsSource = Data;
            cblNumber5.ItemsSource = Data;
            cblNumber6.ItemsSource = Data;

            Data = Calculation.CreateOption(0, 5);
            cblNumber1_2.ItemsSource = Data;
            cblNumber2_2.ItemsSource = Data;
            cblNumber3_2.ItemsSource = Data;
            cblNumber4_2.ItemsSource = Data;
            cblNumber5_2.ItemsSource = Data;
            cblNumber6_2.ItemsSource = Data;

            var Ratio = Calculation.Ratio(5);
            cblRatio.ItemsSource = Ratio;
            cblRatio2.ItemsSource = Ratio;
            cblRatio3.ItemsSource = Ratio;
            //cblAC.ItemsSource = Calculation.CreateOption(1, 9);

            //設定寬度
            cblData1.WrapRow(4);
            cblData2.WrapRow(4);
            cblData3.WrapRow(4);

            /*預設值*/
            SetDefaultValue();
        }

        /// <summary>
        /// 開啟檔案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            Forms.OpenFileDialog openFileDialog = new Forms.OpenFileDialog();

            // 設定Filter，指定只能開啟特定的檔案 

            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            // 選擇圖片的Filter

            // openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";

            var tmp = openFileDialog.ShowDialog();
            if (tmp.ToString() == "OK")
            {
                //檢核是否存在檔案
                if (File.Exists(openFileDialog.FileName))
                {
                    //檢核檔名
                    if (!string.IsNullOrEmpty(openFileDialog.FileName))
                    {
                        if (openFileDialog.FileName.Length >= 3 &&
                            openFileDialog.FileName.Substring(openFileDialog.FileName.Length - 3, 3) == "txt")
                            tePos.Text = File.ReadAllText(openFileDialog.FileName);
                        else
                            System.Windows.Forms.MessageBox.Show("非文本文件无法开启。");
                    }
                    else
                        tePos.Text = "";
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("档案不存在，请确认后再选取。");
                }
            }
        }

        /// <summary>
        /// doubleclick清除文字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEditor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var te = (sender as Controls.TextBox);
            if (te != null)
            {
                te.Text = "";
            }
        }

        #region 外部呼叫
        /// <summary>
        /// 設定預設值
        /// </summary>
        public void SetDefaultValue()
        {
            /*CheckBoxList*/
            cblData1.Clear();
            cblData2.Clear();
            cblData3.Clear();

            cblTenThousands.Clear();
            cblThousands.Clear();
            cblHundreds.Clear();
            cblTens.Clear();
            cblUnits.Clear();

            cblSumLast.Clear();
            cblCross.Clear();
            cblComm.Clear();

            cblRatio.Clear();
            cblRatio2.Clear();
            cblRatio3.Clear();
            //cblAC.Clear();

            //膽組
            cblNumber1.Clear();
            cblNumber1_2.Clear();
            cblNumber2.Clear();
            cblNumber2_2.Clear();
            cblNumber3.Clear();
            cblNumber3_2.Clear();
            cblNumber4.Clear();
            cblNumber4_2.Clear();
            cblNumber5.Clear();
            cblNumber5_2.Clear();
            cblNumber6.Clear();
            cblNumber6_2.Clear();
            btnCountRepeat.IsChecked = false;
            Hashtable ht = new Hashtable();
            BaseHelper.GetChildren(dpAll, ht);
            foreach (var b in ht.Values)
            {
                if (b is Controls.Button)
                {
                    Controls.Button bt = b as Controls.Button;
                    bt.Background = System.Windows.Media.Brushes.Gainsboro;
                }
            }

            /*TextBox*/
            teSum.Text = "";
            tePos.Text = "";
        }

        /// <summary>
        /// 選取全部選項
        /// </summary>
        public void SelectAll()
        {
            /*CheckBoxList*/
            cblData1.SelectedAll();
            cblData2.SelectedAll();
            cblData3.SelectedAll();

            cblTenThousands.SelectedAll();
            cblThousands.SelectedAll();
            cblHundreds.SelectedAll();
            cblTens.SelectedAll();
            cblUnits.SelectedAll();

            cblSumLast.SelectedAll();
            cblCross.SelectedAll();
            cblComm.SelectedAll();

            cblRatio.SelectedAll();
            cblRatio2.SelectedAll();
            cblRatio3.SelectedAll();
        }

        /// <summary>
        /// 過濾前檢核
        /// </summary>
        /// <returns></returns>
        public bool BeforeCheck()
        {
            if ((cblNumber1.SelectedValue.IndexOf('1') > -1 && cblNumber1_2.SelectedValue.IndexOf('1') == -1) ||
                (cblNumber2.SelectedValue.IndexOf('1') > -1 && cblNumber2_2.SelectedValue.IndexOf('1') == -1) ||
                (cblNumber3.SelectedValue.IndexOf('1') > -1 && cblNumber3_2.SelectedValue.IndexOf('1') == -1) ||
                (cblNumber4.SelectedValue.IndexOf('1') > -1 && cblNumber4_2.SelectedValue.IndexOf('1') == -1) ||
                (cblNumber5.SelectedValue.IndexOf('1') > -1 && cblNumber5_2.SelectedValue.IndexOf('1') == -1) ||
                (cblNumber6.SelectedValue.IndexOf('1') > -1 && cblNumber6_2.SelectedValue.IndexOf('1') == -1))
            {
                System.Windows.MessageBox.Show("已设定胆组但未设定出胆个数。");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            //殺直選
            tmp = Calculation.AssignNumber(tmp, tePos.Text, false);

            //定位殺
            tmp = Calculation.PosNumber(tmp, cblTenThousands, "0", false);
            tmp = Calculation.PosNumber(tmp, cblThousands, "1", false);
            tmp = Calculation.PosNumber(tmp, cblHundreds, "2", false);
            tmp = Calculation.PosNumber(tmp, cblTens, "3", false);
            tmp = Calculation.PosNumber(tmp, cblUnits, "4", false);

            //殺和尾
            tmp = Calculation.SumLastNumber(tmp, cblSumLast, false);

            //殺和值
            tmp = Calculation.SumNumber(tmp, teSum.Text, false);

            //殺跨度
            tmp = Calculation.CrossNumber(tmp, cblCross, false);

            //殺通碼
            tmp = Calculation.ExistsNumber(tmp, cblComm, 1, false, false);

            //殺大小
            tmp = Calculation.CheckValueNumber(tmp, cblData1, 1, false);

            //殺奇偶
            tmp = Calculation.OddEvenNumber(tmp, cblData2, false);

            //殺质合
            tmp = Calculation.PrimeNumber(tmp, cblData3, false);

            //膽碼
            if (cblNumber1.SelectedValue.IndexOf('1') > -1)
                tmp = Calculation.ExistsNumber2(tmp, cblNumber1, cblNumber1_2, null, (bool)btnCountRepeat.IsChecked);
            if (cblNumber2.SelectedValue.IndexOf('1') > -1)
                tmp = Calculation.ExistsNumber2(tmp, cblNumber2, cblNumber2_2, null, (bool)btnCountRepeat.IsChecked);
            if (cblNumber3.SelectedValue.IndexOf('1') > -1)
                tmp = Calculation.ExistsNumber2(tmp, cblNumber3, cblNumber3_2, null, (bool)btnCountRepeat.IsChecked);
            if (cblNumber4.SelectedValue.IndexOf('1') > -1)
                tmp = Calculation.ExistsNumber2(tmp, cblNumber4, cblNumber4_2, null, (bool)btnCountRepeat.IsChecked);
            if (cblNumber5.SelectedValue.IndexOf('1') > -1)
                tmp = Calculation.ExistsNumber2(tmp, cblNumber5, cblNumber5_2, null, (bool)btnCountRepeat.IsChecked);
            if (cblNumber6.SelectedValue.IndexOf('1') > -1)
                tmp = Calculation.ExistsNumber2(tmp, cblNumber6, cblNumber6_2, null, (bool)btnCountRepeat.IsChecked);

            //比例大小比
            tmp = Calculation.BigSmallRatio(tmp, cblRatio.SelectedValue);

            //比例奇偶比
            tmp = Calculation.OddEvenRatio(tmp, cblRatio2.SelectedValue);

            //比例質合比
            tmp = Calculation.PrimeRatio(tmp, cblRatio3.SelectedValue);
            return tmp;
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controls.Button btn = sender as Controls.Button;
            if (btn != null)
            {
                if (btn.Tag != null)
                {
                    int index = 0;
                    char[] tmp;
                    Hashtable ht = new Hashtable();
                    CheckBoxList cbl = null;
                    switch ((string)btn.Tag)
                    {
                        case "Type1":
                            cbl = cblNumber1;
                            break;
                        case "Type2":
                            cbl = cblNumber2;
                            break;
                        case "Type3":
                            cbl = cblNumber3;
                            break;
                        case "Type4":
                            cbl = cblNumber4;
                            break;
                        case "Type5":
                            cbl = cblNumber5;
                            break;
                        case "Type6":
                            cbl = cblNumber6;
                            break;
                        case "Unit1":
                            cbl = cblNumber1_2;
                            break;
                        case "Unit2":
                            cbl = cblNumber2_2;
                            break;
                        case "Unit3":
                            cbl = cblNumber3_2;
                            break;
                        case "Unit4":
                            cbl = cblNumber4_2;
                            break;
                        case "Unit5":
                            cbl = cblNumber5_2;
                            break;
                        case "Unit6":
                            cbl = cblNumber6_2;
                            break;
                        case "Clear1":
                            cblNumber1.Clear();
                            cblNumber1_2.Clear();
                            BaseHelper.GetChildren(dpType1, ht);
                            break;
                        case "Clear2":
                            cblNumber2.Clear();
                            cblNumber2_2.Clear();
                            BaseHelper.GetChildren(dpType2, ht);
                            break;
                        case "Clear3":
                            cblNumber3.Clear();
                            cblNumber3_2.Clear();
                            BaseHelper.GetChildren(dpType3, ht);
                            break;
                        case "Clear4":
                            cblNumber4.Clear();
                            cblNumber4_2.Clear();
                            BaseHelper.GetChildren(dpType4, ht);
                            break;
                        case "Clear5":
                            cblNumber5.Clear();
                            cblNumber5_2.Clear();
                            BaseHelper.GetChildren(dpType5, ht);
                            break;
                        case "Clear6":
                            cblNumber6.Clear();
                            cblNumber6_2.Clear();
                            BaseHelper.GetChildren(dpType6, ht);
                            break;
                        case "Select1":
                            cblNumber1.SelectedAll();
                            cblNumber1_2.SelectedValue = cblNumber1_2.SelectedValue[0] + "11111";
                            BaseHelper.GetChildren(dpType1, ht);
                            break;
                        case "Select2":
                            cblNumber2.SelectedAll();
                            cblNumber2_2.SelectedValue = cblNumber2_2.SelectedValue[0] + "11111";
                            BaseHelper.GetChildren(dpType2, ht);
                            break;
                        case "Select3":
                            cblNumber3.SelectedAll();
                            cblNumber3_2.SelectedValue = cblNumber3_2.SelectedValue[0] + "11111";
                            BaseHelper.GetChildren(dpType3, ht);
                            break;
                        case "Select4":
                            cblNumber4.SelectedAll();
                            cblNumber4_2.SelectedValue = cblNumber4_2.SelectedValue[0] + "11111";
                            BaseHelper.GetChildren(dpType4, ht);
                            break;
                        case "Select5":
                            cblNumber5.SelectedAll();
                            cblNumber5_2.SelectedValue = cblNumber5_2.SelectedValue[0] + "11111";
                            BaseHelper.GetChildren(dpType5, ht);
                            break;
                        case "Select6":
                            cblNumber6.SelectedAll();
                            cblNumber6_2.SelectedValue = cblNumber6_2.SelectedValue[0] + "11111";
                            BaseHelper.GetChildren(dpType6, ht);
                            break;
                        case "Remark":
                            Forms.MessageBox.Show("可以选择多个胆组。");
                            break;
                    }

                    if (cbl != null)
                    {
                        int.TryParse(btn.Content.ToString(), out index);
                        tmp = cbl.SelectedValue.ToArray();
                        tmp[index] = (tmp[index] == '1' ? '0' : '1');
                        if (tmp[index] == '1')
                            btn.Background = System.Windows.Media.Brushes.LawnGreen;
                        else
                            btn.Background = System.Windows.Media.Brushes.Gainsboro;
                        cbl.SelectedValue = string.Join("", tmp);
                    }

                    foreach (var b in ht.Values)
                    {
                        if (b is Controls.Button)
                        {
                            Controls.Button bt = b as Controls.Button;
                            if (((string)bt.Tag).Contains("Select") || ((string)bt.Tag).Contains("Clear"))
                                continue;

                            if (((string)btn.Tag).Contains("Select"))
                            {
                                if (bt.Content.ToString() == "0" && ((string)bt.Tag).Contains("Unit"))
                                    continue;
                                bt.Background = System.Windows.Media.Brushes.LawnGreen;
                            }
                            else
                                bt.Background = System.Windows.Media.Brushes.Gainsboro;
                        }
                    }
                }
            }
        }
    }
}
