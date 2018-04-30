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
using Form = System.Windows.Forms;
using WpfAppTest.Base;

namespace WpfAppTest
{
    /// <summary>
    /// UcFiveStart1.xaml 的互動邏輯
    /// </summary>
    public partial class UcFiveStart5 : Control.UserControl
    {
        public UcFiveStart5()
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
                GetTextBox();
                Initial();
                IsFirstTime = false;
            }
        }

        /// <summary>
        /// 給TextBlock名稱
        /// </summary>
        void Initial()
        {
            List<BaseOptions> list = DB.FiveStartTab();
            foreach (var tmp in result.Values)
            {
                if (tmp is Control.TextBlock)
                {
                    var tb = (tmp as Control.TextBlock);
                    var item = list.Where(x => (x.ID - 1).ToString() == tb.Name.Replace("tbType", "")).FirstOrDefault();
                    if (item != null)
                        tb.Text = item.Name;
                }
            }
        }

        /// <summary>
        /// 物件存放的雜湊函數
        /// </summary>
        Hashtable result = new Hashtable();

        /// <summary>
        /// 取得物件
        /// </summary>
        void GetTextBox()
        {
            Base.BaseHelper.GetChildren(gdContainer, result);
        }

        /// <summary>
        /// TextBox是否可啟用
        /// </summary>
        /// <param name="i"></param>
        public void TextBoxEnable(int i)
        {
            string name = "teType" + i.ToString();
            foreach (var tmp in result.Values)
            {
                if (tmp is Control.TextBox)
                {
                    var tb = (tmp as Control.TextBox);
                    tb.IsEnabled = (tb.Name == name);
                    if (tb.IsEnabled)
                    {
                        tb.BorderBrush = System.Windows.Media.Brushes.Red;
                        tb.BorderThickness = new Thickness(5);
                    }
                    else
                    {
                        tb.BorderBrush = System.Windows.Media.Brushes.Black;
                        tb.BorderThickness = new Thickness(1);
                    }
                }
            }
        }

        /// <summary>
        /// doubleclick清除文字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Type_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var te = (sender as Control.TextBox);
            if (te != null)
            {
                te.Text = "";
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (sender as Control.Button);
            if (btn != null)
            {
                if (btn.Name == "btnImport")
                {
                    Form.OpenFileDialog openFileDialog = new Form.OpenFileDialog();

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
                                    teNumber.Text = File.ReadAllText(openFileDialog.FileName);
                                else
                                    System.Windows.Forms.MessageBox.Show("非文字檔無法開啟。");
                            }
                            else
                                teNumber.Text = "";
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("檔案不存在，請確認後再選取。");
                        }
                    }
                }
                else if (btn.Name == "btnClear")
                {
                    teNumber.Text = "";
                }
            }
        }

        #region 外部呼叫
        /// <summary>
        /// 設定預設值
        /// </summary>
        public void SetDefaultValue()
        {
            /*TextBox*/
            teNumber.Text = "";
        }

        /// <summary>
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            //大底號碼
            var n = teNumber.Text.Split(' ').Except(new string[1] { "" });
            if (n.Count() > 0)
                tmp = tmp.Where(x => n.Contains(x.Code)).ToList();
            return tmp;
        }
        #endregion
    }
}
