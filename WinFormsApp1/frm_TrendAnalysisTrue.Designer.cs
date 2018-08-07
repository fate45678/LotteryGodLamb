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
            this.btn30Issue = new System.Windows.Forms.Button();
            this.btn10Issue = new System.Windows.Forms.Button();
            this.btn50Issue = new System.Windows.Forms.Button();
            this.cbplayNumber = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lbIssue = new System.Windows.Forms.Label();
            this.lbPlayKind = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnStartTrend = new System.Windows.Forms.Button();
            this.dgShowTrend = new System.Windows.Forms.DataGridView();
            this.Issue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.coldnumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wormnumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hotnumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.playNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgShowTrend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkGreen;
            this.panel1.Controls.Add(this.btn30Issue);
            this.panel1.Controls.Add(this.btn10Issue);
            this.panel1.Controls.Add(this.btn50Issue);
            this.panel1.Controls.Add(this.cbplayNumber);
            this.panel1.Controls.Add(this.comboBox2);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.lbIssue);
            this.panel1.Controls.Add(this.lbPlayKind);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnStartTrend);
            this.panel1.Controls.Add(this.dgShowTrend);
            this.panel1.Location = new System.Drawing.Point(1, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(814, 396);
            this.panel1.TabIndex = 0;
            // 
            // btn30Issue
            // 
            this.btn30Issue.Location = new System.Drawing.Point(219, 368);
            this.btn30Issue.Name = "btn30Issue";
            this.btn30Issue.Size = new System.Drawing.Size(75, 23);
            this.btn30Issue.TabIndex = 10;
            this.btn30Issue.Text = "最近30期";
            this.btn30Issue.UseVisualStyleBackColor = true;
            // 
            // btn10Issue
            // 
            this.btn10Issue.BackColor = System.Drawing.Color.LightGray;
            this.btn10Issue.ForeColor = System.Drawing.Color.Black;
            this.btn10Issue.Location = new System.Drawing.Point(300, 368);
            this.btn10Issue.Name = "btn10Issue";
            this.btn10Issue.Size = new System.Drawing.Size(75, 23);
            this.btn10Issue.TabIndex = 9;
            this.btn10Issue.Text = "最近10期";
            this.btn10Issue.UseVisualStyleBackColor = false;
            // 
            // btn50Issue
            // 
            this.btn50Issue.Location = new System.Drawing.Point(138, 368);
            this.btn50Issue.Name = "btn50Issue";
            this.btn50Issue.Size = new System.Drawing.Size(75, 23);
            this.btn50Issue.TabIndex = 8;
            this.btn50Issue.Text = "最近50期";
            this.btn50Issue.UseVisualStyleBackColor = true;
            // 
            // cbplayNumber
            // 
            this.cbplayNumber.FormattingEnabled = true;
            this.cbplayNumber.Items.AddRange(new object[] {
            "前二",
            "后二",
            "前三",
            "中三",
            "后三"});
            this.cbplayNumber.Location = new System.Drawing.Point(11, 369);
            this.cbplayNumber.Name = "cbplayNumber";
            this.cbplayNumber.Size = new System.Drawing.Size(121, 20);
            this.cbplayNumber.TabIndex = 7;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(690, 347);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 20);
            this.comboBox2.TabIndex = 6;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "重庆时时彩"});
            this.comboBox1.Location = new System.Drawing.Point(690, 321);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.Text = "请选择彩种";
            // 
            // lbIssue
            // 
            this.lbIssue.AutoSize = true;
            this.lbIssue.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbIssue.Location = new System.Drawing.Point(649, 349);
            this.lbIssue.Name = "lbIssue";
            this.lbIssue.Size = new System.Drawing.Size(35, 13);
            this.lbIssue.TabIndex = 4;
            this.lbIssue.Text = "期号";
            // 
            // lbPlayKind
            // 
            this.lbPlayKind.AutoSize = true;
            this.lbPlayKind.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbPlayKind.Location = new System.Drawing.Point(649, 326);
            this.lbPlayKind.Name = "lbPlayKind";
            this.lbPlayKind.Size = new System.Drawing.Size(35, 13);
            this.lbPlayKind.TabIndex = 3;
            this.lbPlayKind.Text = "彩种";
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
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
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
            // dgShowTrend
            // 
            this.dgShowTrend.AllowUserToDeleteRows = false;
            this.dgShowTrend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgShowTrend.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Issue,
            this.coldnumber,
            this.wormnumber,
            this.hotnumber,
            this.playNumber,
            this.number});
            this.dgShowTrend.Location = new System.Drawing.Point(3, 3);
            this.dgShowTrend.Name = "dgShowTrend";
            this.dgShowTrend.ReadOnly = true;
            this.dgShowTrend.RowTemplate.Height = 24;
            this.dgShowTrend.Size = new System.Drawing.Size(642, 360);
            this.dgShowTrend.TabIndex = 0;
            // 
            // Issue
            // 
            this.Issue.DataPropertyName = "Issue";
            this.Issue.HeaderText = "期号";
            this.Issue.Name = "Issue";
            this.Issue.ReadOnly = true;
            // 
            // coldnumber
            // 
            this.coldnumber.DataPropertyName = "coldnumber";
            this.coldnumber.HeaderText = "冷码";
            this.coldnumber.Name = "coldnumber";
            this.coldnumber.ReadOnly = true;
            // 
            // wormnumber
            // 
            this.wormnumber.DataPropertyName = "wormnumber";
            this.wormnumber.HeaderText = "温码";
            this.wormnumber.Name = "wormnumber";
            this.wormnumber.ReadOnly = true;
            // 
            // hotnumber
            // 
            this.hotnumber.DataPropertyName = "hotnumber";
            this.hotnumber.HeaderText = "热码";
            this.hotnumber.Name = "hotnumber";
            this.hotnumber.ReadOnly = true;
            // 
            // playNumber
            // 
            this.playNumber.DataPropertyName = "playNumber";
            this.playNumber.HeaderText = "中奖";
            this.playNumber.Name = "playNumber";
            this.playNumber.ReadOnly = true;
            // 
            // number
            // 
            this.number.DataPropertyName = "number";
            this.number.HeaderText = "开奖号";
            this.number.Name = "number";
            this.number.ReadOnly = true;
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
            ((System.ComponentModel.ISupportInitialize)(this.dgShowTrend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgShowTrend;
        private System.Windows.Forms.Button btnStartTrend;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lbIssue;
        private System.Windows.Forms.Label lbPlayKind;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ComboBox cbplayNumber;
        private System.Windows.Forms.Button btn30Issue;
        private System.Windows.Forms.Button btn10Issue;
        private System.Windows.Forms.Button btn50Issue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Issue;
        private System.Windows.Forms.DataGridViewTextBoxColumn coldnumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn wormnumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn hotnumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn playNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn number;
    }
}