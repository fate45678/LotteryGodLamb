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
using System.Windows.Forms;
using System.IO;
using WpfAppTest.AP;

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
            AllConbination = DB.CombinationNumber(5, 0, 9);
            cblSpecialExclude.ItemsSource = DB.FiveStart_SpecialExclude();

            if (form == null)
                form = new Dictionary<int, System.Windows.Controls.UserControl>();
            int i = 0;
            foreach (var tmp in DB.FiveStartTab())
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
        /// 右半-結果區的按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (sender as System.Windows.Controls.Button);
            if (btn != null)
            {
                if (btn.Name == "btnCopy")
                {
                    /*全部複製功能*/
                    if (!string.IsNullOrEmpty(teResult.Text))
                    {
                        System.Windows.Forms.Clipboard.SetText(teResult.Text);
                        System.Windows.Forms.MessageBox.Show("號碼複製成功。");
                    }
                }
                else if (btn.Name == "btnExport")
                {
                    /*匯出全部號碼*/
                    FolderBrowserDialog path = new FolderBrowserDialog();
                    if (path.ShowDialog() == DialogResult.OK)
                    {
                        string exportPath = @"" + path.SelectedPath + @"\五星號碼匯出.txt";
                        FileStream fs = new FileStream(exportPath, FileMode.Create, FileAccess.ReadWrite);
                        StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                        sw.Write(teResult.Text);
                        sw.Close();
                        fs.Close();
                        System.Windows.Forms.MessageBox.Show("號碼匯出成功。");
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
                int i = (tcSettings.SelectedIndex > 1 && tcSettings.SelectedIndex < 10 ? 2 : tcSettings.SelectedIndex + 1); ;
                (form[i] as UcFiveStart2).TextBoxEnable(tcSettings.SelectedIndex);
            }
        }
    }
}
