using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WinFormsApp1
{
    public partial class frm_TrendAnalysis : Form
    {
        bool isFisrtClick = true;
        bool isTwoClick = true;
        bool isThreeClick = true;
        bool isFourthClick = true;
        bool isFiveClick = true;

        //標題 最大數值
        Series series1 = new Series("萬位", 10);
        Series series2 = new Series("千位", 10);
        Series series3 = new Series("百位", 10);
        Series series4 = new Series("十位", 10);
        Series series5 = new Series("个位", 10);

        Series series6 = new Series("遗漏", 120);

        string issueCount = "", playKind = "";
        int DrawCount = 0;
        DataTable dtHistoryNumber = new DataTable();
        string IssueCountForTopButton = "";

        public frm_TrendAnalysis(string count, string play)
        {
            InitializeComponent();
            issueCount = count;
            DrawCount = int.Parse(count);
            playKind = play;

            //設定起始按鈕
            if (frm_PlanCycle.GameLotteryName == "北京PK10")
            {
                btnType0.Visible = false;
                btnType10.Visible = true;
            }
            else if (frm_PlanCycle.GameLotteryName == "重庆时时彩" || frm_PlanCycle.GameLotteryName == "天津时时彩" || frm_PlanCycle.GameLotteryName == "腾讯奇趣彩" || frm_PlanCycle.GameLotteryName == "腾讯官方彩" || frm_PlanCycle.GameLotteryName == "VR金星1.5分彩")
            {
                btnType11.Visible = btnType10.Visible = false;
            }
            else if (frm_PlanCycle.GameLotteryName == "广东" || frm_PlanCycle.GameLotteryName == "河北" || frm_PlanCycle.GameLotteryName == "江苏" || frm_PlanCycle.GameLotteryName == "上海" || frm_PlanCycle.GameLotteryName == "江西" || frm_PlanCycle.GameLotteryName == "山东")
            {
                btnType0.Visible = false;
                btnType11.Visible = btnType10.Visible = true;
            }
        }

        private void frm_TrendAnalysis_Load(object sender, EventArgs e)
        {
            setIssueInit(issueCount);
            DrawChart(DrawCount);

            btnFirst.BackColor = Color.Blue;
            btnFirst.ForeColor = Color.White;

            btnSecond.BackColor = Color.Blue;
            btnSecond.ForeColor = Color.White;

            btnThree.BackColor = Color.Blue;
            btnThree.ForeColor = Color.White;

            btnFourth.BackColor = Color.Blue;
            btnFourth.ForeColor = Color.White;

            btnFive.BackColor = Color.Blue;
            btnFive.ForeColor = Color.White;

        }

        private void DrawChart(int IssueCount)
        {
            this.chart1.Series.Clear();
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            //chart1.Series[0].Points.Clear();
            dtHistoryNumber = getHistoryNumber();

            List<string> termsList = new List<string>();
            foreach (DataRow dr in dtHistoryNumber.Rows)
            {
                termsList.Add(dr["number"].ToString());
            }
            var arr = termsList.ToArray();

            series1 = new Series("萬位", 10);
            series2 = new Series("千位", 10);
            series3 = new Series("百位", 10);
            series4 = new Series("十位", 10);
            series5 = new Series("个位", 10);


            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisX2.Enabled = AxisEnabled.False;

            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;                    

            //設定線條顏色
            series1.Color = Color.Blue;
            series2.Color = Color.Red;
            series3.Color = Color.Green;
            series4.Color = Color.Yellow;
            series5.Color = Color.Pink;

            //設定字型
            series1.Font = new Font("標楷體", 12);
            series2.Font = new Font("標楷體", 12);
            series3.Font = new Font("標楷體", 12);
            series4.Font = new Font("標楷體", 12);
            series5.Font = new Font("標楷體", 12);

            //折線圖
            series1.ChartType = SeriesChartType.Line;
            series2.ChartType = SeriesChartType.Line;
            series3.ChartType = SeriesChartType.Line;
            series4.ChartType = SeriesChartType.Line;
            series5.ChartType = SeriesChartType.Line;

            //將數值顯示在線上
            series1.IsValueShownAsLabel = true;
            series2.IsValueShownAsLabel = true;
            series3.IsValueShownAsLabel = true;
            series4.IsValueShownAsLabel = true;
            series5.IsValueShownAsLabel = true;

            //將數值新增至序列
            double date = double.Parse(dtHistoryNumber.Rows[0]["Issue"].ToString());
            string Number = "";

            //遺漏用
            string subsringNumer = "";
            
            for (int index = 0; index < IssueCount; index++)
            {
                if (index >= arr.Count())
                    break;
                Number = arr[index].ToString();                             

                //遺漏
                if (frm_PlanCycle.GameLotteryName == "北京PK10")
                {
                    series1.Points.AddXY(date - index, double.Parse(Number.Replace(",", "").Substring(0, 2)));
                    series2.Points.AddXY(date - index, double.Parse(Number.Replace(",", "").Substring(2, 2)));
                    series3.Points.AddXY(date - index, double.Parse(Number.Replace(",", "").Substring(4, 2)));
                    series4.Points.AddXY(date - index, double.Parse(Number.Replace(",", "").Substring(6, 2)));
                    series5.Points.AddXY(date - index, double.Parse(Number.Replace(",", "").Substring(8, 2)));

                    if (playKind.Contains("前一"))
                    {
                        subsringNumer = Number.Substring(0, 2);
                    }
                    else if (playKind.Contains("前二"))
                    {
                        subsringNumer = Number.Substring(0, 5);
                    }
                    else if (playKind.Contains("前三"))
                    {
                        subsringNumer = Number.Substring(0, 8);
                    }
                    else if (playKind.Contains("前四"))
                    {
                        subsringNumer = Number.Substring(0, 11);
                    }
                    else if (playKind.Contains("前五"))
                    {
                        subsringNumer = Number.Substring(0, 14);
                    }
                }
                else if (frm_PlanCycle.GameLotteryName == "重庆时时彩" || frm_PlanCycle.GameLotteryName == "天津时时彩" || frm_PlanCycle.GameLotteryName == "腾讯奇趣彩" || frm_PlanCycle.GameLotteryName == "腾讯官方彩" || frm_PlanCycle.GameLotteryName == "VR金星1.5分彩")
                {
                    series1.Points.AddXY(date - index, double.Parse(Number.Substring(0, 1)));
                    series2.Points.AddXY(date - index, double.Parse(Number.Substring(1, 1)));
                    series3.Points.AddXY(date - index, double.Parse(Number.Substring(2, 1)));
                    series4.Points.AddXY(date - index, double.Parse(Number.Substring(3, 1)));
                    series5.Points.AddXY(date - index, double.Parse(Number.Substring(4, 1)));

                    if (playKind.Contains("前二"))
                    {
                        subsringNumer = Number.Substring(0, 2);
                    }
                    else if (playKind.Contains("后二"))
                    {
                        subsringNumer = Number.Substring(2, 3);
                    }
                    else if (playKind.Contains("前三"))
                    {
                        subsringNumer = Number.Substring(0, 3);
                    }
                    else if (playKind.Contains("中三"))
                    {
                        subsringNumer = Number.Substring(1, 3);
                    }
                    else if (playKind.Contains("后三"))
                    {
                        subsringNumer = Number.Substring(2, 3);
                    }
                }
                else if(frm_PlanCycle.GameLotteryName == "广东" || frm_PlanCycle.GameLotteryName == "河北" || frm_PlanCycle.GameLotteryName == "江苏" || frm_PlanCycle.GameLotteryName == "上海" || frm_PlanCycle.GameLotteryName == "江西" || frm_PlanCycle.GameLotteryName == "山东")
                {
                    series1.Points.AddXY(date - index, double.Parse(Number.Replace(",", "").Substring(0, 2)));
                    series2.Points.AddXY(date - index, double.Parse(Number.Replace(",", "").Substring(2, 2)));
                    series3.Points.AddXY(date - index, double.Parse(Number.Replace(",", "").Substring(4, 2)));
                    series4.Points.AddXY(date - index, double.Parse(Number.Replace(",", "").Substring(6, 2)));
                    series5.Points.AddXY(date - index, double.Parse(Number.Replace(",", "").Substring(8, 2)));

                    if (playKind.Contains("前二"))
                    {
                        subsringNumer = Number.Substring(0, 5);
                    }
                    else if (playKind.Contains("前三"))
                    {
                        subsringNumer = Number.Substring(0, 8);
                    }
                }
              
            }

            //將序列新增到圖上
            this.chart1.Series.Add(series1);         
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            this.chart1.Series.Add(series4);
            this.chart1.Series.Add(series5);

            //標題
            this.chart1.Titles.Clear();
            this.chart1.Titles.Add(frm_PlanCycle.GameLotteryName + "K線分析");
            this.chart1.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;

            this.lbChartKdesc.Titles.Clear();
            this.lbChartKdesc.Titles.Add(frm_PlanCycle.GameLotteryName + playKind +"遺漏分析");
            this.lbChartKdesc.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;
            //this.chart1.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = false;
        }

        private void DrawKline(string type)
        {

            lbChartKdesc.Series.Clear();
            foreach (var series in lbChartKdesc.Series)
            {
                series.Points.Clear();
            }
            Series seriesKhit = new Series("", 10);
            seriesKhit.ChartType = SeriesChartType.Candlestick;
            lbChartKdesc.Series.Add(seriesKhit);

            DataTable dtShow = dtHistoryNumber.Copy();

            dtShow.Columns.Add("isHit");
            dtShow.Columns.Add("High");

            int withStart = 0, High = 0;
            int breakcount = int.Parse(issueCount);
            string Number = "";

            dtShow = dtShow.Rows.Cast<System.Data.DataRow>().Take(int.Parse(issueCount)).OrderBy(x => x["Issue"]).CopyToDataTable();

            foreach (DataRow dr in dtShow.Rows)
            {
                if (breakcount == 0)
                    break;

                Number = dr["Number"].ToString();
                dr["High"] = High;
                if (frm_PlanCycle.GameLotteryName == "北京PK10")
                {
                    type = int.Parse(type).ToString("d2");
                    if (playKind == "前一")
                    {
                        Number = Number.Substring(0, 2);
                    }
                    else if (playKind == "前二")
                    {
                        Number = Number.Substring(0, 5);
                    }
                    else if (playKind == "前三")
                    {
                        Number = Number.Substring(0, 8);
                    }
                    else if (playKind == "前四")
                    {
                        Number = Number.Substring(0, 11);
                    }
                    else if (playKind == "前五")
                    {
                        Number = Number.Substring(0, 14);
                    }
                }
                else if (frm_PlanCycle.GameLotteryName == "重庆时时彩" || frm_PlanCycle.GameLotteryName == "天津时时彩" || frm_PlanCycle.GameLotteryName == "腾讯奇趣彩" || frm_PlanCycle.GameLotteryName == "腾讯官方彩" || frm_PlanCycle.GameLotteryName == "VR金星1.5分彩")
                {
                    if (playKind == "前二")
                    {
                        Number = Number.Substring(0, 2);
                    }
                    else if (playKind == "后二")
                    {
                        Number = Number.Substring(2, 3);
                    }
                    else if (playKind == "前三")
                    {
                        Number = Number.Substring(0, 3);
                    }
                    else if (playKind == "中三")
                    {
                        Number = Number.Substring(1, 3);
                    }
                    else if (playKind == "后三")
                    {
                        Number = Number.Substring(2, 3);
                    }
                }
                else if (frm_PlanCycle.GameLotteryName == "广东" || frm_PlanCycle.GameLotteryName == "河北" || frm_PlanCycle.GameLotteryName == "江苏" || frm_PlanCycle.GameLotteryName == "上海" || frm_PlanCycle.GameLotteryName == "江西" || frm_PlanCycle.GameLotteryName == "山东")
                {
                    type = int.Parse(type).ToString("d2");
                    if (playKind == "前二")
                    {
                        Number = Number.Substring(0, 5);
                    }
                    else if (playKind == "前三")
                    {
                        Number = Number.Substring(0, 8);
                    }
                }
              

                if (Number.Contains(type))
                {
                    withStart += 10;
                    High = withStart;
                    dr["isHit"] = withStart;
                }
                else
                {
                    withStart -= 3;
                    High = withStart;
                    dr["isHit"] = withStart;
                }

                breakcount--;
            }          

            //clear
            lbChartKdesc.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            lbChartKdesc.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

            //設定圖樣                     
            lbChartKdesc.Series[0].XValueMember = "Issue";
            lbChartKdesc.Series[0].YValueMembers = "isHit,High,isHit,High";
            lbChartKdesc.Series[0].CustomProperties = "PriceDownColor=Red, PriceUpColor=Blue";

            lbChartKdesc.Series[0]["OpenCloseStyle"] = "Triangle";
            lbChartKdesc.Series[0]["ShowOpenClose"] = "Both";
            lbChartKdesc.ChartAreas["ChartArea1"].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            lbChartKdesc.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = false;
            //lbChartKdesc.DataManipulator.IsStartFromFirst = true;
            lbChartKdesc.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;
            lbChartKdesc.Series[0]["PointWidth"] = "0.2";
            lbChartKdesc.DataSource = dtShow;
            lbChartKdesc.DataBind();

            //繪製折線圖
            double Topcount = 0, Downcount = 0, Avcount = 0;
            Series seriesKlinechartTop = new Series("", 3);
            Series seriesKlinechartDown = new Series("", 3);
            Series seriesKlinechartAvege = new Series("", 3);
            seriesKlinechartTop.ChartType = seriesKlinechartDown.ChartType = seriesKlinechartAvege.ChartType = SeriesChartType.Spline;
            int i = 0;
            foreach (DataRow dr in dtShow.Rows)
            {
                //上界
                Topcount = double.Parse(dr["High"].ToString());
                if(i == 0)
                    Topcount = double.Parse(dtShow.Rows[i]["isHit"].ToString());
                seriesKlinechartTop.Points.Add(Topcount);

                //下界
                Downcount = double.Parse(dr["isHit"].ToString());
                if (i == 0)
                    Downcount = double.Parse(dtShow.Rows[i]["High"].ToString());
                seriesKlinechartDown.Points.Add(Downcount);

                //均線
                if (i == 0)
                    Avcount = double.Parse(dr["isHit"].ToString()) / 2;
                else
                    Avcount = (double.Parse(dtShow.Rows[i - 1]["isHit"].ToString())+ double.Parse(dr["isHit"].ToString())) / 2;
               

                seriesKlinechartAvege.Points.Add(Avcount);
                i++;
            }
            lbChartKdesc.Series.Add(seriesKlinechartTop);
            lbChartKdesc.Series.Add(seriesKlinechartDown);
            lbChartKdesc.Series.Add(seriesKlinechartAvege);
            lbChartKdesc.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;
            
        }

        private DataTable getHistoryNumber()
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";

            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
            con = new SqlConnection(connetionString);
            string date = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            string Sqlstr = "";
            string db = "";
            try
            {
                switch (frm_PlanCycle.GameLotteryName)
                {
                    case "重庆时时彩":
                        db = "HistoryNumber";
                        break;
                    case "天津时时彩":
                        db = "TJSSC_HistoryNumber";
                        break;
                    case "腾讯官方彩":
                        db = "QQFFC_HistoryNumber";
                        break;
                    case "腾讯奇趣彩":
                        db = "TENCENTFFC_HistoryNumber";
                        break;
                    case "新疆时时彩":
                        db = "XJSSC_HistoryNumber";
                        break;
                    case "广东":
                        db = "GD115_HistoryNumber";
                        break;
                    case "北京PK10":
                        db = "PK10_HistoryNumber";
                        break;
                }

                con.Open();
                if (frm_PlanCycle.GameLotteryName == "北京PK10")
                {
                    Sqlstr = "SELECT * FROM " + db +" ORDER BY ISSUE DESC";
                }
                else
                { 
                    Sqlstr = "SELECT * FROM "+ db + " WHERE ISSUE LIKE '"+ date + "%' ORDER BY ISSUE DESC";
                }
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

        private void setIssueInit(string type)
        {
            if(type == "50")
            { 
                btn50Issue.BackColor = Color.Blue;
                btn50Issue.ForeColor = Color.White;

                btn30Issue.BackColor = Color.LightGray;
                btn30Issue.ForeColor = Color.Black;

                btn10Issue.BackColor = Color.LightGray;
                btn10Issue.ForeColor = Color.Black;
            }
            else if (type == "30")
            {
                btn50Issue.BackColor = Color.LightGray;
                btn50Issue.ForeColor = Color.Black;

                btn30Issue.BackColor = Color.Blue;
                btn30Issue.ForeColor = Color.White;

                btn10Issue.BackColor = Color.LightGray;
                btn10Issue.ForeColor = Color.Black;
            }
            else if(type == "10")
            {
                btn50Issue.BackColor = Color.LightGray;
                btn50Issue.ForeColor = Color.Black;

                btn30Issue.BackColor = Color.LightGray;
                btn30Issue.ForeColor = Color.Black;

                btn10Issue.BackColor = Color.Blue;
                btn10Issue.ForeColor = Color.White;
            }
        }

        private void btn50Issue_Click(object sender, EventArgs e)
        {
            setIssueInit("50");
            DrawChart(50);

            if (IssueCountForTopButton != "")
            {
                issueCount = "50";
                DrawKline(IssueCountForTopButton);
            }
        }

        private void btn30Issue_Click(object sender, EventArgs e)
        {
            setIssueInit("30");
            DrawChart(30);
            if (IssueCountForTopButton != "")
            {
                issueCount = "30";
                DrawKline(IssueCountForTopButton);
            }
        }

        private void btn10Issue_Click(object sender, EventArgs e)
        {
            setIssueInit("10");
            DrawChart(10);
            if (IssueCountForTopButton != "")
            {
                issueCount = "10";
                DrawKline(IssueCountForTopButton);
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (isFisrtClick)
            {
                btnFirst.BackColor = Color.LightGray;
                btnFirst.ForeColor = Color.Black;
                this.chart1.Series.Remove(series1);
                isFisrtClick = false;
            }
            else
            {
                btnFirst.BackColor = Color.Blue;
                btnFirst.ForeColor = Color.White;
                this.chart1.Series.Add(series1);
                isFisrtClick = true;
            }
        }

        private void btnSecond_Click(object sender, EventArgs e)
        {
            if (isTwoClick)
            {
                btnSecond.BackColor = Color.LightGray;
                btnSecond.ForeColor = Color.Black;
                this.chart1.Series.Remove(series2);
                isTwoClick = false;
            }
            else
            {
                btnSecond.BackColor = Color.Blue;
                btnSecond.ForeColor = Color.White;
                this.chart1.Series.Add(series2);
                isTwoClick = true;
            }
        }

        private void btnThree_Click(object sender, EventArgs e)
        {
            if (isThreeClick)
            {
                btnThree.BackColor = Color.LightGray;
                btnThree.ForeColor = Color.Black;
                this.chart1.Series.Remove(series3);
                isThreeClick = false;
            }
            else
            {
                btnThree.BackColor = Color.Blue;
                btnThree.ForeColor = Color.White;
                this.chart1.Series.Add(series3);
                isThreeClick = true;
            }
        }

        private void btnFourth_Click(object sender, EventArgs e)
        {
            if (isFourthClick)
            {
                btnFourth.BackColor = Color.LightGray;
                btnFourth.ForeColor = Color.Black;
                this.chart1.Series.Remove(series4);
                isFourthClick = false;
            }
            else
            {
                btnFourth.BackColor = Color.Blue;
                btnFourth.ForeColor = Color.White;
                this.chart1.Series.Add(series4);
                isFourthClick = true;
            }
        }

        private void btnFive_Click(object sender, EventArgs e)
        {
            if (isFiveClick)
            {
                btnFive.BackColor = Color.LightGray;
                btnFive.ForeColor = Color.Black;
                this.chart1.Series.Remove(series5);
                isFiveClick = false;
            }
            else
            {
                btnFive.BackColor = Color.Blue;
                btnFive.ForeColor = Color.White;
                this.chart1.Series.Add(series5);
                isFiveClick = true;
            }
        }

        private void btnType0_Click(object sender, EventArgs e)
        {
            string type = (sender as Button).Text;
            DrawKline(type);
            IssueCountForTopButton = "0";
        }

        private void btnType1_Click(object sender, EventArgs e)
        {
            string type = (sender as Button).Text;
            DrawKline(type);
            IssueCountForTopButton = "1";
        }

        private void btnType2_Click(object sender, EventArgs e)
        {
            string type = (sender as Button).Text;
            DrawKline(type);
            IssueCountForTopButton = "2";
        }

        private void btnType3_Click(object sender, EventArgs e)
        {
            string type = (sender as Button).Text;
            DrawKline(type);
            IssueCountForTopButton = "3";
        }

        private void btnType4_Click(object sender, EventArgs e)
        {
            string type = (sender as Button).Text;
            DrawKline(type);
            IssueCountForTopButton = "4";
        }

        private void btnType5_Click(object sender, EventArgs e)
        {
            string type = (sender as Button).Text;
            DrawKline(type);
            IssueCountForTopButton = "5";
        }

        private void btnType6_Click(object sender, EventArgs e)
        {
            string type = (sender as Button).Text;
            DrawKline(type);
            IssueCountForTopButton = "6";
        }

        private void btnType7_Click(object sender, EventArgs e)
        {
            string type = (sender as Button).Text;
            DrawKline(type);
            IssueCountForTopButton = "7";
        }

        private void btnType8_Click(object sender, EventArgs e)
        {
            string type = (sender as Button).Text;
            DrawKline(type);
            IssueCountForTopButton = "8";
        }

        private void btnType9_Click(object sender, EventArgs e)
        {
            string type = (sender as Button).Text;
            DrawKline(type);
            IssueCountForTopButton = "9";
        }

        private void btnType10_Click(object sender, EventArgs e)
        {
            string type = (sender as Button).Text;
            DrawKline(type);
            IssueCountForTopButton = "10";
        }

        private void btnType11_Click(object sender, EventArgs e)
        {
            string type = (sender as Button).Text;
            DrawKline(type);
            IssueCountForTopButton = "11";
        }

        private void btnRefrash_Click(object sender, EventArgs e)
        {
            setIssueInit("50");
            DrawChart(50);
            if(IssueCountForTopButton != "")
                DrawKline(IssueCountForTopButton);

            btnFirst.BackColor = Color.Blue;
            btnFirst.ForeColor = Color.White;

            btnSecond.BackColor = Color.Blue;
            btnSecond.ForeColor = Color.White;

            btnThree.BackColor = Color.Blue;
            btnThree.ForeColor = Color.White;

            btnFourth.BackColor = Color.Blue;
            btnFourth.ForeColor = Color.White;

            btnFive.BackColor = Color.Blue;
            btnFive.ForeColor = Color.White;
        }
    }
}
