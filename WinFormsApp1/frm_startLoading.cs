using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
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
            this.ControlBox = false;
            bkgStart.RunWorkerAsync();
            bkgStart.WorkerReportsProgress = true;//啟動回報進度
            pgbShow.Maximum = 7;//ProgressBar上限
            pgbShow.Minimum = 0;//ProgressBar下限
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
                            if (GameKind == "后二") //&& (cbGameCycle.Text == "三期一周" || cbGameCycle.Text == "二期一周")
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

                                    for (int iii = jArrHistoryNumber.Count() - 1; iii >= 0; iii--) //從歷史結果開始比
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
                                                if (numHistory[0].IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                    //if (ckWinToNextCycle.Checked == true) //中奖即进入下一周期                                    
                                                    //{
                                                    //    i--;
                                                    //    sumBets++;
                                                    //    periodtWin = j + 1;
                                                    //    break;
                                                    //}
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

                                        if (isWin == true)
                                        {
                                            sumWin++;
                                        }
                                    }

                                    #endregion

                                    #region 計算
                                    //每期注數 共?元
                                    //lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString(".###");
                                    //目前下注?周期
                                    string CurrentBetsCycle = (sumBets).ToString(); 
                                    //string CurrentBetsCycle = (sumBets).ToString(); 
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(CurrentBetsCycle));
                                    //lblPlanWinOpp.Text = WinOpp.ToString("0.00");
                                    string insertInfo = GameKind + "," + GameDirect + "," + GamePlanName + ", " + GameCycle + "," + GameType ;
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
                                    #region 驗證是否中奖

                                    bool isWin = false; //中了沒
                                    int periodtWin = 0; //第幾期中
                                    string[] temp = { "", "", "" }; //存放combobox的值

                                    for (int iii = jArrHistoryNumber.Count() - 1; iii >= 0; iii--) //從歷史結果開始比
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
                                                if (numHistory[0].IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                    //todo 變成中獎下一周期要使用
                                                    //if (ckWinToNextCycle.Checked == true) //中奖即进入下一周期                                    
                                                    //{
                                                    //    i--;
                                                    //    sumBets++;
                                                    //    periodtWin = j + 1;
                                                    //    break;
                                                    //}
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
                                    //每期注數 共?元
                                    //lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString(".###");
                                    //目前下注?周期
                                    string CurrentBetsCycle = (sumBets).ToString(); 
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(CurrentBetsCycle));
                                    //lblPlanWinOpp.Text = WinOpp.ToString("0.00");
                                    string insertInfo = GameKind + "," + GameDirect + "," + GamePlanName + ", " + GameCycle + "," + GameType ;
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
                                    #region 驗證是否中奖

                                    bool isWin = false; //中了沒
                                    int periodtWin = 0; //第幾期中
                                    string[] temp = { "", "", "" }; //存放combobox的值

                                    for (int iii = jArrHistoryNumber.Count() - 1; iii >= 0; iii--) //從歷史結果開始比
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
                                                if (numHistory[0].IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                    //todo 變成中獎下一周期要使用
                                                    //if (ckWinToNextCycle.Checked == true) //中奖即进入下一周期                                    
                                                    //{
                                                    //    i--;
                                                    //    sumBets++;
                                                    //    periodtWin = j + 1;
                                                    //    break;
                                                    //}
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
                                    //每期注數 共?元
                                    //lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString(".###");
                                    //目前下注?周期
                                    string CurrentBetsCycle = (sumBets).ToString(); 
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(CurrentBetsCycle));
                                    //lblPlanWinOpp.Text = WinOpp.ToString("0.00");
                                    string insertInfo = GameKind + "," + GameDirect + "," + GamePlanName + ", " + GameCycle + "," + GameType ;
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
                                    #region 驗證是否中奖

                                    bool isWin = false; //中了沒
                                    int periodtWin = 0; //第幾期中
                                    string[] temp = { "", "", "" }; //存放combobox的值

                                    for (int iii = jArrHistoryNumber.Count() - 1; iii >= 0; iii--) //從歷史結果開始比
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
                                                if (numHistory[0].IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                    //todo 變成中獎下一周期要使用
                                                    //if (ckWinToNextCycle.Checked == true) //中奖即进入下一周期                                    
                                                    //{
                                                    //    i--;
                                                    //    sumBets++;
                                                    //    periodtWin = j + 1;
                                                    //    break;
                                                    //}
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
                                    //每期注數 共?元
                                    //lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString(".###");
                                    //目前下注?周期
                                    string CurrentBetsCycle = (sumBets).ToString(); 
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(CurrentBetsCycle));
                                    //lblPlanWinOpp.Text = WinOpp.ToString("0.00");
                                    string insertInfo = GameKind + "," + GameDirect + "," + GamePlanName + ", " + GameCycle + "," + GameType ;
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
                                    #region 驗證是否中奖

                                    bool isWin = false; //中了沒
                                    int periodtWin = 0; //第幾期中
                                    string[] temp = { "", "", "" }; //存放combobox的值

                                    for (int iii = jArrHistoryNumber.Count() - 1; iii >= 0; iii--) //從歷史結果開始比
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
                                                if (numHistory[0].IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                    //todo 變成中獎下一周期要使用
                                                    //if (ckWinToNextCycle.Checked == true) //中奖即进入下一周期                                    
                                                    //{
                                                    //    i--;
                                                    //    sumBets++;
                                                    //    periodtWin = j + 1;
                                                    //    break;
                                                    //}
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
                                    //每期注數 共?元
                                    //lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString(".###");
                                    //目前下注?周期
                                    string CurrentBetsCycle = (sumBets).ToString(); 
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(CurrentBetsCycle));
                                    //lblPlanWinOpp.Text = WinOpp.ToString("0.00");
                                    string insertInfo = GameKind + "," + GameDirect + "," + GamePlanName + ", " + GameCycle + "," + GameType ;
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
                                    #region 驗證是否中奖

                                    bool isWin = false; //中了沒
                                    int periodtWin = 0; //第幾期中
                                    string[] temp = { "", "", "" }; //存放combobox的值

                                    for (int iii = jArrHistoryNumber.Count() - 1; iii >= 0; iii--) //從歷史結果開始比
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
                                                if (numHistory[0].IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                    //todo 變成中獎下一周期要使用
                                                    //if (ckWinToNextCycle.Checked == true) //中奖即进入下一周期                                    
                                                    //{
                                                    //    i--;
                                                    //    sumBets++;
                                                    //    periodtWin = j + 1;
                                                    //    break;
                                                    //}
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
                                    //每期注數 共?元
                                    //lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString(".###");
                                    //目前下注?周期
                                    string CurrentBetsCycle = (sumBets).ToString(); 
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(CurrentBetsCycle));
                                    //lblPlanWinOpp.Text = WinOpp.ToString("0.00");
                                    string insertInfo = GameKind + "," + GameDirect + "," + GamePlanName + ", " + GameCycle + "," + GameType ;
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

                                    for (int iii = jArrHistoryNumber.Count() - 1; iii >= 0; iii--) //從歷史結果開始比
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
                                                if (numHistory[0].IndexOf(strMatch) > -1) //中
                                                {
                                                    temp[j] = "  " + jArrHistoryNumber[iii]["Number"].ToString().Replace(",", " ") + " 中";
                                                    isWin = true;

                                                    //todo 變成中獎下一周期要使用
                                                    //if (ckWinToNextCycle.Checked == true) //中奖即进入下一周期                                    
                                                    //{
                                                    //    i--;
                                                    //    sumBets++;
                                                    //    periodtWin = j + 1;
                                                    //    break;
                                                    //}
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
                                    //每期注數 共?元
                                    //lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString(".###");
                                    //目前下注?周期
                                    string CurrentBetsCycle = (sumBets).ToString(); 
                                    //中奖率
                                    double WinOpp = (sumWin * 100 / Convert.ToDouble(CurrentBetsCycle));
                                    //lblPlanWinOpp.Text = WinOpp.ToString("0.00");
                                    string insertInfo = GameKind + "," + GameDirect + "," + GamePlanName + ", " + GameCycle + "," + GameType ;
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

        //取得歷史資料
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

        private void DeleteGod()
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";
            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
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
        }

        private void bkgStart_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispose();
        }
    }
}
