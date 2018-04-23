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
using WpfAppTest.Base;

namespace WpfAppTest
{
    /// <summary>
    /// UcFourStart1.xaml 的互動邏輯
    /// </summary>
    public partial class UcThreeStart1 : System.Windows.Controls.UserControl
    {
        public UcThreeStart1()
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
                IsFirstTime = false;
            }
        }

        /// <summary>
        /// 設定資料
        /// </summary>
        void SetData()
        {
            var data = DB.CreateContinueNumber().OrderBy(x => x.ID).ToList();

            /*CheckBoxList*/
            cblData1.ItemsSource = DB.ZeroOneCombination(4, '大', '小').OrderByDescending(x => x.Code).ToList();
            cblData2.ItemsSource = DB.ZeroOneCombination(4, '奇', '偶').OrderByDescending(x => x.Code).ToList();
            cblData3.ItemsSource = DB.ZeroOneCombination(4, '質', '合').OrderByDescending(x => x.Code).ToList();

            cblThousands.ItemsSource = data;
            cblHundreds.ItemsSource = data;
            cblTens.ItemsSource = data;
            cblUnits.ItemsSource = data;

            cblType1.ItemsSource = data;
            cblType2.ItemsSource = data;
            cblType3.ItemsSource = data;
            cblType4.ItemsSource = data;

            cbl012.ItemsSource = DB.CombinationNumber(4, 0, 2).OrderBy(x => x.Code).ToList();
            cbl123.ItemsSource = DB.CreateOption(0, 9);

            cblSpecial.ItemsSource = DB.CreateOption(1, 6, new string[6] { "上山", "下山", "凸型", "凹型", "N型", "反N型" });
            cblSpecialExcept.ItemsSource = DB.CreateOption(1, 9, new string[9] { "豹子", "不連", "2連", "3連", "4連", "散號", "對子號", "三同號", "兩個對子" });

            /*設定預設值*/
            SetDefaultValue();
        }

        /// <summary>
        /// 是否由程式設定值
        /// </summary>
        bool IsSetting = false;

        /// <summary>
        /// 設定預設值
        /// </summary>
        public void SetDefaultValue()
        {
            IsSetting = true;

            /*CheckBoxList*/
            cblData1.Clear();
            cblData2.Clear();
            cblData3.Clear();

            cblThousands.Clear();
            cblHundreds.Clear();
            cblTens.Clear();
            cblUnits.Clear();

            cblType1.Clear();
            cblType2.Clear();
            cblType3.Clear();
            cblType4.Clear();

            cblSpecial.Clear();
            cblSpecialExcept.Clear();

            /*TextBox*/
            teEditor1.Text = "";
            teEditor2.Text = "";
            //teEditor3_1.Text = "";
            //teEditor3_2.Text = "";
            //teEditor4_1.Text = "";
            //teEditor4_2.Text = "";
            teEditor5.Text = "";

            IsSetting = false;
        }

        ///// <summary>
        ///// 選取全部選項
        ///// </summary>
        //public void SelectAll()
        //{
        //    IsSetting = true;

        //    IsSetting = false;
        //}

        /// <summary>
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            tmp = Calculation.CheckValueNumber(tmp, cblData1, 1, false);
            tmp = Calculation.OddEvenNumber(tmp, cblData2, false);
            tmp = Calculation.PrimeNumber(tmp, cblData2, false);
            tmp = Calculation.DivThreeRemainder(tmp, cblData2, false);
            return tmp;
        }

        /// <summary>
        /// doubleclick清除文字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEditor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //txtEditor.Text = "";
        }
    }
}
