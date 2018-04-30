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
            OldText = new int[2] { 0, 27 };

            /*CheckBoxList*/
            cblData1.ItemsSource = DB.ZeroOneCombination(3, '大', '小').OrderByDescending(x => x.Code).ToList();
            cblData2.ItemsSource = DB.ZeroOneCombination(3, '奇', '偶').OrderByDescending(x => x.Code).ToList();
            cblData3.ItemsSource = DB.ZeroOneCombination(3, '质', '合').OrderByDescending(x => x.Code).ToList();

            //0123456789
            var data = DB.CreateContinueNumber().OrderBy(x => x.ID).ToList();
            cblType1.ItemsSource = data;
            cblType2.ItemsSource = data;
            cblType3.ItemsSource = data;

            //AC值
            cblAC.ItemsSource = DB.CreateContinueNumber(1, 3);

            //匹配過濾
            cblMatch.ItemsSource = DB.CreateOption(0, 3);

            //特別排除
            cblSpecialExcept.ItemsSource = DB.CreateOption(1, 6, new string[6] { "豹子", "組三", "組六", "不連", "二連", "三連" });

            //012路
            cbl012.ItemsSource = DB.CombinationNumber(3, 0, 2).OrderBy(x => x.Code).ToList();

            /*RadioButtonList*/
            rblSelect.ItemsSource = DB.CreateOption(1, 3, new string[3] { "出1膽", "出2膽", "出3膽" });
            rblSpecialCalc.ItemsSource = DB.CreateOption(1, 3, new string[3] { "南山雪算膽法", "黃金算膽法", "wpshh算膽法" });

            /*ComboBox*/
            var data2 = DB.CreateOption(1, 3, new string[3] { "百", "十", "個" });
            cbPosMatchNumber.ItemsSource = data2;
            cbPosMatchNumber2.ItemsSource = data2;
            cbEqual.ItemsSource = DB.CreateOption(1, 6, new string[6] { "大於", "小於", "等於", "大於等於", "小於等於", "不等於" });


            /*CheckBox*/

            /*DataGrid*/
            dgData1.ItemsSource = new List<Match>();
            dgData2.ItemsSource = new List<PosMatch>();

            /*設定預設值*/
            SetDefaultValue();
        }

        /// <summary>
        /// 設定預設值
        /// </summary>
        public void SetDefaultValue()
        {
            /*CheckBoxList*/
            cblData1.Clear();
            cblData2.Clear();
            cblData3.Clear();

            cblType1.Clear();
            cblType2.Clear();
            cblType3.Clear();

            cblMatch.Clear();
            cblSpecialExcept.Clear();
            cbl012.Clear();

            /*RadioButtonList*/
            rblSelect.SelectedValue = 1;
            rblSpecialCalc.SelectedValue = 1;

            /*ComboBox*/
            cbPosMatchNumber.SelectedValue = 1;
            cbPosMatchNumber2.SelectedValue = 1;
            cbEqual.SelectedValue = 1;

            /*CheckBox*/
            cbRemoveSum.IsChecked = false;
            CheckBox_Checked(cbRemoveSum, null);

            cbCheck1.IsChecked = false;
            CheckBox_Checked(cbCheck1, null);

            cbCheck2.IsChecked = false;
            CheckBox_Checked(cbCheck2, null);

            cbCheck3.IsChecked = false;
            CheckBox_Checked(cbCheck3, null);

            cbCheck4.IsChecked = false;
            CheckBox_Checked(cbCheck4, null);

            cbMatch.IsChecked = false;
            CheckBox_Checked(cbMatch, null);

            cbPosMatch.IsChecked = false;
            CheckBox_Checked(cbPosMatch, null);
        }

        /// <summary>
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            tmp = Calculation.CheckValueNumber(tmp, cblData1, 1, false);
            tmp = Calculation.OddEvenNumber(tmp, cblData2, false);
            tmp = Calculation.PrimeNumber(tmp, cblData3, false);
            tmp = Calculation.DivThreeRemainder(tmp, cbl012, false);
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

        int[] OldText;
        /// <summary>
        /// 數字遮罩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void te_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsFirstTime)
            {
                var te = (sender as System.Windows.Controls.TextBox);
                int index = (te.Name == "teRange1" ? 0 : 1);
                if (te.Text == "" || te.Text == null)
                    te.Text = "0";
                else
                {
                    int i = 0;
                    if (!int.TryParse(te.Text, out i))
                        te.Text = OldText[index].ToString();
                    else
                    {
                        te.Text = i.ToString();
                        OldText[index] = i;
                    }
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var cb = sender as System.Windows.Controls.CheckBox;
            if (cb != null)
            {
                bool ischeck = ((bool)cb.IsChecked);
                if (cb.Name == "cbCheck1")
                {
                    teCheckHundred.IsEnabled = teCheckTen.IsEnabled = teCheckUnit.IsEnabled = ischeck;
                }
                else if (cb.Name == "cbCheck2")
                {
                    teCheckHundred2.IsEnabled = teCheckTen2.IsEnabled = teCheckUnit2.IsEnabled = ischeck;
                }
                else if (cb.Name == "cbCheck3")
                {
                    if ((bool)cbCheck4.IsChecked)
                    {
                        System.Windows.MessageBox.Show("大底和復式不能同時選擇。");
                        return;
                    }
                    teCheckHundred3.IsEnabled = teCheckTen3.IsEnabled = teCheckUnit3.IsEnabled = ischeck;
                }
                else if (cb.Name == "cbCheck4")
                {
                    if ((bool)cbCheck3.IsChecked)
                    {
                        System.Windows.MessageBox.Show("大底和復式不能同時選擇。");
                        return;
                    }
                    btnIsGroup.IsEnabled = btnSelect.IsEnabled = teBottom.IsEnabled = ischeck;
                }
                else if (cb.Name == "cbRemoveSum")
                {
                    teSum.IsEnabled = teRange1.IsEnabled = teRange2.IsEnabled = ischeck;
                }
                else if (cb.Name == "cbMatch")
                {
                    cblMatch.IsEnabled = teMatch.IsEnabled = btnAdd1.IsEnabled = btnDelete1.IsEnabled = dgData1.IsEnabled = ischeck;
                }
                else if (cb.Name == "cbPosMatch")
                {
                    cbPosMatchNumber.IsEnabled = cbPosMatchNumber2.IsEnabled = btnAdd2.IsEnabled = cbEqual.IsEnabled =
                    btnDelete2.IsEnabled = dgData2.IsEnabled = ischeck;
                }
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            if (btn != null)
            {
                if (btn.Name == "btnAdd1")
                {

                }
                else if (btn.Name == "btnDelete1")
                {

                }
                else if (btn.Name == "btnAdd2")
                {
                    var tmp = new PosMatch() { Value1 = "1", Operator = "2", Value2 = "3" };
                    dgData2.Items.Add(tmp);
                }
                else if (btn.Name == "btnDelete2")
                {

                }
            }
        }
    }

    public class Match
    {
        private string _Value1;

        private string _Operator;

        private string _Value2;

        public string Value1
        {
            get
            {
                return _Value1;
            }
            set
            {
                _Value1 = value;
            }
        }

        public string Operator
        {
            get
            {
                return _Operator;
            }
            set
            {
                _Operator = value;
            }
        }

        public string Value2
        {
            get
            {
                return _Value2;
            }
            set
            {
                _Value2 = value;
            }
        }
    }

    public class PosMatch
    {
        private string _Value1;

        private string _Operator;

        private string _Value2;

        public string Value1
        {
            get
            {
                return _Value1;
            }
            set
            {
                _Value1 = value;
            }
        }

        public string Operator
        {
            get
            {
                return _Operator;
            }
            set
            {
                _Operator = value;
            }
        }

        public string Value2
        {
            get
            {
                return _Value2;
            }
            set
            {
                _Value2 = value;
            }
        }
    }
}
