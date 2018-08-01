namespace WinFormsApp1
{
    partial class frm_TrendAnalysisTrue
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Issue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.coldnumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wormnumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hotnumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isWin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opennumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnStartTrend = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lbPlayKind = new System.Windows.Forms.Label();
            this.lbIssue = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkGreen;
            this.panel1.Controls.Add(this.comboBox2);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.lbIssue);
            this.panel1.Controls.Add(this.lbPlayKind);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnStartTrend);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Location = new System.Drawing.Point(1, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(814, 396);
            this.panel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Issue,
            this.coldnumber,
            this.wormnumber,
            this.hotnumber,
            this.isWin,
            this.opennumber});
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(642, 360);
            this.dataGridView1.TabIndex = 0;
            // 
            // Issue
            // 
            this.Issue.HeaderText = "期号";
            this.Issue.Name = "Issue";
            this.Issue.ReadOnly = true;
            // 
            // coldnumber
            // 
            this.coldnumber.HeaderText = "冷码";
            this.coldnumber.Name = "coldnumber";
            this.coldnumber.ReadOnly = true;
            // 
            // wormnumber
            // 
            this.wormnumber.HeaderText = "温码";
            this.wormnumber.Name = "wormnumber";
            this.wormnumber.ReadOnly = true;
            // 
            // hotnumber
            // 
            this.hotnumber.HeaderText = "热码";
            this.hotnumber.Name = "hotnumber";
            this.hotnumber.ReadOnly = true;
            // 
            // isWin
            // 
            this.isWin.HeaderText = "中奖";
            this.isWin.Name = "isWin";
            this.isWin.ReadOnly = true;
            // 
            // opennumber
            // 
            this.opennumber.HeaderText = "开奖号";
            this.opennumber.Name = "opennumber";
            this.opennumber.ReadOnly = true;
            // 
            // btnStartTrend
            // 
            this.btnStartTrend.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnStartTrend.Location = new System.Drawing.Point(674, 140);
            this.btnStartTrend.Name = "btnStartTrend";
            this.btnStartTrend.Size = new System.Drawing.Size(117, 67);
            this.btnStartTrend.TabIndex = 1;
            this.btnStartTrend.Text = "趋势分析";
            this.btnStartTrend.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnRefresh.Location = new System.Drawing.Point(674, 213);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(117, 67);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "数据刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // lbPlayKind
            // 
            this.lbPlayKind.AutoSize = true;
            this.lbPlayKind.Font = new System.Drawing.Font("新細明體", 10F);
            this.lbPlayKind.Location = new System.Drawing.Point(649, 326);
            this.lbPlayKind.Name = "lbPlayKind";
            this.lbPlayKind.Size = new System.Drawing.Size(35, 14);
            this.lbPlayKind.TabIndex = 3;
            this.lbPlayKind.Text = "彩种";
            // 
            // lbIssue
            // 
            this.lbIssue.AutoSize = true;
            this.lbIssue.Font = new System.Drawing.Font("新細明體", 10F);
            this.lbIssue.Location = new System.Drawing.Point(649, 349);
            this.lbIssue.Name = "lbIssue";
            this.lbIssue.Size = new System.Drawing.Size(35, 14);
            this.lbIssue.TabIndex = 4;
            this.lbIssue.Text = "期号";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(690, 321);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.Text = "请选择彩种";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(690, 347);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 20);
            this.comboBox2.TabIndex = 6;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(1, 404);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.YValuesPerPoint = 4;
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(811, 199);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // frm_TrendAnalysisTrue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 604);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.panel1);
            this.Name = "frm_TrendAnalysisTrue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "趨勢分析";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Issue;
        private System.Windows.Forms.DataGridViewTextBoxColumn coldnumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn wormnumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn hotnumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn isWin;
        private System.Windows.Forms.DataGridViewTextBoxColumn opennumber;
        private System.Windows.Forms.Button btnStartTrend;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lbIssue;
        private System.Windows.Forms.Label lbPlayKind;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}