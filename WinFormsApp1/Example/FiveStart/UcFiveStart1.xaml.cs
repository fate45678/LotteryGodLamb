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
    public partial class UcFiveStart1 : System.Windows.Controls.UserControl
    {
        public UcFiveStart1()
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
                GetCombinationData();
                SetComboBox();
                IsFirstTime = false;
            }
        }

        List<BaseOptions> CombinationData;
        List<BaseOptions> Data1;
        List<BaseOptions> Data2;
        List<BaseOptions> Data3;
        List<BaseOptions> Data4;
        //List<BaseOptions> SpecialExculde;
        List<BaseOptions> Ratio;

        void GetCombinationData()
        {
            CombinationData = DB.ZeroOneCombination(5);
            Data1 = DB.ZeroOneCombination(5, '大', '小').OrderByDescending(x=>x.Code).ToList();
            Data2 = DB.ZeroOneCombination(5, '奇', '偶').OrderByDescending(x=>x.Code).ToList();
            Data3 = DB.ZeroOneCombination(5, '質', '合').OrderByDescending(x=>x.Code).ToList();
            Data4 = DB.ZeroToNine().OrderBy(x => x.ID).ToList();
            //SpecialExculde = DB.SpecialExclude();
            Ratio = DB.Ratio(5);
        }

        void SetComboBox()
        {
            cblData1.ItemsSource = Data1;
            cblData2.ItemsSource = Data2;
            cblData3.ItemsSource = Data3;

            cblTenThousands.ItemsSource = Data4;
            cblThousands.ItemsSource = Data4;
            cblHundreds.ItemsSource = Data4;
            cblTens.ItemsSource = Data4;
            cblUnits.ItemsSource = Data4;

            cblTenThousands2.ItemsSource = Data4;
            cblHundreds2.ItemsSource = Data4;
            cblTens2.ItemsSource = Data4;

            cblRatio.ItemsSource = Ratio;
            cblRatio2.ItemsSource = Ratio;
            cblRatio3.ItemsSource = Ratio;

            //cblSpecialExclude.ItemsSource = SpecialExculde;
        }

        /// <summary>
        /// 開啟檔案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

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
                            txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
                        else
                            System.Windows.Forms.MessageBox.Show("非文字檔無法開啟。");
                    }
                    else
                        txtEditor.Text = "";
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("檔案不存在，請確認後再選取。");
                }
            }
        }

        /// <summary>
        /// doubleclick清除文字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEditor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtEditor.Text = "";
        }

        ///// <summary>
        ///// 右半-結果區的按鈕事件
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btn_Click(object sender, RoutedEventArgs e)
        //{
        //    var btn = (sender as System.Windows.Controls.Button);
        //    if (btn != null)
        //    {
        //        if (btn.Name == "btnCopy")
        //        {
        //            /*全部複製功能*/
        //            if (!string.IsNullOrEmpty(teResult.Text))
        //            {
        //                System.Windows.Forms.Clipboard.SetText(teResult.Text);
        //                System.Windows.Forms.MessageBox.Show("號碼複製成功。");
        //            }
        //        }
        //        else if (btn.Name == "btnExport")
        //        {
        //            /*匯出全部號碼*/
        //            FolderBrowserDialog path = new FolderBrowserDialog();
        //            if (path.ShowDialog() == DialogResult.OK)
        //            {
        //                string exportPath = @"" + path.SelectedPath + @"\五星號碼匯出.txt";
        //                FileStream fs = new FileStream(exportPath, FileMode.Create, FileAccess.ReadWrite);
        //                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
        //                sw.Write(teResult.Text);
        //                sw.Close();
        //                fs.Close();
        //                System.Windows.Forms.MessageBox.Show("號碼匯出成功。");
        //            }
        //        }
                
        //    }
        //}
    }
}
