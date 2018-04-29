using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinFormsApp1
{
    /// <summary>
    /// bigbuttom.xaml 的互動邏輯
    /// </summary>
    public partial class bigbuttom : UserControl
    {
        Connection con = new Connection();
        public bigbuttom()
        {
            InitializeComponent();
            UpdateProgressBar(0,0);
        }

        private void GDMaster_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateProgressBar(double Sum,int count)
        {
            if (Sum == 0 && count == 0)
            {
                pg.Value = 0;
                lbpercent.Content = "0 %";
            }
            else
            {
                pg.Value = Sum / count;
                lbpercent.Content = (Sum / count)+" %";
            }
            
        }

        private void dpEnd_Loaded(object sender, RoutedEventArgs e)
        {
            dpEnd.SelectedDate = DateTime.Today;
        }

        private void dpStart_Loaded(object sender, RoutedEventArgs e)
        {
            dpStart.SelectedDate = DateTime.Today;
        }

        List<string> dt;
        private BitmapSource ScreenCapture;

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (rbChongqingLottery.IsChecked == true)
            {
                UpdateProgressBar(100, 1);//更新進度條 之後改用多執行緒更新

                Dictionary<int, string> dic = new Dictionary<int, string>();
                dic.Add(0, "issue");
                dic.Add(1, "number");
                dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from HistoryNumber where  convert(datetime,substring(issue,0,9)) between '" + dpStart.SelectedDate.ToString().Substring(0, dpStart.SelectedDate.ToString().IndexOf("上") - 1) + "' and '" + dpEnd.SelectedDate.ToString().Substring(0, dpEnd.SelectedDate.ToString().IndexOf("上") - 1) + "'", dic);
                if (dt != null)
                {
                    Dictionary<string, string> history = new Dictionary<string, string>();
                    for (int i = 0; i < dt.Count; i = i + 2)
                        history.Add(dt.ElementAt(i), dt.ElementAt(i + 1));
                    if (history != null)
                    {
                        //GDMaster.ItemsSource = history;
                        List<history> ht = new List<history>();
                        for (int i = 0; i < dt.Count; i = i + 2)
                        {
                            ht.Add(new history()
                            {
                                issue = dt.ElementAt(i),
                                number = dt.ElementAt(i + 1),
                                result = ""
                            });

                        }
                        GDMaster.ItemsSource = ht;
                    }
                }
            }
            else
                MessageBox.Show("請選擇彩種。");
           
            
        }

        public static BitmapImage toBitmap(Byte[] value)
        {
            if (value != null && value is byte[])
            {
                byte[] ByteArray = value as byte[];
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(ByteArray);
                bmp.EndInit();
                return bmp;
            }
            return null;
        }

        private void tbNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strData = tbNum.Text;
            string goal = ",";
            string strReplace = strData.Replace(goal, "");
            tbCount.Text = ((strData.Length - strReplace.Length) / goal.Length).ToString();
        }

        private void btCopy_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText(tbNum.Text);
            System.Windows.Forms.MessageBox.Show("複製成功。");
        }

        private void btClear_Click(object sender, RoutedEventArgs e)
        {
            tbNum.Text = "";
        }

        private void btPaste_Click(object sender, RoutedEventArgs e)
        {
            tbNum.Text += System.Windows.Clipboard.GetText();
        }

        private void btStart_Click(object sender, RoutedEventArgs e)
        {
            //檢查
            if (checkCbNTExt())
            {
                List<history> ht = new List<history>();
                for (int i = 0; i < dt.Count; i = i + 2)
                {
                    //判斷號碼是否相符

                    ht.Add(new history()
                    {
                        issue = dt.ElementAt(i),
                        number = dt.ElementAt(i + 1),
                        result = "V"
                    });
                }
                GDMaster.ItemsSource = ht;
            }
            else
            {

            }
        }
        /// <summary>
        /// 檢查號碼是否符合規則
        /// </summary>
        /// <param name="number"></param>
        /// <param name="rule"></param>
        /// <param name="type"></param>
        private string checkNum(string number,string rule,int type)
        {
            //一樣
            if (1==1)
                return "V";
            else
                return "X";
        }

        /// <summary>
        /// 檢查最上面checkbox勾選數量是否小於等於我的號碼textbox數字
        /// </summary>
        private bool checkCbNTExt()
        {
            #region 計算最上面個十百千萬 checkbox勾選數量
            int count = 0;
            if (CBtenThousand.IsChecked==true)
                count++;
            if (CBThousand.IsChecked == true)
                count++;
            if (CBhundred.IsChecked == true)
                count++;
            if (CBten.IsChecked == true)
                count++;
            if (CBones.IsChecked == true)
                count++;
            #endregion
            //只有一組規則
            //if (count != tbNum.Text.Length && tbNum.Text.IndexOf(" ") != -1)
            //{
            //    MessageBox.Show("沒有可用的號碼。");
            //    return false;
            //}
            ////有一組以上規則
            //else if ()
            //{

            //}
            //else
            //    return true;


            //if (tbNum.Text.Trim().Length % count == 0)
            //{

            //}
            //else
            //{

            //}
            return true;

        }
        public class history
        {
            public string issue { get; set; }
            public string number { get; set; }
            public string result { get; set; }
            
        }
    }
}
