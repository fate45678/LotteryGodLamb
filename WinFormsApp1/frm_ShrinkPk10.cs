using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class frm_ShrinkPk10 : Form
    {
        JArray ja = null;
        Color color = new Color();
        public frm_ShrinkPk10()
        {
            InitializeComponent();
            color = btnChooseModeAll.BackColor;
        }

        private void btnStartWork_Click(object sender, EventArgs e)
        {
            string TableNmae = tabMakeNumber.SelectedTab.Name;
            DataTable dt = SelectDB(TableNmae);
            var str_json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            JArray ja = (JArray)JsonConvert.DeserializeObject(str_json);

            //組選號碼
            var ResultChooseModePara = ja.ToList();
            if (isTwoChoose)
            {
                if (ChooseModePara == "Big")
                {
                    ResultChooseModePara = ResultChooseModePara.Where(x => int.Parse(x["HUN"].ToString()) > 5 && int.Parse(x["TEN"].ToString()) > 5).ToList();
                }
                else if (ChooseModePara == "Small")
                {
                    ResultChooseModePara = ResultChooseModePara.Where(x => int.Parse(x["HUN"].ToString()) < 6 && int.Parse(x["TEN"].ToString()) < 6).ToList();
                }
                else if (ChooseModePara == "Odd")
                {
                    ResultChooseModePara = ResultChooseModePara.Where(x => (int.Parse(x["HUN"].ToString()) % 2) == 1 && (int.Parse(x["TEN"].ToString()) % 2) == 1).ToList();
                }
                else if (ChooseModePara == "Even")
                {
                    ResultChooseModePara = ResultChooseModePara.Where(x => (int.Parse(x["HUN"].ToString()) % 2) == 0 && (int.Parse(x["TEN"].ToString()) % 2) == 0).ToList();
                }
                else
                {
                    //ResultChooseModePara = ja;
                    if (ChooseModeTenPara != "" && ChooseModeTenPara != "0") //&& ChooseModeOnePara != "" && ChooseModeOnePara != "0" && ChooseModeTenPara != "" && ChooseModeTenPara != "0")
                    {
                        var ParaChooseModeTenPara = ChooseModeTenPara.Substring(1).Split(',');
                        for (int i = 0; i < ParaChooseModeTenPara.Count(); i++)
                        {
                            ResultChooseModePara = ResultChooseModePara
                                .Where(x => int.Parse(x["HUN"].ToString()) != int.Parse(ParaChooseModeTenPara[i])).ToList();
                        }

                        if (ChooseModeOnePara != "" && ChooseModeOnePara != "0")
                        {
                            var ParaChooseModeOnePara_in = ChooseModeOnePara.Substring(1).Split(',');
                            for (int i = 0; i < ParaChooseModeOnePara_in.Count(); i++)
                            {
                                ResultChooseModePara = ResultChooseModePara
                                    .Where(x => int.Parse(x["TEN"].ToString()) != int.Parse(ParaChooseModeOnePara_in[i])).ToList();
                            }
                        }                      
                    }
                    else if(ChooseModeOnePara != "" && ChooseModeOnePara != "0")
                    {
                        var ParaChooseModeOnePara_in = ChooseModeOnePara.Substring(1).Split(',');
                        for (int i = 0; i < ParaChooseModeOnePara_in.Count(); i++)
                        {
                            ResultChooseModePara = ResultChooseModePara
                                .Where(x => int.Parse(x["TEN"].ToString()) != int.Parse(ParaChooseModeOnePara_in[i])).ToList();
                        }
                    }
                }
            }
            else
            {
                if (ChooseModePara == "Big")
                {
                    ResultChooseModePara = ResultChooseModePara.Where(x => int.Parse(x["HUN"].ToString()) > 5 && int.Parse(x["TEN"].ToString()) > 5 && int.Parse(x["ONE"].ToString()) > 5).ToList();
                }
                else if (ChooseModePara == "Small")
                {
                    ResultChooseModePara = ResultChooseModePara.Where(x => int.Parse(x["HUN"].ToString()) < 6 && int.Parse(x["TEN"].ToString()) < 6 && int.Parse(x["ONE"].ToString()) < 6).ToList();
                }
                else if (ChooseModePara == "Odd")
                {
                    ResultChooseModePara = ResultChooseModePara.Where(x => (int.Parse(x["HUN"].ToString()) % 2) == 1 && (int.Parse(x["TEN"].ToString()) % 2) == 1 && (int.Parse(x["ONE"].ToString()) % 2) == 1).ToList();
                }
                else if (ChooseModePara == "Even")
                {
                    ResultChooseModePara = ResultChooseModePara.Where(x => (int.Parse(x["HUN"].ToString()) % 2) == 0 && (int.Parse(x["TEN"].ToString()) % 2) == 0 && (int.Parse(x["ONE"].ToString()) % 2) == 0).ToList();
                }
                else 
                {
                    if (ChooseModeHunPara != "" && ChooseModeHunPara != "0") //&& ChooseModeOnePara != "" && ChooseModeOnePara != "0" && ChooseModeTenPara != "" && ChooseModeTenPara != "0")
                    {
                        var ParaChooseModeHunPara = ChooseModeHunPara.Substring(1).Split(',');
                        for (int i = 0; i < ParaChooseModeHunPara.Count(); i++)
                        {
                            ResultChooseModePara = ResultChooseModePara
                                .Where(x => int.Parse(x["HUN"].ToString()) != int.Parse(ParaChooseModeHunPara[i])).ToList();
                        }

                        if (ChooseModeTenPara != "" && ChooseModeTenPara != "0")
                        {
                            var ParaChooseModeTenPara_in = ChooseModeTenPara.Substring(1).Split(',');
                            for (int i = 0; i < ParaChooseModeTenPara_in.Count(); i++)
                            {
                                ResultChooseModePara = ResultChooseModePara
                                    .Where(x => int.Parse(x["TEN"].ToString()) != int.Parse(ParaChooseModeTenPara_in[i])).ToList();
                            }

                            if (ChooseModeOnePara != "" && ChooseModeOnePara != "0")
                            {
                                var ParaChooseModeOnePara_in = ChooseModeOnePara.Substring(1).Split(',');
                                for (int i = 0; i < ParaChooseModeOnePara_in.Count(); i++)
                                {
                                    ResultChooseModePara = ResultChooseModePara
                                        .Where(x => int.Parse(x["ONE"].ToString()) != int.Parse(ParaChooseModeOnePara_in[i])).ToList();
                                }
                            }
                        }
                        else
                        {
                            if (ChooseModeOnePara != "" && ChooseModeOnePara != "0")
                            {
                                var ParaChooseModeOnePara_in = ChooseModeOnePara.Substring(1).Split(',');
                                for (int i = 0; i < ParaChooseModeOnePara_in.Count(); i++)
                                {
                                    ResultChooseModePara = ResultChooseModePara
                                        .Where(x => int.Parse(x["ONE"].ToString()) != int.Parse(ParaChooseModeOnePara_in[i])).ToList();
                                }
                            }
                        }
                    }
                    else if (ChooseModeTenPara != "" && ChooseModeTenPara != "0")
                    {
                        var ParaChooseModeTenPara_in = ChooseModeTenPara.Substring(1).Split(',');
                        for (int i = 0; i < ParaChooseModeTenPara_in.Count(); i++)
                        {
                            ResultChooseModePara = ResultChooseModePara
                                .Where(x => int.Parse(x["TEN"].ToString()) != int.Parse(ParaChooseModeTenPara_in[i])).ToList();
                        }

                        if (ChooseModeOnePara != "" && ChooseModeOnePara != "0")
                        {
                            var ParaChooseModeOnePara_in = ChooseModeOnePara.Substring(1).Split(',');
                            for (int i = 0; i < ParaChooseModeOnePara_in.Count(); i++)
                            {
                                ResultChooseModePara = ResultChooseModePara
                                    .Where(x => int.Parse(x["ONE"].ToString()) != int.Parse(ParaChooseModeOnePara_in[i])).ToList();
                            }
                        }
                    }
                    else if(ChooseModeOnePara != "" && ChooseModeOnePara != "0")
                    {
                        var ParaChooseModeOnePara = ChooseModeOnePara.Substring(1).Split(',');
                        for (int i = 0; i < ParaChooseModeOnePara.Count(); i++)
                        {
                            ResultChooseModePara = ResultChooseModePara
                                .Where(x => int.Parse(x["ONE"].ToString()) != int.Parse(ParaChooseModeOnePara[i])).ToList();
                        }
                    }                    
                }
            }


            //殺二碼組合
            var ResultKillTwoCombineFirst = ResultChooseModePara;
            if(selectKillTwoCombineFirstPara != "0" && selectKillTwoCombineFirstPara != "" && selectKillTwoCombineFirstPara != "0" && selectKillTwoCombineFirstPara != "")
            { 
                ResultChooseModePara.Where(x => x["HUN"].ToString() != selectKillTwoCombineFirstPara)
                                           .Where(x => x["TEN"].ToString() != selectKillTwoCombineSecPara).ToList();
            }
            //var ResultKillTwoCombineSec = ResultKillTwoCombineFirst.Where(x => x["TEN"].ToString() != selectKillTwoCombineSecPara).ToList();

            //殺二碼和
            var ResultKillTwoSum = ResultKillTwoCombineFirst;
            if (KillTwoSumPara != "0" && KillTwoSumPara != "")
            {
                var ParaKill012RoadPara = Kill012RoadPara.Substring(1).Split(',');
                for (int i = 0; i < ParaKill012RoadPara.Count(); i++)
                {
                    ResultKillTwoSum = ResultKillTwoSum
                                                .Where(x => int.Parse(x["HUN"].ToString()) + int.Parse(x["TEN"].ToString()) != int.Parse(ParaKill012RoadPara[i]))
                                                .Where(x => int.Parse(x["HUN"].ToString()) + int.Parse(x["ONE"].ToString()) != int.Parse(ParaKill012RoadPara[i]))
                                                .Where(x => int.Parse(x["ONE"].ToString()) + int.Parse(x["TEN"].ToString()) != int.Parse(ParaKill012RoadPara[i])).ToList();
                }
            }

            //殺二碼差
            var ResultKillTwoLess = ResultKillTwoSum;
            if (KillTwoLessPara != "0" && KillTwoLessPara != "")
            {
                var ParaKillTwoLessPara = KillTwoLessPara.Substring(1).Split(',');
                for (int i = 0; i < ParaKillTwoLessPara.Count(); i++)
                {
                    ResultKillTwoLess = ResultKillTwoLess
                                                .Where(x => Math.Abs(int.Parse(x["HUN"].ToString()) - int.Parse(x["TEN"].ToString())) != int.Parse(ParaKillTwoLessPara[i]))
                                                .Where(x => Math.Abs(int.Parse(x["HUN"].ToString()) - int.Parse(x["ONE"].ToString())) != int.Parse(ParaKillTwoLessPara[i]))
                                                .Where(x => Math.Abs(int.Parse(x["ONE"].ToString()) - int.Parse(x["TEN"].ToString())) != int.Parse(ParaKillTwoLessPara[i])).ToList();
                }
            }

            //殺012路
            var ResultKill012Road = ResultKillTwoLess;
            if (Kill012RoadPara != "0" && Kill012RoadPara != "")
            {
                if (isTwoRoad)
                {
                    var ParaKill012RoadPara = Kill012RoadPara.Substring(1).Split(',');
                    for (int i = 0; i < ParaKill012RoadPara.Count(); i++)
                    {
                        ResultKill012Road = ResultKill012Road
                                                   .Where(x => Convert.ToInt32(x["Number"].ToString().Substring(0, 2)) % 3 != int.Parse(ParaKill012RoadPara[i].Substring(0, 1))
                                                            || Convert.ToInt32(x["Number"].ToString().Substring(3, 2)) % 3 != int.Parse(ParaKill012RoadPara[i].Substring(1, 1))).ToList();
                    }
                }
                else
                { 
                    var ParaKill012RoadPara = Kill012RoadPara.Substring(1).Split(',');
                    for (int i = 0; i < ParaKill012RoadPara.Count(); i++)
                    {
                        //var iii = int.Parse(ParaKill012RoadPara[i].Substring(0, 1));
                        ResultKill012Road = ResultKill012Road
                                                   .Where(x => Convert.ToInt32(x["Number"].ToString().Substring(0, 2)) % 3 != int.Parse(ParaKill012RoadPara[i].Substring(0, 1))
                                                           || Convert.ToInt32(x["Number"].ToString().Substring(3, 2)) % 3 != int.Parse(ParaKill012RoadPara[i].Substring(1, 1))
                                                           || Convert.ToInt32(x["Number"].ToString().Substring(6, 2)) % 3 != int.Parse(ParaKill012RoadPara[i].Substring(2, 1))).ToList();
                    }
                }
            }

            //殺奇偶
            var ResultKillOddEven = ResultKill012Road;
            if (KillOddEvenPara != "0" && KillOddEvenPara != "")
            {
                if(isTwoOddEven)
                {
                    var ParaKillOddEven = KillOddEvenPara.Substring(1).Split(',');
                    for (int i = 0; i < ParaKillOddEven.Count(); i++)
                    {
                        ResultKillOddEven = ResultKillOddEven
                                                   .Where(x => Convert.ToInt32(x["HUN"]) % 2 != int.Parse(ParaKillOddEven[i].Substring(0, 1)))
                                                   .Where(y => Convert.ToInt32(y["TEN"]) % 2 != int.Parse(ParaKillOddEven[i].Substring(1, 1))).ToList();
                    }
                }
                else
                { 
                    var ParaKillOddEven = KillOddEvenPara.Substring(1).Split(',');
                    for (int i = 0; i < ParaKillOddEven.Count(); i++)
                    {
                        ResultKillOddEven = ResultKillOddEven
                                                   .Where(x => Convert.ToInt32(x["HUN"]) % 2 != int.Parse(ParaKillOddEven[i].Substring(0, 1)))
                                                   .Where(y => Convert.ToInt32(y["TEN"]) % 2 != int.Parse(ParaKillOddEven[i].Substring(1, 1)))
                                                   .Where(z => Convert.ToInt32(z["ONE"]) % 2 != int.Parse(ParaKillOddEven[i].Substring(2, 1))).ToList();
                    }
                }
            }

            //殺大小
            var ResultKillBigSmall = ResultKillOddEven;
            if (KillBigSmallPara != "0" && KillBigSmallPara != "")
            {
                var ParaKillBigSmallPara = KillBigSmallPara.Substring(1).Split(',');
                for (int i = 0; i < ParaKillBigSmallPara.Count(); i++)
                {
                    switch (ParaKillBigSmallPara[i].ToString())
                    {
                        case "555":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUN"].ToString()) <= 5 && int.Parse(x["TEN"].ToString()) <= 5 && int.Parse(x["ONE"].ToString()) < 5).ToList();
                            break;
                        case "556":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUN"].ToString()) <= 5 && int.Parse(x["TEN"].ToString()) <= 5 && int.Parse(x["ONE"].ToString()) > 5).ToList();
                            break;
                        case "565":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUN"].ToString()) <= 5 && int.Parse(x["TEN"].ToString()) > 5 && int.Parse(x["ONE"].ToString()) < 5).ToList();
                            break;
                        case "566":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUN"].ToString()) <= 5 && int.Parse(x["TEN"].ToString()) > 5 && int.Parse(x["ONE"].ToString()) > 5).ToList();
                            break;
                        case "666":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUN"].ToString()) > 5 && int.Parse(x["TEN"].ToString()) > 5 && int.Parse(x["ONE"].ToString()) > 5).ToList();
                            break;
                        case "655":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUN"].ToString()) > 5 && int.Parse(x["TEN"].ToString()) <= 5 && int.Parse(x["ONE"].ToString()) <= 5).ToList();
                            break;
                        case "656":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUN"].ToString()) < 5 && int.Parse(x["TEN"].ToString()) < 5 && int.Parse(x["ONE"].ToString()) < 5).ToList();
                            break;
                        case "665":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUN"].ToString()) > 5 && int.Parse(x["TEN"].ToString()) <= 5 && int.Parse(x["ONE"].ToString()) > 5).ToList();
                            break;
                        case "66":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUN"].ToString()) <= 5 && int.Parse(x["TEN"].ToString()) <= 5).ToList();
                            break;
                        case "65":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUN"].ToString()) <= 5 && int.Parse(x["TEN"].ToString()) > 5).ToList();
                            break;
                        case "56":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUN"].ToString()) > 5 && int.Parse(x["TEN"].ToString()) <= 5).ToList();
                            break;
                        case "55":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUN"].ToString()) > 5 && int.Parse(x["TEN"].ToString()) > 5).ToList();
                            break;
                    }
                }
            }

            //殺13位差
            var ResultKill13Less = ResultKillBigSmall;
            if (Kill13LessPara != "0" && Kill13LessPara != "")
            {
                var ParaKill13LessPara = Kill13LessPara.Substring(1).Split(',');
                for (int i = 0; i < ParaKill13LessPara.Count(); i++)
                {
                    ResultKill13Less = ResultKill13Less
                                           .Where(x => Math.Abs(int.Parse(x["HUN"].ToString()) - int.Parse(x["ONE"].ToString())) != int.Parse(ParaKill13LessPara[i])).ToList();
                }
            }

            //殺23位差
            var ResultKill23Less = ResultKill13Less;
            if (Kill23LessPara != "0" && Kill23LessPara != "")
            {
                var ParaKill23LessPara = Kill23LessPara.Substring(1).Split(',');
                for (int i = 0; i < ParaKill23LessPara.Count(); i++)
                {
                    ResultKill23Less = ResultKill23Less
                                           .Where(x => Math.Abs(int.Parse(x["TEN"].ToString()) - int.Parse(x["ONE"].ToString())) != int.Parse(ParaKill23LessPara[i])).ToList();
                }
            }

            //殺12位差
            var ResultKill12Less = ResultKill23Less;
            if (Kill12LessPara != "0" && Kill12LessPara != "")
            {
                var ParaKill12LessPara = Kill12LessPara.Substring(1).Split(',');
                for (int i = 0; i < ParaKill12LessPara.Count(); i++)
                {
                    ResultKill12Less = ResultKill12Less
                                           .Where(x => Math.Abs(int.Parse(x["HUN"].ToString()) - int.Parse(x["TEN"].ToString())) != int.Parse(ParaKill12LessPara[i])).ToList();
                }
            }

            //殺13碼和
            var ResultKill13Sum = ResultKill12Less;
            if (Kill13SumPara != "0" && Kill13SumPara != "")
            {
                var ParaKill13SumPara = Kill13SumPara.Substring(1).Split(',');
                for (int i = 0; i < ParaKill13SumPara.Count(); i++)
                {
                    ResultKill13Sum = ResultKill13Sum
                                               .Where(x => int.Parse(x["HUN"].ToString()) + int.Parse(x["ONE"].ToString()) != int.Parse(ParaKill13SumPara[i])).ToList();
                }
            }

            //殺23碼和
            var ResultKill23Sum = ResultKill13Sum;
            if (Kill23SumPara != "0" && Kill23SumPara != "")
            {
                var ParaKill23SumPara = Kill23SumPara.Substring(1).Split(',');
                for (int i = 0; i < ParaKill23SumPara.Count(); i++)
                {
                    ResultKill23Sum = ResultKill23Sum
                                               .Where(x => int.Parse(x["HUN"].ToString()) + int.Parse(x["ONE"].ToString()) != int.Parse(ParaKill23SumPara[i])).ToList();
                }
            }

            //殺12碼和
            var ResultKill12Sum = ResultKill23Sum;
            if (Kill12SumPara != "0" && Kill12SumPara != "")
            {
                var ParaKill12SumPara = Kill12SumPara.Substring(1).Split(',');
                for (int i = 0; i < ParaKill12SumPara.Count(); i++)
                {
                    ResultKill12Sum = ResultKill12Sum
                                               .Where(x => int.Parse(x["HUN"].ToString()) + int.Parse(x["TEN"].ToString()) != int.Parse(ParaKill12SumPara[i])).ToList();
                }
            }

            //殺型態號
            var ResultKillType = ResultKill12Sum;
            if (KillTypeNumberPara != "0" && KillTypeNumberPara != "")
            {
                var ParaKillTypeNumberPara = KillTypeNumberPara.Substring(1).Split(',');
                for (int i = 0; i < ParaKillTypeNumberPara.Count(); i++)
                {
                    switch (ParaKillTypeNumberPara[i].ToString())
                    {
                        case "1": //上階梯 021,03  01,02,03
                            ResultKillType = ResultKillType
                                                .Where(x => int.Parse(x["HUN"].ToString()) > int.Parse(x["TEN"].ToString())
                                                || int.Parse(x["TEN"].ToString()) > int.Parse(x["ONE"].ToString())
                                                || int.Parse(x["HUN"].ToString()) > int.Parse(x["ONE"].ToString()))
                                                .ToList();

                            break;
                        case "2": //下階梯
                            ResultKillType = ResultKillType
                                               .Where(x => int.Parse(x["HUN"].ToString()) < int.Parse(x["TEN"].ToString())
                                                || int.Parse(x["TEN"].ToString()) < int.Parse(x["ONE"].ToString())
                                                || int.Parse(x["HUN"].ToString()) < int.Parse(x["ONE"].ToString()))
                                                .ToList();
                            break;
                        case "3": //凸型
                            ResultKillType = ResultKillType
                                               .Where(x => int.Parse(x["TEN"].ToString()) < int.Parse(x["HUN"].ToString())
                                                || int.Parse(x["TEN"].ToString()) < int.Parse(x["ONE"].ToString()))
                                               .ToList();
                            break;
                        case "4": //凹型
                            ResultKillType = ResultKillType
                                                .Where(x => int.Parse(x["TEN"].ToString()) > int.Parse(x["HUN"].ToString())
                                                || int.Parse(x["TEN"].ToString()) > int.Parse(x["ONE"].ToString()))
                                               .ToList();
                            break;
                        case "5": //三連號 01 02 03 , 02 03 04
                            ResultKillType = ResultKillType
                                                .Where(x => int.Parse(x["HUN"].ToString()) + 2 != int.Parse(x["TEN"].ToString()) + 1 && int.Parse(x["TEN"].ToString()) + 1 != int.Parse(x["ONE"].ToString()))
                                                .Where(x => int.Parse(x["ONE"].ToString()) + 2 != int.Parse(x["HUN"].ToString()) + 1 && int.Parse(x["TEN"].ToString()) + 1 != int.Parse(x["HUN"].ToString())).ToList();
                            break;
                        case "6": //二連號01 02 04 , 04 02 01 
                            ResultKillType = ResultKillType
                                                .Where(x => int.Parse(x["HUN"].ToString()) + 1 != int.Parse(x["TEN"].ToString())
                                                         && int.Parse(x["HUN"].ToString()) - 1 != int.Parse(x["TEN"].ToString())
                                                         && int.Parse(x["TEN"].ToString()) + 1 != int.Parse(x["ONE"].ToString())
                                                         && int.Parse(x["TEN"].ToString()) - 1 != int.Parse(x["ONE"].ToString())).ToList();
                            break;
                    }
                }
            }

            //殺和值
            var ResultKillSum = ResultKillType;
            if (KillSumPara != "0" && KillSumPara != "")
            {
                if (isTwoSum)
                {
                    var ParaKillSumPara = KillSumPara.Substring(1).Split(',');
                    for (int i = 0; i < ParaKillSumPara.Count(); i++)
                    {
                        ResultKillSum = ResultKillSum
                                            .Where(x => int.Parse(x["HUN"].ToString()) + int.Parse(x["TEN"].ToString()) != int.Parse(ParaKillSumPara[i])).ToList();
                    }
                }
                else
                { 
                    var ParaKillSumPara = KillSumPara.Substring(1).Split(',');
                    for (int i = 0; i < ParaKillSumPara.Count(); i++)
                    {
                        ResultKillSum = ResultKillSum
                                            .Where(x => int.Parse(x["HUN"].ToString()) + int.Parse(x["TEN"].ToString()) + int.Parse(x["ONE"].ToString()) != int.Parse(ParaKillSumPara[i])).ToList();
                    }
                }
            }

            //殺跨度
            var ResultKillCross = ResultKillSum;
            if (KillCrossPara != "0" && KillCrossPara != "")
            {
                if (isTwoCross)
                {
                    var ParaKillCrossPara = KillCrossPara.Substring(1).Split(',');
                    for (int i = 0; i < ParaKillCrossPara.Count(); i++)
                    {
                        ResultKillCross = ResultKillCross
                                            .Where(x => Math.Abs(int.Parse(x["HUN"].ToString()) - int.Parse(x["TEN"].ToString())) != int.Parse(ParaKillCrossPara[i]))
                                            .ToList();
                    }
                }
                else
                {
                    var ParaKillCrossPara = KillCrossPara.Substring(1).Split(',');
                    for (int i = 0; i < ParaKillCrossPara.Count(); i++)
                    {

                        ResultKillCross = ResultKillCross
                                            .Where(x => Math.Abs(int.Parse(x["HUN"].ToString()) - int.Parse(x["TEN"].ToString())) != int.Parse(ParaKillCrossPara[i]))
                                            .Where(x => Math.Abs(int.Parse(x["HUN"].ToString()) - int.Parse(x["ONE"].ToString())) != int.Parse(ParaKillCrossPara[i]))
                                            .Where(x => Math.Abs(int.Parse(x["TEN"].ToString()) - int.Parse(x["HUN"].ToString())) != int.Parse(ParaKillCrossPara[i]))
                                            .Where(x => Math.Abs(int.Parse(x["TEN"].ToString()) - int.Parse(x["ONE"].ToString())) != int.Parse(ParaKillCrossPara[i]))
                                            .Where(x => Math.Abs(int.Parse(x["ONE"].ToString()) - int.Parse(x["TEN"].ToString())) != int.Parse(ParaKillCrossPara[i]))
                                            .Where(x => Math.Abs(int.Parse(x["ONE"].ToString()) - int.Parse(x["HUN"].ToString())) != int.Parse(ParaKillCrossPara[i]))
                                            .ToList();
                    }
                }
                
            }

            //膽碼
            var ResultLocal = ResultKillType;
            if ((isKillPara != "0" && LocalNumber0LocalPara != "0") && (isKillPara != "" && LocalNumber0LocalPara != ""))
            {
                if (isTwoLocalNumber)
                {
                    var ParaLocalNumber0LocalPara = LocalNumber0LocalPara.Substring(1).Split(',');
                    for (int i = 0; i < ParaLocalNumber0LocalPara.Count(); i++)
                    {
                        if (isKillPara == "Kill")
                        {
                            ResultKillCross = ResultKillCross
                                                .Where(x => int.Parse(x["HUN"].ToString()) != int.Parse(ParaLocalNumber0LocalPara[i])
                                                         || int.Parse(x["TEN"].ToString()) != int.Parse(ParaLocalNumber0LocalPara[i])).ToList();
                        }
                        else
                        {
                            ResultKillCross = ResultKillCross
                                                .Where(x => int.Parse(x["HUN"].ToString()) == int.Parse(ParaLocalNumber0LocalPara[i])
                                                         || int.Parse(x["TEN"].ToString()) == int.Parse(ParaLocalNumber0LocalPara[i])).ToList();
                        }
                    }
                }
                else
                { 
                    var ParaLocalNumber0LocalPara = LocalNumber0LocalPara.Substring(1).Split(',');
                    for (int i = 0; i < ParaLocalNumber0LocalPara.Count(); i++)
                    {

                        if (isKillPara == "Kill")
                        {
                            ResultKillCross = ResultKillCross
                                                .Where(x => int.Parse(x["HUN"].ToString()) != int.Parse(ParaLocalNumber0LocalPara[i])
                                                         || int.Parse(x["TEN"].ToString()) != int.Parse(ParaLocalNumber0LocalPara[i])
                                                         || int.Parse(x["ONE"].ToString()) != int.Parse(ParaLocalNumber0LocalPara[i])).ToList();
                        }
                        else
                        {
                            ResultKillCross = ResultKillCross
                                                .Where(x => int.Parse(x["HUN"].ToString()) == int.Parse(ParaLocalNumber0LocalPara[i])
                                || int.Parse(x["TEN"].ToString()) == int.Parse(ParaLocalNumber0LocalPara[i])
                                || int.Parse(x["ONE"].ToString()) == int.Parse(ParaLocalNumber0LocalPara[i])).ToList();
                        }
                    }
                }
            }


            //最後處理得再補上
            string Result = "";
            foreach (var item in ResultKillCross)
            {
                Result += " ," + item["Number"];
            }
            Result = " " + Result.Substring(2);
            if(TableNmae.Contains("MakeTwo"))
            {
                rtbTwoResult.Text = Result;
            }
            else
            { 
                rtbResultNumber.Text = Result;
            }
            //var iii = ResultKillTwoCombineSec.ToString();

        }

        private DataTable SelectDB(string tableName)
        {
            string Table = "PK10WaterForThree";
            string Sqlstr = @"select Number ,substring(Number, 0,3) AS HUN ,substring(Number, 3,3)AS TEN,substring(Number, 6,3)AS ONE From " + Table;

            //二星作號
            if (tableName.Contains("Two"))
            {
                Table = "PK10WaterForTwo";
                Sqlstr = @"select Number ,substring(Number, 0,3) AS HUN ,substring(Number, 3,3)AS TEN From " + Table;
            }
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";
            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
            con = new SqlConnection(connetionString);

            try
            {
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(Sqlstr, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                return null;
            }
        }


        #region 殺二碼組合

        string selectKillTwoCombineFirstPara = "0";
        string selectKillTwoCombineSecPara = "0";
        //bool isTwoKillTwoCombine = false;

        private void KillTwoCombineFisrt_ButtomClick(object sender, EventArgs e)
        {
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnKillTwoCombine", "");

            if (Buttom.ForeColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                selectKillTwoCombineFirstPara += "," + Click;
            }
            else
            {
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                selectKillTwoCombineFirstPara = selectKillTwoCombineFirstPara.Replace("," + Click, "");
            }
        }

        private void KillTwoCombineSec_ButtomClick(object sender, EventArgs e)
        {
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnKillTwoCombineSec", "");

            if (Buttom.ForeColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                selectKillTwoCombineSecPara += "," + Click;
            }
            else
            {
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                selectKillTwoCombineSecPara = selectKillTwoCombineSecPara.Replace("," + Click, "");
            }
        }


        #endregion

        #region 殺二碼和
        string KillTwoSumPara = "0";
        private void KillTwoSum_ButtomClick(object sender, EventArgs e)
        {
            if (KillTwoSumPara == "0")
                KillTwoSumPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnKillTwoSum", "");

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;               
                KillTwoSumPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                KillTwoSumPara = KillTwoSumPara.Replace("," + Click, "");
            }
        }

        #endregion

        #region 殺二碼差
        string KillTwoLessPara = "0";
        private void KillTwoLess_ButtomClick(object sender, EventArgs e)
        {
            if (KillTwoLessPara == "0")
                KillTwoLessPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("KillTwoLess", "");

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                KillTwoLessPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                KillTwoLessPara = KillTwoLessPara.Replace("," + Click, "");
            }
        }

        #endregion

        //色碼修改
        public Color HexColor(String hex)
        {
            //將井字號移除
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;
            int start = 0;

            //處理ARGB字串 
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            // 將RGB文字轉成byte
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }

        #region 選號模式
        string ChooseModePara = "";
        //全選
        private void btnChooseModeAll_Click(object sender, EventArgs e)
        {
            isTwoChoose = false;

            btnChooseModeAll.BackColor = Color.OrangeRed;
            //把這框內的選擇都恢復
            btnChooseModeOneBig.BackColor = color;
            btnChooseModeOneSmall.BackColor = color;
            btnChooseModeOneOdd.BackColor = color;
            btnChooseModeOneEven.BackColor = color;

            //百位
            btnChooseModeHundred01.BackColor = Color.OrangeRed;
            btnChooseModeHundred02.BackColor = Color.OrangeRed;
            btnChooseModeHundred03.BackColor = Color.OrangeRed;
            btnChooseModeHundred04.BackColor = Color.OrangeRed;
            btnChooseModeHundred05.BackColor = Color.OrangeRed;
            btnChooseModeHundred06.BackColor = Color.OrangeRed;
            btnChooseModeHundred07.BackColor = Color.OrangeRed;
            btnChooseModeHundred08.BackColor = Color.OrangeRed;
            btnChooseModeHundred09.BackColor = Color.OrangeRed;
            btnChooseModeHundred10.BackColor = Color.OrangeRed;

            //十位
            btnChooseModeTen01.BackColor = Color.OrangeRed;
            btnChooseModeTen02.BackColor = Color.OrangeRed;
            btnChooseModeTen03.BackColor = Color.OrangeRed;
            btnChooseModeTen04.BackColor = Color.OrangeRed;
            btnChooseModeTen05.BackColor = Color.OrangeRed;
            btnChooseModeTen06.BackColor = Color.OrangeRed;
            btnChooseModeTen07.BackColor = Color.OrangeRed;
            btnChooseModeTen08.BackColor = Color.OrangeRed;
            btnChooseModeTen09.BackColor = Color.OrangeRed;
            btnChooseModeTen10.BackColor = Color.OrangeRed;

            //個位
            btnChooseModeOne01.BackColor = Color.OrangeRed;
            btnChooseModeOne02.BackColor = Color.OrangeRed;
            btnChooseModeOne03.BackColor = Color.OrangeRed;
            btnChooseModeOne04.BackColor = Color.OrangeRed;
            btnChooseModeOne05.BackColor = Color.OrangeRed;
            btnChooseModeOne06.BackColor = Color.OrangeRed;
            btnChooseModeOne07.BackColor = Color.OrangeRed;
            btnChooseModeOne08.BackColor = Color.OrangeRed;
            btnChooseModeOne09.BackColor = Color.OrangeRed;
            btnChooseModeOne10.BackColor = Color.OrangeRed;

            ChooseModePara = "All";
        }

        //選大
        private void btnChooseModeOneBig_Click(object sender, EventArgs e)
        {
            isTwoChoose = false;

            ClearAllForChooseMode();
            btnChooseModeOneBig.BackColor = Color.OrangeRed;
            //把這框內的選擇都恢復
            btnChooseModeAll.BackColor = color;
            btnChooseModeOneSmall.BackColor = color;
            btnChooseModeOneOdd.BackColor = color;
            btnChooseModeOneEven.BackColor = color;

            //百位
            btnChooseModeHundred06.BackColor = Color.OrangeRed;
            btnChooseModeHundred07.BackColor = Color.OrangeRed;
            btnChooseModeHundred08.BackColor = Color.OrangeRed;
            btnChooseModeHundred09.BackColor = Color.OrangeRed;
            btnChooseModeHundred10.BackColor = Color.OrangeRed;

            //十位
            btnChooseModeTen06.BackColor = Color.OrangeRed;
            btnChooseModeTen07.BackColor = Color.OrangeRed;
            btnChooseModeTen08.BackColor = Color.OrangeRed;
            btnChooseModeTen09.BackColor = Color.OrangeRed;
            btnChooseModeTen10.BackColor = Color.OrangeRed;

            //個位
            btnChooseModeOne06.BackColor = Color.OrangeRed;
            btnChooseModeOne07.BackColor = Color.OrangeRed;
            btnChooseModeOne08.BackColor = Color.OrangeRed;
            btnChooseModeOne09.BackColor = Color.OrangeRed;
            btnChooseModeOne10.BackColor = Color.OrangeRed;

            ChooseModePara = "Big";
        }


        //選小
        private void btnChooseModeOneSmall_Click(object sender, EventArgs e)
        {
            isTwoChoose = false;

            ClearAllForChooseMode();
            btnChooseModeOneSmall.BackColor = Color.OrangeRed;
            //把這框內的選擇都恢復
            btnChooseModeOneBig.BackColor = color;
            btnChooseModeAll.BackColor = color;
            btnChooseModeOneOdd.BackColor = color;
            btnChooseModeOneEven.BackColor = color;

            //百位
            btnChooseModeHundred01.BackColor = Color.OrangeRed;
            btnChooseModeHundred02.BackColor = Color.OrangeRed;
            btnChooseModeHundred03.BackColor = Color.OrangeRed;
            btnChooseModeHundred04.BackColor = Color.OrangeRed;
            btnChooseModeHundred05.BackColor = Color.OrangeRed;

            //十位
            btnChooseModeTen01.BackColor = Color.OrangeRed;
            btnChooseModeTen02.BackColor = Color.OrangeRed;
            btnChooseModeTen03.BackColor = Color.OrangeRed;
            btnChooseModeTen04.BackColor = Color.OrangeRed;
            btnChooseModeTen05.BackColor = Color.OrangeRed;

            //個位
            btnChooseModeOne01.BackColor = Color.OrangeRed;
            btnChooseModeOne02.BackColor = Color.OrangeRed;
            btnChooseModeOne03.BackColor = Color.OrangeRed;
            btnChooseModeOne04.BackColor = Color.OrangeRed;
            btnChooseModeOne05.BackColor = Color.OrangeRed;

            ChooseModePara = "Small";
        }

        //選奇數
        private void btnChooseModeOneOdd_Click(object sender, EventArgs e)
        {
            isTwoChoose = false;

            ClearAllForChooseMode();
            btnChooseModeOneOdd.BackColor = Color.OrangeRed;
            //把這框內的選擇都恢復
            btnChooseModeOneBig.BackColor = color;
            btnChooseModeOneSmall.BackColor = color;
            btnChooseModeAll.BackColor = color;
            btnChooseModeOneEven.BackColor = color;

            //百位
            btnChooseModeHundred01.BackColor = Color.OrangeRed;
            btnChooseModeHundred03.BackColor = Color.OrangeRed;
            btnChooseModeHundred05.BackColor = Color.OrangeRed;
            btnChooseModeHundred07.BackColor = Color.OrangeRed;
            btnChooseModeHundred09.BackColor = Color.OrangeRed;

            //十位
            btnChooseModeTen01.BackColor = Color.OrangeRed;
            btnChooseModeTen03.BackColor = Color.OrangeRed;
            btnChooseModeTen05.BackColor = Color.OrangeRed;
            btnChooseModeTen07.BackColor = Color.OrangeRed;
            btnChooseModeTen09.BackColor = Color.OrangeRed;

            //個位
            btnChooseModeOne01.BackColor = Color.OrangeRed;
            btnChooseModeOne03.BackColor = Color.OrangeRed;
            btnChooseModeOne05.BackColor = Color.OrangeRed;
            btnChooseModeOne07.BackColor = Color.OrangeRed;
            btnChooseModeOne09.BackColor = Color.OrangeRed;

            ChooseModePara = "Odd";
        }

        //選偶數
        private void btnChooseModeOneEven_Click(object sender, EventArgs e)
        {
            isTwoChoose = false;
            ClearAllForChooseMode();
            btnChooseModeOneEven.BackColor = Color.OrangeRed;
            //把這框內的選擇都恢復
            btnChooseModeOneBig.BackColor = color;
            btnChooseModeOneSmall.BackColor = color;
            btnChooseModeOneOdd.BackColor = color;
            btnChooseModeAll.BackColor = color;

            //百位
            btnChooseModeHundred02.BackColor = Color.OrangeRed;
            btnChooseModeHundred04.BackColor = Color.OrangeRed;
            btnChooseModeHundred06.BackColor = Color.OrangeRed;
            btnChooseModeHundred08.BackColor = Color.OrangeRed;
            btnChooseModeHundred10.BackColor = Color.OrangeRed;

            //十位
            btnChooseModeTen02.BackColor = Color.OrangeRed;
            btnChooseModeTen04.BackColor = Color.OrangeRed;
            btnChooseModeTen06.BackColor = Color.OrangeRed;
            btnChooseModeTen08.BackColor = Color.OrangeRed;
            btnChooseModeTen10.BackColor = Color.OrangeRed;

            //個位
            btnChooseModeOne02.BackColor = Color.OrangeRed;
            btnChooseModeOne04.BackColor = Color.OrangeRed;
            btnChooseModeOne06.BackColor = Color.OrangeRed;
            btnChooseModeOne08.BackColor = Color.OrangeRed;
            btnChooseModeOne10.BackColor = Color.OrangeRed;

            ChooseModePara = "Even";
        }

        //清除
        private void ClearAllForChooseMode()
        {
            //把這框內的選擇都恢復
            btnChooseModeAll.BackColor = color;
            btnChooseModeOneBig.BackColor = color;
            btnChooseModeOneSmall.BackColor = color;
            btnChooseModeOneOdd.BackColor = color;
            btnChooseModeOneEven.BackColor = color;

            //百位
            btnChooseModeHundred01.BackColor = color;
            btnChooseModeHundred02.BackColor = color;
            btnChooseModeHundred03.BackColor = color; ;
            btnChooseModeHundred04.BackColor = color; ;
            btnChooseModeHundred05.BackColor = color; ;
            btnChooseModeHundred06.BackColor = color; ;
            btnChooseModeHundred07.BackColor = color; ;
            btnChooseModeHundred08.BackColor = color; ;
            btnChooseModeHundred09.BackColor = color; ;
            btnChooseModeHundred10.BackColor = color; ;

            //十位
            btnChooseModeTen01.BackColor = color; ;
            btnChooseModeTen02.BackColor = color; ;
            btnChooseModeTen03.BackColor = color; ;
            btnChooseModeTen04.BackColor = color; ;
            btnChooseModeTen05.BackColor = color; ;
            btnChooseModeTen06.BackColor = color; ;
            btnChooseModeTen07.BackColor = color; ;
            btnChooseModeTen08.BackColor = color; ;
            btnChooseModeTen09.BackColor = color; ;
            btnChooseModeTen10.BackColor = color; ;

            //個位
            btnChooseModeOne01.BackColor = color; ;
            btnChooseModeOne02.BackColor = color; ;
            btnChooseModeOne03.BackColor = color; ;
            btnChooseModeOne04.BackColor = color; ;
            btnChooseModeOne05.BackColor = color; ;
            btnChooseModeOne06.BackColor = color; ;
            btnChooseModeOne07.BackColor = color; ;
            btnChooseModeOne08.BackColor = color; ;
            btnChooseModeOne09.BackColor = color; ;
            btnChooseModeOne10.BackColor = color; ;
        }

        private void btnChooseModeOneClear_Click(object sender, EventArgs e)
        {
            ClearAllForChooseMode();
            ChooseModePara = "";

        }

        //單點百位
        string ChooseModeHunPara = "0";
        //bool isTwoChooseMode = false;
        private void btnChooseModeHundred_Click(object sender, EventArgs e)
        {
            isTwoChoose = false;
            if (ChooseModeHunPara == "0")
                ChooseModeHunPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnChooseModeHundred", "");

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                ChooseModeHunPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                ChooseModeHunPara = ChooseModeHunPara.Replace("," + Click, "");
            }
        }

        //單點十位
        string ChooseModeTenPara = "0";
        private void btnChooseModeTen_Click(object sender, EventArgs e)
        {
            isTwoChoose = false;
            if (ChooseModeTenPara == "0")
                ChooseModeTenPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnChooseModeTen", "");
            if (Click.Contains("TwoChooseModeTen"))
            {
                isTwoChoose = true;
                Click = Buttom.Name.Replace("btnTwoChooseModeTen", "");
            }

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                ChooseModeTenPara += "," + Click;
            }
            else
            {
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                ChooseModeTenPara = ChooseModeTenPara.Replace("," + Click, "");
            }
        }

        //單點個位
        string ChooseModeOnePara = "0";
        private void btnChooseModeOne_Click(object sender, EventArgs e)
        {
            isTwoChoose = false;
            if (ChooseModeOnePara == "0")
                ChooseModeOnePara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnChooseModeOne", "");
            if (Click.Contains("TwoChooseModeOne"))
            {
                isTwoChoose = true;
                Click = Buttom.Name.Replace("btnTwoChooseModeOne", "");
            }

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                ChooseModeOnePara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                ChooseModeOnePara = ChooseModeOnePara.Replace("," + Click, "");
            }
        }

        #endregion

        #region 殺012路
        string Kill012RoadPara = "0";
        bool isTwoRoad = false;
        private void Kill012Road_ButtomClick(object sender, EventArgs e)
        {
            isTwoRoad = false;
            if (Kill012RoadPara == "0")
                Kill012RoadPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnKill02Road", "");
            if (Click.Contains("TwoChooseModeOne"))
            {
                isTwoChoose = true;
                Click = Buttom.Name.Replace("btnTwoChooseModeOne", "");
            }

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                Kill012RoadPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                Kill012RoadPara = Kill012RoadPara.Replace("," + Click, "");
            }
        }

        #endregion

        #region 殺奇偶
        string KillOddEvenPara = "0";
        bool isTwoOddEven = false;
        private void KillOddEven_ButtomClick(object sender, EventArgs e)
        {
            isTwoOddEven = false;
            if (KillOddEvenPara == "0")
                KillOddEvenPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnKillOddEven", "");
            if (Click.Contains("TwoKillOddEven"))
            {
                isTwoOddEven = true;
                Click = Buttom.Name.Replace("btnTwoKillOddEven", "");
            }

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                KillOddEvenPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                Kill012RoadPara = Kill012RoadPara.Replace("," + Click, "");
            }
        }

        #endregion

        #region 殺大小
        string KillBigSmallPara = "0";
        private void KillBigSmall_ButtomClick(object sender, EventArgs e)
        {
            if (KillBigSmallPara == "0")
                KillBigSmallPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("KillBigSmall", "");
            if(Click.Contains("Two"))
                Click = Buttom.Name.Replace("btnTwoKillBigSmall", "");//btnTwoKillBigSmall65

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                KillBigSmallPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                KillBigSmallPara = KillBigSmallPara.Replace("," + Click, "");
            }
        }
        #endregion

        #region 殺13位差
        string Kill13LessPara = "0";
        private void Kill13Less_ButtomClick(object sender, EventArgs e)
        {
            if (Kill13LessPara == "0")
                Kill13LessPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnKill13Less", "");

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                Kill13LessPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                Kill13LessPara = Kill13LessPara.Replace("," + Click, "");
            }
        }
        #endregion

        #region 殺23位差
        string Kill23LessPara = "0";
        private void Kill23Less_ButtomClick(object sender, EventArgs e)
        {
            if (Kill23LessPara == "0")
                Kill23LessPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnKill23Less", "");

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                Kill23LessPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                Kill23LessPara = Kill23LessPara.Replace("," + Click, "");
            }
        }
        #endregion

        #region 殺12位差
        string Kill12LessPara = "0";
        private void Kill12Less_ButtomClick(object sender, EventArgs e)
        {
            if (Kill12LessPara == "0")
                Kill12LessPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnKill12Less", "");

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                Kill12LessPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                Kill12LessPara = Kill12LessPara.Replace("," + Click, "");
            }
        }
        #endregion

        #region 殺13位和
        string Kill13SumPara = "0";
        private void Kill13Sum_ButtomClick(object sender, EventArgs e)
        {
            if (Kill13SumPara == "0")
                Kill13SumPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnKill13Sum", "");

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                Kill13SumPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                Kill13SumPara = Kill13SumPara.Replace("," + Click, "");
            }
        }
        #endregion

        #region 殺23位和
        string Kill23SumPara = "0";
        private void Kill23Sum_ButtomClick(object sender, EventArgs e)
        {
            if (Kill23SumPara == "0")
                Kill23SumPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnKill23Sum", "");

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                Kill23SumPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                Kill23SumPara = Kill23SumPara.Replace("," + Click, "");
            }
        }
        #endregion

        #region 殺12位和
        string Kill12SumPara = "0";
        private void Kill12Sum_ButtomClick(object sender, EventArgs e)
        {
            if (Kill12SumPara == "0")
                Kill12SumPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnKill12Sum", "");

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                Kill12SumPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                Kill12SumPara = Kill12SumPara.Replace("," + Click, "");
            }
        }
        #endregion

        #region 殺型態號
        string KillTypeNumberPara = "0";
        private void KillTypeNumber_ButtomClick(object sender, EventArgs e)
        {
            if (KillTypeNumberPara == "0")
                KillTypeNumberPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnKillTypeNumber", "");

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                KillTypeNumberPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                KillTypeNumberPara = KillTypeNumberPara.Replace("," + Click, "");
            }
        }
        #endregion

        #region 殺和值
        string KillSumPara = "0";
        bool isTwoSum = false;
        private void KillSum_ButtomClick(object sender, EventArgs e)
        {
            isTwoSum = false;
            if (KillSumPara == "0")
                KillSumPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnKillSum", "");
            if (Click.Contains("Two"))
            {
                isTwoSum = true;
                Click = Buttom.Name.Replace("btnTwoKillSum", "");
            }

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                KillSumPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                KillSumPara = KillSumPara.Replace("," + Click, "");
            }
        }
        #endregion

        #region 殺誇度
        string KillCrossPara = "0";
        bool isTwoCross = false;
        private void KillCross_ButtomClick(object sender, EventArgs e)
        {
            isTwoCross = false;
            if (KillCrossPara == "0")
                KillCrossPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnKillCross", "");
            if (Click.Contains("TwoKillCross"))
            {
                isTwoCross = true;
                Click = Buttom.Name.Replace("btnTwoKillCross", "");
            }

            if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                KillCrossPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                KillCrossPara = KillCrossPara.Replace("," + Click, "");
            }
        }
        #endregion

        #region 膽碼

        string LocalNumber0LocalPara = "0";
        bool isTwoLocalNumber = false;
        private void LocalNumber0LocalPara_ButtomClick(object sender, EventArgs e)
        {
            if (LocalNumber0LocalPara == "0")
                LocalNumber0LocalPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnLocalNumber", "");
            if (Click.Contains("TwoLocalNumber"))
            {
                isTwoLocalNumber = true;
                Click = Buttom.Name.Replace("btnTwoLocalNumber", "");
            }
                //
                if (Buttom.BackColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                LocalNumber0LocalPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
                LocalNumber0LocalPara = LocalNumber0LocalPara.Replace("," + Click, "");
            }
        }

        string isKillPara = "0";
        private void btnLocalNumber0Local_Click(object sender, EventArgs e)
        {
            isKillPara = "Kill";
            if (isTwoLocalNumber)
            {
                btnTwoLocalNumber1Local.BackColor = Color.OrangeRed;
                btnTwoLocalNumber0Local.BackColor = color;
            }
            else
            { 
                btnLocalNumber0Local.BackColor = Color.OrangeRed;
                btnLocalNumber1Local.BackColor = color;
            }
        }
        private void btnLocalNumber1Local_Click(object sender, EventArgs e)
        {
            isKillPara = "LIVE";
            if (isTwoLocalNumber)
            {
                btnLocalNumber0Local.BackColor = Color.OrangeRed;
                btnLocalNumber1Local.BackColor = color;
            }
            else
            {
                btnLocalNumber1Local.BackColor = Color.OrangeRed;
                btnLocalNumber0Local.BackColor = color;
            }
        }

        #endregion

        #region 全選 全清
        private void ALLCheckClear_ButtomClick(object sender, EventArgs e)
        {
            var Buttom = (sender as Button);
            string Click = Buttom.Name;

            if (Click.Contains("02Road"))
            {
                if (Click.Contains("All"))
                {
                    btnKill02Road000.BackColor = Color.OrangeRed;
                    btnKill02Road001.BackColor = Color.OrangeRed;
                    btnKill02Road002.BackColor = Color.OrangeRed;
                    btnKill02Road010.BackColor = Color.OrangeRed;
                    btnKill02Road011.BackColor = Color.OrangeRed;
                    btnKill02Road012.BackColor = Color.OrangeRed;
                    btnKill02Road020.BackColor = Color.OrangeRed;
                    btnKill02Road021.BackColor = Color.OrangeRed;
                    btnKill02Road022.BackColor = Color.OrangeRed;
                    btnKill02Road100.BackColor = Color.OrangeRed;
                    btnKill02Road101.BackColor = Color.OrangeRed;
                    btnKill02Road102.BackColor = Color.OrangeRed;
                    btnKill02Road110.BackColor = Color.OrangeRed;
                    btnKill02Road111.BackColor = Color.OrangeRed;
                    btnKill02Road112.BackColor = Color.OrangeRed;
                    btnKill02Road120.BackColor = Color.OrangeRed;
                    btnKill02Road121.BackColor = Color.OrangeRed;
                    btnKill02Road122.BackColor = Color.OrangeRed;
                    btnKill02Road200.BackColor = Color.OrangeRed;
                    btnKill02Road201.BackColor = Color.OrangeRed;
                    btnKill02Road202.BackColor = Color.OrangeRed;
                    btnKill02Road210.BackColor = Color.OrangeRed;
                    btnKill02Road211.BackColor = Color.OrangeRed;
                    btnKill02Road212.BackColor = Color.OrangeRed;
                    btnKill02Road220.BackColor = Color.OrangeRed;
                    btnKill02Road221.BackColor = Color.OrangeRed;
                    btnKill02Road222.BackColor = Color.OrangeRed;
                }
                else
                {
                    btnKill02Road000.BackColor = color;
                    btnKill02Road001.BackColor = color;
                    btnKill02Road002.BackColor = color;
                    btnKill02Road010.BackColor = color;
                    btnKill02Road011.BackColor = color;
                    btnKill02Road012.BackColor = color;
                    btnKill02Road020.BackColor = color;
                    btnKill02Road021.BackColor = color;
                    btnKill02Road022.BackColor = color;
                    btnKill02Road100.BackColor = color;
                    btnKill02Road101.BackColor = color;
                    btnKill02Road102.BackColor = color;
                    btnKill02Road110.BackColor = color;
                    btnKill02Road111.BackColor = color;
                    btnKill02Road112.BackColor = color;
                    btnKill02Road120.BackColor = color;
                    btnKill02Road121.BackColor = color;
                    btnKill02Road122.BackColor = color;
                    btnKill02Road200.BackColor = color;
                    btnKill02Road201.BackColor = color;
                    btnKill02Road202.BackColor = color;
                    btnKill02Road210.BackColor = color;
                    btnKill02Road211.BackColor = color;
                    btnKill02Road212.BackColor = color;
                    btnKill02Road220.BackColor = color;
                    btnKill02Road221.BackColor = color;
                    btnKill02Road222.BackColor = color;
                }
            }
            else if (Click.Contains("OddEven"))
            {
                if (Click.Contains("All"))
                {
                    btnKillOddEven111.BackColor = Color.OrangeRed;
                    btnKillOddEven110.BackColor = Color.OrangeRed;
                    btnKillOddEven101.BackColor = Color.OrangeRed;
                    btnKillOddEven100.BackColor = Color.OrangeRed;
                    btnKillOddEven000.BackColor = Color.OrangeRed;
                    btnKillOddEven001.BackColor = Color.OrangeRed;
                    btnKillOddEven011.BackColor = Color.OrangeRed;
                    btnKillOddEven010.BackColor = Color.OrangeRed;
                }
                else
                {
                    btnKillOddEven111.BackColor = color;
                    btnKillOddEven110.BackColor = color;
                    btnKillOddEven101.BackColor = color;
                    btnKillOddEven100.BackColor = color;
                    btnKillOddEven000.BackColor = color;
                    btnKillOddEven001.BackColor = color;
                    btnKillOddEven011.BackColor = color;
                    btnKillOddEven010.BackColor = color;
                }
            }
            else if (Click.Contains("KillCross"))
            {
                if (Click.Contains("All"))
                {
                    btnKillCross2.BackColor = Color.OrangeRed;
                    btnKillCross3.BackColor = Color.OrangeRed;
                    btnKillCross4.BackColor = Color.OrangeRed;
                    btnKillCross5.BackColor = Color.OrangeRed;
                    btnKillCross6.BackColor = Color.OrangeRed;
                    btnKillCross7.BackColor = Color.OrangeRed;
                    btnKillCross8.BackColor = Color.OrangeRed;
                    btnKillCross9.BackColor = Color.OrangeRed;
                }
                else
                {
                    btnKillCross2.BackColor = color;
                    btnKillCross3.BackColor = color;
                    btnKillCross4.BackColor = color;
                    btnKillCross5.BackColor = color;
                    btnKillCross6.BackColor = color;
                    btnKillCross7.BackColor = color;
                    btnKillCross8.BackColor = color;
                    btnKillCross9.BackColor = color;
                }
            }
            else if (Click.Contains("KillSum"))
            {
                if (Click.Contains("All"))
                {
                    btnKillSum6.BackColor = Color.OrangeRed;
                    btnKillSum7.BackColor = Color.OrangeRed;
                    btnKillSum8.BackColor = Color.OrangeRed;
                    btnKillSum9.BackColor = Color.OrangeRed;
                    btnKillSum10.BackColor = Color.OrangeRed;
                    btnKillSum11.BackColor = Color.OrangeRed;
                    btnKillSum12.BackColor = Color.OrangeRed;
                    btnKillSum13.BackColor = Color.OrangeRed;
                    btnKillSum14.BackColor = Color.OrangeRed;
                    btnKillSum15.BackColor = Color.OrangeRed;
                    btnKillSum16.BackColor = Color.OrangeRed;
                    btnKillSum17.BackColor = Color.OrangeRed;
                    btnKillSum18.BackColor = Color.OrangeRed;
                    btnKillSum19.BackColor = Color.OrangeRed;
                    btnKillSum20.BackColor = Color.OrangeRed;
                    btnKillSum21.BackColor = Color.OrangeRed;
                    btnKillSum22.BackColor = Color.OrangeRed;
                    btnKillSum23.BackColor = Color.OrangeRed;
                    btnKillSum24.BackColor = Color.OrangeRed;
                    btnKillSum25.BackColor = Color.OrangeRed;
                    btnKillSum26.BackColor = Color.OrangeRed;
                    btnKillSum27.BackColor = Color.OrangeRed;
                }
                else
                {
                    btnKillSum6.BackColor = color;
                    btnKillSum7.BackColor = color;
                    btnKillSum8.BackColor = color;
                    btnKillSum9.BackColor = color;
                    btnKillSum10.BackColor = color;
                    btnKillSum11.BackColor = color;
                    btnKillSum12.BackColor = color;
                    btnKillSum13.BackColor = color;
                    btnKillSum14.BackColor = color;
                    btnKillSum15.BackColor = color;
                    btnKillSum16.BackColor = color;
                    btnKillSum17.BackColor = color;
                    btnKillSum18.BackColor = color;
                    btnKillSum19.BackColor = color;
                    btnKillSum20.BackColor = color;
                    btnKillSum21.BackColor = color;
                    btnKillSum22.BackColor = color;
                    btnKillSum23.BackColor = color;
                    btnKillSum24.BackColor = color;
                    btnKillSum25.BackColor = color;
                    btnKillSum26.BackColor = color;
                    btnKillSum27.BackColor = color;
                }
            }
            else if (Click.Contains("LocalNumber"))
            {
                if (Click.Contains("All"))
                {
                    btnLocalNumber1.BackColor = Color.OrangeRed;
                    btnLocalNumber2.BackColor = Color.OrangeRed;
                    btnLocalNumber3.BackColor = Color.OrangeRed;
                    btnLocalNumber4.BackColor = Color.OrangeRed;
                    btnLocalNumber5.BackColor = Color.OrangeRed;
                    btnLocalNumber6.BackColor = Color.OrangeRed;
                    btnLocalNumber7.BackColor = Color.OrangeRed;
                    btnLocalNumber8.BackColor = Color.OrangeRed;
                    btnLocalNumber9.BackColor = Color.OrangeRed;
                    btnLocalNumber10.BackColor = Color.OrangeRed;
                }
                else
                {
                    btnLocalNumber1.BackColor = color;
                    btnLocalNumber2.BackColor = color;
                    btnLocalNumber3.BackColor = color;
                    btnLocalNumber4.BackColor = color;
                    btnLocalNumber5.BackColor = color;
                    btnLocalNumber6.BackColor = color;
                    btnLocalNumber7.BackColor = color;
                    btnLocalNumber8.BackColor = color;
                    btnLocalNumber9.BackColor = color;
                    btnLocalNumber10.BackColor = color;
                }
            }
            else if (Click.Contains("TwoKillOddEven"))
            {
                if (Click.Contains("All"))
                {
                    btnLocalNumber1.BackColor = Color.OrangeRed;
                    btnLocalNumber2.BackColor = Color.OrangeRed;
                    btnLocalNumber3.BackColor = Color.OrangeRed;
                    btnLocalNumber4.BackColor = Color.OrangeRed;
                    btnLocalNumber5.BackColor = Color.OrangeRed;
                    btnLocalNumber6.BackColor = Color.OrangeRed;
                    btnLocalNumber7.BackColor = Color.OrangeRed;
                    btnLocalNumber8.BackColor = Color.OrangeRed;
                    btnLocalNumber9.BackColor = Color.OrangeRed;
                    btnLocalNumber10.BackColor = Color.OrangeRed;
                }
                else
                {
                    btnLocalNumber1.BackColor = color;
                    btnLocalNumber2.BackColor = color;
                    btnLocalNumber3.BackColor = color;
                    btnLocalNumber4.BackColor = color;
                    btnLocalNumber5.BackColor = color;
                    btnLocalNumber6.BackColor = color;
                    btnLocalNumber7.BackColor = color;
                    btnLocalNumber8.BackColor = color;
                    btnLocalNumber9.BackColor = color;
                    btnLocalNumber10.BackColor = color;
                }
            }
            else if (Click.Contains("TwoLocalNumber"))
            {
                if (Click.Contains("All"))
                {
                    btnTwoLocalNumber01.BackColor = Color.Orange;
                    btnTwoLocalNumber02.BackColor = Color.Orange;
                    btnTwoLocalNumber03.BackColor = Color.Orange;
                    btnTwoLocalNumber04.BackColor = Color.Orange;
                    btnTwoLocalNumber05.BackColor = Color.Orange;
                    btnTwoLocalNumber06.BackColor = Color.Orange;
                    btnTwoLocalNumber07.BackColor = Color.Orange;
                    btnTwoLocalNumber08.BackColor = Color.Orange;
                    btnTwoLocalNumber09.BackColor = Color.Orange;
                    btnTwoLocalNumber10.BackColor = Color.Orange;
                }
                else
                {
                    btnTwoLocalNumber01.BackColor = color;
                    btnTwoLocalNumber02.BackColor = color;
                    btnTwoLocalNumber03.BackColor = color;
                    btnTwoLocalNumber04.BackColor = color;
                    btnTwoLocalNumber05.BackColor = color;
                    btnTwoLocalNumber06.BackColor = color;
                    btnTwoLocalNumber07.BackColor = color;
                    btnTwoLocalNumber08.BackColor = color;
                    btnTwoLocalNumber09.BackColor = color;
                    btnTwoLocalNumber10.BackColor = color;

                }
            }
            else if (Click.Contains("TwoLocalNumber"))
            {
                if (Click.Contains("All"))
                {
                    btnTwoKillCross1.BackColor = Color.Orange;
                    btnTwoKillCross2.BackColor = Color.Orange;
                    btnTwoKillCross3.BackColor = Color.Orange;
                    btnTwoKillCross5.BackColor = Color.Orange;
                    btnTwoKillCross6.BackColor = Color.Orange;
                    btnTwoKillCross7.BackColor = Color.Orange;
                    btnTwoKillCross8.BackColor = Color.Orange;
                    btnTwoKillCross9.BackColor = Color.Orange;
                }
                else
                {
                    btnTwoKillCross1.BackColor = color;
                    btnTwoKillCross2.BackColor = color;
                    btnTwoKillCross3.BackColor = color;
                    btnTwoKillCross4.BackColor = color;
                    btnTwoKillCross5.BackColor = color;
                    btnTwoKillCross6.BackColor = color;
                    btnTwoKillCross7.BackColor = color;
                    btnTwoKillCross8.BackColor = color;
                    btnTwoKillCross9.BackColor = color;

                }
            }
        }
        #endregion

        #region 二星選號模式
        bool isTwoChoose = false;
        //全選
        private void btnTwoLocalNumberAll_Click(object sender, EventArgs e)
        {
            isTwoChoose = true;
            btnChooseModeAll.BackColor = Color.OrangeRed;
            //把這框內的選擇都恢復btnTwoChooseModeBig
            btnTwoChooseModeBig.BackColor = color;
            btnTwoChooseModeSmall.BackColor = color;
            btnTwoChooseModeOdd.BackColor = color;
            btnTwoChooseModeEven.BackColor = color;

            //百位
            btnTwoChooseModeTen01.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen02.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen03.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen04.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen05.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen06.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen07.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen08.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen09.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen10.BackColor = Color.OrangeRed;

            //十位
            btnTwoChooseModeOne01.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne02.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne03.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne04.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne05.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne06.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne07.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne08.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne09.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne10.BackColor = Color.OrangeRed;

            ChooseModePara = "All";
        }

        //選大
        private void btnTwoChooseModeOneBig_Click(object sender, EventArgs e)
        {
            isTwoChoose = true;

            TwoClearAllForChooseMode();

            btnTwoChooseModeBig.BackColor = Color.OrangeRed;
            //把這框內的選擇都恢復btnTwoChooseModeBig
            btnChooseModeAll.BackColor = color;
            btnTwoChooseModeSmall.BackColor = color;
            btnTwoChooseModeOdd.BackColor = color;
            btnTwoChooseModeEven.BackColor = color;

            //百位
            btnTwoChooseModeTen06.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen07.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen08.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen09.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen10.BackColor = Color.OrangeRed;

            //十位
            btnTwoChooseModeOne06.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne07.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne08.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne09.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne10.BackColor = Color.OrangeRed;

            ChooseModePara = "Big";
        }


        //選小
        private void btnTwoChooseModeOneSmall_Click(object sender, EventArgs e)
        {
            isTwoChoose = true;

            TwoClearAllForChooseMode();

            btnTwoChooseModeSmall.BackColor = Color.OrangeRed;
            //把這框內的選擇都恢復btnTwoChooseModeBig
            btnChooseModeAll.BackColor = color;
            btnTwoChooseModeBig.BackColor = color;
            btnTwoChooseModeOdd.BackColor = color;
            btnTwoChooseModeEven.BackColor = color;

            //百位
            btnTwoChooseModeTen01.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen02.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen03.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen04.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen05.BackColor = Color.OrangeRed;

            //十位
            btnTwoChooseModeOne01.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne02.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne03.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne04.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne05.BackColor = Color.OrangeRed;

            ChooseModePara = "Small";
        }

        //選奇數
        private void btnTwoChooseModeOneOdd_Click(object sender, EventArgs e)
        {
            isTwoChoose = true;

            TwoClearAllForChooseMode();

            btnTwoChooseModeOdd.BackColor = Color.OrangeRed;
            //把這框內的選擇都恢復btnTwoChooseModeBig
            btnChooseModeAll.BackColor = color;
            btnTwoChooseModeBig.BackColor = color;
            btnTwoChooseModeSmall.BackColor = color;
            btnTwoChooseModeEven.BackColor = color;

            //百位
            btnTwoChooseModeTen01.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen03.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen05.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen07.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen09.BackColor = Color.OrangeRed;

            //十位
            btnTwoChooseModeOne01.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne03.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne05.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne07.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne09.BackColor = Color.OrangeRed;

            ChooseModePara = "Odd";
        }

        //選偶數
        private void btnTwoChooseModeOneEven_Click(object sender, EventArgs e)
        {
            isTwoChoose = true;

            TwoClearAllForChooseMode();

            btnTwoChooseModeEven.BackColor = Color.OrangeRed;
            //把這框內的選擇都恢復btnTwoChooseModeBig
            btnChooseModeAll.BackColor = color;
            btnTwoChooseModeBig.BackColor = color;
            btnTwoChooseModeSmall.BackColor = color;
            btnTwoChooseModeOdd.BackColor = color;

            //百位
            btnTwoChooseModeTen02.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen04.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen06.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen08.BackColor = Color.OrangeRed;
            btnTwoChooseModeTen10.BackColor = Color.OrangeRed;

            //十位
            btnTwoChooseModeOne02.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne04.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne06.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne08.BackColor = Color.OrangeRed;
            btnTwoChooseModeOne10.BackColor = Color.OrangeRed;

            ChooseModePara = "Even";
        }

        private void btnTwoChooseModeClear_Click(object sender, EventArgs e)
        {

        }

        private void TwoClearAllForChooseMode()
        {
            //把這框內的選擇都恢復
            btnTwoChooseModeAll.BackColor = color;
            btnTwoChooseModeBig.BackColor = color;
            btnTwoChooseModeSmall.BackColor = color;
            btnTwoChooseModeOdd.BackColor = color;
            btnTwoChooseModeEven.BackColor = color;

            //百位
            btnTwoChooseModeTen01.BackColor = color;
            btnTwoChooseModeTen02.BackColor = color;
            btnTwoChooseModeTen03.BackColor = color; ;
            btnTwoChooseModeTen04.BackColor = color; ;
            btnTwoChooseModeTen05.BackColor = color; ;
            btnTwoChooseModeTen06.BackColor = color; ;
            btnTwoChooseModeTen07.BackColor = color; ;
            btnTwoChooseModeTen08.BackColor = color; ;
            btnTwoChooseModeTen09.BackColor = color; ;
            btnTwoChooseModeTen10.BackColor = color; ;

            //十位
            btnTwoChooseModeOne01.BackColor = color; ;
            btnTwoChooseModeOne02.BackColor = color; ;
            btnTwoChooseModeOne03.BackColor = color; ;
            btnTwoChooseModeOne04.BackColor = color; ;
            btnTwoChooseModeOne05.BackColor = color; ;
            btnTwoChooseModeOne06.BackColor = color; ;
            btnTwoChooseModeOne07.BackColor = color; ;
            btnTwoChooseModeOne08.BackColor = color; ;
            btnTwoChooseModeOne09.BackColor = color; ;
            btnTwoChooseModeOne10.BackColor = color; ;
        }


        #endregion

        private void btnTwoClear_Click(object sender, EventArgs e)
        {
            rtbTwoResult.Text = "";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbResultNumber.Text = "";
        }
    }
}
