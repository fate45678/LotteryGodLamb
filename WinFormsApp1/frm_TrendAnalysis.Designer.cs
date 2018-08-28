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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            this.lbChartKdesc = new System.Windows.Forms.DataVisualization.Charting.Chart();
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnType10 = new System.Windows.Forms.Button();
            this.btnType11 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbChartKdesc)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea7.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea7);
            legend7.Name = "Legend1";
            this.chart1.Legends.Add(legend7);
            this.chart1.Location = new System.Drawing.Point(4, 102);
            this.chart1.Name = "chart1";
            series7.ChartArea = "ChartArea1";
            series7.Legend = "Legend1";
            series7.Name = "Series1";
            this.chart1.Series.Add(series7);
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
            // lbChartKdesc
            // 
            chartArea8.Name = "ChartArea1";
            this.lbChartKdesc.ChartAreas.Add(chartArea8);
            legend8.Name = "Legend1";
            this.lbChartKdesc.Legends.Add(legend8);
            this.lbChartKdesc.Location = new System.Drawing.Point(4, 520);
            this.lbChartKdesc.Name = "lbChartKdesc";
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series8.CustomProperties = "PointWidth=0.2";
            series8.Legend = "Legend1";
            series8.Name = "Series1";
            series8.YValuesPerPoint = 4;
            this.lbChartKdesc.Series.Add(series8);
            this.lbChartKdesc.Size = new System.Drawing.Size(1217, 356);
            this.lbChartKdesc.TabIndex = 10;
            this.lbChartKdesc.Text = "chart2";
            // 
            // btnType1
            // 
            this.btnType1.Location = new System.Drawing.Point(93, 491);
            this.btnType1.Name = "btnType1";
            this.btnType1.Size = new System.Drawing.Size(29, 23);
            this.btnType1.TabIndex = 11;
            this.btnType1.Text = "1";
            this.btnType1.UseVisualStyleBackColor = true;
            this.btnType1.Click += new System.EventHandler(this.btnType1_Click);
            // 
            // btnType2
            // 
            this.btnType2.Location = new System.Drawing.Point(128, 491);
            this.btnType2.Name = "btnType2";
            this.btnType2.Size = new System.Drawing.Size(29, 23);
            this.btnType2.TabIndex = 12;
            this.btnType2.Text = "2";
            this.btnType2.UseVisualStyleBackColor = true;
            this.btnType2.Click += new System.EventHandler(this.btnType2_Click);
            // 
            // btnType3
            // 
            this.btnType3.Location = new System.Drawing.Point(163, 491);
            this.btnType3.Name = "btnType3";
            this.btnType3.Size = new System.Drawing.Size(28, 23);
            this.btnType3.TabIndex = 13;
            this.btnType3.Text = "3";
            this.btnType3.UseVisualStyleBackColor = true;
            this.btnType3.Click += new System.EventHandler(this.btnType3_Click);
            // 
            // btnType4
            // 
            this.btnType4.Location = new System.Drawing.Point(197, 491);
            this.btnType4.Name = "btnType4";
            this.btnType4.Size = new System.Drawing.Size(29, 23);
            this.btnType4.TabIndex = 14;
            this.btnType4.Text = "4";
            this.btnType4.UseVisualStyleBackColor = true;
            this.btnType4.Click += new System.EventHandler(this.btnType4_Click);
            // 
            // btnType5
            // 
            this.btnType5.Location = new System.Drawing.Point(232, 491);
            this.btnType5.Name = "btnType5";
            this.btnType5.Size = new System.Drawing.Size(28, 23);
            this.btnType5.TabIndex = 15;
            this.btnType5.Text = "5";
            this.btnType5.UseVisualStyleBackColor = true;
            this.btnType5.Click += new System.EventHandler(this.btnType5_Click);
            // 
            // btnType6
            // 
            this.btnType6.Location = new System.Drawing.Point(266, 491);
            this.btnType6.Name = "btnType6";
            this.btnType6.Size = new System.Drawing.Size(28, 23);
            this.btnType6.TabIndex = 16;
            this.btnType6.Text = "6";
            this.btnType6.UseVisualStyleBackColor = true;
            this.btnType6.Click += new System.EventHandler(this.btnType6_Click);
            // 
            // btnType7
            // 
            this.btnType7.Location = new System.Drawing.Point(300, 491);
            this.btnType7.Name = "btnType7";
            this.btnType7.Size = new System.Drawing.Size(28, 23);
            this.btnType7.TabIndex = 17;
            this.btnType7.Text = "7";
            this.btnType7.UseVisualStyleBackColor = true;
            this.btnType7.Click += new System.EventHandler(this.btnType7_Click);
            // 
            // btnType8
            // 
            this.btnType8.Location = new System.Drawing.Point(334, 491);
            this.btnType8.Name = "btnType8";
            this.btnType8.Size = new System.Drawing.Size(28, 23);
            this.btnType8.TabIndex = 18;
            this.btnType8.Text = "8";
            this.btnType8.UseVisualStyleBackColor = true;
            this.btnType8.Click += new System.EventHandler(this.btnType8_Click);
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
            this.btnType9.Click += new System.EventHandler(this.btnType9_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(12, 493);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 16);
            this.label1.TabIndex = 21;
            this.label1.Text = "胆码";
            // 
            // btnType10
            // 
            this.btnType10.Location = new System.Drawing.Point(402, 491);
            this.btnType10.Name = "btnType10";
            this.btnType10.Size = new System.Drawing.Size(28, 23);
            this.btnType10.TabIndex = 22;
            this.btnType10.Text = "10";
            this.btnType10.UseVisualStyleBackColor = true;
            this.btnType10.Visible = false;
            this.btnType10.Click += new System.EventHandler(this.btnType10_Click);
            // 
            // btnType11
            // 
            this.btnType11.Location = new System.Drawing.Point(436, 491);
            this.btnType11.Name = "btnType11";
            this.btnType11.Size = new System.Drawing.Size(28, 23);
            this.btnType11.TabIndex = 23;
            this.btnType11.Text = "11";
            this.btnType11.UseVisualStyleBackColor = true;
            this.btnType11.Visible = false;
            this.btnType11.Click += new System.EventHandler(this.btnType11_Click);
            // 
            // frm_TrendAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.ClientSize = new System.Drawing.Size(1224, 888);
            this.Controls.Add(this.btnType11);
            this.Controls.Add(this.btnType10);
            this.Controls.Add(this.label1);
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
            this.Controls.Add(this.lbChartKdesc);
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
            ((System.ComponentModel.ISupportInitialize)(this.lbChartKdesc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.DataVisualization.Charting.Chart lbChartKdesc;
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnType10;
        private System.Windows.Forms.Button btnType11;
    }
}