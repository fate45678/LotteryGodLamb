namespace WinFormsApp1
{
    partial class frm_TwoMessageShow
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
            this.btnFrontTwo = new System.Windows.Forms.Button();
            this.btnBackTwo = new System.Windows.Forms.Button();
            this.lbDesc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnFrontTwo
            // 
            this.btnFrontTwo.Location = new System.Drawing.Point(30, 67);
            this.btnFrontTwo.Name = "btnFrontTwo";
            this.btnFrontTwo.Size = new System.Drawing.Size(88, 28);
            this.btnFrontTwo.TabIndex = 0;
            this.btnFrontTwo.Text = "前二";
            this.btnFrontTwo.UseVisualStyleBackColor = true;
            this.btnFrontTwo.Click += new System.EventHandler(this.btnFrontTwo_Click);
            // 
            // btnBackTwo
            // 
            this.btnBackTwo.Location = new System.Drawing.Point(178, 67);
            this.btnBackTwo.Name = "btnBackTwo";
            this.btnBackTwo.Size = new System.Drawing.Size(88, 28);
            this.btnBackTwo.TabIndex = 1;
            this.btnBackTwo.Text = "后二";
            this.btnBackTwo.UseVisualStyleBackColor = true;
            this.btnBackTwo.Click += new System.EventHandler(this.btnBackTwo_Click);
            // 
            // lbDesc
            // 
            this.lbDesc.AutoSize = true;
            this.lbDesc.Font = new System.Drawing.Font("新細明體", 15F);
            this.lbDesc.Location = new System.Drawing.Point(26, 30);
            this.lbDesc.Name = "lbDesc";
            this.lbDesc.Size = new System.Drawing.Size(249, 20);
            this.lbDesc.TabIndex = 2;
            this.lbDesc.Text = "请选择【前二】或【后二】";
            // 
            // frm_TwoMessageShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(307, 107);
            this.Controls.Add(this.lbDesc);
            this.Controls.Add(this.btnBackTwo);
            this.Controls.Add(this.btnFrontTwo);
            this.Name = "frm_TwoMessageShow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选项";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFrontTwo;
        private System.Windows.Forms.Button btnBackTwo;
        private System.Windows.Forms.Label lbDesc;
    }
}