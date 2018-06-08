using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Base;

namespace WpfAppTest
{
    public class CustomOption
    {
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
