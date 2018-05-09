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
    public partial class UcTwoStart8 : UserControl
    {
        public UcTwoStart8()
        {
            InitializeComponent();
        }

        #region 外部呼叫程式
        /// <summary>
        /// 設定預設值
        /// </summary>
        public void SetDefaultValue()
        {
            /*TextBox*/
            teCompareA.Text = "";
            teCompareB.Text = "";
        }
        #endregion

        /// <summary>
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            //垃圾複式
            tmp = Base.Calculation.GarbageNumber(tmp, teCompareA.Text, 2, '/', 2);

            //垃圾單式
            tmp = Base.Calculation.GarbageNumber(tmp, teCompareB.Text, 1, ',');
            return tmp;
        }
    }
}
