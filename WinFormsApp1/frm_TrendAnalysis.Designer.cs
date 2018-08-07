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
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
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
            // chart2
            // 
            chartArea2.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart2.Legends.Add(legend2);
            this.chart2.Location = new System.Drawing.Point(4, 455);
            this.chart2.Name = "chart2";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart2.Series.Add(series2);
            this.chart2.Size = new System.Drawing.Size(1217, 356);
            this.chart2.TabIndex = 10;
            this.chart2.Text = "chart2";
            // 
            // frm_TrendAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.ClientSize = new System.Drawing.Size(1224, 823);
            this.Controls.Add(this.chart2);
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
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
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
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
    }
}