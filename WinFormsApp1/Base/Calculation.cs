using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.Windows;
using System.Collections;
using System.Windows.Data;
using System.Windows.Controls;
using System.ComponentModel;

namespace WpfAppTest.Base
{
    public static class Calculation
    {
        static string[] emptyArray = new string[1] { "" };

        #region 通用判斷
        /// <summary>
        /// 判斷前組合
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbl"></param>
        /// <param name="type">Type: 1.用code 2.用id</param>
        /// <returns></returns>
        public static string BeforeCheck(List<BaseOptions> data, CheckBoxList cbl, int type = 1)
        {
            if (data == null || data.Count() == 0 ||
                cbl == null || cbl.SelectedValue == "" ||
                cbl.SelectedValue == "".PadRight(cbl.SelectedValue.Length, '0'))
                return "";

            var tmp = cbl.ItemsSource.Cast<BaseOptions>().ToList();
            if (tmp == null)
                return "";

            string condition = "";
            for (int i = 0; i < cbl.SelectedValue.Count(); i++)
            {
                if (cbl.SelectedValue[i] == '1')
                    condition += (condition == "" ? "" : " ") + string.Join(" ", tmp.Where(x => x.ID == i + 1).Select(x => x.Code));
            }
            return condition;
        }
        #endregion

        #region 通用

