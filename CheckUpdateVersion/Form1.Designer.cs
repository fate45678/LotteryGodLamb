namespace CheckUpdateVersion
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pgbshow = new System.Windows.Forms.ProgressBar();
            this.lbCheckVersionDesc = new System.Windows.Forms.Label();
            this.bkgCheckVersion = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // pgbshow
            // 
            this.pgbshow.Location = new System.Drawing.Point(2, 129);
            this.pgbshow.Name = "pgbshow";
            this.pgbshow.Size = new System.Drawing.Size(280, 23);
            this.pgbshow.TabIndex = 0;
            // 
            // lbCheckVersionDesc
            // 
            this.lbCheckVersionDesc.AutoSize = true;
            this.lbCheckVersionDesc.Location = new System.Drawing.Point(81, 101);
            this.lbCheckVersionDesc.Name = "lbCheckVersionDesc";
            this.lbCheckVersionDesc.Size = new System.Drawing.Size(110, 12);
            this.lbCheckVersionDesc.TabIndex = 1;
            this.lbCheckVersionDesc.Text = "检查版本中请稍候...";
            // 
            // bkgCheckVersion
            // 
            this.bkgCheckVersion.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bkgCheckVersion_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 158);
            this.Controls.Add(this.lbCheckVersionDesc);
            this.Controls.Add(this.pgbshow);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "检查版本中...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pgbshow;
        private System.Windows.Forms.Label lbCheckVersionDesc;
        private System.ComponentModel.BackgroundWorker bkgCheckVersion;
    }
}

