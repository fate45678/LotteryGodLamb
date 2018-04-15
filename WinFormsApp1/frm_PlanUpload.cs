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

        bool isFirstTime = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateHistory();
            refreshInterface();
            if(isFirstTime)
                InitcbItem();//初始化combobox
            filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length-3, 3)));
            label2.Text ="共"+calPeriod()+"期";
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
        int temp = 0;
        private void filtercbItem(int current)
        {
            if (current > temp)
            {
                var dt_plan = Items.Where(x => x.Key > current-1);
                var dt_cycle = Items.Where(x => x.Key > current);
                cbGamePlan.DataSource = new BindingSource(dt_plan, null);
                cbGameCycle.DataSource = new BindingSource(dt_cycle, null);
                temp = current;
            }
            else if (current == 120)
            {
                var dt_plan = Items.Where(x => x.Key < 999 );
                var dt_cycle = Items.Where(x => x.Key >  1);
                cbGamePlan.DataSource = new BindingSource(dt_plan, null);
                cbGameCycle.DataSource = new BindingSource(dt_cycle, null);
                temp = current;
            }
        }

        Dictionary<int, string> Items = new Dictionary<int, string>();
        private void InitcbItem()
        {
            cbGameCycle.Items.Clear();//不知道怎麼把預設的選項清掉
            for (int i = 1; i < 121; i++)
            {
                if (i < 10)
                    Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "00" + i.ToString());
                else if (i > 9 && i < 100)
                    Items.Add(i,frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "0" + i.ToString());
                else
                    Items.Add(i,frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + i.ToString());

                cbGamePlan.DisplayMember = "Value";
                cbGamePlan.ValueMember = "Key";
                cbGameCycle.DisplayMember = "Value";
                cbGameCycle.ValueMember = "Key";
                cbGamePlan.DataSource = new BindingSource(Items, null);
                cbGameCycle.DataSource = new BindingSource(Items, null);
            }
            isFirstTime = false;
        }
        private int calPeriod()
        {
            return ((int)cbGameCycle.SelectedValue - (int)cbGamePlan.SelectedValue);
        }
        private void cbGamePlan_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var dt_cycle = Items.Where(x => x.Key > (int)cbGamePlan.SelectedValue);
            cbGameCycle.DataSource = new BindingSource(dt_cycle, null);
        }
    }
}
