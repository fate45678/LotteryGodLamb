using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinFormsApp1
{
    public partial class frmGameMain : Form
    {
        public bool openMessage = false;
        //public static string strHistory;
        //public static string strHistoryCount; 
        public static string strHistoryNumberOpen;
        public static JArray jArr;
        public static string globalUserName;
        public static string globalUserAccount;
        public static string globalGetCurrentPeriod;
        public static string globalMessageTemp = "";
        //檢查是否為新開獎
        string checkIsnewIssue = "";

        //背景更新大神榜
        public static JArray jArrHistoryNumberForGod;
        string NowAnalyzeNumber = "";
        public static JArray NowAnalyzeNumberArr;

        public static JArray jArrHistoryNumber;

        public class NextPeriod
        {
            public string CloseTime { get; set; }
            public string SerialNumber { get; set; }
        }

        public frmGameMain()
        {
            frm_startLoading frm_startLoading = new frm_startLoading();
            frm_startLoading.ShowDialog();

            InitializeComponent();
            lblMenuPlanCycle.Click += new System.EventHandler(btnMenu_Click);
            lblMenuPlanAgent.Click += new System.EventHandler(btnMenu_Click);
            lblMenuPlanUpload.Click += new System.EventHandler(btnMenu_Click);
            lblMenuShrink.Click += new System.EventHandler(btnMenu_Click);
            lblMenuTrend.Click += new System.EventHandler(btnMenu_Click);
            lblMenuChart.Click += new System.EventHandler(btnMenu_Click);
            lblGame1_1.Click += new System.EventHandler(btnGame_Click);
            lblGame1_2.Click += new System.EventHandler(btnGame_Click);
            lblGame1_3.Click += new System.EventHandler(btnGame_Click);
            lblGame1_5.Click += new System.EventHandler(btnGame_Click);
            lblGame1_6.Click += new System.EventHandler(btnGame_Click);
            lblGame1_7.Click += new System.EventHandler(btnGame_Click);            
            lblGame2_1.Click += new System.EventHandler(btnGame_Click);
            lblGame2_2.Click += new System.EventHandler(btnGame_Click);
            lblGame2_3.Click += new System.EventHandler(btnGame_Click);
            lblGame2_4.Click += new System.EventHandler(btnGame_Click);
            lblGame2_6.Click += new System.EventHandler(btnGame_Click);
            lblGame3_1.Click += new System.EventHandler(btnGame_Click);
            lblGame3_2.Click += new System.EventHandler(btnGame_Click);
            lblGame3_3.Click += new System.EventHandler(btnGame_Click);
            lblGame3_4.Click += new System.EventHandler(btnGame_Click);
            lblGame3_5.Click += new System.EventHandler(btnGame_Click);
            lblGame3_6.Click += new System.EventHandler(btnGame_Click);
            lblGame3_7.Click += new System.EventHandler(btnGame_Click);
            lblGame3_9.Click += new System.EventHandler(btnGame_Click);

            timer_ShowMessage.Enabled = true;
            timer_GetGameInfo.Enabled = true;
        }

        private void frmGameMain_Load(object sender, EventArgs e)
        {
            //確認是否已經過了維護時間
            int NowDate = Int32.Parse(DateTime.Now.ToString("u").Replace("Z", "").Replace(":", "").Substring(10, 5));
            if (NowDate < 10 || NowDate > 2355)
            {
                MessageBox.Show("目前维护中请于12:10分后使用");
                this.Dispose();
            }

            frm_PlanCycle f_PlanCycle = new frm_PlanCycle();
            f_PlanCycle.TopLevel = false;
            f_PlanCycle.Size = this.Size;
            this.pnlMenuPlanCycle.Controls.Add(f_PlanCycle);
            f_PlanCycle.Show();

            frm_PlanAgent f_PlanAgent = new frm_PlanAgent();
            f_PlanAgent.TopLevel = false;
            f_PlanAgent.Size = this.Size;
            this.pnlMenuPlanAgent.Controls.Add(f_PlanAgent);
            f_PlanAgent.Show();

            frm_PlanUpload f_PlanUpload = new frm_PlanUpload();
            f_PlanUpload.TopLevel = false;
            f_PlanUpload.Size = this.Size;
            this.pnlMenuPlanUpload.Controls.Add(f_PlanUpload);
            f_PlanUpload.Show();

            frm_Shrink f_Shrink = new frm_Shrink();
            f_Shrink.TopLevel = false;
            f_Shrink.Size = this.Size;
            this.pnlMenuShrink.Controls.Add(f_Shrink);
            f_Shrink.Show();

            frm_Chart f_Chart = new frm_Chart();
            f_Chart.TopLevel = false;
            f_Chart.Size = this.Size;
            this.pnlMenuChart.Controls.Add(f_Chart);
            f_Chart.Show();
            #region 進入時預設停在哪一分頁 哪一彩種
            HD_MenuSelect.Text = "神灯周期计划";
            lblMenuPlanCycle.BackColor = Color.White;
            lblMenuPlanCycle.Refresh();
            pnlMenuPlanAgent.Visible = false;
            pnlMenuPlanUpload.Visible = false;
            pnlMenuShrink.Visible = false;
            pnlMenuChart.Visible = false;
            HD_GameSelect.Text = "重庆时时彩";
            #endregion

        }

        //按下上方選單
        private void btnMenu_Click(object sender, EventArgs e)
        {
            if (HD_MenuSelect.Text == ((Label)(sender)).Text)
                return;

            if (((Label)(sender)).Text == "趋势分析")
            {
                frm_Trend f_Trend = new frm_Trend();
                MessageBox.Show("尚未开放");
                return;
                //f_Trend.Show();
                //return;
            }


            ResetAllMenu(); //重設選單
            //DisposeForm(HD_MenuSelect.Text);
            HD_MenuSelect.Text = ((Label)(sender)).Text;

            switch (HD_MenuSelect.Text)
            {
                case "神灯周期计划":
                    lblMenuPlanCycle.BackColor = Color.White;
                    lblMenuPlanCycle.Refresh();
                    pnlMenuPlanCycle.Visible = true;
                    break;
                case "代理计划":
                    lblMenuPlanAgent.BackColor = Color.White;
                    lblMenuPlanAgent.Refresh();
                    pnlMenuPlanAgent.Visible = true;
                    break;
                case "计划上传":
                    lblMenuPlanUpload.BackColor = Color.White;
                    lblMenuPlanUpload.Refresh();
                    pnlMenuPlanUpload.Visible = true;
                    break;
                case "缩水工具":
                    lblMenuShrink.BackColor = Color.White;
                    lblMenuShrink.Refresh();
                    pnlMenuShrink.Visible = true;
                    break;
                case "走势图":
                    lblMenuChart.BackColor = Color.White;
                    lblMenuChart.Refresh();
                    pnlMenuChart.Visible = true;
                    break;
            }
        }

        //按下彩票種類
        private void btnGame_Click(object sender, EventArgs e)
        {
            if (HD_GameSelect.Text == ((Label)(sender)).Text)
                return;
            
            switch (((Label)(sender)).Text)
            {
                case "重庆时时彩":
                    ResetAllGame(); //重設彩票
                    lblGame1_1.BackColor = Color.Black;
                    lblGame1_1.ForeColor = Color.White;
                    lblGame1_1.Refresh();
                    HD_GameSelect.Text = ((Label)(sender)).Text;
                    frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                    MessageBox.Show("若是未更新请按下刷新按钮");
                    break;
                case "天津时时彩":
                    ResetAllGame(); //重設彩票
                    lblGame1_5.BackColor = Color.Black;
                    lblGame1_5.ForeColor = Color.White;
                    lblGame1_5.Refresh();
                    HD_GameSelect.Text = ((Label)(sender)).Text;
                    frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                    MessageBox.Show("若是未更新请按下刷新按钮");
                    break;
                case "腾讯官方彩":
                    ResetAllGame(); //重設彩票
                    lblGame1_2.BackColor = Color.Black;
                    lblGame1_2.ForeColor = Color.White;
                    lblGame1_2.Refresh();
                    HD_GameSelect.Text = ((Label)(sender)).Text;
                    frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                    MessageBox.Show("若是未更新请按下刷新按钮");
                    break;
                case "腾讯奇趣彩":
                    ResetAllGame(); //重設彩票
                    lblGame1_3.BackColor = Color.Black;
                    lblGame1_3.ForeColor = Color.White;
                    lblGame1_3.Refresh();
                    HD_GameSelect.Text = ((Label)(sender)).Text;
                    frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                    MessageBox.Show("若是未更新请按下刷新按钮");
                    break;
                case "新疆时时彩":
                    ResetAllGame(); //重設彩票
                    lblGame1_6.BackColor = Color.Black;
                    lblGame1_6.ForeColor = Color.White;
                    lblGame1_6.Refresh();
                    HD_GameSelect.Text = ((Label)(sender)).Text;
                    frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                    break;
                default:
                    MessageBox.Show(((Label)(sender)).Text + " 尚未開放");
                    break;
            }
            useHttpWebRequest_GetNextPeriod();
            useHttpWebRequest_GetHistory();

            //重新放置新的物件
            frm_PlanCycle f_PlanCycle = new frm_PlanCycle();
            pnlMenuPlanCycle.Controls.Clear();
            f_PlanCycle.TopLevel = false;
            f_PlanCycle.Size = this.Size;
            this.pnlMenuPlanCycle.Controls.Add(f_PlanCycle);
            f_PlanCycle.Show();

            frm_PlanUpload frm_PlanUpload = new frm_PlanUpload();
            frm_PlanUpload.isFirstTime = true;
            frm_PlanUpload.isChangeLotteryName = true;

            frm_PlanAgent frm_PlanAgent = new frm_PlanAgent();
            frm_PlanAgent.ischagneGameName = true;
        }

        //重設選單
        private void ResetAllMenu()
        {
            lblMenuPlanCycle.BackColor = Color.LightGray; lblMenuPlanCycle.Refresh();
            lblMenuPlanAgent.BackColor = Color.LightGray; lblMenuPlanAgent.Refresh();
            lblMenuPlanUpload.BackColor = Color.LightGray; lblMenuPlanUpload.Refresh();
            lblMenuShrink.BackColor = Color.LightGray; lblMenuShrink.Refresh();
            lblMenuChart.BackColor = Color.LightGray; lblMenuChart.Refresh();
            pnlMenuPlanCycle.Visible = false;
            pnlMenuPlanAgent.Visible = false;
            pnlMenuPlanUpload.Visible = false;
            pnlMenuShrink.Visible = false;
            pnlMenuChart.Visible = false;
        }

        //重設彩票
        private void ResetAllGame()
        {
            lblGame1_1.BackColor = Color.Transparent; lblGame1_1.ForeColor = Color.Black; lblGame1_1.Refresh();
            lblGame1_2.BackColor = Color.Transparent; lblGame1_2.ForeColor = Color.Black; lblGame1_2.Refresh();
            lblGame1_3.BackColor = Color.Transparent; lblGame1_3.ForeColor = Color.Black; lblGame1_3.Refresh();
            lblGame1_5.BackColor = Color.Transparent; lblGame1_5.ForeColor = Color.Black; lblGame1_5.Refresh();
            lblGame1_6.BackColor = Color.Transparent; lblGame1_6.ForeColor = Color.Black; lblGame1_6.Refresh();
            lblGame1_7.BackColor = Color.Transparent; lblGame1_7.ForeColor = Color.Black; lblGame1_7.Refresh();
            lblGame2_1.BackColor = Color.Transparent; lblGame2_1.ForeColor = Color.Black; lblGame2_1.Refresh();
            lblGame2_2.BackColor = Color.Transparent; lblGame2_2.ForeColor = Color.Black; lblGame2_2.Refresh();
            lblGame2_3.BackColor = Color.Transparent; lblGame2_3.ForeColor = Color.Black; lblGame2_3.Refresh();
            lblGame2_4.BackColor = Color.Transparent; lblGame2_4.ForeColor = Color.Black; lblGame2_4.Refresh();
            lblGame2_6.BackColor = Color.Transparent; lblGame2_6.ForeColor = Color.Black; lblGame2_6.Refresh();
            lblGame3_1.BackColor = Color.Transparent; lblGame3_1.ForeColor = Color.Black; lblGame3_1.Refresh();
            lblGame3_2.BackColor = Color.Transparent; lblGame3_2.ForeColor = Color.Black; lblGame3_2.Refresh();
            lblGame3_3.BackColor = Color.Transparent; lblGame3_3.ForeColor = Color.Black; lblGame3_3.Refresh();
            lblGame3_4.BackColor = Color.Transparent; lblGame3_4.ForeColor = Color.Black; lblGame3_4.Refresh();
            lblGame3_5.BackColor = Color.Transparent; lblGame3_5.ForeColor = Color.Black; lblGame3_5.Refresh();
            lblGame3_6.BackColor = Color.Transparent; lblGame3_6.ForeColor = Color.Black; lblGame3_6.Refresh();
            lblGame3_7.BackColor = Color.Transparent; lblGame3_7.ForeColor = Color.Black; lblGame3_7.Refresh();
            lblGame3_9.BackColor = Color.Transparent; lblGame3_9.ForeColor = Color.Black; lblGame3_9.Refresh();
        }

        //取得下一期時間
        private void useHttpWebRequest_GetNextPeriod()
        {
            //a.hywin888.net  hyqa.azurewebsites.net
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://a.hywin888.net/Bet/GetCurrentIssueByGameName?name=" + Game_Function.GameNameToCode(HD_GameSelect.Text) + "");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://hyqa.azurewebsites.net/Bet/GetCurrentIssueByGameName?name=" + Game_Function.GameNameToCode(HD_GameSelect.Text) + "");
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = "application/json";

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var temp = reader.ReadToEnd();
                        string json = JsonConvert.SerializeObject(temp);
                        NextPeriod NextPeriod = JsonConvert.DeserializeObject<NextPeriod>(temp);

                        if (NextPeriod.SerialNumber == null)
                        {
                            lblNextPeriod.Text = "00000000000";
                            frmGameMain.globalGetCurrentPeriod = "00000000000";
                            lblNextPeriodTime.Text = "-- : -- : --";
                        }
                        else
                        {
                            lblNextPeriod.Text = NextPeriod.SerialNumber;
                            frmGameMain.globalGetCurrentPeriod = NextPeriod.SerialNumber;
                            DateTime dt1 = Convert.ToDateTime(NextPeriod.CloseTime);
                            DateTime dt2 = DateTime.Now;
                            TimeSpan ts = new TimeSpan(dt1.Ticks - dt2.Ticks);
                            string hh = ts.Hours.ToString("00");
                            string mm = ts.Minutes.ToString("00");
                            string ss = ts.Seconds.ToString("00");
                            if (ss.IndexOf("-") > -1)
                                ss = "00";
                            lblNextPeriodTime.Text = hh + " : " + mm + " : " + ss;
                        }
                    }
                }
                else
                { }
            }
        }

        //取得歷史開獎
        private void useHttpWebRequest_GetHistory()
        {
            //a.hywin888.net hyqa.azurewebsites.net/
            DateTime dt = DateTime.Now.AddDays(0); //最早取前2天
            string dt1 = dt.Year + dt.Month.ToString("00") + dt.Day.ToString("00");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://hyqa.azurewebsites.net/DrawHistory/GetBySerialNumber?name=" + Game_Function.GameNameToCode(HD_GameSelect.Text) + "&startSerialNumber=" + dt1 + "&endSerialNumber=" + dt1 + "120");
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = "application/json";
            #region test in DL
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var temp = reader.ReadToEnd();
                        JArray ja = (JArray)JsonConvert.DeserializeObject(temp);
                        jArr = ja;
                        //處理最近開獎號碼
                        string lastWinPeriod = ja[0]["Issue"].ToString(); //最近開獎的期數

                        if ((lastWinPeriod.Substring(8, 3) == "120" && lblNextPeriod.Text.Substring(8, 3) == "002")
                            || (lastWinPeriod.Substring(8, 3) == "119" && lblNextPeriod.Text.Substring(8, 3) == "001")) //倒數結束後到完成開獎的空檔 針對跨日( 0404120期>0405002期 或 0404119期>0405001期 )
                        {
                            if (lastWinPeriod.Substring(8, 3) == "120")
                                lblCurrentPeriod.Text = lblNextPeriod.Text.Substring(0, 8) + "" + "001"; //當期
                            else
                                lblCurrentPeriod.Text = lastWinPeriod.Substring(0, 8) + "" + "120"; //當期
                            lblNumber1.Text = lblNumber2.Text = lblNumber3.Text = lblNumber4.Text = lblNumber5.Text = "?";
                            strHistoryNumberOpen = "?";
                        }
                        else if (Int16.Parse(lblNextPeriod.Text.Substring(8, 3)) - Int16.Parse(lastWinPeriod.Substring(8, 3)) == 2)//倒數結束後到完成開獎的空檔 針對同一日( 0404100期>0404098期 )
                        {
                            lblCurrentPeriod.Text = (Convert.ToInt64(lastWinPeriod) + 1).ToString(); //當期
                            lblNumber1.Text = lblNumber2.Text = lblNumber3.Text = lblNumber4.Text = lblNumber5.Text = "?";
                            strHistoryNumberOpen = "?";
                        }
                        else
                        {
                            lblCurrentPeriod.Text = lastWinPeriod; //當期
                            lblNumber1.Text = ja[0]["Number"].ToString().Substring(0, 1);
                            lblNumber2.Text = ja[0]["Number"].ToString().Substring(2, 1);
                            lblNumber3.Text = ja[0]["Number"].ToString().Substring(4, 1);
                            lblNumber4.Text = ja[0]["Number"].ToString().Substring(6, 1);
                            lblNumber5.Text = ja[0]["Number"].ToString().Substring(8, 1);
                            strHistoryNumberOpen = ja[0]["Number"].ToString().Substring(0, 1);


                            if (ja[0]["Number"].ToString() != checkIsnewIssue)
                            {
                                notifyIcon1.ShowBalloonTip(3000);
                                string tipTitle = "提示";
                                string tipContent = "第 " + ja[0]["Issue"].ToString() + " 期 " + ja[0]["Number"].ToString() + " 已開獎";
                                ToolTipIcon tipType = ToolTipIcon.Info;
                                notifyIcon1.ShowBalloonTip(3000, tipTitle, tipContent, tipType);                              
                             }

                            checkIsnewIssue = ja[0]["Number"].ToString();
                        }
                        //處理歷史開獎
                        //strHistory = "";
                        //strHistoryCount = ja.Count.ToString();
                        //for (int i = 0; i < ja.Count; i++)
                        //{
                        //    if (i == 120) break; //寫120筆就好
                        //    strHistory += ja[i]["Issue"].ToString() + "  " + ja[i]["Number"].ToString().Replace(",", " ") + "\r\n";
                        //}
                    }
                }
                else
                { }
            }
            #endregion

        }

        //顯示右下廣告
        private void ShowMessage()
        {
            if (openMessage == false)
            {
                openMessage = true;
                frm_Message f_Message = new frm_Message();
                int xWidth = SystemInformation.PrimaryMonitorSize.Width;
                int yHeight = SystemInformation.PrimaryMonitorSize.Height;
                f_Message.Location = new Point(xWidth - 295, yHeight - 305);

                f_Message.Show();
                closeIcon(clsIcon);
            }
        }

        private void timer_ShowMessage_Tick(object sender, EventArgs e)
        {
            //ShowMessage();
        }

        private void timer_GetGameInfo_Tick(object sender, EventArgs e)
        {
            //這邊取得時間自動關機
            int NowDate = Int32.Parse(DateTime.Now.ToString("u").Replace("Z", "").Replace(":", "").Substring(10, 5));
            if (NowDate < 10 || NowDate > 2355)
            {
                timer_GetGameInfo.Stop();
                MessageBox.Show("目前维护中请于12:10分后使用");
                this.Dispose();
                Application.Exit();
            }
            useHttpWebRequest_GetNextPeriod(); //取得下一期時間       
            useHttpWebRequest_GetHistory(); //取得歷史開獎
           

        }
        private void lblMenuPlanUpload_Click(object sender, EventArgs e)
        {

        }

        private void lblMenuPlanAgent_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 關閉視窗事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGameMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        public static int clsIcon = 0;
        public void closeIcon(int i)
        {
            if (i == 0)
                notifyIcon1.Dispose();
        }

        private void timerFourFiveGodinsert_Tick(object sender, EventArgs e)
        {
            if(!bgwGodinsert.IsBusy)
                bgwGodinsert.RunWorkerAsync();
        }

        private void ConnectDbGetHistoryNumber()
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";

            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
            con = new SqlConnection(connetionString);
            string date = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            try
            {
                con.Open();
                string Sqlstr = @"SELECT issue as Issue, number as Number FROM HistoryNumber WHERE issue LIKE '" + date + "%' ORDER BY issue DESC";
                SqlDataAdapter da = new SqlDataAdapter(Sqlstr, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                var str_json = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                //MessageBox.Show("Connection Open ! ");
                JArray ja = (JArray)JsonConvert.DeserializeObject(str_json);
                //string ii = ja[0]["issue"].ToString();
                jArrHistoryNumberForGod = ja;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        //抓取計畫名稱
        private DataTable getPlanName(string GamePlanType)
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";
            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
            con = new SqlConnection(connetionString);

            string date = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            try
            {
                con.Open();
                string Sqlstr = @"SELECT [GamePlan_Name] FROM [lottery].[dbo].[GamePlan] WHERE [GamePlan_type] = '{0}'";
                SqlDataAdapter da = new SqlDataAdapter(string.Format(Sqlstr, GamePlanType), con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        //抓取對獎資料
        private void ConnectDbGetRandomNumber(string type, int PlanName)
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";
            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
            con = new SqlConnection(connetionString);
            string date = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            try
            {
                //todo 修改每種不同的號碼
                if (PlanName == 0)
                {
                    con.Open();
                    string Sqlstr = @"SELECT top(40) number AS Number FROM RandomNumber WHERE date = '{0}' AND type = '{1}' ";
                    SqlDataAdapter da = new SqlDataAdapter(string.Format(Sqlstr, date, type), con);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    //NowAnalyzeNumber = ds.Tables[0].Rows[0]["Number"].ToString();
                    DataTable dt = ds.Tables[0];

                    NowAnalyzeNumber = dt.Rows[0]["Number"].ToString();
                    var str_json = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    //MessageBox.Show("Connection Open ! ");
                    JArray ja = (JArray)JsonConvert.DeserializeObject(str_json);
                    //string ii = ja[0]["issue"].ToString();
                    NowAnalyzeNumberArr = ja;
                }
                else if (PlanName == 1)
                {
                    con.Open();
                    string Sqlstr = @"SELECT [number] AS Number FROM 
(
SELECT ROW_NUMBER() OVER(ORDER BY [number]) NUM,
* FROM [RandomNumber]
WHERE date = '{0}' AND type = '{1}'
) A
WHERE NUM >40 AND NUM <81";
                    //string Sqlstr = @"SELECT top(40) number AS Number FROM RandomNumber WHERE date = '{0}' AND type = '{1}' order by NewID()";
                    SqlDataAdapter da = new SqlDataAdapter(string.Format(Sqlstr, date, type), con);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    //NowAnalyzeNumber = ds.Tables[0].Rows[0]["Number"].ToString();
                    DataTable dt = ds.Tables[0];

                    NowAnalyzeNumber = dt.Rows[0]["Number"].ToString();
                    var str_json = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    //MessageBox.Show("Connection Open ! ");
                    JArray ja = (JArray)JsonConvert.DeserializeObject(str_json);
                    //string ii = ja[0]["issue"].ToString();
                    NowAnalyzeNumberArr = ja;
                }
                else
                {
                    con.Open();
                    string Sqlstr = @"SELECT [number] AS Number FROM 
(
SELECT ROW_NUMBER() OVER(ORDER BY [number]) NUM,
* FROM [RandomNumber]
WHERE date = '{0}' AND type = '{1}'
) A
WHERE NUM >40 AND NUM <80";
                    //string Sqlstr = @"SELECT top(40) number AS Number FROM RandomNumber WHERE date = '{0}' AND type = '{1}' order by NewID()";
                    SqlDataAdapter da = new SqlDataAdapter(string.Format(Sqlstr, date, type), con);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    NowAnalyzeNumber = ds.Tables[0].Rows[2]["Number"].ToString();
                    da.Fill(ds);
                    //NowAnalyzeNumber = ds.Tables[0].Rows[0]["Number"].ToString();
                    DataTable dt = ds.Tables[0];

                    NowAnalyzeNumber = dt.Rows[0]["Number"].ToString();
                    var str_json = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    //MessageBox.Show("Connection Open ! ");
                    JArray ja = (JArray)JsonConvert.DeserializeObject(str_json);
                    //string ii = ja[0]["issue"].ToString();
                    NowAnalyzeNumberArr = ja;
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        private void InsertIntoGod(string insertInfo, string Rate)
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";

            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
            con = new SqlConnection(connetionString);
            try
            {
                con.Open();
                string Sqlstr = @"Insert into GodListPlanCycle values('{0}','{1}')";
                var cmd = new SqlCommand(string.Format(Sqlstr, insertInfo, Rate), con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteReader();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        private void bgwGodinsert_DoWork(object sender, DoWorkEventArgs e)
        {
            ConnectDbGetHistoryNumber();
            //玩法
            string GameKind = "";
            //數量
            string GameType = "";
            //單式或複式
            string GameDirect = "单式";

            //GameKind的七種玩法
            for (int i = 0; i < 2; i++)
            {
                switch (i)
                {
                    case 1:
                        GameKind = "四星";
                        break;
                    case 0:
                        GameKind = "五星";
                        break;
                }

                //玩法底下有分成各種數量的號碼
                for (int iPlus = 0; iPlus < 3; iPlus++)
                {
                    if (GameKind == "五星")
                    {
                        switch (iPlus)
                        {
                            case 0:
                                GameType = "30000+";
                                break;
                            case 1:
                                GameType = "40000+";
                                break;
                            case 2:
                                GameType = "50000+";
                                break;
                        }
                    }
                    else if (GameKind == "四星")
                    {
                        switch (iPlus)
                        {
                            case 0:
                                GameType = "3000+";
                                break;
                            case 1:
                                GameType = "4000+";
                                break;
                            case 2:
                                GameType = "5000+";
                                break;
                        }
                    }

                    //抓取計畫名稱
                    DataTable dtGamePlan = getPlanName(GameKind);
                    string GamePlanName = "";
                    string[] numHistory;

                    for (int iPlan = 0; iPlan < dtGamePlan.Rows.Count; iPlan++)
                    {
                        GamePlanName = dtGamePlan.Rows[iPlan]["GamePlan_name"].ToString();

                        //抓取比對的投注數量
                        ConnectDbGetRandomNumber(GameType, iPlan);

                        string threeNumber = NowAnalyzeNumber;
                        List<string> numHistoryList = new List<string>();
                        numHistoryList.Add(threeNumber);
                        numHistory = numHistoryList.ToArray();

                        string GameCycle = "";
                        //三期一周 / 二期一周 / 一期一周
                        for (int iGameCycle = 1; iGameCycle < 4; iGameCycle++)
                        {
                            int cycle_2 = 1; //比對開獎的周期數
                            int sumBets = 0;
                            int sumWin = 0;

                            if (GameType.Contains("3"))
                            {
                                switch (iGameCycle)
                                {
                                    case 1:
                                        GameCycle = "一期一周";
                                        break;
                                    case 2:
                                        GameCycle = "二期一周";
                                        break;
                                    case 3:
                                        GameCycle = "三期一周";
                                        break;
                                }
                            }
                            else if (GameType.Contains("4"))
                            {
                                switch (iGameCycle)
                                {
                                    case 1:
                                        GameCycle = "一期一周";
                                        break;
                                    case 2:
                                        GameCycle = "二期一周";
                                        break;
                                }
                            }
                            else
                            {
                                GameCycle = "一期一周";
                            }

                            //開始比對
                            if (GameKind == "四星") //&& (cbGameCycle.Text == "三期一周" || cbGameCycle.Text == "二期一周")
                            {

                                if (GameDirect == "复式")
                                {

                                }
                                else if (GameDirect == "单式")
                                {
                                    #region 驗證是否中奖

                                    bool isWin = false; //中了沒
                                    int periodtWin = 0; //第幾期中
                                    string[] temp = { "", "", "" }; //存放combobox的值

                                    for (int iii = jArrHistoryNumberForGod.Count() - 1; iii >= 0; iii--) //從歷史結果開始比
                                    {
                                        //reset
                                        isWin = false;
                                        periodtWin = 0;
                                        temp[0] = "";
                                        temp[1] = "";
                                        temp[2] = "";

                                        int NumberArrCount = numHistory.Count();

                                        for (int j = 0; j < iGameCycle; j++)
                                        {
                                            if (iii < 0) break;

                                            string strMatch = "";
                                            switch (GameKind)
                                            {
                                                case "五星":
                                                    strMatch = jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", "");
                                                    break;
                                                case "四星":
                                                    strMatch = jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", "").Substring(0, 4);
                                                    break;
                                                case "前三":
                                                    strMatch = jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", "").Substring(0, 3);
                                                    break;
                                                case "中三":
                                                    strMatch = jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", "").Substring(1, 3);
                                                    break;
                                                case "后三":
                                                    strMatch = jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", "").Substring(2, 3);
                                                    break;
                                                case "前二":
                                                    strMatch = jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", "").Substring(0, 2);
                                                    break;
                                                case "后二":
                                                    strMatch = jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", "").Substring(3, 2);
                                                    break;
                                            }
                                            if (isWin == false) //還沒中
                                            {
                                                if (numHistory[0].IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                }
                                                else //挂
                                                {
                                                    temp[j] = "  " + jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", " ") + " 挂";
                                                }
                                                sumBets++;
                                                periodtWin = j + 1;
                                            }
                                            else //前面已中奖
                                            {
                                                temp[j] = "  " + jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", " ") + " 停";
                                                //cycle_2++;
                                            }
                                            iii--;
                                        }

                                        cycle_2++;
                                        iii++;

                                        ComboBox cb_1 = new ComboBox();
                                        for (int k = 0; k < 3; k++)
                                        {
                                            if (temp[k] != "")
                                                cb_1.Items.Add(temp[k]);
                                        }


                                        //lbl_3 = new Label();
                                        if (isWin == true)
                                        {
                                            sumWin++;
                                        }
                                    }
                                    #endregion

                                    #region 計算
                                    string CurrentBetsCycle = (sumBets).ToString(); 
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(CurrentBetsCycle));
                                    string insertInfo = GameKind + "," + GameDirect + "," + GamePlanName + ", " + GameCycle + "," + GameType;
                                    InsertIntoGod(insertInfo, WinOpp.ToString("0.00"));
                                    #endregion
                                }
                            }
                            else if (GameKind == "五星") //&& (cbGameCycle.Text == "三期一周" || cbGameCycle.Text == "二期一周")
                            {

                                if (GameDirect == "复式")
                                {

                                }
                                else if (GameDirect == "单式")
                                {
                                    #region 驗證是否中奖

                                    bool isWin = false; //中了沒
                                    int periodtWin = 0; //第幾期中
                                    string[] temp = { "", "", "" }; //存放combobox的值

                                    for (int iii = jArrHistoryNumberForGod.Count() - 1; iii >= 0; iii--) //從歷史結果開始比
                                    {
                                        //reset
                                        isWin = false;
                                        periodtWin = 0;
                                        temp[0] = "";
                                        temp[1] = "";
                                        temp[2] = "";

                                        int NumberArrCount = numHistory.Count();

                                        for (int j = 0; j < iGameCycle; j++)
                                        {
                                            if (iii < 0) break;

                                            string strMatch = "";
                                            switch (GameKind)
                                            {
                                                case "五星":
                                                    strMatch = jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", "");
                                                    break;
                                                case "四星":
                                                    strMatch = jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", "").Substring(0, 4);
                                                    break;
                                                case "前三":
                                                    strMatch = jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", "").Substring(0, 3);
                                                    break;
                                                case "中三":
                                                    strMatch = jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", "").Substring(1, 3);
                                                    break;
                                                case "后三":
                                                    strMatch = jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", "").Substring(2, 3);
                                                    break;
                                                case "前二":
                                                    strMatch = jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", "").Substring(0, 2);
                                                    break;
                                                case "后二":
                                                    strMatch = jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", "").Substring(3, 2);
                                                    break;
                                            }
                                            if (isWin == false) //還沒中
                                            {
                                                ///////////////cycle_2 - 1
                                                if (numHistory[0].IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                }
                                                else //挂
                                                {
                                                    temp[j] = "  " + jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", " ") + " 挂";
                                                }
                                                sumBets++;
                                                periodtWin = j + 1;
                                            }
                                            else //前面已中奖
                                            {
                                                temp[j] = "  " + jArrHistoryNumberForGod[iii]["Number"].ToString().Replace(",", " ") + " 停";
                                                //cycle_2++;
                                            }
                                            iii--;
                                        }

                                        cycle_2++;
                                        iii++;

                                        ComboBox cb_1 = new ComboBox();
                                        for (int k = 0; k < 3; k++)
                                        {
                                            if (temp[k] != "")
                                                cb_1.Items.Add(temp[k]);
                                        }


                                        //lbl_3 = new Label();
                                        if (isWin == true)
                                        {
                                            sumWin++;
                                        }
                                    }
                                    #endregion

                                    #region 計算
                                    string CurrentBetsCycle = (sumBets).ToString(); 
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(CurrentBetsCycle));

                                    string insertInfo = GameKind + "," + GameDirect + "," + GamePlanName + ", " + GameCycle + "," + GameType;
                                    InsertIntoGod(insertInfo, WinOpp.ToString("0.00"));
                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
