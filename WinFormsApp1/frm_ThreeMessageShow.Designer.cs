namespace WinFormsApp1
{
    partial class frm_ThreeMessageShow
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
            this.btnFrontThree = new System.Windows.Forms.Button();
            this.btnMidThree = new System.Windows.Forms.Button();
            this.btnBackThree = new System.Windows.Forms.Button();
            this.lbDesc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnFrontThree
            // 
            this.btnFrontThree.Location = new System.Drawing.Point(12, 67);
            this.btnFrontThree.Name = "btnFrontThree";
            this.btnFrontThree.Size = new System.Drawing.Size(88, 28);
            this.btnFrontThree.TabIndex = 0;
            this.btnFrontThree.Text = "前三";
            this.btnFrontThree.UseVisualStyleBackColor = true;
            this.btnFrontThree.Click += new System.EventHandler(this.btnFrontThree_Click);
            // 
            // btnMidThree
            // 
            this.btnMidThree.Location = new System.Drawing.Point(109, 67);
            this.btnMidThree.Name = "btnMidThree";
            this.btnMidThree.Size = new System.Drawing.Size(88, 28);
            this.btnMidThree.TabIndex = 1;
            this.btnMidThree.Text = "中三";
            this.btnMidThree.UseVisualStyleBackColor = true;
            this.btnMidThree.Click += new System.EventHandler(this.btnMidThree_Click);
            // 
            // btnBackThree
            // 
            this.btnBackThree.Location = new System.Drawing.Point(207, 67);
            this.btnBackThree.Name = "btnBackThree";
            this.btnBackThree.Size = new System.Drawing.Size(88, 28);
            this.btnBackThree.TabIndex = 2;
            this.btnBackThree.Text = "后三";
            this.btnBackThree.UseVisualStyleBackColor = true;
            this.btnBackThree.Click += new System.EventHandler(this.btnBackThree_Click);
            // 
            // lbDesc
            // 
            this.lbDesc.AutoSize = true;
            this.lbDesc.Font = new System.Drawing.Font("新細明體", 12F);
            this.lbDesc.Location = new System.Drawing.Point(12, 34);
            this.lbDesc.Name = "lbDesc";
            this.lbDesc.Size = new System.Drawing.Size(280, 16);
            this.lbDesc.TabIndex = 3;
            this.lbDesc.Text = "请选择【前三】、【中三】或【后三】";
            // 
            // frm_ThreeMessageShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(221)))), ((int)(((byte)(202)))));
            this.ClientSize = new System.Drawing.Size(307, 107);
            this.Controls.Add(this.lbDesc);
            this.Controls.Add(this.btnBackThree);
            this.Controls.Add(this.btnMidThree);
            this.Controls.Add(this.btnFrontThree);
            this.Name = "frm_ThreeMessageShow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选项";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFrontThree;
        private System.Windows.Forms.Button btnMidThree;
        private System.Windows.Forms.Button btnBackThree;
        private System.Windows.Forms.Label lbDesc;
    }
}