        #region 轉為組選
        /// <summary>
        /// 轉為組選
        /// </summary>
        /// <param name="data">全部組合資料</param>
        public static List<BaseOptions> TransNumber(List<BaseOptions> data)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0)
            {
                int number = data.FirstOrDefault().Code.Length;
                for (int i = 0; i < data.Count; i++)
                {
                    if (tmpData.Where(x => x.ID == data[i].ID).Count() > 0)
                    {
                        var tmp = ExistsNumber(tmpData, data[i].Code, number, true, true).Where(x => x.Code != data[i].Code).ToList();
                        tmpData = tmpData.Except(tmp).ToList();
                    }
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }
        #endregion

        #region 定位 or 定位殺 or 組合

        /// <summary>
        /// 複式: 定位(殺) or 組合(殺)
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="condition"></param>
        /// <param name="number">定n位:n=-1 表示為組合, 不限定個數; n>0, 則n表示定n位</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> PosNumber(List<BaseOptions> data, string condition, int number, bool isKeep)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.Select(x => new BaseOptions { ID = x.ID, Name = x.Name, Code = x.Code, Check = false }).ToList());
            if (tmpData != null && tmpData.Count > 0 && !string.IsNullOrEmpty(condition))
            {
                condition = condition.Replace(",", " ").Replace("\r\n", " ");
                var conArray = condition.Split(' ').Where(x => (number == -1) || (number > -1 && x.Length - x.Replace("*", "").Length == x.Length - number)).ToArray();
                if (conArray.Count() > 0)
                {
                    foreach (var item in tmpData)
                    {
                        foreach (var n in conArray)
                        {
                            bool isok = true;
                            for (int i = 0; i < n.Length; i++)
                            {
                                if (n[i] == '*' || !isok)
                                    continue;

                                if (item.Code[i] != n[i])
                                    isok = false;
                            }
                            if (!item.Check && isok)
                                item.Check = true;
                        }
                    }
                    tmpData = tmpData.Where(x => (isKeep && x.Check) || (!isKeep && !x.Check)).ToList();
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }

        /// <summary>
        /// 單式: 定位 or 定位殺
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="condition"></param>
        /// <param name="pos">定位的index</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> PosNumber(List<BaseOptions> data, string condition, string pos, bool isKeep)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && !string.IsNullOrEmpty(condition))
            {
                var conArray = condition.Split(' ');
                int posindex = 0;
                bool isexists = false;

                //跑全部資料
                foreach (var item in data)
                {
                    //結果判斷
                    if (int.TryParse(pos, out posindex))
                    {
                        if (conArray.Contains(item.Code[posindex].ToString()))
                            isexists = true;
                        else
                            isexists = false;
                    }
                    else
                    {
                        foreach (var c in item.Code)
                        {
                            if (conArray.Contains(c.ToString()))
                            {
                                isexists = true;
                                break;
                            }
                            else
                                isexists = false;
                        }
                    }

                    if (isexists)
                    {
                        if (!isKeep)
                            tmpData.Remove(item);
                    }
                    else
                    {
                        if (isKeep)
                            tmpData.Remove(item);
                    }
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }

        /// <summary>
        /// 定位(殺) or 組合(殺)-for CheckBoxList使用
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="cbl"></param>
        /// <param name="pos">定位的index</param>
        /// <param name="isKeep">是否保留 true定位 false定位殺</param>
        public static List<BaseOptions> PosNumber(List<BaseOptions> data, CheckBoxList cbl, string pos, bool isKeep = true)
        {
            string condition = BeforeCheck(data, cbl);
            return PosNumber(data, condition, pos, isKeep);
        }
        #endregion

        #region 直選/殺直選
        /// <summary>
        /// 直選/殺直選
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="condition"></param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> AssignNumber(List<BaseOptions> data, string condition, bool isKeep)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && !string.IsNullOrEmpty(condition))
            {
                condition = condition.Replace(",", " ");
                var conArray = condition.Split(' ');

                //不保留
                if (!isKeep)
                    tmpData = tmpData.Where(x => !conArray.Contains(x.Code)).ToList();
                else
                    tmpData = tmpData.Where(x => conArray.Contains(x.Code)).ToList();
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }
        #endregion

        #region 任n碼
        /// <summary>
        /// 任n碼
        /// 組號裡出現某些數字,任n碼
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="condition"></param>
        /// <param name="n">出n碼</param>
        /// <param name="checkUnit">是否參考個數</param>
        /// <param name="isKeep">是否保留 true出n碼 false殺n碼</param>
        public static List<BaseOptions> ExistsNumber(List<BaseOptions> data, string condition, int n, bool checkUnit, bool isKeep)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && !string.IsNullOrEmpty(condition))
            {
                condition = condition.Replace(",", " ");
                var conArray = condition.Split(' ');
                conArray = conArray.Where(x => x.ToString().Length == n).ToArray();

                if (conArray.Count() > 0)
                {
                    bool match = true;
                    foreach (var item in tmpData)
                    {
                        item.Check = false;
                        foreach (var number in conArray)
                        {
                            match = true;

                            //暫存code
                            string tmpCode = item.Code;
                            int index = -1;
                            foreach (var c in number)
                            {
                                if (checkUnit)
                                {
                                    if (tmpCode.Contains(c.ToString()))
                                    {
                                        index = tmpCode.IndexOf(c);
                                        if (index > -1)
                                            tmpCode = tmpCode.Remove(index, 1);
                                        else
                                            match = false;
                                    }
                                    else
                                        match = false;
                                }
                                else
                                {
                                    if (!tmpCode.Contains(c.ToString()))
                                        match = false;
                                }

                                if (!match)
                                    break;
                            }

                            if (match)
                            {
                                item.Check = true;
                                break;
                            }
                        }
                    }
                    tmpData = tmpData.Where(x => (isKeep && x.Check) || (!isKeep && !x.Check)).ToList();
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }

        /// <summary>
        /// 任n碼
        /// 組號裡出現某些數字,任n碼
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="cbl"></param>
        /// <param name="n">出n碼</param>
        /// <param name="checkUnit">是否參考個數</param>
        /// <param name="isKeep">是否保留 true出n碼 false殺n碼</param>
        public static List<BaseOptions> ExistsNumber2(List<BaseOptions> data, CheckBoxList cbl, CheckBoxList cbl2, int[] n, bool checkUnit)
        {
            if ((cbl2 != null && n != null) || (cbl2 == null && n == null) || data == null || data.Count() == 0)
                return data;

            string condition = Calculation.BeforeCheck(data, cbl);
            string condition2 = Calculation.BeforeCheck(data, cbl2);

            if (!string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(condition2))
            {
                List<BaseOptions> tmpData = new List<BaseOptions>(); ;
                if (cbl2 != null)
                    n = condition2.Split(' ').Select(x => int.Parse(x.ToString())).ToArray();

                if (n == null || n.Count() == 0)
                    return data;

                var conArray = condition.Split(' ').Except(emptyArray).ToArray();
                foreach (var s in n)
                {
                    List<BaseOptions> tmp;
                    if (!checkUnit)
                    {
                        tmp = data.Where(x => x.Code.Select(y => y.ToString()).ToArray().Intersect(conArray).Count() == s).ToList();
                        tmpData.AddRange(tmp);
                        data = data.Except(tmp).ToList();
                    }
                    else
                    {
                        foreach (var item in data)
                        {
                            string code = item.Code;
                            foreach (var c in conArray)
                                code = code.Replace(c, "");
                            if (item.Code.Length - code.Length == s)
                                tmpData.Add(item);
                        }
                    }
                }
                return (tmpData.OrderBy(x => x.Code).ToList());
            }
            return data;
        }


        /// <summary>
        /// 任n碼-for CheckBoxList使用
        /// 組號裡出現某些數字,任n碼
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="condition"></param>
        /// <param name="n">出n碼</param>
        /// <param name="checkUnit">是否參考個數</param>
        /// <param name="isKeep">是否保留 true出n碼 false殺n碼</param>
        public static List<BaseOptions> ExistsNumber(List<BaseOptions> data, CheckBoxList cbl, int n, bool checkUnit, bool isKeep = true)
        {
            string condition = BeforeCheck(data, cbl);
            return ExistsNumber(data, condition, n, checkUnit, isKeep);
        }
        #endregion

        #region 奇偶判斷
        /// <summary>
        /// 奇偶數判斷-for CheckBoxList使用
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="condition"></param>
        /// <param name="isKeep">是否保留</param>
        private static List<BaseOptions> OddEvenNumber(List<BaseOptions> data, string condition, bool isKeep = true)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && !string.IsNullOrEmpty(condition))
            {
                var conArray = condition.Split(' ');
                int number = 0;
                string checkvalue = "";

                //跑全部資料
                foreach (var item in data)
                {
                    checkvalue = "";
                    //每一筆資料的數字
                    foreach (var c in item.Code)
                    {
                        int.TryParse(c.ToString(), out number);
                        checkvalue += (number % 2).ToString();
                    }

                    //結果判斷
                    if (conArray.Contains(checkvalue))
                    {
                        if (!isKeep)
                            tmpData.Remove(item);
                    }
                    else
                    {
                        if (isKeep)
                            tmpData.Remove(item);
                    }
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }

        /// <summary>
        /// 奇偶數判斷-for CheckBoxList使用
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="cbl">CheckBoxList</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> OddEvenNumber(List<BaseOptions> data, CheckBoxList cbl, bool isKeep = true)
        {
            string condition = BeforeCheck(data, cbl);
            return OddEvenNumber(data, condition, isKeep);
        }
        #endregion

        #region 大中小判斷
        /// <summary>
        /// 大中小判斷
        /// 大0 中1 小2 ; 大0 小1
        /// </summary>
        /// <param name="type">type=1 : 大中小判斷 type=2:大小判斷</param>
        private static List<BaseOptions> CheckValueNumber(List<BaseOptions> data, string condition, int type, bool isKeep = true)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && !string.IsNullOrEmpty(condition))
            {
                var conArray = condition.Split(' ');
                int number = 0;
                string checkvalue = "";
                double lenth = (double)9 / (type == 1 ? 2 : 3);

                //跑全部資料
                foreach (var item in data)
                {
                    checkvalue = "";
                    //每一筆資料的數字
                    foreach (var c in item.Code)
                    {
                        int.TryParse(c.ToString(), out number);
                        if (type == 1)
                            checkvalue += ((int)(number / lenth) - (number > 0 && number % lenth == 0 ? 1 : 0)).ToString();
                        else
                            checkvalue += ((c == '6' || c == '9' ? -1 : 0) + (int)(number / lenth)).ToString();
                    }

                    //結果判斷
                    if (conArray.Contains(checkvalue))
                    {
                        if (!isKeep)
                            tmpData.Remove(item);
                    }
                    else
                    {
                        if (isKeep)
                            tmpData.Remove(item);
                    }
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }

        /// <summary>
        /// 大中小判斷 for CheckBoxList使用
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="cbl">CheckBoxList</param>
        /// <param name="type">Type=1:判斷大小 Type=2:判斷大中小</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> CheckValueNumber(List<BaseOptions> data, CheckBoxList cbl, int type, bool isKeep = true)
        {
            string condition = BeforeCheck(data, cbl);
            return CheckValueNumber(data, condition, type, isKeep);
        }
        #endregion

        #region 012路判斷 or 除三餘數
        /// <summary>
        /// 012路判斷 or 除三餘數
        /// </summary>
        private static List<BaseOptions> DivThreeRemainder(List<BaseOptions> data, string condition, bool isKeep = true)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && !string.IsNullOrEmpty(condition))
            {
                var conArray = condition.Split(' ');
                int number = 0;
                string checkvalue = "";

                //跑全部資料
                foreach (var item in data)
                {
                    checkvalue = "";
                    //每一筆資料的數字
                    foreach (var c in item.Code)
                    {
                        int.TryParse(c.ToString(), out number);
                        checkvalue += (number % 3).ToString();
                    }

                    //結果判斷
                    if (conArray.Contains(checkvalue))
                    {
                        if (!isKeep)
                            tmpData.Remove(item);
                    }
                    else
                    {
                        if (isKeep)
                            tmpData.Remove(item);
                    }
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }

        /// <summary>
        /// 012路判斷 or 除三餘數 for CheckBoxList使用
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="cbl">CheckBoxList</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> DivThreeRemainder(List<BaseOptions> data, CheckBoxList cbl, bool isKeep = true)
        {
            string condition = BeforeCheck(data, cbl);
            return DivThreeRemainder(data, condition, isKeep);
        }
        #endregion

        #region 質合判斷
        /// <summary>
        /// 質合判斷
        /// </summary>
        private static List<BaseOptions> PrimeNumber(List<BaseOptions> data, string condition, bool isKeep = true)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && !string.IsNullOrEmpty(condition))
            {
                var conArray = condition.Split(' ');
                string checkvalue = "";
                string[] prime = new string[5] { "1", "2", "3", "5", "7" };

                //跑全部資料
                foreach (var item in data)
                {
                    checkvalue = "";
                    //每一筆資料的數字
                    foreach (var c in item.Code)
                    {
                        checkvalue += (prime.Contains(c.ToString()) ? "1" : "0");
                    }

                    //結果判斷
                    if (conArray.Contains(checkvalue))
                    {
                        if (!isKeep)
                            tmpData.Remove(item);
                    }
                    else
                    {
                        if (isKeep)
                            tmpData.Remove(item);
                    }
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }

        /// <summary>
        /// 质合判斷 for CheckBoxList使用
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="cbl">CheckBoxList</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> PrimeNumber(List<BaseOptions> data, CheckBoxList cbl, bool isKeep = true)
        {
            string condition = BeforeCheck(data, cbl);
            return PrimeNumber(data, condition, isKeep);
        }
        #endregion

        #region 和值判斷
        /// <summary>
        /// 和值判斷
        /// </summary>
        public static List<BaseOptions> SumNumber(List<BaseOptions> data, string condition, bool isKeep = true)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && !string.IsNullOrEmpty(condition))
            {
                var conArray = condition.Split(' ');
                int number = 0;

                //跑全部資料
                foreach (var item in data)
                {
                    number = 0;
                    //每一筆資料的數字
                    foreach (var c in item.Code)
                    {
                        number += int.Parse(c.ToString());
                    }

                    //結果判斷
                    if (conArray.Contains(number.ToString()))
                    {
                        if (!isKeep)
                            tmpData.Remove(item);
                    }
                    else
                    {
                        if (isKeep)
                            tmpData.Remove(item);
                    }
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }

        /// <summary>
        /// 和值判斷 for CheckBoxList使用
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="cbl">CheckBoxList</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> SumNumber(List<BaseOptions> data, CheckBoxList cbl, bool isKeep = true)
        {
            string condition = BeforeCheck(data, cbl);
            return SumNumber(data, condition, isKeep);
        }
        #endregion

        #region 和尾判斷
        /// <summary>
        /// 和尾判斷
        /// </summary>
        private static List<BaseOptions> SumLastNumber(List<BaseOptions> data, string condition, bool isKeep = true)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && !string.IsNullOrEmpty(condition))
            {
                var conArray = condition.Split(' ');
                int number = 0;

                //跑全部資料
                foreach (var item in data)
                {
                    number = 0;
                    //每一筆資料的數字
                    foreach (var c in item.Code)
                    {
                        number += int.Parse(c.ToString());
                    }

                    //結果判斷
                    if (conArray.Contains(number.ToString().Substring(number.ToString().Length - 1)))
                    {
                        if (!isKeep)
                            tmpData.Remove(item);
                    }
                    else
                    {
                        if (isKeep)
                            tmpData.Remove(item);
                    }
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }

        /// <summary>
        /// 和尾判斷 for CheckBoxList使用
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="cbl">CheckBoxList</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> SumLastNumber(List<BaseOptions> data, CheckBoxList cbl, bool isKeep = true)
        {
            string condition = BeforeCheck(data, cbl);
            return SumLastNumber(data, condition, isKeep);
        }
        #endregion

        #region 跨度判斷
        /// <summary>
        /// 跨度判斷
        /// </summary>
        private static List<BaseOptions> CrossNumber(List<BaseOptions> data, string condition, bool isKeep = true)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && !string.IsNullOrEmpty(condition))
            {
                var conArray = condition.Split(' ');
                char[] tmp;
                int number = 0;

                //跑全部資料
                foreach (var item in data)
                {
                    tmp = item.Code.ToArray();
                    number = int.Parse(tmp.Max().ToString()) - int.Parse(tmp.Min().ToString());

                    //結果判斷
                    if (conArray.Contains(number.ToString()))
                    {
                        if (!isKeep)
                            tmpData.Remove(item);
                    }
                    else
                    {
                        if (isKeep)
                            tmpData.Remove(item);
                    }
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }

        /// <summary>
        /// 跨度判斷 for CheckBoxList使用
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="cbl">CheckBoxList</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> CrossNumber(List<BaseOptions> data, CheckBoxList cbl, bool isKeep = true)
        {
            string condition = BeforeCheck(data, cbl);
            return CrossNumber(data, condition, isKeep);
        }
        #endregion

        #region 垃圾複式/垃圾單式
        /// <summary>
        /// 垃圾複式/垃圾單式
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="condition">條件</param>
        /// <param name="mode">單式or複式 mode:1->單式 2->複式</param>
        /// <param name="split">單式複式的分格符號</param>
        /// <param name="number">幾星</param>
        public static List<BaseOptions> GarbageNumber(List<BaseOptions> data, string condition, int mode, char split = ',', int number = 0)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null)
            {
                if (mode == 1)
                {
                    //單式
                    var array = condition.Split(split).ToArray();
                    foreach (var tmp in array)
                    {
                        var item = tmpData.Where(x => x.Code == tmp.Replace(" ", "")).FirstOrDefault();
                        if (item != null)
                            tmpData.Remove(item);
                    }
                }
                else if (mode == 2)
                {
                    //複式
                    var array = condition.Split(new string[1] { "\r\n" }, StringSplitOptions.None).ToArray();
                    foreach (var tmp in array)
                    {
                        tmpData = CompoundNumber(tmpData, tmp, split, number, false);
                    }
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }
        #endregion

        #region 排除複式/輸入複式
        /// <summary>
        /// 排除複式/輸入複式
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="condition">條件</param>
        /// <param name="mode">單式or複式 mode:1->單式 2->複式</param>
        /// <param name="split">單式複式的分格符號</param>
        /// <param name="number">幾星</param>
        public static List<BaseOptions> CompoundNumber(List<BaseOptions> data, string condition, char split = ',', int number = 0, bool iskeep = false)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null)
            {
                //複式
                var s = condition.Split(split).ToArray();
                foreach (var item in data)
                {
                    if (number > 0 && s.Count() != number)
                        break;

                    bool isExists = true;
                    for (int i = 0; i < s.Count(); i++)
                    {
                        if (isExists && !s[i].Contains(item.Code[i].ToString()))
                            isExists = false;
                    }

                    if ((isExists && !iskeep) || (!isExists && iskeep))
                        tmpData.Remove(item);
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }
        #endregion

        #endregion

        #region 特殊

        #region 二星-不定型態-類型
        /// <summary>
        /// 二星-不定型態-類型
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="value">選項值</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> TwoStartType(List<BaseOptions> data, string value, bool isKeep)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && value.Contains("1"))
            {
                bool match = false;
                foreach (var item in data)
                {
                    match = (value[0] == '1' && PairNumber(item)) ||
                            (value[1] == '1' && TwoStartContinueNumber(item)) ||
                            (value[3] == '1' && FalsePair(item, 5));
                    //(value[4] == '1' && FalsePair(item, 6));

                    //散號
                    if (value[2] == '1')
                        match = (match || (!PairNumber(item) && !TwoStartContinueNumber(item)));

                    if (isKeep)
                    {
                        if (!match)
                            tmpData.Remove(item);
                    }
                    else
                    {
                        if (match)
                            tmpData.Remove(item);
                    }
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }
        #endregion

        #region 三星-位置大小匹配
        /// <summary>
        /// 三星-位置大小匹配
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="rows">資料列回傳</param>
        /// 百十個 | 大于 小于 等于 大于等于 小于等于 不等于 | 百十個
        public static List<BaseOptions> ThreeStartMatch(List<BaseOptions> data, List<Match> rows)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && rows != null && rows.Count > 0)
            {
                foreach (Match row in rows)
                {
                    if (row.Value1 == row.Value2)
                        continue;

                    if (row.Operator == 1)
                        tmpData = tmpData.Where(x => x.Code[row.Value1 - 1] > x.Code[row.Value2 - 1]).ToList();
                    else if (row.Operator == 2)
                        tmpData = tmpData.Where(x => x.Code[row.Value1 - 1] < x.Code[row.Value2 - 1]).ToList();
                    else if (row.Operator == 3)
                        tmpData = tmpData.Where(x => x.Code[row.Value1 - 1] == x.Code[row.Value2 - 1]).ToList();
                    else if (row.Operator == 4)
                        tmpData = tmpData.Where(x => x.Code[row.Value1 - 1] >= x.Code[row.Value2 - 1]).ToList();
                    else if (row.Operator == 5)
                        tmpData = tmpData.Where(x => x.Code[row.Value1 - 1] <= x.Code[row.Value2 - 1]).ToList();
                    else if (row.Operator == 6)
                        tmpData = tmpData.Where(x => x.Code[row.Value1 - 1] != x.Code[row.Value2 - 1]).ToList();
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }
        #endregion

        #region 三星-特別排除
        /// <summary>
        /// 三星-特別排除
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> ThreeSpecialData(List<BaseOptions> data, string value)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && value.Contains("1"))
            {
                bool match = true;
                foreach (var item in data)
                {
                    match = (value[0] == '1' && SameNumber(item)) ||
                            (value[1] == '1' && NSameNumber(item, 2)) ||
                            (value[2] == '1' && ExclusiveNumber(item)) ||
                            (value[3] == '1' && DisContinueNumber(item)) ||
                            (value[4] == '1' && ContinueNumber(item, 2)) ||
                            (value[5] == '1' && ContinueNumber(item, 3));

                    if (match)
                        tmpData.Remove(item);
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }
        #endregion

        #region 四星-殺特殊型態
        /// <summary>
        /// 四星-殺特殊型態
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="ht">資料集</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> FourSpecialData(List<BaseOptions> data, string value)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && value.Contains("1"))
            {
                bool match = true;
                foreach (var item in data)
                {
                    match = (value[0] == '1' && ASCNumber(item)) ||
                            (value[1] == '1' && DESCNumber(item)) ||
                            (value[2] == '1' && ConvexNumber(item)) ||
                            (value[3] == '1' && ConcaveNumber(item)) ||
                            (value[4] == '1' && NNumber(item)) ||
                            (value[5] == '1' && ReverseNNumber(item));

                    if (match)
                        tmpData.Remove(item);
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }
        #endregion

        #region 四星-特別排除
        /// <summary>
        /// 四星-特別排除
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="ht">資料集</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> FourSpecial2Data(List<BaseOptions> data, string value)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && value.Contains("1"))
            {
                bool match = true;
                foreach (var item in data)
                {
                    match = (value[0] == '1' && SameNumber(item)) ||
                            (value[1] == '1' && DisContinueNumber(item)) ||
                            (value[2] == '1' && ContinueNumber(item, 2)) ||
                            (value[3] == '1' && ContinueNumber(item, 3)) ||
                            (value[4] == '1' && ContinueNumber(item, 4)) ||
                            (value[6] == '1' && PairNumber(item)) ||
                            (value[7] == '1' && NUpSameNumber(item, 3)) ||
                            (value[8] == '1' && TwoPairNumber(item));

                    if (value[5] == '1')
                        match = (match || (!SameNumber(item) && !PairNumber(item) && !NSameNumber(item, 3) && !TwoPairNumber(item)));

                    if (match)
                        tmpData.Remove(item);
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }
        #endregion

        #region 五星-特別排除
        /// <summary>
        /// 五星-特別排除
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="ht">資料集</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> FiveSpecialData(List<BaseOptions> data, string value)
        {
            List<BaseOptions> tmpData = (data == null ? null : data.ToList());
            if (tmpData != null && tmpData.Count > 0 && value.Contains("1"))
            {
                bool match = true;
                foreach (var item in data)
                {
                    //上山.下山.不連.2連.3連.4連.5連.AAAAA.AABCD.AABBC.AAABB.AAABC.AAAAB.ABCDE
                    match = (value[0] == '1' && ASCNumber(item)) ||
                            (value[1] == '1' && DESCNumber(item)) ||
                            (value[2] == '1' && DisContinueNumber(item)) ||
                            (value[3] == '1' && ContinueNumber(item, 2)) ||
                            (value[4] == '1' && ContinueNumber(item, 3)) ||
                            (value[5] == '1' && ContinueNumber(item, 4)) ||
                            (value[6] == '1' && ContinueNumber(item, 5)) ||
                            (value[7] == '1' && AAAAANumber(item)) ||
                            (value[8] == '1' && AABCDNumber(item)) ||
                            (value[9] == '1' && AABBCNumber(item)) ||
                            (value[10] == '1' && AAABBNumber(item)) ||
                            (value[11] == '1' && AAABCNumber(item)) ||
                            (value[12] == '1' && AAAABNumber(item)) ||
                            (value[13] == '1' && ABCDENumber(item));

                    if (match)
                        tmpData.Remove(item);
                }
            }
            return (tmpData == null ? null : tmpData.OrderBy(x => x.Code).ToList());
        }
        #endregion

        /// <summary>
        /// 五星-大小比
        /// </summary>
        /// <param name="data">全部組合資料</param>
        public static List<BaseOptions> BigSmallRatio(List<BaseOptions> data, string value)
        {
            bool match = true;
            if (data != null && data.Count > 0)
            {
                string number = (data[0].ID == 100000 ? 0 : data[0].ID).ToString();
                var i = number.Where(x => int.Parse(x.ToString()) >= 5).Count();
                for (int j = 0; j < value.Length; j++)
                {
                    if (value[j] == '1')
                    {
                        if (i == j)
                            match = false;
                    }

                    if (!match)
                        break;
                }
            }
            return match ? data : new List<BaseOptions>();
        }

        /// <summary>
        /// 五星-奇偶比
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="ht">資料集</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> OddEvenRatio(List<BaseOptions> data, string value)
        {
            bool match = true;
            if (data != null && data.Count > 0)
            {
                string number = (data[0].ID == 100000 ? 0 : data[0].ID).ToString();
                var i = number.Where(x => int.Parse(x.ToString()) % 2 == 1).Count();
                for (int j = 0; j < value.Length; j++)
                {
                    if (value[j] == '1')
                    {
                        if (i == j)
                            match = false;
                    }

                    if (!match)
                        break;
                }
            }
            return match ? data : new List<BaseOptions>();
        }

        /// <summary>
        /// 五星-質合比
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="ht">資料集</param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> PrimeRatio(List<BaseOptions> data, string value)
        {
            bool match = true;
            if (data != null && data.Count > 0)
            {
                string number = (data[0].ID == 100000 ? 0 : data[0].ID).ToString();
                var i = number.Where(x => "12357".Contains(x.ToString())).Count();
                for (int j = 0; j < value.Length; j++)
                {
                    if (value[j] == '1')
                    {
                        if (i == j)
                            match = false;
                    }

                    if (!match)
                        break;
                }
            }
            return match ? data : new List<BaseOptions>();
        }
        #endregion

        #region 特殊處理

        #region (二星使用)連號
        /// <summary>
        /// 連號-二星使用
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:連號 false:不連號</returns>
        private static bool TwoStartContinueNumber(BaseOptions data)
        {
            int last = -99;
            int tmpNumber = 0;
            foreach (var c in data.Code)
            {
                int.TryParse(c.ToString(), out tmpNumber);
                if (tmpNumber - 1 == last || tmpNumber + 1 == last)
                    return true;

                last = tmpNumber;
            }
            return false;
        }
        #endregion

        #region (二星使用)假對/假連
        /// <summary>
        /// 假對/假連-二星使用
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:假對 false:非假對</returns>
        private static bool FalsePair(BaseOptions data, int value)
        {
            int i = int.Parse(data.Code[0].ToString()) - int.Parse(data.Code[1].ToString());
            return (i < 0 ? -1 : 1) * i == value;
        }
        #endregion

        #region 上山
        /// <summary>
        /// 上山
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:有符合上山 false:未符合上山</returns>
        private static bool ASCNumber(BaseOptions data)
        {
            bool match = false;

            if (data != null)
            {
                int tmpNumber = -1;
                int codeChar = 0;
                if (data.Code != "".PadRight(data.Code.Count(), data.Code[0]))
                {
                    foreach (var c in data.Code)
                    {
                        int.TryParse(c.ToString(), out codeChar);

                        if (codeChar >= tmpNumber)
                        {
                            tmpNumber = codeChar;
                            match = true;
                        }
                        else
                            match = false;

                        if (!match)
                            break;
                    }
                }
            }
            return match;
        }
        #endregion

        #region 下山
        /// <summary>
        /// 下山
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:有符合下山 false:未符合下山</returns>
        private static bool DESCNumber(BaseOptions data)
        {
            bool match = false;

            if (data != null)
            {
                if (data.Code != "".PadRight(data.Code.Count(), data.Code[0]))
                {
                    int tmpNumber = 10;
                    int codeChar = 0;
                    foreach (var c in data.Code)
                    {
                        int.TryParse(c.ToString(), out codeChar);
                        if (codeChar <= tmpNumber)
                        {
                            tmpNumber = codeChar;
                            match = true;
                        }
                        else
                            match = false;

                        if (!match)
                            break;
                    }
                }
            }
            return match;
        }
        #endregion

        #region 凸型
        /// <summary>
        /// 凸型
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:有符合凸型 false:未符合凸型</returns>
        private static bool ConvexNumber(BaseOptions data)
        {
            bool match = false;

            if (data != null)
            {
                //相同數字or數字小於2直接過濾掉
                if (data.Code.Length <= 2 || SameNumber(data))
                    return false;

                match = true;

                //第一碼
                int firstNo = 0;
                int.TryParse(data.Code[0].ToString(), out firstNo);

                //最後一碼
                int lastNo = 0;
                int.TryParse(data.Code[data.Code.Length - 1].ToString(), out lastNo);

                int max = 0;
                int min = 0;
                int[] number = new int[data.Code.Length - 2];
                for (int x = 1; x < data.Code.Length - 1; x++)
                    number[x - 1] = int.Parse(data.Code[x].ToString());

                max = number.Max();
                min = number.Min();
                int i = string.Join("", number).IndexOf(char.Parse(max.ToString()));
                int i2 = string.Join("", number).IndexOf(char.Parse(min.ToString()));

                if (max <= firstNo || max <= lastNo || (i != i2 && ((i > i2 && min < firstNo) || (i < i2 && min < lastNo))))
                    match = false;
            }
            return match;
        }
        #endregion

        #region 凹型
        /// <summary>
        /// 凹型
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:有符合凹型 false:未符合凹型</returns>
        private static bool ConcaveNumber(BaseOptions data)
        {
            bool match = false;

            if (data != null)
            {
                //相同數字or數字小於2直接過濾掉
                if (data.Code.Length <= 2 || SameNumber(data))
                    return false;

                match = true;

                //第一碼
                int firstNo = 0;
                int.TryParse(data.Code[0].ToString(), out firstNo);

                //最後一碼
                int lastNo = 0;
                int.TryParse(data.Code[data.Code.Length - 1].ToString(), out lastNo);

                int max = 0;
                int min = 0;
                int[] number = new int[data.Code.Length - 2];
                for (int x = 1; x < data.Code.Length - 1; x++)
                    number[x - 1] = int.Parse(data.Code[x].ToString());

                max = number.Max();
                min = number.Min();
                int i = string.Join("", number).IndexOf(char.Parse(max.ToString()));
                int i2 = string.Join("", number).IndexOf(char.Parse(min.ToString()));

                if (min >= firstNo || min >= lastNo || (i != i2 && ((i2 > i && max > firstNo) || (i2 < i && max > lastNo))))
                    match = false;
            }
            return match;
        }
        #endregion

        #region N型
        /// <summary>
        /// N型
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:有符合N型 false:未符合N型</returns>
        private static bool NNumber(BaseOptions data)
        {
            bool match = true;

            if (data != null)
            {
                int codeChar = 0;

                //暫存數字
                int tmpNumber = 10;

                int count = 1;
                foreach (var c in data.Code)
                {
                    int.TryParse(c.ToString(), out codeChar);
                    if (count % 2 == 1)
                    {
                        //奇數
                        if (codeChar >= tmpNumber)
                            match = false;
                        else
                            tmpNumber = codeChar;
                    }
                    else
                    {
                        //偶數
                        if (codeChar <= tmpNumber)
                            match = false;
                        else
                            tmpNumber = codeChar;
                    }

                    if (!match)
                        break;
                    count++;
                }
            }
            return match;
        }
        #endregion

        #region 反N型
        /// <summary>
        /// 反N型
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:有符合反N型 false:未符合反N型</returns>
        private static bool ReverseNNumber(BaseOptions data)
        {
            bool match = true;

            if (data != null)
            {
                int codeChar = 0;

                //暫存數字
                int tmpNumber = -1;

                int count = 1;
                foreach (var c in data.Code)
                {
                    int.TryParse(c.ToString(), out codeChar);
                    if (count % 2 == 1)
                    {
                        //奇數
                        if (codeChar <= tmpNumber)
                            match = false;
                        else
                            tmpNumber = codeChar;
                    }
                    else
                    {
                        //偶數
                        if (codeChar >= tmpNumber)
                            match = false;
                        else
                            tmpNumber = codeChar;
                    }

                    if (!match)
                        break;
                    count++;
                }
            }
            return match;
        }
        #endregion

        #region 同數字(豹子)
        /// <summary>
        /// 同數字(豹子)
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:數字皆為同一數字 false:數字非同一數字</returns>
        private static bool SameNumber(BaseOptions data)
        {
            return data.Code.ToCharArray().Distinct().Count() == 1;
        }
        #endregion

        #region 不同數字
        /// <summary>
        /// 不同數字
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:完全不同數字 false:有同數字的</returns>
        private static bool ExclusiveNumber(BaseOptions data)
        {
            return data.Code.ToCharArray().Distinct().Count() == data.Code.Length;
        }
        #endregion

        #region 不連
        /// <summary>
        /// 不連
        /// remark:0 跟 9 是連號
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:不連號 false:有連號</returns>
        private static bool DisContinueNumber(BaseOptions data)
        {
            var code = string.Join("", data.Code.OrderBy(x => x).Distinct());

            //最後一碼
            int last = 0;
            int.TryParse(code[code.Length - 1].ToString(), out last);
            int tmpNumber = 0;
            foreach (var c in code)
            {
                int.TryParse(c.ToString(), out tmpNumber);
                if ((tmpNumber - 1 == -1 ? 9 : tmpNumber - 1) == last ||
                    (tmpNumber + 1 == 10 ? 0 : tmpNumber + 1) == last)
                    return false;
                int.TryParse(c.ToString(), out last);
            }
            return true;
        }
        #endregion

        #region n連號
        /// <summary>
        /// 是否n連號
        /// </summary>
        /// <param name="data"></param>
        /// <param name="n">n連 => n=0 則為不連號</param>
        /// <param name="isUp">n連以上</param>
        /// <returns>是否n連號</returns>
        private static bool ContinueNumber(BaseOptions data, int n, bool isUp = false)
        {
            List<int> list = data.Code.OrderBy(x => x).Select(x => int.Parse(x.ToString())).Distinct().ToList();
            if (list.Count < n || n < 2)
                return false;

            //if (isUp)
            //{
            //var array = string.Join("|", chararray);
            //array = (chararray[chararray.Count() - 1] == '9' && chararray[0] == '0' ? "-1|" : "") + array + (chararray[chararray.Count() - 1] == '9' && chararray[0] == '0' ? "|0" : "");
            //var tmp = array.Split('|').ToArray();

            ////上一碼
            //int last = 0;
            //int.TryParse(tmp[0].ToString(), out last);
            //int tmpNumber = 0;
            //int max = 0;
            //int count = 1;

            //for (int i = 1; i < tmp.Length; i++)
            //{
            //    int.TryParse(tmp[i].ToString(), out tmpNumber);
            //    if ((last == 9 ? -1 : last) + 1 == tmpNumber)
            //        count++;
            //    else
            //        count = 1;

            //    last = tmpNumber;
            //    if (count > max)
            //        max = count;
            //}
            //return (n == 0 ? max >= 2 : (isDistinct ? (max >= n) : (max == n)));
            //}

            //上一碼
            int tmpNumber = 0;
            int max = 0;
            int count = 1;

            List<int> tmp = list.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 1; j < list.Count; j++)
                {
                    if ((tmp[j - 1] == 9 ? -1 : tmp[j - 1]) + 1 == tmp[j])
                        count++;
                    else
                        count = 1;

                    if (count > max)
                        max = count;

                    //提早回傳, 節省效能
                    if (isUp && max >= n)
                        return true;
                }

                if (i == list.Count - 1)
                    break;

                tmpNumber = tmp[0];
                tmp = tmp.GetRange(1, list.Count - 1);
                tmp.Add(tmpNumber);
                count = 1;
            }
            return (max == n);
        }
        #endregion

