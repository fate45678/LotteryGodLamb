using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using WpfAppTest.AP;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using System.Drawing;

namespace WpfAppTest
{
    /// <summary>
    /// UcFiveStart1.xaml 的互動邏輯
    /// </summary>
    public partial class UcFiveStart2 : UserControl
    {
        public UcFiveStart2()
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
                if (tmp is TextBlock)
                {
                    var tb = (tmp as TextBlock);
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
                if (tmp is TextBox)
                {
                    var tb = (tmp as TextBox);
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

        private void Type_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var te = (sender as TextBox);
            if (te != null)
            {
                te.Text = "";
            }
        }
    }
}
