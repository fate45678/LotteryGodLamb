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
    public partial class frm_editNumber : Form
    {
        public frm_editNumber(string Number)
        {
            InitializeComponent();
            txtEdit.Text = Number;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            frm_PlanUpload.richBox1Number = txtEdit.Text;
            MessageBox.Show("修改完成");
            this.Close();
        }
    }
}
