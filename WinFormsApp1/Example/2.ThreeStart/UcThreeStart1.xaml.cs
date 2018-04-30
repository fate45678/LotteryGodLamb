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
            OldText = new int[3] { 0, 27, 0 };

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
            dgData2.ItemsSource = new List<Match>();

            /*設定預設值*/
            SetDefaultValue();
        }

        #region 外部呼叫事件
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
            #region group1-殺直選.垃圾復式.殺2碼.定位殺2碼.必出2碼.交集.公式.其他
            //殺直選
            tmp = Calculation.AssignNumber(tmp, teEditor1.Text, false);

            //垃圾復式-邏輯待確定

            //殺兩碼
            tmp = Calculation.ExistsNumber(tmp, teEditor3.Text, 2, false);

            //定位殺兩碼

            //必出兩碼
            tmp = Calculation.ExistsNumber(tmp, teEditor5.Text, 2, true);

            //交集

            //公式

            //其他
            #endregion

            #region
            //殺和尾
            tmp = Calculation.SumLastNumber(tmp, cblType1, false);

            //殺跨度
            tmp = Calculation.CrossNumber(tmp, cblType2, false);

            //選膽
            #endregion

            #region 和值
            //殺指定和值
            tmp = Calculation.SumNumber(tmp, teSum.Text, false);

            //和值範圍
            int rangeStart = 0;
            int rangeEnd = 0;
            int.TryParse(teRange1.Text, out rangeStart);
            int.TryParse(teRange2.Text, out rangeEnd);
            var range = DB.CreateContinueNumber(rangeStart, rangeEnd);
            tmp = Calculation.SumNumber(tmp, string.Join(" ", range.Select(x => x.Code)), true);
            #endregion

            #region 排除復式/定位殺
            //排除復式

            //定位殺
            if ((bool)cbCheck2.IsChecked)
            {
                tmp = Calculation.PosNumber(tmp, teCheckHundred2.Text, "0", false);
                tmp = Calculation.PosNumber(tmp, teCheckTen2.Text, "1", false);
                tmp = Calculation.PosNumber(tmp, teCheckUnit2.Text, "2", false);
            }

            //AC值
            #endregion

            #region 大小
            tmp = Calculation.CheckValueNumber(tmp, cblData1, 1, true);
            #endregion

            #region 奇偶
            tmp = Calculation.OddEvenNumber(tmp, cblData2, true);
            #endregion

            #region 質合
            tmp = Calculation.PrimeNumber(tmp, cblData3, true);
            #endregion

            #region 012路特別排除
            tmp = Calculation.DivThreeRemainder(tmp, cbl012, false);
            #endregion

            #region 大底 / 復式
            if ((bool)cbCheck4.IsChecked)
                tmp = Calculation.AssignNumber(tmp, teBottom.Text, true);
            #endregion

            return tmp;
        }
        #endregion

        #region 內部使用事件
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
                int index = 0;

                if (te.Name == "teRange1")
                    index = 0;
                else if (te.Name == "teRange2")
                    index = 1;
                else if (te.Name == "teMatch")
                    index = 2;

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

        bool IsSetting = false;

        /// <summary>
        /// CheckBox事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (IsSetting)
                return;

            var cb = sender as System.Windows.Controls.CheckBox;
            if (cb != null)
            {
                IsSetting = true;
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
                        cbCheck3.IsChecked = !ischeck;
                        System.Windows.MessageBox.Show("大底和復式不能同時選擇。");
                    }
                    else
                        teCheckHundred3.IsEnabled = teCheckTen3.IsEnabled = teCheckUnit3.IsEnabled = ischeck;
                }
                else if (cb.Name == "cbCheck4")
                {
                    if ((bool)cbCheck3.IsChecked)
                    {
                        cbCheck4.IsChecked = !ischeck;
                        System.Windows.MessageBox.Show("大底和復式不能同時選擇。");
                    }
                    else
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
                IsSetting = false;
            }
        }

        /// <summary>
        /// Button事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            if (btn != null)
            {
                if (btn.Name == "btnAdd1")
                {
                    if (!string.IsNullOrEmpty(teMatch.Text))
                    {
                        var tmp = dgData1.ItemsSource.Cast<Match>().ToList();
                        tmp.Add(new Match { Value1 = teMatch.Text, Value2 = GetCheckBoxDisplayName(cblMatch), Operator = "" });

                        dgData1.ItemsSource = tmp;
                    }
                }
                else if (btn.Name == "btnDelete1")
                {
                    var tmp = dgData1.ItemsSource.Cast<Match>().ToList();
                    tmp.Remove(dgData1.SelectedItem as Match);
                    dgData1.ItemsSource = tmp;
                }
                else if (btn.Name == "btnAdd2")
                {
                    var tmp = dgData2.ItemsSource.Cast<Match>().ToList();
                    tmp.Add(new Match
                    {
                        Value1 = GetComboBoxDisplayName(cbPosMatchNumber),
                        Operator = GetComboBoxDisplayName(cbEqual),
                        Value2 = GetComboBoxDisplayName(cbPosMatchNumber2)
                    });

                    dgData2.ItemsSource = tmp;
                }
                else if (btn.Name == "btnDelete2")
                {
                    var tmp = dgData2.ItemsSource.Cast<Match>().ToList();
                    tmp.Remove(dgData2.SelectedItem as Match);
                    dgData2.ItemsSource = tmp;
                }
                else if (btn.Name == "btnSelect")
                {
                    /*開啟檔案並讀取*/
                    System.Windows.Controls.TextBox te = teBottom;
                    System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

                    // 設定Filter，指定只能開啟特定的檔案 

                    openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                    // 選擇圖片的Filter

                    // openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";

                    var tmp = openFileDialog.ShowDialog();
                    if (tmp.ToString() == "OK")
                    {
                        //檢核是否存在檔案
                        if (File.Exists(openFileDialog.FileName))
                        {
                            //檢核檔名
                            if (!string.IsNullOrEmpty(openFileDialog.FileName))
                            {
                                if (openFileDialog.FileName.Length >= 3 &&
                                    openFileDialog.FileName.Substring(openFileDialog.FileName.Length - 3, 3) == "txt")
                                    te.Text = File.ReadAllText(openFileDialog.FileName);
                                else
                                    System.Windows.Forms.MessageBox.Show("非文字檔無法開啟。");
                            }
                            else
                                te.Text = "";
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("檔案不存在，請確認後再選取。");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 取得ComboBox顯示名稱
        /// </summary>
        /// <param name="cb"></param>
        /// <returns></returns>
        string GetComboBoxDisplayName(System.Windows.Controls.ComboBox cb)
        {
            if (cb != null)
            {
                if (cb.SelectedValue != null)
                {
                    var tmp = cb.ItemsSource.Cast<BaseOptions>().Where(x => (int)cb.SelectedValue == x.ID).FirstOrDefault();
                    if (tmp != null)
                        return tmp.Name;
                }
            }
            return "";
        }

        /// <summary>
        /// 取得CheckBox顯示名稱
        /// </summary>
        /// <param name="cb"></param>
        /// <returns></returns>
        string GetCheckBoxDisplayName(CheckBoxList cb)
        {
            if (!string.IsNullOrEmpty(cb.SelectedValue))
            {
                var tmp = cb.ItemsSource.Cast<BaseOptions>().Where(x => cb.SelectedValue.ToString()[x.ID - 1] == '1');
                if (tmp.Count() > 0)
                    return string.Join("", tmp.Select(x => x.Code));
            }
            return "";
        }

        #endregion
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
}
