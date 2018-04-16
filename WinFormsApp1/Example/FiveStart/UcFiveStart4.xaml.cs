using System;
using System.Linq;
using System.Windows;
using Control = System.Windows.Controls;
using System.IO;
using WpfAppTest.AP;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using System.Drawing;
using Forms = System.Windows.Forms;

namespace WpfAppTest
{
    /// <summary>
    /// UcFiveStart1.xaml 的互動邏輯
    /// </summary>
    public partial class UcFiveStart4 :Control.UserControl
    {
        public UcFiveStart4()
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
                IsFirstTime = false;
            }
        }

        /// <summary>
        /// 按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (sender as Control.Button);
            if (btn != null)
            {
                if (btn.Name.Contains("btnImport"))
                {
                    /*開啟檔案並讀取*/
                    Control.TextBox te = (btn.Name == "btnImportA" ? teCompareA : teCompareB);
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
                                    te.Text = File.ReadAllText(openFileDialog.FileName);
                                else
                                    System.Windows.Forms.MessageBox.Show("非文字檔無法開啟。");
                            }
                            else
                                te.Text = "";
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("檔案不存在，請確認後再選取。");
                        }
                    }
                }
                else if (btn.Name.Contains("btnClear"))
                {
                    /*清空*/
                    Control.TextBox te = (btn.Name == "btnClearA" ? teCompareA : teCompareB);
                    te.Text = "";
                }
                else if (btn.Name == "btnCopy")
                {
                    /*複製功能*/
                    if (!string.IsNullOrEmpty(teResult.Text))
                    {
                        System.Windows.Forms.Clipboard.SetText(teResult.Text);
                        System.Windows.Forms.MessageBox.Show("號碼複製成功。");
                    }
                }
                else if (btn.Name == "btnIntersection" || btn.Name == "btnUnion" ||
                         btn.Name == "btnExcludeB" || btn.Name == "btnExcludeA")
                {
                    string[] arrayA = new string[] { };
                    string[] arrayB = new string[] { };
                    string[] arrayC = new string[] { };
                    string[] empty = new string[1] { "" };
                    arrayA = teCompareA.Text.Split(' ').ToArray();
                    arrayB = teCompareB.Text.Split(' ').ToArray();

                    if (btn.Name == "btnIntersection")
                    {
                        /*AB交集*/
                        arrayC = arrayA.Intersect(arrayB).Except(empty).ToArray();
                    }
                    else if (btn.Name == "btnUnion")
                    {
                        /*AB並集*/
                        arrayC = arrayA.Union(arrayB).Except(empty).ToArray();
                    }
                    else if (btn.Name == "btnExcludeB")
                    {
                        /*A排除B*/
                        arrayC = arrayA.Except(arrayB).ToArray();
                    }
                    else if (btn.Name == "btnExcludeA")
                    {
                        /*B排除A*/
                        arrayC = arrayB.Except(arrayA).ToArray();
                    }
                    teResult.Text = string.Join(" ", arrayC);
                    teUnit.Text = arrayC.Count().ToString();
                }
            }
        }

        int unit = 0;
        /// <summary>
        /// 數字遮罩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teUnit_TextChanged(object sender, Control.TextChangedEventArgs e)
        {
            if (!IsFirstTime)
            {
                if (teUnit.Text == "" || teUnit.Text == null)
                    teUnit.Text = "0";
                else
                {
                    int i = 0;
                    if (!int.TryParse(teUnit.Text, out i))
                        teUnit.Text = unit.ToString();
                    else
                    {
                        teUnit.Text = i.ToString();
                        unit = i;
                    }
                }
            }
        }
    }
}
