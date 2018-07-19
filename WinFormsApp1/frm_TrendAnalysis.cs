using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WinFormsApp1
{
    public partial class frm_TrendAnalysis : Form
    {
        public frm_TrendAnalysis()
        {
            InitializeComponent();
        }

        private void frm_TrendAnalysis_Load(object sender, EventArgs e)
        {
            int[,] array = new int[,] {
            {0,1,2,3,4,5,6,7,8,9},
            {9,8,7,6,5,4,3,2,1,0},
            {2,5,8,7,9,6,4,3,0,2},
            {8,1,4,3,1,5,6,9,2,0},
            {3,6,8,9,8,4,6,1,5,7}
            };


            //標題 最大數值
            Series series1 = new Series("萬位", 10);
            Series series2 = new Series("千位", 10);
            Series series3 = new Series("百位", 10);
            Series series4 = new Series("十位", 10);
            Series series5 = new Series("个位", 10);

            //設定線條顏色
            series1.Color = Color.Blue;
            series2.Color = Color.Red;
            series3.Color = Color.Green;
            series4.Color = Color.Yellow;
            series5.Color = Color.Pink;

            //設定字型
            series1.Font = new System.Drawing.Font("新細明體", 14);
            series2.Font = new System.Drawing.Font("標楷體", 12);
            series3.Font = new System.Drawing.Font("標楷體", 12);
            series4.Font = new System.Drawing.Font("標楷體", 12);
            series5.Font = new System.Drawing.Font("標楷體", 12);

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
            for (int index = 1; index < 10; index++)
            {
                series1.Points.AddXY(index, array[0, index]);
                series2.Points.AddXY(index, array[1, index]);
                series3.Points.AddXY(index, array[2, index]);
                series4.Points.AddXY(index, array[3, index]);
                series5.Points.AddXY(index, array[4, index]);
            }

            //將序列新增到圖上
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            this.chart1.Series.Add(series4);
            this.chart1.Series.Add(series5);

            //標題
            this.chart1.Titles.Add("標題");
        }
    }
}
