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
using System.Text.RegularExpressions;

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
            //cblAC.ItemsSource = DB.CreateContinueNumber(1, 3);

            //匹配過濾
            cblMatch.ItemsSource = DB.CreateOption(0, 3);

            //特別排除
            cblSpecialExcept.ItemsSource = DB.CreateOption(1, 6, new string[6] { "豹子", "組三", "組六", "不连", "二连", "三连" });

            //012路
            cbl012.ItemsSource = DB.CombinationNumber(3, 0, 2).OrderBy(x => x.Code).ToList();

            /*RadioButtonList*/
            rblSelect.ItemsSource = DB.CreateOption(1, 3, new string[3] { "出1胆", "出2胆", "出3胆" });
            //rblSpecialCalc.ItemsSource = DB.CreateOption(1, 3, new string[3] { "南山雪算胆法", "黄金算胆法", "wpshh算胆法" });

            /*ComboBox*/
            var data2 = DB.CreateOption(1, 3, new string[3] { "百", "十", "个" });
            cbPosMatchNumber.ItemsSource = data2;
            cbPosMatchNumber2.ItemsSource = data2;
            cbEqual.ItemsSource = DB.CreateOption(1, 6, new string[6] { "大于", "小于", "等于", "大于等于", "小于等于", "不等于" });
            cbRow.ItemsSource = DB.CreateOption(1, 20);

            /*CheckBox*/

            /*DataGrid*/
            dgData1.ItemsSource = new List<Match>();
            dgData2.ItemsSource = new List<Match>();

            //定義寬度
            cblData1.WrapRow(4);
            cblData2.WrapRow(4);
            cblData3.WrapRow(4);
            cbl012.WrapRow(3);

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
            cblData1.SelectedAll();
            cblData2.SelectedAll();
            cblData3.SelectedAll();

            cblType1.Clear();
            cblType2.Clear();
            cblType3.Clear();

            cblMatch.Clear();
            cblSpecialExcept.Clear();
            cbl012.Clear();

            /*RadioButtonList*/
            rblSelect.SelectedValue = 1;
            //rblSpecialCalc.SelectedValue = 1;

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

            cbIsGroup.IsChecked = false;

            /*DataGrid*/
            dgData1.ItemsSource = new List<Match>();
            dgData2.ItemsSource = new List<Match>();

            /*TextBox*/
            teEditor1.Text = teEditor2.Text = teEditor3.Text = teEditor4.Text =
            teEditor5.Text = teEditor6.Text = //teEditor7.Text = teEditor8.Text =
            teMatch.Text = teBottom.Text = teSum.Text =
            teCheckHundred.Text = teCheckHundred2.Text = teCheckHundred3.Text =
            teCheckTen.Text = teCheckTen2.Text = teCheckTen3.Text =
            teCheckUnit.Text = teCheckUnit2.Text = teCheckUnit3.Text = "";
            teRange1.Text = "0";
            teRange2.Text = "27";

            /*ComboBox*/
            cbRow.SelectedValue = 8;
        }

        /// <summary>
        /// 過濾前檢核
        /// </summary>
        /// <returns></returns>
        public bool BeforeCheck()
        {
            string check = cblType3.SelectedValue.Replace("0", "");
            if (check.Count() > 0)
            {
                if (check.Count() < (int)rblSelect.SelectedValue)
                {
                    System.Windows.MessageBox.Show("出胆个数与选择不符，请重新选择。");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            //大底
            #region 大底 / 複式
            //複式
            if ((bool)cbCheck3.IsChecked)
            {
                string condition = teCheckHundred3.Text + "/" + teCheckTen3.Text + "/" + teCheckUnit3.Text;
                tmp = Calculation.CompoundNumber(tmp, condition, '/', 3, true);
            }

            //大底
            if ((bool)cbCheck4.IsChecked && !(bool)cbIsGroup.IsChecked)
                tmp = Calculation.AssignNumber(tmp, teBottom.Text, true);
            #endregion

            //公式
            //其他

            #region group1-殺直選.垃圾複式.殺2碼.定位殺2碼.必出2碼.交集.公式.其他
            //殺直選
            tmp = Calculation.AssignNumber(tmp, teEditor1.Text, false);

            //垃圾複式
            tmp = Calculation.GarbageNumber(tmp, teEditor2.Text, 2, '*', 3);

            //殺兩碼
            tmp = Calculation.ExistsNumber(tmp, teEditor3.Text, 2, true, false);

            //定位殺兩碼
            tmp = Calculation.PosNumber(tmp, teEditor4.Text, 2, false);

            //必出兩碼
            tmp = Calculation.ExistsNumber(tmp, teEditor5.Text, 2, true, true);

            //交集
            if (!string.IsNullOrEmpty(teEditor6.Text))
            {
                var inter = Regex.Replace(Regex.Replace(teEditor6.Text, "\n", " "), "[^0-9|\\s]", "").Split(' ').Except(new string[1] { "" }).Where(x => x.Length == 3);
                if (inter.Count() > 0)
                    tmp = tmp.Where(x => inter.Contains(x.Code)).ToList();
            }
            //公式

            //其他
            #endregion

            #region group2-和 跨 膽
            //殺和尾
            tmp = Calculation.SumLastNumber(tmp, cblType1, false);

            //殺跨度
            tmp = Calculation.CrossNumber(tmp, cblType2, false);

            //選膽
            string conditions = Calculation.BeforeCheck(tmp, cblType3);
            conditions = conditions.Replace(",", " ");
            var conArray = conditions.Split(' ').Except(new string[1] { "" });
            int unit = (int)rblSelect.SelectedValue;
            if (unit >= 2 && conArray.Count() >= unit)
            {
                WpfAppTest.AP.DB.test tmps = WpfAppTest.AP.DB.CombinationNNumber("", conditions.Split(' ').ToArray(), unit, new WpfAppTest.AP.DB.test());
                conditions = string.Join(" ", tmps.result2.Split(' ').Where(x => x.Distinct().Count() == unit));
            }
            tmp = Calculation.ExistsNumber(tmp, conditions, (int)rblSelect.SelectedValue, true, true);
            #endregion

            #region group3-和值
            if ((bool)cbRemoveSum.IsChecked)
            {
                //殺指定和值
                tmp = Calculation.SumNumber(tmp, teSum.Text, false);

                //和值範圍
                int rangeStart = 0;
                int rangeEnd = 0;
                int.TryParse(teRange1.Text, out rangeStart);
                int.TryParse(teRange2.Text, out rangeEnd);
                var range = DB.CreateContinueNumber(rangeStart, rangeEnd);
                tmp = Calculation.SumNumber(tmp, string.Join(" ", range.Select(x => x.Code)), true);
            }
            #endregion

            #region group4-排除複式/定位殺
            //排除複式
            if ((bool)cbCheck1.IsChecked)
            {
                string condition = teCheckHundred.Text + "/" + teCheckTen.Text + "/" + teCheckUnit.Text;
                tmp = Calculation.CompoundNumber(tmp, condition, '/', 3);
            }

            //定位殺
            if ((bool)cbCheck2.IsChecked)
            {
                tmp = Calculation.PosNumber(tmp, teCheckHundred2.Text, "0", false);
                tmp = Calculation.PosNumber(tmp, teCheckTen2.Text, "1", false);
                tmp = Calculation.PosNumber(tmp, teCheckUnit2.Text, "2", false);
            }

            //AC值
            #endregion

            #region group5-大小
            tmp = Calculation.CheckValueNumber(tmp, cblData1, 1, true);
            #endregion

            #region group6-奇偶
            tmp = Calculation.OddEvenNumber(tmp, cblData2, true);
            #endregion

            #region group7-質合
            tmp = Calculation.PrimeNumber(tmp, cblData3, true);
            #endregion

            #region group8-012路特別排除
            tmp = Calculation.DivThreeRemainder(tmp, cbl012, false);
            #endregion

            #region group9-位置大小匹配
            if ((bool)cbPosMatch.IsChecked)
                tmp = Calculation.ThreeStartMatch(tmp, dgData2.ItemsSource.Cast<Match>().ToList());
            #endregion

            #region group10-特別排除
            tmp = Calculation.ThreeSpecialData(tmp, cblSpecialExcept.SelectedValue);
            #endregion

            return tmp;
        }

        /// <summary>
        /// 更新大底
        /// </summary>
        /// <param name="text"></param>
        public void RefreshBottom(string text)
        {
            teBottom.Text = text;
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
            var te = sender as System.Windows.Controls.TextBox;
            if (te != null)
                te.Text = "";
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
                    if ((bool)cbCheck4.IsChecked && ischeck)
                    {
                        cbCheck3.IsChecked = !ischeck;
                        System.Windows.MessageBox.Show("大底和复式不能同时选择。");
                    }
                    else
                        teCheckHundred3.IsEnabled = teCheckTen3.IsEnabled = teCheckUnit3.IsEnabled = ischeck;
                }
                else if (cb.Name == "cbCheck4")
                {
                    if ((bool)cbCheck3.IsChecked && ischeck)
                    {
                        cbCheck4.IsChecked = !ischeck;
                        System.Windows.MessageBox.Show("大底和复式不能同时选择。");
                    }
                    else
                        cbIsGroup.IsEnabled = btnSelect.IsEnabled = teBottom.IsEnabled = ischeck;
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
                        tmp.Add(new Match
                        {
                            ValueName1 = teMatch.Text,
                            ValueName2 = GetCheckBoxDisplayName(cblMatch),
                            OperatorName = "",
                            Value1 = 0,
                            Value2 = 0,
                            Operator = 0
                        });

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
                        ValueName1 = GetComboBoxDisplayName(cbPosMatchNumber),
                        ValueName2 = GetComboBoxDisplayName(cbPosMatchNumber2),
                        OperatorName = GetComboBoxDisplayName(cbEqual),
                        Value1 = (int)cbPosMatchNumber.SelectedValue,
                        Value2 = (int)cbPosMatchNumber2.SelectedValue,
                        Operator = (int)cbEqual.SelectedValue
                    });

                    dgData2.ItemsSource = tmp;
                }
                else if (btn.Name == "btnDelete2")
                {
                    var tmp = dgData2.ItemsSource.Cast<Match>().ToList();
                    tmp.Remove(dgData2.SelectedItem as Match);
                    dgData2.ItemsSource = tmp;
                }
                else if (btn.Name == "btnExport")
                {
                    /*匯出結果*/
                    FolderBrowserDialog path = new FolderBrowserDialog();
                    if (path.ShowDialog() == DialogResult.OK)
                    {
                        string exportPath = @"" + path.SelectedPath + @"\三星号码导出.txt";
                        FileStream fs = new FileStream(exportPath, FileMode.Create, FileAccess.ReadWrite);
                        StreamWriter sw = new StreamWriter(fs, Encoding.Default);

                        var array = Regex.Replace(teBottom.Text, "[^0-9|\\s]", "").Split(' ').Except(new string[1] { "" }).ToArray();
                        //切割行列, n幾行
                        int n = (array.Count() / (int)cbRow.SelectedValue) + ((array.Count() % (int)cbRow.SelectedValue) > 0 ? 1 : 0);
                        string text = "";
                        for (int i = 1; i <= array.Count(); i++)
                        {
                            text = text + array[i - 1] + " ";
                            if (i % n == 0)
                                text += "\r\n";
                        }
                        sw.Write(text);
                        sw.Close();
                        fs.Close();
                        System.Windows.Forms.MessageBox.Show("号码导出成功。");
                    }
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
                                {
                                    string text = File.ReadAllText(openFileDialog.FileName);
                                    te.Text = string.Join("    ", text.Split(' ').Except(new string[1] { " " }));
                                }
                                else
                                    System.Windows.Forms.MessageBox.Show("非文本文件无法开启。");
                            }
                            else
                                te.Text = "";
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("档案不存在，请确认后再选取。");
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
        private int _Value1;

        private int _Value2;

        private int _Operator;

        private string _ValueName1;

        private string _ValueName2;

        private string _OperatorName;

        public int Value1
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

        public int Value2
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

        public int Operator
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

        public string ValueName1
        {
            get
            {
                return _ValueName1;
            }
            set
            {
                _ValueName1 = value;
            }
        }

        public string ValueName2
        {
            get
            {
                return _ValueName2;
            }
            set
            {
                _ValueName2 = value;
            }
        }

        public string OperatorName
        {
            get
            {
                return _OperatorName;
            }
            set
            {
                _OperatorName = value;
            }
        }
    }
}
