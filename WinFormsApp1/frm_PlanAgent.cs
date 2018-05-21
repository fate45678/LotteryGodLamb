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

            string NowDate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", @"/");
            label115.Text = NowDate + "历史开奖";

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
                    cbGameDirect.Items.Add("单式");
                    cbGameDirect.Items.Add("复式");
                    //cbGameDirect.Items.Add("五星组合");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "四星":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("单式");
                    cbGameDirect.Items.Add("复式");
                    //cbGameDirect.Items.Add("四星组合");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "前三":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("单式");
                    cbGameDirect.Items.Add("复式");
                    //cbGameDirect.Items.Add("前三组合");
                    cbGameDirect.Items.Add("和值");
                    cbGameDirect.Items.Add("跨度");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "中三":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("单式");
                    cbGameDirect.Items.Add("复式");
                    ///cbGameDirect.Items.Add("中三组合");
                    cbGameDirect.Items.Add("和值");
                    cbGameDirect.Items.Add("跨度");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "后三":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("单式");
                    cbGameDirect.Items.Add("复式");
                    //cbGameDirect.Items.Add("后三组合");
                    cbGameDirect.Items.Add("和值");
                    cbGameDirect.Items.Add("跨度");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "前二":
                case "后二":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("单式");
                    cbGameDirect.Items.Add("复式");
                    cbGameDirect.Items.Add("和值");
                    cbGameDirect.Items.Add("跨度");
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
            string date = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            if (rtxtHistory.Text == "") //無資料就全寫入(第一次載入頁面)
            {
                cnd.Start();

                for (int i = 120; i > 0; i--)
                {
                    if (frmGameMain.jArr[i]["Issue"].ToString().Contains(date))
                    { 
                        rtxtHistory.Text += "第" + frmGameMain.jArr[i]["Issue"].ToString() + "期  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", "") + "\r\n";
                        dt_history.Add(frmGameMain.jArr[i]["Issue"].ToString() + "     " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", ""));
                    }
                    //改成server端記錄每期開獎?
                    //con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "exec[PR_checkNadd] '" + frmGameMain.jArr[i]["Issue"].ToString() + "','" + frmGameMain.jArr[i]["Number"].ToString().Replace(",", "") + "'");
                }
            }
            else if (!string.IsNullOrEmpty(rtxtHistory.Text) && dt_history.ElementAt(dt_history.Count() - 1).IndexOf(frmGameMain.jArr[0]["Issue"].ToString()) == -1)
            {              
                rtxtHistory.Text += "第" + frmGameMain.jArr[0]["Issue"].ToString() + "期  " + frmGameMain.jArr[0]["Number"].ToString().Replace(",", "") + "\r\n";
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
        int amount = 0;
        private void dynamicBt_Click(object sender, EventArgs e)
        {
            amount = 0;
            choosePlanName = (sender as Button).Name;
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "p_name");
            dic.Add(1, "p_account");
            dic.Add(2, "p_start");
            dic.Add(3, "p_end");
            dic.Add(4, "p_rule");
            dic.Add(5, "p_note");
            string iiii = "select * from Upplan where p_name = '" + (sender as Button).Text + "'";
            var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_id = '" + (sender as Button).Name + "'", dic);
            richTextBox1.Text = getData.ElementAt(4).Substring(0, getData.ElementAt(4).Length);
            listBox1.Items.Clear();
            //檢查期數

            string NowDate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            var showJa = frmGameMain.jArr.Where(x => x["Issue"].ToString().Contains(NowDate)).ToList();

            long Start = Int64.Parse(getData.ElementAt(2));
            long End = Int64.Parse(getData.ElementAt(3));

            //用來填入今天的期數
            string tmpIssue = NowDate.Substring(0, 8);
            string Issue = "";

            //紀錄比對的Number
            string CheckNumber = "";

            //確認是否面一筆已開獎但是後一筆尚未開獎
            bool nextIsOpen = false;

            //中挂次數
            int win = 0, fail = 0;

            //最後補上的敘述的參數
            int totalWin = 0;

            //辨識種類
            string GameKind = cbGameKind.Text;

            for (int i = 1; i <= 120; i++)
            {
                Issue = tmpIssue + i.ToString("d3");
                var getNumber = frmGameMain.jArr.Where(x => x["Issue"].ToString().Contains(Issue)).ToList();

                //不等於零表示已經開獎
                if (getNumber.Count() != 0)
                {
                    //如果上傳的投注期數比較小表示沒投注
                    if (Int64.Parse(getData.ElementAt(2)) > Int64.Parse(Issue) || Int64.Parse(getData.ElementAt(3)) < Int64.Parse(Issue))
                    {
                        if (i % 2 == 0)
                        {
                            if (nextIsOpen)
                            {
                                if (win != 0)
                                {
                                    //win++;
                                    listBox1.Items.Add(Int64.Parse(Issue) - 1 + " 到 " + Issue + " 中(" + win + ")");
                                    win = 0;
                                    fail = 0;
                                }
                                else
                                {
                                    fail++;
                                    listBox1.Items.Add(Int64.Parse(Issue) - 1 + " 到 " + Issue + " 挂(" + fail + ")");
                                    fail = 0;
                                }
                            }
                            else
                            {
                                //listBox1.Items.Add(Int64.Parse(Issue) - 1 + " 到 " + Issue + " 未上傳計畫");
                            }

                            nextIsOpen = false;
                        }
                    }
                    else if (Int64.Parse(getData.ElementAt(2)) <= Int64.Parse(Issue) && Int64.Parse(getData.ElementAt(3)) >= Int64.Parse(Issue))
                    {
                        amount++;
                        CheckNumber = getNumber[0]["Number"].ToString().Replace(",", "");

                        if (GameKind.Contains("五星"))
                        {
                            CheckNumber = getNumber[0]["Number"].ToString().Replace(",", "");
                        }
                        else if (GameKind.Contains("四星"))
                        {
                            CheckNumber = getNumber[0]["Number"].ToString().Replace(",", "").Substring(0, 4);
                            //前三中三后三前二后二
                        }
                        else if (GameKind.Contains("中三"))
                        {
                            CheckNumber = getNumber[0]["Number"].ToString().Replace(",", "").Substring(1, 3);
                        }
                        else if (GameKind.Contains("前三"))
                        {
                            CheckNumber = getNumber[0]["Number"].ToString().Replace(",", "").Substring(0, 3);
                        }
                        else if (GameKind.Contains("后三"))
                        {
                            CheckNumber = getNumber[0]["Number"].ToString().Replace(",", "").Substring(2, 3);
                        }
                        else if (GameKind.Contains("前二"))
                        {
                            CheckNumber = getNumber[0]["Number"].ToString().Replace(",", "").Substring(0, 2);
                        }
                        else if (GameKind.Contains("后二"))
                        {
                            CheckNumber = getNumber[0]["Number"].ToString().Replace(",", "").Substring(3, 2);
                        }



                        //判斷有沒有中奖
                        if (getData.ElementAt(4).Contains(CheckNumber))
                        {
                            win++;
                            totalWin++;
                            if (i % 2 == 0)
                            {
                                listBox1.Items.Add(Int64.Parse(Issue) - 1 + " 到 " + Issue + " 中(" + win + ")");
                                win = 0;
                                fail = 0;
                            }
                        }
                        else if (win != 0) //如果第一個就中了 則只顯示中(1)
                        {
                            //totalWin++;
                            listBox1.Items.Add(Int64.Parse(Issue) - 1 + " 到 " + Issue + " 中(" + win + ")");
                            win = 0;
                            fail = 0;
                        }
                        else
                        {
                            fail++;
                            if (i % 2 == 0)
                            {
                                listBox1.Items.Add(Int64.Parse(Issue) - 1 + " 到 " + Issue + " 挂(" + fail + ")");
                                fail = 0;
                            }
                        }
                        nextIsOpen = true;
                    }
                }
                else //這裡開始表示尚未開獎
                {
                    if (i % 2 == 0)
                    {
                        if (nextIsOpen)
                            listBox1.Items.Add(Int64.Parse(Issue) - 1 + " 到 " + Issue + " 尚未開獎(2)");
                        else
                            listBox1.Items.Add(Int64.Parse(Issue) - 1 + " 到 " + Issue + " 尚未開獎(1)");
                    }
                    nextIsOpen = false;
                }
            }


            //最後補上Note
            //long amount = Int64.Parse(getData.ElementAt(3)) - Int64.Parse(getData.ElementAt(2)) + 1;
            if (getData.ElementAt(3).Substring(0,8) != NowDate)
            {
                amount = 0;
            }
            string Winning = "";
            listBox2.Items.Clear();
            listBox2.Items.Add(getData.ElementAt(5));
            listBox2.Items.Add("已投注: " + amount + "期");
            listBox2.Items.Add("中奖: " + totalWin + "期");

            if (totalWin == 0)
            {
                Winning = "中奖率0%";
            }
            else
            {
                calhits(getData.ElementAt(2), getData.ElementAt(3), getData.ElementAt(4), getData.ElementAt(0));
                Winning = "中奖率" + winRate.ToString("0.00") +"%";//calhits(getData.ElementAt(2), getData.ElementAt(3), getData.ElementAt(4), getData.ElementAt(0));//"中奖率" + (((double)totalWin / (double)amount) * 100).ToString("0.00") + "%";//calhits(getData.ElementAt(2), getData.ElementAt(3), getData.ElementAt(4), getData.ElementAt(0));
            }
            listBox2.Items.Add(Winning);
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
                string nowdate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");

                string[] hitTimesElementAt;

                for (int i = 0; i < hitTimes.Count; i++)
                {
                    Control control = new Button();
                    hitTimesElementAt = hitTimes.ElementAt(i).Key.ToString().Split(',');
                    control.Text = hitTimesElementAt[1] + "\r\n 中奖率" + hitTimes.ElementAt(i).Value.ToString("0.00") + "%  \r\n" + hitTimesElementAt[3];
                    if (hitTimesElementAt[4].Substring(0, 8) != nowdate)
                    {
                        control.Text = hitTimesElementAt[1] + "\r\n 中奖率0%  \r\n" + hitTimesElementAt[3];
                    }
                    
                    control.Size = new System.Drawing.Size(140, 130);
                    control.Name = hitTimesElementAt[0];
                    if (hitTimes.ElementAt(i).Value >= 80)
                    { 
                        control.BackColor = Color.Red;
                        control.ForeColor = Color.White;
                    }
                    else if (hitTimes.ElementAt(i).Value < 80 && hitTimes.ElementAt(i).Value >= 70)
                    {
                        control.BackColor = Color.Blue;
                        control.ForeColor = Color.White;
                    }
                    else if (hitTimes.ElementAt(i).Value < 70 && hitTimes.ElementAt(i).Value >= 50)
                    {
                        control.BackColor = Color.Green;
                        control.ForeColor = Color.White;
                    }
                    else if (hitTimes.ElementAt(i).Value < 50)
                    {
                        control.BackColor = Color.White;
                        control.ForeColor = Color.Black;
                    }
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
            string dateNow = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "") + "%"; 
            dic_history.Add(0, "number");
            string sqlQuery = "select * from Upplan";
            var getHistory = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from HistoryNumber where issue LIKE '" + dateNow + "'", dic_history);
            //取得上傳計畫 id 號碼

            if (type == 0)
                sqlQuery = "select * from Upplan where p_isoldplan = '1' AND p_name like '%" + (string)cbGameKind.SelectedItem + (string)cbGameDirect.SelectedItem + "%' order by p_id";
            else if (type == 1)
                sqlQuery = "select a.* from Upplan a left join userData b on a.p_account = b.account where p_isoldplan = '1' AND b.name like '%" + txtSearchUser.Text + "%'  order by p_id";
            else if (type == 2)
                sqlQuery = "select a.* from Upplan a left join userData b on a.p_account = b.account where p_isoldplan = '1' AND b.name like '%" + frmGameMain.globalUserName + "%'  order by p_id ";
            else if (type == 4)
                sqlQuery = "select * from Upplan where p_isoldplan = '1' order by p_id";
            else if (type == 5)
                sqlQuery = "select * from Upplan where p_isoldplan = '1' AND p_name like '%" + (string)cbGameKind.SelectedItem + (string)cbGameDirect.SelectedItem + "%' order by p_id desc";
            else if (type == 6)
                sqlQuery = "select a.* from Upplan a left join userData b on a.p_account = b.account where p_isoldplan = '1' AND b.name like '%" + txtSearchUser.Text + "%' order by p_id desc";

            Dictionary<int, string> dic_plan = new Dictionary<int, string>();
            dic_plan.Add(0, "p_name");
            dic_plan.Add(1, "p_rule");
            dic_plan.Add(2, "p_id");
            dic_plan.Add(3, "p_uploadDate");
            dic_plan.Add(4, "p_start");
            dic_plan.Add(5, "p_end");
            var getPlan = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", sqlQuery, dic_plan);
            //把 dic_plan 轉換成比較好操作的格式

            for (int i = 0; i < getPlan.Count; i = i + 6)
            {
                if(dic.ContainsKey(getPlan.ElementAt(i)))
                    dic.Add(getPlan.ElementAt(i)+"("+i+")", getPlan.ElementAt(i + 1));
                else
                    dic.Add(getPlan.ElementAt(i) + "," + getPlan.ElementAt(i + 2) + "," + getPlan.ElementAt(i + 3) , getPlan.ElementAt(i + 1));

            }
            //統計擊中次數
            hitTimes.Clear();
            string showTest = "";
            string hitElem = "";
            for (int i = 0; i < getPlan.Count; i= i + 6)
            {
                hitElem = "";

                showTest = calhits(getPlan[i + 4], getPlan[i + 5] , getPlan[i + 1].Trim(), getPlan[i]);
                hitElem = getPlan[i + 2] + "," + getPlan[i] + "," + showTest + " ," + getPlan[i + 3].Substring(10) + " ," + getPlan[i + 4];
                hitTimes.Add(hitElem, winRate);
                //hitTimes.Add(dic.ElementAt(i).Key, hitPercent);
                //int temp = 0;
                //for (int j = 0; j < getHistory.Count; j++)
                //{
                //    if (dic.ElementAt(i).Value.IndexOf(getHistory.ElementAt(j)) != -1)
                //        temp++;
                //}
                //int DoHaocount = 0;
                //foreach (Match m in Regex.Matches(dic.ElementAt(i).Value, ","))
                //{
                //    DoHaocount++;
                //}
                //double hitPercent = (double)temp / 4;
                //hitTimes.Add(dic.ElementAt(i).Key, hitPercent);
            }
        }

        double winRate;
        public static int loginButtonType = 0;
        private string calhits(string start, string end, string number, string type)
        {
            if (loginButtonType == 0)
                return "";
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "number");
            var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from HistoryNumber where issue between " + start + " and " + end, dic).ToArray();
            var st = Regex.Split(number, " ").ToArray() ;
            int sum = 0;
            int hits = 0;
            #region 玩法
            if (type.IndexOf("五星") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    if (st.Length == 1)
                    {
                        sum++;
                        if (st[0] == dt[i])
                        {
                            hits++;
                            break;
                        }
                    }
                    else
                    { 
                        for (int j = 0; j < st.Length -1; j++)
                        {                     
                            sum++;
                            if (Array.IndexOf(st, dt[i]) > -1)//st[j] == dt[i]
                            {
                                hits++;
                                break;
                            }
                        }
                    }
                }
                #region 中奖率
                //string date
                if (hits == 0)
                {
                    winRate = 0;
                    return "中奖率0%";
                }
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    winRate = result;
                    if (result.ToString().Length > 3)
                    {
                        //Winning = "中奖率" + result.ToString().Substring(0, 5) + "%";
                        return "中奖率" + result.ToString("0.00").Substring(0, 3) + "%";
                    }
                    else
                    {
                        //Winning = "中奖率" + result.ToString() + "%";
                        return "中奖率" + result.ToString("0.00") + "%";
                    }
                }
                #endregion
            }
            else if (type.IndexOf("四星") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    if (st.Length == 1)
                    {
                        sum++;
                        if (st[0] == dt[i])
                        {
                            hits++;
                            break;
                        }
                    }
                    else
                    { 
                        for (int j = 0; j < st.Length - 1; j++)
                        {
                            sum++;
                            if (Array.IndexOf(st, dt[i].Substring(0, 4)) > -1) //dt[i].Substring(1, 4) == st[j].Substring(1, 4)
                            {
                                hits++;
                                break;
                            }
                        }
                    }
                }
                #region 中奖率
                if (hits == 0)
                {
                    winRate = 0;
                    return "中奖率0%";
                }
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    winRate = result;
                    if (result.ToString().Length > 3)
                    {
                        //Winning = "中奖率" + result.ToString().Substring(0, 5) + "%";
                        return "中奖率" + result.ToString().Substring(0, 5) + "%";
                    }
                    else
                    {
                        //Winning = "中奖率" + result.ToString() + "%";
                        return "中奖率" + result.ToString() + "%";
                    }
                }
                #endregion
            }
            else if (type.IndexOf("前三") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    if (st.Length == 1)
                    {
                        sum++;
                        if (st[0] == dt[i])
                        {
                            hits++;
                            break;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < st.Length - 1; j++)
                        {
                            sum++;
                            if (Array.IndexOf(st, dt[i].Substring(0, 3)) > -1)//dt[i].Substring(0, 3) == st[j].Substring(0, 3))
                            {
                                hits++;
                                break;
                            }
                        }
                    }
                }
                #region 中奖率
                if (hits == 0)
                {
                    winRate = 0;
                    return "中奖率0%";
                }
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    winRate = result;
                    if (result.ToString().Length > 3)
                    {
                        //Winning = "中奖率" + result.ToString().Substring(0, 5) + "%";
                        return "中奖率" + result.ToString().Substring(0, 5) + "%";
                    }
                    else
                    {
                        //Winning = "中奖率" + result.ToString() + "%";
                        return "中奖率" + result.ToString() + "%";
                    }
                }
                #endregion
            }
            else if (type.IndexOf("中三") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    if (st.Length == 1)
                    {
                        sum++;
                        if (st[0] == dt[i])
                        {
                            hits++;
                            break;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < st.Length - 1; j++)
                        {
                            sum++;
                            if (Array.IndexOf(st, dt[i].Substring(1, 3)) > -1)//dt[i].Substring(1, 3) == st[j].Substring(1, 3))
                            {
                                hits++;
                                break;
                            }
                        }
                    }
                }
                #region 中奖率
                if (hits == 0)
                {
                    winRate = 0;
                    return "中奖率0%";
                }
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    winRate = result;
                    if (result.ToString().Length > 3)
                    {
                        //Winning = "中奖率" + result.ToString().Substring(0, 5) + "%";
                        return "中奖率" + result.ToString().Substring(0, 5) + "%";
                    }
                    else
                    {
                        //Winning = "中奖率" + result.ToString() + "%";
                        return "中奖率" + result.ToString() + "%";
                    }
                }
                #endregion
            }
            else if (type.IndexOf("后三") != -1)
            {

                for (int i = 0; i < dt.Count(); i++)
                {
                    if (st.Length == 1)
                    {
                        sum++;
                        if (st[0] == dt[i])
                        {
                            hits++;
                            break;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < st.Length - 1; j++)
                        {
                            sum++;
                            if (Array.IndexOf(st, dt[i].Substring(2, 3)) > -1)//dt[i].Substring(2, 3) == st[j].Substring(2, 3))
                            {
                                hits++;
                                break;
                            }
                        }
                    }
                }
                #region 中奖率
                if (hits == 0)
                {
                    winRate = 0;
                    return "中奖率0%";
                }
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    winRate = result;
                    if (result.ToString().Length > 3)
                    {
                        //Winning = "中奖率" + result.ToString().Substring(0, 5) + "%";
                        return "中奖率" + result.ToString().Substring(0, 5) + "%";
                    }
                    else
                    {
                        //Winning = "中奖率" + result.ToString() + "%";
                        return "中奖率" + result.ToString() + "%";
                    }
                }
                #endregion
            }
            else if (type.IndexOf("前二") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    if (st.Length == 1)
                    {
                        sum++;
                        if (st[0] == dt[i])
                        {
                            hits++;
                            break;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < st.Length - 1; j++)
                        {
                            sum++;
                            if (Array.IndexOf(st, dt[i].Substring(0, 2)) > -1)//dt[i].Substring(0, 2) == st[j].Substring(0, 2))
                            {
                                hits++;
                                break;
                            }
                        }
                    }
                }
                #region 中奖率
                if (hits == 0)
                {
                    winRate = 0;
                    return "中奖率0%";
                }
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    winRate = result;
                    if (result.ToString().Length > 3)
                    {
                        //Winning = "中奖率" + result.ToString().Substring(0, 5) + "%";
                        return "中奖率" + result.ToString().Substring(0, 5) + "%";
                    }
                    else
                    {
                        //Winning = "中奖率" + result.ToString() + "%";
                        return "中奖率" + result.ToString() + "%";
                    }
                }
                #endregion
            }
            else if (type.IndexOf("后二") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    if (st.Length == 1)
                    {
                        sum++;
                        if (st[0] == dt[i])
                        {
                            hits++;
                            break;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < st.Length - 1; j++)
                        {
                            sum++;
                            if (Array.IndexOf(st, dt[i].Substring(3, 2)) > -1)//dt[i].Substring(3, 2) == st[j].Substring(3, 2))
                            {
                                hits++;
                                break;
                            }
                        }
                    }
                }
                #region 中奖率
                if (hits == 0)
                {
                    winRate = 0;
                    return "中奖率0%";
                }
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    winRate = result;
                    if (result.ToString().Length > 3)
                    {
                        //Winning = "中奖率" + result.ToString().Substring(0, 5) + "%";
                        return "中奖率" + result.ToString().Substring(0, 5) + "%";
                    }
                    else
                    {
                        //Winning = "中奖率" + result.ToString() + "%";
                        return "中奖率" + result.ToString() + "%";
                    }
                }
                #endregion
            }
            else if (type.IndexOf("定位胆") != -1 && type.IndexOf("萬") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    for (int j = 0; j < st.Length - 1; j++)
                    {
                        sum++;
                        if (dt[i].Substring(0, 1) == st[j].Substring(0, 1))
                        {
                            hits++;
                            break;
                        }
                    }
                }
                #region 中奖率
                if (hits == 0)
                {
                    winRate = 0;
                    return "中奖率0%";
                }
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    winRate = result;
                    if (result.ToString().Length > 3)
                    {
                        //Winning = "中奖率" + result.ToString().Substring(0, 5) + "%";
                        return "中奖率" + result.ToString().Substring(0, 5) + "%";
                    }
                    else
                    {
                        //Winning = "中奖率" + result.ToString() + "%";
                        return "中奖率" + result.ToString() + "%";
                    }
                }
                #endregion
            }
            else if (type.IndexOf("定位胆") != -1 && type.IndexOf("千") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    for (int j = 0; j < st.Length - 1; j++)
                    {
                        sum++;
                        if (dt[i].Substring(1, 1) == st[j].Substring(1, 1))
                        {
                            hits++;
                            break;
                        }
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return "0%";
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 5) + "%";
                    else
                        return result.ToString() + "%";
                }
                #endregion
            }
            else if (type.IndexOf("定位胆") != -1 && type.IndexOf("百") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    for (int j = 0; j < st.Length - 1; j++)
                    {
                        sum++;
                        if (dt[i].Substring(2, 1) == st[j].Substring(2, 1))
                        {
                            hits++;
                            break;
                        }
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return "0%";
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 5) + "%";
                    else
                        return result.ToString() + "%";
                }
                #endregion
            }
            else if (type.IndexOf("定位胆") != -1 && type.IndexOf("十") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    for (int j = 0; j < st.Length - 1; j++)
                    {
                        sum++;
                        if (dt[i].Substring(3, 1) == st[j].Substring(3, 1))
                        {
                            hits++;
                            break;
                        }
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return "0%";
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 5) + "%";
                    else
                        return result.ToString() + "%";
                }
                #endregion
            }
            else if (type.IndexOf("定位胆") != -1 && type.IndexOf("個") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    for (int j = 0; j < st.Length - 1; j++)
                    {
                        sum++;
                        if (dt[i].Substring(4, 0) == st[j].Substring(4, 0))
                        {
                            hits++;
                            break;
                        }
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return "0%";
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 5) + "%";
                    else
                        return result.ToString() + "%";
                }
                #endregion
            }
            #endregion
            return "";
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
            //richTextBox2.Text = "";
            tableLayoutPanel3.Controls.Clear();
            calHits(4);
            string[] hitTimesElementAt;
            for (int i = 0; i < hitTimes.Count; i++)
            {
                //hitTimesElementAt = hitTimes.ElementAt(i).Key.ToString().Split(',');
                Control control = new Button();
                hitTimesElementAt = hitTimes.ElementAt(i).Key.ToString().Split(',');
                control.Text = hitTimesElementAt[1];
                control.Size = new System.Drawing.Size(100, 110);
                control.Name = hitTimesElementAt[0];

                control.Padding = new Padding(5);
                control.Dock = DockStyle.Fill;
                control.Click += dynamicBt_Click;
                this.tableLayoutPanel3.Controls.Add(control, 0, 0);

                //richTextBox2.Text += hitTimesElementAt[0] + "\r\n";
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
