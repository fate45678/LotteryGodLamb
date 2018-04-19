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
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinFormsApp1
{
    public partial class frm_PlanUpload : Form
    {
        Connection con = new Connection();
        public frm_PlanUpload()
        {
            InitializeComponent();
            //cbGameKind.SelectedIndex = 0;
            //cbGamePlan.SelectedIndex = 0;
        }
        //取得歷史開獎
        private void UpdateHistory()
        {
            if (rtxtHistory.Text == "") //無資料就全寫入
            {
                for (int i = 0; i < frmGameMain.jArr.Count; i++)
                {
                    if (i == 120) break; //寫120筆就好
                    {
                        rtxtHistory.Text += frmGameMain.jArr[i]["Issue"].ToString() + "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "\r\n";
                        //addNCheckHistoryNumber(i);
                    }
                    
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
                        addNCheckHistoryNumber(i);
                    }
                }
            }
            
               
        }
     
        private void picAD4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.cwl.gov.cn/");
        }

        bool isFirstTime = true;
        int updateCount=0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateHistory();
            refreshInterface();
            if (isFirstTime)
            {
                if (cbGameKind.SelectedIndex == -1)
                    cbGameKind.SelectedIndex = 0;
                if (cbGameDirect.SelectedIndex == -1)
                    cbGameDirect.SelectedIndex = 0;
                InitcbItem();//初始化combobox

            }
            filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 3, 3)));
            label2.Text = "共" + calPeriod() + "期";
            label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + calPeriod() + "期";

            if (updateCount % 3 == 0 && allorwUpdate)
                updatelbSent();
            updateCount++;


        }
        private void updatelbSent()
        {
            lsbSent.DataSource = con.ConSQLtoList4cb("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select p_name as 'name' from Upplan");
            lsbSent.DisplayMember = "value";
            lsbSent.ValueMember = "id";
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
                comboBox1.DataSource = new BindingSource(dt_plan, null);
                cbGameCycle.DataSource = new BindingSource(dt_cycle, null);
                comboBox2.DataSource = new BindingSource(dt_cycle, null);
                temp = current;
            }
            else if (current == 120)
            {
                var dt_plan = Items.Where(x => x.Key < 999 );
                var dt_cycle = Items.Where(x => x.Key >  1);
                cbGamePlan.DataSource = new BindingSource(dt_plan, null);
                comboBox1.DataSource = new BindingSource(dt_plan, null);
                cbGameCycle.DataSource = new BindingSource(dt_cycle, null);
                comboBox2.DataSource = new BindingSource(dt_cycle, null);
                temp = current;
            }
        }

        Dictionary<int, string> Items = new Dictionary<int, string>();
        private void InitcbItem()
        {
            if (!string.IsNullOrEmpty(frmGameMain.globalGetCurrentPeriod))
            {
                cbGameCycle.Items.Clear();//不知道怎麼把預設的選項清掉
                for (int i = 1; i < 121; i++)
                {
                    if (i < 10)
                        Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "00" + i.ToString());
                    else if (i > 9 && i < 100)
                        Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "0" + i.ToString());
                    else
                        Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + i.ToString());

                    cbGamePlan.DisplayMember = "Value";
                    cbGamePlan.ValueMember = "Key";
                    cbGameCycle.DisplayMember = "Value";
                    cbGameCycle.ValueMember = "Key";
                    comboBox1.DisplayMember = "Value";
                    comboBox1.ValueMember = "Key";
                    comboBox2.DisplayMember = "Value";
                    comboBox2.ValueMember = "Key";
                    cbGamePlan.DataSource = new BindingSource(Items, null);
                    cbGameCycle.DataSource = new BindingSource(Items, null);
                    comboBox1.DataSource = new BindingSource(Items, null);
                    comboBox2.DataSource = new BindingSource(Items, null);
                }
                isFirstTime = false;
            }
        }
        private int calPeriod()
        {
            if ((int)cbGamePlan.SelectedValue != 120)
                return ((int)cbGameCycle.SelectedValue - (int)cbGamePlan.SelectedValue);
            else return 0;
        }
        private void cbGamePlan_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var dt_cycle = Items.Where(x => x.Key > (int)cbGamePlan.SelectedValue);
            cbGameCycle.DataSource = new BindingSource(dt_cycle, null);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("我也不知道這個要做什麼。");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            string withoutComma = richTextBox2.Text.Replace(",","");
            if (richTextBox2.TextLength > 4 && withoutComma.Length %5==0 && !richTextBox2.Text.Substring(richTextBox2.SelectionStart-1, 1).Equals(","))
            {
                richTextBox2.Text += ",";
                richTextBox2.Select(richTextBox2.MaxLength, 0);
            }
            MatchCollection mc;
            Regex r = new Regex(",");
            mc = r.Matches(richTextBox2.Text);
            label21.Text = "共" + mc.Count + "注";
            }

        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9') return;
            if (e.KeyChar == '+' || e.KeyChar == '-') return;
            if (e.KeyChar == 8) return;
            e.Handled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(label4.Text))
                MessageBox.Show("尚未登入。");
            else
            {
                
                string planName = label24.Text.Replace("重庆时时彩  ", "");
                
                var plancount = con.ConSQLtoList4cb("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select convert(nvarchar(3),count(*)) as 'name' from Upplan where p_account = '" + frmGameMain.globalUserAccount+"'");
                string maxNum = plancount.Where(x => !x.value.Equals("")).FirstOrDefault().value.ToString();
                int planNUmber = int.Parse(maxNum) + 1;
                con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "Insert into Upplan(p_name, p_account, p_start, p_end, p_rule) values('" + label4.Text+ planName + planNUmber+"','" + frmGameMain.globalUserAccount + "','"+ cbGamePlan.Text + "','"+ cbGameCycle.Text + "','"+ richTextBox2.Text + "')");

                MessageBox.Show("上傳成功。");
                updatelbSent();
            }
            allorwUpdate = true;


        }

        private void cbGameKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateLabel24();
        }

        private void cbGameDirect_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateLabel24();
        }

        private void updateLabel24()
        {
            label24.Text = "重庆时时彩  " + (string)cbGameKind.SelectedItem + (string)cbGameDirect.SelectedItem;
        }

        /// <summary>
        /// 檢查資料庫是否有歷史號碼,沒有的話新增
        /// </summary>
        private void addNCheckHistoryNumber(int i)
        {
                string st = frmGameMain.jArr[i]["Issue"].ToString();
                var dt = con.ConSQLtoList4cb("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select issue as 'name' from HistoryNumber where issue = '"+ st + "'");
                if (dt.Count == 0)//沒找到期數
                {
                    con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "Insert into HistoryNumber(issue, number) values('"+ st + "','"+ frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "')");
                }
        }

        private void lsbSent_SelectedIndexChanged(object sender, EventArgs e)
        {

            richTextBox1.Text = "";
        }
        bool allorwUpdate = true;
        private void lsbSent_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Connection.Item dt = lsbSent.SelectedItem as Connection.Item;
            string st = dt.value;
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "p_name");
            dic.Add(1, "p_account");
            dic.Add(2, "p_start");
            dic.Add(3, "p_end");
            dic.Add(4, "p_rule");
            allorwUpdate = false;
            var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_name = '"+ st+"'", dic);
            for (int i = 0; i < getData.Count; i++)
            {

                if (i == 0)
                    label16.Text = getData.ElementAt(i);
                else if (i == 2)
                    label17.Text = "已上传: 第" + getData.ElementAt(i) + "~" + getData.ElementAt(i) + "期";
                else if (i == 4)
                {
                    richTextBox1.Text = getData.ElementAt(i).Substring(0, getData.ElementAt(i).Length - 1);
                    string strReplace = getData.ElementAt(i).Replace(",", "");
                    int times = (getData.ElementAt(i).Length - strReplace.Length) / 1;
                    label15.Text = "共"+times+"注";

                }


            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            lsbSent.SelectionMode = SelectionMode.MultiSimple;
            allorwUpdate = false;
            if (checkBox1.Checked)
            {
                for (int i = 0; i < lsbSent.Items.Count; i++)
                {
                    lsbSent.SetSelected(i, true); 
                }
            }
            else
            {
                for (int i = 0; i < lsbSent.Items.Count; i++)
                {

                    lsbSent.SetSelected(i, false);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<string> lt = new List<string>();
            int[] id = new int[lsbSent.SelectedIndices.Count];
            for (int i = 0; i < lsbSent.SelectedIndices.Count; i++)
            {
                id[i] = (int)lsbSent.SelectedIndices[i];
            }
            for (int i = 0; i < id.Length; i++)
            {
                if (lsbSent.GetSelected(i))
                {
                    Connection.Item dt = lsbSent.Items[i] as Connection.Item;
                    con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "delete from Upplan where p_name = '"+dt.value+"'");
                }
            }
            MessageBox.Show("刪除成功。");
            updatelbSent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label16.Text = "";
            label17.Text = "已上传:";
            label15.Text = "共0注";
            richTextBox1.Text = "";
        }
    }
}
