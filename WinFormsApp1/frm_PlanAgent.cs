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

        checkNupdateData cnd = new checkNupdateData();
        //取得歷史開獎
        private void UpdateHistory()
        {
            if (rtxtHistory.Text == "") //無資料就全寫入(第一次載入頁面)
            {
                cnd.Start();

                for (int i = 120; i > 0; i--)
                {
                    rtxtHistory.Text += frmGameMain.jArr[i]["Issue"].ToString() + "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", "") + "\r\n";
                    dt_history.Add(frmGameMain.jArr[i]["Issue"].ToString() + "     " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", ""));
                    //改成server端記錄每期開獎?
                    //con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "exec[PR_checkNadd] '" + frmGameMain.jArr[i]["Issue"].ToString() + "','" + frmGameMain.jArr[i]["Number"].ToString().Replace(",", "") + "'");
                }
            }
            else if (!string.IsNullOrEmpty(rtxtHistory.Text) && dt_history.ElementAt(dt_history.Count() - 1).IndexOf(frmGameMain.jArr[0]["Issue"].ToString()) == -1)
            {
                rtxtHistory.Text += frmGameMain.jArr[0]["Issue"].ToString() + "  " + frmGameMain.jArr[0]["Number"].ToString().Replace(",", "") + "\r\n";
                dt_history.Add(frmGameMain.jArr[0]["Issue"].ToString() + "     " + frmGameMain.jArr[0]["Number"].ToString().Replace(",", ""));
                con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "exec[PR_checkNadd] '" + frmGameMain.jArr[0]["Issue"].ToString() + "','" + frmGameMain.jArr[0]["Number"].ToString().Replace(",", "") + "'");
            }
        }

        private void picAD3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.cwl.gov.cn/");
        }

        int timeCount = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timeCount++;
            if (timeCount % 15 == 0 || timeCount == 3)
                updateGod();
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
                            control.ForeColor = Color.Blue;
                            control.Padding = new Padding(5);
                            control.Dock = DockStyle.Fill;
                            control.Click += dynamicBt_Click;
                            this.tableLayoutPanel2.Controls.Add(control, x, y);
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


        int searchType = 0;//0 初始值 1 cb search 2 userNmae search
        /// <summary>
        /// 查看按鈕功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnView_Click(object sender, EventArgs e)
        {
            searchType = 1;
            button42.Enabled = false;
            tableLayoutPanel1.Controls.Clear();
            calHits(0);
            if (hitTimes.Count > 0)
            {
                for (int i = 0; i < hitTimes.Count; i++)
                {
                    Control control = new Button();
                    control.Text = hitTimes.ElementAt(i).Key;
                    control.Size = new System.Drawing.Size(140, 30);
                    control.Name = String.Format("btx{0}y{1}", 0, 0);
                    if (hitTimes.ElementAt(i).Value >= 0.8)
                        control.ForeColor = Color.Red;
                    else if (hitTimes.ElementAt(i).Value < 0.8 && hitTimes.ElementAt(i).Value >= 0.7)
                        control.ForeColor = Color.Blue;
                    else if (hitTimes.ElementAt(i).Value < 0.7 && hitTimes.ElementAt(i).Value >= 0.5)
                        control.ForeColor = Color.Gray;
                    else if (hitTimes.ElementAt(i).Value < 0.5)
                        control.ForeColor = Color.Gray;
                    else
                        control.BackColor = Color.Yellow;

                    control.Padding = new Padding(5);
                    control.Dock = DockStyle.Fill;
                    control.Click += dynamicBt_Click;
                    this.tableLayoutPanel1.Controls.Add(control, 0, 0);

                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("查無資料。");
            }

        }

        Dictionary<string, double> hitTimes = new Dictionary<string, double>();
        Dictionary<string, string> dic = new Dictionary<string, string>();
        public void calHits(int type)//0 cbSearch 1 text search
        {
            dic.Clear();
            //取得過去所有號碼
            Dictionary<int, string> dic_history = new Dictionary<int, string>();
            dic_history.Add(0, "number");
            string sqlQuery = "select * from Upplan";
            var getHistory = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from HistoryNumber", dic_history);
            //取得上傳計畫 id 號碼

            if (type == 0)
                sqlQuery = "select * from Upplan where p_name like '%" + (string)cbGameKind.SelectedItem + (string)cbGameDirect.SelectedItem + "%' order by p_id";
            else if (type == 1)
                sqlQuery = "select a.* from Upplan a left join userData b on a.p_account = b.account where b.name like '%" + txtSearchUser.Text + "%'  order by p_id";
            else if (type == 2)
                sqlQuery = "select a.* from Upplan a left join userData b on a.p_account = b.account where b.name like '%" + frmGameMain.globalUserName + "%'  order by p_id ";
            else if (type == 4)
                sqlQuery = "select * from Upplan  order by p_id";
            else if (type == 5)
                sqlQuery = "select * from Upplan where p_name like '%" + (string)cbGameKind.SelectedItem + (string)cbGameDirect.SelectedItem + "%' order by p_id desc";
            else if (type == 6)
                sqlQuery = "select a.* from Upplan a left join userData b on a.p_account = b.account where b.name like '%" + txtSearchUser.Text + "%'  order by p_id desc";

            Dictionary<int, string> dic_plan = new Dictionary<int, string>();
            dic_plan.Add(0, "p_name");
            dic_plan.Add(1, "p_rule");
            var getPlan = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", sqlQuery, dic_plan);
            //把 dic_plan 轉換成比較好操作的格式

            for (int i = 0; i < getPlan.Count; i = i + 2)
            {
                if(dic.ContainsKey(getPlan.ElementAt(i)))
                    dic.Add(getPlan.ElementAt(i)+"("+i+")", getPlan.ElementAt(i + 1));
                else
                    dic.Add(getPlan.ElementAt(i), getPlan.ElementAt(i + 1));

            }
            //統計擊中次數
            hitTimes.Clear();
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
        }

        private void button40_Click(object sender, EventArgs e)
        {
            System.Windows.Clipboard.SetText(richTextBox1.Text);
            System.Windows.Forms.MessageBox.Show("複製成功。");
        }

        private void btnSearchPlan_Click(object sender, EventArgs e)
        {
            searchType = 2;
            tableLayoutPanel1.Controls.Clear();
            calHits(1);
            if (hitTimes.Count > 0)
            {
                for (int i = 0; i < hitTimes.Count; i++)
                {
                    Control control = new Button();
                    control.Text = hitTimes.ElementAt(i).Key;
                    control.Size = new System.Drawing.Size(140, 30);
                    control.Name = String.Format("btx{0}y{1}", 0, 0);
                    if (hitTimes.ElementAt(i).Value >= 0.8)
                        control.ForeColor = Color.Red;
                    else if (hitTimes.ElementAt(i).Value < 0.8 && hitTimes.ElementAt(i).Value >= 0.7)
                        control.ForeColor = Color.Blue;
                    else if (hitTimes.ElementAt(i).Value < 0.7 && hitTimes.ElementAt(i).Value >= 0.5)
                        control.ForeColor = Color.Gray;
                    else if (hitTimes.ElementAt(i).Value < 0.5)
                        control.ForeColor = Color.Gray;
                    else
                        control.BackColor = Color.Yellow;

                    control.Padding = new Padding(5);
                    control.Dock = DockStyle.Fill;
                    control.Click += dynamicBt_Click;
                    this.tableLayoutPanel1.Controls.Add(control, 0, 0);

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

                for (int i = 0; i < hitTimes.Count; i++)
                {
                    Control control = new Button();
                    control.Text = hitTimes.ElementAt(i).Key;
                    control.Size = new System.Drawing.Size(140, 30);
                    control.Name = String.Format("btx{0}y{1}", x, y);
                    control.Tag = hitTimes.ElementAt(i).Key;

                    if (hitTimes.ElementAt(i).Value >= 0.8)
                        control.ForeColor = Color.Red;
                    else if (hitTimes.ElementAt(i).Value < 0.8 && hitTimes.ElementAt(i).Value >= 0.7)
                        control.ForeColor = Color.Blue;
                    else if (hitTimes.ElementAt(i).Value < 0.7 && hitTimes.ElementAt(i).Value >= 0.5)
                        control.ForeColor = Color.Gray;
                    else if (hitTimes.ElementAt(i).Value < 0.5)
                        control.ForeColor = Color.Gray;
                    else
                        control.BackColor = Color.Yellow;

                    control.Padding = new Padding(5);
                    control.Dock = DockStyle.Fill;
                    control.Click += dynamicBt_Click;
                    this.tableLayoutPanel2.Controls.Add(control, x, y);

                }
            }
            else
                System.Windows.Forms.MessageBox.Show("尚未登入帳號。");

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (hitTimes.Count > 0)
            {
                tableLayoutPanel1.Controls.Clear();
                for (int i = pageIndex; i < pageIndex + 27 && i < hitTimes.Count; i++)
                {
                    Control control = new Button();
                    control.Text = hitTimes.ElementAt(i).Key;
                    control.Size = new System.Drawing.Size(140, 30);
                    control.Name = String.Format("btx{0}y{1}", 0, 0);
                    if (hitTimes.ElementAt(i).Value >= 0.8)
                        control.ForeColor = Color.Red;
                    else if (hitTimes.ElementAt(i).Value < 0.8 && hitTimes.ElementAt(i).Value >= 0.7)
                        control.ForeColor = Color.Blue;
                    else if (hitTimes.ElementAt(i).Value < 0.7 && hitTimes.ElementAt(i).Value >= 0.5)
                        control.ForeColor = Color.Gray;
                    else if (hitTimes.ElementAt(i).Value < 0.5)
                        control.ForeColor = Color.Gray;
                    else
                        control.BackColor = Color.Yellow;
                    control.Padding = new Padding(5);
                    control.Dock = DockStyle.Fill;
                    control.Click += dynamicBt_Click;
                    this.tableLayoutPanel1.Controls.Add(control, i, 0);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (searchType == 1)//cb search
                calHits(5);
            else if (searchType == 2) //userName search
                calHits(6);

            if (hitTimes.Count > 0)
            {
                tableLayoutPanel1.Controls.Clear();
                for (int i = pageIndex; i < pageIndex + 27 && i < hitTimes.Count; i++)
                {
                    Control control = new Button();
                    control.Text = hitTimes.ElementAt(i).Key;
                    control.Size = new System.Drawing.Size(140, 30);
                    control.Name = String.Format("btx{0}y{1}", 0, 0);
                    if (hitTimes.ElementAt(i).Value >= 0.8)
                        control.ForeColor = Color.Red;
                    else if (hitTimes.ElementAt(i).Value < 0.8 && hitTimes.ElementAt(i).Value >= 0.7)
                        control.ForeColor = Color.Blue;
                    else if (hitTimes.ElementAt(i).Value < 0.7 && hitTimes.ElementAt(i).Value >= 0.5)
                        control.ForeColor = Color.Gray;
                    else if (hitTimes.ElementAt(i).Value < 0.5)
                        control.ForeColor = Color.Gray;
                    else
                        control.BackColor = Color.Yellow;
                    control.Padding = new Padding(5);
                    control.Dock = DockStyle.Fill;
                    control.Click += dynamicBt_Click;
                    this.tableLayoutPanel1.Controls.Add(control, i, 0);
                }
            }
        }
        #region 刷新大神榜資料
        private void updateGod()
        {
            richTextBox2.Text = "";
            calHits(4);
            for (int i = 0; i < hitTimes.Count; i++)
            {
                richTextBox2.Text += hitTimes.ElementAt(i).Key + "\r\n";
            }
        }
        #endregion
        #region 切換頁相關功能
        int pageIndex = 0;
        private void button43_Click(object sender, EventArgs e)
        {
            pageIndex = 0;
            button42.Enabled = false;
        }

        private void button42_Click(object sender, EventArgs e)
        {
            pageIndex -= 27;
            refreshTBpanel();
        }

        private void button41_Click(object sender, EventArgs e)
        {
            pageIndex += 27;
            refreshTBpanel();
        }

        private void refreshTBpanel()
        {
            tableLayoutPanel1.Controls.Clear();
            if (pageIndex >= 27)
                button42.Enabled = true;
            else
                button42.Enabled = false;
            for (int i = pageIndex; i < pageIndex + 27 && i < hitTimes.Count; i++)
            {
                Control control = new Button();
                control.Text = hitTimes.ElementAt(i).Key;
                control.Size = new System.Drawing.Size(140, 30);
                control.Name = String.Format("btx{0}y{1}", 0, 0);
                if (hitTimes.ElementAt(i).Value >= 0.8)
                    control.ForeColor = Color.Red;
                else if (hitTimes.ElementAt(i).Value < 0.8 && hitTimes.ElementAt(i).Value >= 0.7)
                    control.ForeColor = Color.Blue;
                else if (hitTimes.ElementAt(i).Value < 0.7 && hitTimes.ElementAt(i).Value >= 0.5)
                    control.ForeColor = Color.Gray;
                else if (hitTimes.ElementAt(i).Value < 0.5)
                    control.ForeColor = Color.Gray;
                else
                    control.BackColor = Color.Yellow;

                control.Padding = new Padding(5);
                control.Dock = DockStyle.Fill;
                control.Click += dynamicBt_Click;
                this.tableLayoutPanel1.Controls.Add(control, 0, 0);

            }
        }
        #endregion
    }

    class checkNupdateData
    {
        BackgroundWorker ww = new BackgroundWorker();
        Connection con = new Connection();
        //不帶參數建構子
        public checkNupdateData()
        { }
        public void Start()
        {
            ww.DoWork += new DoWorkEventHandler(doWork);
            ww.RunWorkerAsync();
            ww.WorkerSupportsCancellation = true;
        }
        public void Stop(){ }
        private void doWork(object sender,DoWorkEventArgs e)
        {
            for (int i =0;i<120;i++)
                con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "exec[PR_checkNadd] '" + frmGameMain.jArr[i]["Issue"].ToString() + "','" + frmGameMain.jArr[i]["Number"].ToString().Replace(",", "") + "'");
        }
    }
}