        #region 對子號
        /// <summary>
        /// 對子號
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:有符合對子號 false:未符合對子號</returns>
        private static bool PairNumber(BaseOptions data)
        {
            return data.Code.ToCharArray().Distinct().Count() <= (data.Code.Length - 1);
        }
        #endregion

        #region n同號
        /// <summary>
        /// n同號
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:有符合n同號 false:未符合n同號</returns>
        private static bool NSameNumber(BaseOptions data, int n)
        {
            for (int i = 0; i <= 9; i++)
            {
                if (data.Code.Where(x => x.ToString() == i.ToString()).Count() == n)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// n同號(含以上)
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:有符合n同號(含以上) false:未符合n同號(含以上)</returns>
        private static bool NUpSameNumber(BaseOptions data, int n)
        {
            for (int i = 0; i <= 9; i++)
            {
                if (data.Code.Where(x => x.ToString() == i.ToString()).Count() >= n)
                    return true;
            }
            return false;
        }
        #endregion

        #region 兩個對子
        /// <summary>
        /// 兩個對子
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:有符合兩個對子 false:未符合兩個對子</returns>
        private static bool TwoPairNumber(BaseOptions data)
        {
            var tmp = data.Code.ToCharArray().Distinct();
            if (data.Code.ToCharArray().Distinct().Count() != 2)
                return false;

            foreach (var x in tmp)
            {
                if (data.Code.Replace(x.ToString(), "").Length != 2)
                    return false;
            }
            return true;
        }
        #endregion

        #region AAAAA
        /// <summary>
        /// 豹子
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:有符合豹子 false:未符合豹子</returns>
        private static bool AAAAANumber(BaseOptions data)
        {
            return data.Code.ToCharArray().Distinct().Count() == 1;
        }
        #endregion

        #region AABCD
        /// <summary>
        /// AABCD
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:符合 false:未符合</returns>
        private static bool AABCDNumber(BaseOptions data)
        {
            if (data.Code.ToCharArray().Distinct().Count() == 4)
            {
                //舊寫法有考慮到位置
                //if (data.Code[0] == data.Code[1])
                //    return true;

                return true;
            }
            return false;
        }
        #endregion

        #region AABBC
        /// <summary>
        /// AABBC
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:符合 false:未符合</returns>
        private static bool AABBCNumber(BaseOptions data)
        {
            if (data.Code.ToCharArray().Distinct().Count() == 3)
            {
                if (data.Code.Where(x => data.Code.Length - data.Code.Replace(x.ToString(), "").Length > 2).Count() == 0)
                    return true;
            }
            return false;

            //舊寫法有考慮到位置
            //if (data.Code.ToCharArray().Distinct().Count() == 3)
            //{
            //    //if (data.Code[0] == data.Code[1] &&
            //    //    data.Code[2] == data.Code[3])
            //    //    return true;
            //}
            //return false;
        }
        #endregion

        #region AAABB
        /// <summary>
        /// AAABB
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:符合 false:未符合</returns>
        private static bool AAABBNumber(BaseOptions data)
        {
            //舊的寫法有考慮位置
            //if (data.Code.ToCharArray().Distinct().Count() == 2)
            //{
            //    if (data.Code[0] == data.Code[1] &&
            //        data.Code[0] == data.Code[2] &&
            //        data.Code[3] == data.Code[4])
            //        return true;
            //}

            if (data.Code.ToCharArray().Distinct().Count() == 2)
            {
                if (data.Code.Where(x => data.Code.Length - data.Code.Replace(x.ToString(), "").Length > 3).Count() == 0)
                    return true;
            }
            return false;
        }
        #endregion

        #region AAABC
        /// <summary>
        /// AAABC
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:符合 false:未符合</returns>
        private static bool AAABCNumber(BaseOptions data)
        {
            //if (data.Code.ToCharArray().Distinct().Count() == 3)
            //{
            //    if (data.Code[0] == data.Code[1] &&
            //        data.Code[0] == data.Code[2])
            //        return true;
            //}

            if (data.Code.ToCharArray().Distinct().Count() == 3)
            {
                if (data.Code.Where(x => data.Code.Length - data.Code.Replace(x.ToString(), "").Length == 3).Count() > 0)
                    return true;
            }
            return false;
        }
        #endregion

        #region AAAAB
        /// <summary>
        /// AAAAB
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:符合 false:未符合</returns>
        private static bool AAAABNumber(BaseOptions data)
        {
            //if (data.Code.ToCharArray().Distinct().Count() == 2)
            //{
            //    if (data.Code[0] == data.Code[1] &&
            //        data.Code[0] == data.Code[2] &&
            //        data.Code[0] == data.Code[3])
            //        return true;
            //}

            if (data.Code.ToCharArray().Distinct().Count() == 2)
            {
                if (data.Code.Where(x => data.Code.Length - data.Code.Replace(x.ToString(), "").Length == 4).Count() > 0)
                    return true;
            }
            return false;
        }
        #endregion

        #region ABCDE
        /// <summary>
        /// ABCDE
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:符合 false:未符合</returns>
        private static bool ABCDENumber(BaseOptions data)
        {
            return data.Code.ToCharArray().Distinct().Count() == 5;
        }
        #endregion

        #endregion

    }
}
