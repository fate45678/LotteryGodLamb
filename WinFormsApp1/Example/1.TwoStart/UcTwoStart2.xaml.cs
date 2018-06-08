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
    public partial class UcTwoStart2 : UserControl
    {
        public UcTwoStart2()
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

        void SetData()
        {
            /*CheckBoxList*/
            cblOption1.ItemsSource = Calculation.CombinationNumber(2, 0, 2).OrderBy(x => x.Code);
            cblOption2.ItemsSource = Calculation.ZeroOneCombination(2, '奇', '偶').OrderBy(x => x.Code);
            cblOption3.ItemsSource = Calculation.ZeroOneCombination(2, '大', '小').OrderByDescending(x => x.Code);
            cblOption4.ItemsSource = Calculation.ZeroOneCombination(2, '质', '合').OrderByDescending(x => x.Code);

            /*RadioButtonList*/
            var Data = Calculation.CreateOption(1, 2, new string [2] { "保留", "排除" });
            rblOption1.ItemsSource = Data;
            rblOption2.ItemsSource = Data;
            rblOption3.ItemsSource = Data;
            rblOption4.ItemsSource = Data;

            /*預設值*/
            SetDefaultValue();
        }

        /// <summary>
        /// 設定預設值
        /// </summary>
        public void SetDefaultValue()
        {
            /*CheckBoxList*/
            cblOption1.Clear();
            cblOption2.Clear();
            cblOption3.Clear();
            cblOption4.Clear();

            /*RadioButtonList*/
            rblOption1.SelectedValue = 1;
            rblOption2.SelectedValue = 1;
            rblOption3.SelectedValue = 1;
            rblOption4.SelectedValue = 1;
        }

        /// <summary>
        /// 選取全部選項
        /// </summary>
        public void SelectAll()
        {
            /*CheckBoxList*/
            cblOption1.SelectedAll();
            cblOption2.SelectedAll();
            cblOption3.SelectedAll();
            cblOption4.SelectedAll();
        }

        /// <summary>
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            tmp = Calculation.DivThreeRemainder(tmp, cblOption1, ((int)rblOption1.SelectedValue == 1));
            tmp = Calculation.OddEvenNumber(tmp, cblOption2, ((int)rblOption2.SelectedValue == 1));
            tmp = Calculation.CheckValueNumber(tmp, cblOption3, 1, ((int)rblOption3.SelectedValue == 1));
            tmp = Calculation.PrimeNumber(tmp, cblOption4, ((int)rblOption4.SelectedValue == 1));
            return tmp;
        }
    }
}
