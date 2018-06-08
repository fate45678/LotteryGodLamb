using System.Collections.Generic;
using System.Windows.Controls;
using Wpf.Base;
using WpfApp.Custom;

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
            tmp = Calculation.GarbageNumber(tmp, teCompareA.Text, 2, '/', 2);

            //垃圾單式
            tmp = Calculation.GarbageNumber(tmp, teCompareB.Text, 1, ',');
            return tmp;
        }
    }
}
