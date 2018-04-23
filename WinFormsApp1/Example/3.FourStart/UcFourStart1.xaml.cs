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
            cblData3.ItemsSource = DB.ZeroOneCombination(4, '質', '合').OrderByDescending(x => x.Code).ToList();

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
            cblSpecialExcept.ItemsSource = DB.CreateOption(1, 9, new string[9] { "豹子", "不連", "2連", "3連", "4連", "散號", "對子號", "三同號", "兩個對子" });

            /*設定預設值*/
            SetDefaultValue();
        }

        /// <summary>
        /// 是否由程式設定值
        /// </summary>
        bool IsSetting = false;

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

            /*TextBox*/
            teEditor1.Text = "";
            teEditor2.Text = "";
            teEditor3_1.Text = "";
            teEditor3_2.Text = "";
            teEditor4_1.Text = "";
            teEditor4_2.Text = "";
            teEditor5.Text = "";
            teNumber.Text = "";

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
            tmp = Calculation.CheckValueNumber(tmp, cblData1, 1, false);
            tmp = Calculation.OddEvenNumber(tmp, cblData2, false);
            tmp = Calculation.PrimeNumber(tmp, cblData2, false);
            tmp = Calculation.DivThreeRemainder(tmp, cblData2, false);
            return tmp;
        }

        /// <summary>
        /// doubleclick清除文字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEditor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Controls.TextBox te = sender as Controls.TextBox;
            if (te != null)
            {
                if (te.Name == "teEditor1")
                    teEditor1.Text = "";
                else if (te.Name == "teEditor2")
                    teEditor2.Text = "";
                else if (te.Name == "teEditor3_1")
                    teEditor3_1.Text = "";
                else if (te.Name == "teEditor3_2")
                    teEditor3_2.Text = "";
                else if (te.Name == "teEditor4_1")
                    teEditor4_1.Text = "";
                else if (te.Name == "teEditor4_2")
                    teEditor4_2.Text = "";
                else if (te.Name == "teEditor5")
                    teEditor5.Text = "";
                else if (te.Name == "teNumber")
                    teNumber.Text = "";
            }
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
                    switch ((string)btn.Tag)
                    {
                        case "Type1":
                            int.TryParse(btn.Content.ToString(), out index);
                            tmp = cblNumber1.SelectedValue.ToArray();
                            tmp[index] = (tmp[index] == '1' ? '0' : '1');
                            if (tmp[index] == '1')
                                btn.Background = System.Windows.Media.Brushes.LawnGreen;
                            else
                                btn.Background = System.Windows.Media.Brushes.LightGray;
                            cblNumber1.SelectedValue = string.Join("", tmp);
                            break;
                        case "Type2":
                            int.TryParse(btn.Content.ToString(), out index);
                            tmp = cblNumber2.SelectedValue.ToArray();
                            tmp[index] = (tmp[index] == '1' ? '0' : '1');
                            if (tmp[index] == '1')
                                btn.Background = System.Windows.Media.Brushes.LawnGreen;
                            else
                                btn.Background = System.Windows.Media.Brushes.LightGray;
                            cblNumber2.SelectedValue = string.Join("", tmp);
                            break;
                        case "Type3":
                            int.TryParse(btn.Content.ToString(), out index);
                            tmp = cblNumber3.SelectedValue.ToArray();
                            tmp[index] = (tmp[index] == '1' ? '0' : '1');
                            if (tmp[index] == '1')
                                btn.Background = System.Windows.Media.Brushes.LawnGreen;
                            else
                                btn.Background = System.Windows.Media.Brushes.LightGray;
                            cblNumber3.SelectedValue = string.Join("", tmp);
                            break;
                        case "Type4":
                            int.TryParse(btn.Content.ToString(), out index);
                            tmp = cblNumber4.SelectedValue.ToArray();
                            tmp[index] = (tmp[index] == '1' ? '0' : '1');
                            if (tmp[index] == '1')
                                btn.Background = System.Windows.Media.Brushes.LawnGreen;
                            else
                                btn.Background = System.Windows.Media.Brushes.LightGray;
                            cblNumber4.SelectedValue = string.Join("", tmp);
                            break;
                        case "Unit1":
                            int.TryParse(btn.Content.ToString(), out index);
                            tmp = cblNumber1_2.SelectedValue.ToArray();
                            tmp[index] = (tmp[index] == '1' ? '0' : '1');
                            if (tmp[index] == '1')
                                btn.Background = System.Windows.Media.Brushes.LawnGreen;//LawnGreen;
                            else
                                btn.Background = System.Windows.Media.Brushes.LightGray;
                            cblNumber1_2.SelectedValue = string.Join("", tmp);
                            break;
                        case "Unit2":
                            int.TryParse(btn.Content.ToString(), out index);
                            tmp = cblNumber2_2.SelectedValue.ToArray();
                            tmp[index] = (tmp[index] == '1' ? '0' : '1');
                            if (tmp[index] == '1')
                                btn.Background = System.Windows.Media.Brushes.LawnGreen;
                            else
                                btn.Background = System.Windows.Media.Brushes.LightGray;
                            cblNumber2_2.SelectedValue = string.Join("", tmp);
                            break;
                        case "Unit3":
                            int.TryParse(btn.Content.ToString(), out index);
                            tmp = cblNumber3_2.SelectedValue.ToArray();
                            tmp[index] = (tmp[index] == '1' ? '0' : '1');
                            if (tmp[index] == '1')
                                btn.Background = System.Windows.Media.Brushes.LawnGreen;
                            else
                                btn.Background = System.Windows.Media.Brushes.LightGray;
                            cblNumber3_2.SelectedValue = string.Join("", tmp);
                            break;
                        case "Unit4":
                            int.TryParse(btn.Content.ToString(), out index);
                            tmp = cblNumber4_2.SelectedValue.ToArray();
                            tmp[index] = (tmp[index] == '1' ? '0' : '1');
                            if (tmp[index] == '1')
                                btn.Background = System.Windows.Media.Brushes.LawnGreen;
                            else
                                btn.Background = System.Windows.Media.Brushes.LightGray;
                            cblNumber4_2.SelectedValue = string.Join("", tmp);
                            break;
                        case "Clear1":
                            cblNumber1.Clear();
                            cblNumber1_2.Clear();
                            break;
                        case "Clear2":
                            cblNumber2.Clear();
                            cblNumber2_2.Clear();
                            break;
                        case "Clear3":
                            cblNumber3.Clear();
                            cblNumber3_2.Clear();
                            break;
                        case "Clear4":
                            cblNumber4.Clear();
                            cblNumber4_2.Clear();
                            break;
                        case "Select1":
                            cblNumber1.SelectedAll();
                            cblNumber1_2.SelectedAll();
                            break;
                        case "Select2":
                            cblNumber2.SelectedAll();
                            cblNumber2_2.SelectedAll();
                            break;
                        case "Select3":
                            cblNumber3.SelectedAll();
                            cblNumber3_2.SelectedAll();
                            break;
                        case "Select4":
                            cblNumber4.SelectedAll();
                            cblNumber4_2.SelectedAll();
                            break;
                        case "Remark":
                            Forms.MessageBox.Show("可以選擇多個膽組。");
                            break;
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
