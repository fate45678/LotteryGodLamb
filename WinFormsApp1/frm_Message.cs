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
    public partial class frm_Message : Form
    {
        public frm_Message()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            //frmGameMain fGameMain = new frmGameMain();
            frmGameMain.openMessage = false;
            this.Dispose();
            this.Close();
        }

        private void frm_Message_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmGameMain.clsIcon = 0;
        }
    }
}
