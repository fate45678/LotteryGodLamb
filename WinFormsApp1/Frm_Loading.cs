using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Frm_Loading : Form
    {
        public Frm_Loading()
        {
            InitializeComponent();
        }

        private void Frm_Loading_Load(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            path  = Directory.GetParent(Directory.GetParent(path).ToString()).ToString();
           // this.BackgroundImage = Image.FromFile(path+"\\bt_icon\\loading_icon.gif");
        }
        public void closeWin()
        {
            frm_PlanUpload frmUploadPreload = new frm_PlanUpload();
            frmUploadPreload.checkHistoryOnlyonce();
            Thread.Sleep(5000);
            this.Close();
        }
    }
}
