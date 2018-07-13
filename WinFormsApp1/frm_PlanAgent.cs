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
using System.Data.SqlClient;

namespace WinFormsApp1
{
    public partial class frm_PlanAgent : Form
    {
        Connection con = new Connection();
        public frm_PlanAgent()
        {
            InitializeComponent();

            //讀取廣告
            loadUrlAdPicture();

            string NowDate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", @"/");
            label115.Text = NowDate + "历史开奖";

            //timeCheck.Visible = false;//先拿掉廣告 之後版本更新
            txtSearchUser.ForeColor = Color.LightGray;
            txtSearchUser.Text = "输入计划员名称";
            this.txtSearchUser.Leave += new System.EventHandler(this.txtSearchUser_Leave);
            this.txtSearchUser.Enter += new System.EventHandler(this.txtSearchUser_Enter);

            cbGameKind.SelectedIndex = 0;
            cbGameDirect.SelectedIndex = 0;
        }

        private void loadUrlAdPicture()
        {
            if (frmGameMain.PlanProxyUser != "" && frmGameMain.PlanProxyPassWord != "")
            {
                string User = frmGameMain.PlanProxyUser;
                clickUrl = getBackPlatfromDb(User);

                if (clickUrl == null)
                {
                    System.Windows.Forms.MessageBox.Show("读取错误请洽客服");
                    return;
                }

                string Url = string.Format("http://43.252.208.201:81/Upload/{0}/1/1.jpg", User);
                var request = WebRequest.Create(Url);
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    picBoxAdAngent.Image = Bitmap.FromStream(stream);
                    picBoxAdAngent.Click += new EventHandler(pic_Click);
                }
            }
        }

        string clickUrl = "";
        void pic_Click(object sender, EventArgs e)
        {
            // 將sender轉型成PictureBox
            PictureBox pic = sender as PictureBox;

            if (null == pic)
                return;

            System.Diagnostics.Process.Start(clickUrl);
        }

        private string getBackPlatfromDb(string User)
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";

            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
            con = new SqlConnection(connetionString);
            string date = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            string Sqlstr = "";
            string response = "";
            try
            {
                con.Open();
                Sqlstr = "Select Ad_ConnectUrl From AdBackPlatform WHERE Ad_UserName = '{0}' AND Ad_Type = '3'";
                SqlDataAdapter da = new SqlDataAdapter(string.Format(Sqlstr, User), con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                con.Close();

                response = ds.Tables[0].Rows[0]["Ad_ConnectUrl"].ToString();
                return response;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                return null;
            }
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
                    //cbGameDirect.Items.Add("复式");
                    //cbGameDirect.Items.Add("五星组合");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "四星":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("单式");
                    //cbGameDirect.Items.Add("复式");
                    //cbGameDirect.Items.Add("四星组合");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "前三":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("单式");
                    //cbGameDirect.Items.Add("复式");
                    //cbGameDirect.Items.Add("前三组合");
                    cbGameDirect.Items.Add("和值");
                    cbGameDirect.Items.Add("跨度");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "中三":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("单式");
                    //cbGameDirect.Items.Add("复式");
                    ///cbGameDirect.Items.Add("中三组合");
                    cbGameDirect.Items.Add("和值");
                    cbGameDirect.Items.Add("跨度");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "后三":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("单式");
                    //cbGameDirect.Items.Add("复式");
                    //cbGameDirect.Items.Add("后三组合");
                    cbGameDirect.Items.Add("和值");
                    cbGameDirect.Items.Add("跨度");
                    cbGameDirect.SelectedIndex = 0;
                    break;
                case "前二":
                case "后二":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("单式");
                    //cbGameDirect.Items.Add("复式");
                    //cbGameDirect.Items.Add("和值");
                    //cbGameDirect.Items.Add("跨度");
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

            if (rtxtHistory.Text == "") //無資料就全寫入
            {
                for (int i = 0; i < frmGameMain.jArr.Count; i++)
                {
                    //if (i == 120) break; //寫120筆就好
                    if (frmGameMain.jArr[i]["Issue"].ToString().Contains(date))
                        rtxtHistory.Text += "第" + frmGameMain.jArr[i]["Issue"].ToString() + "期  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "\r\n";
                }
            }
            else //有資料先判斷
            {

                if ((rtxtHistory.Text.Substring(0, 11) != frmGameMain.jArr[0]["Issue"].ToString()) && (frmGameMain.strHistoryNumberOpen != "?")) //有新資料了
                {
                    //cnd.Start();
                    rtxtHistory.Text = "";
                    for (int i = 0; i < frmGameMain.jArr.Count; i++)
                    {
                        //if (i == 120) break; //寫120筆就好
                        if (frmGameMain.jArr[i]["Issue"].ToString().Contains(date))
                            rtxtHistory.Text += "第" + frmGameMain.jArr[i]["Issue"].ToString() + "期  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "\r\n";
                    }
                }
            }
        }

