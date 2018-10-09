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
        public frm_ShrinkPk10()
        {
            InitializeComponent();
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

            //殺二碼組合
            var ResultKillTwoCombineFirst = ja.Where(x => x["HUN"].ToString() != selectKillTwoCombineFirstPara).ToList();
            var ResultKillTwoCombineSec = ResultKillTwoCombineFirst.Where(x => x["TEN"].ToString() != selectKillTwoCombineSecPara).ToList();

            //殺二碼和
            var ParaKillTwoSumPara = KillTwoSumPara.Substring(1).Split(',');
            var ResultKillTwoSum = ResultKillTwoCombineSec;
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

            //殺二碼差
            var ParaKillTwoLessPara = KillTwoLessPara.Substring(1).Split(',');
            var ResultKillTwoLess = ResultKillTwoSum;
            for (int i = 0; i < ParaKillTwoSumPara.Count(); i++)
            {
                if (i == 0)
                {
                    ResultKillTwoLess = ResultKillTwoSum
                        .Where(x => int.Parse(x["HUN"].ToString()) - int.Parse(x["TEN"].ToString()) != int.Parse(ParaKillTwoSumPara[i])).ToList();
                }
                else
                {
                    ResultKillTwoLess = ResultKillTwoLess
                        .Where(x => int.Parse(x["HUN"].ToString()) - int.Parse(x["TEN"].ToString()) != int.Parse(ParaKillTwoSumPara[i])).ToList();
                }
            }

            string Result = "";
            foreach (var item in ResultKillTwoLess)
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

            if (Buttom.ForeColor == Color.Black)
            {
                Buttom.BackColor = Color.OrangeRed;
                Buttom.ForeColor = Color.White;
                KillTwoCombineFisrt_ButtomChange(Click);
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = Control.DefaultBackColor;
                Buttom.ForeColor = Color.Black;
            }
        }

        private void KillTwoCombineSec_ButtomClick(object sender, EventArgs e)
        {
            var Buttom = (sender as Button);
            string Click = Buttom.Name;

            //全部打開
            btnKillTwoCombine1.Enabled = btnKillTwoCombine2.Enabled = btnKillTwoCombine3.Enabled = btnKillTwoCombine4.Enabled = btnKillTwoCombine5.Enabled = btnKillTwoCombine6.Enabled = btnKillTwoCombine7.Enabled = btnKillTwoCombine8.Enabled = btnKillTwoCombine9.Enabled = btnKillTwoCombine10.Enabled = true;

            if (Buttom.ForeColor == Control.DefaultBackColor)
            {
                Buttom.BackColor = Color.OrangeRed;
                Buttom.ForeColor = Color.White;
                KillTwoCombineSec_ButtomChange(Click);
            }
            else
            {
                Buttom.Refresh();
                //Buttom.BackColor = HexColor("#E3DDCA");
                //Buttom.ForeColor = Control.DefaultBackColor;
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

            if (Buttom.ForeColor == Color.Black)
            {
                Buttom.BackColor = Color.OrangeRed;
                Buttom.ForeColor = Color.White;               
                KillTwoSumPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = Control.DefaultBackColor;
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

            if (Buttom.ForeColor == Color.Black)
            {
                Buttom.BackColor = Color.OrangeRed;
                Buttom.ForeColor = Color.White;
                KillTwoSumPara += "," + Click;
            }
            else
            {
                //Buttom.Refresh();
                Buttom.BackColor = Control.DefaultBackColor;
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
    }
}
