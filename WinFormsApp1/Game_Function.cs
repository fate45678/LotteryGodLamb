using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormsApp1
{
    public static class Game_Function
    {
        public static string GameNameToCode(string strGameName)
        {
            string strCode = "";
            switch (strGameName)
            {
                case "重庆时时彩":
                    strCode = "CQSSC";
                    break;
                case "天津时时彩":
                    strCode = "TJSSC";
                    break;
                case "腾讯官方彩":
                    strCode = "QQFFC";
                    break;
                case "腾讯奇趣彩":
                    strCode = "TENCENTFFC";
                    break;
                default:
                    strCode = "CQSSC";
                    break;
            }
            return strCode;
        }

        /// <summary>
        /// 大或小
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetNumBigOrSmall(int num)
        {
            string result;
            if (num >= 0 && num <= 4)
                result = "小";
            else
                result = "大";
            return result;
        }

        /// <summary>
        /// 單數或雙數
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetNumOddOrEven(int num)
        {
            string result;
            if (num % 2 == 1)
                result = "单";
            else
                result = "双";
            return result;
        }

        /// <summary>
        /// 質數或合數
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetNumPrimeOrNot(int num)
        {
            string result;
            if (num == 2 || num == 3 || num == 5 || num == 7)
                result = "质";
            else
                result = "合";
            return result;
        }

        /// <summary>
        /// 除以3的餘數
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetNum012(int num)
        {
            string result = (num % 3).ToString();
            return result;
        }

        /// <summary>
        /// 是否有其中兩個相同
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="num3"></param>
        /// <returns></returns>
        public static string GetNumIsSame2(int num1, int num2, int num3)
        {
            string result = "";
            if ((num1 == num2 && num2 != num3) || (num2 == num3 && num1 != num3) || (num1 == num3 && num1 != num2))
                result = "V";
            else
                result = "";
            return result;
        }

        /// <summary>
        /// 三個都不同
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="num3"></param>
        /// <returns></returns>
        public static string GetNumIsDiff(int num1, int num2, int num3)
        {
            string result = "";
            if (num1 != num2 && num2 != num3 && num3 != num1)
                result = "V";
            else
                result = "";
            return result;
        }

        /// <summary>
        /// 三個都相同
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="num3"></param>
        /// <returns></returns>
        public static string GetNumAllTheSame(int num1, int num2, int num3)
        {
            string result = "";
            if (num1 == num2 && num2 == num3)
                result = "V";
            else
                result = "";
            return result;
        }

        /// <summary>
        /// 兩個都相同
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static string GetNumIsPair(int num1, int num2)
        {
            string result = "";
            if (num1 == num2)
                result = "V";
            else
                result = "";
            return result;
        }

        /// <summary>
        /// 兩個的差值
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static string GetNumSubstract(int num1, int num2)
        {
            string result = "";
            result = Math.Abs(num1 - num2).ToString();
            return result;
        }

        /// <summary>
        /// 三個中，最大與最小的差值
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="num3"></param>
        /// <returns></returns>
        public static string GetNumSubstract(int num1, int num2, int num3)
        {
            string result = "";
            int max = 0;
            int min = 10;
            if (num1 > max) max = num1;
            if (num2 > max) max = num2;
            if (num3 > max) max = num3;
            if (num1 < min) min = num1;
            if (num2 < min) min = num2;
            if (num3 < min) min = num3;
            result = (max - min).ToString();
            return result;
        }

        /// <summary>
        /// 兩個的和值
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static string GetNumSum(int num1, int num2)
        {
            string result = "";
            result = (num1 + num2).ToString();
            return result;
        }

        /// <summary>
        /// 三個的和值
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="num3"></param>
        /// <returns></returns>
        public static string GetNumSum(int num1, int num2, int num3)
        {
            string result = "";
            result = (num1 + num2 + num3).ToString();
            return result;
        }

        /// <summary>
        /// 三個和值之尾數
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="num3"></param>
        /// <returns></returns>
        public static string GetNumSumTrail(int num1, int num2, int num3)
        {
            string result = "";
            result = ((num1 + num2 + num3) % 10).ToString();
            return result;
        }
    }
}