        int timeCount = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //timeCount++;
            //if (timeCount % 15 == 0 || timeCount == 3)
            updateGod();
            if (frmGameMain.jArr != null)
            {
                UpdateHistory();
                //updateMyfavorite();
                timer1.Interval = 120000;
            }
            //label10.Text = "欢迎: " + frmGameMain.globalUserName;
        }

        static bool isFirstTime = true;
        public static void resetFavoriteFlag()
        {
            isFirstTime = true;
        }

        #region 我的最愛
        private void updateMyfavorite()
        {
            if (!string.IsNullOrEmpty(frmGameMain.globalUserAccount))
            {
                tableLayoutPanel2.Controls.Clear();
                //重新整理我的最愛
                DataTable dtFavorite = getfavorite();

                double winRate = 0;
                int x = 0, y = 0;
                for (int i = 0; i < dtFavorite.Rows.Count; i++)
                {
                    Control control = new Button();
                    control.Text = dtFavorite.Rows[i]["f_name"].ToString() + "中獎率" + dtFavorite.Rows[i]["f_hits"].ToString() + "%";
                    control.Size = new System.Drawing.Size(140, 30);
                    control.Name = dtFavorite.Rows[i]["f_id"].ToString();
                    control.Tag = dtFavorite.Rows[i]["f_id"].ToString();

                    winRate = double.Parse(dtFavorite.Rows[i]["f_hits"].ToString());
                    if (winRate >= 80)
                    {
                        control.BackColor = Color.Red;
                        control.ForeColor = Color.White;
                    }
                    else if (winRate < 80 && winRate >= 70)
                    {
                        control.BackColor = Color.Blue;
                        control.ForeColor = Color.White;
                    }
                    else if (winRate < 70 && winRate >= 50)
                    {
                        control.BackColor = Color.Green;
                        control.ForeColor = Color.White;
                    }
                    else if (winRate < 50)
                    {
                        control.BackColor = Color.White;
                        control.ForeColor = Color.Black;
                    }
                    else
                        control.BackColor = Color.Yellow;

                    control.Padding = new Padding(5);
                    control.Dock = DockStyle.Fill;
                    control.Click += dynamicFavoriteBt_Click;
                    this.tableLayoutPanel2.Controls.Add(control, x, y);

                    if (y < 3)
                        y++;
                    else
                    {
                        x++;
                        y = 0;
                    }
                }
            }
            //else
            //    System.Windows.Forms.MessageBox.Show("尚未登入帳號。");
        }

        private DataTable getfavorite()
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";

            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
            con = new SqlConnection(connetionString);
            //string SelectNowDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Substring(0, 10);
            try
            {
                con.Open();
                string Sqlstr = @"select * from favorite where user_account = '" + frmGameMain.globalUserAccount + "'";
                SqlDataAdapter da = new SqlDataAdapter(Sqlstr, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                return null;
            }
        }

        private DataTable getfavorite(string id)
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";

            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
            con = new SqlConnection(connetionString);
            //string SelectNowDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Substring(0, 10);
            try
            {
                con.Open();
                string Sqlstr = @"select f_issue, f_note, f_number from favorite where f_id = '" + id + "'";
                SqlDataAdapter da = new SqlDataAdapter(Sqlstr, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                return null;
            }
        }

        //收藏我的最愛
        private void button37_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(frmGameMain.globalUserAccount))
            {
                if (choosePlanName == "")
                {
                    System.Windows.Forms.MessageBox.Show("請先點選計畫");
                    return;
                }

                string insertName = choosePlanName;
                var insertArr = insertName.Trim().Replace("\r\n", "").Replace(" ", ",").Split(',');
                insertArr = insertArr.Where(val => val != "").ToArray();

                string fName = insertArr[0];
                string fHits = insertArr[1].Replace("中奖率", "").Replace("%", "");
                string fdate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace(@"/", "").Replace(" ", "").Replace(":", "");
                string fNumber = richTextBox1.Text;
                string fNote = "";
                for (int i = 0; i < listBox2.Items.Count; i++)
                    fNote += ',' + listBox2.Items[i].ToString();
                fNote = fNote.Substring(1);

                string fIssue = "";
                for (int i = 0; i < listBox1.Items.Count; i++)
                    fIssue += ',' + listBox1.Items[i].ToString();
                fIssue = fIssue.Substring(1);

                string sql = "Insert into favorite values('{0}', '{1}', '{2}', '{3}', {4}, '{5}', '{6}')";
                con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", string.Format(sql, fName, frmGameMain.globalUserAccount, fHits, fNumber, fdate, fNote, fIssue));
                System.Windows.Forms.MessageBox.Show("新增完成。");

                //重新整理我的最愛
                DataTable dtFavorite = getfavorite();
                tableLayoutPanel2.Controls.Clear();
                double winRate = 0;
                int x = 0, y = 0;
                for (int i = 0; i < dtFavorite.Rows.Count; i++)
                {
                    Control control = new Button();
                    control.Text = dtFavorite.Rows[i]["f_name"].ToString() + "中獎率" + dtFavorite.Rows[i]["f_hits"].ToString() + "%";
                    control.Size = new System.Drawing.Size(140, 30);
                    control.Name = dtFavorite.Rows[i]["f_id"].ToString();
                    control.Tag = dtFavorite.Rows[i]["f_id"].ToString();

                    winRate = double.Parse(dtFavorite.Rows[i]["f_hits"].ToString());
                    if (winRate >= 80)
                    {
                        control.BackColor = Color.Red;
                        control.ForeColor = Color.White;
                    }
                    else if (winRate < 80 && winRate >= 70)
                    {
                        control.BackColor = Color.Blue;
                        control.ForeColor = Color.White;
                    }
                    else if (winRate < 70 && winRate >= 50)
                    {
                        control.BackColor = Color.Green;
                        control.ForeColor = Color.White;
                    }
                    else if (winRate < 50)
                    {
                        control.BackColor = Color.White;
                        control.ForeColor = Color.Black;
                    }
                    else
                        control.BackColor = Color.Yellow;

                    control.Padding = new Padding(5);
                    control.Dock = DockStyle.Fill;
                    control.Click += dynamicFavoriteBt_Click;
                    this.tableLayoutPanel2.Controls.Add(control, x, y);

                    if (y < 3)
                        y++;
                    else
                    { 
                        x++;
                        y = 0;
                    }
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("尚未登入帳號。");

        }

        private void dynamicFavoriteBt_Click(object sender, EventArgs e)
        {
            choosePlanName = (sender as Button).Text;
            listBox2.Items.Clear();
            listBox1.Items.Clear();
            string id = (sender as Button).Name;
            DataTable dtFavorite = getfavorite(id);
            richTextBox1.Text = dtFavorite.Rows[0]["f_number"].ToString();

            string[] noteArr = dtFavorite.Rows[0]["f_note"].ToString().Split(',');
            for (int i = 0; i < noteArr.Count(); i++)
            {
                listBox2.Items.Add(noteArr[i]);
            }

            string[] issueArr = dtFavorite.Rows[0]["f_issue"].ToString().Split(',');
            for (int i = 0; i < issueArr.Count(); i++)
            {
                listBox1.Items.Add(issueArr[i]);
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {          
            if (frm_PlanUpload.loginButtonType == 1)
            {
                updateMyfavorite();
                timer3.Interval = 600000;
            }
        }
        #endregion

        private static string choosePlanName = "";
        List<string> dt_history = new List<string>();
        //int amount = 0;
        private void dynamicBt_Click(object sender, EventArgs e)
        {
            //int searchType = 0;//0 初始值 1 cb search 2 userNmae search

            string NowDate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            var showJa = frmGameMain.jArr.Where(x => x["Issue"].ToString().Contains(NowDate)).ToList();
            //amount = 0;
            choosePlanName = (sender as Button).Text;
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "p_name");
            dic.Add(1, "p_account");
            dic.Add(2, "p_start");
            dic.Add(3, "p_end");
            dic.Add(4, "p_rule");
            dic.Add(5, "p_note");
            listBox1.Items.Clear();

            string selectGameKind = cbGameKind.Text;

            //總共中獎幾次 掛幾次 總共投了幾注
            int oldtotalWin = 0, oldtotalFail = 0, oldtotalPlay = 0, countWin = 0, countPlay = 0; ;
            string SelectNowDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Substring(0, 10);
            string[] checkName = choosePlanName.Replace("\r\n", "").Split(' ');
            //先處理舊資料
            var getOldData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_uploadDate LIKE '" + SelectNowDate + "%' AND p_isoldplan = '2' AND p_name LIKE '%" + checkName[0] + "%' order by p_id", dic);
            for (int i = 0; i < getOldData.Count; i = i + 6)
            {
                oldtotalWin = 0; oldtotalFail = 0; countPlay = 0;
                //玩法種類
                string oldGameKind = getOldData.ElementAt(i);

                //上傳的開始以及結束期數
                long oldstart = Int64.Parse(getOldData.ElementAt(i + 2));
                long oldend = Int64.Parse(getOldData.ElementAt(i + 3));

                //上傳了幾期
                long oldamount = (oldend - oldstart) + 1;               
                
                string oldcheckNumber = "";

                bool OldisAllOpen = true;

                for (int ii = 0; ii < oldamount; ii++)
                {
                    var oldshowIssue = showJa.Where(x => x["Issue"].ToString().Contains((oldstart + ii).ToString())).ToList();

                    //表示還沒開獎
                    if (oldshowIssue.Count == 0)
                    {
                        listBox1.Items.Add(oldstart + " 到 " + oldend + " 尚未开奖(" + (oldamount) + ")");
                        OldisAllOpen = false;
                        break;
                    }
                    else if (oldGameKind.Contains("五星"))
                    {
                        oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "");
                    }
                    else if (oldGameKind.Contains("四星"))
                    {
                        oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(0, 4);
                        //前三中三后三前二后二
                    }
                    else if (oldGameKind.Contains("中三"))
                    {
                        oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(1, 3);
                    }
                    else if (oldGameKind.Contains("前三"))
                    {
                        oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(0, 3);
                    }
                    else if (oldGameKind.Contains("后三"))
                    {
                        oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(2, 3);
                    }
                    else if (oldGameKind.Contains("前二"))
                    {
                        oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(0, 2);
                    }
                    else if (oldGameKind.Contains("后二"))
                    {
                        oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(3, 2);
                    }

                    //checkNumber = showIssue[0]["Number"].ToString().Replace(",","");

                    //是否有中獎
                    if (getOldData.ElementAt(i + 4).Contains(oldcheckNumber))
                    {
                        oldtotalWin++;
                        oldtotalPlay++;
                        countWin += oldtotalWin;
                        countPlay++;
                        break;
                    }
                    else
                    {
                        oldtotalFail++;
                        oldtotalPlay++;
                        countPlay++;
                    }
                }

                if (OldisAllOpen)
                {
                    if (oldtotalWin != 0)
                    {
                        listBox1.Items.Add(oldstart + " 到 " + oldend + " 中(" + countPlay + ")");
                    }
                    else
                    {
                        listBox1.Items.Add(oldstart + " 到 " + oldend + " 挂(" + oldtotalFail + ")");
                    }
                }
            }

            var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_id = '" + (sender as Button).Name + "'", dic);

            if (getData.Count > 0)
            {
                //玩法種類
                string GameKind = getData.ElementAt(0);

                //上傳的開始以及結束期數
                long start = Int64.Parse(getData.ElementAt(2));
                long end = Int64.Parse(getData.ElementAt(3));

                //上傳了幾期
                long amount = (end - start) + 1;

                //總共中獎幾次 掛幾次 總共投了幾注
                int totalWin = 0, totalFail = 0, totalPlay = 0;

                string checkNumber = "";

                bool isAllOpen = true;

                for (int i = 0; i < amount; i++)
                {
                    var showIssue = showJa.Where(x => x["Issue"].ToString().Contains((start + i).ToString())).ToList();

                    //表示還沒開獎
                    if (showIssue.Count == 0)
                    {
                        listBox1.Items.Add(start + " 到 " + end + " 尚未开奖(" + (amount - totalPlay) + ")");
                        isAllOpen = false;
                        break;
                    }
                    else if (GameKind.Contains("五星"))
                    {
                        checkNumber = showIssue[0]["Number"].ToString().Replace(",", "");
                    }
                    else if (GameKind.Contains("四星"))
                    {
                        checkNumber = showIssue[0]["Number"].ToString().Replace(",", "").Substring(0, 4);
                        //前三中三后三前二后二
                    }
                    else if (GameKind.Contains("中三"))
                    {
                        checkNumber = showIssue[0]["Number"].ToString().Replace(",", "").Substring(1, 3);
                    }
                    else if (GameKind.Contains("前三"))
                    {
                        checkNumber = showIssue[0]["Number"].ToString().Replace(",", "").Substring(0, 3);
                    }
                    else if (GameKind.Contains("后三"))
                    {
                        checkNumber = showIssue[0]["Number"].ToString().Replace(",", "").Substring(2, 3);
                    }
                    else if (GameKind.Contains("前二"))
                    {
                        checkNumber = showIssue[0]["Number"].ToString().Replace(",", "").Substring(0, 2);
                    }
                    else if (GameKind.Contains("后二"))
                    {
                        checkNumber = showIssue[0]["Number"].ToString().Replace(",", "").Substring(3, 2);
                    }

                    //checkNumber = showIssue[0]["Number"].ToString().Replace(",","");

                    //是否有中獎
                    if (getData.ElementAt(4).Contains(checkNumber))
                    {
                        totalWin++;
                        totalPlay++;
                        countWin += totalWin;
                        break;
                    }
                    else
                    {
                        totalFail++;
                        totalPlay++;
                    }
                }

                if (isAllOpen)
                {
                    if (totalWin != 0)
                    {
                        listBox1.Items.Add(start + " 到 " + end + " 中(" + totalPlay + ")");
                    }
                    else
                    {
                        listBox1.Items.Add(start + " 到 " + end + " 挂(" + totalFail + ")");
                    }
                }


                //補上note敘述
                listBox2.Items.Clear();
                richTextBox1.Text = "";
                richTextBox1.Text = getData.ElementAt(4);
                if (getData.ElementAt(3).Substring(0, 8) != NowDate)
                {
                    amount = 0;
                }
                string WinRate = "";
                listBox2.Items.Add(getData.ElementAt(5));
                listBox2.Items.Add("已投注: " + (oldtotalPlay +totalPlay) + "期");
                listBox2.Items.Add("中奖: " + (countWin) + "期");
                WinRate = "中奖率0.00%";
                if (countWin != 0)
                    WinRate = "中奖率" + (((double)(countWin) / (double)(oldtotalPlay + totalPlay)) * 100).ToString("0.00") + "%";
                listBox2.Items.Add(WinRate);

                int item = 0;
                if (getData.ElementAt(0).Contains("五星"))//五星
                {
                    item = richTextBox1.Text.Replace(" ", "").Length / 5;
                }
                else if (getData.ElementAt(0).Contains("四星"))//四星
                {
                    item = richTextBox1.Text.Replace(" ", "").Length / 4;

                }
                else if (getData.ElementAt(0).Contains("前三") || getData.ElementAt(0).Contains("中三") || getData.ElementAt(0).Contains("后三"))//三星中三后三前二
                {
                    item = richTextBox1.Text.Replace(" ", "").Length / 3;
                }
                else if (getData.ElementAt(0).Contains("前二") || getData.ElementAt(0).Contains("后三"))//二星
                {
                    item = richTextBox1.Text.Replace(" ", "").Length / 2;
                }
                else //一星
                {
                    item = richTextBox1.Text.Replace(" ", "").Length / 1;
                }


                //todo都要修正中奖幾期ˊ
                //label15.Text = "共" + item + "注 ";
            }
            else
            {
                System.Windows.MessageBox.Show("查無資料");
                return;
            }


        }

        int searchType = 0;//0 初始值 1 cb search 2 userNmae search
        string[] hitTimesElementAt;
        /// <summary>
        /// 查看按鈕功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnView_Click(object sender, EventArgs e)
        {
            choosePlanName = "";
            searchType = 1;
            button42.Enabled = false;
            tableLayoutPanel1.Controls.Clear();
            calHits(0);
            if (hitTimes.Count > 0)
            {
                string nowdate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
               
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
            updateMyfavorite();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            richTextBox1.Text = "";
        }

        Dictionary<string, double> hitTimes = new Dictionary<string, double>();
        Dictionary<string, string> dic = new Dictionary<string, string>();
        public void calHits(int type)//0 cbSearch 1 text search
        {
            hitTimes.Clear();
            dic.Clear();
            //取得過去所有號碼
            Dictionary<int, string> dic_history = new Dictionary<int, string>();
            string dateNow = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss").Substring(0, 10).Replace(@"/","") + "%";
            string SelectNowDate = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss").Substring(0, 10);
            dic_history.Add(0, "number");
            string sqlQuery = "select * from Upplan";

            if (frm_PlanCycle.GameLotteryName == "重庆时时彩")
            {
                var getHistory = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from HistoryNumber where issue LIKE '" + dateNow + "'", dic_history);
            }
            else if (frm_PlanCycle.GameLotteryName == "腾讯官方彩")
            {
                var getHistory = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from QQFFC_HistoryNumber where issue LIKE '" + dateNow + "'", dic_history);
            }
            else if (frm_PlanCycle.GameLotteryName == "腾讯奇趣彩")
            {
                var getHistory = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from TENCENTFFC_HistoryNumber where issue LIKE '" + dateNow + "'", dic_history);
            }
            else if (frm_PlanCycle.GameLotteryName == "天津时时彩")
            {
                var getHistory = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from TJSSC_HistoryNumber where issue LIKE '" + dateNow + "'", dic_history);
            }
            else if (frm_PlanCycle.GameLotteryName == "新疆时时彩")
            {
                var getHistory = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from XJSSC_HistoryNumber where issue LIKE '" + dateNow + "'", dic_history);
            }

            Dictionary<int, string> dic_Name = new Dictionary<int, string>();
            dic_Name.Add(0, "name");
            var getUser = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "  select userData.name from Upplan inner join userData on  Upplan.p_account = userData.account where p_name like '%" + frm_PlanCycle.GameLotteryName + (string)cbGameKind.SelectedItem + (string)cbGameDirect.SelectedItem + "%' AND [p_isoldplan] = 1 and p_uploadDate LIKE '%" + SelectNowDate + "%' group by userData.name", dic_Name);
                
            //取得上傳計畫 id 號碼
            for (int iName = 0; iName < getUser.Count; iName++)
            {
                string user = getUser.ElementAt(iName).ToString();

                if (type == 0)
                    //"select * from Upplan where p_uploadDate LIKE '" + SelectNowDate + "%' AND p_account = '" + frmGameMain.globalUserAccount + "' order by p_uploadDate desc"
                    sqlQuery = "select * from Upplan where p_uploadDate LIKE '" + SelectNowDate + "%' AND p_name like '%" + user + frm_PlanCycle.GameLotteryName + (string)cbGameKind.SelectedItem + (string)cbGameDirect.SelectedItem + "%' order by p_id desc";
                else if (type == 1)
                    sqlQuery = "select a.* from Upplan a left join userData b on a.p_account = b.account where p_name='%" + user + frm_PlanCycle.GameLotteryName + "%' AND p_isoldplan = '1' AND b.name like '%" + txtSearchUser.Text + "%'  order by p_id desc";
                else if (type == 2)
                    sqlQuery = "select a.* from Upplan a left join userData b on a.p_account = b.account where p_name='%" + user + frm_PlanCycle.GameLotteryName + "%' AND p_isoldplan = '1' AND b.name like '%" + frmGameMain.globalUserName + "%'  order by p_id desc ";
                else if (type == 4)
                    sqlQuery = "select * from Upplan where p_uploadDate LIKE '" + SelectNowDate + "%' AND p_name LIKE '%" + user + frm_PlanCycle.GameLotteryName + "%' AND p_isoldplan = '1' AND p_hits > 30 order by p_hits desc";
                else if (type == 5)
                    sqlQuery = "select * from Upplan where p_name like '%" + (string)cbGameKind.SelectedItem + (string)cbGameDirect.SelectedItem + "%' order by p_id desc";
                else if (type == 6)
                    sqlQuery = "select a.* from Upplan a left join userData b on a.p_account = b.account where p_name='%" + user + frm_PlanCycle.GameLotteryName + "%' AND p_isoldplan = '1' AND b.name like '%" + txtSearchUser.Text + "%' order by p_id desc";

                Dictionary<int, string> dic_plan = new Dictionary<int, string>();
                dic_plan.Add(0, "p_name");
                dic_plan.Add(1, "p_rule");
                dic_plan.Add(2, "p_id");
                dic_plan.Add(3, "p_uploadDate");
                dic_plan.Add(4, "p_start");
                dic_plan.Add(5, "p_end");
                var getPlan = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", sqlQuery.Replace(" 上","").Replace("下",""), dic_plan);

                //把 dic_plan 轉換成比較好操作的格式
                if (getPlan.Count() == 0)
                {
                    System.Windows.MessageBox.Show("查無資料");
                    return;
                }

                for (int i = 0; i < getPlan.Count - 6; i = i + 6)
                {
                    if (dic.ContainsKey(getPlan.ElementAt(i)))
                        dic.Add(getPlan.ElementAt(i) + "(" + i + ")", getPlan.ElementAt(i + 1));
                    else
                        dic.Add(getPlan.ElementAt(i) + "," + getPlan.ElementAt(i + 2) + "," + getPlan.ElementAt(i + 3), getPlan.ElementAt(i + 1));

                }
                //統計擊中次數
                //hitTimes.Clear();
                string showTest = "";
                string hitElem = "";

                string NowDate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
                var showJa = frmGameMain.jArr.Where(x => x["Issue"].ToString().Contains(NowDate)).ToList();
                int oldtotalWin = 0, oldtotalFail = 0, oldtotalPlay = 0, countWin = 0;

                for (int i = 0; i < getPlan.Count; i = i + 6)
                {
                    oldtotalWin = 0; oldtotalFail = 0;

                    hitElem = "";
                    //玩法種類
                    string oldGameKind = getPlan.ElementAt(i);

                    //上傳的開始以及結束期數
                    long oldstart = Int64.Parse(getPlan.ElementAt(i + 4));
                    long oldend = Int64.Parse(getPlan.ElementAt(i + 5));

                    //上傳了幾期
                    long oldamount = (oldend - oldstart) + 1;

                    //總共中獎幾次 掛幾次 總共投了幾注


                    string oldcheckNumber = "";


                    for (int ii = 0; ii < oldamount; ii++)
                    {
                        var oldshowIssue = showJa.Where(x => x["Issue"].ToString().Contains((oldstart + ii).ToString())).ToList();

                        //表示還沒開獎
                        if (oldshowIssue.Count == 0)
                        {
                            listBox1.Items.Add(oldstart + " 到 " + oldend + " 尚未开奖(" + (oldamount - oldtotalPlay) + ")");

                            break;
                        }
                        else if (oldGameKind.Contains("五星"))
                        {
                            oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "");
                        }
                        else if (oldGameKind.Contains("四星"))
                        {
                            oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(0, 4);
                            //前三中三后三前二后二
                        }
                        else if (oldGameKind.Contains("中三"))
                        {
                            oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(1, 3);
                        }
                        else if (oldGameKind.Contains("前三"))
                        {
                            oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(0, 3);
                        }
                        else if (oldGameKind.Contains("后三"))
                        {
                            oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(2, 3);
                        }
                        else if (oldGameKind.Contains("前二"))
                        {
                            oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(0, 2);
                        }
                        else if (oldGameKind.Contains("后二"))
                        {
                            oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(3, 2);
                        }

                        //checkNumber = showIssue[0]["Number"].ToString().Replace(",","");

                        //是否有中獎
                        if (getPlan.ElementAt(i + 1).Contains(oldcheckNumber))
                        {
                            oldtotalWin++;
                            oldtotalPlay++;
                            countWin += oldtotalWin;
                            break;
                        }
                        else
                        {
                            oldtotalFail++;
                            oldtotalPlay++;
                        }
                    }
                }

                //showTest = calhits(getPlan[i + 4], getPlan[i + 5] , getPlan[i + 1].Trim(), getPlan[i]);
                string win = "中奖率0.00%";
                if (countWin != 0)
                    win = "中奖率" + (((double)(countWin) / (double)oldtotalPlay) * 100).ToString("0.00") + "%";
                hitElem = getPlan[2] + "," + getPlan[0] + "," + win + " ," + getPlan[3].Substring(10) + " ," + getPlan[4];
                double num = 0;
                if (countWin == 0)
                    num = 0;
                else
                    num = (((double)(countWin) / (double)oldtotalPlay) * 100);
                hitTimes.Add(hitElem, num);

                string[] updateWinRateArr = hitElem.Split(',');

                con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "Update Upplan set p_hits = '" + num + "' WHERE p_id = '" + int.Parse(updateWinRateArr[0]) + "'");
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


        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (hitTimes.Count > 0)
            {
                var hitTimesTmp = hitTimes.OrderByDescending(s => s.Value).ToArray();
                tableLayoutPanel1.Controls.Clear();
                string nowdate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
                int x = 0, y = 0;
                for (int i = 0; i < hitTimesTmp.Count(); i++)
                {
                    Control control = new Button();
                    hitTimesElementAt = hitTimesTmp.ElementAt(i).Key.ToString().Split(',');
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
                    this.tableLayoutPanel1.Controls.Add(control, x, y);
                    if (x < 2)
                        x++;
                    else
                    {
                        y++;
                        x = 0;
                    }
                }
                //for (int i = pageIndex; i < pageIndex + 27 && i < hitTimes.Count; i++)
                //{
                //    Control control = new Button();
                //    control.Text = hitTimes.ElementAt(i).Key;
                //    control.Size = new System.Drawing.Size(140, 30);
                //    control.Name = hitTimesElementAt[0];

                //    if (hitTimes.ElementAt(i).Value >= 80)
                //    {
                //        control.BackColor = Color.Red;
                //        control.ForeColor = Color.White;
                //    }
                //    else if (hitTimes.ElementAt(i).Value < 80 && hitTimes.ElementAt(i).Value >= 70)
                //    {
                //        control.BackColor = Color.Blue;
                //        control.ForeColor = Color.White;
                //    }
                //    else if (hitTimes.ElementAt(i).Value < 70 && hitTimes.ElementAt(i).Value >= 50)
                //    {
                //        control.BackColor = Color.Green;
                //        control.ForeColor = Color.White;
                //    }
                //    else if (hitTimes.ElementAt(i).Value < 50)
                //    {
                //        control.BackColor = Color.White;
                //        control.ForeColor = Color.Black;
                //    }
                //    else
                //        control.BackColor = Color.Yellow;

                //    control.Padding = new Padding(5);
                //    control.Dock = DockStyle.Fill;
                //    control.Click += dynamicBt_Click;
                //    this.tableLayoutPanel1.Controls.Add(control, i, 0);
                //}
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (hitTimes.Count > 0)
            {
                tableLayoutPanel1.Controls.Clear();
                string nowdate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
                int x = 0, y = 0;
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
                    this.tableLayoutPanel1.Controls.Add(control, x, y);
                    if (x < 2)
                        x++;
                    else
                    {
                        y++;
                        x = 0;
                    }
                }
            }
        }

        #region 刷新大神榜資料
        private void updateGod()
        {
            //richTextBox2.Text = "";
            tableLayoutPanel3.Controls.Clear();

            //calHits(4);
            DataTable GodList = GodUpdateTable();
            //string[] hitTimesElementAt;
            double checkWinRate = 0;
            int y = 0;
            for (int i = 0; i < GodList.Rows.Count; i++)
            {
                Control control = new Button();
                control.Text = GodList.Rows[i]["p_name"].ToString() + " 中獎率" + GodList.Rows[i]["p_hits"].ToString() + "%";
                control.Name = GodList.Rows[i]["p_id"].ToString();
                control.Size = new System.Drawing.Size(100, 110);
                checkWinRate = double.Parse(GodList.Rows[i]["p_hits"].ToString());
                if (checkWinRate >= 80)
                {
                    control.BackColor = Color.Red;
                    control.ForeColor = Color.White;
                }
                else if (checkWinRate < 80 && checkWinRate >= 70)
                {
                    control.BackColor = Color.Blue;
                    control.ForeColor = Color.White;
                }
                else if (checkWinRate < 70 && checkWinRate >= 50)
                {
                    control.BackColor = Color.Green;
                    control.ForeColor = Color.White;
                }
                else if (checkWinRate < 50)
                {
                    control.BackColor = Color.White;
                    control.ForeColor = Color.Black;
                }
                else
                    control.BackColor = Color.Yellow;
                control.Padding = new Padding(5);
                control.Dock = DockStyle.Fill;
                control.Click += dynamicGodBt_Click;
                this.tableLayoutPanel3.Controls.Add(control, 0, y);
                y++;
                //richTextBox2.Text += hitTimesElementAt[0] + "\r\n";
            }
        }

        private DataTable GodUpdateTable()
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";

            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
            con = new SqlConnection(connetionString);
            string SelectNowDate = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss").Substring(0, 10);
            try
            {
                con.Open();
                string Sqlstr = @"select * from Upplan where p_uploadDate LIKE '" + SelectNowDate + "%' AND p_name LIKE '%" + frm_PlanCycle.GameLotteryName + "%' AND p_isoldplan = '1' AND p_hits > 30 order by p_hits desc";
                SqlDataAdapter da = new SqlDataAdapter(Sqlstr, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                return null;
            }
        }

        private void dynamicGodBt_Click(object sender, EventArgs e)
        {
            //int searchType = 0;//0 初始值 1 cb search 2 userNmae search

            string NowDate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            var showJa = frmGameMain.jArr.Where(x => x["Issue"].ToString().Contains(NowDate)).ToList();
            //amount = 0;
            string[] choosePlanNameArr = ((sender as Button).Text).Split(' ');
            choosePlanName = choosePlanNameArr[0];
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "p_name");
            dic.Add(1, "p_account");
            dic.Add(2, "p_start");
            dic.Add(3, "p_end");
            dic.Add(4, "p_rule");
            dic.Add(5, "p_note");
            listBox1.Items.Clear();

            string selectGameKind = cbGameKind.Text;

            //總共中獎幾次 掛幾次 總共投了幾注
            int oldtotalWin = 0, oldtotalFail = 0, oldtotalPlay = 0, countWin = 0, countPlay = 0; ;
            string SelectNowDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Substring(0, 10);
            //先處理舊資料
            var getOldData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_uploadDate LIKE '" + SelectNowDate + "%' AND p_isoldplan = '2' AND p_name LIKE '%" + choosePlanName + "%' order by p_id", dic);
            for (int i = 0; i < getOldData.Count; i = i + 6)
            {
                oldtotalWin = 0; oldtotalFail = 0; countPlay = 0;
                //玩法種類
                string oldGameKind = getOldData.ElementAt(i);

                //上傳的開始以及結束期數
                long oldstart = Int64.Parse(getOldData.ElementAt(i + 2));
                long oldend = Int64.Parse(getOldData.ElementAt(i + 3));

                //上傳了幾期
                long oldamount = (oldend - oldstart) + 1;

                string oldcheckNumber = "";

                bool OldisAllOpen = true;

                for (int ii = 0; ii < oldamount; ii++)
                {
                    var oldshowIssue = showJa.Where(x => x["Issue"].ToString().Contains((oldstart + ii).ToString())).ToList();

                    //表示還沒開獎
                    if (oldshowIssue.Count == 0)
                    {
                        listBox1.Items.Add(oldstart + " 到 " + oldend + " 尚未开奖(" + (oldamount - oldtotalPlay) + ")");
                        OldisAllOpen = false;
                        break;
                    }
                    else if (oldGameKind.Contains("五星"))
                    {
                        oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "");
                    }
                    else if (oldGameKind.Contains("四星"))
                    {
                        oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(0, 4);
                        //前三中三后三前二后二
                    }
                    else if (oldGameKind.Contains("中三"))
                    {
                        oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(1, 3);
                    }
                    else if (oldGameKind.Contains("前三"))
                    {
                        oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(0, 3);
                    }
                    else if (oldGameKind.Contains("后三"))
                    {
                        oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(2, 3);
                    }
                    else if (oldGameKind.Contains("前二"))
                    {
                        oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(0, 2);
                    }
                    else if (oldGameKind.Contains("后二"))
                    {
                        oldcheckNumber = oldshowIssue[0]["Number"].ToString().Replace(",", "").Substring(3, 2);
                    }

                    //checkNumber = showIssue[0]["Number"].ToString().Replace(",","");

                    //是否有中獎
                    if (getOldData.ElementAt(i + 4).Contains(oldcheckNumber))
                    {
                        oldtotalWin++;
                        oldtotalPlay++;
                        countWin += oldtotalWin;
                        countPlay++;
                        break;
                    }
                    else
                    {
                        oldtotalFail++;
                        oldtotalPlay++;
                        countPlay++;
                    }
                }

                if (OldisAllOpen)
                {
                    if (oldtotalWin != 0)
                    {
                        listBox1.Items.Add(oldstart + " 到 " + oldend + " 中(" + countPlay + ")");
                    }
                    else
                    {
                        listBox1.Items.Add(oldstart + " 到 " + oldend + " 挂(" + oldtotalFail + ")");
                    }
                }
            }

            var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_id = '" + (sender as Button).Name + "'", dic);

            if (getData.Count > 0)
            {
                //玩法種類
                string GameKind = getData.ElementAt(0);

                //上傳的開始以及結束期數
                long start = Int64.Parse(getData.ElementAt(2));
                long end = Int64.Parse(getData.ElementAt(3));

                //上傳了幾期
                long amount = (end - start) + 1;

                //總共中獎幾次 掛幾次 總共投了幾注
                int totalWin = 0, totalFail = 0, totalPlay = 0;

                string checkNumber = "";

                bool isAllOpen = true;

                for (int i = 0; i < amount; i++)
                {
                    var showIssue = showJa.Where(x => x["Issue"].ToString().Contains((start + i).ToString())).ToList();

                    //表示還沒開獎
                    if (showIssue.Count == 0)
                    {
                        listBox1.Items.Add(start + " 到 " + end + " 尚未开奖(" + (amount - totalPlay) + ")");
                        isAllOpen = false;
                        break;
                    }
                    else if (GameKind.Contains("五星"))
                    {
                        checkNumber = showIssue[0]["Number"].ToString().Replace(",", "");
                    }
                    else if (GameKind.Contains("四星"))
                    {
                        checkNumber = showIssue[0]["Number"].ToString().Replace(",", "").Substring(0, 4);
                        //前三中三后三前二后二
                    }
                    else if (GameKind.Contains("中三"))
                    {
                        checkNumber = showIssue[0]["Number"].ToString().Replace(",", "").Substring(1, 3);
                    }
                    else if (GameKind.Contains("前三"))
                    {
                        checkNumber = showIssue[0]["Number"].ToString().Replace(",", "").Substring(0, 3);
                    }
                    else if (GameKind.Contains("后三"))
                    {
                        checkNumber = showIssue[0]["Number"].ToString().Replace(",", "").Substring(2, 3);
                    }
                    else if (GameKind.Contains("前二"))
                    {
                        checkNumber = showIssue[0]["Number"].ToString().Replace(",", "").Substring(0, 2);
                    }
                    else if (GameKind.Contains("后二"))
                    {
                        checkNumber = showIssue[0]["Number"].ToString().Replace(",", "").Substring(3, 2);
                    }

                    //checkNumber = showIssue[0]["Number"].ToString().Replace(",","");

                    //是否有中獎
                    if (getData.ElementAt(4).Contains(checkNumber))
                    {
                        totalWin++;
                        totalPlay++;
                        countWin += totalWin;
                        break;
                    }
                    else
                    {
                        totalFail++;
                        totalPlay++;
                    }
                }

                if (isAllOpen)
                {
                    if (totalWin != 0)
                    {
                        listBox1.Items.Add(start + " 到 " + end + " 中(" + totalPlay + ")");
                    }
                    else
                    {
                        listBox1.Items.Add(start + " 到 " + end + " 挂(" + totalFail + ")");
                    }
                }


                //補上note敘述
                listBox2.Items.Clear();
                richTextBox1.Text = "";
                richTextBox1.Text = getData.ElementAt(4);
                if (getData.ElementAt(3).Substring(0, 8) != NowDate)
                {
                    amount = 0;
                }
                string WinRate = "";
                listBox2.Items.Add(getData.ElementAt(5));
                listBox2.Items.Add("已投注: " + (oldtotalPlay + totalPlay) + "期");
                listBox2.Items.Add("中奖: " + (countWin) + "期");
                WinRate = "中奖率0.00%";
                if (countWin != 0)
                    WinRate = "中奖率" + (((double)(countWin) / (double)(oldtotalPlay + totalPlay)) * 100).ToString("0.00") + "%";
                listBox2.Items.Add(WinRate);

                int item = 0;
                if (getData.ElementAt(0).Contains("五星"))//五星
                {
                    item = richTextBox1.Text.Replace(" ", "").Length / 5;
                }
                else if (getData.ElementAt(0).Contains("四星"))//四星
                {
                    item = richTextBox1.Text.Replace(" ", "").Length / 4;

                }
                else if (getData.ElementAt(0).Contains("前三") || getData.ElementAt(0).Contains("中三") || getData.ElementAt(0).Contains("后三"))//三星中三后三前二
                {
                    item = richTextBox1.Text.Replace(" ", "").Length / 3;
                }
                else if (getData.ElementAt(0).Contains("前二") || getData.ElementAt(0).Contains("后三"))//二星
                {
                    item = richTextBox1.Text.Replace(" ", "").Length / 2;
                }
                else //一星
                {
                    item = richTextBox1.Text.Replace(" ", "").Length / 1;
                }


                //todo都要修正中奖幾期ˊ
                //label15.Text = "共" + item + "注 ";
            }
            else
            {
                System.Windows.MessageBox.Show("查無資料");
                return;
            }


        }
        #endregion

        #region 切換頁相關功能
        int pageIndex = 0;
        private void button43_Click(object sender, EventArgs e)
        {
            pageIndex = 0;
            button42.Enabled = false;
            refreshTBpanel();
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

            string nowdate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            if (hitTimesElementAt == null || hitTimesElementAt.Count() == 0)
                return;

            for (int i = pageIndex; i < pageIndex + 27 && i < hitTimes.Count; i++)
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
        #endregion

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //int index = this.listBox1.IndexFromPoint(e.Location);
            string name = this.listBox1.Text.Replace("到", ",");
            if (name == "")
                return;
            string[] nameArr = name.Split(',');
            long start = Int64.Parse(nameArr[0].Trim());
            long end = 0;
            if (frm_PlanCycle.GameLotteryName == "重庆时时彩" || frm_PlanCycle.GameLotteryName == "天津时时彩" || frm_PlanCycle.GameLotteryName == "新疆时时彩")
                end = Int64.Parse(nameArr[1].Substring(1, 11).Trim());
            else if (frm_PlanCycle.GameLotteryName == "腾讯奇趣彩" || frm_PlanCycle.GameLotteryName == "腾讯官方彩")
                end = Int64.Parse(nameArr[1].Substring(1, 12).Trim());

            //long end = Int64.Parse(nameArr[1].Substring(1, 11).Trim());
            string GameKind = choosePlanName;

            //int itmeType = 0;
            string[] itemCount;

            if (GameKind.Contains("五星"))
            {
                GameKind = "五星";
                //itmeType = 5;
            }
            else if (GameKind.Contains("四星"))
            {
                GameKind = "四星";
                //itmeType = 4;
                //前三中三后三前二后二
            }
            else if (GameKind.Contains("中三"))
            {
                GameKind = "中三";
                //itmeType = 3;
            }
            else if (GameKind.Contains("前三"))
            {
                GameKind = "前三";
                //itmeType = 3;
            }
            else if (GameKind.Contains("后三"))
            {
                GameKind = "后三";
                //itmeType = 3;
            }
            else if (GameKind.Contains("前二"))
            {
                GameKind = "前二";
                //itmeType = 2;
            }
            else if (GameKind.Contains("后二"))
            {
                GameKind = "后二";
                //itmeType = 2;
            }
            //string pid = checkedListBoxEx1.SelectedValue.ToString();
            Dictionary<int, string> dic = new Dictionary<int, string>();

            dic.Add(0, "p_rule");
            dic.Add(1, "p_end");
            //allorwUpdate = false;
            var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_account = '" + frmGameMain.globalUserAccount + "' AND p_name LIKE '%" + frm_PlanCycle.GameLotteryName + GameKind + "%'", dic);
            richTextBox1.Text = "";
            for (int i = 0; i < getData.Count(); i = i + 2)
            {
                if (name.Contains(getData.ElementAt(i + 1)))
                {
                    richTextBox1.Text = getData.ElementAt(i);
                    itemCount = richTextBox1.Text.Split(' ');
                    //label15.Text = "共" + itemCount.Count().ToString() + "注";
                    break;
                }
                else if (i + 3 < getData.Count() && end < Int64.Parse(getData.ElementAt(i + 3)))
                {
                    richTextBox1.Text = getData.ElementAt(i + 2);
                    break;
                }

            }
        }

        public static bool ischagneGameName = false;

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (ischagneGameName)
            {            
                tableLayoutPanel1.Controls.Clear();
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                richTextBox1.Text = "";

                ischagneGameName = false;

                if (frm_PlanCycle.GameLotteryName == "重庆时时彩")
                {
                    cbGameKind.Items.Clear();
                    cbGameKind.Items.Add("前二");
                    cbGameKind.Items.Add("后二");
                    cbGameKind.Items.Add("前三");
                    cbGameKind.Items.Add("中三");
                    cbGameKind.Items.Add("后三");
                    cbGameKind.Items.Add("四星");
                    cbGameKind.Items.Add("五星");
                }
                else if (frm_PlanCycle.GameLotteryName == "腾讯官方彩")
                {
                    cbGameKind.Items.Clear();
                    cbGameKind.Items.Add("前二");
                    cbGameKind.Items.Add("后二");
                    cbGameKind.Items.Add("前三");
                    cbGameKind.Items.Add("中三");
                    cbGameKind.Items.Add("后三");                
                }
                else if (frm_PlanCycle.GameLotteryName == "腾讯奇趣彩")
                {
                    cbGameKind.Items.Clear();
                    cbGameKind.Items.Add("前二");
                    cbGameKind.Items.Add("后二");
                    cbGameKind.Items.Add("前三");
                    cbGameKind.Items.Add("中三");
                    cbGameKind.Items.Add("后三");                 
                }
                else if (frm_PlanCycle.GameLotteryName == "天津时时彩")
                {
                    cbGameKind.Items.Clear();
                    cbGameKind.Items.Add("前二");
                    cbGameKind.Items.Add("后二");
                    cbGameKind.Items.Add("前三");
                    cbGameKind.Items.Add("中三");
                    cbGameKind.Items.Add("后三");
                    cbGameKind.Items.Add("四星");
                    cbGameKind.Items.Add("五星");
                }
                else if (frm_PlanCycle.GameLotteryName == "新疆时时彩")
                {
                    cbGameKind.Items.Clear();
                    cbGameKind.Items.Add("前二");
                    cbGameKind.Items.Add("后二");
                    cbGameKind.Items.Add("前三");
                    cbGameKind.Items.Add("中三");
                    cbGameKind.Items.Add("后三");
                    cbGameKind.Items.Add("四星");
                    cbGameKind.Items.Add("五星");
                }
                //
                cbGameKind.SelectedIndex = 0;              

                UpdateHistory();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            updateGod();
            choosePlanName = "";
            searchType = 1;
            button42.Enabled = false;
            tableLayoutPanel1.Controls.Clear();
            calHits(0);
            if (hitTimes.Count > 0)
            {
                string nowdate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");

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
            updateMyfavorite();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            richTextBox1.Text = "";
        }

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
            if (!ww.IsBusy)
                ww.RunWorkerAsync();
            ww.WorkerSupportsCancellation = true;
        }
        public void Stop(){ }
        private void doWork(object sender,DoWorkEventArgs e)
        {
            if (frm_PlanCycle.GameLotteryName == "重庆时时彩")
            {
                for (int i = 0; i < 240; i++)
                    con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "exec[PR_checkNadd] '" + frmGameMain.jArr[i]["Issue"].ToString() + "','" + frmGameMain.jArr[i]["Number"].ToString().Replace(",", "") + "'");
            }
            else if (frm_PlanCycle.GameLotteryName == "腾讯官方彩")
            {
                for (int i = 0; i < frmGameMain.jArr.Count; i++)
                    con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "exec[QQFFC_checkNadd] '" + frmGameMain.jArr[i]["Issue"].ToString() + "','" + frmGameMain.jArr[i]["Number"].ToString().Replace(",", "") + "'");
            }
            else if (frm_PlanCycle.GameLotteryName == "腾讯奇趣彩")
            {
                for (int i = 0; i < frmGameMain.jArr.Count; i++)
                    con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "exec[TENCENTFFC_checkNadd] '" + frmGameMain.jArr[i]["Issue"].ToString() + "','" + frmGameMain.jArr[i]["Number"].ToString().Replace(",", "") + "'");
            }
            else if (frm_PlanCycle.GameLotteryName == "新疆时时彩")
            {
                for (int i = 0; i < frmGameMain.jArr.Count; i++)
                    con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "exec[XJSSC_checkNadd] '" + frmGameMain.jArr[i]["Issue"].ToString() + "','" + frmGameMain.jArr[i]["Number"].ToString().Replace(",", "") + "'");
            }
        }


    }
}
