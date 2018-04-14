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
                default:
                    strCode = "CQSSC";
                    break;
            }
            return strCode;
        }
    }
}
