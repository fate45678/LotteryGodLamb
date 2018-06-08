using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.IO;
using WpfApp.Custom;
using Wpf.Base;

namespace WpfAppTest
{
    /// <summary>
    /// UcFiveStart1.xaml 的互動邏輯
    /// </summary>
    public partial class UcFiveStart : System.Windows.Controls.UserControl
    {
        public UcFiveStart()
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
                if (tcSettings.Items.Count > 9)
                    (tcSettings.Items[9] as TabItem).Visibility = System.Windows.Visibility.Collapsed;
                IsFirstTime = false;
            }
        }

        Dictionary<int, System.Windows.Controls.UserControl> form;

        /// <summary>
        /// 五星所有組合
        /// </summary>
        List<BaseOptions> AllConbination;

        /// <summary>
        /// 設定資料
        /// </summary>
        void SetData()
        {
            btnTransfer.IsEnabled = false;
            AllConbination = Calculation.CombinationNumber(5, 0, 9);
            cblSpecialExclude.ItemsSource = Calculation.CreateOption(1, 14, new string[14] { "上山", "下山", "不连", "2连", "3连", "4连", "5连", "AAAAA", "AABCD", "AABBC", "AAABB", "AAABC", "AAAAB", "ABCDE" });

            if (form == null)
                form = new Dictionary<int, System.Windows.Controls.UserControl>();
            int i = 0;
            foreach (var tmp in CustomOption.FiveStartTab())
            {
                if (tmp.ID == 1)
                    form.Add(tmp.ID, new UcFiveStart1());
                else if (tmp.ID == 2)
                    form.Add(tmp.ID, new UcFiveStart2());
                else if (tmp.ID == 11)
                    form.Add(tmp.ID, new UcFiveStart3());
                else if (tmp.ID == 12)
                    form.Add(tmp.ID, new UcFiveStart4());
                else if (tmp.ID == 13)
                    form.Add(tmp.ID, new UcFiveStart5());

                i = (tmp.ID > 2 && tmp.ID < 10 ? 2 : tmp.ID);
                TabItem tb = new TabItem() { Content = (form.Where(x => x.Key == i).FirstOrDefault().Value), Header = tmp.Name };
                tcSettings.Items.Add(tb);
            }
        }

        /// <summary>
        /// 設定預設值
        /// </summary>
        void SetDefaultValue()
        {
            /*ComboBoxList*/
            cblSpecialExclude.Clear();

            /*TextBox*/
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

                    //BaseHelper.DynamicPublicMethod((tcSettings.Items[0] as TabItem).Content, "Filter", new object[1] { AllConbination });

                    /*原寫法
                    ////大底先篩
                    //var tmp = ((tcSettings.Items[12] as TabItem).Content as UcFiveStart5).Filter(AllConbination);

                    ////特別排除
                    //tmp = Calculation.FiveSpecialData(tmp, cblSpecialExclude.SelectedValue);

                    //tmp = ((tcSettings.Items[0] as TabItem).Content as UcFiveStart1).Filter(tmp);
                    //tmp = ((tcSettings.Items[1] as TabItem).Content as UcFiveStart2).Filter(tmp);
                    //tmp = ((tcSettings.Items[10] as TabItem).Content as UcFiveStart3).Filter(tmp);
                    */

                    //大底先篩
                    List<BaseOptions> data = ((tcSettings.Items[12] as TabItem).Content as UcFiveStart5).Filter(AllConbination);

                    //最後結果
                    List<BaseOptions> tmp = new List<BaseOptions>();

                    foreach (var item in data)
                    {
                        var UnitItem = new List<BaseOptions>() { item };

                        //特別排除
                        UnitItem = Calculation.FiveSpecialData(UnitItem, cblSpecialExclude.SelectedValue);

                        UnitItem = ((tcSettings.Items[0] as TabItem).Content as UcFiveStart1).Filter(UnitItem);
                        UnitItem = ((tcSettings.Items[1] as TabItem).Content as UcFiveStart2).Filter(UnitItem);
                        UnitItem = ((tcSettings.Items[10] as TabItem).Content as UcFiveStart3).Filter(UnitItem);

                        if (UnitItem.Count > 0)
                            tmp.Add(UnitItem[0]);
                    }

                    teResult.Text = string.Join(" ", tmp.OrderBy(x => x.Code).Select(x => x.Code));
                    tbCount.Text = tmp.Count.ToString();

                    //組選
                    FilterData = tmp;
                    btnTransfer.IsEnabled = true;

                    //btn啟用
                    btn.Content = btncontent;
                    btn.IsEnabled = true;
                }
                else if (btn.Name == "btnClear")
                {
                    /*清空所有條件*/
                    foreach (var items in form)
                        BaseHelper.DynamicPublicMethod(items.Value, "SetDefaultValue", new object[] { });
                    SetDefaultValue();
                }
                else if (btn.Name == "btnTransfer")
                {
                    /*轉為組選*/
                    if (FilterData != null)
                    {
                        var tmp = Calculation.TransNumber(FilterData);
                        teResult.Text = string.Join(" ", tmp.Select(x => x.Code));
                        tbCount.Text = tmp.Count.ToString();
                    }
                }
                else if (btn.Name == "btnCopy")
                {
                    /*全部複製功能*/
                    if (!string.IsNullOrEmpty(teResult.Text))
                    {
                        System.Windows.Forms.Clipboard.SetText(teResult.Text);
                        System.Windows.Forms.MessageBox.Show("号码复制成功。");
                    }
                }
                else if (btn.Name == "btnExport")
                {
                    /*匯出全部號碼*/
                    FolderBrowserDialog path = new FolderBrowserDialog();
                    if (path.ShowDialog() == DialogResult.OK)
                    {
                        string exportPath = @"" + path.SelectedPath + @"\五星号码导出.txt";
                        FileStream fs = new FileStream(exportPath, FileMode.Create, FileAccess.ReadWrite);
                        StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                        sw.Write(teResult.Text);
                        sw.Close();
                        fs.Close();
                        System.Windows.Forms.MessageBox.Show("号码导出成功。");
                    }
                }
            }
        }

        /// <summary>
        /// 頁籤切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcSettings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsFirstTime) return;
            if (e.Source != sender || e.OriginalSource != sender) return;

            if (tcSettings.SelectedIndex > 0 && tcSettings.SelectedIndex < 10)
            {
                BaseHelper.DynamicPublicMethod((tcSettings.Items[tcSettings.SelectedIndex] as TabItem).Content, "TextBoxEnable", new object[1] { tcSettings.SelectedIndex });

                //int i = (tcSettings.SelectedIndex > 1 && tcSettings.SelectedIndex < 10 ? 2 : tcSettings.SelectedIndex + 1); ;
                //(form[i] as UcFiveStart2).TextBoxEnable(tcSettings.SelectedIndex);
            }
        }
    }
}
