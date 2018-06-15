using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class frm_TwoMessageShow : Form
    {
        public frm_TwoMessageShow()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        private void btnFrontTwo_Click(object sender, EventArgs e)
        {
            frm_PlanUpload frm_PlanUpload = (frm_PlanUpload)this.Owner; //把Form2的父窗口指針賦給lForm1  
            frm_PlanUpload.gameKindValue = "FrontTwo";//使用父窗口指針賦值  
            this.Close();
        }

        private void btnBackTwo_Click(object sender, EventArgs e)
        {
            frm_PlanUpload frm_PlanUpload = (frm_PlanUpload)this.Owner; //把Form2的父窗口指針賦給lForm1  
            frm_PlanUpload.gameKindValue = "BackTwo";//使用父窗口指針賦值  
            this.Close();
        }
    }
}
