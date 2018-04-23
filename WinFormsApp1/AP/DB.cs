using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTest.AP
{
    public class DB
    {
        #region 共用
        /// <summary>
        /// 產生連續數字
        /// ex. 0 1 2 3 4 5 6 7 8 9
        /// </summary>
        /// <param name="start">開始數字</param>
        /// <param name="end">結束數字</param>
        /// <param name="optionLenth">選項長度 ex. 若OptionLenth=2, 則0這個數字就會被替換成00</param>
        /// <returns></returns>
        public static List<BaseOptions> CreateContinueNumber(int start = 0, int end = 9, int optionLenth = 0)
        {
            var tmp = new List<BaseOptions>();
            string padString = "";
            int count = 1;
            for (int i = start; i <= end; i++)
            {
                if (optionLenth > 0)
                    padString = (optionLenth > i.ToString().Length ? "".PadRight(optionLenth - i.ToString().Length, '0') : "");

                tmp.Add(new BaseOptions
                {
                    ID = count,
                    Name = padString + i.ToString(),
                    Code = i.ToString()
                });

                count++;
            }
            return tmp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start">開始數字</param>
        /// <param name="end">結束數字</param>
        /// <param name="replace">替換文字</param>
        /// <returns></returns>
        public static List<BaseOptions> CreateOption(int start, int end, string[] replace = null)
        {
            var tmp = CreateContinueNumber(start, end, 0);
            if (replace != null)
            {
                int arrayIndex = 0;
                foreach (var item in tmp)
                {
                    item.Name = replace[arrayIndex];
                    arrayIndex++;
                    if (arrayIndex > replace.Count())
                        break;
                }
            }
            return tmp;
        }

        /// <summary>
        /// 01排列組合
        /// </summary>
        /// <param name="number"></param>
        /// <param name="replaceOne"></param>
        /// <param name="replaceZero"></param>
        /// <returns></returns>
        public static List<BaseOptions> ZeroOneCombination(int number, char replaceOne = '1', char replaceZero = '0')
        {
            var tmp = new List<BaseOptions>();
            if (number > 0)
            {
                string comb = "".PadRight(number, '0');
                char[] charArray;

                int count = 0;
                int.TryParse(Math.Pow(2, number).ToString(), out count);
                int no = 0;

                for (int i = 1; i <= count; i++)
                {
                    charArray = comb.ToArray();
                    for (int j = number - 1; j >= 0; j--)
                    {
                        int.TryParse(charArray[j].ToString(), out no);
                        if (no == 0)
                        {
                            charArray[j] = '1';
                            comb = string.Join("", charArray);
                            tmp.Add(new WpfAppTest.BaseOptions { ID = i, Name = comb.Replace('1', replaceOne).Replace('0', replaceZero), Code = comb });
                            break;
                        }
                        else if (no == 1)
                        {
                            charArray[j] = '0';
                            if (j == 0)
                            {
                                comb = string.Join("", charArray);
                                tmp.Add(new WpfAppTest.BaseOptions { ID = i, Name = comb.Replace('1', replaceOne).Replace('0', replaceZero), Code = comb });
                            }
                            continue;
                        }
                    }
                }
            }
            return tmp;
        }

        /// <summary>
        /// 比例-reverse: 0-5, 1-4, 2-3, 3-2, 4-1, 5-0
        /// </summary>
        /// <param name="number"></param>
        /// <param name="start">起始數字</param>
        /// <returns></returns>
        public static List<BaseOptions> Ratio(int number, int start = 0)
        {
            var tmp = new List<BaseOptions>();
            int reverse = number;
            for (int i = start; i <= number; i++)
            {
                tmp.Add(new BaseOptions { ID = i, Name = i.ToString() + "：" + reverse.ToString() });
                reverse--;
            }
            return tmp;
        }

        /// <summary>
        /// 數字組合
        /// </summary>
        /// <param name="number">號碼長度</param>
        /// <param name="start">開始數字</param>
        /// <param name="end">結束數字</param>
        /// <returns></returns>
        public static List<BaseOptions> CombinationNumber(int number, int start = 0, int end = 1, string[] replace = null)
        {
            var tmp = new List<BaseOptions>();
            if (number > 0 && start >= 0 && end >= 1)
            {
                char c = '0';
                char.TryParse(start.ToString(), out c);

                string comb = "".PadRight(number, c);
                char[] charArray = new char[] { };

                int count = 0;
                int.TryParse(Math.Pow(end - start + 1, number).ToString(), out count);
                int no = 0;
                for (int i = 1; i <= count; i++)
                {
                    charArray = comb.ToArray();
                    for (int j = number - 1; j >= 0; j--)
                    {
                        int.TryParse(charArray[j].ToString(), out no);
                        if (no < end)
                        {
                            charArray[j] = char.Parse((no + 1).ToString());
                            char[] charArray2 = charArray.ToArray();
                            if (replace != null)
                            {
                                for (int x = 0; x < number; x++)
                                {
                                    for (int y = 0; y < replace.Count(); y++)
                                    {
                                        if (charArray[x].ToString() == y.ToString())
                                        {
                                            charArray2[x] = char.Parse(charArray[x].ToString().Replace(charArray[x].ToString(), replace[y]));
                                            break;
                                        }
                                    }
                                }
                            }
                            comb = string.Join("", charArray);
                            tmp.Add(new WpfAppTest.BaseOptions { ID = i, Name = string.Join("", charArray2), Code = comb });
                            break;
                        }
                        else
                        {
                            charArray[j] = char.Parse(start.ToString());
                            if (j == 0)
                            {
                                char[] charArray2 = charArray.ToArray();
                                if (replace != null)
                                {
                                    for (int x = 0; x < number; x++)
                                    {
                                        for (int y = 0; y < replace.Count(); y++)
                                        {
                                            if (charArray[x].ToString() == y.ToString())
                                            {
                                                charArray2[x] = char.Parse(charArray[x].ToString().Replace(charArray[x].ToString(), replace[y]));
                                                break;
                                            }
                                        }
                                    }
                                }
                                comb = string.Join("", charArray);
                                tmp.Add(new WpfAppTest.BaseOptions { ID = i, Name = string.Join("", charArray2), Code = comb });
                            }
                            continue;
                        }
                    }
                }
            }
            return tmp;
        }
        #endregion

        #region 2星

        #region 不定位形
        /// <summary>
        /// ex. ?子  假?  ??  假?  ??
        /// </summary>
        /// <returns></returns>
        public static List<BaseOptions> TwoStartOption1()
        {
            var tmp = new List<BaseOptions>
            {
                new BaseOptions
                {
                    ID = 1, Name = "?子", Code = "1"
                },
                new BaseOptions
                {
                    ID = 2, Name = "假?", Code = "2"
                },
                new BaseOptions
                {
                    ID = 3, Name = "??", Code = "3"
                },
                new BaseOptions
                {
                    ID = 4, Name = "假?", Code = "4"
                },
                new BaseOptions
                {
                    ID = 5, Name = "??", Code = "5"
                }
            };
            return tmp;
        }
        #endregion

        #region 和值
        /// <summary>
        /// 和值分類一 
        /// ex. A [0-7]  B [7-11]  C [11-18]
        /// </summary>
        /// <returns></returns>
        public static List<BaseOptions> TwoStartSumType1()
        {
            var tmp = new List<BaseOptions>
            {
                new BaseOptions
                {
                    ID = 1, Name = "A [0-7]", Code = "0,7"
                },
                new BaseOptions
                {
                    ID = 2, Name = "B [7-11]", Code = "7,11"
                },
                new BaseOptions
                {
                    ID = 3, Name = "C [11-18]", Code = "11,18"
                }
            };
            return tmp;
        }

        /// <summary>
        /// 和值分類二
        /// ex. A (7-11)  B (6-12)  C (0-5|13-18)
        /// </summary>
        /// <returns></returns>
        public static List<BaseOptions> TwoStartSumType2()
        {
            var tmp = new List<BaseOptions>
            {
                new BaseOptions
                {
                    ID = 1, Name = "A (7-11)", Code = "0,7"
                },
                new BaseOptions
                {
                    ID = 2, Name = "B (6-12)", Code = "7,11"
                },
                new BaseOptions
                {
                    ID = 3, Name = "C (0-5|13-18)", Code = "13,18"
                }
            };
            return tmp;
        }

        /// <summary>
        /// 和值分類三
        /// ex. A (0-6)  B (7-11)  C (12-18)
        /// </summary>
        /// <returns></returns>
        public static List<BaseOptions> TwoStartSumType3()
        {
            var tmp = new List<BaseOptions>
            {
                new BaseOptions
                {
                    ID = 1, Name = "A (0-6)", Code = "0,6"
                },
                new BaseOptions
                {
                    ID = 2, Name = "B (7-11)", Code = "7,11"
                },
                new BaseOptions
                {
                    ID = 3, Name = "C (12-18)", Code = "12,18"
                }
            };
            return tmp;
        }

        /// <summary>
        /// 和值分類四
        /// ex. A (0-3)  B (4-8)  C (9-13)  D (14-18)
        /// </summary>
        /// <returns></returns>
        public static List<BaseOptions> TwoStartSumType4()
        {
            var tmp = new List<BaseOptions>
            {
                new BaseOptions
                {
                    ID = 1, Name = "A (0-3)", Code = "0,3"
                },
                new BaseOptions
                {
                    ID = 2, Name = "B (4-8)", Code = "4,8"
                },
                new BaseOptions
                {
                    ID = 3, Name = "C (9-13)", Code = "9,13"
                },
                new BaseOptions
                {
                    ID = 4, Name = "D (14-18)", Code = "14,18"
                }
            };
            return tmp;
        }
        #endregion

        #region 和尾
        /// <summary>
        /// 和尾分類一
        /// ex. A [0 3 4 8]  B [2 6 7]  C [1 5 9]
        /// </summary>
        /// <returns></returns>
        public static List<BaseOptions> TwoStart_SumLastType1()
        {
            var tmp = new List<BaseOptions>
            {
                new BaseOptions
                {
                    ID = 1, Name = "A [0 3 4 8]", Code = "0,3,4,8"
                },
                new BaseOptions
                {
                    ID = 2, Name = "B [2 6 7]", Code = "2,6,7"
                },
                new BaseOptions
                {
                    ID = 3, Name = "C [1 5 9]", Code = "1,5,9"
                }
            };
            return tmp;
        }

        /// <summary>
        /// 和尾分類二
        /// ex. A (0-3)  B (4-8)  C (9-13)  D (14-18)
        /// </summary>
        /// <returns></returns>
        public static List<BaseOptions> TwoStart_SumLastType2()
        {
            var tmp = new List<BaseOptions>
            {
                new BaseOptions
                {
                    ID = 1, Name = "A [2 4 9]", Code = "2,4,9"
                },
                new BaseOptions
                {
                    ID = 2, Name = "B [0 3 5 7]", Code = "0,3,5,7"
                },
                new BaseOptions
                {
                    ID = 3, Name = "C [1 6 8]", Code = "1,6,8"
                }
            };
            return tmp;
        }
        #endregion

        #endregion

        #region 3星獨有

        #endregion

        #region 4星獨有

        #endregion

        #region 5星獨有
        /// <summary>
        /// 特別排除項目-5星
        /// </summary>
        /// <returns></returns>
        public static List<BaseOptions> FiveStart_SpecialExclude()
        {
            var tmp = new List<BaseOptions>
            {
                new BaseOptions
                {
                    ID = 1, Name = "上山"
                },
                new BaseOptions
                {
                    ID = 2, Name = "下山"
                },
                new BaseOptions
                {
                    ID = 3, Name = "不連"
                },
                new BaseOptions
                {
                    ID = 4, Name = "2連"
                },
                new BaseOptions
                {
                    ID = 5, Name = "3連"
                },
                new BaseOptions
                {
                    ID = 6, Name = "4連"
                },
                new BaseOptions
                {
                    ID = 7, Name = "5連"
                },
                new BaseOptions
                {
                    ID = 8, Name = "AAAAA"
                },
                new BaseOptions
                {
                    ID = 9, Name = "AABCD"
                },
                new BaseOptions
                {
                    ID = 10, Name = "AAABB"
                },
                new BaseOptions
                {
                    ID = 11, Name = "AAABC"
                },
                new BaseOptions
                {
                    ID = 12, Name = "AAAAB"
                },
                new BaseOptions
                {
                    ID = 13, Name = "ABCDE"
                }
            };
            return tmp;
        }
        #endregion

        #region 頁籤選項2-5星

        /// <summary>
        /// 頁籤選項-五星
        /// </summary>
        /// <returns></returns>
        public static List<BaseOptions> FiveStartTab()
        {
            var tmp = new List<BaseOptions>
            {
                new BaseOptions
                {
                    ID = 1, Name = "殺直選號碼", Code = "1"
                },
                new BaseOptions
                {
                    ID = 2, Name = "殺垃圾負試", Code = "2"
                },
                new BaseOptions
                {
                    ID = 3, Name = "殺兩碼", Code = "3"
                },
                new BaseOptions
                {
                    ID = 4, Name = "殺三碼", Code = "4"
                },
                new BaseOptions
                {
                    ID = 5, Name = "殺四碼", Code = "5"
                },
                new BaseOptions
                {
                    ID = 6, Name = "必出兩碼", Code = "6"
                },
                new BaseOptions
                {
                    ID = 7, Name = "必出三碼", Code = "7"
                },
                new BaseOptions
                {
                    ID = 8, Name = "必出四碼", Code = "8"
                },
                new BaseOptions
                {
                    ID = 9, Name = "組合定位殺", Code = "9"
                },
                new BaseOptions
                {
                    ID = 10, Name = "定為二三四碼組合", Code = "10"
                },
                new BaseOptions
                {
                    ID = 11, Name = "殺012路", Code = "11"
                },
                new BaseOptions
                {
                    ID = 12, Name = "交集功能", Code = "12"
                },
                new BaseOptions
                {
                    ID = 13, Name = "大底號碼", Code = "13"
                }
            };
            return tmp;
        }
        #endregion
    }
}
