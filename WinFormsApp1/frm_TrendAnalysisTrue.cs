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
    public partial class frm_TrendAnalysisTrue : Form
    {
        public frm_TrendAnalysisTrue()
        {
            InitializeComponent();
            cbplayNumber.SelectedIndex = 0;
            DataTable dt = connectDb(10);
            ColdHotNumber(dt);
        }

        private void ColdHotNumber(DataTable dt)
        {
            string number = "";
            
            //計算冷號熱號溫號用
            double baseCount = 0;
            int countOne = 0, countTwo = 0, countThree = 0, countFour = 0, countFive = 0, countSix = 0, countSeven = 0, countEight = 0, countNine = 0, countZero = 0;
            double avergeOne = 0, avergeTwo = 0, avergeThree = 0, avergeFour = 0, avergeFive = 0, avergeSix = 0, avergeSeven = 0, avergeEight = 0, avergeNine = 0, avergeZero = 0;
            string coldNumber = "", hotNumber = "", WormNumber = "";

            foreach (DataRow dr in dt.Rows)
            {
                baseCount += 5;
                //number = dr["number"].ToString();
                //放開獎號碼
                if (cbplayNumber.Text == "前二")
                {
                    number = dr["number"].ToString().Substring(0, 2);
                    //baseCount = 2 * issueCount;
                }
                else if (cbplayNumber.Text == "后二")
                {
                    number = dr["number"].ToString().Substring(3, 2);
                    //baseCount = 2 * issueCount;
                }
                else if (cbplayNumber.Text == "前三")
                {
                    number = dr["number"].ToString().Substring(0, 3);
                    //baseCount = 3 * issueCount;
                }
                else if (cbplayNumber.Text == "中三")
                {
                    number = dr["number"].ToString().Substring(1, 3);
                    //baseCount = 3 * issueCount;
                }
                else if (cbplayNumber.Text == "后三")
                {
                    number = dr["number"].ToString().Substring(2, 3);
                    //baseCount = 3 * issueCount;
                }
                dr["playNumber"] = number;


                //計算冷號 熱號 溫號
                #region

                if (cbplayNumber.Text.Contains("二"))
                {
                    //第一個號碼
                    switch (number.Substring(0, 1))
                    {
                        case "1":
                            countOne++;
                            break;
                        case "2":
                            countTwo++;
                            break;
                        case "3":
                            countThree++;
                            break;
                        case "4":
                            countFour++;
                            break;
                        case "5":
                            countFive++;
                            break;
                        case "6":
                            countSix++;
                            break;
                        case "7":
                            countSeven++;
                            break;
                        case "8":
                            countEight++;
                            break;
                        case "9":
                            countNine++;
                            break;
                        case "0":
                            countZero++;
                            break;
                    }

                    //第二個號碼
                    switch (number.Substring(1, 1))
                    {
                        case "1":
                            countOne++;
                            break;
                        case "2":
                            countTwo++;
                            break;
                        case "3":
                            countThree++;
                            break;
                        case "4":
                            countFour++;
                            break;
                        case "5":
                            countFive++;
                            break;
                        case "6":
                            countSix++;
                            break;
                        case "7":
                            countSeven++;
                            break;
                        case "8":
                            countEight++;
                            break;
                        case "9":
                            countNine++;
                            break;
                        case "0":
                            countZero++;
                            break;
                    }
                }
                else if (cbplayNumber.Text.Contains("三"))
                {

                    //第一個號碼
                    switch (number.Substring(0, 1))
                    {
                        case "1":
                            countOne++;
                            break;
                        case "2":
                            countTwo++;
                            break;
                        case "3":
                            countThree++;
                            break;
                        case "4":
                            countFour++;
                            break;
                        case "5":
                            countFive++;
                            break;
                        case "6":
                            countSix++;
                            break;
                        case "7":
                            countSeven++;
                            break;
                        case "8":
                            countEight++;
                            break;
                        case "9":
                            countNine++;
                            break;
                        case "0":
                            countZero++;
                            break;
                    }

                    //第二個號碼
                    switch (number.Substring(1, 1))
                    {
                        case "1":
                            countOne++;
                            break;
                        case "2":
                            countTwo++;
                            break;
                        case "3":
                            countThree++;
                            break;
                        case "4":
                            countFour++;
                            break;
                        case "5":
                            countFive++;
                            break;
                        case "6":
                            countSix++;
                            break;
                        case "7":
                            countSeven++;
                            break;
                        case "8":
                            countEight++;
                            break;
                        case "9":
                            countNine++;
                            break;
                        case "0":
                            countZero++;
                            break;
                    }

                    //第三個號碼
                    switch (number.Substring(2, 1))
                    {
                        case "1":
                            countOne++;
                            break;
                        case "2":
                            countTwo++;
                            break;
                        case "3":
                            countThree++;
                            break;
                        case "4":
                            countFour++;
                            break;
                        case "5":
                            countFive++;
                            break;
                        case "6":
                            countSix++;
                            break;
                        case "7":
                            countSeven++;
                            break;
                        case "8":
                            countEight++;
                            break;
                        case "9":
                            countNine++;
                            break;
                        case "0":
                            countZero++;
                            break;
                    }
                }

                avergeOne = Math.Round((countOne / baseCount), 2, MidpointRounding.AwayFromZero) * 100;
                avergeTwo = Math.Round((countTwo / baseCount), 2, MidpointRounding.AwayFromZero) * 100;
                avergeThree = Math.Round((countThree / baseCount), 2, MidpointRounding.AwayFromZero) * 100;
                avergeFour = Math.Round((countFour / baseCount), 2, MidpointRounding.AwayFromZero) * 100;
                avergeFive = Math.Round((countFive / baseCount), 2, MidpointRounding.AwayFromZero) * 100;
                avergeSix = Math.Round((countSix / baseCount), 2, MidpointRounding.AwayFromZero) * 100;
                avergeSeven = Math.Round((countSeven / baseCount), 2, MidpointRounding.AwayFromZero) * 100;
                avergeEight = Math.Round((countEight / baseCount), 2, MidpointRounding.AwayFromZero) * 100;
                avergeNine = Math.Round((countNine / baseCount), 2, MidpointRounding.AwayFromZero) * 100;
                avergeZero = Math.Round((countZero / baseCount), 2, MidpointRounding.AwayFromZero) * 100;

                WormNumber = ""; hotNumber = ""; coldNumber = "";

                if (avergeOne == 10)
                {
                    WormNumber += "1";
                }
                else if (avergeOne > 10)
                {
                    hotNumber += "1";
                }
                else if (avergeOne < 10)
                {
                    coldNumber += "1";
                }

                if (avergeTwo == 10)
                {
                    WormNumber += "2";
                }
                else if (avergeTwo > 10)
                {
                    hotNumber += "2";
                }
                else if (avergeTwo < 10)
                {
                    coldNumber += "2";
                }

                if (avergeThree == 10)
                {
                    WormNumber += "3";
                }
                else if (avergeThree > 10)
                {
                    hotNumber += "3";
                }
                else if (avergeThree < 10)
                {
                    coldNumber += "3";
                }

                if (avergeFour == 10)
                {
                    WormNumber += "4";
                }
                else if (avergeFour > 10)
                {
                    hotNumber += "4";
                }
                else if (avergeFour < 10)
                {
                    coldNumber += "4";
                }

                if (avergeFive == 10)
                {
                    WormNumber += "5";
                }
                else if (avergeFive > 10)
                {
                    hotNumber += "5";
                }
                else if (avergeFive < 10)
                {
                    coldNumber += "5";
                }

                if (avergeSix == 10)
                {
                    WormNumber += "6";
                }
                else if (avergeSix > 10)
                {
                    hotNumber += "6";
                }
                else if (avergeSix < 10)
                {
                    coldNumber += "6";
                }

                if (avergeSeven == 10)
                {
                    WormNumber += "7";
                }
                else if (avergeSeven > 10)
                {
                    hotNumber += "7";
                }
                else if (avergeSeven < 10)
                {
                    coldNumber += "7";
                }

                if (avergeEight == 10)
                {
                    WormNumber += "8";
                }
                else if (avergeEight > 10)
                {
                    hotNumber += "8";
                }
                else if (avergeEight < 10)
                {
                    coldNumber += "8";
                }

                if (avergeNine == 10)
                {
                    WormNumber += "9";
                }
                else if (avergeNine > 10)
                {
                    hotNumber += "9";
                }
                else if (avergeNine < 10)
                {
                    coldNumber += "9";
                }

                if (avergeZero == 10)
                {
                    WormNumber += "0";
                }
                else if (avergeZero > 10)
                {
                    hotNumber += "0";
                }
                else if (avergeZero < 10)
                {
                    coldNumber += "0";
                }

                #endregion

                dr["coldnumber"] = coldNumber.ToString();
                dr["hotnumber"] = hotNumber.ToString();
                dr["wormnumber"] = WormNumber.ToString();
            }

            dgShowTrend.DataSource = dt;
        }

        private DataTable connectDb(int topCount)
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";

            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
            con = new SqlConnection(connetionString);
            string SelectNowDate = DateTime.Now.ToString("yyyyMMdd");
            try
            {
                con.Open();

                string Sqlstr = @"SELECT top (" + topCount + ") issue AS Issue,''coldnumber,''wormnumber, ''hotnumber,''playNumber, number AS Number FROM HistoryNumber where issue like '" + SelectNowDate + "%'order by issue desc";
                SqlDataAdapter da = new SqlDataAdapter(Sqlstr, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }
    }
}
