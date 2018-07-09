namespace WinFormsApp1
{
    partial class frm_VersionCheck
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
            this.pgbshow = new System.Windows.Forms.ProgressBar();
            this.bkgVersionCheck = new System.ComponentModel.BackgroundWorker();
            this.lbVersionCheckDesc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pgbshow
            // 
            this.pgbshow.Location = new System.Drawing.Point(2, 125);
            this.pgbshow.Name = "pgbshow";
            this.pgbshow.Size = new System.Drawing.Size(282, 23);
            this.pgbshow.TabIndex = 0;
            // 
            // bkgVersionCheck
            // 
            this.bkgVersionCheck.WorkerReportsProgress = true;
            this.bkgVersionCheck.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bkgVersionCheck_DoWork);
            this.bkgVersionCheck.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bkgVersionCheck_ProgressChanged);
            this.bkgVersionCheck.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bkgVersionCheck_RunWorkerCompleted);
            // 
            // lbVersionCheckDesc
            // 
            this.lbVersionCheckDesc.AutoSize = true;
            this.lbVersionCheckDesc.Location = new System.Drawing.Point(89, 96);
            this.lbVersionCheckDesc.Name = "lbVersionCheckDesc";
            this.lbVersionCheckDesc.Size = new System.Drawing.Size(101, 12);
            this.lbVersionCheckDesc.TabIndex = 1;
            this.lbVersionCheckDesc.Text = "检查版本中请稍候";
            // 
            // frm_VersionCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 155);
            this.Controls.Add(this.lbVersionCheckDesc);
            this.Controls.Add(this.pgbshow);
            this.Name = "frm_VersionCheck";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "检查版本中...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pgbshow;
        private System.ComponentModel.BackgroundWorker bkgVersionCheck;
        private System.Windows.Forms.Label lbVersionCheckDesc;
    }
}