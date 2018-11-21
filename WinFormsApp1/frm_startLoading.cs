using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class frm_startLoading : Form
    {
        public static JArray jArrHistoryNumber;
        string NowAnalyzeNumber = "";
        public static JArray NowAnalyzeNumberArr;

        public frm_startLoading()
        {
            InitializeComponent();
            loadUrlAdPicture();

            this.ControlBox = false;
            bkgStart.RunWorkerAsync();
            bkgStart.WorkerReportsProgress = true;//啟動回報進度
            pgbShow.Maximum = 5;//ProgressBar上限
            pgbShow.Minimum = 0;//ProgressBar下限
        }

        string clickUrl = "";
        private void loadUrlAdPicture()
        {
            if (frmGameMain.PlanProxyUser != "" && frmGameMain.PlanProxyPassWord != "")
            {
                string User = frmGameMain.PlanProxyUser;
                clickUrl = getBackPlatfromDb(User);

                if (clickUrl == null)
                {
                    MessageBox.Show("读取错误请洽客服");
                    return;
                }

                string Url = string.Format("http://43.252.208.201:81/Upload/{0}/3/3.jpg", User);
                var request = WebRequest.Create(Url);
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    ptbAd.Image = Bitmap.FromStream(stream);
                    ptbAd.Click += new EventHandler(pic_Click);
                }
            }
        }

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
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = abc; Password=123456";
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
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        private void bkgStart_DoWork(object sender, DoWorkEventArgs e)
        {
            //刪除舊有資料
            DeleteGod();

            ConnectDbGetHistoryNumber();
            //玩法
            string GameKind = "";
            //數量
            string GameType = "";
            //單式或複式
            string GameDirect = "单式";

            //GameKind的七種玩法
            for (int i = 0; i < 5; i++)
            {
                //System.Threading.Thread.Sleep(10);
                bkgStart.ReportProgress(i+1);

                switch (i)
                {
                    case 4:
                        GameKind = "前三";
                        break;
                    case 3:
                        GameKind = "中三";
                        break;
                    case 2:
                        GameKind = "后三";
                        break;
                    case 1:
                        GameKind = "前二";
                        break;
                    case 0:
                        GameKind = "后二";
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
                    else if (GameKind == "前三")
                    {
                        switch (iPlus)
                        {
                            case 0:
                                GameType = "300+";
                                break;
                            case 1:
                                GameType = "400+";
                                break;
                            case 2:
                                GameType = "500+";
                                break;
                        }
                    }
                    else if (GameKind == "中三")
                    {
                        switch (iPlus)
                        {
                            case 0:
                                GameType = "300+";
                                break;
                            case 1:
                                GameType = "400+";
                                break;
                            case 2:
                                GameType = "500+";
                                break;
                        }
                    }
                    else if (GameKind == "后三")
                    {
                        switch (iPlus)
                        {
                            case 0:
                                GameType = "300+";
                                break;
                            case 1:
                                GameType = "400+";
                                break;
                            case 2:
                                GameType = "500+";
                                break;
                        }
                    }
                    else if (GameKind == "前二")
                    {
                        switch (iPlus)
                        {
                            case 0:
                                GameType = "30+";
                                break;
                            case 1:
                                GameType = "40+";
                                break;
                            case 2:
                                GameType = "50+";
                                break;
                        }
                    }
                    else if (GameKind == "后二")
                    {
                        switch (iPlus)
                        {
                            case 0:
                                GameType = "30+";
                                break;
                            case 1:
                                GameType = "40+";
                                break;
                            case 2:
                                GameType = "50+";
                                break;
                        }
                    }

                    //抓取計畫名稱
                    DataTable dtGamePlan = getPlanName(GameKind);
                    string GamePlanName = "";
                    //string[] numHistory;
                   
                    for (int iPlan = 0; iPlan < dtGamePlan.Rows.Count; iPlan++)
                    {
                        
                        GamePlanName = dtGamePlan.Rows[iPlan]["GamePlan_name"].ToString();

                        string GameCycle = "";
                        //三期一周 / 二期一周 / 一期一周
                        for (int iGameCycle = 1; iGameCycle < 4; iGameCycle++)
                        {
                            int cycle_2 = 1; //比對開獎的周期數
                            int sumBets = 0;

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
                            else if (GameType.Contains("5"))
                            {
                                GameCycle = "一期一周";
                            }
                            else
                            {
                                continue;
                            }

                            //抓取比對的投注數量
                            ConnectDbGetRandomNumber(GameType, iPlan, GameCycle);

                            string threeNumber = NowAnalyzeNumber;
                            List<string> numHistoryList = new List<string>();
                            numHistoryList.Add(threeNumber);
                            var numHistory = NowAnalyzeNumberArr.ToArray();

                            //開始比對
                            if (GameKind == "后二") //&& (cbGameCycle.Text == "三期一周" || cbGameCycle.Text == "二期一周")
                            {

                                if (GameDirect == "复式")
                                {

                                }
                                else if (GameDirect == "单式")
                                {
                                    int hisArr = 0;
                                    int sumWin = 0;
                                    int LastBets = 0;

                                    #region 驗證是否中奖
                                    bool isWin = false; //中了沒
                                    int periodtWin = 0; //第幾期中
                                    string[] temp = { "", "", "" }; //存放combobox的值

                                    for (int iii = 0; iii < jArrHistoryNumber.Count(); iii++) //從歷史結果開始比
                                    {
                                        //reset
                                        isWin = false;
                                        periodtWin = 0;
                                        temp[0] = "";
                                        temp[1] = "";
                                        temp[2] = "";

                                        for (int j = 0; j < iGameCycle; j++)
                                        {
                                            if (iii >= jArrHistoryNumber.Count()) break;

                                            string strMatch = "";
                                            switch (GameKind)
                                            {
                                                case "五星":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "");
                                                    break;
                                                case "四星":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 4);
                                                    break;
                                                case "前三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 3);
                                                    break;
                                                case "中三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(1, 3);
                                                    break;
                                                case "后三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(2, 3);
                                                    break;
                                                case "前二":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 2);
                                                    break;
                                                case "后二":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(3, 2);
                                                    break;
                                            }
                                            if (isWin == false) //還沒中
                                            {
                                                ///////////////cycle_2 - 1
                                                if (NowAnalyzeNumberArr[hisArr].ToString().IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                }
                                                else //挂
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 挂";
                                                }
                                                sumBets++;
                                                periodtWin = j + 1;
                                            }
                                            else //前面已中奖
                                            {
                                                temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 停";
                                                //cycle_2++;
                                            }
                                            iii++;
                                        }

                                        cycle_2++;
                                        iii--;
                                        hisArr++;

                                        Label lbl_2 = new Label();
                                        lbl_2.Text = periodtWin.ToString();
                                        lbl_2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                                        lbl_2.Padding = new System.Windows.Forms.Padding(20, 6, 20, 6);
                                        lbl_2.Size = new System.Drawing.Size(53, 25);
                                        lbl_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                                        LastBets += Convert.ToInt32(lbl_2.Text);
                                        ComboBox cb_1 = new ComboBox();
                                        for (int k = 0; k < 3; k++)
                                        {
                                            if (temp[k] != "")
                                                cb_1.Items.Add(temp[k]);
                                        }

                                        if (isWin == true)
                                        {
                                            sumWin++;
                                        }
                                    }

                                    #endregion

                                    #region 計算
                                    //每期注數 共?元
                                    //lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString("0.000");
                                    //目前下注?周期
                                    string CurrentBetsCycle = (cycle_2 - 1).ToString();
                                    int test = LastBets;
                                    string aaaa = GameKind + GameCycle + GamePlanName + GameType;
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(LastBets));
                                    //lblPlanWinOpp.Text = WinOpp.ToString("0.00");
                                    string insertInfo = GameKind + "," + GameDirect + "," + GamePlanName + ", " + GameCycle + "," + GameType;
                                    InsertIntoGod(insertInfo, WinOpp.ToString("0.00"));
                                    #endregion
                                }
                            }
                            else if (GameKind == "前二") //&& (cbGameCycle.Text == "三期一周" || cbGameCycle.Text == "二期一周")
                            {

                                if (GameDirect == "复式")
                                {

                                }
                                else if (GameDirect == "单式")
                                {
                                    int hisArr = 0;
                                    int sumWin = 0;
                                    int LastBets = 0;

                                    #region 驗證是否中奖
                                    bool isWin = false; //中了沒
                                    int periodtWin = 0; //第幾期中
                                    string[] temp = { "", "", "" }; //存放combobox的值

                                    for (int iii = 0; iii < jArrHistoryNumber.Count(); iii++) //從歷史結果開始比
                                    {
                                        //reset
                                        isWin = false;
                                        periodtWin = 0;
                                        temp[0] = "";
                                        temp[1] = "";
                                        temp[2] = "";

                                        for (int j = 0; j < iGameCycle; j++)
                                        {
                                            if (iii >= jArrHistoryNumber.Count()) break;

                                            string strMatch = "";
                                            switch (GameKind)
                                            {
                                                case "五星":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "");
                                                    break;
                                                case "四星":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 4);
                                                    break;
                                                case "前三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 3);
                                                    break;
                                                case "中三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(1, 3);
                                                    break;
                                                case "后三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(2, 3);
                                                    break;
                                                case "前二":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 2);
                                                    break;
                                                case "后二":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(3, 2);
                                                    break;
                                            }
                                            if (isWin == false) //還沒中
                                            {
                                                ///////////////cycle_2 - 1
                                                if (NowAnalyzeNumberArr[hisArr].ToString().IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                }
                                                else //挂
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 挂";
                                                }
                                                sumBets++;
                                                periodtWin = j + 1;
                                            }
                                            else //前面已中奖
                                            {
                                                temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 停";
                                                //cycle_2++;
                                            }
                                            iii++;
                                        }

                                        cycle_2++;
                                        iii--;
                                        hisArr++;

                                        Label lbl_2 = new Label();
                                        lbl_2.Text = periodtWin.ToString();
                                        lbl_2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                                        lbl_2.Padding = new System.Windows.Forms.Padding(20, 6, 20, 6);
                                        lbl_2.Size = new System.Drawing.Size(53, 25);
                                        lbl_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                                        LastBets += Convert.ToInt32(lbl_2.Text);
                                        ComboBox cb_1 = new ComboBox();
                                        for (int k = 0; k < 3; k++)
                                        {
                                            if (temp[k] != "")
                                                cb_1.Items.Add(temp[k]);
                                        }

                                        if (isWin == true)
                                        {
                                            sumWin++;
                                        }
                                    }

                                    #endregion

                                    #region 計算
                                    //每期注數 共?元
                                    //lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString("0.000");
                                    //目前下注?周期
                                    string CurrentBetsCycle = (cycle_2 - 1).ToString();
                                    int test = LastBets;
                                    string aaaa = GameKind + GameCycle + GamePlanName + GameType;
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(LastBets));
                                    //lblPlanWinOpp.Text = WinOpp.ToString("0.00");
                                    string insertInfo = GameKind + "," + GameDirect + "," + GamePlanName + ", " + GameCycle + "," + GameType;
                                    InsertIntoGod(insertInfo, WinOpp.ToString("0.00"));
                                    #endregion
                                }
                            }
                            else if (GameKind == "后三") //&& (cbGameCycle.Text == "三期一周" || cbGameCycle.Text == "二期一周")
                            {

                                if (GameDirect == "复式")
                                {

                                }
                                else if (GameDirect == "单式")
                                {
                                    int hisArr = 0;
                                    int sumWin = 0;
                                    int LastBets = 0;

                                    #region 驗證是否中奖
                                    bool isWin = false; //中了沒
                                    int periodtWin = 0; //第幾期中
                                    string[] temp = { "", "", "" }; //存放combobox的值

                                    for (int iii = 0; iii < jArrHistoryNumber.Count(); iii++) //從歷史結果開始比
                                    {
                                        //reset
                                        isWin = false;
                                        periodtWin = 0;
                                        temp[0] = "";
                                        temp[1] = "";
                                        temp[2] = "";

                                        for (int j = 0; j < iGameCycle; j++)
                                        {
                                            if (iii >= jArrHistoryNumber.Count()) break;

                                            string strMatch = "";
                                            switch (GameKind)
                                            {
                                                case "五星":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "");
                                                    break;
                                                case "四星":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 4);
                                                    break;
                                                case "前三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 3);
                                                    break;
                                                case "中三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(1, 3);
                                                    break;
                                                case "后三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(2, 3);
                                                    break;
                                                case "前二":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 2);
                                                    break;
                                                case "后二":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(3, 2);
                                                    break;
                                            }
                                            if (isWin == false) //還沒中
                                            {
                                                ///////////////cycle_2 - 1
                                                if (NowAnalyzeNumberArr[hisArr].ToString().IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                }
                                                else //挂
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 挂";
                                                }
                                                sumBets++;
                                                periodtWin = j + 1;
                                            }
                                            else //前面已中奖
                                            {
                                                temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 停";
                                                //cycle_2++;
                                            }
                                            iii++;
                                        }

                                        cycle_2++;
                                        iii--;
                                        hisArr++;

                                        Label lbl_2 = new Label();
                                        lbl_2.Text = periodtWin.ToString();
                                        lbl_2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                                        lbl_2.Padding = new System.Windows.Forms.Padding(20, 6, 20, 6);
                                        lbl_2.Size = new System.Drawing.Size(53, 25);
                                        lbl_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                                        LastBets += Convert.ToInt32(lbl_2.Text);
                                        ComboBox cb_1 = new ComboBox();
                                        for (int k = 0; k < 3; k++)
                                        {
                                            if (temp[k] != "")
                                                cb_1.Items.Add(temp[k]);
                                        }

                                        if (isWin == true)
                                        {
                                            sumWin++;
                                        }
                                    }

                                    #endregion

                                    #region 計算
                                    //每期注數 共?元
                                    //lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString("0.000");
                                    //目前下注?周期
                                    string CurrentBetsCycle = (cycle_2 - 1).ToString();
                                    int test = LastBets;
                                    string aaaa = GameKind + GameCycle + GamePlanName + GameType;
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(LastBets));
                                    //lblPlanWinOpp.Text = WinOpp.ToString("0.00");
                                    string insertInfo = GameKind + "," + GameDirect + "," + GamePlanName + ", " + GameCycle + "," + GameType;
                                    InsertIntoGod(insertInfo, WinOpp.ToString("0.00"));
                                    #endregion
                                }
                            }
                            else if (GameKind == "中三") //&& (cbGameCycle.Text == "三期一周" || cbGameCycle.Text == "二期一周")
                            {

                                if (GameDirect == "复式")
                                {

                                }
                                else if (GameDirect == "单式")
                                {
                                    int hisArr = 0;
                                    int sumWin = 0;
                                    int LastBets = 0;

                                    #region 驗證是否中奖
                                    bool isWin = false; //中了沒
                                    int periodtWin = 0; //第幾期中
                                    string[] temp = { "", "", "" }; //存放combobox的值

                                    for (int iii = 0; iii < jArrHistoryNumber.Count(); iii++) //從歷史結果開始比
                                    {
                                        //reset
                                        isWin = false;
                                        periodtWin = 0;
                                        temp[0] = "";
                                        temp[1] = "";
                                        temp[2] = "";

                                        for (int j = 0; j < iGameCycle; j++)
                                        {
                                            if (iii >= jArrHistoryNumber.Count()) break;

                                            string strMatch = "";
                                            switch (GameKind)
                                            {
                                                case "五星":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "");
                                                    break;
                                                case "四星":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 4);
                                                    break;
                                                case "前三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 3);
                                                    break;
                                                case "中三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(1, 3);
                                                    break;
                                                case "后三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(2, 3);
                                                    break;
                                                case "前二":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 2);
                                                    break;
                                                case "后二":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(3, 2);
                                                    break;
                                            }
                                            if (isWin == false) //還沒中
                                            {
                                                ///////////////cycle_2 - 1
                                                if (NowAnalyzeNumberArr[hisArr].ToString().IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                }
                                                else //挂
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 挂";
                                                }
                                                sumBets++;
                                                periodtWin = j + 1;
                                            }
                                            else //前面已中奖
                                            {
                                                temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 停";
                                                //cycle_2++;
                                            }
                                            iii++;
                                        }

                                        cycle_2++;
                                        iii--;
                                        hisArr++;

                                        Label lbl_2 = new Label();
                                        lbl_2.Text = periodtWin.ToString();
                                        lbl_2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                                        lbl_2.Padding = new System.Windows.Forms.Padding(20, 6, 20, 6);
                                        lbl_2.Size = new System.Drawing.Size(53, 25);
                                        lbl_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                                        LastBets += Convert.ToInt32(lbl_2.Text);
                                        ComboBox cb_1 = new ComboBox();
                                        for (int k = 0; k < 3; k++)
                                        {
                                            if (temp[k] != "")
                                                cb_1.Items.Add(temp[k]);
                                        }

                                        if (isWin == true)
                                        {
                                            sumWin++;
                                        }
                                    }

                                    #endregion

                                    #region 計算
                                    //每期注數 共?元
                                    //lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString("0.000");
                                    //目前下注?周期
                                    string CurrentBetsCycle = (cycle_2 - 1).ToString();
                                    int test = LastBets;
                                    string aaaa = GameKind + GameCycle + GamePlanName + GameType;
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(LastBets));
                                    //lblPlanWinOpp.Text = WinOpp.ToString("0.00");
                                    string insertInfo = GameKind + "," + GameDirect + "," + GamePlanName + ", " + GameCycle + "," + GameType;
                                    InsertIntoGod(insertInfo, WinOpp.ToString("0.00"));
                                    #endregion
                                }
                            }
                            else if (GameKind == "前三") //&& (cbGameCycle.Text == "三期一周" || cbGameCycle.Text == "二期一周")
                            {

                                if (GameDirect == "复式")
                                {

                                }
                                else if (GameDirect == "单式")
                                {
                                    int hisArr = 0;
                                    int sumWin = 0;
                                    int LastBets = 0;

                                    #region 驗證是否中奖
                                    bool isWin = false; //中了沒
                                    int periodtWin = 0; //第幾期中
                                    string[] temp = { "", "", "" }; //存放combobox的值

                                    for (int iii = 0; iii < jArrHistoryNumber.Count(); iii++) //從歷史結果開始比
                                    {
                                        //reset
                                        isWin = false;
                                        periodtWin = 0;
                                        temp[0] = "";
                                        temp[1] = "";
                                        temp[2] = "";

                                        for (int j = 0; j < iGameCycle; j++)
                                        {
                                            if (iii >= jArrHistoryNumber.Count()) break;

                                            string strMatch = "";
                                            switch (GameKind)
                                            {
                                                case "五星":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "");
                                                    break;
                                                case "四星":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 4);
                                                    break;
                                                case "前三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 3);
                                                    break;
                                                case "中三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(1, 3);
                                                    break;
                                                case "后三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(2, 3);
                                                    break;
                                                case "前二":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 2);
                                                    break;
                                                case "后二":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(3, 2);
                                                    break;
                                            }
                                            if (isWin == false) //還沒中
                                            {
                                                ///////////////cycle_2 - 1
                                                if (NowAnalyzeNumberArr[hisArr].ToString().IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                }
                                                else //挂
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 挂";
                                                }
                                                sumBets++;
                                                periodtWin = j + 1;
                                            }
                                            else //前面已中奖
                                            {
                                                temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 停";
                                                //cycle_2++;
                                            }
                                            iii++;
                                        }

                                        cycle_2++;
                                        iii--;
                                        hisArr++;

                                        Label lbl_2 = new Label();
                                        lbl_2.Text = periodtWin.ToString();
                                        lbl_2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                                        lbl_2.Padding = new System.Windows.Forms.Padding(20, 6, 20, 6);
                                        lbl_2.Size = new System.Drawing.Size(53, 25);
                                        lbl_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                                        LastBets += Convert.ToInt32(lbl_2.Text);
                                        ComboBox cb_1 = new ComboBox();
                                        for (int k = 0; k < 3; k++)
                                        {
                                            if (temp[k] != "")
                                                cb_1.Items.Add(temp[k]);
                                        }

                                        if (isWin == true)
                                        {
                                            sumWin++;
                                        }
                                    }

                                    #endregion

                                    #region 計算
                                    //每期注數 共?元
                                    //lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString("0.000");
                                    //目前下注?周期
                                    string CurrentBetsCycle = (cycle_2 - 1).ToString();
                                    int test = LastBets;
                                    string aaaa = GameKind + GameCycle + GamePlanName + GameType;
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(LastBets));
                                    //lblPlanWinOpp.Text = WinOpp.ToString("0.00");
                                    string insertInfo = GameKind + "," + GameDirect + "," + GamePlanName + ", " + GameCycle + "," + GameType;
                                    InsertIntoGod(insertInfo, WinOpp.ToString("0.00"));
                                    #endregion
                                }
                            }
                            else if (GameKind == "四星") //&& (cbGameCycle.Text == "三期一周" || cbGameCycle.Text == "二期一周")
                            {

                                if (GameDirect == "复式")
                                {

                                }
                                else if (GameDirect == "单式")
                                {
                                    int hisArr = 0;
                                    int sumWin = 0;
                                    int LastBets = 0;

                                    #region 驗證是否中奖
                                    bool isWin = false; //中了沒
                                    int periodtWin = 0; //第幾期中
                                    string[] temp = { "", "", "" }; //存放combobox的值

                                    for (int iii = 0; iii < jArrHistoryNumber.Count(); iii++) //從歷史結果開始比
                                    {
                                        //reset
                                        isWin = false;
                                        periodtWin = 0;
                                        temp[0] = "";
                                        temp[1] = "";
                                        temp[2] = "";

                                        for (int j = 0; j < iGameCycle; j++)
                                        {
                                            if (iii >= jArrHistoryNumber.Count()) break;

                                            string strMatch = "";
                                            switch (GameKind)
                                            {
                                                case "五星":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "");
                                                    break;
                                                case "四星":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 4);
                                                    break;
                                                case "前三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 3);
                                                    break;
                                                case "中三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(1, 3);
                                                    break;
                                                case "后三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(2, 3);
                                                    break;
                                                case "前二":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 2);
                                                    break;
                                                case "后二":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(3, 2);
                                                    break;
                                            }
                                            if (isWin == false) //還沒中
                                            {
                                                ///////////////cycle_2 - 1
                                                if (NowAnalyzeNumberArr[hisArr].ToString().IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                }
                                                else //挂
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 挂";
                                                }
                                                sumBets++;
                                                periodtWin = j + 1;
                                            }
                                            else //前面已中奖
                                            {
                                                temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 停";
                                                //cycle_2++;
                                            }
                                            iii++;
                                        }

                                        cycle_2++;
                                        iii--;
                                        hisArr++;

                                        Label lbl_2 = new Label();
                                        lbl_2.Text = periodtWin.ToString();
                                        lbl_2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                                        lbl_2.Padding = new System.Windows.Forms.Padding(20, 6, 20, 6);
                                        lbl_2.Size = new System.Drawing.Size(53, 25);
                                        lbl_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                                        LastBets += Convert.ToInt32(lbl_2.Text);
                                        ComboBox cb_1 = new ComboBox();
                                        for (int k = 0; k < 3; k++)
                                        {
                                            if (temp[k] != "")
                                                cb_1.Items.Add(temp[k]);
                                        }

                                        if (isWin == true)
                                        {
                                            sumWin++;
                                        }
                                    }

                                    #endregion

                                    #region 計算
                                    //每期注數 共?元
                                    //lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString("0.000");
                                    //目前下注?周期
                                    string CurrentBetsCycle = (cycle_2 - 1).ToString();
                                    int test = LastBets;
                                    string aaaa = GameKind + GameCycle + GamePlanName + GameType;
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(LastBets));
                                    //lblPlanWinOpp.Text = WinOpp.ToString("0.00");
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
                                    int hisArr = 0;
                                    int sumWin = 0;
                                    int LastBets = 0;

                                    #region 驗證是否中奖
                                    bool isWin = false; //中了沒
                                    int periodtWin = 0; //第幾期中
                                    string[] temp = { "", "", "" }; //存放combobox的值

                                    for (int iii = 0; iii < jArrHistoryNumber.Count(); iii++) //從歷史結果開始比
                                    {
                                        //reset
                                        isWin = false;
                                        periodtWin = 0;
                                        temp[0] = "";
                                        temp[1] = "";
                                        temp[2] = "";

                                        for (int j = 0; j < iGameCycle; j++)
                                        {
                                            if (iii >= jArrHistoryNumber.Count()) break;

                                            string strMatch = "";
                                            switch (GameKind)
                                            {
                                                case "五星":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "");
                                                    break;
                                                case "四星":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 4);
                                                    break;
                                                case "前三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 3);
                                                    break;
                                                case "中三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(1, 3);
                                                    break;
                                                case "后三":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(2, 3);
                                                    break;
                                                case "前二":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(0, 2);
                                                    break;
                                                case "后二":
                                                    strMatch = jArrHistoryNumber[iii]["Number"].ToString().Replace(",", "").Substring(3, 2);
                                                    break;
                                            }
                                            if (isWin == false) //還沒中
                                            {
                                                ///////////////cycle_2 - 1
                                                if (NowAnalyzeNumberArr[hisArr].ToString().IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                }
                                                else //挂
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 挂";
                                                }
                                                sumBets++;
                                                periodtWin = j + 1;
                                            }
                                            else //前面已中奖
                                            {
                                                temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 停";
                                                //cycle_2++;
                                            }
                                            iii++;
                                        }

                                        cycle_2++;
                                        iii--;
                                        hisArr++;

                                        Label lbl_2 = new Label();
                                        lbl_2.Text = periodtWin.ToString();
                                        lbl_2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                                        lbl_2.Padding = new System.Windows.Forms.Padding(20, 6, 20, 6);
                                        lbl_2.Size = new System.Drawing.Size(53, 25);
                                        lbl_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                                        LastBets += Convert.ToInt32(lbl_2.Text);
                                        ComboBox cb_1 = new ComboBox();
                                        for (int k = 0; k < 3; k++)
                                        {
                                            if (temp[k] != "")
                                                cb_1.Items.Add(temp[k]);
                                        }

                                        if (isWin == true)
                                        {
                                            sumWin++;
                                        }
                                    }

                                    #endregion

                                    #region 計算
                                    //每期注數 共?元
                                    //lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString("0.000");
                                    //目前下注?周期
                                    string CurrentBetsCycle = (cycle_2 - 1).ToString();
                                    int test = LastBets;
                                    string aaaa = GameKind + GameCycle + GamePlanName + GameType;
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(LastBets));
                                    //lblPlanWinOpp.Text = WinOpp.ToString("0.00");
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
        private void ConnectDbGetRandomNumber(string type, int PlanName, string GameCycle)
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";
            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = abc; Password=123456";
            con = new SqlConnection(connetionString);
            string date = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            try
            {
                if (GameCycle == "三期一周")
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
                else if (GameCycle == "二期一周")
                {
                    if (PlanName == 0)
                    {
                        con.Open();
                        string Sqlstr = @"SELECT top(60) number AS Number FROM RandomNumber WHERE date = '20180720' AND type = '{1}' ";
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
WHERE NUM >60 AND NUM <121";
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
                        string Sqlstr = @"SELECT top(60) number AS Number FROM RandomNumber WHERE date = '20180720' AND type = '{1}'";
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
                else
                {
                    if (PlanName == 0)
                    {
                        con.Open();
                        string Sqlstr = @"SELECT number AS Number FROM RandomNumber WHERE date = '20180720' AND type = '{1}' ";
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
                        string Sqlstr = @"SELECT number AS Number FROM RandomNumber WHERE date = '20180720' AND type = '{1}'";
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
                        string Sqlstr = @"SELECT number AS Number FROM RandomNumber WHERE date = '20180720' AND type = '{1}'";
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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        //取得歷史資料
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
                jArrHistoryNumber = ja;
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

        private void bkgStart_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgbShow.Value = e.ProgressPercentage;

            switch (pgbShow.Value)
            {
                case 0:
                    lbDoingDesc.Text = "载入资讯中...0%";
                    break;
                case 1:
                    lbDoingDesc.Text = "载入资讯中...20%";
                    break;
                case 2:
                    lbDoingDesc.Text = "载入资讯中...40%";
                    break;
                case 3:
                    lbDoingDesc.Text = "载入资讯中...60%";
                    break;
                case 4:
                    lbDoingDesc.Text = "载入资讯中...80%";
                    break;
                case 5:
                    lbDoingDesc.Text = "即将完成软件载入...100%";
                    break;
            }

        }

        private void bkgStart_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispose();
        }
    }
}
