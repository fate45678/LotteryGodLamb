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

namespace WpfAppTest
{
    /// <summary>
    /// UcNumberFilter.xaml 的互動邏輯
    /// </summary>
    public partial class UcNumberFilter : UserControl
    {
        public UcNumberFilter()
        {
            InitializeComponent();
            SetCheckBox();
            this.DataContext = this;
        }

        public List<BaseOptions> Checklist;

        private void SetCheckBox()
        {
            Checklist = new List<BaseOptions> { new BaseOptions { ID = 1, Name = "0" },
                                                new BaseOptions { ID = 2, Name = "1" },
                                                new BaseOptions { ID = 3, Name = "2" },
                                                new BaseOptions { ID = 4, Name = "3" }};
            cbloption1.ItemsSource = Checklist;
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (sender as Button);
            if (btn != null)
            {
                if (btn.Name == "btnAll")
                {
                    /*左邊全選*/
                    cbloption1.SelectedAll();
                }
                else if (btn.Name == "btnClear")
                {
                    /*左邊全清*/
                    cbloption1.Clear();
                }
                else if (btn.Name == "btnFilter")
                {
                    /*縮水*/

                }
                else if (btn.Name == "btnResultCopy")
                {
                    /*結果複製*/
                    tbResult.Copy();
                }
                else if (btn.Name == "btnResultClear")
                {
                    /*結果全清*/
                    tbResult.Text = "";
                }
            }
        }

        private void cbItems_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 結果區的buttonclick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResult_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbloption1_SelectedValueChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}
