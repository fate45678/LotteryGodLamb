using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp.Custom;
using Wpf.Base;

namespace WpfAppTest
{
    /// <summary>
    /// UcTwoStart.xaml 的互動邏輯
    /// </summary>
    public partial class UcTwoStart : UserControl
    {
        public UcTwoStart()
        {
            InitializeComponent();
        }

        bool IsFirstTime = true;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsFirstTime)
            {
                SetData();
                SetContextMenu();
                SetDefaultValue();
                IsFirstTime = false;
            }
        }

        #region 其他及初始事件
        /// <summary>
        /// 委派事件
        /// </summary>
        public delegate void DataRefreshHandler(string text);

        public void DataRefresh(string text)
        {
            if (tcSettings.SelectedIndex == 8)
            {
                teResult.Text = text;
            }
        }

        /// <summary>
        /// 存放form
        /// </summary>
        Dictionary<int, System.Windows.Controls.UserControl> form;

        /// <summary>
        /// 二星所有組合
        /// </summary>
        List<BaseOptions> AllConbination;

        /// <summary>
        /// 設定資料
        /// </summary>
        void SetData()
        {
            if (form == null)
                form = new Dictionary<int, System.Windows.Controls.UserControl>();

            AllConbination = Calculation.CombinationNumber(2, 0, 9);

            var tab = Calculation.CreateOption(1, 9, new string[9] { "胆码", "定位形态", "不定位形态", "和值", "和尾", "跨度", "和差", "垃圾组合", "大底集合" });
            foreach (var tmp in tab)
            {
                if (tmp.ID == 1)
                    form.Add(tmp.ID, new UcTwoStart1());
                else if (tmp.ID == 2)
                    form.Add(tmp.ID, new UcTwoStart2());
                else if (tmp.ID == 3)
                    form.Add(tmp.ID, new UcTwoStart3());
                else if (tmp.ID == 4)
                    form.Add(tmp.ID, new UcTwoStart4());
                else if (tmp.ID == 5)
                    form.Add(tmp.ID, new UcTwoStart5());
                else if (tmp.ID == 6)
                    form.Add(tmp.ID, new UcTwoStart6());
                else if (tmp.ID == 7)
                    form.Add(tmp.ID, new UcTwoStart7());
                else if (tmp.ID == 8)
                    form.Add(tmp.ID, new UcTwoStart8());
                else if (tmp.ID == 9)
                {
                    UcTwoStart9 tmp2 = new UcTwoStart9();
                    tmp2.DataRefresh = DataRefresh;
                    form.Add(tmp.ID, tmp2);
                }
                TabItem tb = new TabItem() { Content = form[tmp.ID], Header = tmp.Name };
                tcSettings.Items.Add(tb);
            }
            (tcSettings.Items[6] as TabItem).Visibility = Visibility.Collapsed;
        }

        bool IsSetting = false;
        void SetDefaultValue()
        {
            IsSetting = true;
            if (cm != null)
                mi_Click(cm.Items[0], null);
            teResult.Text = "";
            tbCount.Text = "0";
            IsSetting = false;
        }
        #endregion

        #region 複製相關事件
        /// <summary>
        /// 複製的contextmenu
        /// </summary>
        ContextMenu cm;

        /// <summary>
        /// 設定button contextmenu
        /// </summary>
        private void SetContextMenu()
        {
            if (cm == null)
            {
                cm = new ContextMenu();

                MenuItem miCopyResult = new MenuItem() { Header = "复制", Tag = "miCopyResult" };
                //miCopyResult.Icon = new Image() { Source = ImageCollection.GetGlyph("Toolbar_GoTo16") };
                miCopyResult.Click += mi_Click;
                cm.Items.Add(miCopyResult);

                MenuItem miCopyA = new MenuItem() { Header = "复制到大底A", Tag = "miCopyA" };
                //miCopyA.Icon = new Image() { Source = ImageCollection.GetGlyph("Toolbar_Clear16") };
                miCopyA.Click += mi_Click;
                cm.Items.Add(miCopyA);

                MenuItem miCopyB = new MenuItem() { Header = "复制到大底B", Tag = "miCopyB" };
                //miCopyB.Icon = new Image() { Source = ImageCollection.GetGlyph("Toolbar_Clear16") };
                miCopyB.Click += mi_Click;
                cm.Items.Add(miCopyB);
            }
            btnCopy.ContextMenu = cm;
        }

        /// <summary>
        /// ContextMenu click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mi_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            if (mi != null)
            {
                if (mi.Tag != null)
                {
                    if ((string)mi.Tag == "miCopyResult")
                    {
                        /*複製號碼*/
                        if (!string.IsNullOrEmpty(teResult.Text))
                        {
                            System.Windows.Forms.Clipboard.SetText(teResult.Text);
                            if (!IsSetting)
                                System.Windows.Forms.MessageBox.Show("号码复制成功。");
                        }
                    }
                    else if ((string)mi.Tag == "miCopyA")
                    {
                        /*複製到大底A*/
                        if (!string.IsNullOrEmpty(teResult.Text))
                            (form[9] as UcTwoStart9).teCompareA.Text = teResult.Text;
                    }
                    else if ((string)mi.Tag == "miCopyB")
                    {
                        /*複製到大底B*/
                        if (!string.IsNullOrEmpty(teResult.Text))
                            (form[9] as UcTwoStart9).teCompareB.Text = teResult.Text;
                    }
                    btnCopy.Content = mi.Header;
                    btnCopy.Tag = (string)mi.Tag;
                }
            }
        }
        #endregion

        #region tab切換事件
        /// <summary>
        /// Tab Change事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcSettings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tcSettings.SelectedIndex == 8)
            {
                //大底集合才要出現
                btnSelectAll.IsEnabled = false;
                btnClearA.Visibility = Visibility.Visible;
                btnClearB.Visibility = Visibility.Visible;
            }
            else
            {
                btnSelectAll.IsEnabled = (tcSettings.SelectedIndex < 6);
                btnClearA.Visibility = Visibility.Collapsed;
                btnClearB.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region 按鈕事件
        /// <summary>
        /// 按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (sender as Button);
            if (btn.Name == "btnOptionClear")
            {
                /*全清(此頁)*/
                BaseHelper.DynamicPublicMethod(form[tcSettings.SelectedIndex + 1], "SetDefaultValue", new object[] { });
            }
            else if (btn.Name == "btnSelectAll")
            {
                /*全選(此頁)*/
                if (tcSettings.SelectedIndex >= 0 && tcSettings.SelectedIndex <= 6)
                    BaseHelper.DynamicPublicMethod(form[tcSettings.SelectedIndex + 1], "SelectAll", new object[] { });
            }
            else if (btn.Name == "btnFilter")
            {
                /*號碼縮水*/
                string btncontent = (string)btn.Content;
                btn.IsEnabled = false;
                btn.Content = "载入中...";

                var tmp = (form[1] as UcTwoStart1).Filter(AllConbination);
                tmp = (form[2] as UcTwoStart2).Filter(tmp);
                tmp = (form[3] as UcTwoStart3).Filter(tmp);
                tmp = (form[4] as UcTwoStart4).Filter(tmp);
                tmp = (form[5] as UcTwoStart5).Filter(tmp);
                tmp = (form[6] as UcTwoStart6).Filter(tmp);
                tmp = (form[8] as UcTwoStart8).Filter(tmp);

                //和差邏輯目前不需要, 大底集合不須縮水
                //tmp = (form[7] as UcTwoStart7).Filter(tmp);

                teResult.Text = string.Join(" ", tmp.Select(x => x.Code));
                tbCount.Text = tmp.Count.ToString();
                btn.Content = btncontent;
                btn.IsEnabled = true;
            }
            else if (btn.Name == "btnClear")
            {
                /*結果清除*/
                foreach (var item in tcSettings.Items)
                    BaseHelper.DynamicPublicMethod((item as TabItem).Content, "SetDefaultValue", new object[] { });
                SetDefaultValue();
            }
            else if (btn.Name == "btnClearA")
            {
                /*清除大底A*/
                (form[9] as UcTwoStart9).teCompareA.Text = "";
            }
            else if (btn.Name == "btnClearB")
            {
                /*清除大底B*/
                (form[9] as UcTwoStart9).teCompareB.Text = "";
            }
            else if (btn.Name == "btnCopy")
            {
                /*複製*/
                if (btnCopy.Tag != null)
                {
                    if (cm != null)
                    {
                        switch ((string)btnCopy.Tag)
                        {
                            case "miCopyResult":
                                mi_Click(cm.Items[0], null);
                                break;
                            case "miCopyA":
                                mi_Click(cm.Items[1], null);
                                break;
                            case "miCopyB":
                                mi_Click(cm.Items[2], null);
                                break;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
