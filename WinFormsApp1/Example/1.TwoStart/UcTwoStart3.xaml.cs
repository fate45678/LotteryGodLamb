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
    public partial class UcTwoStart3 : UserControl
    {
        public UcTwoStart3()
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

        CheckBoxList cblOption1_2;
        CheckBoxList cblOption2_2;
        void SetData()
        {
            cblOption1_2 = new CheckBoxList();
            cblOption1_2.DisplayMemberPath = "Name";
            cblOption1_2.ValueMemberPath = "ID";

            cblOption2_2 = new CheckBoxList();
            cblOption2_2.DisplayMemberPath = "Name";
            cblOption2_2.ValueMemberPath = "ID";

            /*CheckBoxList*/
            var dataOption1 = Calculation.CombinationNumber(2, 0, 2).OrderBy(x => x.Code).ToList();
            var dataOption2 = Calculation.CombinationNumber(2, 0, 2, new string[3] { "小", "中", "大" }).ToList();
            cblOption1_2.ItemsSource = dataOption1;
            cblOption2_2.ItemsSource = dataOption2;

            var array1 = new string[6] { "00", "01", "02", "12", "11", "22" };

            cblOption1.ItemsSource = dataOption1.Where(x => array1.Contains(x.Code)).ToList();
            cblOption2.ItemsSource = dataOption2.Where(x => array1.Contains(x.Code)).OrderByDescending(x => x.Code).ToList();
            cblOption3.ItemsSource = Calculation.CreateOption(1, 4, new string[4] { "对子", "连号", "杂号", "假对" }).OrderBy(x => x.ID); //, "假连"

            /*RadioButtonList*/
            var data = Calculation.CreateOption(1, 2, new string[2] { "保留", "排除" });
            rblOption1.ItemsSource = data;
            rblOption2.ItemsSource = data;
            rblOption3.ItemsSource = data;

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

            /*RadioButtonList*/
            rblOption1.SelectedValue = 1;
            rblOption2.SelectedValue = 1;
            rblOption3.SelectedValue = 1;
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
        }

        /// <summary>
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            //012路
            cblOption1_2.SelectedValue = changevalue(cblOption1_2, cblOption1);
            tmp = Calculation.DivThreeRemainder(tmp, cblOption1_2, ((int)rblOption1.SelectedValue == 1));

            //大中小
            cblOption2_2.SelectedValue = changevalue(cblOption2_2, cblOption2);
            tmp = Calculation.CheckValueNumber(tmp, cblOption2_2, 2, ((int)rblOption2.SelectedValue == 1));

            //類型
            tmp = Calculation.TwoStartType(tmp, cblOption3.SelectedValue, ((int)rblOption3.SelectedValue == 1));
            return tmp;
        }

        private string changevalue(CheckBoxList dest, CheckBoxList source)
        {
            string str = source.SelectedValue;
            var datadest = dest.ItemsSource.Cast<BaseOptions>().ToList();
            var datasource = source.ItemsSource.Cast<BaseOptions>().ToList();

            for (int i = 0; i < source.SelectedValue.Length; i++)
            {
                if (source.SelectedValue[i] == '1')
                {
                    string test = datasource.Where(x => x.ID == i + 1).FirstOrDefault().Code;
                    string test2 = (test[1].ToString() + test[0].ToString());
                    int id = datadest.Where(x => x.Code == test2).FirstOrDefault().ID;
                    var array = str.ToCharArray();
                    array[id - 1] = '1';
                    str = string.Join("", array);
                }
            }
            return str;
        }
    }
}
