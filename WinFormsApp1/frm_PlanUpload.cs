using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinFormsApp1
{
    public partial class frm_PlanUpload : Form
    {
        public frm_PlanUpload()
        {
            InitializeComponent();
        }
        //取得歷史開獎
        private void UpdateHistory()
        {
            if (rtxtHistory.Text == "") //無資料就全寫入
            {
                for (int i = 0; i < frmGameMain.jArr.Count; i++)
                {
                    if (i == 120) break; //寫120筆就好
                    rtxtHistory.Text += frmGameMain.jArr[i]["Issue"].ToString() + "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "\r\n";
                }
            }
            else //有資料先判斷
            {
                if ((rtxtHistory.Text.Substring(0, 11) != frmGameMain.jArr[0]["Issue"].ToString()) && (frmGameMain.strHistoryNumberOpen != "?")) //有新資料了
                {
                    rtxtHistory.Text = "";
                    for (int i = 0; i < frmGameMain.jArr.Count; i++)
                    {
                        if (i == 120) break; //寫120筆就好
                        rtxtHistory.Text += frmGameMain.jArr[i]["Issue"].ToString() + "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "\r\n";
                    }
                }
            }
        }
     
        private void picAD4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.cwl.gov.cn/");
        }   
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateHistory();
            refreshInterface();
        }

        private void btnViewResult_Click(object sender, EventArgs e)
        {
            frm_Register register = new frm_Register();
            register.Owner = this;
            register.Show();
            return;
        }
        
        public void refreshInterface()
        {
            label4.Text = frmGameMain.globalUserName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmGameMain.globalUserName = "";
        }
    }
}
