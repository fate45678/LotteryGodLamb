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
        //代理人的帳號密碼
        public static string PlanProxyUser = "test01";
        public static string PlanProxyPassWord = "123456";

        public static bool openMessage = false;
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

        //是否為第一次開啟表單
        bool isFirstf_PlanCycle = true;
        bool isFirstf_PlanAgent = true;
        bool isFirstf_PlanUpload = true;
        bool isFirstf_Shrink = true;
        bool isFirstf_ShrinkPK10 = true;
        bool isFirstf_Chart = true;
        public class NextPeriod
        {
            public string CloseTime { get; set; }
            public string SerialNumber { get; set; }
        }

        //判斷是否為PK10的縮水
        bool isShrinkPk10_Open = false;

        public frmGameMain()
        {
            frm_VersionCheck frm_VersionCheck = new frm_VersionCheck();
            frm_VersionCheck.ShowDialog();

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
            lblGame2_6.Click += new System.EventHandler(btnGame_Click);
            lblGame3_1.Click += new System.EventHandler(btnGame_Click);
            lblGame3_2.Click += new System.EventHandler(btnGame_Click);
            lblGame3_3.Click += new System.EventHandler(btnGame_Click);
            lblGame3_4.Click += new System.EventHandler(btnGame_Click);
            lblGame3_5.Click += new System.EventHandler(btnGame_Click);
            lblGame3_6.Click += new System.EventHandler(btnGame_Click);


            timer_ShowMessage.Enabled = true;
            timer_GetGameInfo.Enabled = true;

            string ProVersion = this.GetType().Assembly.GetName().Version.ToString();
            this.Text = this.Text + "【" + PlanProxyUser + "】" + ProVersion + " 提示: 如果沒有更新到最新期號，請按刷新"; 
        }

        private void frmGameMain_Load(object sender, EventArgs e)
        {

            frm_PlanCycle f_PlanCycle = new frm_PlanCycle();
            f_PlanCycle.TopLevel = false;
            f_PlanCycle.Size = this.Size;
            this.pnlMenuPlanCycle.Controls.Add(f_PlanCycle);
            f_PlanCycle.Show();

            frm_Shrink f_Shrink = new frm_Shrink();
            f_Shrink.TopLevel = false;
            f_Shrink.Size = this.Size;
            this.pnlMenuShrink.Controls.Add(f_Shrink);
            f_Shrink.Show();

            //frm_PlanAgent f_PlanAgent = new frm_PlanAgent();
            //f_PlanAgent.TopLevel = false;
            //f_PlanAgent.Size = this.Size;
            //this.pnlMenuPlanAgent.Controls.Add(f_PlanAgent);
            //f_PlanAgent.Show();

            //frm_PlanUpload f_PlanUpload = new frm_PlanUpload();
            //f_PlanUpload.TopLevel = false;
            //f_PlanUpload.Size = this.Size;
            //this.pnlMenuPlanUpload.Controls.Add(f_PlanUpload);
            //f_PlanUpload.Show();

            //frm_Shrink f_Shrink = new frm_Shrink();
            //f_Shrink.TopLevel = false;
            //f_Shrink.Size = this.Size;
            //this.pnlMenuShrink.Controls.Add(f_Shrink);
            //f_Shrink.Show();

            //frm_ShrinkPk10 f_ShrinkPK10 = new frm_ShrinkPk10();
            //f_ShrinkPK10.TopLevel = false;
            //f_ShrinkPK10.Size = this.Size;
            //this.pnlPk10Shrink.Controls.Add(f_ShrinkPK10);
            //f_ShrinkPK10.Show();

            //frm_Chart f_Chart = new frm_Chart();
            //f_Chart.TopLevel = false;
            //f_Chart.Size = this.Size;
            //this.pnlMenuChart.Controls.Add(f_Chart);
            //f_Chart.Show();

            #region 進入時預設停在哪一分頁 哪一彩種
            HD_MenuSelect.Text = "神灯周期计划";
            lblMenuPlanCycle.BackColor = HexColor("#bd3100");
            lblMenuPlanCycle.Refresh();
            pnlMenuPlanAgent.Visible = false;
            pnlMenuPlanUpload.Visible = false;
            pnlMenuShrink.Visible = false;
            pnlPk10Shrink.Visible = false;
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
                //System.Diagnostics.Process.Start("http://cimoch.com/fenxitu/index.html");
                frm_TrendAnalysisTrue f_Trend = new frm_TrendAnalysisTrue();
                //MessageBox.Show("尚未开放");
                //return;
                f_Trend.Show();
                return;
            }


            ResetAllMenu(); //重設選單
            //DisposeForm(HD_MenuSelect.Text);
            HD_MenuSelect.Text = ((Label)(sender)).Text;

            switch (HD_MenuSelect.Text)
            {
                case "神灯周期计划":
                    if(isFirstf_PlanCycle)
                    { 
                        frm_PlanCycle f_PlanCycle = new frm_PlanCycle();
                        f_PlanCycle.TopLevel = false;
                        f_PlanCycle.Size = this.Size;
                        this.pnlMenuPlanCycle.Controls.Add(f_PlanCycle);
                        f_PlanCycle.Show();
                        isFirstf_PlanCycle = false;
                    }
                    lblMenuPlanCycle.BackColor = HexColor("#bd3100");
                    lblMenuPlanCycle.Refresh();
                    pnlMenuPlanCycle.Visible = true;
                    break;
                case "代理计划":
                    if(isFirstf_PlanAgent)
                    { 
                        frm_PlanAgent f_PlanAgent = new frm_PlanAgent();
                        f_PlanAgent.TopLevel = false;
                        f_PlanAgent.Size = this.Size;
                        this.pnlMenuPlanAgent.Controls.Add(f_PlanAgent);
                        f_PlanAgent.Show();
                        isFirstf_PlanAgent = false;
                    }
                    lblMenuPlanAgent.BackColor = HexColor("#bd3100");
                    lblMenuPlanAgent.Refresh();
                    pnlMenuPlanAgent.Visible = true;
                    break;
                case "计划上传":
                    if (isFirstf_PlanUpload)
                    { 
                        frm_PlanUpload f_PlanUpload = new frm_PlanUpload();
                        f_PlanUpload.TopLevel = false;
                        f_PlanUpload.Size = this.Size;
                        this.pnlMenuPlanUpload.Controls.Add(f_PlanUpload);
                        f_PlanUpload.Show();
                        isFirstf_PlanUpload = false;
                    }
                    lblMenuPlanUpload.BackColor = HexColor("#bd3100");
                    lblMenuPlanUpload.Refresh();
                    pnlMenuPlanUpload.Visible = true;
                    break;
                case "缩水工具":                   
                    lblMenuShrink.BackColor = HexColor("#bd3100");
                    lblMenuShrink.Refresh();
                    if (HD_GameSelect.Text == "北京PK10")
                    {
                        if(isFirstf_ShrinkPK10)
                        { 
                            frm_ShrinkPk10 f_ShrinkPK10 = new frm_ShrinkPk10();
                            f_ShrinkPK10.TopLevel = false;
                            f_ShrinkPK10.Size = this.Size;
                            this.pnlPk10Shrink.Controls.Add(f_ShrinkPK10);
                            f_ShrinkPK10.Show();
                        }
                        pnlPk10Shrink.Visible = true;
                    }                        
                    else
                    {
                        pnlMenuShrink.Visible = true;
                    }
                    break;
                case "走势图":
                    if(isFirstf_Chart)
                    { 
                        frm_Chart f_Chart = new frm_Chart();
                        f_Chart.TopLevel = false;
                        f_Chart.Size = this.Size;
                        this.pnlMenuChart.Controls.Add(f_Chart);
                        f_Chart.Show();
                        isFirstf_Chart = false;
                    }
                    lblMenuChart.BackColor = HexColor("#bd3100");
                    lblMenuChart.Refresh();
                    pnlMenuChart.Visible = true;
                    break;
            }
        }

        //按下彩票種類
        private void btnGame_Click(object sender, EventArgs e)
        {
            try
            {
                if (HD_GameSelect.Text == ((Label)(sender)).Text)
                    return;

                pnlPk10Shrink.Visible = false;
                pnlMenuShrink.Visible = true;

                //frm_PlanUpload up = new frm_PlanUpload();          

                switch (((Label)(sender)).Text)
                {
                    case "重庆时时彩":
                        ResetAllGame(); //重設彩票
                        lblGame1_1.BackColor = HexColor("#df6600");
                        lblGame1_1.ForeColor = Color.White;
                        lblGame1_1.Refresh();
                        HD_GameSelect.Text = ((Label)(sender)).Text;
                        frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                        frm_Chart.isChange = true;
                        MessageBox.Show("若是未更新请按下刷新按钮");
                        break;
                    case "天津时时彩":
                        ResetAllGame(); //重設彩票
                        lblGame1_5.BackColor = HexColor("#df6600");
                        lblGame1_5.ForeColor = Color.White;
                        lblGame1_5.Refresh();
                        HD_GameSelect.Text = ((Label)(sender)).Text;
                        frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                        frm_Chart.isChange = true;
                        MessageBox.Show("若是未更新请按下刷新按钮");
                        break;
                    case "腾讯官方彩":
                        ResetAllGame(); //重設彩票
                        lblGame1_2.BackColor = HexColor("#df6600");
                        lblGame1_2.ForeColor = Color.White;
                        lblGame1_2.Refresh();
                        HD_GameSelect.Text = ((Label)(sender)).Text;
                        frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                        frm_Chart.isChange = true;
                        MessageBox.Show("若是未更新请按下刷新按钮");
                        break;
                    case "腾讯奇趣彩":
                        ResetAllGame(); //重設彩票
                        lblGame1_3.BackColor = HexColor("#df6600");
                        lblGame1_3.ForeColor = Color.White;
                        lblGame1_3.Refresh();
                        HD_GameSelect.Text = ((Label)(sender)).Text;
                        frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                        frm_Chart.isChange = true;
                        MessageBox.Show("若是未更新请按下刷新按钮");
                        break;
                    case "新疆时时彩":
                        ResetAllGame(); //重設彩票
                        lblGame1_6.BackColor = HexColor("#df6600");
                        lblGame1_6.ForeColor = Color.White;
                        lblGame1_6.Refresh();
                        HD_GameSelect.Text = ((Label)(sender)).Text;
                        frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                        frm_Chart.isChange = true;
                        MessageBox.Show("若是未更新请按下刷新按钮");
                        break;
                    //case "VR金星1.5分彩":
                    //    ResetAllGame(); //重設彩票
                    //    lblGame1_7.BackColor = HexColor("#df6600");
                    //    lblGame1_7.ForeColor = Color.White;
                    //    lblGame1_7.Refresh();
                    //    HD_GameSelect.Text = ((Label)(sender)).Text;
                    //    frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                    //    frm_Chart.isChange = true;
                    //    MessageBox.Show("若是未更新请按下刷新按钮");
                    //    break;
                    case "广东":
                        ResetAllGame(); //重設彩票
                        lblGame3_1.BackColor = HexColor("#df6600");
                        lblGame3_1.ForeColor = Color.White;
                        lblGame3_1.Refresh();
                        HD_GameSelect.Text = ((Label)(sender)).Text;
                        frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                        frm_Chart.isChange = true;
                        MessageBox.Show("若是未更新请按下刷新按钮");
                        break;
                    case "山东":
                        ResetAllGame(); //重設彩票
                        lblGame3_2.BackColor = HexColor("#df6600");
                        lblGame3_2.ForeColor = Color.White;
                        lblGame3_2.Refresh();
                        HD_GameSelect.Text = ((Label)(sender)).Text;
                        frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                        frm_Chart.isChange = true;
                        MessageBox.Show("若是未更新请按下刷新按钮");
                        break;
                    case "江西":
                        ResetAllGame(); //重設彩票
                        lblGame3_3.BackColor = HexColor("#df6600");
                        lblGame3_3.ForeColor = Color.White;
                        lblGame3_3.Refresh();
                        HD_GameSelect.Text = ((Label)(sender)).Text;
                        frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                        frm_Chart.isChange = true;
                        MessageBox.Show("若是未更新请按下刷新按钮");
                        break;
                    case "上海":
                        ResetAllGame(); //重設彩票
                        lblGame3_4.BackColor = HexColor("#df6600");
                        lblGame3_4.ForeColor = Color.White;
                        lblGame3_4.Refresh();
                        HD_GameSelect.Text = ((Label)(sender)).Text;
                        frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                        frm_Chart.isChange = true;
                        MessageBox.Show("若是未更新请按下刷新按钮");
                        break;
                    case "江苏":
                        ResetAllGame(); //重設彩票
                        lblGame3_5.BackColor = HexColor("#df6600");
                        lblGame3_5.ForeColor = Color.White;
                        lblGame3_5.Refresh();
                        HD_GameSelect.Text = ((Label)(sender)).Text;
                        frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                        frm_Chart.isChange = true;
                        MessageBox.Show("若是未更新请按下刷新按钮");
                        break;
                    case "河北":
                        ResetAllGame(); //重設彩票
                        lblGame3_6.BackColor = HexColor("#df6600");
                        lblGame3_6.ForeColor = Color.White;
                        lblGame3_6.Refresh();
                        HD_GameSelect.Text = ((Label)(sender)).Text;
                        frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                        frm_Chart.isChange = true;
                        MessageBox.Show("若是未更新请按下刷新按钮");
                        break;
                    case "北京PK10":
                        ResetAllGame(); //重設彩票
                        lblGame2_1.BackColor = HexColor("#df6600");
                        lblGame2_1.ForeColor = Color.White;
                        lblGame2_1.Refresh();
                        HD_GameSelect.Text = ((Label)(sender)).Text;
                        frm_PlanCycle.GameLotteryName = ((Label)(sender)).Text;
                        frm_Chart.isChange = true;
                        pnlPk10Shrink.Visible = true;
                        pnlMenuShrink.Visible = false;
                        if (isFirstf_ShrinkPK10)
                        {
                            frm_ShrinkPk10 f_ShrinkPK10 = new frm_ShrinkPk10();
                            f_ShrinkPK10.TopLevel = false;
                            f_ShrinkPK10.Size = this.Size;
                            this.pnlPk10Shrink.Controls.Add(f_ShrinkPK10);
                            f_ShrinkPK10.Show();
                        }
                        isFirstf_ShrinkPK10 = false;
                        MessageBox.Show("若是未更新请按下刷新按钮");
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
                frm_PlanUpload.InitcbItem();
                frm_PlanUpload.Funtest();
                frm_PlanUpload.isNeedRefresh = true;
                frm_PlanUpload.isFirstTime = true;
                frm_PlanUpload.isChangeLotteryName = true;


                frm_PlanAgent frm_PlanAgent = new frm_PlanAgent();
                frm_PlanAgent.ischagneGameName = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }           
        }

        //色碼修改
        public Color HexColor(String hex)
        {
          
            //將井字號移除
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;
            int start = 0;

            //處理ARGB字串 
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            // 將RGB文字轉成byte
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }

        //重設選單
        private void ResetAllMenu()
        {
            lblMenuPlanCycle.BackColor = HexColor("#df8514"); lblMenuPlanCycle.Refresh();
            lblMenuPlanAgent.BackColor = HexColor("#df8514"); lblMenuPlanAgent.Refresh();
            lblMenuPlanUpload.BackColor = HexColor("#df8514"); lblMenuPlanUpload.Refresh();
            lblMenuShrink.BackColor = HexColor("#df8514"); lblMenuShrink.Refresh();
            lblMenuChart.BackColor = HexColor("#df8514"); lblMenuChart.Refresh();
            pnlMenuPlanCycle.Visible = false;
            pnlMenuPlanAgent.Visible = false;
            pnlMenuPlanUpload.Visible = false;
            pnlMenuShrink.Visible = false;
            pnlMenuChart.Visible = false;
            pnlPk10Shrink.Visible = false;
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
            lblGame2_6.BackColor = Color.Transparent; lblGame2_6.ForeColor = Color.Black; lblGame2_6.Refresh();
            lblGame3_1.BackColor = Color.Transparent; lblGame3_1.ForeColor = Color.Black; lblGame3_1.Refresh();
            lblGame3_2.BackColor = Color.Transparent; lblGame3_2.ForeColor = Color.Black; lblGame3_2.Refresh();
            lblGame3_3.BackColor = Color.Transparent; lblGame3_3.ForeColor = Color.Black; lblGame3_3.Refresh();
            lblGame3_4.BackColor = Color.Transparent; lblGame3_4.ForeColor = Color.Black; lblGame3_4.Refresh();
            lblGame3_5.BackColor = Color.Transparent; lblGame3_5.ForeColor = Color.Black; lblGame3_5.Refresh();
            lblGame3_6.BackColor = Color.Transparent; lblGame3_6.ForeColor = Color.Black; lblGame3_6.Refresh();
        }

        //取得下一期時間
        DateTime dt1Vr = DateTime.Now.AddSeconds(30).AddMinutes(1);
        DateTime dt1PK10 = DateTime.Now.AddMinutes(5);

        private void useHttpWebRequest_GetNextPeriod()
        {
            try
            {
                if (HD_GameSelect.Text == "北京PK10")
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
                    /*
                    DateTime dt2PK10 = DateTime.Now;
                    if (dt1PK10 < dt2PK10)
                        dt1PK10 = DateTime.Now.AddMinutes(5);

                    TimeSpan ts = new TimeSpan(dt1PK10.Ticks - dt2PK10.Ticks);
                    string hh = ts.Hours.ToString("00");
                    string mm = ts.Minutes.ToString("00");
                    string ss = ts.Seconds.ToString("00");
                    if (ss.IndexOf("-") > -1)
                        ss = "00";
                    lblNextPeriodTime.Text = hh + " : " + mm + " : " + ss;
                    */
                }
                else if (HD_GameSelect.Text == "VR金星1.5分彩")
                {
                    DateTime dt2Vr = DateTime.Now;
                    if (dt1Vr < dt2Vr)
                        dt1Vr = DateTime.Now.AddSeconds(30).AddMinutes(1);

                    TimeSpan ts = new TimeSpan(dt1Vr.Ticks - dt2Vr.Ticks);
                    string hh = ts.Hours.ToString("00");
                    string mm = ts.Minutes.ToString("00");
                    string ss = ts.Seconds.ToString("00");
                    if (ss.IndexOf("-") > -1)
                        ss = "00";
                    lblNextPeriodTime.Text = hh + " : " + mm + " : " + ss;
                }
                else if (HD_GameSelect.Text == "广东" || HD_GameSelect.Text == "江西" || HD_GameSelect.Text == "上海" || HD_GameSelect.Text == "山东" || HD_GameSelect.Text == "河北" || HD_GameSelect.Text == "江苏")
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
                else
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }            
        }

        //取得歷史開獎
        private void useHttpWebRequest_GetHistory()
        {
            try
            {
                if (HD_GameSelect.Text == "北京PK10")
                {
                    pnlGameLastNumber.Size = new Size(500, 82);
                    //設定右上方的號碼圖樣
                    lblNumber1.Visible = true;
                    lblNumber6.Visible = true;
                    lblNumber7.Visible = true;
                    lblNumber8.Visible = true;
                    lblNumber9.Visible = true;
                    lblNumber10.Visible = true;
                    picNumber6.Visible = true;
                    picNumber7.Visible = true;
                    picNumber8.Visible = true;
                    picNumber9.Visible = true;
                    picNumber10.Visible = true;

                    lblNumber1.Font = new Font("Verdana", 12, FontStyle.Bold);
                    lblNumber1.Location = new Point(9, 43);
                    picNumber1.Location = new Point(0, 30);

                    lblNumber2.Font = new Font("Verdana", 12, FontStyle.Bold);
                    lblNumber2.Location = new Point(59, 43);
                    picNumber2.Location = new Point(51, 30);

                    lblNumber3.Font = new Font("Verdana", 12, FontStyle.Bold);
                    lblNumber3.Location = new Point(109, 43);
                    picNumber3.Location = new Point(101, 30);

                    lblNumber4.Font = new Font("Verdana", 12, FontStyle.Bold);
                    lblNumber4.Location = new Point(159, 43);
                    picNumber4.Location = new Point(151, 30);

                    lblNumber5.Font = new Font("Verdana", 12, FontStyle.Bold);
                    lblNumber5.Location = new Point(209, 43);
                    picNumber5.Location = new Point(201, 30);

                    lblNumber6.Font = new Font("Verdana", 12, FontStyle.Bold);
                    lblNumber6.Location = new Point(259, 43);
                    picNumber6.Location = new Point(251, 30);

                    lblNumber7.Font = new Font("Verdana", 12, FontStyle.Bold);
                    lblNumber7.Location = new Point(309, 43);
                    picNumber7.Location = new Point(301, 30);

                    lblNumber8.Font = new Font("Verdana", 12, FontStyle.Bold);
                    lblNumber8.Location = new Point(360, 43);
                    picNumber8.Location = new Point(351, 30);

                    lblNumber9.Font = new Font("Verdana", 12, FontStyle.Bold);
                    lblNumber9.Location = new Point(410, 43);
                    picNumber9.Location = new Point(401, 30);

                    lblNumber10.Font = new Font("Verdana", 12, FontStyle.Bold);
                    lblNumber10.Location = new Point(460, 43);
                    picNumber10.Location = new Point(451, 30);


                    DataTable dtPK10 = ConnectDbGetHistoryNumberForPK10();
                    string str_json = JsonConvert.SerializeObject(dtPK10, Formatting.Indented);
                    JArray ja = (JArray)JsonConvert.DeserializeObject(str_json);
                    jArr = ja;
                    string lastWinPeriod = ja[0]["Issue"].ToString(); //最近開獎的期數
                    globalGetCurrentPeriod = (double.Parse(lastWinPeriod) + 1).ToString();

                    lblCurrentPeriod.Text = lastWinPeriod; //當期
                    string Number = ja[0]["Number"].ToString().Replace(",", "");
                    lblNumber1.Text = ja[0]["Number"].ToString().Substring(0, 2);
                    lblNumber2.Text = ja[0]["Number"].ToString().Substring(2, 2);
                    lblNumber3.Text = ja[0]["Number"].ToString().Substring(4, 2);
                    lblNumber4.Text = ja[0]["Number"].ToString().Substring(6, 2);
                    lblNumber5.Text = ja[0]["Number"].ToString().Substring(8, 2);
                    lblNumber6.Text = ja[0]["Number"].ToString().Substring(10, 2);
                    lblNumber7.Text = ja[0]["Number"].ToString().Substring(12, 2);
                    lblNumber8.Text = ja[0]["Number"].ToString().Substring(14, 2);
                    lblNumber9.Text = ja[0]["Number"].ToString().Substring(16, 2);
                    lblNumber10.Text = ja[0]["Number"].ToString().Substring(18, 2);
                    strHistoryNumberOpen = ja[0]["Number"].ToString().Substring(0, 2);


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
                else if (HD_GameSelect.Text == "VR金星1.5分彩")
                {
                    pnlGameLastNumber.Size = new Size(358, 82);

                    lblNumber1.Font = new Font("Verdana", 18, FontStyle.Bold);
                    lblNumber1.Location = new Point(49, 38);
                    picNumber1.Location = new Point(40, 30);

                    lblNumber2.Font = new Font("Verdana", 18, FontStyle.Bold);
                    lblNumber2.Location = new Point(101, 38);
                    picNumber2.Location = new Point(91, 30);

                    lblNumber3.Font = new Font("Verdana", 18, FontStyle.Bold);
                    lblNumber3.Location = new Point(152, 38);
                    picNumber3.Location = new Point(142, 30);

                    lblNumber4.Font = new Font("Verdana", 18, FontStyle.Bold);
                    lblNumber4.Location = new Point(204, 38);
                    picNumber4.Location = new Point(194, 30);

                    lblNumber5.Font = new Font("Verdana", 18, FontStyle.Bold);
                    lblNumber5.Location = new Point(255, 38);
                    picNumber5.Location = new Point(245, 30);

                    //PK10專用
                    lblNumber6.Visible = false;
                    lblNumber7.Visible = false;
                    lblNumber8.Visible = false;
                    lblNumber9.Visible = false;
                    lblNumber10.Visible = false;
                    picNumber6.Visible = false;
                    picNumber7.Visible = false;
                    picNumber8.Visible = false;
                    picNumber9.Visible = false;
                    picNumber10.Visible = false;

                    DataTable dtVR15 = ConnectDbGetHistoryNumberForVR15();
                    if (dtVR15 == null || dtVR15.Rows.Count == 0)
                    {
                        dtVR15 = ConnectDbGetHistoryNumberForVR15YesterDay();
                    }
                    string str_json = JsonConvert.SerializeObject(dtVR15, Formatting.Indented);
                    JArray ja = (JArray)JsonConvert.DeserializeObject(str_json);
                    jArr = ja;
                    string lastWinPeriod = ja[0]["Issue"].ToString(); //最近開獎的期數
                    globalGetCurrentPeriod = (double.Parse(lastWinPeriod) + 1).ToString();
                    if ((lastWinPeriod.Substring(8, 3) == "84" && lblNextPeriod.Text.Substring(8, 3) == "002")
                        || (lastWinPeriod.Substring(8, 3) == "119" && lblNextPeriod.Text.Substring(8, 3) == "001")) //倒數結束後到完成開獎的空檔 針對跨日( 0404120期>0405002期 或 0404119期>0405001期 )
                    {
                        if (lastWinPeriod.Substring(8, 3) == "84")
                            lblCurrentPeriod.Text = lblNextPeriod.Text.Substring(0, 8) + "" + "001"; //當期
                        else
                            lblCurrentPeriod.Text = lastWinPeriod.Substring(0, 8) + "" + "84"; //當期
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
                        lblNumber2.Text = ja[0]["Number"].ToString().Substring(1, 1);
                        lblNumber3.Text = ja[0]["Number"].ToString().Substring(2, 1);
                        lblNumber4.Text = ja[0]["Number"].ToString().Substring(3, 1);
                        lblNumber5.Text = ja[0]["Number"].ToString().Substring(4, 1);
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
                }
                else if (HD_GameSelect.Text == "广东" || HD_GameSelect.Text == "江西" || HD_GameSelect.Text == "上海" || HD_GameSelect.Text == "山东" || HD_GameSelect.Text == "河北" || HD_GameSelect.Text == "江苏")//江苏
                {
                    pnlGameLastNumber.Size = new Size(358, 82);
                    //PK10專用
                    lblNumber6.Visible = false;
                    lblNumber7.Visible = false;
                    lblNumber8.Visible = false;
                    lblNumber9.Visible = false;
                    lblNumber10.Visible = false;
                    picNumber6.Visible = false;
                    picNumber7.Visible = false;
                    picNumber8.Visible = false;
                    picNumber9.Visible = false;
                    picNumber10.Visible = false;

                    lblNumber1.Font = new Font("Verdana", 12, FontStyle.Bold);
                    lblNumber1.Location = new Point(49, 43);
                    lblNumber2.Font = new Font("Verdana", 12, FontStyle.Bold);
                    lblNumber2.Location = new Point(101, 43);
                    lblNumber3.Font = new Font("Verdana", 12, FontStyle.Bold);
                    lblNumber3.Location = new Point(152, 43);
                    lblNumber4.Font = new Font("Verdana", 12, FontStyle.Bold);
                    lblNumber4.Location = new Point(204, 43);
                    lblNumber5.Font = new Font("Verdana", 12, FontStyle.Bold);
                    lblNumber5.Location = new Point(255, 43);

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
                                //else if (Int16.Parse(lblNextPeriod.Text.Substring(8, 3)) - Int16.Parse(lastWinPeriod.Substring(8, 3)) == 2)//倒數結束後到完成開獎的空檔 針對同一日( 0404100期>0404098期 )
                                //{
                                //    lblCurrentPeriod.Text = (Convert.ToInt64(lastWinPeriod) + 1).ToString(); //當期
                                //    lblNumber1.Text = lblNumber2.Text = lblNumber3.Text = lblNumber4.Text = lblNumber5.Text = "?";
                                //    strHistoryNumberOpen = "?";
                                //}
                                else
                                {
                                    lblCurrentPeriod.Text = lastWinPeriod; //當期
                                    lblNumber1.Text = ja[0]["Number"].ToString().Substring(0, 2);
                                    lblNumber2.Text = ja[0]["Number"].ToString().Substring(3, 2);
                                    lblNumber3.Text = ja[0]["Number"].ToString().Substring(6, 2);
                                    lblNumber4.Text = ja[0]["Number"].ToString().Substring(9, 2);
                                    lblNumber5.Text = ja[0]["Number"].ToString().Substring(12, 2);
                                    strHistoryNumberOpen = ja[0]["Number"].ToString().Substring(0, 2);


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
                                //    strHistory += ja[i]["4Issue"].ToString() + "  " + ja[i]["Number"].ToString().Replace(",", " ") + "\r\n";
                                //}
                            }
                        }
                        //else
                        //{ }
                    }
                    #endregion
                }
                else
                {
                    pnlGameLastNumber.Size = new Size(358, 82);
                    //PK10專用
                    lblNumber6.Visible = false;
                    lblNumber7.Visible = false;
                    lblNumber8.Visible = false;
                    lblNumber9.Visible = false;
                    lblNumber10.Visible = false;
                    picNumber6.Visible = false;
                    picNumber7.Visible = false;
                    picNumber8.Visible = false;
                    picNumber9.Visible = false;
                    picNumber10.Visible = false;


                    lblNumber1.Font = new Font("Verdana", 18, FontStyle.Bold);
                    lblNumber1.Location = new Point(49, 38);
                    picNumber1.Location = new Point(40, 30);

                    lblNumber2.Font = new Font("Verdana", 18, FontStyle.Bold);
                    lblNumber2.Location = new Point(101, 38);
                    picNumber2.Location = new Point(91, 30);

                    lblNumber3.Font = new Font("Verdana", 18, FontStyle.Bold);
                    lblNumber3.Location = new Point(152, 38);
                    picNumber3.Location = new Point(142, 30);

                    lblNumber4.Font = new Font("Verdana", 18, FontStyle.Bold);
                    lblNumber4.Location = new Point(204, 38);
                    picNumber4.Location = new Point(194, 30);

                    lblNumber5.Font = new Font("Verdana", 18, FontStyle.Bold);
                    lblNumber5.Location = new Point(255, 38);
                    picNumber5.Location = new Point(245, 30);

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
                                globalGetCurrentPeriod = (double.Parse(jArr.First()["Issue"].ToString()) + 1).ToString();
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
                                //    strHistory += ja[i]["4Issue"].ToString() + "  " + ja[i]["Number"].ToString().Replace(",", " ") + "\r\n";
                                //}
                            }
                        }
                        //else
                        //{ }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
          
        }

        private DataTable ConnectDbGetHistoryNumberForVR15()
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";

            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = abc; Password=123456";
            con = new SqlConnection(connetionString);
            string date = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            try
            {
                con.Open();
                string Sqlstr = @"SELECT issue as Issue, replace(number,',','') as Number FROM VR15_HistoryNumber WHERE issue LIKE '" + date + "%' ORDER BY issue DESC";
                SqlDataAdapter da = new SqlDataAdapter(Sqlstr, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        private DataTable ConnectDbGetHistoryNumberForPK10()
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";

            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = abc; Password=123456";
            con = new SqlConnection(connetionString);
            string date = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            try
            {
                con.Open();
                string Sqlstr = @"SELECT issue as Issue, replace(number,',','') as Number FROM PK10_HistoryNumber ORDER BY issue DESC";//WHERE issue LIKE '" + date + "%'
                SqlDataAdapter da = new SqlDataAdapter(Sqlstr, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        private DataTable ConnectDbGetHistoryNumberForVR15YesterDay()
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";

            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = abc; Password=123456";
            con = new SqlConnection(connetionString);
            string date = DateTime.Now.AddDays(-1).ToString("u").Substring(0, 10).Replace("-", "");
            try
            {
                con.Open();
                string Sqlstr = @"SELECT issue as Issue, replace(number,',','') as Number FROM VR15_HistoryNumber WHERE issue LIKE '" + date + "%' ORDER BY issue DESC";
                SqlDataAdapter da = new SqlDataAdapter(Sqlstr, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
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
            ShowMessage();
            timer_ShowMessage.Interval = 300000;
            //timer_ShowMessage.Dispose();
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
            //timer_GetGameInfo.Dispose();

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
            //if(!bgwGodinsert.IsBusy)
            //    bgwGodinsert.RunWorkerAsync();
        }

        private void ConnectDbGetHistoryNumber()
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";

            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = abc; Password=123456";
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
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = abc; Password=123456";
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
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = abc; Password=123456";
            con = new SqlConnection(connetionString);
            string date = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            try
            {
                //todo 修改每種不同的號碼
                if (PlanName == 0)
                {
                    con.Open();
                    string Sqlstr = @"SELECT top(40) number AS Number FROM RandomNumber WHERE date = '20180720' AND type = '{1}' ";
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
WHERE date = '20180720' AND type = '{1}'
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
WHERE date = '20180720' AND type = '{1}'
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
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = abc; Password=123456";
            con = new SqlConnection(connetionString);
            try
            {
                con.Open();
                string Sqlstr = @"BEGIN IF NOT EXISTS (SELECT * FROM GodListPlanCycle WHERE g_buttomName = '{0}') BEGIN Insert into GodListPlanCycle values('{0}','{1}') END END";
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
        private void DeleteGod()
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";
            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = abc; Password=123456";
            con = new SqlConnection(connetionString);
            try
            {
                con.Open();
                string Sqlstr = @"Delete GodListPlanCycle";
                var cmd = new SqlCommand(Sqlstr, con);
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
    }
}
