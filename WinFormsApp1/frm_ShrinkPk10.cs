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

        private void changeButtomColor()
        {
        }

        private void btnStartWork_Click(object sender, EventArgs e)
        {
            string TableNmae = tabMakeNumber.SelectedTab.Name;
            DataTable dt = SelectDB(TableNmae);
            var str_json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            JArray ja = (JArray)JsonConvert.DeserializeObject(str_json);

            //組選號碼
            var ResultChooseModePara = ja.ToList();            
            if (ChooseModePara == "Big")
            {
                ResultChooseModePara = ja.Where(x => int.Parse(x["HUN"].ToString()) > 5 && int.Parse(x["TEN"].ToString()) > 5 && int.Parse(x["ONE"].ToString()) > 5).ToList();
            }
            else if (ChooseModePara == "Small")
            {
                ResultChooseModePara = ja.Where(x => int.Parse(x["HUN"].ToString()) < 6 && int.Parse(x["TEN"].ToString()) < 6 && int.Parse(x["ONE"].ToString()) < 6).ToList();
            }
            else if (ChooseModePara == "Odd")
            {
                ResultChooseModePara = ja.Where(x => (int.Parse(x["HUN"].ToString()) % 2) == 1 && (int.Parse(x["TEN"].ToString()) % 2) == 1 && (int.Parse(x["ONE"].ToString()) % 2) == 1).ToList();
            }
            else if (ChooseModePara == "Even")
            {
                ResultChooseModePara = ja.Where(x => (int.Parse(x["HUN"].ToString()) % 2) == 0 && (int.Parse(x["TEN"].ToString()) % 2) == 0 && (int.Parse(x["ONE"].ToString()) % 2) == 0).ToList();
            }

            //殺二碼組合
            var ResultKillTwoCombineFirst = ResultChooseModePara.Where(x => x["HUN"].ToString() != selectKillTwoCombineFirstPara).ToList();
            var ResultKillTwoCombineSec = ResultKillTwoCombineFirst.Where(x => x["TEN"].ToString() != selectKillTwoCombineSecPara).ToList();

            //殺二碼和
            var ResultKillTwoSum = ResultKillTwoCombineSec;
            if(KillTwoSumPara != "0")
            { 
                var ParaKillTwoSumPara = KillTwoSumPara.Substring(1).Split(',');            
                for (int i =0; i< ParaKillTwoSumPara.Count(); i++)
                {
                    if (i == 0)
                    {
                        ResultKillTwoSum = ResultKillTwoCombineSec
                            .Where(x => int.Parse(x["HUN"].ToString()) + int.Parse(x["TEN"].ToString()) != int.Parse(ParaKillTwoSumPara[i])).ToList();
                    }
                    else
                    {
                        ResultKillTwoSum = ResultKillTwoSum
                            .Where(x => int.Parse(x["HUN"].ToString()) + int.Parse(x["TEN"].ToString()) != int.Parse(ParaKillTwoSumPara[i])).ToList();
                    }
                }
            }

            //殺二碼差
            var ResultKillTwoLess = ResultKillTwoSum;
            if(KillTwoLessPara != "0")
            { 
                var ParaKillTwoLessPara = KillTwoLessPara.Substring(1).Split(',');
                for (int i = 0; i < ParaKillTwoLessPara.Count(); i++)
                {
                    if (i == 0)
                    {
                        ResultKillTwoLess = ResultKillTwoSum
                            .Where(x => Math.Abs(int.Parse(x["HUN"].ToString()) - int.Parse(x["TEN"].ToString())) != int.Parse(ParaKillTwoLessPara[i])).ToList();
                    }
                    else
                    {
                        ResultKillTwoLess = ResultKillTwoLess
                            .Where(x => Math.Abs(int.Parse(x["HUN"].ToString()) - int.Parse(x["TEN"].ToString())) != int.Parse(ParaKillTwoLessPara[i])).ToList();
                    }
                }
            }

            //殺012路
            var ResultKill012Road = ResultKillTwoLess;
            if (Kill012RoadPara != "0")
            { 
                var ParaKill012RoadPara = Kill012RoadPara.Split(',');

                for (int i = 0; i < ParaKill012RoadPara.Count(); i++)
                {
                    if (i == 0)
                    {
                        ResultKill012Road = ResultKillTwoLess
                            .Where(x => int.Parse(x["HUM"].ToString()) % 3 != int.Parse(ParaKill012RoadPara[i].Substring(0,1)) && int.Parse(x["TEN"].ToString()) % 3 != int.Parse(ParaKill012RoadPara[i].Substring(1, 1)) && int.Parse(x["ONE"].ToString()) % 3 != int.Parse(ParaKill012RoadPara[i].Substring(2, 1))).ToList();
                    }
                    else
                    {
                        ResultKill012Road = ResultKill012Road
                            .Where(x => int.Parse(x["HUM"].ToString()) % 3 != int.Parse(ParaKill012RoadPara[i].Substring(0, 1)) && int.Parse(x["TEN"].ToString()) % 3 != int.Parse(ParaKill012RoadPara[i].Substring(1, 1)) && int.Parse(x["ONE"].ToString()) % 3 != int.Parse(ParaKill012RoadPara[i].Substring(2, 1))).ToList();
                    }
                }
            }

            //殺奇偶
            var ResultKillOddEven = ResultKill012Road;
            if(KillOddEvenPara != "0")
            { 
                var ParaKillOddEven = KillOddEvenPara.Split(',');
                for (int i = 0; i < ParaKillOddEven.Count(); i++)
                {
                    if (i == 0)
                    {
                        ResultKill012Road = ResultKillTwoLess
                            .Where(x => int.Parse(x["HUM"].ToString()) % 2 != int.Parse(ParaKillOddEven[i].Substring(0, 1)) && int.Parse(x["TEN"].ToString()) % 2 != int.Parse(ParaKillOddEven[i].Substring(1, 1)) && int.Parse(x["ONE"].ToString()) % 2 != int.Parse(ParaKillOddEven[i].Substring(2, 1))).ToList();
                    }
                    else
                    {
                        ResultKill012Road = ResultKill012Road
                            .Where(x => int.Parse(x["HUM"].ToString()) % 2 != int.Parse(ParaKillOddEven[i].Substring(0, 1)) && int.Parse(x["TEN"].ToString()) % 2 != int.Parse(ParaKillOddEven[i].Substring(1, 1)) && int.Parse(x["ONE"].ToString()) % 2 != int.Parse(ParaKillOddEven[i].Substring(2, 1))).ToList();
                    }
                }
            }

            //殺大小
            var ResultKillBigSmall = ResultKillOddEven;
            if (KillBigSmallPara != "0")
            {
                var ParaKillBigSmallPara = KillBigSmallPara.Split(',');
                for (int i = 0; i < ParaKillBigSmallPara.Count(); i++)
                {
                    switch (ParaKillBigSmallPara[i].ToString())
                    {
                        case "555":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUM"].ToString()) <= 5 && int.Parse(x["TEN"].ToString()) <= 5 && int.Parse(x["ONE"].ToString()) < 5).ToList(); 
                            break;
                        case "556":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUM"].ToString()) <= 5 && int.Parse(x["TEN"].ToString()) <= 5 && int.Parse(x["ONE"].ToString()) > 5).ToList();
                            break;
                        case "565":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUM"].ToString()) <= 5 && int.Parse(x["TEN"].ToString()) > 5 && int.Parse(x["ONE"].ToString()) < 5).ToList();
                            break;
                        case "566":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUM"].ToString()) <= 5 && int.Parse(x["TEN"].ToString()) > 5 && int.Parse(x["ONE"].ToString()) > 5).ToList();
                            break;
                        case "666":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUM"].ToString()) > 5 && int.Parse(x["TEN"].ToString()) > 5 && int.Parse(x["ONE"].ToString()) > 5).ToList();
                            break;
                        case "655":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUM"].ToString()) > 5 && int.Parse(x["TEN"].ToString()) <= 5 && int.Parse(x["ONE"].ToString()) <= 5).ToList();
                            break;
                        case "656":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUM"].ToString()) < 5 && int.Parse(x["TEN"].ToString()) < 5 && int.Parse(x["ONE"].ToString()) < 5).ToList();
                            break;
                        case "665":
                            ResultKillBigSmall = ResultKillBigSmall
                            .Where(x => int.Parse(x["HUM"].ToString()) > 5 && int.Parse(x["TEN"].ToString()) <= 5 && int.Parse(x["ONE"].ToString()) > 5).ToList();
                            break;
                    }
                }
            }

            //殺13位差
            var ResultKill13Less = ResultKillBigSmall;
            if (Kill13LessPara != "0")
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
            if (Kill23LessPara != "0")
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
            if (Kill12LessPara != "0")
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
            if (Kill13SumPara != "0")
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
            if (Kill23SumPara != "0")
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
            if (Kill12SumPara != "0")
            {
                var ParaKill12SumPara = Kill12SumPara.Substring(1).Split(',');
                for (int i = 0; i < ParaKill12SumPara.Count(); i++)
                {
                    ResultKill12Sum = ResultKill12Sum
                                               .Where(x => int.Parse(x["HUN"].ToString()) + int.Parse(x["ONE"].ToString()) != int.Parse(ParaKill12SumPara[i])).ToList();
                }
            }

            //殺型態號
            var ResultKillType = ResultKill23Sum;
            if (KillTypeNumberPara != "0")
            {
                var ParaKillTypeNumberPara = KillTypeNumberPara.Substring(1).Split(',');
                for (int i = 0; i < ParaKillTypeNumberPara.Count(); i++)
                {
                    switch (ParaKillTypeNumberPara[i].ToString())
                    {
                        case "1": //上階梯
                            ResultKillType = ResultKillType
                                                .Where(x => int.Parse(x["HUN"].ToString()) + 2 != int.Parse(x["TEN"].ToString()) + 1 && int.Parse(x["TEN"].ToString()) + 1 != int.Parse(x["ONE"].ToString())).ToList();
                            break;
                        case "2": //下階梯
                            ResultKillType = ResultKillType
                                                .Where(x => int.Parse(x["HUN"].ToString()) != int.Parse(x["TEN"].ToString()) + 1 && int.Parse(x["TEN"].ToString()) + 1 != int.Parse(x["ONE"].ToString()) + 2).ToList();
                            break;
                        case "3": //凸型
                            ResultKillType = ResultKillType
                                                .Where(x => int.Parse(x["TEN"].ToString()) > int.Parse(x["ONE"].ToString()) && int.Parse(x["TEN"].ToString()) > int.Parse(x["HUN"].ToString())).ToList();
                            break;
                        case "4": //凹型
                            ResultKillType = ResultKillType
                                                .Where(x => int.Parse(x["TEN"].ToString()) < int.Parse(x["ONE"].ToString()) && int.Parse(x["TEN"].ToString()) < int.Parse(x["HUN"].ToString())).ToList();
                            break;
                        case "5": //三連號
                            ResultKillType = ResultKillType
                                                .Where(x => int.Parse(x["HUN"].ToString()) + 2 != int.Parse(x["TEN"].ToString()) + 1 && int.Parse(x["TEN"].ToString()) + 1 != int.Parse(x["ONE"].ToString()))
                                                    .Where(x => int.Parse(x["HUN"].ToString()) != int.Parse(x["TEN"].ToString()) + 1 && int.Parse(x["TEN"].ToString()) + 1 != int.Parse(x["ONE"].ToString()) + 2).ToList();
                            break;
                        case "6": //二連號
                            ResultKillType = ResultKillType
                                                .Where(x => int.Parse(x["HUN"].ToString()) + 1 != int.Parse(x["TEN"].ToString()) + 1 && int.Parse(x["HUN"].ToString()) != int.Parse(x["TEN"].ToString()) + 1)
                                                    .Where(x => int.Parse(x["TEN"].ToString()) + 1 != int.Parse(x["ONE"].ToString()) + 1 && int.Parse(x["TEN"].ToString()) != int.Parse(x["ONE"].ToString()) + 1).ToList();
                            break;
                    }
                }
            }

            //殺和值
            var ResultKillSum = ResultKillType;
            if (KillSumPara != "0")
            {
                var ParaKillSumPara = KillSumPara.Substring(1).Split(',');
                for (int i = 0; i < ParaKillSumPara.Count(); i++)
                {
                    ResultKillSum = ResultKillSum
                                        .Where(x => int.Parse(x["HUN"].ToString()) + int.Parse(x["TEN"].ToString()) + int.Parse(x["ONE"].ToString()) != int.Parse(ParaKillSumPara[i])).ToList();
                }
            }

            //殺跨度
            var ResultKillCross = ResultKillType;
            if (KillCrossPara != "0")
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

            //膽碼
            var ResultLocal = ResultKillType;
            if (isKillPara != "0" && LocalNumber0LocalPara != "0")
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


            //最後處理得再補上
            string Result = "";
            foreach (var item in ResultKillCross)
            {
                Result += " ," + item["Number"];
            }
            Result = " "+Result.Substring(2);
            rtbResultNumber.Text = Result;
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
                Sqlstr = @"select Number ,substring(Number, 0,3) AS TEN ,substring(Number, 3,3)AS ONE From " + Table;
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
        private void KillTwoCombineFisrt_ButtomChange(string Click)
        {
            Click = Click.Replace("btnKillTwoCombine","");
            selectKillTwoCombineFirstPara = (int.Parse(Click.ToString())).ToString("d2");
            //changeButtomColor();

            switch (Click)
            {
                case "1":
                    btnKillTwoCombineSec1.Enabled = false;
                    break;
                case "2":
                    btnKillTwoCombineSec2.Enabled = false;
                    break;
                case "3":
                    btnKillTwoCombineSec3.Enabled = false;
                    break;
                case "4":
                    btnKillTwoCombineSec4.Enabled = false;
                    break;
                case "5":
                    btnKillTwoCombineSec5.Enabled = false;
                    break;
                case "6":
                    btnKillTwoCombineSec6.Enabled = false;
                    break;
                case "7":
                    btnKillTwoCombineSec7.Enabled = false;
                    break;
                case "8":
                    btnKillTwoCombineSec8.Enabled = false;
                    break;
                case "9":
                    btnKillTwoCombineSec9.Enabled = false;
                    break;

            }
        }

        private void KillTwoCombineSec_ButtomChange(string Click)
        {
            Click = Click.Replace("btnKillTwoCombine", "");
            selectKillTwoCombineSecPara = (int.Parse(Click.ToString())).ToString("d2");
            //changeButtomColor();

            switch (Click)
            {
                case "1":
                    btnKillTwoCombine1.Enabled = false;
                    break;
                case "2":
                    btnKillTwoCombine2.Enabled = false;
                    break;
                case "3":
                    btnKillTwoCombine3.Enabled = false;
                    break;
                case "4":
                    btnKillTwoCombine4.Enabled = false;
                    break;
                case "5":
                    btnKillTwoCombine5.Enabled = false;
                    break;
                case "6":
                    btnKillTwoCombine6.Enabled = false;
                    break;
                case "7":
                    btnKillTwoCombine7.Enabled = false;
                    break;
                case "8":
                    btnKillTwoCombine8.Enabled = false;
                    break;
                case "9":
                    btnKillTwoCombine9.Enabled = false;
                    break;
                case "10":
                    btnKillTwoCombine10.Enabled = false;
                    break;
            }
        }

        private void KillTwoCombineFisrt_ButtomClick(object sender, EventArgs e)
        {
            var Buttom = (sender as Button);
            string Click = Buttom.Name;
            //全部打開
            btnKillTwoCombineSec1.Enabled = btnKillTwoCombineSec2.Enabled = btnKillTwoCombineSec3.Enabled = btnKillTwoCombineSec4.Enabled = btnKillTwoCombineSec5.Enabled = btnKillTwoCombineSec6.Enabled = btnKillTwoCombineSec7.Enabled = btnKillTwoCombineSec8.Enabled = btnKillTwoCombineSec9.Enabled = btnKillTwoCombineSec10.Enabled = true;

            if (Buttom.ForeColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                KillTwoCombineFisrt_ButtomChange(Click);
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = color;
                Buttom.ForeColor = Color.Black;
            }
        }

        private void KillTwoCombineSec_ButtomClick(object sender, EventArgs e)
        {
            var Buttom = (sender as Button);
            string Click = Buttom.Name;

            //全部打開
            btnKillTwoCombine1.Enabled = btnKillTwoCombine2.Enabled = btnKillTwoCombine3.Enabled = btnKillTwoCombine4.Enabled = btnKillTwoCombine5.Enabled = btnKillTwoCombine6.Enabled = btnKillTwoCombine7.Enabled = btnKillTwoCombine8.Enabled = btnKillTwoCombine9.Enabled = btnKillTwoCombine10.Enabled = true;

            if (Buttom.ForeColor == color)
            {
                Buttom.BackColor = Color.OrangeRed;
                //Buttom.ForeColor = Color.White;
                KillTwoCombineSec_ButtomChange(Click);
            }
            else
            {
                Buttom.Refresh();
                //Buttom.BackColor = HexColor("#E3DDCA");
                //Buttom.ForeColor = color;
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

            if(Buttom.BackColor == color)
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
            if (KillTwoSumPara == "0")
                KillTwoSumPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("btnKillTwoLess", "");

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

        private void btnChooseModeOneClear_Click(object sender, EventArgs e)
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
            btnChooseModeHundred03.BackColor = color;;
            btnChooseModeHundred04.BackColor = color;;
            btnChooseModeHundred05.BackColor = color;;
            btnChooseModeHundred06.BackColor = color;;
            btnChooseModeHundred07.BackColor = color;;
            btnChooseModeHundred08.BackColor = color;;
            btnChooseModeHundred09.BackColor = color;;
            btnChooseModeHundred10.BackColor = color;;

            //十位
            btnChooseModeTen01.BackColor = color;;
            btnChooseModeTen02.BackColor = color;;
            btnChooseModeTen03.BackColor = color;;
            btnChooseModeTen04.BackColor = color;;
            btnChooseModeTen05.BackColor = color;;
            btnChooseModeTen06.BackColor = color;;
            btnChooseModeTen07.BackColor = color;;
            btnChooseModeTen08.BackColor = color;;
            btnChooseModeTen09.BackColor = color;;
            btnChooseModeTen10.BackColor = color;;

            //個位
            btnChooseModeOne01.BackColor = color;;
            btnChooseModeOne02.BackColor = color;;
            btnChooseModeOne03.BackColor = color;;
            btnChooseModeOne04.BackColor = color;;
            btnChooseModeOne05.BackColor = color;;
            btnChooseModeOne06.BackColor = color;;
            btnChooseModeOne07.BackColor = color;;
            btnChooseModeOne08.BackColor = color;;
            btnChooseModeOne09.BackColor = color;;
            btnChooseModeOne10.BackColor = color;;

            ChooseModePara = "All";

        }
        #endregion

        #region 殺012路
        string Kill012RoadPara = "0";
        private void Kill012Road_ButtomClick(object sender, EventArgs e)
        {
            if (Kill012RoadPara == "0")
                Kill012RoadPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("Kill02Road", "");

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
        private void KillOddEven_ButtomClick(object sender, EventArgs e)
        {
            if (KillOddEvenPara == "0")
                KillOddEvenPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("KillOddEven", "");

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

        #region 殺大小
        string KillBigSmallPara = "0";
        private void KillBigSmall_ButtomClick(object sender, EventArgs e)
        {
            if (KillBigSmallPara == "0")
                KillBigSmallPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("KillBigSmall", "");

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
                Kill13LessPara = KillBigSmallPara.Replace("," + Click, "");
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
            string Click = Buttom.Name.Replace("Kill23Less", "");

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
            string Click = Buttom.Name.Replace("Kill12Less", "");

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
            string Click = Buttom.Name.Replace("Kill13Sum", "");

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
            string Click = Buttom.Name.Replace("Kill23Sum", "");

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
            string Click = Buttom.Name.Replace("Kill12Sum", "");

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
            string Click = Buttom.Name.Replace("KillTypeNumber", "");

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
        private void KillSum_ButtomClick(object sender, EventArgs e)
        {
            if (KillSumPara == "0")
                KillSumPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("KillSum", "");

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
        private void KillCross_ButtomClick(object sender, EventArgs e)
        {
            if (KillCrossPara == "0")
                KillCrossPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("KillCross", "");

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
        
        private void LocalNumber0LocalPara_ButtomClick(object sender, EventArgs e)
        {
            if (LocalNumber0LocalPara == "0")
                LocalNumber0LocalPara = "";
            var Buttom = (sender as Button);
            string Click = Buttom.Name.Replace("KillCross", "");

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
            btnLocalNumber0Local.BackColor = Color.OrangeRed;
            btnLocalNumber1Local.BackColor = color;
        }
        private void btnLocalNumber1Local_Click(object sender, EventArgs e)
        {
            isKillPara = "";
            btnLocalNumber1Local.BackColor = Color.OrangeRed;
            btnLocalNumber0Local.BackColor = color;
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
        }
        #endregion

    }
}
