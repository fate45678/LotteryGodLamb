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
                IsFirstTime = false;
            }
        }

        Dictionary<int, System.Windows.Controls.UserControl> form;

        void SetData()
        {
            if (form == null)
                form = new Dictionary<int, System.Windows.Controls.UserControl>();
            
            foreach (var tmp in DB.TwoStartTab())
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
                    form.Add(tmp.ID, new UcTwoStart9());

                TabItem tb = new TabItem() { Content = (form.Where(x => x.Key == tmp.ID).FirstOrDefault().Value), Header = tmp.Name };
                tcSettings.Items.Add(tb);
            }
        }

        private void tcSettings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (sender as Button);
            if (btn.Name == "btnFilter")
            {
                /*號碼縮水*/
            }
            else if (btn.Name == "btnCopy")
            {
                /*複製結果*/
            }
            else if (btn.Name == "btnClear")
            {
                /*結果清除*/
            }
            else if (btn.Name == "btnOptionClear")
            {

            }
        }

    }
}
