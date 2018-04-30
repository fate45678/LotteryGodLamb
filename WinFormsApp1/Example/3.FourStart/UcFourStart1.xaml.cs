using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Controls = System.Windows.Controls;
using System.Windows.Input;
using Forms = System.Windows.Forms;
using System.IO;
using WpfAppTest.AP;
using WpfAppTest.Base;
using System.Collections;

namespace WpfAppTest
{
    /// <summary>
    /// UcFourStart1.xaml 的互動邏輯
    /// </summary>
    public partial class UcFourStart1 : System.Windows.Controls.UserControl
    {
        public UcFourStart1()
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
            OldText = new int[2] { 0, 0 };

            var data = DB.CreateContinueNumber().OrderBy(x => x.ID).ToList();

            /*CheckBoxList*/
            cblData1.ItemsSource = DB.ZeroOneCombination(4, '大', '小').OrderByDescending(x => x.Code).ToList();
            cblData2.ItemsSource = DB.ZeroOneCombination(4, '奇', '偶').OrderByDescending(x => x.Code).ToList();
            cblData3.ItemsSource = DB.ZeroOneCombination(4, '质', '合').OrderByDescending(x => x.Code).ToList();

            cblThousands.ItemsSource = data;
            cblHundreds.ItemsSource = data;
            cblTens.ItemsSource = data;
            cblUnits.ItemsSource = data;

            cblType1.ItemsSource = data;
            cblType2.ItemsSource = data;
            cblType3.ItemsSource = data;
            cblType4.ItemsSource = data;

            //012路
            cbl012.ItemsSource = DB.CombinationNumber(4, 0, 2).OrderBy(x => x.Code).ToList();

            //膽碼
            cblNumber1.ItemsSource = data;
            cblNumber2.ItemsSource = data;
            cblNumber3.ItemsSource = data;
            cblNumber4.ItemsSource = data;

            data = DB.CreateOption(0, 4);
            cblNumber1_2.ItemsSource = data;
            cblNumber2_2.ItemsSource = data;
            cblNumber3_2.ItemsSource = data;
            cblNumber4_2.ItemsSource = data;

            cblSpecial.ItemsSource = DB.CreateOption(1, 6, new string[6] { "上山", "下山", "凸型", "凹型", "N型", "反N型" });
            cblSpecialExcept.ItemsSource = DB.CreateOption(1, 9, new string[9] { "豹子", "不连", "2连", "3连", "4连", "散号", "对子号", "三同号", "两个对子" });

            /*設定預設值*/
            SetDefaultValue();
        }

        /// <summary>
        /// 是否由程式設定值
        /// </summary>
        bool IsSetting = false;

        #region 外部呼叫
        /// <summary>
        /// 設定預設值
        /// </summary>
        public void SetDefaultValue()
        {
            IsSetting = true;

            /*CheckBoxList*/
            cblData1.Clear();
            cblData2.Clear();
            cblData3.Clear();
            cbl012.Clear();

            cblThousands.Clear();
            cblHundreds.Clear();
            cblTens.Clear();
            cblUnits.Clear();

            cblType1.Clear();
            cblType2.Clear();
            cblType3.Clear();
            cblType4.Clear();

            cblSpecial.Clear();
            cblSpecialExcept.Clear();

            //膽組
            cblNumber1.Clear();
            cblNumber1_2.Clear();
            cblNumber2.Clear();
            cblNumber2_2.Clear();
            cblNumber3.Clear();
            cblNumber3_2.Clear();
            cblNumber4.Clear();
            cblNumber4_2.Clear();
            btnCountRepeat.IsChecked = false;
            Hashtable ht = new Hashtable();
            Base.BaseHelper.GetChildren(dpAll, ht);
            foreach (var b in ht.Values)
            {
                if (b is Controls.Button)
                {
                    Controls.Button bt = b as Controls.Button;
                    bt.Background = System.Windows.Media.Brushes.LightGray;
                }
            }

            /*TextBox*/
            teEditor1.Text = "";
            teEditor2.Text = "";
            teEditor3_1.Text = "";
            teEditor3_2.Text = "";
            teEditor4_1.Text = "";
            teEditor4_2.Text = "";
            teEditor5.Text = "";
            teNumber.Text = "";
            teSum.Text = "";
            teStart.Text = "0";
            teEnd.Text = "0";
            IsSetting = false;
        }

        ///// <summary>
        ///// 選取全部選項
        ///// </summary>
        //public void SelectAll()
        //{
        //    IsSetting = true;

        //    IsSetting = false;
        //}

        /// <summary>
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            //大底先篩再過濾
            var n = teNumber.Text.Split(' ').Except(new string[1] { "" });
            if (n.Count() > 0)
                tmp = tmp.Where(x => n.Contains(x.Code)).ToList();

