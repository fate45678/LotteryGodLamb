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
        public static string globalGetCurrentPeriod;

        public class NextPeriod
        {
            public string CloseTime { get; set; }
            public string SerialNumber { get; set; }
        }
        public frmGameMain()
        {
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
            lblGame1_8.Click += new System.EventHandler(btnGame_Click);
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
            lblGame3_8.Click += new System.EventHandler(btnGame_Click);
            lblGame3_9.Click += new System.EventHandler(btnGame_Click);
            
            timer_ShowMessage.Enabled = true;
            timer_GetGameInfo.Enabled = true;
        }

        private void frmGameMain_Load(object sender, EventArgs e)
        {
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
                f_Trend.Show();
                return;
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
                    lblGame1_1.ForeColor= Color.White;
                    lblGame1_1.Refresh();
                    HD_GameSelect.Text = ((Label)(sender)).Text;
                    break;
                case "天津时时彩":
                    ResetAllGame(); //重設彩票
                    lblGame1_7.BackColor = Color.Black;
                    lblGame1_7.ForeColor = Color.White;
                    lblGame1_7.Refresh();
                    HD_GameSelect.Text = ((Label)(sender)).Text;
                    break;
                default:
                    MessageBox.Show(((Label)(sender)).Text + " 尚未開放");
                    break;
            }
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
            lblGame1_8.BackColor = Color.Transparent; lblGame1_8.ForeColor = Color.Black; lblGame1_8.Refresh();
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
            lblGame3_8.BackColor = Color.Transparent; lblGame3_8.ForeColor = Color.Black; lblGame3_8.Refresh();
            lblGame3_9.BackColor = Color.Transparent; lblGame3_9.ForeColor = Color.Black; lblGame3_9.Refresh();
        }

        //private void DisposeForm(string Menu)
        //{
        //    switch (HD_MenuSelect.Text)
        //    {
        //        case "神灯周期计划":
        //            frm_PlanCycle f_PlanCycle = new frm_PlanCycle();
        //            f_PlanCycle.Hide();
        //            break;
        //        case "代理计划":
        //            frm_PlanAgent f_PlanAgent = new frm_PlanAgent();
        //            f_PlanAgent.Hide();
        //            break;
        //        case "计划上传":
        //            frm_PlanUpload f_PlanUpload = new frm_PlanUpload();
        //            f_PlanUpload.Hide();
        //            break;
        //        case "缩水工具":
        //            frm_Shrink f_Shrink = new frm_Shrink();
        //            f_Shrink.Hide();
        //            break;
        //        case "走势图":
        //            frm_Chart f_Chart = new frm_Chart();
        //            f_Chart.Hide();
        //            break;
        //    }
        //}

        //取得下一期時間

        private void useHttpWebRequest_GetNextPeriod()
        {
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

            #region test in DL
            ////var temp = "{\"SerialNumber]\":null,\"CloseTime\":null}";
            //var temp = "{\"CloseTime\":\"2018/04/12 12:09:30\",\"SerialNumber\":\"20180411062\"}";
            //string json = JsonConvert.SerializeObject(temp);
            //NextPeriod NextPeriod = JsonConvert.DeserializeObject<NextPeriod>(temp);

            //if (NextPeriod.SerialNumber == null)
            //{
            //    lblNextPeriod.Text = "00000000000";
            //    lblNextPeriodTime.Text = "-- : -- : --";
            //}
            //else
            //{
            //    lblNextPeriod.Text = NextPeriod.SerialNumber;
            //    DateTime dt1 = Convert.ToDateTime(NextPeriod.CloseTime);
            //    DateTime dt2 = DateTime.Now;
            //    TimeSpan ts = new TimeSpan(dt1.Ticks - dt2.Ticks);
            //    string hh = ts.Hours.ToString("00");
            //    string mm = ts.Minutes.ToString("00");
            //    string ss = ts.Seconds.ToString("00");
            //    if (ss.IndexOf("-") > -1)
            //        ss = "00";
            //    lblNextPeriodTime.Text = hh + " : " + mm + " : " + ss;
            //}
            #endregion
        }

        //取得歷史開獎
        private void useHttpWebRequest_GetHistory()
        {
            DateTime dt = DateTime.Now.AddDays(-2); //最早取前2天
            string dt1 = dt.Year + dt.Month.ToString("00") + dt.Day.ToString("00");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://hyqa.azurewebsites.net/DrawHistory/GetBySerialNumber?name=" + Game_Function.GameNameToCode(HD_GameSelect.Text) + "&startSerialNumber=" + dt1 + "&endSerialNumber=" + dt1 + "120");
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

            #region test in DL
            //var temp = "[{\"Number\":\"5,8,3,8,0\",\"Issue\":\"20180411023\"},{\"Number\":\"7,0,1,4,2\",\"Issue\":\"20180411022\"},{\"Number\":\"7,1,8,0,8\",\"Issue\":\"20180411021\"},{\"Number\":\"0,7,2,9,4\",\"Issue\":\"20180411020\"},{\"Number\":\"9,9,3,3,0\",\"Issue\":\"20180411019\"},{\"Number\":\"8,0,5,8,0\",\"Issue\":\"20180411018\"},{\"Number\":\"1,0,1,2,6\",\"Issue\":\"20180411017\"},{\"Number\":\"8,4,5,8,1\",\"Issue\":\"20180411016\"},{\"Number\":\"3,2,4,5,0\",\"Issue\":\"20180411015\"},{\"Number\":\"8,0,3,1,2\",\"Issue\":\"20180411014\"},{\"Number\":\"1,4,5,4,8\",\"Issue\":\"20180411013\"},{\"Number\":\"7,6,6,1,3\",\"Issue\":\"20180411012\"},{\"Number\":\"3,3,3,2,8\",\"Issue\":\"20180411011\"},{\"Number\":\"4,7,1,7,8\",\"Issue\":\"20180411010\"},{\"Number\":\"2,1,8,8,6\",\"Issue\":\"20180411009\"},{\"Number\":\"9,4,3,6,7\",\"Issue\":\"20180411008\"},{\"Number\":\"8,0,1,0,1\",\"Issue\":\"20180411007\"},{\"Number\":\"3,2,9,5,0\",\"Issue\":\"20180411006\"},{\"Number\":\"0,1,2,7,5\",\"Issue\":\"20180411005\"},{\"Number\":\"6,3,3,7,2\",\"Issue\":\"20180411004\"},{\"Number\":\"8,5,9,0,4\",\"Issue\":\"20180411003\"},{\"Number\":\"1,9,5,4,5\",\"Issue\":\"20180411002\"},{\"Number\":\"0,3,8,3,2\",\"Issue\":\"20180411001\"},{\"Number\":\"2,5,0,9,8\",\"Issue\":\"20180410120\"},{\"Number\":\"4,2,8,7,8\",\"Issue\":\"20180410119\"},{\"Number\":\"7,7,1,4,3\",\"Issue\":\"20180410118\"},{\"Number\":\"4,0,4,7,6\",\"Issue\":\"20180410117\"},{\"Number\":\"8,1,3,0,9\",\"Issue\":\"20180410116\"},{\"Number\":\"2,6,0,7,3\",\"Issue\":\"20180410115\"},{\"Number\":\"3,3,1,7,9\",\"Issue\":\"20180410114\"},{\"Number\":\"9,7,7,4,4\",\"Issue\":\"20180410113\"},{\"Number\":\"1,4,3,5,7\",\"Issue\":\"20180410112\"},{\"Number\":\"2,3,3,1,7\",\"Issue\":\"20180410111\"},{\"Number\":\"7,6,8,2,8\",\"Issue\":\"20180410110\"},{\"Number\":\"9,0,7,0,8\",\"Issue\":\"20180410109\"},{\"Number\":\"8,7,2,0,4\",\"Issue\":\"20180410108\"},{\"Number\":\"8,7,8,2,8\",\"Issue\":\"20180410107\"},{\"Number\":\"5,3,4,1,9\",\"Issue\":\"20180410106\"},{\"Number\":\"3,4,1,6,6\",\"Issue\":\"20180410105\"},{\"Number\":\"9,5,4,7,9\",\"Issue\":\"20180410104\"},{\"Number\":\"5,0,2,7,5\",\"Issue\":\"20180410103\"},{\"Number\":\"6,2,0,8,5\",\"Issue\":\"20180410102\"},{\"Number\":\"8,6,7,9,0\",\"Issue\":\"20180410101\"},{\"Number\":\"8,4,7,2,8\",\"Issue\":\"20180410100\"},{\"Number\":\"9,8,3,3,4\",\"Issue\":\"20180410099\"},{\"Number\":\"4,7,9,7,9\",\"Issue\":\"20180410098\"},{\"Number\":\"1,4,3,4,0\",\"Issue\":\"20180410097\"},{\"Number\":\"5,9,5,0,2\",\"Issue\":\"20180410096\"},{\"Number\":\"5,6,4,9,7\",\"Issue\":\"20180410095\"},{\"Number\":\"7,3,1,8,3\",\"Issue\":\"20180410094\"},{\"Number\":\"1,5,0,9,9\",\"Issue\":\"20180410093\"},{\"Number\":\"0,2,5,8,5\",\"Issue\":\"20180410092\"},{\"Number\":\"6,8,1,6,5\",\"Issue\":\"20180410091\"},{\"Number\":\"9,4,3,2,1\",\"Issue\":\"20180410090\"},{\"Number\":\"2,5,8,8,3\",\"Issue\":\"20180410089\"},{\"Number\":\"2,8,3,0,6\",\"Issue\":\"20180410088\"},{\"Number\":\"1,0,8,2,2\",\"Issue\":\"20180410087\"},{\"Number\":\"6,8,3,2,2\",\"Issue\":\"20180410086\"},{\"Number\":\"1,2,5,0,7\",\"Issue\":\"20180410085\"},{\"Number\":\"9,7,8,5,6\",\"Issue\":\"20180410084\"},{\"Number\":\"6,8,4,6,2\",\"Issue\":\"20180410083\"},{\"Number\":\"3,3,0,3,4\",\"Issue\":\"20180410082\"},{\"Number\":\"9,3,7,2,9\",\"Issue\":\"20180410081\"},{\"Number\":\"0,1,6,7,1\",\"Issue\":\"20180410080\"},{\"Number\":\"3,4,8,5,3\",\"Issue\":\"20180410079\"},{\"Number\":\"7,0,4,3,7\",\"Issue\":\"20180410078\"},{\"Number\":\"9,3,6,8,1\",\"Issue\":\"20180410077\"},{\"Number\":\"1,3,4,9,4\",\"Issue\":\"20180410076\"},{\"Number\":\"5,1,4,9,8\",\"Issue\":\"20180410075\"},{\"Number\":\"4,9,1,9,9\",\"Issue\":\"20180410074\"},{\"Number\":\"2,1,9,0,7\",\"Issue\":\"20180410073\"},{\"Number\":\"2,4,7,9,4\",\"Issue\":\"20180410072\"},{\"Number\":\"3,8,3,0,1\",\"Issue\":\"20180410071\"},{\"Number\":\"5,6,5,6,3\",\"Issue\":\"20180410070\"},{\"Number\":\"8,3,6,1,9\",\"Issue\":\"20180410069\"},{\"Number\":\"7,9,4,3,2\",\"Issue\":\"20180410068\"},{\"Number\":\"7,8,3,0,7\",\"Issue\":\"20180410067\"},{\"Number\":\"0,5,6,0,5\",\"Issue\":\"20180410066\"},{\"Number\":\"8,2,2,5,8\",\"Issue\":\"20180410065\"},{\"Number\":\"4,6,2,7,1\",\"Issue\":\"20180410064\"},{\"Number\":\"3,2,6,3,3\",\"Issue\":\"20180410063\"},{\"Number\":\"6,6,1,2,8\",\"Issue\":\"20180410062\"},{\"Number\":\"2,6,6,1,1\",\"Issue\":\"20180410061\"},{\"Number\":\"6,4,4,8,2\",\"Issue\":\"20180410060\"},{\"Number\":\"8,2,9,9,4\",\"Issue\":\"20180410059\"},{\"Number\":\"6,1,8,1,2\",\"Issue\":\"20180410058\"},{\"Number\":\"9,7,3,0,7\",\"Issue\":\"20180410057\"},{\"Number\":\"4,4,8,2,1\",\"Issue\":\"20180410056\"},{\"Number\":\"0,6,3,2,8\",\"Issue\":\"20180410055\"},{\"Number\":\"6,6,7,0,0\",\"Issue\":\"20180410054\"},{\"Number\":\"4,5,0,3,7\",\"Issue\":\"20180410053\"},{\"Number\":\"5,0,5,3,1\",\"Issue\":\"20180410052\"},{\"Number\":\"7,4,2,0,6\",\"Issue\":\"20180410051\"},{\"Number\":\"2,6,4,3,5\",\"Issue\":\"20180410050\"},{\"Number\":\"9,5,5,9,1\",\"Issue\":\"20180410049\"},{\"Number\":\"4,8,5,7,7\",\"Issue\":\"20180410048\"},{\"Number\":\"3,7,0,6,0\",\"Issue\":\"20180410047\"},{\"Number\":\"0,5,1,0,5\",\"Issue\":\"20180410046\"},{\"Number\":\"9,1,2,6,7\",\"Issue\":\"20180410045\"},{\"Number\":\"1,6,0,5,7\",\"Issue\":\"20180410044\"},{\"Number\":\"4,7,8,6,8\",\"Issue\":\"20180410043\"},{\"Number\":\"3,5,6,4,8\",\"Issue\":\"20180410042\"},{\"Number\":\"8,4,1,8,0\",\"Issue\":\"20180410041\"},{\"Number\":\"9,8,8,7,7\",\"Issue\":\"20180410040\"},{\"Number\":\"1,4,1,6,8\",\"Issue\":\"20180410039\"},{\"Number\":\"8,9,2,4,6\",\"Issue\":\"20180410038\"},{\"Number\":\"8,4,6,0,7\",\"Issue\":\"20180410037\"},{\"Number\":\"6,1,4,4,4\",\"Issue\":\"20180410036\"},{\"Number\":\"2,1,4,6,8\",\"Issue\":\"20180410035\"},{\"Number\":\"2,9,0,6,5\",\"Issue\":\"20180410034\"},{\"Number\":\"3,0,3,5,2\",\"Issue\":\"20180410033\"},{\"Number\":\"8,8,8,2,0\",\"Issue\":\"20180410032\"},{\"Number\":\"2,5,7,0,5\",\"Issue\":\"20180410031\"},{\"Number\":\"6,3,9,0,9\",\"Issue\":\"20180410030\"},{\"Number\":\"9,8,2,8,1\",\"Issue\":\"20180410029\"},{\"Number\":\"7,1,9,5,9\",\"Issue\":\"20180410028\"},{\"Number\":\"4,2,9,9,2\",\"Issue\":\"20180410027\"},{\"Number\":\"6,3,8,6,0\",\"Issue\":\"20180410026\"},{\"Number\":\"6,9,5,4,0\",\"Issue\":\"20180410025\"},{\"Number\":\"5,6,1,2,3\",\"Issue\":\"20180410024\"},{\"Number\":\"9,9,0,8,7\",\"Issue\":\"20180410023\"},{\"Number\":\"1,8,2,0,6\",\"Issue\":\"20180410022\"},{\"Number\":\"9,5,6,8,4\",\"Issue\":\"20180410021\"},{\"Number\":\"3,2,8,5,4\",\"Issue\":\"20180410020\"},{\"Number\":\"6,8,8,6,3\",\"Issue\":\"20180410019\"},{\"Number\":\"5,1,7,1,3\",\"Issue\":\"20180410018\"},{\"Number\":\"4,9,7,3,5\",\"Issue\":\"20180410017\"},{\"Number\":\"5,3,8,0,4\",\"Issue\":\"20180410016\"},{\"Number\":\"2,9,9,1,0\",\"Issue\":\"20180410015\"},{\"Number\":\"6,3,8,4,2\",\"Issue\":\"20180410014\"},{\"Number\":\"5,4,1,3,6\",\"Issue\":\"20180410013\"},{\"Number\":\"2,5,9,0,3\",\"Issue\":\"20180410012\"},{\"Number\":\"3,7,5,3,8\",\"Issue\":\"20180410011\"},{\"Number\":\"1,3,7,2,5\",\"Issue\":\"20180410010\"},{\"Number\":\"8,0,0,3,9\",\"Issue\":\"20180410009\"},{\"Number\":\"0,8,0,8,7\",\"Issue\":\"20180410008\"},{\"Number\":\"5,8,3,1,5\",\"Issue\":\"20180410007\"},{\"Number\":\"4,8,1,4,4\",\"Issue\":\"20180410006\"},{\"Number\":\"0,6,5,7,6\",\"Issue\":\"20180410005\"},{\"Number\":\"2,0,6,9,8\",\"Issue\":\"20180410004\"},{\"Number\":\"8,7,3,8,6\",\"Issue\":\"20180410003\"},{\"Number\":\"4,7,1,7,5\",\"Issue\":\"20180410002\"},{\"Number\":\"9,7,1,4,3\",\"Issue\":\"20180410001\"},{\"Number\":\"9,2,9,5,6\",\"Issue\":\"20180409120\"},{\"Number\":\"0,4,0,1,0\",\"Issue\":\"20180409119\"},{\"Number\":\"8,1,5,9,3\",\"Issue\":\"20180409118\"},{\"Number\":\"2,0,2,9,6\",\"Issue\":\"20180409117\"},{\"Number\":\"0,6,9,8,0\",\"Issue\":\"20180409116\"},{\"Number\":\"1,8,5,7,1\",\"Issue\":\"20180409115\"},{\"Number\":\"5,7,5,5,3\",\"Issue\":\"20180409114\"},{\"Number\":\"5,9,5,2,4\",\"Issue\":\"20180409113\"},{\"Number\":\"8,0,0,0,0\",\"Issue\":\"20180409112\"},{\"Number\":\"2,6,1,6,8\",\"Issue\":\"20180409111\"},{\"Number\":\"9,2,6,0,5\",\"Issue\":\"20180409110\"},{\"Number\":\"5,9,2,2,7\",\"Issue\":\"20180409109\"},{\"Number\":\"6,1,2,7,7\",\"Issue\":\"20180409108\"},{\"Number\":\"4,0,9,2,7\",\"Issue\":\"20180409107\"},{\"Number\":\"4,0,5,5,2\",\"Issue\":\"20180409106\"},{\"Number\":\"1,9,1,8,0\",\"Issue\":\"20180409105\"},{\"Number\":\"6,3,4,8,3\",\"Issue\":\"20180409104\"},{\"Number\":\"1,8,7,8,4\",\"Issue\":\"20180409103\"},{\"Number\":\"6,3,8,7,8\",\"Issue\":\"20180409102\"},{\"Number\":\"2,9,7,9,1\",\"Issue\":\"20180409101\"},{\"Number\":\"4,4,3,1,9\",\"Issue\":\"20180409100\"},{\"Number\":\"2,6,6,4,7\",\"Issue\":\"20180409099\"},{\"Number\":\"1,9,3,0,6\",\"Issue\":\"20180409098\"},{\"Number\":\"5,6,1,0,8\",\"Issue\":\"20180409097\"},{\"Number\":\"4,8,2,7,7\",\"Issue\":\"20180409096\"},{\"Number\":\"5,2,8,9,1\",\"Issue\":\"20180409095\"},{\"Number\":\"5,7,9,0,5\",\"Issue\":\"20180409094\"},{\"Number\":\"6,1,5,7,5\",\"Issue\":\"20180409093\"},{\"Number\":\"7,5,3,9,4\",\"Issue\":\"20180409092\"},{\"Number\":\"4,2,2,3,0\",\"Issue\":\"20180409091\"},{\"Number\":\"2,8,1,6,4\",\"Issue\":\"20180409090\"},{\"Number\":\"4,0,5,4,4\",\"Issue\":\"20180409089\"},{\"Number\":\"7,1,0,1,2\",\"Issue\":\"20180409088\"},{\"Number\":\"3,8,7,2,5\",\"Issue\":\"20180409087\"},{\"Number\":\"0,2,3,7,0\",\"Issue\":\"20180409086\"},{\"Number\":\"8,4,2,6,6\",\"Issue\":\"20180409085\"},{\"Number\":\"8,4,9,1,9\",\"Issue\":\"20180409084\"},{\"Number\":\"6,8,9,6,6\",\"Issue\":\"20180409083\"},{\"Number\":\"5,9,8,4,5\",\"Issue\":\"20180409082\"},{\"Number\":\"4,5,1,9,9\",\"Issue\":\"20180409081\"},{\"Number\":\"6,4,9,9,4\",\"Issue\":\"20180409080\"},{\"Number\":\"3,6,2,6,7\",\"Issue\":\"20180409079\"},{\"Number\":\"3,8,3,4,0\",\"Issue\":\"20180409078\"},{\"Number\":\"4,7,7,2,7\",\"Issue\":\"20180409077\"},{\"Number\":\"3,7,6,8,1\",\"Issue\":\"20180409076\"},{\"Number\":\"1,1,8,6,5\",\"Issue\":\"20180409075\"},{\"Number\":\"6,2,9,5,9\",\"Issue\":\"20180409074\"},{\"Number\":\"5,3,6,8,8\",\"Issue\":\"20180409073\"},{\"Number\":\"0,1,1,0,7\",\"Issue\":\"20180409072\"},{\"Number\":\"6,4,0,2,4\",\"Issue\":\"20180409071\"},{\"Number\":\"2,6,4,2,2\",\"Issue\":\"20180409070\"},{\"Number\":\"2,2,3,3,6\",\"Issue\":\"20180409069\"},{\"Number\":\"9,8,5,0,4\",\"Issue\":\"20180409068\"},{\"Number\":\"8,5,7,0,5\",\"Issue\":\"20180409067\"},{\"Number\":\"7,9,3,5,3\",\"Issue\":\"20180409066\"},{\"Number\":\"3,5,6,5,1\",\"Issue\":\"20180409065\"},{\"Number\":\"0,1,9,4,0\",\"Issue\":\"20180409064\"},{\"Number\":\"3,8,4,1,1\",\"Issue\":\"20180409063\"},{\"Number\":\"5,2,6,8,4\",\"Issue\":\"20180409062\"},{\"Number\":\"3,1,1,5,9\",\"Issue\":\"20180409061\"},{\"Number\":\"4,3,7,1,3\",\"Issue\":\"20180409060\"},{\"Number\":\"1,1,1,4,0\",\"Issue\":\"20180409059\"},{\"Number\":\"6,9,0,6,9\",\"Issue\":\"20180409058\"},{\"Number\":\"3,0,2,9,2\",\"Issue\":\"20180409057\"},{\"Number\":\"2,5,2,0,7\",\"Issue\":\"20180409056\"},{\"Number\":\"9,3,2,0,9\",\"Issue\":\"20180409055\"},{\"Number\":\"7,3,5,5,9\",\"Issue\":\"20180409054\"},{\"Number\":\"8,1,3,8,4\",\"Issue\":\"20180409053\"},{\"Number\":\"8,4,3,7,1\",\"Issue\":\"20180409052\"},{\"Number\":\"3,8,1,9,8\",\"Issue\":\"20180409051\"},{\"Number\":\"9,3,5,6,4\",\"Issue\":\"20180409050\"},{\"Number\":\"0,2,9,2,3\",\"Issue\":\"20180409049\"},{\"Number\":\"4,9,5,8,4\",\"Issue\":\"20180409048\"},{\"Number\":\"6,8,4,1,6\",\"Issue\":\"20180409047\"},{\"Number\":\"6,0,5,6,2\",\"Issue\":\"20180409046\"},{\"Number\":\"5,4,8,5,1\",\"Issue\":\"20180409045\"},{\"Number\":\"7,8,0,1,3\",\"Issue\":\"20180409044\"},{\"Number\":\"2,2,5,0,0\",\"Issue\":\"20180409043\"},{\"Number\":\"1,9,5,0,7\",\"Issue\":\"20180409042\"},{\"Number\":\"5,9,1,0,3\",\"Issue\":\"20180409041\"},{\"Number\":\"0,7,1,2,0\",\"Issue\":\"20180409040\"},{\"Number\":\"0,1,5,6,7\",\"Issue\":\"20180409039\"},{\"Number\":\"8,8,9,1,5\",\"Issue\":\"20180409038\"},{\"Number\":\"3,7,5,4,2\",\"Issue\":\"20180409037\"},{\"Number\":\"8,7,6,1,0\",\"Issue\":\"20180409036\"},{\"Number\":\"3,7,5,4,6\",\"Issue\":\"20180409035\"},{\"Number\":\"8,9,9,8,1\",\"Issue\":\"20180409034\"},{\"Number\":\"4,0,9,9,3\",\"Issue\":\"20180409033\"},{\"Number\":\"3,8,0,2,7\",\"Issue\":\"20180409032\"},{\"Number\":\"0,9,8,2,3\",\"Issue\":\"20180409031\"},{\"Number\":\"0,3,0,6,5\",\"Issue\":\"20180409030\"},{\"Number\":\"2,4,6,9,7\",\"Issue\":\"20180409029\"},{\"Number\":\"8,9,8,8,5\",\"Issue\":\"20180409028\"},{\"Number\":\"5,5,9,2,0\",\"Issue\":\"20180409027\"},{\"Number\":\"3,5,5,3,6\",\"Issue\":\"20180409026\"},{\"Number\":\"1,0,7,0,1\",\"Issue\":\"20180409025\"},{\"Number\":\"6,0,9,9,4\",\"Issue\":\"20180409024\"},{\"Number\":\"9,8,6,6,2\",\"Issue\":\"20180409023\"},{\"Number\":\"8,4,4,8,6\",\"Issue\":\"20180409022\"},{\"Number\":\"8,5,7,3,0\",\"Issue\":\"20180409021\"},{\"Number\":\"2,0,7,3,8\",\"Issue\":\"20180409020\"},{\"Number\":\"0,2,7,2,6\",\"Issue\":\"20180409019\"},{\"Number\":\"7,6,9,9,7\",\"Issue\":\"20180409018\"},{\"Number\":\"3,0,4,4,0\",\"Issue\":\"20180409017\"},{\"Number\":\"2,0,8,1,6\",\"Issue\":\"20180409016\"},{\"Number\":\"6,0,6,4,1\",\"Issue\":\"20180409015\"},{\"Number\":\"8,4,1,4,5\",\"Issue\":\"20180409014\"},{\"Number\":\"5,8,0,7,3\",\"Issue\":\"20180409013\"},{\"Number\":\"2,6,6,9,9\",\"Issue\":\"20180409012\"},{\"Number\":\"0,9,9,7,0\",\"Issue\":\"20180409011\"},{\"Number\":\"1,3,5,3,5\",\"Issue\":\"20180409010\"},{\"Number\":\"2,4,0,5,7\",\"Issue\":\"20180409009\"},{\"Number\":\"3,2,2,7,4\",\"Issue\":\"20180409008\"},{\"Number\":\"6,8,4,6,4\",\"Issue\":\"20180409007\"},{\"Number\":\"0,9,7,9,2\",\"Issue\":\"20180409006\"},{\"Number\":\"7,3,0,6,1\",\"Issue\":\"20180409005\"},{\"Number\":\"2,1,3,9,4\",\"Issue\":\"20180409004\"},{\"Number\":\"0,2,2,9,8\",\"Issue\":\"20180409003\"},{\"Number\":\"0,8,2,8,7\",\"Issue\":\"20180409002\"},{\"Number\":\"1,5,5,1,5\",\"Issue\":\"20180409001\"}]";
            //JArray ja = (JArray)JsonConvert.DeserializeObject(temp);
            //jArr = ja;
            ////處理最近開獎號碼
            //string lastWinPeriod = ja[0]["Issue"].ToString(); //最近開獎的期數

            //if ((lastWinPeriod.Substring(8, 3) == "120" && lblNextPeriod.Text.Substring(8, 3) == "002")
            //    || (lastWinPeriod.Substring(8, 3) == "119" && lblNextPeriod.Text.Substring(8, 3) == "001")) //倒數結束後到完成開獎的空檔 針對跨日( 0404120期>0405002期 或 0404119期>0405001期 )
            //{
            //    if (lastWinPeriod.Substring(8, 3) == "120")
            //        lblCurrentPeriod.Text = lblNextPeriod.Text.Substring(0, 8) + "" + "001"; //當期
            //    else
            //        lblCurrentPeriod.Text = lastWinPeriod.Substring(0, 8) + "" + "120"; //當期
            //    lblNumber1.Text = lblNumber2.Text = lblNumber3.Text = lblNumber4.Text = lblNumber5.Text = "?";
            //    strHistoryNumberOpen = "?";
            //}
            //else if (Int16.Parse(lblNextPeriod.Text.Substring(8, 3)) - Int16.Parse(lastWinPeriod.Substring(8, 3)) == 2)//倒數結束後到完成開獎的空檔 針對同一日( 0404100期>0404098期 )
            //{
            //    lblCurrentPeriod.Text = (Convert.ToInt64(lastWinPeriod) + 1).ToString(); //當期
            //    lblNumber1.Text = lblNumber2.Text = lblNumber3.Text = lblNumber4.Text = lblNumber5.Text = "?";
            //    strHistoryNumberOpen = "?";
            //}
            //else
            //{
            //    lblCurrentPeriod.Text = lastWinPeriod; //當期
            //    lblNumber1.Text = ja[0]["Number"].ToString().Substring(0, 1);
            //    lblNumber2.Text = ja[0]["Number"].ToString().Substring(2, 1);
            //    lblNumber3.Text = ja[0]["Number"].ToString().Substring(4, 1);
            //    lblNumber4.Text = ja[0]["Number"].ToString().Substring(6, 1);
            //    lblNumber5.Text = ja[0]["Number"].ToString().Substring(8, 1);
            //    strHistoryNumberOpen = ja[0]["Number"].ToString().Substring(0, 1);
            //}
            ////處理歷史開獎
            ////strHistory = "";
            ////strHistoryCount = ja.Count.ToString();
            ////for (int i = 0; i < ja.Count; i++)
            ////{
            ////    if (i == 120) break; //寫120筆就好
            ////    strHistory += ja[i]["Issue"].ToString() + "  " + ja[i]["Number"].ToString().Replace(",", " ") + "\r\n";
            ////}
            #region 測試:撈到新資料
            //DateTime dtTest = DateTime.Now;
            //if (dtTest.Hour == 16 && dtTest.Minute == 24 && dtTest.Second == 40)
            //{
            //    temp = "[{\"Number\":\"9,0,0,4,7\",\"Issue\":\"20180406062\"},{\"Number\":\"2,4,0,7,9\",\"Issue\":\"20180406061\"},{\"Number\":\"8,5,1,8,3\",\"Issue\":\"20180406060\"}]";
            //    ja = (JArray)JsonConvert.DeserializeObject(temp);

            //    //處理最近開獎號碼
            //    lastWinPeriod = ja[0]["Issue"].ToString(); //最近開獎的期數

            //    if ((lastWinPeriod.Substring(8, 3) == "120" && lblNextPeriod.Text.Substring(8, 3) == "002")
            //        || (lastWinPeriod.Substring(8, 3) == "119" && lblNextPeriod.Text.Substring(8, 3) == "001")) //倒數結束後到完成開獎的空檔 針對跨日( 0404120期>0405002期 或 0404119期>0405001期 )
            //    {
            //        if (lastWinPeriod.Substring(8, 3) == "120")
            //            lblCurrentPeriod.Text = lblNextPeriod.Text.Substring(0, 8) + "" + "001"; //當期
            //        else
            //            lblCurrentPeriod.Text = lastWinPeriod.Substring(0, 8) + "" + "120"; //當期
            //        lblNumber1.Text = lblNumber2.Text = lblNumber3.Text = lblNumber4.Text = lblNumber5.Text = "?";
            //    }
            //    else if (Int16.Parse(lblNextPeriod.Text.Substring(8, 3)) - Int16.Parse(lastWinPeriod.Substring(8, 3)) == 2)//倒數結束後到完成開獎的空檔 針對同一日( 0404100期>0404098期 )
            //    {
            //        lblCurrentPeriod.Text = (Convert.ToInt64(lastWinPeriod) + 1).ToString(); //當期
            //        lblNumber1.Text = lblNumber2.Text = lblNumber3.Text = lblNumber4.Text = lblNumber5.Text = "?";
            //    }
            //    else
            //    {
            //        lblCurrentPeriod.Text = lastWinPeriod; //當期
            //        lblNumber1.Text = ja[0]["Number"].ToString().Substring(0, 1);
            //        lblNumber2.Text = ja[0]["Number"].ToString().Substring(2, 1);
            //        lblNumber3.Text = ja[0]["Number"].ToString().Substring(4, 1);
            //        lblNumber4.Text = ja[0]["Number"].ToString().Substring(6, 1);
            //        lblNumber5.Text = ja[0]["Number"].ToString().Substring(8, 1);
            //    }
            //    if ((rtxtHistory.Text.Substring(0, 11) != lblCurrentPeriod.Text) && (lblNumber1.Text != "?")) //有新資料了
            //    {
            //        string strHistory = rtxtHistory.Text;
            //        rtxtHistory.Text = "";
            //        rtxtHistory.AppendText(ja[0]["Issue"].ToString() + "  " + ja[0]["Number"].ToString().Replace(",", " ") + "\r\n");
            //        rtxtHistory.AppendText(strHistory);
            //    }
            //}
            #endregion
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
            }
        }
        
        private void timer_ShowMessage_Tick(object sender, EventArgs e)
        {
            //ShowMessage();
        }

        private void timer_GetGameInfo_Tick(object sender, EventArgs e)
        {
            useHttpWebRequest_GetNextPeriod(); //取得下一期時間       
            useHttpWebRequest_GetHistory(); //取得歷史開獎
        }

        private void lblMenuPlanUpload_Click(object sender, EventArgs e)
        {

        }

        private void lblMenuPlanAgent_Click(object sender, EventArgs e)
        {

        }
    }
}
