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
using System.Web;
using System.Windows;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinFormsApp1
{
    public partial class frm_PlanAgent : Form
    {
        Connection con = new Connection();
        public frm_PlanAgent()
        {
            InitializeComponent();

            txtSearchUser.ForeColor = Color.LightGray;
            txtSearchUser.Text = "输入计划员名称";
            this.txtSearchUser.Leave += new System.EventHandler(this.txtSearchUser_Leave);
            this.txtSearchUser.Enter += new System.EventHandler(this.txtSearchUser_Enter);

            cbGameKind.SelectedIndex = 0;
            cbGameDirect.SelectedIndex = 0;
        }

        #region TextBox的提示
        private void txtSearchUser_Leave(object sender, EventArgs e)
        {
            if (txtSearchUser.Text == "")
            {
                txtSearchUser.Text = "输入计划员名称";
                txtSearchUser.ForeColor = Color.Gray;
            }
        }
        private void txtSearchUser_Enter(object sender, EventArgs e)
        {
            if (txtSearchUser.Text == "输入计划员名称")
            {
                txtSearchUser.Text = "";
                txtSearchUser.ForeColor = Color.Black;
            }
        }
        #endregion

        #region ComboBox的切換處理
        private void cbGameKind_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (cbGameKind.SelectedItem.ToString())
            {
                case "五星":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("五星组合");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "四星":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("四星组合");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "前三":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("前三组合");
                    cbGameDirect.Items.Add("直选和值");
                    cbGameDirect.Items.Add("直选跨度");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "中三":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("中三组合");
                    cbGameDirect.Items.Add("直选和值");
                    cbGameDirect.Items.Add("直选跨度");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "后三":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("后三组合");
                    cbGameDirect.Items.Add("直选和值");
                    cbGameDirect.Items.Add("直选跨度");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "前二":
                case "后二":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("直选和值");
                    cbGameDirect.Items.Add("直选跨度");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "定位胆":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("定位胆");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }
        #endregion

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

        private void picAD3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.cwl.gov.cn/");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateHistory();
        }
        /// <summary>
        /// 查看按鈕功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnView_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "p_name");
            var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_name like '%" + (string)cbGameKind.SelectedItem + (string)cbGameDirect.SelectedItem + "%'", dic);
            int x = 0;
            int y = 0;
            tableLayoutPanel1.Controls.Clear();


            for (int i = 0; i < getData.Count; i++)
            {
                Control control = new Button();
                control.Text = getData.ElementAt(i).ToString();
                control.Size = new System.Drawing.Size(140, 30);
                control.Name = String.Format("btx{0}y{1}", x, y);
                control.BackColor = Color.Red;
                control.Padding = new Padding(5);
                control.Dock = DockStyle.Fill;
                this.tableLayoutPanel1.Controls.Add(control, x, y);
                calHits();

            }
        }

        Dictionary<string, string> dic = new Dictionary<string, string>();
        public void calHits()
        {
            dic.Clear();
            //取得過去所有號碼
            Dictionary<int, string> dic_history = new Dictionary<int, string>();
            dic_history.Add(0, "number");
            var getHistory = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from HistoryNumber", dic_history);
            //取得上傳計畫 id 號碼
            Dictionary<int, string> dic_plan = new Dictionary<int, string>();
            dic_plan.Add(0, "p_name");
            dic_plan.Add(1, "p_rule");
            var getPlan = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan", dic_plan);
            //把 dic_plan 轉換成比較好操作的格式
            
            for (int i = 0; i < getPlan.Count; i = i + 2)
            {
                dic.Add(getPlan.ElementAt(i), getPlan.ElementAt(i + 1));
            }
            //統計擊中次數
            Dictionary<string, double> hitTimes = new Dictionary<string, double>();

            for (int i = 0; i < getPlan.Count / 2; i++)
            {
                int temp = 0;
                for (int j = 0; j < getHistory.Count; j++)
                {
                    if (dic.ElementAt(i).Value.IndexOf(getHistory.ElementAt(j)) != -1)
                        temp++;
                }
                int DoHaocount = 0;
                foreach (Match m in Regex.Matches(dic.ElementAt(i).Value, ","))
                {
                    DoHaocount++;
                }
                double hitPercent = (double)temp / 4;
                hitTimes.Add(dic.ElementAt(i).Key, hitPercent);

            }

            //依照擊中次數加入lsbsent
            //Dictionary<string, int> dic1_SortedByKey = hitTimes.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);

        }
    }
}
