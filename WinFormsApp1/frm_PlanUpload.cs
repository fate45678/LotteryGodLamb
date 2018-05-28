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

namespace WinFormsApp1
{
    public partial class frm_PlanUpload : Form
    {
        string Winning = "";
        Connection con = new Connection();
        public frm_PlanUpload()
        {
            InitializeComponent();
            LogInHide();

            string NowDate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", @"/");
            label115.Text = NowDate + "历史开奖";
            picAD4.Visible = false;
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
                        rtxtHistory.Text += "第" + frmGameMain.jArr[i]["Issue"].ToString() + "期  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "\r\n";
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
                            rtxtHistory.Text += "第" + frmGameMain.jArr[i]["Issue"].ToString() + "期  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "\r\n";
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

            System.Windows.Forms.Label lb11 = new System.Windows.Forms.Label();

            string calhitsNumber = "";
            string NowDate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", ""); 

            if (type == 0)
            {
                var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_isoldplan = '1' AND p_account = '" + frmGameMain.globalUserAccount + "'", dic);
                dic.Clear();
                if (dt.Count > 0)
                {
                    for (int i = 0; i < dt.Count; i = i + 6)
                    {
                        if (dt.ElementAt(i + 2).Substring(0, 8) != NowDate)
                        {
                            calhitsNumber = "中奖率0%";
                        }
                        else 
                        {
                            calhitsNumber = calhits
                                         (
                                             dt.ElementAt(i + 2),
                                             dt.ElementAt(i + 3),
                                             dt.ElementAt(i + 4),
                                             dt.ElementAt(i + 1).Replace("重慶時時彩", "")
                                         );
                        }

                       content.Add(new compentContent {id= int.Parse(dt.ElementAt(i)),value= dt.ElementAt(i + 1).Replace("重慶時時彩","") + " " +
                            calhitsNumber 
                             + " 最新上傳時間 \r\n " + dt.ElementAt(i + 5).Substring(9)
                             + " 已上傳第" + dt.ElementAt(i + 2) + "期 - 第" + dt.ElementAt(i + 3) + "期"
                             + " ," + dt.ElementAt(i)
                       });                     
                    }                   
                }
            }
            else if (type == 1)
            {
                var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_isoldplan = '1' AND p_account = '" + frmGameMain.globalUserAccount + "' order by p_id desc", dic);
                dic.Clear();
                if (dt.Count > 0)
                {
                    for (int i = 0; i < dt.Count; i = i + 6)
                    {
                        if (dt.ElementAt(i + 2).Substring(0, 8) != NowDate)
                        {
                            calhitsNumber = "中奖率0%";
                        }
                        else
                        {
                            calhitsNumber = calhits
                                         (
                                             dt.ElementAt(i + 2),
                                             dt.ElementAt(i + 3),
                                             dt.ElementAt(i + 4),
                                             dt.ElementAt(i + 1).Replace("重慶時時彩", "")
                                         );
                        }

                        content.Add(new compentContent
                        {
                            id = int.Parse(dt.ElementAt(i)),
                            value = dt.ElementAt(i + 1).Replace("重慶時時彩", "") + " " +
                                        calhitsNumber
                                        + " 最新上傳時間 \r\n " + dt.ElementAt(i + 5).Substring(9)
                                        + " 已上傳第" + dt.ElementAt(i + 2) + "期 - 第" + dt.ElementAt(i + 3) + "期"
                                        + " ," + dt.ElementAt(i)
                        }
                        );
                    }
                }
            }
            else if (type == 2)
            {
                var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_isoldplan = '1' AND p_account = '" + frmGameMain.globalUserAccount + "' order by p_id desc", dic);
                dic.Clear();
                if (dt.Count > 0)
                {
                    Dictionary<string, double> repeat = new Dictionary<string, double>();
                    for (int i = 0; i < dt.Count; i = i + 6)
                    {
                        if (repeat.ContainsKey(dt.ElementAt(i + 1)))
                        {
                            repeat.Add(dt.ElementAt(i) + "_" + dt.ElementAt(i + 1).Replace("重慶時時彩", "") + "(" + i + ")", calhitsRtndouble
                            (
                                dt.ElementAt(i + 2),
                                dt.ElementAt(i + 3),
                                dt.ElementAt(i + 4),
                                dt.ElementAt(i + 1).Replace("重慶時時彩", "")
                            ));

                        }
                        else
                        {
                            repeat.Add(dt.ElementAt(i) + "_" + dt.ElementAt(i + 1).Replace("重慶時時彩", ""), calhitsRtndouble
                            (
                                dt.ElementAt(i + 2),
                                dt.ElementAt(i + 3),
                                dt.ElementAt(i + 4),
                                dt.ElementAt(i + 1).Replace("重慶時時彩", "")
                            ));

                        }
                    }
                    Dictionary<string, double> dic1_SortedByKey = new Dictionary<string, double>();

                    var dicSort = from objDic in repeat orderby objDic.Value descending select objDic;
                    foreach (KeyValuePair<string, double> kvp in dicSort)
                        content.Add(new compentContent
                            { id = int.Parse(kvp.Key.Substring(0, kvp.Key.IndexOf("_"))),
                            value = kvp.Key.Substring(kvp.Key.IndexOf("_"))+"   "+ kvp.Value+"%"
                        });
                }
            }

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
            if (current > temp)
            {
                var dt_plan = Items.Where(x => x.Key > current-1);
                var dt_cycle = Items.Where(x => x.Key > current);
                cbGamePlan.DataSource = new BindingSource(dt_plan, null);
                comboBox1.DataSource = new BindingSource(dt_plan, null);
                cbGameCycle.DataSource = new BindingSource(dt_cycle, null);
                comboBox2.DataSource = new BindingSource(dt_cycle, null);
                temp = current;
            }
            else if (current == 120)
            {
                var dt_plan = Items.Where(x => x.Key < 999 );
                var dt_cycle = Items.Where(x => x.Key >  1);
                cbGamePlan.DataSource = new BindingSource(dt_plan, null);
                comboBox1.DataSource = new BindingSource(dt_plan, null);
                cbGameCycle.DataSource = new BindingSource(dt_cycle, null);
                comboBox2.DataSource = new BindingSource(dt_cycle, null);
                temp = current;
            }
        }
        Dictionary<int, string> Items = new Dictionary<int, string>();
        /// <summary>
        /// 初始化combobx
        /// </summary>
        private void InitcbItem()
        {
            if (!string.IsNullOrEmpty(frmGameMain.globalGetCurrentPeriod))
            {
                cbGameCycle.Items.Clear();//不知道怎麼把預設的選項清掉
                for (int i = 1; i < 121; i++)
                {
                    if (i < 10)
                        Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "00" + i.ToString());
                    else if (i > 9 && i < 100)
                        Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + "0" + i.ToString());
                    else
                        Items.Add(i, frmGameMain.globalGetCurrentPeriod.Substring(0, 8) + i.ToString());

                    cbGamePlan.DisplayMember = "Value";
                    cbGamePlan.ValueMember = "Key";
                    cbGameCycle.DisplayMember = "Value";
                    cbGameCycle.ValueMember = "Key";
                    comboBox1.DisplayMember = "Value";
                    comboBox1.ValueMember = "Key";
                    comboBox2.DisplayMember = "Value";
                    comboBox2.ValueMember = "Key";
                    cbGamePlan.DataSource = new BindingSource(Items, null);
                    cbGameCycle.DataSource = new BindingSource(Items, null);
                    comboBox1.DataSource = new BindingSource(Items, null);
                    comboBox2.DataSource = new BindingSource(Items, null);
                }
                isFirstTime = false;
            }
        }
        /// <summary>
        /// 計算共幾注
        /// </summary>
        /// <returns></returns>
        private int calPeriod()
        {
            if ((int)cbGamePlan.SelectedValue != 120)
                return ((int)cbGameCycle.SelectedValue - (int)cbGamePlan.SelectedValue) +1;
            else return 0;
        }
       /// <summary>
       /// 更新label24
       /// </summary>
        private void updateLabel24()
        {
            label24.Text = "重庆时时彩  " + (string)cbGameKind.SelectedItem + (string)cbGameDirect.SelectedItem;
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
        /// <summary>
        /// 登入按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (loginButtonType == 0)
            {
                frm_Login frm_login = new frm_Login();
                //frm_login.di
                frm_login.Show();               
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

        private void LogInHide()
        {
            pnlNewPlan.Visible = false;
            pnlSentConti.Visible = false;
            pnlSent.Visible = false;
        }

        private void LogInShow()
        {
            pnlNewPlan.Visible = true;
            pnlSentConti.Visible = true;
            pnlSent.Visible = true;
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
        /// <summary>
        /// 廣告圖片點擊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picAD4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.cwl.gov.cn/");
        }

        bool isFirstTime = true;
        int updateCount = 0;
        /// <summary>
        /// 計時器tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //timer1.Interval = 30000;
            UpdateHistory();
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
            filtercbItem(int.Parse(frmGameMain.globalGetCurrentPeriod.Substring(frmGameMain.globalGetCurrentPeriod.Length - 3, 3)));
            label2.Text = "共" + calPeriod() + "期";
            label23.Text = cbGamePlan.Text + "~" + cbGameCycle.Text + " 共" + calPeriod() + "期";

            if (updateCount % 3 == 0)//&& allorwUpdate
                updatecheckboxlist1(updateLstbType);
            updateCount++;
            //timer1.Interval = 300000;

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
            if (string.IsNullOrEmpty(label4.Text))
                MessageBox.Show("尚未登入。");
            else
            {
                //checkData("A");
                string Kind = cbGameKind.Text;
                string NowDateInsert = DateTime.Now.ToString();
                //var aaaa = frmGameMain.globalMessageTemp;
                string planName = label24.Text.Replace("重庆时时彩  ", "");
                con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "update Upplan set p_isoldplan = '2' WHERE p_name LIKE '%" + Kind + "%' ;Insert into Upplan(p_name, p_account, p_start, p_end, p_rule,p_curNum, p_note, p_uploadDate, p_isoldplan) values('" + label4.Text + "重慶時時彩" + planName + "','" + frmGameMain.globalUserAccount + "','" + cbGamePlan.Text + "','" + cbGameCycle.Text + "','" + richTextBox2.Text + "','0','" + frmGameMain.globalMessageTemp + "','" + NowDateInsert + "', '1')");

                MessageBox.Show("上傳成功。");
                updatecheckboxlist1(0);
            }
            allorwUpdate = true;
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
                cbGameDirect.Items.Add("复式");
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
            var dt_cycle = Items.Where(x => x.Key > (int)cbGamePlan.SelectedValue);
            cbGameCycle.DataSource = new BindingSource(dt_cycle, null);
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
            checkData("A");
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
            foreach (object checkedItem in checkedListBoxEx1.CheckedItems)
                con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "delete from Upplan where p_id = "+ checkedListBoxEx1.SelectedValue);
            MessageBox.Show("刪除成功。");
            updatecheckboxlist1(0);
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
                MessageBox.Show("請先登入。");
            else
            {
               
                string updatePid = this.checkedListBoxEx1.Text.Split(',')[1];
                //取得帳號
                Dictionary<int, string> dic = new Dictionary<int, string>();
                dic.Add(0, "account");
                //string iiii = "select * from userData where account = '" + frmGameMain.globalUserAccount + "'";
                var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from userData where account = '" + frmGameMain.globalUserAccount + "'", dic);
                if (getData.Count != 0)
                {
                    string PName = label16.Text;
                    string PNowDate = DateTime.Now.ToString("");
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

            //amount = 0;
            if (checkedListBoxEx1.DataSource == null) { return; }
            Dictionary<int, string> dic = new Dictionary<int, string>();

            dic.Add(0, "p_name");
            dic.Add(1, "p_account");
            dic.Add(2, "p_start");
            dic.Add(3, "p_end");
            dic.Add(4, "p_rule");
            dic.Add(5, "p_note");
            allorwUpdate = false;

            listBox1.Items.Clear();
            //先處理舊資料
            var getOldData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan inner join userData on userData.account = Upplan.p_account where p_isoldplan = '2' AND userData.name = '" + label4.Text + "' order by p_uploadDate , p_name", dic);
            for (int i = 0; i < getOldData.Count ; i = i+6)
            {
                //玩法種類
                string oldGameKind = getOldData.ElementAt(i);

                //上傳的開始以及結束期數
                long oldstart = Int64.Parse(getOldData.ElementAt(i+2));
                long oldend = Int64.Parse(getOldData.ElementAt(i+3));

                //上傳了幾期
                long oldamount = (oldend - oldstart) + 1;

                //總共中獎幾次 掛幾次 總共投了幾注
                int oldtotalWin = 0, oldtotalFail = 0, oldtotalPlay = 0;

                string oldcheckNumber = "";

                bool OldisAllOpen = true;

                for (int ii = 0; ii < oldamount; ii++)
                {
                    var oldshowIssue = showJa.Where(x => x["Issue"].ToString().Contains((oldstart + ii).ToString())).ToList();

                    //表示還沒開獎
                    if (oldshowIssue.Count == 0)
                    {
                        listBox1.Items.Add(oldstart + " 到 " + oldend + " 尚未開獎(" + (oldamount - oldtotalPlay) + ")");
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
                        break;
                    }
                    else
                    {
                        oldtotalFail++;
                        oldtotalPlay++;
                    }
                }

                if (OldisAllOpen)
                {
                    if (oldtotalWin != 0)
                    {
                        listBox1.Items.Add(oldstart + " 到 " + oldend + " 中(" + oldtotalPlay + ")");
                    }
                    else
                    {
                        listBox1.Items.Add(oldstart + " 到 " + oldend + " 掛(" + oldtotalFail + ")");
                    }
                }
            }

            var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_id = '" + checkedListBoxEx1.SelectedValue + "'", dic);
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
                        listBox1.Items.Add(start + " 到 " + end + " 尚未開獎(" + (amount - totalPlay) + ")");
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

                //補上note敘述
                listBox2.Items.Clear();
                if (getData.ElementAt(3).Substring(0, 8) != NowDate)
                {
                    amount = 0;
                }
                string WinRate = "";
                listBox2.Items.Add(getData.ElementAt(5));
                listBox2.Items.Add("已投注: " + totalPlay + "期");
                listBox2.Items.Add("中奖: " + totalWin + "期");
                if (totalWin == 0)
                {
                    WinRate = "中奖率0%";
                }
                else
                {
                    WinRate = "中獎率" + (((double)totalWin / (double)totalPlay) * 100).ToString("0.00") + "%";//"中獎率" + calhits(getData.ElementAt(2), getData.ElementAt(3), getData.ElementAt(4), getData.ElementAt(0)) ;
                    //Winning = "中奖率" + WinRate + "%";
                }
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
                label15.Text = "共" + item + "注 ";
            }
            else
            {
                MessageBox.Show("查無資料");
                return;
            }
                /*
                //確認資料庫中紀錄的上傳開始期數以及結束期數
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
                string GameKind = label16.Text;

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
                                //amount++;
                                if (i % 2 == 0)
                                {
                                    listBox1.Items.Add(Int64.Parse(Issue) - 1 + " 到 " + Issue + " 中(" + win + ")");
                                    win = 0;
                                    fail = 0;
                                }
                            }
                            else if (win != 0) //如果第一個就中了 則只顯示中(1)
                            {
                                totalWin++;
                                listBox1.Items.Add(Int64.Parse(Issue) - 1 + " 到 " + Issue + " 中(" + win + ")");
                                //listBox1.Items.Add(Int64.Parse(Issue) - 1 + " 到 " + Issue + " 停");
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
                    //else if ((win != 0))
                    //{
                    //    if (i % 2 == 0)
                    //    {
                    //        listBox1.Items.Add(Int64.Parse(Issue) - 1 + " 到 " + Issue + " 停");
                    //    }
                        
                    //}
                    else //這裡開始表示尚未開獎
                    {
                        if (Int32.Parse(End.ToString().Substring(8)) <= i)
                        {
                            if (i % 2 == 0)
                            {
                                listBox1.Items.Add(Int64.Parse(Issue)-1 + " 到 " + (Int64.Parse(Issue)) + " 尚未開獎(1)");
                                break;
                            }
                        }
                        else if (i % 2 == 0)
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
                listBox2.Items.Clear();
                //long amount = Int64.Parse(getData.ElementAt(3)) - Int64.Parse(getData.ElementAt(2)) + 1;
                string iii = getData.ElementAt(3).Substring(0,8);
                if (getData.ElementAt(3).Substring(0, 8) != NowDate)
                {
                    amount = 0;
                }
                //long amount = Int64.Parse(getData.ElementAt(3)) - Int64.Parse(getData.ElementAt(2))+1;
                string WinRate = "";
                listBox2.Items.Add(getData.ElementAt(5));
                listBox2.Items.Add("已投注: " + amount + "期");
                listBox2.Items.Add("中奖: " + totalWin + "期");
                if (totalWin == 0)
                {
                    WinRate = "中奖率0%";
                }
                else
                {

                    WinRate = "中獎率" + (((double)totalWin / (double)amount) * 100).ToString("0.00") + "%";//"中獎率" + calhits(getData.ElementAt(2), getData.ElementAt(3), getData.ElementAt(4), getData.ElementAt(0)) ;
                    //Winning = "中奖率" + WinRate + "%";
                }
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
                label15.Text = "共" + item + "注 ";   
                */
                var showNowIssue = frmGameMain.jArr.First["Issue"].ToString();
            comboBox1.DataSource = new BindingSource(Items.Where(x => x.Key > int.Parse(showNowIssue.Substring(8))), null);
            comboBox2.DataSource = new BindingSource(Items.Where(x => x.Key > (int.Parse(showNowIssue.Substring(8))) + 1), null);
        }


        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var dt_cycle = Items.Where(x => x.Key > (int)comboBox1.SelectedValue);
            comboBox2.DataSource = new BindingSource(dt_cycle, null);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            frm_Note frm = new frm_Note();
            frm.Show();
        }

        public void button12_Click(object sender, EventArgs e)
        {
            updatecheckboxlist1(updateLstbType);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (checkedListBoxEx1.SelectedValue!=null)
            {
                frm_Note frm = new frm_Note((int)checkedListBoxEx1.SelectedValue);
                frm.Show();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            checkData("B");
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //int index = this.listBox1.IndexFromPoint(e.Location);
            string name = this.listBox1.Text.Replace("到",",");
            string[] nameArr = name.Split(',');
            long start = Int64.Parse(nameArr[0].Trim());
            long end = Int64.Parse(nameArr[1].Substring(1,11).Trim());
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
            var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_account = '" + frmGameMain.globalUserAccount + "' AND p_name LIKE '%" + GameKind + "%'", dic);
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

        private void btnCopyUp_Click(object sender, EventArgs e)
        {
            richTextBox2.Text += System.Windows.Clipboard.GetText();
        }
    }
}
