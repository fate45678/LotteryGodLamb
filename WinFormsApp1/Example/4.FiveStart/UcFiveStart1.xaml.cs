using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Controls = System.Windows.Controls;
using Forms = System.Windows.Forms;
using System.IO;
using WpfAppTest.AP;
using WpfAppTest.Base;

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
            cblData1.ItemsSource = DB.ZeroOneCombination(5, '大', '小').OrderByDescending(x => x.Code).ToList();
            cblData2.ItemsSource = DB.ZeroOneCombination(5, '奇', '偶').OrderByDescending(x => x.Code).ToList();
            cblData3.ItemsSource = DB.ZeroOneCombination(5, '质', '合').OrderByDescending(x => x.Code).ToList();

            var Data = DB.CreateContinueNumber().OrderBy(x => x.ID).ToList();
            cblTenThousands.ItemsSource = Data;
            cblThousands.ItemsSource = Data;
            cblHundreds.ItemsSource = Data;
            cblTens.ItemsSource = Data;
            cblUnits.ItemsSource = Data;

            cblSumLast.ItemsSource = Data;
            cblCross.ItemsSource = Data;
            cblComm.ItemsSource = Data;

            var Ratio = DB.Ratio(5);
            cblRatio.ItemsSource = Ratio;
            cblRatio2.ItemsSource = Ratio;
            cblRatio3.ItemsSource = Ratio;
            cblAC.ItemsSource = DB.CreateOption(1, 9);

            /*預設值*/
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
                            System.Windows.Forms.MessageBox.Show("非文字檔無法開啟。");
                    }
                    else
                        tePos.Text = "";
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("檔案不存在，請確認後再選取。");
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

            teSum.Text = "";
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
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            ////殺直選
            //tmp = Calculation.PosNumber(tmp, tePos1.Text, 1, false);

            //定位殺
            tmp = Calculation.PosNumber(tmp, cblTenThousands, "4", false);
            tmp = Calculation.PosNumber(tmp, cblThousands, "3", false);
            tmp = Calculation.PosNumber(tmp, cblHundreds, "2", false);
            tmp = Calculation.PosNumber(tmp, cblTens, "1", false);
            tmp = Calculation.PosNumber(tmp, cblUnits, "0", false);

            //殺和尾
            tmp = Calculation.SumLastNumber(tmp, cblData1, false);

            //殺和值
            tmp = Calculation.SumNumber(tmp, teSum.Text, false);

            //殺跨度
            tmp = Calculation.CrossNumber(tmp, cblCross, false);

            //殺大小
            tmp = Calculation.CheckValueNumber(tmp, cblData1, 1, false);

            //殺奇偶
            tmp = Calculation.OddEvenNumber(tmp, cblData2, false);

            //殺质合
            tmp = Calculation.PrimeNumber(tmp, cblData3, false);

            
            return tmp;
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void te_TextChanged(object sender, Controls.TextChangedEventArgs e)
        {

        }
    }
}
