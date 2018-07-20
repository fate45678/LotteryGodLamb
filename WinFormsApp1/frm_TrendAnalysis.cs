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

        public frm_TrendAnalysis()
        {
            InitializeComponent();
        }

        private void frm_TrendAnalysis_Load(object sender, EventArgs e)
        {
            setIssueInit("50");
            DrawChart(50);

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
            DataTable dt = getHistoryNumber();

            List<string> termsList = new List<string>();
            foreach (DataRow dr in dt.Rows)
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
            double date = double.Parse(dt.Rows[0]["Issue"].ToString());
            string Number = "";
            for (int index = 0; index < IssueCount; index++)
            {
                Number = arr[index].ToString();
                              
                series1.Points.AddXY(date - index, double.Parse(Number.Substring(0, 1)));
                series2.Points.AddXY(date - index, double.Parse(Number.Substring(1, 1)));
                series3.Points.AddXY(date - index, double.Parse(Number.Substring(2, 1)));
                series4.Points.AddXY(date - index, double.Parse(Number.Substring(3, 1)));
                series5.Points.AddXY(date - index, double.Parse(Number.Substring(4, 1)));
            }

            //將序列新增到圖上

            this.chart1.Series.Add(series1);
            //DataPoint dp1 = new DataPoint(series1);            
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            this.chart1.Series.Add(series4);
            this.chart1.Series.Add(series5);

            //標題
            this.chart1.Titles.Clear();
            this.chart1.Titles.Add(frm_PlanCycle.GameLotteryName + "K線分析");

            //this.chart1.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = false;
            this.chart1.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;
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
            try
            {
                con.Open();
                Sqlstr = "SELECT * FROM HistoryNumber WHERE ISSUE LIKE '"+ date + "%' ORDER BY ISSUE DESC";
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
        }

        private void btn30Issue_Click(object sender, EventArgs e)
        {
            setIssueInit("30");
            DrawChart(30);
        }

        private void btn10Issue_Click(object sender, EventArgs e)
        {
            setIssueInit("10");
            DrawChart(10);
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

        private void btnRefrash_Click(object sender, EventArgs e)
        {
            setIssueInit("50");
            DrawChart(50);

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
