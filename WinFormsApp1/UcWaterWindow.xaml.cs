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
using WpfAppTest;
using WpfAppTest.AP;

namespace WinFormsApp1
{
    /// <summary>
    /// UcWaterWindow.xaml 的互動邏輯
    /// </summary>
    public partial class UcWaterWindow : UserControl
    {
        public UcWaterWindow()
        {
            InitializeComponent();
        }
        
        bool IsFirstTime = true;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsFirstTime)
            {
                SetListBoxItems();
                IsFirstTime = false;
            }
        }

        /// <summary>
        /// 設定選項
        /// </summary>
        void SetListBoxItems()
        {
            List<BaseOptions> tmp = new List<BaseOptions>();
            for (int i = 2; i <= 5; i++)
                lbType.Items.Add(new BaseOptions { ID = i, Name = i.ToString() + "星組號系統", Code = "lbType" + (i - 1).ToString() });
            if (lbType.Items.Count > 0)
                lbType.SelectedIndex = 0;
        }

        Dictionary<string, UserControl> form;

        /// <summary>
        /// ListBox選取選項事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BaseOptions tmp = (lbType.SelectedItem as BaseOptions);
            if (tmp != null)
            {
                if (form == null)
                    form = new Dictionary<string, UserControl>();

                if (!form.ContainsKey(tmp.Code))
                {
                    if (tmp.Code == "lbType1")
                        form.Add(tmp.Code, new UcTwoStart());
                    else if (tmp.Code == "lbType2")
                        form.Add(tmp.Code, new UcFiveStart());
                    else if (tmp.Code == "lbType3")
                        form.Add(tmp.Code, new UcFiveStart());
                    else if (tmp.Code == "lbType4")
                        form.Add(tmp.Code, new UcFiveStart());
                }

                dpContent.Children.Clear();
                dpContent.Children.Add(form[tmp.Code]);
            }
        }
    }
}
