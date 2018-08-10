namespace WinFormsApp1
{
    partial class frm_TrendAnalysis
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btn50Issue = new System.Windows.Forms.Button();
            this.btn10Issue = new System.Windows.Forms.Button();
            this.btn30Issue = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnSecond = new System.Windows.Forms.Button();
            this.btnThree = new System.Windows.Forms.Button();
            this.btnFive = new System.Windows.Forms.Button();
            this.btnFourth = new System.Windows.Forms.Button();
            this.btnRefrash = new System.Windows.Forms.Button();
            this.chartKline = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnType1 = new System.Windows.Forms.Button();
            this.btnType2 = new System.Windows.Forms.Button();
            this.btnType3 = new System.Windows.Forms.Button();
            this.btnType4 = new System.Windows.Forms.Button();
            this.btnType5 = new System.Windows.Forms.Button();
            this.btnType6 = new System.Windows.Forms.Button();
            this.btnType7 = new System.Windows.Forms.Button();
            this.btnType8 = new System.Windows.Forms.Button();
            this.btnType0 = new System.Windows.Forms.Button();
            this.btnType9 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartKline)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(4, 102);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(1217, 368);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // btn50Issue
            // 
            this.btn50Issue.Location = new System.Drawing.Point(12, 68);
            this.btn50Issue.Name = "btn50Issue";
            this.btn50Issue.Size = new System.Drawing.Size(75, 23);
            this.btn50Issue.TabIndex = 1;
            this.btn50Issue.Text = "最近50期";
            this.btn50Issue.UseVisualStyleBackColor = true;
            this.btn50Issue.Click += new System.EventHandler(this.btn50Issue_Click);
            // 
            // btn10Issue
            // 
            this.btn10Issue.BackColor = System.Drawing.Color.LightGray;
            this.btn10Issue.ForeColor = System.Drawing.Color.Black;
            this.btn10Issue.Location = new System.Drawing.Point(174, 68);
            this.btn10Issue.Name = "btn10Issue";
            this.btn10Issue.Size = new System.Drawing.Size(75, 23);
            this.btn10Issue.TabIndex = 2;
            this.btn10Issue.Text = "最近10期";
            this.btn10Issue.UseVisualStyleBackColor = false;
            this.btn10Issue.Click += new System.EventHandler(this.btn10Issue_Click);
            // 
            // btn30Issue
            // 
            this.btn30Issue.Location = new System.Drawing.Point(93, 68);
            this.btn30Issue.Name = "btn30Issue";
            this.btn30Issue.Size = new System.Drawing.Size(75, 23);
            this.btn30Issue.TabIndex = 3;
            this.btn30Issue.Text = "最近30期";
            this.btn30Issue.UseVisualStyleBackColor = true;
            this.btn30Issue.Click += new System.EventHandler(this.btn30Issue_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(697, 68);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(75, 23);
            this.btnFirst.TabIndex = 4;
            this.btnFirst.Text = "萬位";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnSecond
            // 
            this.btnSecond.Location = new System.Drawing.Point(778, 68);
            this.btnSecond.Name = "btnSecond";
            this.btnSecond.Size = new System.Drawing.Size(75, 23);
            this.btnSecond.TabIndex = 5;
            this.btnSecond.Text = "千位";
            this.btnSecond.UseVisualStyleBackColor = true;
            this.btnSecond.Click += new System.EventHandler(this.btnSecond_Click);
            // 
            // btnThree
            // 
            this.btnThree.Location = new System.Drawing.Point(859, 68);
            this.btnThree.Name = "btnThree";
            this.btnThree.Size = new System.Drawing.Size(75, 23);
            this.btnThree.TabIndex = 6;
            this.btnThree.Text = "百位";
            this.btnThree.UseVisualStyleBackColor = true;
            this.btnThree.Click += new System.EventHandler(this.btnThree_Click);
            // 
            // btnFive
            // 
            this.btnFive.Location = new System.Drawing.Point(1021, 68);
            this.btnFive.Name = "btnFive";
            this.btnFive.Size = new System.Drawing.Size(75, 23);
            this.btnFive.TabIndex = 7;
            this.btnFive.Text = "个位";
            this.btnFive.UseVisualStyleBackColor = true;
            this.btnFive.Click += new System.EventHandler(this.btnFive_Click);
            // 
            // btnFourth
            // 
            this.btnFourth.Location = new System.Drawing.Point(940, 68);
            this.btnFourth.Name = "btnFourth";
            this.btnFourth.Size = new System.Drawing.Size(75, 23);
            this.btnFourth.TabIndex = 8;
            this.btnFourth.Text = "十位";
            this.btnFourth.UseVisualStyleBackColor = true;
            this.btnFourth.Click += new System.EventHandler(this.btnFourth_Click);
            // 
            // btnRefrash
            // 
            this.btnRefrash.Location = new System.Drawing.Point(1102, 68);
            this.btnRefrash.Name = "btnRefrash";
            this.btnRefrash.Size = new System.Drawing.Size(75, 23);
            this.btnRefrash.TabIndex = 9;
            this.btnRefrash.Text = "刷新";
            this.btnRefrash.UseVisualStyleBackColor = true;
            this.btnRefrash.Click += new System.EventHandler(this.btnRefrash_Click);
            // 
            // chartKline
            // 
            chartArea2.Name = "ChartArea1";
            this.chartKline.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartKline.Legends.Add(legend2);
            this.chartKline.Location = new System.Drawing.Point(4, 520);
            this.chartKline.Name = "chartKline";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            series2.YValuesPerPoint = 4;
            this.chartKline.Series.Add(series2);
            this.chartKline.Size = new System.Drawing.Size(1217, 356);
            this.chartKline.TabIndex = 10;
            this.chartKline.Text = "chart2";
            // 
            // btnType1
            // 
            this.btnType1.Location = new System.Drawing.Point(93, 491);
            this.btnType1.Name = "btnType1";
            this.btnType1.Size = new System.Drawing.Size(29, 23);
            this.btnType1.TabIndex = 11;
            this.btnType1.Text = "1";
            this.btnType1.UseVisualStyleBackColor = true;
            // 
            // btnType2
            // 
            this.btnType2.Location = new System.Drawing.Point(128, 491);
            this.btnType2.Name = "btnType2";
            this.btnType2.Size = new System.Drawing.Size(29, 23);
            this.btnType2.TabIndex = 12;
            this.btnType2.Text = "2";
            this.btnType2.UseVisualStyleBackColor = true;
            // 
            // btnType3
            // 
            this.btnType3.Location = new System.Drawing.Point(163, 491);
            this.btnType3.Name = "btnType3";
            this.btnType3.Size = new System.Drawing.Size(28, 23);
            this.btnType3.TabIndex = 13;
            this.btnType3.Text = "3";
            this.btnType3.UseVisualStyleBackColor = true;
            // 
            // btnType4
            // 
            this.btnType4.Location = new System.Drawing.Point(197, 491);
            this.btnType4.Name = "btnType4";
            this.btnType4.Size = new System.Drawing.Size(29, 23);
            this.btnType4.TabIndex = 14;
            this.btnType4.Text = "4";
            this.btnType4.UseVisualStyleBackColor = true;
            // 
            // btnType5
            // 
            this.btnType5.Location = new System.Drawing.Point(232, 491);
            this.btnType5.Name = "btnType5";
            this.btnType5.Size = new System.Drawing.Size(28, 23);
            this.btnType5.TabIndex = 15;
            this.btnType5.Text = "5";
            this.btnType5.UseVisualStyleBackColor = true;
            // 
            // btnType6
            // 
            this.btnType6.Location = new System.Drawing.Point(266, 491);
            this.btnType6.Name = "btnType6";
            this.btnType6.Size = new System.Drawing.Size(28, 23);
            this.btnType6.TabIndex = 16;
            this.btnType6.Text = "6";
            this.btnType6.UseVisualStyleBackColor = true;
            // 
            // btnType7
            // 
            this.btnType7.Location = new System.Drawing.Point(300, 491);
            this.btnType7.Name = "btnType7";
            this.btnType7.Size = new System.Drawing.Size(28, 23);
            this.btnType7.TabIndex = 17;
            this.btnType7.Text = "7";
            this.btnType7.UseVisualStyleBackColor = true;
            // 
            // btnType8
            // 
            this.btnType8.Location = new System.Drawing.Point(334, 491);
            this.btnType8.Name = "btnType8";
            this.btnType8.Size = new System.Drawing.Size(28, 23);
            this.btnType8.TabIndex = 18;
            this.btnType8.Text = "8";
            this.btnType8.UseVisualStyleBackColor = true;
            // 
            // btnType0
            // 
            this.btnType0.Location = new System.Drawing.Point(59, 491);
            this.btnType0.Name = "btnType0";
            this.btnType0.Size = new System.Drawing.Size(28, 23);
            this.btnType0.TabIndex = 19;
            this.btnType0.Text = "0";
            this.btnType0.UseVisualStyleBackColor = true;
            this.btnType0.Click += new System.EventHandler(this.btnType0_Click);
            // 
            // btnType9
            // 
            this.btnType9.Location = new System.Drawing.Point(368, 491);
            this.btnType9.Name = "btnType9";
            this.btnType9.Size = new System.Drawing.Size(28, 23);
            this.btnType9.TabIndex = 20;
            this.btnType9.Text = "9";
            this.btnType9.UseVisualStyleBackColor = true;
            // 
            // frm_TrendAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.ClientSize = new System.Drawing.Size(1224, 888);
            this.Controls.Add(this.btnType9);
            this.Controls.Add(this.btnType0);
            this.Controls.Add(this.btnType8);
            this.Controls.Add(this.btnType7);
            this.Controls.Add(this.btnType6);
            this.Controls.Add(this.btnType5);
            this.Controls.Add(this.btnType4);
            this.Controls.Add(this.btnType3);
            this.Controls.Add(this.btnType2);
            this.Controls.Add(this.btnType1);
            this.Controls.Add(this.chartKline);
            this.Controls.Add(this.btnRefrash);
            this.Controls.Add(this.btnFourth);
            this.Controls.Add(this.btnFive);
            this.Controls.Add(this.btnThree);
            this.Controls.Add(this.btnSecond);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.btn30Issue);
            this.Controls.Add(this.btn10Issue);
            this.Controls.Add(this.btn50Issue);
            this.Controls.Add(this.chart1);
            this.Name = "frm_TrendAnalysis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "趨勢分析";
            this.Load += new System.EventHandler(this.frm_TrendAnalysis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartKline)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button btn50Issue;
        private System.Windows.Forms.Button btn10Issue;
        private System.Windows.Forms.Button btn30Issue;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnSecond;
        private System.Windows.Forms.Button btnThree;
        private System.Windows.Forms.Button btnFive;
        private System.Windows.Forms.Button btnFourth;
        private System.Windows.Forms.Button btnRefrash;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartKline;
        private System.Windows.Forms.Button btnType1;
        private System.Windows.Forms.Button btnType2;
        private System.Windows.Forms.Button btnType3;
        private System.Windows.Forms.Button btnType4;
        private System.Windows.Forms.Button btnType5;
        private System.Windows.Forms.Button btnType6;
        private System.Windows.Forms.Button btnType7;
        private System.Windows.Forms.Button btnType8;
        private System.Windows.Forms.Button btnType0;
        private System.Windows.Forms.Button btnType9;
    }
}