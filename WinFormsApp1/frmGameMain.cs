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
            lblGame1_4.Click += new System.EventHandler(btnGame_Click);
            lblGame1_5.Click += new System.EventHandler(btnGame_Click);
            lblGame1_6.Click += new System.EventHandler(btnGame_Click);
            lblGame1_7.Click += new System.EventHandler(btnGame_Click);            
            lblGame2_1.Click += new System.EventHandler(btnGame_Click);
            lblGame2_2.Click += new System.EventHandler(btnGame_Click);
            lblGame2_3.Click += new System.EventHandler(btnGame_Click);
            lblGame2_4.Click += new System.EventHandler(btnGame_Click);
            lblGame2_5.Click += new System.EventHandler(btnGame_Click);
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
                    break;
                case "天津时时彩":
                    ResetAllGame(); //重設彩票
                    lblGame1_7.BackColor = Color.Black;
                    lblGame1_7.ForeColor = Color.White;
                    lblGame1_7.Refresh();
                    HD_GameSelect.Text = ((Label)(sender)).Text;
                    frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                    break;
                case "腾讯官方彩":
                    ResetAllGame(); //重設彩票
                    lblGame1_2.BackColor = Color.Black;
                    lblGame1_2.ForeColor = Color.White;
                    lblGame1_2.Refresh();
                    HD_GameSelect.Text = ((Label)(sender)).Text;
                    frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                    break;
                case "腾讯奇趣彩":
                    ResetAllGame(); //重設彩票
                    lblGame1_3.BackColor = Color.Black;
                    lblGame1_3.ForeColor = Color.White;
                    lblGame1_3.Refresh();
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
            lblGame1_4.BackColor = Color.Transparent; lblGame1_4.ForeColor = Color.Black; lblGame1_4.Refresh();
            lblGame1_5.BackColor = Color.Transparent; lblGame1_5.ForeColor = Color.Black; lblGame1_5.Refresh();
            lblGame1_6.BackColor = Color.Transparent; lblGame1_6.ForeColor = Color.Black; lblGame1_6.Refresh();
            lblGame1_7.BackColor = Color.Transparent; lblGame1_7.ForeColor = Color.Black; lblGame1_7.Refresh();
            lblGame2_1.BackColor = Color.Transparent; lblGame2_1.ForeColor = Color.Black; lblGame2_1.Refresh();
            lblGame2_2.BackColor = Color.Transparent; lblGame2_2.ForeColor = Color.Black; lblGame2_2.Refresh();
            lblGame2_3.BackColor = Color.Transparent; lblGame2_3.ForeColor = Color.Black; lblGame2_3.Refresh();
            lblGame2_4.BackColor = Color.Transparent; lblGame2_4.ForeColor = Color.Black; lblGame2_4.Refresh();
            lblGame2_5.BackColor = Color.Transparent; lblGame2_5.ForeColor = Color.Black; lblGame2_5.Refresh();
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
            DateTime dt = DateTime.Now.AddDays(-2); //最早取前2天
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
    }
}
