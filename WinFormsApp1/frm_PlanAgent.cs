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
            picAD3.Visible = false;//先拿掉廣告 之後版本更新
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
                    dt_history.Add(frmGameMain.jArr[i]["Issue"].ToString() + "     " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", ""));
                }
            }
            else //有資料先判斷
            {
                if ((rtxtHistory.Text.Substring(0, 11) != frmGameMain.jArr[0]["Issue"].ToString()) && (frmGameMain.strHistoryNumberOpen != "?")) //有新資料了
                {
                    rtxtHistory.Text = "";
                    int i = frmGameMain.jArr.Count-1;
                    rtxtHistory.Text += frmGameMain.jArr[i]["Issue"].ToString() + "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "\r\n";
                    dt_history.Add(frmGameMain.jArr[i]["Issue"].ToString() + "     " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", ""));
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
            updateMyfavorite();
            label10.Text = "欢迎: " + frmGameMain.globalUserName;
        }

        static bool isFirstTime = true;
        public static void resetFavoriteFlag()
        {
            isFirstTime = true;
        }

        private void updateMyfavorite()
        {
            if (isFirstTime)
            {
                if (!string.IsNullOrEmpty(frmGameMain.globalUserAccount))
                {
                    Dictionary<int, string> dic = new Dictionary<int, string>();
                    dic.Add(0, "f_name");
                    var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from favorite where user_account = '" + frmGameMain.globalUserAccount + "'", dic);
                    int x = 0;
                    int y = 0;
                    tableLayoutPanel2.Controls.Clear();

                    if (getData.Count > 0)
                    {
                        for (int i = 0; i < getData.Count; i++)
                        {
                            Control control = new Button();
                            control.Text = getData.ElementAt(i).ToString();
                            control.Size = new System.Drawing.Size(140, 30);
                            control.Name = String.Format("btx{0}y{1}", x, y);
                            control.BackColor = Color.Red;
                            control.Padding = new Padding(5);
                            control.Dock = DockStyle.Fill;

                            control.Click += dynamicBt_Click;
                            this.tableLayoutPanel2.Controls.Add(control, x, y);

                            calHits();

                        }
                    }
                    isFirstTime = false;
                }
            }
           
        }

        private static string choosePlanName = "";
        List<string> dt_history = new List<string>();
        private void dynamicBt_Click(object sender, EventArgs e)
        {
            choosePlanName = (sender as Button).Text;
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "p_name");
            dic.Add(1, "p_account");
            dic.Add(2, "p_start");
            dic.Add(3, "p_end");
            dic.Add(4, "p_rule");
            var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_name = '" + (sender as Button).Text + "'", dic);
            richTextBox1.Text = getData.ElementAt(4).Substring(0, getData.ElementAt(4).Length);
            listBox1.Items.Clear();
            //檢查期數
            for (int i = 0; i < dt_history.Count; i++)
            {
                if (int.Parse(dt_history.ElementAt(i).Substring(4, 7)) >= int.Parse(getData.ElementAt(2).Substring(4, 7)) && int.Parse(dt_history.ElementAt(i).Substring(4, 7)) <= int.Parse(getData.ElementAt(3).Substring(4, 7)))
                {
                    if (getData.ElementAt(4).IndexOf(dt_history.ElementAt(i).Substring(dt_history.ElementAt(i).IndexOf(" ") + 1).Trim()) != -1)
                    {
                        listBox1.Items.Add(dt_history[i].ToString() + "     中");

                    }
                    else
                    {
                        listBox1.Items.Add(dt_history[i].ToString() + "     掛");

                    }
                }
                else
                {
                    listBox1.Items.Add(dt_history[i].ToString() + "     停");
                }
            }
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

            if (getData.Count > 0)
            {
                for (int i = 0; i < getData.Count; i++)
                {
                    Control control = new Button();
                    control.Text = getData.ElementAt(i).ToString();
                    control.Size = new System.Drawing.Size(140, 30);
                    control.Name = String.Format("btx{0}y{1}", x, y);
                    control.BackColor = Color.Red;
                    control.Padding = new Padding(5);
                    control.Dock = DockStyle.Fill;

                    control.Click += dynamicBt_Click;
                    this.tableLayoutPanel1.Controls.Add(control, x, y);
                    calHits();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("查無資料。");
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

        private void button40_Click(object sender, EventArgs e)
        {
            System.Windows.Clipboard.SetText(richTextBox1.Text);
            System.Windows.Forms.MessageBox.Show("複製成功。");
        }

        private void btnSearchPlan_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "p_name");
            var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select a.* from Upplan a left join userData b on a.p_account = b.account where b.name like '%"+txtSearchUser.Text+"%'", dic);
            int x = 0;
            int y = 0;
            tableLayoutPanel1.Controls.Clear();

            if (getData.Count > 0)
            {
                for (int i = 0; i < getData.Count; i++)
                {
                    Control control = new Button();
                    control.Text = getData.ElementAt(i).ToString();
                    control.Size = new System.Drawing.Size(140, 30);
                    control.Name = String.Format("btx{0}y{1}", x, y);
                    control.BackColor = Color.Red;
                    control.Padding = new Padding(5);
                    control.Dock = DockStyle.Fill;
                    control.Click += dynamicBt_Click;
                    this.tableLayoutPanel1.Controls.Add(control, x, y);
                    calHits();
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("查無資料。");
        }

        private void button37_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(frmGameMain.globalUserAccount))
            {
                con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "Insert into favorite(f_name,user_account) values('" + choosePlanName + "','" + frmGameMain.globalUserAccount + "')");
                System.Windows.Forms.MessageBox.Show("新增完成。");

                Dictionary<int, string> dic = new Dictionary<int, string>();
                dic.Add(0, "f_name");
                var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from favorite where user_account = '" + frmGameMain.globalUserAccount + "'", dic);
                int x = 0;
                int y = 0;
                tableLayoutPanel2.Controls.Clear();


                for (int i = 0; i < getData.Count; i++)
                {
                    Control control = new Button();
                    control.Text = getData.ElementAt(i).ToString();
                    control.Size = new System.Drawing.Size(140, 30);
                    control.Name = String.Format("btx{0}y{1}", x, y);
                    control.BackColor = Color.Red;
                    control.Padding = new Padding(5);
                    control.Dock = DockStyle.Fill;

                    control.Click += dynamicBt_Click;
                    this.tableLayoutPanel2.Controls.Add(control, x, y);

                    calHits();

                }
            }
            else
                System.Windows.Forms.MessageBox.Show("尚未登入帳號。");
            
        }
    }
}
