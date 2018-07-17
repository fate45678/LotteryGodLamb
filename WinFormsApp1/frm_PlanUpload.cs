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
using System.Threading;
using System.Web;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace WinFormsApp1
{
    public partial class frm_PlanUpload : Form
    {
        string Winning = "";
        Connection con = new Connection();
        public static string globalNote = "";
        public frm_PlanUpload()
        {
            InitializeComponent();
            LogInHide();

            string NowDate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", @"/");
            label115.Text = NowDate + "历史开奖";
            //picAD4.Visible = false;
            pnlAD4.BorderStyle = BorderStyle.None;
            setRule();
        }
       
        List<string> dt_history = new List<string>();
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
                        rtxtHistory.Text += "第 " + frmGameMain.jArr[i]["Issue"].ToString() + " 期  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "\r\n";
                }
            }
            else //有資料先判斷
            {
                if ((rtxtHistory.Text.Substring(0, 11) != frmGameMain.jArr[0]["Issue"].ToString()) && (frmGameMain.strHistoryNumberOpen != "?")) //有新資料了
                {
                    rtxtHistory.Text = "";
                    for (int i = 0; i < frmGameMain.jArr.Count; i++)
                    {
                        //if (i == 120) break; //寫120筆就好
                        if (frmGameMain.jArr[i]["Issue"].ToString().Contains(date))
                            rtxtHistory.Text += "第 " + frmGameMain.jArr[i]["Issue"].ToString() + " 期  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "\r\n";
                    }
                }
            }
        }
        /// <summary>
        /// type 0 一般 1 照新增時間 2 照中奖機率
        /// </summary>
        /// <param name="type"></param>
        private void updatecheckboxlist1(int type)
        {           
            //找今天開獎的
            string NowDate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            var showJa = frmGameMain.jArr.Where(x => x["Issue"].ToString().Contains(NowDate)).ToList();
            int oldtotalWin = 0, oldtotalFail = 0, oldtotalPlay = 0, countWin = 0, countPlay = 0;

            //sql查詢欄位
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "p_id");
            dic.Add(1, "p_name");
            dic.Add(2, "p_start");
            dic.Add(3, "p_end");
            dic.Add(4, "p_rule");
            dic.Add(5, "p_uploadDate");
            //checklistboxitem datasource
            List<compentContent> content = new List<compentContent>();

            //先確認資料庫內有甚麼種類的玩法
            string SelectNowDate = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss").Substring(0, 10);
            Dictionary<int, string> dicGameKind = new Dictionary<int, string>();
            dicGameKind.Add(0, "p_name");

            var dtGameKindtmp = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select distinct p_name from Upplan where p_name LIKE '%"+ frm_PlanCycle.GameLotteryName+"%' AND p_uploadDate LIKE '" + SelectNowDate + "%' AND p_account = '" + frmGameMain.globalUserAccount + "' order by p_name", dicGameKind);

            System.Windows.Forms.Label lb11 = new System.Windows.Forms.Label();

            if (type == 0)
            {
                //string SelectNowDate = DateTime.Now.ToString().Substring(0, 9);
                Dictionary<int, string> dicGameKind0 = new Dictionary<int, string>();
                dicGameKind0.Add(0, "p_name");

                var dtGameKind = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select distinct p_name from Upplan where p_name LIKE '%" + frm_PlanCycle.GameLotteryName + "%' AND p_uploadDate LIKE '" + SelectNowDate + "%' AND p_account = '" + frmGameMain.globalUserAccount + "' order by p_name", dicGameKind0);

                for (int iKind = 0; iKind < dtGameKindtmp.Count; iKind++)
                {
                    oldtotalPlay = 0;
                    listBox1.Items.Clear();
                    var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_name = '" + dtGameKind.ElementAt(iKind) + "' AND p_uploadDate LIKE '" + SelectNowDate + "%' AND p_account = '" + frmGameMain.globalUserAccount + "' order by p_name, p_id desc", dic);
                    //dic.Clear();
                    if (dt.Count > 0)
                    {
                        oldtotalWin = 0; oldtotalFail = 0; //countPlay = 0;//

                        for (int i = 0; i < dt.Count; i = i + 6)
                        {                          
                            //玩法種類
                            string oldGameKind = dt.ElementAt(i+1);

                            //上傳的開始以及結束期數
                            long oldstart = Int64.Parse(dt.ElementAt(i + 2));
                            long oldend = Int64.Parse(dt.ElementAt(i + 3));

                            //上傳了幾期
                            long oldamount = (oldend - oldstart) + 1;

                            //總共中獎幾次 掛幾次 總共投了幾注


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
                                if (dt.ElementAt(i + 4).Contains(oldcheckNumber))
                                {
                                    oldtotalWin++;
                                    oldtotalPlay++;
                                    countWin += oldtotalWin;
                                    countPlay++;
                                    //oldtotalPlay = 0;
                                    break;
                                }
                                else
                                {
                                    oldtotalFail++;
                                    oldtotalPlay++;
                                    countPlay++;
                                }
                            }
                            //oldtotalPlay = oldtotalPlay - countPlay;
                        }

                        string winRate = "";

                        if (oldtotalPlay == 0)
                        {
                            winRate = "中奖率0%";
                        }

                        else
                        {
                            winRate = "中奖率" + (((double)(oldtotalWin) / (double)(oldtotalPlay)) *100).ToString("0.00") + "%";
                        }

                        content.Add(new compentContent
                        {
                            //重庆时时彩 前三直选单式 中奖率0％ 
                            //上传时间：20180609 11：12 
                            //已上传第20180609033期～20180609034期 
                            id = int.Parse(dt.ElementAt(0)),
                            value = "計畫序號: "+ dt.ElementAt(0) + ", "+ dt.ElementAt(1) + " " +
                             winRate
                              + " \r\n上传时间:" + dt.ElementAt(5)
                              + " \r\n已上传第" + dt.ElementAt(dt.Count-4) + "期 ～ 第" + dt.ElementAt(3) + "期"
                              //+ " ," + 
                        });

                        con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "Update Upplan set p_hits = '" + winRate.Replace("中奖率", "").Replace("%","") + "' WHERE p_id = '"+ int.Parse(dt.ElementAt(0)) + "'");
                    }
                }
            }
            else if (type == 1)
            {
                Dictionary<int, string> dicGameKind0 = new Dictionary<int, string>();
                dicGameKind0.Add(0, "p_name");

                var dtGameKind = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select distinct p_name, p_uploadDate from Upplan where p_name LIKE '%" + frm_PlanCycle.GameLotteryName + "%' AND p_uploadDate LIKE '" + SelectNowDate + "%' AND p_account = '" + frmGameMain.globalUserAccount + "' and [p_isoldplan] = '1' order by p_uploadDate desc", dicGameKind0);


                //order by p_uploadDate desc
                for (int iKind = 0; iKind < dtGameKind.Count; iKind++)
                {
                    oldtotalPlay = 0;
                    listBox1.Items.Clear();
                    var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_name = '" + dtGameKind.ElementAt(iKind) + "' AND p_uploadDate LIKE '" + SelectNowDate + "%' AND p_account = '" + frmGameMain.globalUserAccount + "' order by p_uploadDate desc", dic);
                    //dic.Clear();
                    if (dt.Count > 0)
                    {
                        oldtotalWin = 0; oldtotalFail = 0; //countPlay = 0;//

                        for (int i = 0; i < dt.Count; i = i + 6)
                        {
                            //玩法種類
                            string oldGameKind = dt.ElementAt(i + 1);

                            //上傳的開始以及結束期數
                            long oldstart = Int64.Parse(dt.ElementAt(i + 2));
                            long oldend = Int64.Parse(dt.ElementAt(i + 3));

                            //上傳了幾期
                            long oldamount = (oldend - oldstart) + 1;

                            //總共中獎幾次 掛幾次 總共投了幾注


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
                                if (dt.ElementAt(i + 4).Contains(oldcheckNumber))
                                {
                                    oldtotalWin++;
                                    oldtotalPlay++;
                                    countWin += oldtotalWin;
                                    countPlay++;
                                    //oldtotalPlay = 0;
                                    break;
                                }
                                else
                                {
                                    oldtotalFail++;
                                    oldtotalPlay++;
                                    countPlay++;
                                }
                            }
                            //oldtotalPlay = oldtotalPlay - countPlay;
                        }

                        string winRate = "";

                        if (oldtotalPlay == 0)
                        {
                            winRate = "中奖率0%";
                        }

                        else
                        {
                            winRate = "中奖率" + (((double)(oldtotalWin) / (double)(oldtotalPlay)) * 100).ToString("0.00") + "%";
                        }

                        content.Add(new compentContent
                        {
                            //重庆时时彩 前三直选单式 中奖率0％ 
                            //上传时间：20180609 11：12 
                            //已上传第20180609033期～20180609034期 
                            id = int.Parse(dt.ElementAt(0)),
                            value = "計畫序號: " + dt.ElementAt(0) + ", " + dt.ElementAt(1) + " " +
                             winRate
                              + " \r\n上传时间:" + dt.ElementAt(5)
                              + " \r\n已上传第" + dt.ElementAt(dt.Count - 4) + "期 ～ 第" + dt.ElementAt(3) + "期"
                              //+ " ," + dt.ElementAt(0)
                        });

                        //con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "Update FROM Upplan set p_hits = '" + winRate + "' WHERE p_id = '" + int.Parse(dt.ElementAt(0)) + "'");
                    }
                }
            }
            else if (type == 2)
            {
                Dictionary<int, string> dicGameKind0 = new Dictionary<int, string>();
                dicGameKind0.Add(0, "p_name");

                var dtGameKind = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select distinct p_name from Upplan where p_name LIKE '%" + frm_PlanCycle.GameLotteryName + "%' AND p_uploadDate LIKE '" + SelectNowDate + "%' AND p_account = '" + frmGameMain.globalUserAccount + "' order by p_name", dicGameKind0);

                for (int iKind = 0; iKind < dtGameKind.Count; iKind++)
                {

                    oldtotalPlay = 0;
                    listBox1.Items.Clear();
                    var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_name = '" + dtGameKind.ElementAt(iKind) + "' AND p_uploadDate LIKE '" + SelectNowDate + "%' AND p_account = '" + frmGameMain.globalUserAccount + "' order by p_uploadDate", dic);
                    //dic.Clear();
                    if (dt.Count > 0)
                    {
                        oldtotalWin = 0; oldtotalFail = 0; //countPlay = 0;//

                        for (int i = 0; i < dt.Count; i = i + 6)
                        {
                            //玩法種類
                            string oldGameKind = dt.ElementAt(i + 1);

                            //上傳的開始以及結束期數
                            long oldstart = Int64.Parse(dt.ElementAt(i + 2));
                            long oldend = Int64.Parse(dt.ElementAt(i + 3));

                            //上傳了幾期
                            long oldamount = (oldend - oldstart) + 1;

                            //總共中獎幾次 掛幾次 總共投了幾注


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
                                if (dt.ElementAt(i + 4).Contains(oldcheckNumber))
                                {
                                    oldtotalWin++;
                                    oldtotalPlay++;
                                    countWin += oldtotalWin;
                                    countPlay++;
                                    //oldtotalPlay = 0;
                                    break;
                                }
                                else
                                {
                                    oldtotalFail++;
                                    oldtotalPlay++;
                                    countPlay++;
                                }
                            }
                            //oldtotalPlay = oldtotalPlay - countPlay;
                        }

                        string winRate = "";

                        if (oldtotalPlay == 0)
                        {
                            winRate = "中奖率0%";
                        }

                        else
                        {
                            winRate = "中奖率" + (((double)(oldtotalWin) / (double)(oldtotalPlay)) * 100).ToString("0.00") + "%";
                        }

                        content.Add(new compentContent
                        {
                            //重庆时时彩 前三直选单式 中奖率0％ 
                            //上传时间：20180609 11：12 
                            //已上传第20180609033期～20180609034期 
                            id = int.Parse(dt.ElementAt(0)),
                            value = "計畫序號: " + dt.ElementAt(0) + ", " + dt.ElementAt(1) + " " +
                            winRate
                              + " \r\n上传时间:" + dt.ElementAt(5)
                              + " \r\n已上传第" + dt.ElementAt(dt.Count - 4) + "期 ～ 第" + dt.ElementAt(3) + "期",
                              //+ " ," + dt.ElementAt(0),
                            winRateContent = double.Parse(winRate.Replace("中奖率","").Replace("%",""))
                        });

                        //con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "Update FROM Upplan set p_hits = '" + winRate + "' WHERE p_id = '" + int.Parse(dt.ElementAt(0)) + "'");
                    }
                }

                var sortedList = content.OrderByDescending(x => x.winRateContent).ToList();
                content = sortedList;
            }


            //TimeCount = 30000;
            checkedListBoxEx1.DataSource = null;
            checkedListBoxEx1.DataSource = content;
            checkedListBoxEx1.ValueMember = "id";
            checkedListBoxEx1.DisplayMember = "value";
        }

        private string calhits(string start,string end, string number, string type)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0,"number");
            var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from HistoryNumber where issue between "+start+" and "+end, dic).ToArray();
            var st = Regex.Split(number.Replace("  ", " "), " ").ToArray();
            int sum = 0;
            int hits = 0;

            #region 玩法
            if (type.IndexOf("五星") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    sum++;
                    if (Array.IndexOf(st, dt[i]) > -1)//st[j] == dt[i]
                    {
                        hits++;
                        break;
                    }

                    //if (st.Length == 1)
                    //{
                    //    sum++;
                    //    if (st[0] == dt[i])
                    //    {
                    //        hits++;
                    //        break;
                    //    }
                    //}
                    //else
                    //{ 
                    //    for (int j = 0; j < st.Length - 1; j++)
                    //    {
                    //        sum++;
                    //        if (Array.IndexOf(st, dt[i]) > -1)//st[j] == dt[i]
                    //        { 
                    //            hits++;
                    //            break;
                    //        }
                    //    }
                    //}
                }
                #region 中奖率
                if (hits == 0)
                    return "中奖率0%";
                else
                {
                    double result = ((double)hits / dt.Count()) * 100;
                    Winning = "";
                    Winning = result.ToString("0.00");
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
            else if (type.IndexOf("四星") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    sum++;
                    if (Array.IndexOf(st, dt[i].Substring(0, 4)) > -1) //dt[i].Substring(1,4) == st[j].Substring(1, 4))
                    {
                        hits++;
                        break;
                    }
                    //for (int j = 0; j < st.Length - 1; j++)
                    //{
                    //    sum++;
                    //    if (Array.IndexOf(st, dt[i].Substring(0, 4)) > -1) //dt[i].Substring(1,4) == st[j].Substring(1, 4))
                    //    {
                    //        hits++;
                    //        break;
                    //    }
                    //}
                }
                #region 中奖率
                if (hits == 0)
                    return "中奖率0%";
                else
                {
                    double result = ((double)hits / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 5) + "%";
                    else
                        return result.ToString() + "%";
                }
                #endregion
            }
            else if (type.IndexOf("前三") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    sum++;
                    if (Array.IndexOf(st, dt[i].Substring(0, 3)) > -1)//dt[i].Substring(0, 3) == st[j].Substring(0, 3))
                    {
                        hits++;
                        break;
                    }
                    //for (int j = 0; j < st.Length - 1; j++)
                    //{
                    //    sum++;
                    //    if (Array.IndexOf(st, dt[i].Substring(0, 3)) > -1)//dt[i].Substring(0, 3) == st[j].Substring(0, 3))
                    //    {
                    //        hits++;
                    //        break;
                    //    }
                    //}
                }
                #region 中奖率
                if (hits == 0)
                    return "中奖率0%";
                else
                {
                    double result = ((double)hits / sum) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 5) + "%";
                    else
                        return result.ToString() + "%";
                }
                #endregion
            }
            else if (type.IndexOf("中三") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    sum++;
                    if (Array.IndexOf(st, dt[i].Substring(1, 3)) > -1)//dt[i].Substring(1, 3) == st[j].Substring(1, 3))
                    {
                        hits++;
                        break;
                    }
                    //for (int j = 0; j < st.Length - 1; j++)
                    //{
                    //    sum++;
                    //    if (Array.IndexOf(st, dt[i].Substring(1, 3)) > -1)//dt[i].Substring(1, 3) == st[j].Substring(1, 3))
                    //    {
                    //        hits++;
                    //        break;
                    //    }
                    //}
                }
                #region 中奖率
                if (hits == 0)
                    return "中奖率0%";
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
            else if (type.IndexOf("后三") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    sum++;
                    if (Array.IndexOf(st, dt[i].Substring(2, 3)) > -1)//dt[i].Substring(2, 3) == st[j].Substring(2, 3))
                    {
                        hits++;
                        break;
                    }

                    //for (int j = 0; j < st.Length - 1; j++)
                    //{
                    //    sum++;
                    //    if (Array.IndexOf(st, dt[i].Substring(2, 3)) > -1)//dt[i].Substring(2, 3) == st[j].Substring(2, 3))
                    //    {
                    //        hits++;
                    //        break;
                    //    }
                    //}
                }
                #region 中奖率
                if (hits == 0)
                    return "中奖率0%";
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
            else if (type.IndexOf("前二") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    sum++;
                    if (Array.IndexOf(st, dt[i].Substring(0, 2)) > -1)//dt[i].Substring(0, 2) == st[j].Substring(0, 2))
                    {
                        hits++;
                        break;
                    }
                    //for (int j = 0; j < st.Length - 1; j++)
                    //{
                    //    sum++;
                    //    if (Array.IndexOf(st, dt[i].Substring(0, 2)) > -1)//dt[i].Substring(0, 2) == st[j].Substring(0, 2))
                    //    {
                    //        hits++;
                    //        break;
                    //    }
                    //}
                }
                #region 中奖率
                if (hits == 0)
                    return "中奖率0%";
                else
                {
                    double result = ((double)hits / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 5) + "%";
                    else
                        return result.ToString() + "%";
                }
                #endregion
            }
            else if (type.IndexOf("后二") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    sum++;
                    if (Array.IndexOf(st, dt[i].Substring(3, 2)) > -1)//dt[i].Substring(3, 2) == st[j].Substring(3, 2))
                    {
                        hits++;
                        break;
                    }
                    //for (int j = 0; j < st.Length - 1; j++)
                    //{
                    //    sum++;
                    //    if (Array.IndexOf(st, dt[i].Substring(3, 2)) > -1)//dt[i].Substring(3, 2) == st[j].Substring(3, 2))
                    //    {
                    //        hits++;
                    //        break;
                    //    }
                    //}
                }
                #region 中奖率
                if (hits == 0)
                    return "中奖率0%";
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
            else if (type.IndexOf("定位胆") != -1 && type.IndexOf("萬")!=-1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    for (int j = 0; j < st.Length - 1; j++)
                    {
                        sum++;
                        if (dt[i].Substring(0,1) == st[j].Substring(0, 1))
                        {
                            hits++;
                            break;
                        }
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return "中奖率0%";
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
                    return "中奖率0%";
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
                    return "中奖率0%";
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
                    return "中奖率0%";
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
                    return "中奖率0%";
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

        private double calhitsRtndouble(string start, string end, string number, string type)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "number");
            var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from HistoryNumber where issue between " + start + " and " + end, dic);
            var st = Regex.Split(number, " ");
            int sum = 0;
            int hits = 0;
            if (type.IndexOf("五星") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    for (int j = 0; j < st.Length - 1; j++)
                    {
                        sum++;
                        if (dt[i] == st[j])
                            hits++;
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return double.Parse(result.ToString().Substring(0, 3));
                    else
                        return double.Parse(result.ToString());
                }
                #endregion
            }
            else if (type.IndexOf("四星") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    for (int j = 0; j < st.Length - 1; j++)
                    {
                        sum++;
                        if (dt[i].Substring(0, 4) == st[j].Substring(0, 4))
                            hits++;
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return double.Parse(result.ToString().Substring(0, 3));
                    else
                        return double.Parse(result.ToString());
                }
                #endregion
            }
            else if (type.IndexOf("前三") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    for (int j = 0; j < st.Length - 1; j++)
                    {
                        sum++;
                        if (dt[i].Substring(0, 3) == st[j].Substring(0, 3))
                            hits++;
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return double.Parse(result.ToString().Substring(0, 3));
                    else
                        return double.Parse(result.ToString());
                }
                #endregion
            }
            else if (type.IndexOf("中三") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    for (int j = 0; j < st.Length - 1; j++)
                    {
                        sum++;
                        if (dt[i].Substring(1, 3) == st[j].Substring(1, 3))
                            hits++;
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return double.Parse(result.ToString().Substring(0, 3));
                    else
                        return double.Parse(result.ToString());
                }
                #endregion
            }
            else if (type.IndexOf("后三") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    for (int j = 0; j < st.Length - 1; j++)
                    {
                        sum++;
                        if (dt[i].Substring(2, 3) == st[j].Substring(2, 3))
                            hits++;
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return double.Parse(result.ToString().Substring(0, 3));
                    else
                        return double.Parse(result.ToString());
                }
                #endregion
            }
            else if (type.IndexOf("前二") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    for (int j = 0; j < st.Length - 1; j++)
                    {
                        sum++;
                        if (dt[i].Substring(0, 2) == st[j].Substring(0, 2))
                            hits++;
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return double.Parse(result.ToString().Substring(0, 3));
                    else
                        return double.Parse(result.ToString());
                }
                #endregion
            }
            else if (type.IndexOf("后二") != -1)
            {
                for (int i = 0; i < dt.Count(); i++)
                {
                    for (int j = 0; j < st.Length - 1; j++)
                    {
                        sum++;
                        if (dt[i].Substring(3, 2) == st[j].Substring(3, 2))
                            hits++;
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return double.Parse(result.ToString().Substring(0, 3));
                    else
                        return double.Parse(result.ToString());
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
                            hits++;
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return double.Parse(result.ToString().Substring(0, 3));
                    else
                        return double.Parse(result.ToString());
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
                            hits++;
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return double.Parse(result.ToString().Substring(0, 3));
                    else
                        return double.Parse(result.ToString());
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
                            hits++;
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return double.Parse(result.ToString().Substring(0, 3));
                    else
                        return double.Parse(result.ToString());
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
                            hits++;
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return double.Parse(result.ToString().Substring(0, 3));
                    else
                        return double.Parse(result.ToString());
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
                            hits++;
                    }
                }
                #region 中奖率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits  / dt.Count()) * 100;
                    if (result.ToString().Length > 3)
                        return double.Parse(result.ToString().Substring(0, 3));
                    else
                        return double.Parse(result.ToString());
                }
                #endregion
            }
            return 0;

        }

        public void refreshInterface()
        {
            label4.Text = frmGameMain.globalUserName;
            if (loginButtonType == 0)
                button1.Text = "登入";
            else if(loginButtonType ==1)
                button1.Text = "登出";

            if (!string.IsNullOrEmpty(frmGameMain.globalUserAccount))
            {
                button8.Enabled = true;
                button9.Enabled = true;
            }
            else
            {
                button8.Enabled = false;
                button9.Enabled = false;
            }
        }
        class rule
        {
            public string data { get; set; }
        }
        class compentContent
        {
            public int id { get; set; }
            public string value { get; set; }
            public double winRateContent { get; set; }
        }

        List<rule> rule1 = new List<rule>();
        private void setRule()
        {
            rule1.Add(new rule { data = "\b" });
            rule1.Add(new rule { data = "1" });
            rule1.Add(new rule { data = "2" });
            rule1.Add(new rule { data = "3" });
            rule1.Add(new rule { data = "4" });
            rule1.Add(new rule { data = "5" });
            rule1.Add(new rule { data = "6" });
            rule1.Add(new rule { data = "7" });
            rule1.Add(new rule { data = "8" });
            rule1.Add(new rule { data = "9" });
            rule1.Add(new rule { data = "0" });
            rule1.Add(new rule { data = " " });
            rule1.Add(new rule { data = "," });

        }

        int temp = 0;
        private void filtercbItem(int current)
        {
            //string iiii = Items[current - 1].ToString();
            var dt_plantest = Items.Where(x => x.Key > current - 1).ToList();
            var dt_plan = Items.Where(x => x.Key > current - 1).ToList();
            var dt_cycle = Items.Where(x => x.Key > current - 1).ToList();
            cbGamePlan.DataSource = new BindingSource(dt_plan, null);
            comboBox1.DataSource = new BindingSource(dt_plan, null);
            cbGameCycle.DataSource = new BindingSource(dt_cycle, null);
            comboBox2.DataSource = new BindingSource(dt_cycle, null);
            //temp = current;

            //if (current > temp)
            //{
            //    var dt_plan = Items.Where(x => x.Key > current-1);
            //    var dt_cycle = Items.Where(x => x.Key > current-1);
            //    cbGamePlan.DataSource = new BindingSource(dt_plan, null);
            //    comboBox1.DataSource = new BindingSource(dt_plan, null);
            //    cbGameCycle.DataSource = new BindingSource(dt_cycle, null);
            //    comboBox2.DataSource = new BindingSource(dt_cycle, null);
            //    temp = current;
            //}
            //else //if (current == 120)
            //{
            //    var dt_plan = Items.Where(x => x.Key > current - 1);
            //    var dt_cycle = Items.Where(x => x.Key > current - 1);
            //    cbGamePlan.DataSource = new BindingSource(dt_plan, null);
            //    comboBox1.DataSource = new BindingSource(dt_plan, null);
            //    cbGameCycle.DataSource = new BindingSource(dt_cycle, null);
            //    comboBox2.DataSource = new BindingSource(dt_cycle, null);
            //    temp = current;
            //}
        }
        Dictionary<int, string> Items = new Dictionary<int, string>();
        /// <summary>
        /// 初始化combobx
        /// </summary>
        private void InitcbItem()
        {
            if (!string.IsNullOrEmpty(frmGameMain.globalGetCurrentPeriod) || isFirstTime)
            {
                Items = new Dictionary<int, string>();
                cbGameCycle.DataSource = null;
                cbGameCycle.Items.Clear();//不知道怎麼把預設的選項清掉
                cbGamePlan.DataSource = null;
                cbGamePlan.Items.Clear();
                comboBox1.DataSource = null;
                comboBox1.Items.Clear();
                comboBox2.DataSource = null;
                comboBox2.Items.Clear();

                if (frm_PlanCycle.GameLotteryName == "重庆时时彩")
                { 
                    for (int i = 1; i < 121; i++)
                    {
                        if (i < 10)
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "00" + i.ToString());
                        else if (i > 9 && i < 100)
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "0" + i.ToString());
                        else
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + i.ToString());
                      
                        //cbGamePlan.DisplayMember = "Value";
                        //cbGamePlan.ValueMember = "Key";
                        //cbGameCycle.DisplayMember = "Value";
                        //cbGameCycle.ValueMember = "Key";
                        //comboBox1.DisplayMember = "Value";
                        //comboBox1.ValueMember = "Key";
                        //comboBox2.DisplayMember = "Value";
                        //comboBox2.ValueMember = "Key";
                        //cbGamePlan.DataSource = new BindingSource(Items, null);
                        //cbGameCycle.DataSource = new BindingSource(Items, null);
                        //comboBox1.DataSource = new BindingSource(Items, null);
                        //comboBox2.DataSource = new BindingSource(Items, null);
                    }
                }
                else if(frm_PlanCycle.GameLotteryName == "腾讯奇趣彩")
                {
                    for (int i = 1; i < 1441; i++)
                    {
                        if (i < 10)
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "000" + i.ToString());
                        else if (i > 9 && i < 100)
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "00" + i.ToString());
                        else if (i > 100 && i < 1000)
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "0" + i.ToString());
                        else
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + i.ToString());

                    }
                }
                else if (frm_PlanCycle.GameLotteryName == "腾讯官方彩")
                {
                    for (int i = 1; i < 1441; i++)
                    {
                        if (i < 10)
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "000" + i.ToString());
                        else if (i > 9 && i < 100)
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "00" + i.ToString());
                        else if (i > 100 && i < 1000)
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "0" + i.ToString());
                        else
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + i.ToString());
                    }   
                }
                else if (frm_PlanCycle.GameLotteryName == "天津时时彩")
                {
                    for (int i = 1; i < 85; i++)
                    {
                        if (i < 10)
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "00" + i.ToString());
                        else if (i > 9 && i < 100)
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "0" + i.ToString());
                        else
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + i.ToString());

                        //cbGamePlan.DisplayMember = "Value";
                        //cbGamePlan.ValueMember = "Key";
                        //cbGameCycle.DisplayMember = "Value";
                        //cbGameCycle.ValueMember = "Key";
                        //comboBox1.DisplayMember = "Value";
                        //comboBox1.ValueMember = "Key";
                        //comboBox2.DisplayMember = "Value";
                        //comboBox2.ValueMember = "Key";
                        //cbGamePlan.DataSource = new BindingSource(Items, null);
                        //cbGameCycle.DataSource = new BindingSource(Items, null);
                        //comboBox1.DataSource = new BindingSource(Items, null);
                        //comboBox2.DataSource = new BindingSource(Items, null);
                    }
                }
                else if(frm_PlanCycle.GameLotteryName == "新疆时时彩")
                {
                    for (int i = 1; i < 97; i++)
                    {
                        if (i < 10)
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "00" + i.ToString());
                        else if (i > 9 && i < 100)
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "0" + i.ToString());
                        else
                            Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + i.ToString());
                    }
                }

                var test = Items.ToList();
                DataTable dt = ConvertToDataTable(test);

                cbGamePlan.DisplayMember = "Value";
                cbGamePlan.ValueMember = "Key";
                cbGameCycle.DisplayMember = "Value";
                cbGameCycle.ValueMember = "Key";
                comboBox1.DisplayMember = "Value";
                comboBox1.ValueMember = "Key";
                comboBox2.DisplayMember = "Value";
                comboBox2.ValueMember = "Key";
                cbGamePlan.DataSource = dt;
                cbGameCycle.DataSource = dt;
                comboBox1.DataSource = dt;
                comboBox2.DataSource = dt;

                isFirstTime = false;
            }
        }
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
        /// <summary>
        /// 計算共幾注
        /// </summary>
        /// <returns></returns>
        private int calPeriod()
        {
            if ((int)cbGamePlan.SelectedValue != 120)
                return ((int)cbGameCycle.SelectedValue - (int)cbGamePlan.SelectedValue) + 1;
            else return 0;
        }
       /// <summary>
       /// 更新label24
       /// </summary>
        private void updateLabel24()
        {
            label24.Text = frm_PlanCycle.GameLotteryName + (string)cbGameKind.SelectedItem + (string)cbGameDirect.SelectedItem;
        }
        /// <summary>
        /// 檢查資料庫是否有歷史號碼,沒有的話新增
        /// </summary>
        public void addNCheckHistoryNumber(int i)
        {
                string st = frmGameMain.jArr[i]["Issue"].ToString();
                var dt = con.ConSQLtoList4cb("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select issue as 'name' from HistoryNumber where issue = '"+ st + "'");
                if (dt.Count == 0)//沒找到期數
                {
                    con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "Insert into HistoryNumber(issue, number) values('"+ st + "','"+ frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "')");
                }
        }
      
        #region UI事件
        public static int loginButtonType = 0;
        string clickUrl = "";
        /// <summary>
        /// 登入按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (loginButtonType == 0)
            {
                //抓廣告
                loadUrlAdPicture();

                frm_Login frm_login = new frm_Login();
                frm_login.ShowDialog();
                frm_LoadingControl frm_LoadingControl = new frm_LoadingControl();
                frm_LoadingControl.Show();
                Application.DoEvents();
                updatecheckboxlist1(0);
                frm_LoadingControl.Close();

                label16.Text = frm_PlanCycle.GameLotteryName + cbGameKind.Text + cbGameDirect.Text;
                label24.Text = label16.Text;
                //backgroundWorker1.RunWorkerAsync();

            }
            else if (loginButtonType == 1)
            {
                if (!string.IsNullOrEmpty(label4.Text))
                {
                    frmGameMain.globalUserName = "";
                    frmGameMain.globalUserAccount = "";
                    label4.Text = "";
                    //LogoutClear();
                    MessageBox.Show("登出成功。軟件即將關閉");
                    Application.Exit();
                    frm_PlanAgent.resetFavoriteFlag();
                    button1.Text = "登入";
                    loginButtonType = 0;
                    frmGameMain.globalMessageTemp = "";
                }
            }
        }

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

                string Url = string.Format("http://43.252.208.201:81/Upload/{0}/2/2.jpg", User);
                var request = WebRequest.Create(Url);
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    picAD4.Image = Bitmap.FromStream(stream);
                    picAD4.Click += new EventHandler(pic_Click);
                }
            }
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
                MessageBox.Show(ex.ToString());
                return null;
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

        private void LogInHide()
        {
            pnlNewPlan.Visible = false;
            pnlSentConti.Visible = false;
            pnlSent.Visible = false;
            panel2.Visible = false;
            pictureBox1.Visible = true;
            //lbLoginDesc.Visible = true;
        }

        private void LogInShow()
        {
            pnlNewPlan.Visible = true;
            pnlSentConti.Visible = true;
            pnlSent.Visible = true;
            panel2.Visible = true;
            pictureBox1.Visible = false;
            //lbLoginDesc.Visible = false;
        }
        /// <summary>
        /// 註冊按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewResult_Click(object sender, EventArgs e)
        {
            frm_Register register = new frm_Register();
            register.Owner = this;
            register.Show();
            return;
        }

        public static bool isFirst = true;
        public static bool isFirstTime = true;
        public static bool isChangeLotteryName = false;
        int updateCount = 0;
        int TimeCount = 600000;
        /// <summary>
        /// 計時器tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            refreshInterface();

            if (loginButtonType == 1)
                LogInShow();

            if (isFirstTime)
            {
                if (cbGameKind.SelectedIndex == -1)
                    cbGameKind.SelectedIndex = 0;
                if (cbGameDirect.SelectedIndex == -1)
                    cbGameDirect.SelectedIndex = 0;
                InitcbItem();//初始化combobox

            }
            
            frm_PlanCycle frm_PlanCycle = new frm_PlanCycle();
            //string iiii = frmGameMain.globalGetCurrentPeriod;
            if (frm_PlanCycle.GameLotteryName == "重庆时时彩")
            {
                filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 3, 3)));
                label2.Text = "共" + 1 + "期";
                label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + 1 + "期";
                //label2.Text = "共" + calPeriod() + "期";
                //label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + calPeriod() + "期";
            }
            else if (frm_PlanCycle.GameLotteryName == "腾讯奇趣彩")
            {
                filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 4, 4)));            
                label2.Text = "共" + 1 + "期";
                label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + 1 + "期";
            }
            else if (frm_PlanCycle.GameLotteryName == "腾讯官方彩")
            {
                filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 4, 4)));
                label2.Text = "共" + 1 + "期";
                label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + 1 + "期";
            }
            else if (frm_PlanCycle.GameLotteryName == "天津时时彩")
            {
                filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 3, 3)));
                label2.Text = "共" + 1 + "期";
                label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + 1 + "期";
            }
            else if (frm_PlanCycle.GameLotteryName == "新疆时时彩")
            {
                filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 3, 3)));
                label2.Text = "共" + 1 + "期";
                label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + 1 + "期";
            }
            //if (updateCount % 3 == 0 )//&& allorwUpdate
            //{
            //    updatecheckboxlist1(updateLstbType);
            //    //isFirst = true;
            //}

            updateCount++;

            if(!isFirst)
            {
                UpdateHistory();
                timer1.Interval = TimeCount;
                updatecheckboxlist1(0);
                //backgroundWorker1.CancelAsync();
            }
        }

        /// <summary>
        /// 號碼組合視窗限制輸入字元
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            for (int i = 0; i < rule1.Count(); i++)
            {
                if (rule1.ElementAt(i).data == e.KeyChar.ToString())
                    return;
            }
            e.Handled = true;
        }
        /// <summary>
        /// 上傳計畫功能事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            checkdataTest("A");
            if (string.IsNullOrEmpty(label4.Text))
                MessageBox.Show("尚未登入。");
            else
            {
                if (richTextBox2.Text.Trim() == "")
                {
                    MessageBox.Show("请输入号码。");
                    return;
                }

                string NowIssue = frmGameMain.jArr.First()["Issue"].ToString();
                if (double.Parse(NowIssue) >= double.Parse(cbGamePlan.Text))
                {
                    MessageBox.Show(NowIssue + "本期已开出，请重新选择期数");
                    return;
                }
                //checkData("A");
                string Kind = cbGameKind.Text;
                //string NowDateInsert = DateTime.Now.ToString();
                string NowDateInsert = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss");
                string checkDate = NowDateInsert.Substring(0, 10);
                //檢查是否有重複的玩法
                Dictionary<int, string> dic = new Dictionary<int, string>();
                dic.Add(0, "p_name");
                dic.Add(1, "p_uploadDate");
                //string iiii = "select * from userData where account = '" + frmGameMain.globalUserAccount + "'";
                var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select p_name, p_uploadDate from Upplan where p_account = '" + frmGameMain.globalUserAccount +"' AND p_name LIKE '%" + frm_PlanCycle.GameLotteryName + Kind + "%' AND p_uploadDate LIKE '%"+ checkDate + "%'", dic);

                if (getData.Count > 0)
                {
                    MessageBox.Show( Kind + "重复制作，请用续传或删除原有计划并传新上传");
                    return;
                }
                frm_LoadingControl frm_LoadingControl = new frm_LoadingControl();
                frm_LoadingControl.Show();
                Application.DoEvents();
                string planName = label24.Text;
                con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "update Upplan set p_isoldplan = '2' where p_account = '" + frmGameMain.globalUserAccount + "' AND p_name LIKE '%" + frm_PlanCycle.GameLotteryName + Kind + "%' AND p_uploadDate LIKE '%" + checkDate + "%' ;Insert into Upplan(p_name, p_account, p_start, p_end, p_rule,p_curNum, p_note, p_uploadDate, p_isoldplan) values('" + label4.Text + planName + "','" + frmGameMain.globalUserAccount + "','" + cbGamePlan.Text + "','" + cbGameCycle.Text + "','" + richTextBox2.Text + "','0','" + frmGameMain.globalMessageTemp + "','" + NowDateInsert + "', '1')");
                MessageBox.Show("上傳成功。");
                updatecheckboxlist1(0);
                frm_LoadingControl.Close();
            }
            allorwUpdate = true;
        }
        private void CheckUploadIsOnly()
        {

        }

        /// <summary>
        /// 選擇計畫種類cb選項更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbGameKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateLabel24();
            if (cbGameKind.SelectedIndex == 7)
            {
                cbGameDirect.Items.Clear();
                cbGameDirect.Items.Add("萬");
                cbGameDirect.Items.Add("千");
                cbGameDirect.Items.Add("百");
                cbGameDirect.Items.Add("十");
                cbGameDirect.Items.Add("個");
                cbGameDirect.SelectedIndex = 0;

            }
            else
            {
                cbGameDirect.Items.Clear();
                cbGameDirect.Items.Add("单式");
                //cbGameDirect.Items.Add("复式");
                cbGameDirect.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 選擇計畫種類cb選項更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbGameDirect_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateLabel24();
        }
        /// <summary>
        /// 開始期數cb改變事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbGamePlan_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var dt_cycle = Items.Where(x => x.Key > (int)cbGamePlan.SelectedValue-1);
            cbGameCycle.DataSource = new BindingSource(dt_cycle, null);
            label2.Text = "共1期";
            label23.Text = "第" + cbGamePlan.Text + "期 ~ 第" + cbGameCycle.Text + "期 " + label2.Text;
            
        }

        private void checkdataTest(string type)
        {
            //1.先確認玩法
            string GameKind = "";
            if (type == "A")
                GameKind = cbGameKind.Text;
            else
                GameKind = label16.Text.Substring(label16.Text.Length - 4, 4);
            //string GameKind = cbGameKind.Text;
            int lenghCheck = 0;

            if (GameKind.Contains("二"))
            {
                lenghCheck = 2;
            }
            else if (GameKind.Contains("三"))
            {
                lenghCheck = 3;
            }
            else if (GameKind.Contains("四"))
            {
                lenghCheck = 4;
            }
            else if (GameKind.Contains("五"))
            {
                lenghCheck = 5;
            }


            string checkNumber = "";
            if(type == "A")
                checkNumber = richTextBox2.Text.Replace(",", "");
            else
                checkNumber = richTextBox1.Text.Replace(",", "");

            var checkTmp = checkNumber.Split(' ');

            var checkTmpWhere = checkTmp.Where(x => x.Length == lenghCheck).Distinct().ToArray();
            var checkTmpError = checkTmp.Where(x => x.Length != lenghCheck).Distinct().ToArray();

            if (checkTmp.Count() == checkTmpWhere.Count())
            {
                label21.Text = "共" + checkTmp.Count().ToString() + "注";
            }
            else if (checkTmpWhere.Count() != 0)
            {
                string CompletNumber = "";
                for (int i = 0; i < checkTmpWhere.Count(); i++)
                {
                    CompletNumber = CompletNumber + " " + checkTmpWhere[i];
                }

                if(type == "A")
                    richTextBox2.Text = CompletNumber.Substring(1);
                else
                    richTextBox1.Text = CompletNumber.Substring(1);
            }
            else
            {
                if (type == "A")
                    richTextBox2.Text = "";
                else
                    richTextBox1.Text = "";
            }
            label21.Text = "共" + checkTmpWhere.Count().ToString() + "注";

            string errorList = "";
            if (checkTmpError.Count() != 0)
            {
                for (int i = 0; i < checkTmpError.Count(); i++)
                {
                    errorList = errorList + "," + checkTmpError[i];
                }
                MessageBox.Show("已清除重复及错误资料。\n" + errorList);
            }
            else
            {
                MessageBox.Show("除错完成");
            }
           
        }

        private void checkData(string type)
        {
            //type A為一般上傳 type B為續傳上傳
            if (type == "A")
            {
                string[] temp;
                string data = "";
                if (richTextBox2.Text.IndexOf(" ") != -1 && richTextBox2.Text.IndexOf(",") != -1)
                    data = richTextBox2.Text.Replace(" ", ",");
                else if (richTextBox2.Text.IndexOf(" ") != -1)
                    data = richTextBox2.Text.Replace(" ", ",");
                else if (richTextBox2.Text.IndexOf(",") != -1)
                    data = richTextBox2.Text;
                temp = Regex.Split(data, ",");

                List<string> withoutEmpty = new List<string>();
                for (int i = 0; i < temp.Length; i++)
                {
                    if (!string.IsNullOrEmpty(temp[i]))
                        withoutEmpty.Add(temp[i]);
                }
                string[] yee = withoutEmpty.ToArray();
                string[] result = withoutEmpty.Distinct().ToArray();

                if (yee.Length > 0)
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        int count = 0;
                        for (int j = 0; j < yee.Length; j++)
                        {
                            if (result[i] == yee[j])
                                count++;
                            if (count > 1 && yee[j].Equals(result[i]))
                                yee[j] = yee[j] + "x";
                        }
                    }
                    richTextBox2.Text = "";
                    int labelCount = 0;
                    string errorList = "";
                    for (int i = 0; i < yee.Length; i++)
                    {
                        if (yee[i].IndexOf("x") == -1)
                        {
                            if (richTextboxRule == 5 && yee[i].Length == 5)
                            {
                                richTextBox2.Text += yee[i] + " ";
                                labelCount++;
                            }
                            else if (richTextboxRule == 4 && yee[i].Length == 4)
                            {
                                richTextBox2.Text += yee[i] + " ";
                                labelCount++;
                            }
                            else if (richTextboxRule == 3 && yee[i].Length == 3)
                            {
                                richTextBox2.Text += yee[i] + " ";
                                labelCount++;
                            }
                            else if (richTextboxRule == 2 && yee[i].Length == 2)
                            {
                                richTextBox2.Text += yee[i] + " ";
                                labelCount++;
                            }
                            else if (richTextboxRule == 1 && yee[i].Length == 1)
                            {
                                richTextBox2.Text += yee[i] + " ";
                                labelCount++;
                            }
                            else
                                errorList += yee[i] + ", ";

                        }
                        else
                            errorList += yee[i].Replace("x", "") + ", ";

                    }
                    MessageBox.Show("已清除重复及错误资料。\n" + errorList);
                    label21.Text = "共" + labelCount + "注";
                }
                else
                {
                    MessageBox.Show("请输入正确号码。\n 例如:12345 67890");
                    return;
                    //label21.Text = "共" + labelCount + "注";
                }
            }
            else
            {
                string[] temp;
                string data = "";
                if (richTextBox1.Text.IndexOf(" ") != -1 && richTextBox1.Text.IndexOf(",") != -1)
                    data = richTextBox1.Text.Replace(" ", ",");
                else if (richTextBox1.Text.IndexOf(" ") != -1)
                    data = richTextBox1.Text.Replace(" ", ",");
                else if (richTextBox1.Text.IndexOf(",") != -1)
                    data = richTextBox1.Text;
                temp = Regex.Split(data, ",");

                List<string> withoutEmpty = new List<string>();
                for (int i = 0; i < temp.Length; i++)
                {
                    if (!string.IsNullOrEmpty(temp[i]))
                        withoutEmpty.Add(temp[i]);
                }
                string[] yee = withoutEmpty.ToArray();
                string[] result = withoutEmpty.Distinct().ToArray();

                if (yee.Length > 0)
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        int count = 0;
                        for (int j = 0; j < yee.Length; j++)
                        {
                            if (result[i] == yee[j])
                                count++;
                            if (count > 1 && yee[j].Equals(result[i]))
                                yee[j] = yee[j] + "x";
                        }
                    }
                    richTextBox1.Text = "";
                    int labelCount = 0;
                    string errorList = "";
                    for (int i = 0; i < yee.Length; i++)
                    {
                        if (yee[i].IndexOf("x") == -1)
                        {
                            if (richTextboxRule == 5 && yee[i].Length == 5)
                            {
                                richTextBox1.Text += yee[i] + " ";
                                labelCount++;
                            }
                            else if (richTextboxRule == 4 && yee[i].Length == 4)
                            {
                                richTextBox1.Text += yee[i] + " ";
                                labelCount++;
                            }
                            else if (richTextboxRule == 3 && yee[i].Length == 3)
                            {
                                richTextBox1.Text += yee[i] + " ";
                                labelCount++;
                            }
                            else if (richTextboxRule == 2 && yee[i].Length == 2)
                            {
                                richTextBox1.Text += yee[i] + " ";
                                labelCount++;
                            }
                            else if (richTextboxRule == 1 && yee[i].Length == 1)
                            {
                                richTextBox1.Text += yee[i] + " ";
                                labelCount++;
                            }
                            else
                                errorList += yee[i] + ", ";

                        }
                        else
                            errorList += yee[i].Replace("x", "") + ", ";

                    }
                    MessageBox.Show("已清除重复及错误资料。\n" + errorList);
                    label21.Text = "共" + labelCount + "注";
                }
                else
                {
                    MessageBox.Show("请输入正确号码。\n 例如:12345 67890");
                    return;
                    //label21.Text = "共" + labelCount + "注";
                }
            }
           
        }
        /// <summary>
        /// 除錯按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            //checkData("A");
            checkdataTest("A");
        }

        private static void AppendText(System.Windows.Forms.RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
        /// <summary>
        /// 續傳區域清除功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
        }

        int beforeCount = 0;
        /// <summary>
        /// 新計畫制定區域textbox輸入內容更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            #endregion
        }


        int richTextboxRule = 5;
        /// <summary>
        /// 投注種類cb選項更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbGameKind_SelectionChangeCommitted(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            if (cbGameKind.SelectedIndex == 0)//五星
            {
                richTextboxRule = 5;
            }
            else if (cbGameKind.SelectedIndex == 1)//四星
            {
                richTextboxRule = 4;

            }
            else if (cbGameKind.SelectedIndex == 2 || cbGameKind.SelectedIndex == 3 || cbGameKind.SelectedIndex == 4)//三星
            {
                richTextboxRule = 3;
            }
            else if (cbGameKind.SelectedIndex == 5 || cbGameKind.SelectedIndex == 6)//二星
            {
                richTextboxRule = 2;
            }
            else //一星
            {
                richTextboxRule = 1;
            }
        }
        private void upodatelsbSentbyHitTimes()
        {
                //取得過去所有號碼
                Dictionary<int, string> dic_history = new Dictionary<int, string>();
                dic_history.Add(0, "number");
                var getHistory = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from HistoryNumber", dic_history);
                //取得上傳計畫 id 號碼
                Dictionary<int, string> dic_plan = new Dictionary<int, string>();
                dic_plan.Add(0, "p_name");
                dic_plan.Add(1, "p_rule");
                var getPlan = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_account ='"+frmGameMain.globalUserAccount+"'", dic_plan);
                //把 dic_plan 轉換成比較好操作的格式
                Dictionary<string, string> dic = new Dictionary<string, string>();
                for (int i = 0; i < getPlan.Count; i = i + 2)
                {
                    dic.Add(getPlan.ElementAt(i), getPlan.ElementAt(i + 1));
                }
                //統計擊中次數
                Dictionary<string, int> hitTimes = new Dictionary<string, int>();

                for (int i = 0; i < getPlan.Count / 2; i++)
                {
                    int temp = 0;
                    for (int j = 0; j < getHistory.Count; j++)
                    {
                        if (dic.ElementAt(i).Value.IndexOf(getHistory.ElementAt(j)) != -1)
                            temp++;
                    }
                    hitTimes.Add(dic.ElementAt(i).Key, temp);
                }

                //依照擊中次數加入checklistbox
                Dictionary<string, int> dic1_SortedByKey = hitTimes.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
                checkedListBoxEx1.Items.Clear();
                for (int i = 0; i < dic1_SortedByKey.Count(); i++)
                {
                    checkedListBoxEx1.Items.Add(dic1_SortedByKey.ElementAt(i).Key.ToString());
                }
           
        }
        private void lsbSent_SelectedIndexChanged(object sender, EventArgs e)
        {

            richTextBox1.Text = "";
        }
       
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           
            allorwUpdate = false;
            if (checkBox1.Checked)
            {
                for(int i = 0; i < checkedListBoxEx1.Items.Count; i++)
                    checkedListBoxEx1.SetItemChecked(i, true);
            }
            else if (!checkBox1.Checked)
            {
                for (int i = 0; i < checkedListBoxEx1.Items.Count; i++)
                    checkedListBoxEx1.SetItemChecked(i, false);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string pid = "";
            //var aaaa = checkedListBoxEx1.CheckedItems[0];
            //var a = (((WinFormsApp1.frm_PlanUpload.compentContent)checkedListBoxEx1.SelectedValue).id).ToString();
            //var ii = (((WinFormsApp1.frm_PlanUpload.compentContent)checkedListBoxEx1.CheckedItems[0]).id).ToString(); ;
            if (checkedListBoxEx1.CheckedItems.Count == 0)
            {
                MessageBox.Show("請選擇刪除的計畫");
                return;
            }
            string DeleteName = "";

            for (int i = 0; i < checkedListBoxEx1.CheckedItems.Count; i++)
            {
                pid = (((WinFormsApp1.frm_PlanUpload.compentContent)checkedListBoxEx1.CheckedItems[i]).id).ToString();
                DeleteName = checkDeleteName(pid);
                con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "delete from Upplan where p_name = '" + DeleteName + "'");
            }

            //foreach (object checkedItem in checkedListBoxEx1.CheckedItems)
            //{
            //    var iii = checkedItem;
            //    DeleteName = checkDeleteName(pid);
            //    con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "delete from Upplan where p_name = '" + DeleteName + "'");
            //}

            MessageBox.Show("刪除成功。");
            updatecheckboxlist1(0);
        }

        private string checkDeleteName(string pid)
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";
            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
            con = new SqlConnection(connetionString);

            //預設給四星五星
            string Sqlstr = @"SELECT p_name From [Upplan] where p_id ='" + pid + "'";

            try
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(Sqlstr, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                string Pname = dt.Rows[0]["p_name"].ToString();
                con.Close();
                return Pname;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //label16.Text = "";
            //label17.Text = "已上传:";
            //label15.Text = "共0注";
            richTextBox1.Text = "";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(label4.Text))
            {
                MessageBox.Show("請先登入。");
                return;
            }
            else if (richTextBox1.Text.Trim() == "")
            {
                MessageBox.Show("號碼不得為空。");
                return;
            }
            else if (comboBox1.Text.Substring(8) == "120")
            {
                MessageBox.Show("已经是最后一期了");
                return;
            }
            else
            {
                //檢查是否為最後一期
                if (frm_PlanCycle.GameLotteryName == "重庆时时彩" && comboBox1.Text.Substring(8) == "120")
                {
                    MessageBox.Show("已经是最后一期了");
                    return;
                }
                else if (frm_PlanCycle.GameLotteryName == "天津时时彩" && comboBox1.Text.Substring(8) == "84")
                {
                    MessageBox.Show("已经是最后一期了");
                    return;
                }
                else if (frm_PlanCycle.GameLotteryName == "新疆时时彩" && comboBox1.Text.Substring(8) == "96")
                {
                    MessageBox.Show("已经是最后一期了");
                    return;
                }
                else if ((frm_PlanCycle.GameLotteryName == "腾讯官方彩" || frm_PlanCycle.GameLotteryName == "腾讯奇趣彩") && comboBox1.Text.Substring(8) == "1440")
                {
                    MessageBox.Show("已经是最后一期了");
                    return;
                }

                checkdataTest("B");             

                if (richTextBox1.Text.Trim() == "")
                {
                    MessageBox.Show("號碼不得為空。");
                    return;
                }
                frm_LoadingControl frm_LoadingControl = new frm_LoadingControl();
                frm_LoadingControl.Show();
                Application.DoEvents();

                //string updatePid = checkedListBoxEx1.SelectedValue.ToString();
                string updatePid = this.checkedListBoxEx1.Text.Split(',')[0].Replace("計畫序號: ", "");
                //取得帳號
                Dictionary<int, string> dic = new Dictionary<int, string>();
                dic.Add(0, "account");
                //string iiii = "select * from userData where account = '" + frmGameMain.globalUserAccount + "'";
                var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from userData where account = '" + frmGameMain.globalUserAccount + "'", dic);
                if (getData.Count != 0)
                {
                    string PName = label16.Text;
                    string PNowDate = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss");
                    string PStart = comboBox1.Text;
                    //string PStart = label17.Text.Substring(label17.Text.IndexOf("2"), 11);

                    string PEnd = comboBox2.Text;
                    string PRule = richTextBox1.Text;
                    string PNote = frmGameMain.globalMessageTemp;//DELETE Upplan WHERE p_id = '{0}'
                    string cmd = string.Format("UPDATE Upplan SET p_isoldplan = '2' where p_id = '{7}';Insert INTO Upplan(p_name ,p_account ,p_start ,p_end ,p_rule,p_note ,p_uploadDate, p_isoldplan, p_curNum) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','1', '0')", PName, frmGameMain.globalUserAccount, PStart, PEnd, PRule, PNote, PNowDate, updatePid);
                    con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", cmd);
                    //string iiii = "Insert into Upplan(p_name ,p_account ,p_start ,p_end ,p_rule) values('" + label16.Text.Replace("續傳","") + "續傳" + "','" + getData.ElementAt(0) + "','" + comboBox1.Text + "','" + comboBox2.Text + "','" + richTextBox1.Text + "','" + NowDate + "')";
                    //con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "Insert into Upplan(p_name ,p_account ,p_start ,p_end ,p_rule,p_uploadDate) values('" + label16.Text.Replace("續傳", "") + "續傳" + "','" + getData.ElementAt(0) + "','" + comboBox1.Text + "','" + comboBox2.Text + "','" + richTextBox1.Text + "','" + NowDate + "')");
                    updatecheckboxlist1(0);


                }
                else
                    MessageBox.Show("該計畫帳號不存在。");

                frm_LoadingControl.Close();
            }
        }
        int updateLstbType = 0;
        private void button11_Click(object sender, EventArgs e)
        {
            updateLstbType = 1;
            allorwUpdate = false;
            updatecheckboxlist1(updateLstbType);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            updateLstbType = 2;
            allorwUpdate = false;
            updatecheckboxlist1(updateLstbType);
        }

        bool allorwUpdate = true;
        //int amount = 0;
        private void checkedListBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //找今天開獎的
            string NowDate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
            var showJa = frmGameMain.jArr.Where(x => x["Issue"].ToString().Contains(NowDate)).ToList();

            if (checkedListBoxEx1.DataSource == null) { return; }
            Dictionary<int, string> dic = new Dictionary<int, string>();

            dic.Add(0, "p_name");
            dic.Add(1, "p_account");
            dic.Add(2, "p_start");
            dic.Add(3, "p_end");
            dic.Add(4, "p_rule");
            dic.Add(5, "p_note");
            allorwUpdate = false;

            string SelectNowDate = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss").Substring(0, 10);

            var kind = checkedListBoxEx1.Text;
            var check = kind.Split(' ');
            string gameKind = " ";

            if (check.Length > 1)
            {
                gameKind = kind.Split(' ')[2].Substring(kind.Split(' ')[2].Length - 4, 4);
                //int FindWord = gameKind.IndexOf("");
                //gameKind = kind.Substring(gameKind.Length -4, 4);
            }
            //var abc = iii.ToString();

            int oldtotalWin = 0, oldtotalFail = 0, oldtotalPlay = 0, countWint = 0, countPlay = 0;

            listBox1.Items.Clear();
            //先處理舊資料
            var getOldData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan inner join userData on userData.account = Upplan.p_account WHERE p_name LIKE '%" + frm_PlanCycle.GameLotteryName + gameKind + "%' AND p_uploadDate LIKE '" + SelectNowDate + "%' AND p_isoldplan = '2' AND userData.name = '" + label4.Text + "' order by p_uploadDate , p_name", dic);
            for (int i = 0; i < getOldData.Count ; i = i+6)
            {
                oldtotalWin = 0;
                oldtotalFail = 0;
                countPlay = 0;
                //oldtotalPlay = 0;
                //玩法種類
                string oldGameKind = getOldData.ElementAt(i);

                //上傳的開始以及結束期數
                long oldstart = Int64.Parse(getOldData.ElementAt(i+2));
                long oldend = Int64.Parse(getOldData.ElementAt(i+3));

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
                    if (getOldData.ElementAt(i+4).Contains(oldcheckNumber))
                    {
                        oldtotalWin++;
                        oldtotalPlay++;
                        countWint += oldtotalWin;
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
                        listBox1.Items.Add(oldstart + " 到 " + oldend + " 掛(" + oldtotalFail + ")");
                    }
                }
            }
            //this
            //var iii = checkedListBoxEx1.SelectedValue;
            //var iiiii = ((WinFormsApp1.frm_PlanUpload.compentContent)checkedListBoxEx1.SelectedValue).id;

            string pid = "";
            pid = checkedListBoxEx1.SelectedValue.ToString();
            if (frm_PlanCycle.GameLotteryName == "重庆时时彩")
            {
                pid = checkedListBoxEx1.SelectedValue.ToString();
                if (pid.Contains("WinFormsApp1.frm_PlanUpload+compentContent"))
                {
                    pid = (((WinFormsApp1.frm_PlanUpload.compentContent)checkedListBoxEx1.SelectedValue).id).ToString();
                }
            }
            else if(pid.Contains("WinFormsApp1.frm_PlanUpload+compentContent"))
            {
                pid = (((WinFormsApp1.frm_PlanUpload.compentContent)checkedListBoxEx1.SelectedValue).id).ToString();
            }


            var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_id = '" + pid + "'", dic);
            label17.Text = "已上传: 第" + getData.ElementAt(2) + "~" + getData.ElementAt(3) + "期";

            for (int i = 0; i < getData.Count; i++)
            {
                if (i == 0)
                    label16.Text = getData.ElementAt(i).Substring(getData.ElementAt(i).IndexOf(" ") + 1);
                else if (i == 2){}
                else if (i == 4)
                {
                    richTextBox1.Text = getData.ElementAt(i).Substring(0, getData.ElementAt(i).Length);
                    string strReplace = getData.ElementAt(i).Replace(",", "");
                    int times = (Convert.ToInt32(getData.ElementAt(3).Substring(8, 3)) - Convert.ToInt32(getData.ElementAt(2).Substring(8, 3)));
                    label15.Text = "共" + times + "注";
                }
            }

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
                    var showIssue = showJa.Where(x => x["Issue"].ToString().Contains((start+i).ToString())).ToList();

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
                        listBox1.Items.Add(start + " 到 " + end + " 掛(" + totalFail + ")");
                    }
                }

                //note補上敘述
                listBox2.Items.Clear();
                if (getData.ElementAt(3).Substring(0, 8) != NowDate)
                {
                    amount = 0;
                }
                string WinRate = "";
                listBox2.Items.Add(getData.ElementAt(5));
                listBox2.Items.Add("已投注: " + (oldtotalPlay + totalPlay) + "期");
                listBox2.Items.Add("中奖: " + (totalWin + countWint) + "期");

                if (oldtotalPlay != 0)
                    WinRate = "中奖率" + (((double)(totalWin + countWint) / (double)(oldtotalPlay + totalPlay)) * 100).ToString("0.00") + "%";//"中獎率" + calhits(getData.ElementAt(2), getData.ElementAt(3), getData.ElementAt(4), getData.ElementAt(0)) ;
                else
                    WinRate = "中奖率0%";
               //if (totalWin == 0)
               //{
               //    WinRate = "中奖率0%";
               //}
               //else
               //{
               //    WinRate = "中獎率" + (((double)(totalWin + oldtotalWin) / (double)(oldtotalPlay + totalPlay)) * 100).ToString("0.00") + "%";//"中獎率" + calhits(getData.ElementAt(2), getData.ElementAt(3), getData.ElementAt(4), getData.ElementAt(0)) ;
               //    //Winning = "中奖率" + WinRate + "%";
               //}
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
                else if (getData.ElementAt(0).Contains("前二") || getData.ElementAt(0).Contains("后二"))//二星
                {
                    item = richTextBox1.Text.Replace(" ", "").Length / 2;
                }
                else //一星
                {
                    item = richTextBox1.Text.Replace(" ", "").Length / 1;
                }


                //todo都要修正中奖幾期ˊ
                label15.Text = "共" + item + "注 ";
            }
            else
            {
                MessageBox.Show("查無資料");
                return;
            }

            var showNowIssueAPI = frmGameMain.jArr.First["Issue"].ToString();
            var showNowIssue = getData.ElementAt(3).ToString();



            if (int.Parse(showNowIssue.Substring(8)) < int.Parse(showNowIssueAPI.Substring(8)))
            {
                comboBox1.DataSource = new BindingSource(Items.Where(x => x.Key > int.Parse(showNowIssueAPI.Substring(8))), null);
                comboBox2.DataSource = new BindingSource(Items.Where(x => x.Key > (int.Parse(showNowIssueAPI.Substring(8)))), null);
            }
            else if (showNowIssue.Substring(8) == "120" || showNowIssue.Substring(8) == "084" || showNowIssue.Substring(8) == "96" || showNowIssue.Substring(8) == "1440")
            {
                comboBox1.DataSource = new BindingSource(Items.Where(x => x.Key > int.Parse(showNowIssue.Substring(8)) - 1), null);
                comboBox2.DataSource = new BindingSource(Items.Where(x => x.Key > (int.Parse(showNowIssue.Substring(8))) - 1), null);
            }
            else
            {
                comboBox1.DataSource = new BindingSource(Items.Where(x => x.Key > int.Parse(showNowIssue.Substring(8))), null);
                comboBox2.DataSource = new BindingSource(Items.Where(x => x.Key > (int.Parse(showNowIssue.Substring(8)))), null);
            }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var dt_cycle = Items.Where(x => x.Key > (int)comboBox1.SelectedValue - 1);
            comboBox2.DataSource = new BindingSource(dt_cycle, null);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            frm_Note frm = new frm_Note();
            frm.Show();
        }

        public void button12_Click(object sender, EventArgs e)
        {

            label16.Text = frm_PlanCycle.GameLotteryName + cbGameKind.Text + cbGameDirect.Text;
            label24.Text = label16.Text;
            richTextBox1.Text = "";
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            frm_LoadingControl frm_LoadingControl = new frm_LoadingControl();
            frm_LoadingControl.Show();
            Application.DoEvents();
            UpdateHistory();
            updatecheckboxlist1(0);

            //順便更新combobox
            if (cbGameKind.SelectedIndex == -1)
                cbGameKind.SelectedIndex = 0;
            if (cbGameDirect.SelectedIndex == -1)
                cbGameDirect.SelectedIndex = 0;
            InitcbItem();
            if (frm_PlanCycle.GameLotteryName == "重庆时时彩")
            {
                filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 3, 3)));
                label2.Text = "共" + 1 + "期";
                label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + 1 + "期";
                //label2.Text = "共" + calPeriod() + "期";
                //label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + calPeriod() + "期";
            }
            else if (frm_PlanCycle.GameLotteryName == "腾讯奇趣彩")
            {
                filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 4, 4)));
                label2.Text = "共" + 1 + "期";
                label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + 1 + "期";
            }
            else if (frm_PlanCycle.GameLotteryName == "腾讯官方彩")
            {
                filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 4, 4)));
                label2.Text = "共" + 1 + "期";
                label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + 1 + "期";
            }
            else if (frm_PlanCycle.GameLotteryName == "天津时时彩")
            {
                //var iii = int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 3, 3));
                filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 3, 3)));
                label2.Text = "共" + 1 + "期";
                label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + 1 + "期";
            }
            else if (frm_PlanCycle.GameLotteryName == "新疆时时彩")
            {
                filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 3, 3)));
                label2.Text = "共" + 1 + "期";
                label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + 1 + "期";
            }
            frm_LoadingControl.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (checkedListBoxEx1.SelectedValue!=null)
            {
                frm_Note frm = new frm_Note((int)checkedListBoxEx1.SelectedValue);
                frm.ShowDialog();
                listBox2.Items[0] = globalNote;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public static string richBox1Number = "";
        private void button13_Click(object sender, EventArgs e)
        {
            //checkData("B");
            //修改一個視窗
            frm_editNumber edit = new frm_editNumber(richTextBox1.Text);
            edit.ShowDialog();
            if (richBox1Number != "")
                richTextBox1.Text = richBox1Number;
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //int index = this.listBox1.IndexFromPoint(e.Location);
            string name = this.listBox1.Text.Replace("到",",");
            if (name == "")
                return;
            string[] nameArr = name.Split(',');
            long start = Int64.Parse(nameArr[0].Trim());

            long end = 0;
            if (frm_PlanCycle.GameLotteryName == "重庆时时彩" || frm_PlanCycle.GameLotteryName == "天津时时彩" || frm_PlanCycle.GameLotteryName == "新疆时时彩")
                end = Int64.Parse(nameArr[1].Substring(1,11).Trim());
            else if (frm_PlanCycle.GameLotteryName == "腾讯奇趣彩" || frm_PlanCycle.GameLotteryName == "腾讯官方彩")
                end = Int64.Parse(nameArr[1].Substring(1, 12).Trim());

            string GameKind = label16.Text;

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
            var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_account = '" + frmGameMain.globalUserAccount + "' AND p_name LIKE '%" + frm_PlanCycle.GameLotteryName + GameKind + "%' ", dic);
            richTextBox1.Text = "";
            for (int i = 0; i < getData.Count(); i = i + 2)
            {
                if (name.Contains(getData.ElementAt(i + 1)))
                {
                    richTextBox1.Text = getData.ElementAt(i);
                    itemCount = richTextBox1.Text.Split(' ');
                    label15.Text = "共" + itemCount.Count().ToString() + "注";
                    break;
                }
                else if (i + 3 < getData.Count() && end < Int64.Parse(getData.ElementAt(i + 3)))
                {
                    richTextBox1.Text = getData.ElementAt(i + 2);
                    break;
                }

            }

        }

        private void richTextBox2_Leave(object sender, EventArgs e)
        {
            if (richTextBox2.Text.Trim() == "")
            {
                label21.Text = "共0注";
                return;
            }
            string Number = richTextBox2.Text.Replace(","," ") ;
            string[] NumberCount = Number.Split(' ');
            label21.Text = "共" + NumberCount.Count() + "注";
        }

        private string GameKindValue;
        public string gameKindValue
        {
            set
            {
                GameKindValue = value;
            }
        }
        private void btnCopyUp_Click(object sender, EventArgs e)
        {
            string CopyOn = System.Windows.Clipboard.GetText().Trim();
            int CopyCount = CopyOn.Replace(",", " ").Split(' ').Count();
            string CopyKind = CopyOn.Replace(",", " ").Split(' ')[0];

            if (CopyKind.Length == 2)
            {
                frm_TwoMessageShow frm_TwoMessageShow = new frm_TwoMessageShow();

                frm_TwoMessageShow.Owner = this;

                frm_TwoMessageShow.ShowDialog();

                if (GameKindValue == "FrontTwo")
                {
                    //設定
                    cbGameKind.Text = "前二";
                    //cbGameKind.SelectedIndex = 5;
                    richTextboxRule = 2;
                    label24.Text = frm_PlanCycle.GameLotteryName + cbGameKind.Text + cbGameDirect.Text;
                }
                else
                {
                    //設定
                    cbGameKind.Text = "后二";
                    //cbGameKind.SelectedIndex = 6;
                    richTextboxRule = 2;
                    label24.Text = frm_PlanCycle.GameLotteryName + cbGameKind.Text + cbGameDirect.Text;
                }
            }
            else if (CopyKind.Length == 3)
            {
                frm_ThreeMessageShow frm_ThreeMessageShow = new frm_ThreeMessageShow();

                frm_ThreeMessageShow.Owner = this;

                frm_ThreeMessageShow.ShowDialog();

                if (GameKindValue == "FrontThree")
                {
                    //設定
                    cbGameKind.Text = "前三";
                    cbGameKind.SelectedIndex = 2;
                    richTextboxRule = 3;
                    label24.Text = frm_PlanCycle.GameLotteryName + cbGameKind.Text + cbGameDirect.Text;
                }
                else if (GameKindValue == "MidThree")
                {
                    cbGameKind.Text = "中三";
                    cbGameKind.SelectedIndex = 3;
                    richTextboxRule = 3;
                    label24.Text = frm_PlanCycle.GameLotteryName + cbGameKind.Text + cbGameDirect.Text;
                }
                else
                {
                    cbGameKind.Text = "后三";
                    cbGameKind.SelectedIndex = 4;
                    richTextboxRule = 3;
                    label24.Text = frm_PlanCycle.GameLotteryName + cbGameKind.Text + cbGameDirect.Text;
                }
            }
            else if (CopyKind.Length == 4)
            {
                if (frm_PlanCycle.GameLotteryName == "腾讯官方彩" || frm_PlanCycle.GameLotteryName == "腾讯奇趣彩")
                {
                    MessageBox.Show(frm_PlanCycle.GameLotteryName + "没有四星玩法，请重新贴上正确号码");
                    return;
                }

                cbGameKind.Text = "四星";
                cbGameKind.SelectedIndex = 5;
                richTextboxRule = 4;
                label24.Text = frm_PlanCycle.GameLotteryName + cbGameKind.Text + cbGameDirect.Text;
            }
            else if (CopyKind.Length == 5)
            {
                if (frm_PlanCycle.GameLotteryName == "腾讯官方彩" || frm_PlanCycle.GameLotteryName == "腾讯奇趣彩")
                {
                    MessageBox.Show(frm_PlanCycle.GameLotteryName + "没有五星玩法，请重新贴上正确号码");
                    return;
                }

                cbGameKind.Text = "五星";
                cbGameKind.SelectedIndex = 6;
                richTextboxRule = 5;
                label24.Text = frm_PlanCycle.GameLotteryName + cbGameKind.Text + cbGameDirect.Text;
            }

            richTextBox2.Text = CopyOn.Replace(",", " ");
            label21.Text = "共" + CopyCount + "注";
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            UpdateHistory();
            //updatecheckboxlist1(updateLstbType);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {            
            //frm_LoadingControl.Show();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void cbGameCycle_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int backAmount = (int)cbGameCycle.SelectedValue;
            int frontAmount = (int)cbGamePlan.SelectedValue;

            label2.Text = "共" + ((backAmount - frontAmount)+ 1) + "期";

            label23.Text = "第" + cbGamePlan.Text + "期 ~ 第" + cbGameCycle.Text + " " + label2.Text;
        }

        private void timeCheckChange_Tick(object sender, EventArgs e)
        {

            if (isChangeLotteryName && loginButtonType == 1)
            {
                ////調整combobox
                if (frm_PlanCycle.GameLotteryName == "重庆时时彩")
                {
                    filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 3, 3)));
                    label2.Text = "共" + 1 + "期";
                    label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + 1 + "期";

                    cbGameKind.Items.Clear();
                    cbGameKind.Items.Add("前二");
                    cbGameKind.Items.Add("后二");
                    cbGameKind.Items.Add("前三");
                    cbGameKind.Items.Add("中三");
                    cbGameKind.Items.Add("后三");
                    cbGameKind.Items.Add("四星");
                    cbGameKind.Items.Add("五星");
                    //label2.Text = "共" + calPeriod() + "期";
                    //label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + calPeriod() + "期";
                }
                else if (frm_PlanCycle.GameLotteryName == "腾讯奇趣彩")
                {
                    filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 4, 4)));
                    label2.Text = "共" + 1 + "期";
                    label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + 1 + "期";

                    cbGameKind.Items.Clear();
                    cbGameKind.Items.Add("前二");
                    cbGameKind.Items.Add("后二");
                    cbGameKind.Items.Add("前三");
                    cbGameKind.Items.Add("中三");
                    cbGameKind.Items.Add("后三");
                }
                else if (frm_PlanCycle.GameLotteryName == "腾讯官方彩")
                {
                    filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 4, 4)));
                    label2.Text = "共" + 1 + "期";
                    label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + 1 + "期";

                    cbGameKind.Items.Clear();
                    cbGameKind.Items.Add("前二");
                    cbGameKind.Items.Add("后二");
                    cbGameKind.Items.Add("前三");
                    cbGameKind.Items.Add("中三");
                    cbGameKind.Items.Add("后三");
                }
                else if (frm_PlanCycle.GameLotteryName == "天津时时彩")
                {
                    filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 3, 3)));
                    label2.Text = "共" + 1 + "期";
                    label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + 1 + "期";

                    cbGameKind.Items.Clear();
                    cbGameKind.Items.Add("前二");
                    cbGameKind.Items.Add("后二");
                    cbGameKind.Items.Add("前三");
                    cbGameKind.Items.Add("中三");
                    cbGameKind.Items.Add("后三");
                    cbGameKind.Items.Add("四星");
                    cbGameKind.Items.Add("五星");
                    //label2.Text = "共" + calPeriod() + "期";
                    //label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + calPeriod() + "期";
                }
                else if (frm_PlanCycle.GameLotteryName == "新疆时时彩")
                {
                    filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 3, 3)));
                    label2.Text = "共" + 1 + "期";
                    label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + 1 + "期";

                    cbGameKind.Items.Clear();
                    cbGameKind.Items.Add("前二");
                    cbGameKind.Items.Add("后二");
                    cbGameKind.Items.Add("前三");
                    cbGameKind.Items.Add("中三");
                    cbGameKind.Items.Add("后三");
                    cbGameKind.Items.Add("四星");
                    cbGameKind.Items.Add("五星");
                    //label2.Text = "共" + calPeriod() + "期";
                    //label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + calPeriod() + "期";
                }
                

                frm_LoadingControl frm_LoadingControl = new frm_LoadingControl();
                frm_LoadingControl.Show();
                Application.DoEvents();
                frm_LoadingControl.Close();

                UpdateHistory();
                //refreshInterface();

                richTextBox2.Text = "";

                if (cbGameKind.SelectedIndex == -1)
                    cbGameKind.SelectedIndex = 0;
                if (cbGameDirect.SelectedIndex == -1)
                    cbGameDirect.SelectedIndex = 0;
                InitcbItem();//初始化combobox

                //frm_PlanCycle frm_PlanCycle = new frm_PlanCycle();

                label16.Text = frm_PlanCycle.GameLotteryName + "前二" + cbGameDirect.Text;
                label24.Text = label16.Text;

                updateCount++;                             

                richTextBox1.Text = "";
                listBox1.Items.Clear();
                listBox2.Items.Clear();

                //timer1.Interval = 500;
                //isFirst = false;


                cbGameKind.SelectedIndex = 0;

                
                isChangeLotteryName = false;
                updatecheckboxlist1(0);

            }

        }
    }
}