            #region group1-殺直選.殺垃圾.殺兩碼.必出兩碼.殺三碼.必出三碼.定位杀号
            //殺直選
            tmp = Calculation.AssignNumber(tmp, teEditor1.Text, false);

            //殺垃圾

            //殺兩碼
            tmp = Calculation.ExistsNumber(tmp, teEditor3_1.Text, 2, false);

            //必出兩碼
            tmp = Calculation.ExistsNumber(tmp, teEditor3_2.Text, 2, true);

            //殺三碼
            tmp = Calculation.ExistsNumber(tmp, teEditor4_1.Text, 3, false);

            //必出三碼
            tmp = Calculation.ExistsNumber(tmp, teEditor4_2.Text, 3, true);

            //定位杀号
            #endregion

            #region group2-定位殺
            tmp = Calculation.PosNumber(tmp, cblThousands, "3", false);
            tmp = Calculation.PosNumber(tmp, cblHundreds, "2", false);
            tmp = Calculation.PosNumber(tmp, cblTens, "1", false);
            tmp = Calculation.PosNumber(tmp, cblUnits, "0", false);
            #endregion

            #region group3-殺和尾.殺和值.殺跨度.殺通碼.殺AC值
            //殺和尾
            tmp = Calculation.SumLastNumber(tmp, cblType1, false);

            //殺和值
            tmp = Calculation.SumNumber(tmp, teSum.Text, false);

            //殺跨度
            tmp = Calculation.CrossNumber(tmp, cblType2, false);

            //殺通碼

            //殺AC值

            #endregion

            #region group4-殺特殊型態
            tmp = Calculation.FourSpecialData(tmp, cblSpecial.SelectedValue);
            #endregion

            #region group5-特别排除
            tmp = Calculation.FourSpecial2Data(tmp, cblSpecialExcept.SelectedValue);
            #endregion

            #region group6-殺大小
            tmp = Calculation.CheckValueNumber(tmp, cblData1, 1, false);
            #endregion

            #region group7-殺奇偶
            tmp = Calculation.OddEvenNumber(tmp, cblData2, false);
            #endregion

            #region group8-殺質合
            tmp = Calculation.PrimeNumber(tmp, cblData3, false);
            #endregion

            #region group9-殺012路
            tmp = Calculation.DivThreeRemainder(tmp, cbl012, false);
            #endregion

            return tmp;
        }
        #endregion

        /// <summary>
        /// doubleclick清除文字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEditor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Controls.TextBox te = sender as Controls.TextBox;
            if (te != null)
                te.Text = "";
        }

        /// <summary>
        /// button事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        case "Clear1":
                            cblNumber1.Clear();
                            cblNumber1_2.Clear();
                            Base.BaseHelper.GetChildren(dpType1, ht);
                            break;
                        case "Clear2":
                            cblNumber2.Clear();
                            cblNumber2_2.Clear();
                            Base.BaseHelper.GetChildren(dpType2, ht);
                            break;
                        case "Clear3":
                            cblNumber3.Clear();
                            cblNumber3_2.Clear();
                            Base.BaseHelper.GetChildren(dpType3, ht);
                            break;
                        case "Clear4":
                            cblNumber4.Clear();
                            cblNumber4_2.Clear();
                            Base.BaseHelper.GetChildren(dpType4, ht);
                            break;
                        case "Select1":
                            cblNumber1.SelectedAll();
                            cblNumber1_2.SelectedAll();
                            Base.BaseHelper.GetChildren(dpType1, ht);
                            break;
                        case "Select2":
                            cblNumber2.SelectedAll();
                            cblNumber2_2.SelectedAll();
                            Base.BaseHelper.GetChildren(dpType2, ht);
                            break;
                        case "Select3":
                            cblNumber3.SelectedAll();
                            cblNumber3_2.SelectedAll();
                            Base.BaseHelper.GetChildren(dpType3, ht);
                            break;
                        case "Select4":
                            cblNumber4.SelectedAll();
                            cblNumber4_2.SelectedAll();
                            Base.BaseHelper.GetChildren(dpType4, ht);
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
                            btn.Background = System.Windows.Media.Brushes.LightGray;
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
                                bt.Background = System.Windows.Media.Brushes.LawnGreen;
                            else
                                bt.Background = System.Windows.Media.Brushes.LightGray;
                        }
                    }
                }
            }
        }

        int[] OldText;
        /// <summary>
        /// 數字遮罩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void te_TextChanged(object sender, Controls.TextChangedEventArgs e)
        {
            if (!IsFirstTime)
            {
                var te = (sender as Controls.TextBox);
                int index = (te.Name == "teStart" ? 0 : 1);
                if (te.Text == "" || te.Text == null)
                    te.Text = "0";
                else
                {
                    int i = 0;
                    if (!int.TryParse(te.Text, out i))
                        te.Text = OldText[index].ToString();
                    else
                    {
                        te.Text = i.ToString();
                        OldText[index] = i;
                    }
                }
            }
        }
    }
}
