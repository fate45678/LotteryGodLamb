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
        /// <summary>
        /// 判斷前組合
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbl"></param>
        /// <param name="type">Type: 1.用code 2.用id</param>
        /// <returns></returns>
        private static string BeforeCheck(List<BaseOptions> data, CheckBoxList cbl, int type = 1)
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
                    condition += (condition == "" ? "" : ",") + string.Join(",", tmp.Where(x => x.ID == i + 1).Select(x => x.Code));
            }
            return condition;
        }

        //尚未完成
        #region 定位 or 定位殺 or 組合
        /// <summary>
        /// 定位 or 定位殺 or 組合
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="data"></param>
        /// <param name="isKeep">定位 or 定位殺</param>
        private static List<BaseOptions> PosNumber(List<BaseOptions> data, string condition, string pos, bool isKeep)
        {
            List<BaseOptions> tmpData = new List<BaseOptions>();
            if (!string.IsNullOrEmpty(condition))
            {
                tmpData = data.ToList();
                var conArray = condition.Split(',');
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
            return tmpData.OrderBy(x => x.Code).ToList();
        }

        /// <summary>
        /// 奇偶數判斷-for CheckBoxList使用
        /// </summary>
        /// <param name="data">全部組合資料</param>
        /// <param name="cbl"></param>
        /// <param name="isKeep">是否保留</param>
        public static List<BaseOptions> PosNumber(List<BaseOptions> data, CheckBoxList cbl, string pos, bool isKeep = true)
        {
            if (data == null || data.Count() == 0 ||
                cbl == null || cbl.SelectedValue == "" ||
                cbl.SelectedValue == "".PadRight(cbl.SelectedValue.Length, '0'))
                return data;

            var tmp = cbl.ItemsSource.Cast<BaseOptions>().ToList();
            if (tmp == null)
                return data;

            string condition = "";
            for (int i = 0; i < cbl.SelectedValue.Count(); i++)
            {
                if (cbl.SelectedValue[i] == '1')
                {
                    condition += (condition == "" ? "" : ",") + string.Join(",", tmp.Where(x => x.ID == i + 1).Select(x => x.Code));
                }
            }
            return PosNumber(data, condition, pos, isKeep);
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
            List<BaseOptions> tmpData = new List<BaseOptions>();
            if (!string.IsNullOrEmpty(condition))
            {
                tmpData = data.ToList();
                var conArray = condition.Split(',');
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
            return tmpData.OrderBy(x => x.Code).ToList();
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
        private static List<BaseOptions> CheckValueNumber(List<BaseOptions> data, string condition, int type, bool isKeep = true)
        {
            List<BaseOptions> tmpData = new List<BaseOptions>();
            if (!string.IsNullOrEmpty(condition))
            {
                tmpData = data.ToList();
                var conArray = condition.Split(',');
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
                        checkvalue += ((int)(number / lenth) - (number > 0 && number % lenth == 0 ? 1 : 0)).ToString();
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
            return tmpData.OrderBy(x => x.Code).ToList();
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
            List<BaseOptions> tmpData = new List<BaseOptions>();
            if (!string.IsNullOrEmpty(condition))
            {
                tmpData = data.ToList();
                var conArray = condition.Split(',');
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
            return tmpData.OrderBy(x => x.Code).ToList();
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
            List<BaseOptions> tmpData = new List<BaseOptions>();
            if (!string.IsNullOrEmpty(condition))
            {
                tmpData = data.ToList();
                var conArray = condition.Split(',');
                string checkvalue = "";
                string[] prime = new string[4] { "2", "3", "5", "7" };

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
            return tmpData.OrderBy(x => x.Code).ToList();
        }

        /// <summary>
        /// 質合判斷 for CheckBoxList使用
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
        private static List<BaseOptions> SumNumber(List<BaseOptions> data, string condition, bool isKeep = true)
        {
            List<BaseOptions> tmpData = new List<BaseOptions>();
            if (!string.IsNullOrEmpty(condition))
            {
                tmpData = data.ToList();
                var conArray = condition.Split(',');
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
            return tmpData.OrderBy(x => x.Code).ToList();
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

        #region 合尾判斷
        /// <summary>
        /// 合尾判斷
        /// </summary>
        private static List<BaseOptions> SumLastNumber(List<BaseOptions> data, string condition, bool isKeep = true)
        {
            List<BaseOptions> tmpData = new List<BaseOptions>();
            if (!string.IsNullOrEmpty(condition))
            {
                tmpData = data.ToList();
                var conArray = condition.Split(',');
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
            return tmpData.OrderBy(x => x.Code).ToList();
        }

        /// <summary>
        /// 合尾判斷 for CheckBoxList使用
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
            List<BaseOptions> tmpData = new List<BaseOptions>();
            if (!string.IsNullOrEmpty(condition))
            {
                tmpData = data.ToList();
                var conArray = condition.Split(',');
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
            return tmpData.OrderBy(x => x.Code).ToList();
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
    }
}
