using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTest.AP
{
    public class DB
    {
        /// <summary>
        /// 數字0-9
        /// </summary>
        /// <returns></returns>
        public static List<BaseOptions> ZeroToNine(int start = 0, int end = 9)
        {
            var tmp = new List<BaseOptions>();
            for (int i = start; i <= end; i++)
                tmp.Add(new BaseOptions { ID = i, Name = i.ToString(), Code = i.ToString() });
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
        /// 特別排除項目-5星
        /// </summary>
        /// <returns></returns>
        public static List<BaseOptions> SpecialExclude()
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

        /// <summary>
        /// 比例-reverse
        /// </summary>
        /// <returns></returns>
        public static List<BaseOptions> Ratio(int number)
        {
            var tmp = new List<BaseOptions>();
            int reverse = number;
            for (int i = 0; i <= number; i++)
            {
                tmp.Add(new BaseOptions { ID = i, Name = i.ToString() + "：" + reverse.ToString() });
                reverse--;
            }
            return tmp;
        }

        /// <summary>
        /// 5星Tab選項
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

        /// <summary>
        /// 數字組合
        /// </summary>
        /// <returns></returns>
        public static List<BaseOptions> CombinationAllNumber(int number, int start = 0, int end = 1)
        {
            var tmp = new List<BaseOptions>();
            if (number > 0 && start >= 0 && end >= 1)
            {
                char c = '0';
                char.TryParse(start.ToString(), out c);

                string comb = "".PadRight(number, c);
                char[] charArray;

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
                            comb = string.Join("", charArray);
                            tmp.Add(new WpfAppTest.BaseOptions { ID = i, Name = comb, Code = comb });
                            break;
                        }
                        else
                        {
                            charArray[j] = char.Parse(start.ToString());
                            if (j == 0)
                            {
                                comb = string.Join("", charArray);
                                tmp.Add(new WpfAppTest.BaseOptions { ID = i, Name = comb, Code = comb });
                            }
                            continue;
                        }
                    }
                }
            }
            return tmp;
        }

        /// <summary>
        /// 頁籤選項-二星
        /// </summary>
        /// <returns></returns>
        public static List<BaseOptions> TwoStartTab()
        {
            var tmp = new List<BaseOptions>
            {
                new BaseOptions
                {
                    ID = 1,
                    Name = "定位",
                    Code = "1"
                },
                new BaseOptions
                {
                    ID = 2,
                    Name = "定位形",
                    Code = "2"
                },
                new BaseOptions
                {
                    ID = 3,
                    Name = "不定位形",
                    Code = "3"
                },
                new BaseOptions
                {
                    ID = 4,
                    Name = "和值",
                    Code = "4"
                },
                new BaseOptions
                {
                    ID = 5,
                    Name = "合尾",
                    Code = "5"
                },
                new BaseOptions
                {
                    ID = 6,
                    Name = "跨度",
                    Code = "6"
                },
                new BaseOptions
                {
                    ID = 7,
                    Name = "和差",
                    Code = "7"
                },
                new BaseOptions
                {
                    ID = 8,
                    Name = "垃圾集合",
                    Code = "8"
                },
                new BaseOptions
                {
                    ID = 9,
                    Name = "大底集合",
                    Code = "9"
                }
            };
            return tmp;
        }
    }
}
