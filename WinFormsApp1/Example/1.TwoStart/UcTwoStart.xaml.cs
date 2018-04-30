using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppTest.AP;
using WpfAppTest.Base;

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
                IsFirstTime = false;
            }
        }

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

            AllConbination = DB.CombinationNumber(2, 0, 9);

            var tab = DB.CreateOption(1, 9, new string[9] { "胆码", "定位形态", "不定位形态", "和值", "合尾", "跨度", "和差", "垃圾组合", "大底集合" });
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
            cbCopy.SelectedIndex = 0;
        }

        /// <summary>
        /// contextmenu
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

                MenuItem miCopyResult = new MenuItem() { Header = "複製", Tag = "miCopyResult" };
                //miCopyResult.Icon = new Image() { Source = ImageCollection.GetGlyph("Toolbar_GoTo16") };
                miCopyResult.Click += mi_Click;
                cm.Items.Add(miCopyResult);

                MenuItem miCopyA = new MenuItem() { Header = "複製到大底A", Tag = "miCopyA" };
                //miCopyA.Icon = new Image() { Source = ImageCollection.GetGlyph("Toolbar_Clear16") };
                miCopyA.Click += mi_Click;

                MenuItem miCopyB = new MenuItem() { Header = "複製到大底B", Tag = "miCopyB" };
                //miCopyB.Icon = new Image() { Source = ImageCollection.GetGlyph("Toolbar_Clear16") };
                miCopyB.Click += mi_Click;
                cm.Items.Add(miCopyB);
            }
            btnCopy.ContextMenu = cm;
        }

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
                            System.Windows.Forms.MessageBox.Show("號碼複製成功。");
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
                }
            }
        }

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
                var tmp = (form[1] as UcTwoStart1).Filter(AllConbination);
                tmp = (form[2] as UcTwoStart2).Filter(tmp);
                tmp = (form[3] as UcTwoStart3).Filter(tmp);
                tmp = (form[4] as UcTwoStart4).Filter(tmp);
                tmp = (form[5] as UcTwoStart5).Filter(tmp);
                tmp = (form[6] as UcTwoStart6).Filter(tmp);

                //垃圾跟和差邏輯不清楚, 大底集合不須縮水
                //tmp = (form[7] as UcTwoStart7).Filter(tmp);
                //tmp = (form[8] as UcTwoStart8).Filter(tmp);

                teResult.Text = string.Join(" ", tmp.Select(x => x.Code));
            }
            else if (btn.Name == "btnClear")
            {
                /*結果清除*/
                foreach (var item in tcSettings.Items)
                    BaseHelper.DynamicPublicMethod((item as TabItem).Content, "SetDefaultValue", new object[] { });
                teResult.Text = "";
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
                /*複製號碼*/
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCopy.SelectedIndex == 0)
            {
                /*複製號碼*/
                if (!string.IsNullOrEmpty(teResult.Text))
                {
                    System.Windows.Forms.Clipboard.SetText(teResult.Text);
                    System.Windows.Forms.MessageBox.Show("號碼複製成功。");
                }
            }
            else if (cbCopy.SelectedIndex == 1)
            {
                /*複製到大底A*/
                if (!string.IsNullOrEmpty(teResult.Text))
                    (form[9] as UcTwoStart9).teCompareA.Text = teResult.Text;
            }
            else if (cbCopy.SelectedIndex == 2)
            {
                /*複製到大底B*/
                if (!string.IsNullOrEmpty(teResult.Text))
                    (form[9] as UcTwoStart9).teCompareB.Text = teResult.Text;
            }
        }

        private void cbCopy_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void cbCopy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
