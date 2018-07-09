namespace WinFormsApp1
{
    partial class frm_startLoading
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
            this.pgbShow = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.bkgStart = new System.ComponentModel.BackgroundWorker();
            this.tlpAd = new System.Windows.Forms.TableLayoutPanel();
            this.ptbAd = new System.Windows.Forms.PictureBox();
            this.lbDoingDesc = new System.Windows.Forms.Label();
            this.tlpAd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbAd)).BeginInit();
            this.SuspendLayout();
            // 
            // pgbShow
            // 
            this.pgbShow.Location = new System.Drawing.Point(5, 184);
            this.pgbShow.Name = "pgbShow";
            this.pgbShow.Size = new System.Drawing.Size(519, 23);
            this.pgbShow.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("標楷體", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(74, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(397, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "程序加载中...请稍后";
            // 
            // bkgStart
            // 
            this.bkgStart.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bkgStart_DoWork);
            this.bkgStart.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bkgStart_ProgressChanged);
            this.bkgStart.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bkgStart_RunWorkerCompleted);
            // 
            // tlpAd
            // 
            this.tlpAd.ColumnCount = 1;
            this.tlpAd.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAd.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpAd.Controls.Add(this.ptbAd, 0, 0);
            this.tlpAd.Location = new System.Drawing.Point(5, 3);
            this.tlpAd.Name = "tlpAd";
            this.tlpAd.RowCount = 1;
            this.tlpAd.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAd.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.tlpAd.Size = new System.Drawing.Size(519, 135);
            this.tlpAd.TabIndex = 2;
            // 
            // ptbAd
            // 
            this.ptbAd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ptbAd.Location = new System.Drawing.Point(3, 3);
            this.ptbAd.Name = "ptbAd";
            this.ptbAd.Size = new System.Drawing.Size(513, 129);
            this.ptbAd.TabIndex = 0;
            this.ptbAd.TabStop = false;
            // 
            // lbDoingDesc
            // 
            this.lbDoingDesc.AutoSize = true;
            this.lbDoingDesc.Location = new System.Drawing.Point(223, 212);
            this.lbDoingDesc.Name = "lbDoingDesc";
            this.lbDoingDesc.Size = new System.Drawing.Size(77, 12);
            this.lbDoingDesc.TabIndex = 3;
            this.lbDoingDesc.Text = "准备载入资讯";
            // 
            // frm_startLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(527, 233);
            this.Controls.Add(this.lbDoingDesc);
            this.Controls.Add(this.tlpAd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pgbShow);
            this.Name = "frm_startLoading";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "載入窗口";
            this.tlpAd.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptbAd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pgbShow;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker bkgStart;
        private System.Windows.Forms.TableLayoutPanel tlpAd;
        private System.Windows.Forms.PictureBox ptbAd;
        private System.Windows.Forms.Label lbDoingDesc;
    }
}