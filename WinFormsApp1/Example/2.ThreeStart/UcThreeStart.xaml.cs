using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Controls = System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Forms = System.Windows.Forms;
using System.IO;
using WpfAppTest.AP;

namespace WpfAppTest
{
    /// <summary>
    /// UcFourStart.xaml 的互動邏輯
    /// </summary>
    public partial class UcThreeStart : System.Windows.Controls.UserControl
    {
        public UcThreeStart()
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
        /// 四星所有組合
        /// </summary>
        List<BaseOptions> AllConbination;

        /// <summary>
        /// 設定資料
        /// </summary>
        void SetData()
        {
            AllConbination = DB.CombinationNumber(3, 0, 9);
            btnTransfer.IsEnabled = false;
            OldText = new int[2] { 0, 0 };
        }

        void SetDefaultValue()
        {
            teResult.Text = "";
            tbCount.Text = "0";
        }

        /// <summary>
        /// 縮水後結果
        /// </summary>
        List<BaseOptions> FilterData;

        /// <summary>
        /// 右半-結果區的按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (sender as System.Windows.Controls.Button);
            if (btn != null)
            {
                if (btn.Name == "btnFilter")
                {
                    /*開始縮水*/
                    string btncontent = (string)btn.Content;
                    btn.IsEnabled = false;
                    btn.Content = "载入中...";

                    //縮水
                    var tmp = ucThree1.Filter(AllConbination);
                    teResult.Text = string.Join(" ", tmp.Select(x => x.Code));
                    tbCount.Text = tmp.Count.ToString();
                    ucThree1.RefreshBottom(string.Join("    ", tmp.Select(x => x.Code)));

                    //組選
                    FilterData = tmp;
                    btnTransfer.IsEnabled = true;

                    //btn啟用
                    btn.Content = btncontent;
                    btn.IsEnabled = true;
                }
                else if (btn.Name == "btnTransfer")
                {
                    /*轉為組選*/
                    if (FilterData != null)
                    {
                        var tmp = Base.Calculation.TransNumber(FilterData);
                        teResult.Text = string.Join(" ", tmp.Select(x => x.Code));
                        tbCount.Text = tmp.Count.ToString();
                        ucThree1.RefreshBottom(string.Join("    ", tmp.Select(x => x.Code)));
                    }
                }
                else if (btn.Name == "btnCopy")
                {
                    /*複製號碼*/
                    if (!string.IsNullOrEmpty(teResult.Text))
                    {
                        System.Windows.Forms.Clipboard.SetText(teResult.Text);
                        System.Windows.Forms.MessageBox.Show("号码复制成功。");
                    }
                }
                else if (btn.Name == "btnClear")
                {
                    /*清空條件*/
                    ucThree1.SetDefaultValue();

                    SetDefaultValue();
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
