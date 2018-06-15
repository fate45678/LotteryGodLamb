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
    public partial class frm_ThreeMessageShow : Form
    {
        public frm_ThreeMessageShow()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        private void btnFrontThree_Click(object sender, EventArgs e)
        {
            frm_PlanUpload frm_PlanUpload = (frm_PlanUpload)this.Owner; //把Form2的父窗口指針賦給lForm1  
            frm_PlanUpload.gameKindValue = "FrontThree";//使用父窗口指針賦值  
            this.Close();
        }

        private void btnMidThree_Click(object sender, EventArgs e)
        {
            frm_PlanUpload frm_PlanUpload = (frm_PlanUpload)this.Owner; //把Form2的父窗口指針賦給lForm1  
            frm_PlanUpload.gameKindValue = "MidThree";//使用父窗口指針賦值  
            this.Close();
        }

        private void btnBackThree_Click(object sender, EventArgs e)
        {
            frm_PlanUpload frm_PlanUpload = (frm_PlanUpload)this.Owner; //把Form2的父窗口指針賦給lForm1  
            frm_PlanUpload.gameKindValue = "BackThree";//使用父窗口指針賦值  
            this.Close();
        }
    }
}
