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
    /// UcTwoStart.xaml 的互動邏輯
    /// </summary>
    public partial class UcTwoStart9 : UserControl
    {
        public UcTwoStart9()
        {
            InitializeComponent();
        }

        public UcTwoStart.DataRefreshHandler DataRefresh;

        /// <summary>
        /// 設定預設值
        /// </summary>
        public void SetDefaultValue()
        {
            teCompareA.Text = "";
            teCompareB.Text = "";
        }

        /// <summary>
        /// 按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (sender as Button);
            if (btn != null)
            {
                if (btn.Name == "btnIntersection" || btn.Name == "btnUnion" || btn.Name == "btnExclude" ||
                    btn.Name == "btnExcludeB" || btn.Name == "btnExcludeA")
                {
                    string[] arrayA = new string[] { };
                    string[] arrayB = new string[] { };
                    string[] arrayC = new string[] { };
                    string[] empty = new string[1] { "" };
                    arrayA = teCompareA.Text.Split(' ').ToArray();
                    arrayB = teCompareB.Text.Split(' ').ToArray();

                    if (btn.Name == "btnIntersection")
                    {
                        /*AB交集*/
                        arrayC = arrayA.Intersect(arrayB).Except(empty).ToArray();
                    }
                    else if (btn.Name == "btnUnion")
                    {
                        /*AB並集*/
                        arrayC = arrayA.Union(arrayB).Except(empty).ToArray();
                    }
                    else if (btn.Name == "btnExclude")
                    {
                        /*AB互斥*/
                        arrayC = arrayA.Union(arrayB).Except(empty).ToArray();
                        arrayC = arrayC.Except(arrayA.Intersect(arrayB).Except(empty).ToArray()).ToArray();
                    }
                    else if (btn.Name == "btnExcludeB")
                    {
                        /*A排除B*/
                        arrayC = arrayA.Except(arrayB).ToArray();
                    }
                    else if (btn.Name == "btnExcludeA")
                    {
                        /*B排除A*/
                        arrayC = arrayB.Except(arrayA).ToArray();
                    }

                    //Array.Sort(arrayC);
                    DataRefresh(string.Join(" ", arrayC));
                }
            }
        }

    }
}
