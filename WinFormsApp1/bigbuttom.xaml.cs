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
using System.Text.RegularExpressions;
using Microsoft.Win32;

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

        }

        bool IsFirstTime = true;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsFirstTime)
            {
                SetDefaultValue();
                IsFirstTime = false;
            }
        }

        /// <summary>
        /// 設定預設值
        /// </summary>
        void SetDefaultValue()
        {
            //彩種
            rbChongqingLottery.IsChecked = true;

            //UpdateProgressBar(0, 0);

            //初始化開獎期數
            btn_Click(btnUpdate, null);
        }

        /// <summary>
        /// 更新ProgressBar
        /// </summary>
        /// <param name="Sum"></param>
        /// <param name="count"></param>
        private void UpdateProgressBar(double Sum, int count)
        {
            if (Sum == 0 && count == 0)
            {
                pg.Value = 0;
                lbpercent.Content = "0 %";
            }
            else
            {
                pg.Value = Sum / count;
                lbpercent.Content = (Sum / count) + " %";
            }
        }

        public class history
        {
            public int rowNo { get; set; }
            public string issue { get; set; }
            public string number { get; set; }
            public string result { get; set; }
        }

        /// <summary>
        /// 暫存開獎號
        /// </summary>
        List<history> dtHistory;

        /// <summary>
        /// 按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                if (btn.Name == "btnExport")
                {
                    #region 匯入
                    OpenFileDialog openFileDialog = new OpenFileDialog();

                    // 設定Filter，指定只能開啟特定的檔案 
                    openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                    if ((bool)openFileDialog.ShowDialog())
                    {
                        //檢核是否存在檔案
                        if (File.Exists(openFileDialog.FileName))
                        {
                            //檢核檔名
                            if (!string.IsNullOrEmpty(openFileDialog.FileName))
                            {
                                if (openFileDialog.FileName.Length >= 3 &&
                                    openFileDialog.FileName.Substring(openFileDialog.FileName.Length - 3, 3) == "txt")
                                {
                                    string[] array = File.ReadAllLines(openFileDialog.FileName);
                                    List<history> ht = new List<history>();

                                    if (array.Count() > 0)
                                    {
                                        for (int i = 0; i < array.Count(); i++)
                                        {
                                            string text = FilterText(array[i], 0);
                                            if (text.Length <= 5)
                                                continue;

                                            text = text.Insert(text.Length - 5, " ");
                                            var s = text.Split(' ').ToArray();
                                            ht.Add(new history()
                                            {
                                                rowNo = i + 1,
                                                issue = s[0],
                                                number = s[1],
                                                result = ""
                                            });
                                        }
                                    }
                                    dtHistory = ht.Select(x => new history { rowNo = x.rowNo, issue = x.issue, number = x.number, result = x.result }).ToList();
                                    GDMaster.ItemsSource = ht;
                                }
                                else
                                    System.Windows.Forms.MessageBox.Show("非文字檔無法開啟。");
                            }
                            else
                                tbNum.Text = "";
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("檔案不存在，請確認後再選取。");
                        }
                    }
                    #endregion
                }
                else if (btn.Name == "btnUpdate")
                {
                    #region 更新開獎
                    if (rbChongqingLottery.IsChecked == true)
                    {
                        UpdateProgressBar(100, 1);//更新進度條 之後改用多執行緒更新

                        Dictionary<int, string> dic = new Dictionary<int, string>();
                        dic.Add(0, "issue");
                        dic.Add(1, "number");
                        List<string> dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from HistoryNumber where  convert(datetime,substring(issue,0,9)) between '" + dpStart.SelectedDate.ToString().Substring(0, dpStart.SelectedDate.ToString().IndexOf("上") - 1) + "' and '" + dpEnd.SelectedDate.ToString().Substring(0, dpEnd.SelectedDate.ToString().IndexOf("上") - 1) + "'", dic);
                        if (dt != null)
                        {
                            Dictionary<string, string> history = new Dictionary<string, string>();
                            for (int i = 0; i < dt.Count; i = i + 2)
                                history.Add(dt.ElementAt(i), dt.ElementAt(i + 1));

                            if (history != null)
                            {
                                List<history> ht = new List<history>();
                                for (int i = 0; i < dt.Count; i += 2)
                                {
                                    ht.Add(new history()
                                    {
                                        rowNo = i / 2 + 1,
                                        issue = dt.ElementAt(i),
                                        number = dt.ElementAt(i + 1),
                                        result = ""
                                    });
                                }
                                dtHistory = ht.Select(x => new history { rowNo = x.rowNo, issue = x.issue, number = x.number, result = x.result }).ToList();
                                GDMaster.ItemsSource = ht;
                            }
                        }
                    }
                    else
                        MessageBox.Show("请选择彩种。");
                    #endregion
                }
                else if (btn.Name == "btnCopy")
                {
                    #region 複製號碼
                    System.Windows.Clipboard.SetText(tbNum.Text);
                    System.Windows.Forms.MessageBox.Show("复制成功。");
                    #endregion
                }
                else if (btn.Name == "btnPaste")
                {
                    #region 黏貼號碼
                    tbNum.Text += System.Windows.Clipboard.GetText();
                    #endregion
                }
                else if (btn.Name == "btnClear")
                {
                    #region 清空號碼
                    tbNum.Text = "";
                    #endregion
                }
                else if (btn.Name == "btnStart")
                {
                    #region 開始驗證
                    if (dtHistory == null || dtHistory.Count() == 0)
                    {
                        MessageBox.Show("无开奖号码，无法验证。");
                        return;
                    }

                    //處理文字區段
                    int[] check = new int[5] { ((bool)CBtenThousand.IsChecked ? 1 : 0),
                                               ((bool)CBThousand.IsChecked ? 1 : 0),
                                               ((bool)CBhundred.IsChecked ? 1 : 0),
                                               ((bool)CBten.IsChecked ? 1 : 0),
                                               ((bool)CBones.IsChecked ? 1 : 0)};
                    int checkCount = check.Count();
                    int number = check.Where(x => x == 1).Count();

                    if (number == 0)
                    {
                        MessageBox.Show("请选择号码位置。");
                        return;
                    }

                    string text = FilterText(tbNum.Text, number);
                    var tmp = text.Split(' ').Where(x => x.ToString().Length == number).ToArray();
                    if (tmp.Count() == 0)
                    {
                        MessageBox.Show("没有可用的号码。");
                        return;
                    }
                    text = string.Join(" ", tmp);
                    tbNum.Text = text;
                    tbCount.Text = tmp.Count().ToString();

                    //開始驗證
                    List<history> ht = dtHistory.Select(x => new history { rowNo = x.rowNo, issue = x.issue, number = x.number, result = x.result }).ToList();
                    foreach (var h in ht)
                    {
                        foreach (var x in tmp)
                        {
                            if (h.result != "")
                                continue;

                            bool ismatch = true;
                            int j = 0;
                            for (int i = 0; i < checkCount; i++)
                            {
                                if (!ismatch)
                                    continue;

                                if (check[i] == 1)
                                {
                                    if (h.number[i] != x[j])
                                        ismatch = false;
                                    j++;
                                }
                            }

                            if (ismatch)
                                h.result = "V";//"1";
                        }
                        if (h.result == "")
                            h.result = "╳";//"0";
                    }
                    GDMaster.ItemsSource = ht;

                    //更新期數
                    List<history> CheckTrue = ht.Where(x => x.result == "V").ToList();
                    List<history> CheckFalse = ht.Where(x => x.result == "╳").ToList();
                    int MinTrue = (CheckTrue.Count == 0 ? 0 : CheckTrue.Min(x => x.rowNo));
                    int MinFalse = (CheckFalse.Count == 0 ? 0 : CheckFalse.Min(x => x.rowNo));
                    int MaxTrue = (CheckTrue.Count == 0 ? 0 : CheckTrue.Max(x => x.rowNo));
                    int MaxFalse = (CheckFalse.Count == 0 ? 0 : CheckFalse.Max(x => x.rowNo));

                    tePeriod.Text = ht.Count.ToString();
                    teCorrectPeriod.Text = CheckTrue.Count.ToString();
                    teErrorPeriod.Text = CheckFalse.Count.ToString();

                    //c=>上次 c2=>上上次
                    int conti = 0, c = 1, c2 = 0;
                    if (MinTrue > 0)
                    { 
                        foreach (var item in CheckTrue)
                        {

                            if (item.rowNo - c == 1 && c - c2 != 1)
                                conti++;

                            c2 = c;
                            c = item.rowNo;
                        }
                    }
                    teCorrectContinuePeriod.Text = conti.ToString();

                    conti = 0;
                    c = 1;
                    c2 = 0;
                    if (MinFalse > 0)
                    {
                        foreach (var item in CheckFalse)
                        {

                            if (item.rowNo - c == 1 && c - c2 != 1)
                                conti++;

                            c2 = c;
                            c = item.rowNo;
                        }
                    }
                    teErrorContinuePeriod.Text = conti.ToString();

                    teMissPeriod.Text = (MaxFalse - MaxTrue <= 0 ? 0 : MaxFalse - MaxTrue).ToString();

                    //準確率
                    tePercent.Text = string.Format("{0}%", Math.Round((decimal)((decimal)CheckTrue.Count / (decimal)ht.Count * 100), 2));
                    #endregion
                }
            }
        }

        /// <summary>
        /// 篩選文字
        /// </summary>
        /// <param name="text"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        string FilterText(string text, int number)
        {
            text = Regex.Replace(text, "[^0-9]", "");
            if (number > 0)
            {
                int count = text.Length / number;

                for (int i = 1; i <= count; i++)
                {
                    if (count >= i)
                        text = text.Insert(number * i + (i - 1), " ");
                }
            }
            return text;
        }
    }

    /// <summary>
    /// 圖檔轉換
    /// </summary>
    public class BatchTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || value.ToString() == "" || value.GetType() != typeof(Byte))
                return null;

            int result = int.Parse(value.ToString());
            if (parameter.ToString() == "result")
            {
                if (result == 1)
                    return new Uri(@"..\img\AD1.png");
                else
                    return new Uri(@"..\img\AD2.png");
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
