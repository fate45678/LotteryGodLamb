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
        Connection con = new Connection();
        public frm_PlanUpload()
        {
            InitializeComponent();
            picAD4.Visible = false;
            pnlAD4.BorderStyle = BorderStyle.None;
            setRule();
        }



        List<string> dt_history = new List<string>();
        //取得歷史開獎
        public void UpdateHistory()
        {
            if (rtxtHistory.Text == "") //無資料就全寫入(第一次載入頁面)
            {
                for (int i = 120; i > 0; i--)
                {
                    rtxtHistory.Text += frmGameMain.jArr[i]["Issue"].ToString() + "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", "") + "\r\n";
                    dt_history.Add(frmGameMain.jArr[i]["Issue"].ToString() + "     " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", ""));
                    //檢查資料庫 沒有就新增
                }
            }
            else if (!string.IsNullOrEmpty(rtxtHistory.Text) && dt_history.ElementAt(dt_history.Count() - 1).IndexOf(frmGameMain.jArr[0]["Issue"].ToString()) == -1)
            {
                rtxtHistory.Text += frmGameMain.jArr[0]["Issue"].ToString() + "  " + frmGameMain.jArr[0]["Number"].ToString().Replace(",", "") + "\r\n";
                dt_history.Add(frmGameMain.jArr[0]["Issue"].ToString() + "     " + frmGameMain.jArr[0]["Number"].ToString().Replace(",", ""));
                //檢查資料庫 沒有就新增
            }
        }
        /// <summary>
        /// type 0 一般 1 照新增時間 2 照中獎機率
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

            if (type == 0)
            {
                var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_account = '" + frmGameMain.globalUserAccount + "'", dic);
                dic.Clear();
                if (dt.Count > 0)
                {
                    for (int i = 0; i < dt.Count; i = i + 6)
                    {
                        content.Add(new compentContent
                        {
                            id = int.Parse(dt.ElementAt(i)),
                            value = dt.ElementAt(i + 1) + " " +
                                calhits
                                (
                                dt.ElementAt(i + 2),
                                dt.ElementAt(i + 3),
                                dt.ElementAt(i + 4),
                                dt.ElementAt(i + 1)
                                )
                                + "   \r\n" + dt.ElementAt(i + 5).Substring(9)
                        });
                    }
                }
            }
            else if (type == 1)
            {
                var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_account = '" + frmGameMain.globalUserAccount + "' order by p_id desc", dic);
                dic.Clear();
                if (dt.Count > 0)
                {
                    for (int i = 0; i < dt.Count; i = i + 6)
                    {
                        content.Add(new compentContent
                        {
                            id = int.Parse(dt.ElementAt(i)),
                            value = dt.ElementAt(i + 1) + " " +
                                        calhits
                                        (
                                            dt.ElementAt(i + 2),
                                            dt.ElementAt(i + 3),
                                            dt.ElementAt(i + 4),
                                            dt.ElementAt(i + 1)
                                        ) + "  \r\n" + dt.ElementAt(i + 5).Substring(0, dt.ElementAt(i + 5).IndexOf(" "))
                        }
                        );
                    }
                }
            }
            else if (type == 2)
            {
                var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_account = '" + frmGameMain.globalUserAccount + "' order by p_id desc", dic);
                dic.Clear();
                if (dt.Count > 0)
                {
                    Dictionary<string, double> repeat = new Dictionary<string, double>();
                    for (int i = 0; i < dt.Count; i = i + 6)
                    {
                        if (repeat.ContainsKey(dt.ElementAt(i + 1)))
                        {
                            repeat.Add(dt.ElementAt(i) + "_" + dt.ElementAt(i + 1) + "(" + i + ")", calhitsRtndouble
                            (
                                dt.ElementAt(i + 2),
                                dt.ElementAt(i + 3),
                                dt.ElementAt(i + 4),
                                dt.ElementAt(i + 1)
                            ));

                        }
                        else
                        {
                            repeat.Add(dt.ElementAt(i) + "_" + dt.ElementAt(i + 1), calhitsRtndouble
                            (
                                dt.ElementAt(i + 2),
                                dt.ElementAt(i + 3),
                                dt.ElementAt(i + 4),
                                dt.ElementAt(i + 1)
                            ));

                        }
                    }
                    Dictionary<string, double> dic1_SortedByKey = new Dictionary<string, double>();

                    var dicSort = from objDic in repeat orderby objDic.Value descending select objDic;
                    foreach (KeyValuePair<string, double> kvp in dicSort)
                        content.Add(new compentContent
                        {
                            id = int.Parse(kvp.Key.Substring(0, kvp.Key.IndexOf("_"))),
                            value = kvp.Key.Substring(kvp.Key.IndexOf("_")) + "   " + kvp.Value + "%"
                        });
                }
            }
            //adjustListSize(checkedListBox1);
            checkedListBox1.DataSource = content;
            checkedListBox1.ValueMember = "id";
            checkedListBox1.DisplayMember = "value";
            var iiii = this.checkedListBox1.GetItemHeight(0);
            checkedListBox1.ItemHeight = 300;
            //this.checkedListBox1.Height = 200;
        }

        private string calhits(string start, string end, string number, string type)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "number");
            var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from HistoryNumber where issue between " + start + " and " + end, dic);
            var st = Regex.Split(number, " ");
            int sum = 0;
            int hits = 0;
            #region 玩法
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
                #region 中獎率
                if (hits == 0)
                    return "中獎率0%";
                else
                {
                    double result = ((double)hits / sum) * 100;
                    if (result.ToString().Length > 3)
                        return "中獎率" + result.ToString().Substring(0, 3) + "%";
                    else
                        return "中獎率" + result.ToString() + "%";
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
                        if (dt[i].Substring(1, 4) == st[j].Substring(1, 4))
                            hits++;
                    }
                }
                #region 中獎率
                if (hits == 0)
                    return "0%";
                else
                {
                    double result = ((double)hits / sum) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 3) + "%";
                    else
                        return result.ToString() + "%";
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
                #region 中獎率
                if (hits == 0)
                    return "0%";
                else
                {
                    double result = ((double)hits / sum) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 3) + "%";
                    else
                        return result.ToString() + "%";
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
                #region 中獎率
                if (hits == 0)
                    return "0%";
                else
                {
                    double result = ((double)hits / sum) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 3) + "%";
                    else
                        return result.ToString() + "%";
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
                #region 中獎率
                if (hits == 0)
                    return "0%";
                else
                {
                    double result = ((double)hits / sum) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 3) + "%";
                    else
                        return result.ToString() + "%";
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
                #region 中獎率
                if (hits == 0)
                    return "0%";
                else
                {
                    double result = ((double)hits / sum) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 3) + "%";
                    else
                        return result.ToString() + "%";
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
                #region 中獎率
                if (hits == 0)
                    return "0%";
                else
                {
                    double result = ((double)hits / sum) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 3) + "%";
                    else
                        return result.ToString() + "%";
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
                #region 中獎率
                if (hits == 0)
                    return "0%";
                else
                {
                    double result = ((double)hits / sum) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 3) + "%";
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
                            hits++;
                    }
                }
                #region 中獎率
                if (hits == 0)
                    return "0%";
                else
                {
                    double result = ((double)hits / sum) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 3) + "%";
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
                            hits++;
                    }
                }
                #region 中獎率
                if (hits == 0)
                    return "0%";
                else
                {
                    double result = ((double)hits / sum) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 3) + "%";
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
                            hits++;
                    }
                }
                #region 中獎率
                if (hits == 0)
                    return "0%";
                else
                {
                    double result = ((double)hits / sum) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 3) + "%";
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
                            hits++;
                    }
                }
                #region 中獎率
                if (hits == 0)
                    return "0%";
                else
                {
                    double result = ((double)hits / sum) * 100;
                    if (result.ToString().Length > 3)
                        return result.ToString().Substring(0, 3) + "%";
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
                #region 中獎率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits / sum) * 100;
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
                        if (dt[i].Substring(1, 4) == st[j].Substring(1, 4))
                            hits++;
                    }
                }
                #region 中獎率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits / sum) * 100;
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
                #region 中獎率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits / sum) * 100;
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
                #region 中獎率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits / sum) * 100;
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
                #region 中獎率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits / sum) * 100;
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
                #region 中獎率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits / sum) * 100;
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
                #region 中獎率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits / sum) * 100;
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
                #region 中獎率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits / sum) * 100;
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
                #region 中獎率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits / sum) * 100;
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
                #region 中獎率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits / sum) * 100;
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
                #region 中獎率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits / sum) * 100;
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
                #region 中獎率
                if (hits == 0)
                    return 0;
                else
                {
                    double result = ((double)hits / sum) * 100;
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
            else if (loginButtonType == 1)
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
                var dt_plan = Items.Where(x => x.Key > current - 1);
                var dt_cycle = Items.Where(x => x.Key > current);
                cbGamePlan.DataSource = new BindingSource(dt_plan, null);
                comboBox1.DataSource = new BindingSource(dt_plan, null);
                cbGameCycle.DataSource = new BindingSource(dt_cycle, null);
                comboBox2.DataSource = new BindingSource(dt_cycle, null);
                temp = current;
            }
            else if (current == 120)
            {
                var dt_plan = Items.Where(x => x.Key < 999);
                var dt_cycle = Items.Where(x => x.Key > 1);
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
                return ((int)cbGameCycle.SelectedValue - (int)cbGamePlan.SelectedValue) + 1;
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
            var dt = con.ConSQLtoList4cb("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select issue as 'name' from HistoryNumber where issue = '" + st + "'");
            if (dt.Count == 0)//沒找到期數
            {
                con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "Insert into HistoryNumber(issue, number) values('" + st + "','" + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "')");
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
                frm_login.Show();
            }
            else if (loginButtonType == 1)
            {
                if (!string.IsNullOrEmpty(label4.Text))
                {
                    frmGameMain.globalUserName = "";
                    frmGameMain.globalUserAccount = "";
                    label4.Text = "";
                    MessageBox.Show("登出成功。");
                    frm_PlanAgent.resetFavoriteFlag();
                    button1.Text = "登入";
                    loginButtonType = 0;
                    frmGameMain.globalMessageTemp = "";
                }
            }
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
            UpdateHistory();
            refreshInterface();
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

            if (updateCount % 3 == 0 && allorwUpdate)
                updatecheckboxlist1(updateLstbType);
            updateCount++;


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
                checkData("A");
                string planName = label24.Text.Replace("重庆时时彩  ", "");
                con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "Insert into Upplan(p_name, p_account, p_start, p_end, p_rule,p_curNum, p_note, p_uploadDate) values('" + label4.Text + "重慶時時彩" + planName + "','" + frmGameMain.globalUserAccount + "','" + cbGamePlan.Text + "','" + cbGameCycle.Text + "','" + richTextBox2.Text + "','0','" + frmGameMain.globalMessageTemp + "', getDate())");

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
            var getPlan = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_account ='" + frmGameMain.globalUserAccount + "'", dic_plan);
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
            checkedListBox1.Items.Clear();
            for (int i = 0; i < dic1_SortedByKey.Count(); i++)
            {
                checkedListBox1.Items.Add(dic1_SortedByKey.ElementAt(i).Key.ToString());
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
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemChecked(i, true);
            }
            else if (!checkBox1.Checked)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemChecked(i, false);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            foreach (object checkedItem in checkedListBox1.CheckedItems)
                con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "delete from Upplan where p_id = " + checkedListBox1.SelectedValue);
            MessageBox.Show("刪除成功。");
            updatecheckboxlist1(0);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            label16.Text = "";
            label17.Text = "已上传:";
            label15.Text = "共0注";
            richTextBox1.Text = "";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(label4.Text))
                MessageBox.Show("請先登入。");
            else
            {
                string NowDate = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
                //取得帳號
                Dictionary<int, string> dic = new Dictionary<int, string>();
                dic.Add(0, "account");
                //string iiii = "select * from userData where account = '" + frmGameMain.globalUserAccount + "'";
                var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from userData where account = '" + frmGameMain.globalUserAccount + "'", dic);
                if (getData.Count != 0)
                {
                    string iiii = "Insert into Upplan(p_name ,p_account ,p_start ,p_end ,p_rule) values('" + label16.Text.Replace("續傳", "") + "續傳" + "','" + getData.ElementAt(0) + "','" + comboBox1.Text + "','" + comboBox2.Text + "','" + richTextBox1.Text + "','" + NowDate + "')";
                    con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "Insert into Upplan(p_name ,p_account ,p_start ,p_end ,p_rule,p_uploadDate) values('" + label16.Text.Replace("續傳", "") + "續傳" + "','" + getData.ElementAt(0) + "','" + comboBox1.Text + "','" + comboBox2.Text + "','" + richTextBox1.Text + "','" + NowDate + "')");
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
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "p_name");
            dic.Add(1, "p_account");
            dic.Add(2, "p_start");
            dic.Add(3, "p_end");
            dic.Add(4, "p_rule");
            allorwUpdate = false;
            var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_id = '" + checkedListBox1.SelectedValue + "'", dic);
            label17.Text = "已上传: 第" + getData.ElementAt(2) + "~" + getData.ElementAt(3) + "期";

            for (int i = 0; i < getData.Count; i++)
            {
                if (i == 0)
                    label16.Text = getData.ElementAt(i).Substring(getData.ElementAt(i).IndexOf(" ") + 1);
                else if (i == 2) { }
                else if (i == 4)
                {
                    richTextBox1.Text = getData.ElementAt(i).Substring(0, getData.ElementAt(i).Length);
                    //string[] test = getData.ElementAt(i).Split(" ");
                    string strReplace = getData.ElementAt(i).Replace(",", "");
                    //string iiiii = getData.ElementAt(2).Substring(8,3);
                    int times = (Convert.ToInt32(getData.ElementAt(3).Substring(8, 3)) - Convert.ToInt32(getData.ElementAt(2).Substring(8, 3)));
                    label15.Text = "共" + times + "注";
                }
            }
            listBox1.Items.Clear();
            if (getData.Count > 0)
            {
                var ja = frmGameMain.jArr.First;
                string NowIssue = ja["Issue"].ToString();
                long issue = Int64.Parse(getData.ElementAt(2)), NextIssue = 0;
                Decimal amount = Convert.ToDecimal(getData.ElementAt(3)) - Convert.ToDecimal(getData.ElementAt(2)) + 1;
                if (amount % 2 != 0)
                    amount = amount + 1;
                long Number = 0;
                string CheckNumber = "";
                int fail = 0, win = 0, NoOpen = 0;

                if (Int64.Parse(getData.ElementAt(2)) > Int64.Parse(NowIssue))
                {
                    for (int i = 1; i <= amount; i++)
                    {
                        //NoOpen++;
                        //新方法 只秀出我上傳的期數
                        if (i % 2 == 0)
                        {
                            NextIssue = issue + 1;
                            listBox1.Items.Add(issue + " 到 " + NextIssue + " 尚未開獎(1)");
                            issue = issue + 2;
                            NoOpen = 0;
                        }
                        else if (i == amount)
                        {
                            NoOpen = 1;
                            listBox1.Items.Add(issue + " 尚未開獎(1)");
                        }
                        //if (i < 4)//
                        //{
                        //    string aaaa = richTextBox1.Text;
                        //    listBox1.Items.Add(getData.ElementAt(2) + " 到 " + getData.ElementAt(2) + " 未投注");
                        //}
                        //else if (i >= 4 && i < 34)
                        //{
                        //    listBox1.Items.Add(getData.ElementAt(2).Substring(0, 8) + "0" + ((i * 3) - 2) + " 到 " + getData.ElementAt(2).Substring(0, 8) + "0" + i * 3 + " 未投注");
                        //}
                        //else if (i >= 34 && i < 41)
                        //{
                        //    listBox1.Items.Add(getData.ElementAt(2).Substring(0, 8) + ((i * 3) - 2) + " 到 " + getData.ElementAt(2).Substring(0, 8) +  i * 3 + " 未投注");
                        //}
                        //else
                        //{
                        //    listBox1.Items.Add(getData.ElementAt(2).Substring(0, 8) + ((i * 3) - 2) + " 到 " +
                        //    getData.ElementAt(2).Substring(0, 8) + i * 3 + "ERROR");
                        //}

                    }
                }
                else if (Int64.Parse(getData.ElementAt(2)) == Int64.Parse(NowIssue))
                {
                    for (int i = 1; i <= amount; i++)
                    {
                        //NoOpen++;
                        //新方法 只秀出我上傳的期數
                        if (i % 2 == 0)
                        {
                            NextIssue = issue + 1;
                            listBox1.Items.Add(issue + " 到 " + NextIssue + " 尚未開獎(2)");
                            issue = issue + 2;
                            NoOpen = 0;
                        }
                        else if (i == amount)
                        {
                            NoOpen = 1;
                            listBox1.Items.Add(issue + " 尚未開獎(1)");
                        }
                        //if (i < 4)//
                        //{
                        //    string aaaa = richTextBox1.Text;
                        //    listBox1.Items.Add(getData.ElementAt(2) + " 到 " + getData.ElementAt(2) + " 未投注");
                        //}
                        //else if (i >= 4 && i < 34)
                        //{
                        //    listBox1.Items.Add(getData.ElementAt(2).Substring(0, 8) + "0" + ((i * 3) - 2) + " 到 " + getData.ElementAt(2).Substring(0, 8) + "0" + i * 3 + " 未投注");
                        //}
                        //else if (i >= 34 && i < 41)
                        //{
                        //    listBox1.Items.Add(getData.ElementAt(2).Substring(0, 8) + ((i * 3) - 2) + " 到 " + getData.ElementAt(2).Substring(0, 8) +  i * 3 + " 未投注");
                        //}
                        //else
                        //{
                        //    listBox1.Items.Add(getData.ElementAt(2).Substring(0, 8) + ((i * 3) - 2) + " 到 " +
                        //    getData.ElementAt(2).Substring(0, 8) + i * 3 + "ERROR");
                        //}

                    }
                }
                else
                {
                    for (int i = 1; i <= amount; i++)
                    {
                        Number = Int64.Parse(getData.ElementAt(2)) + Int64.Parse(i.ToString());

                        var frmGameMainjArr = frmGameMain.jArr.Where(x => x["Issue"].ToString().Contains(Number.ToString())).ToList();
                        if (frmGameMainjArr.Count() != 0)
                            CheckNumber = frmGameMainjArr[0]["Number"].ToString().Replace(",", "");

                        if (getData.ElementAt(4).Contains(CheckNumber))
                        {
                            win++;
                            NextIssue = issue + 1;
                            listBox1.Items.Add(issue + " 到 " + NextIssue + " 中" + win);
                            issue = issue + 2;
                            fail = 0;
                        }
                        else
                        {
                            fail++;
                            //NextIssue = issue + 1;
                            //listBox1.Items.Add(issue + " 到 " + NextIssue + " 掛" + fail);
                            //issue = issue + 2;

                            if (i % 2 == 0 || i == amount)
                            {
                                NextIssue = issue + 1;
                                listBox1.Items.Add(issue + " 到 " + NextIssue + " 掛" + fail);
                                issue = issue + 2;
                                fail = 0;
                            }
                        }
                    }
                }

            }


            //    for (int i = 0; i < dt_history.Count; i++)
            //    {
            //        if (int.Parse(dt_history.ElementAt(i).Substring(4, 7)) >= int.Parse(getData.ElementAt(2).Substring(4, 7)) && int.Parse(dt_history.ElementAt(i).Substring(4, 7)) <= int.Parse(getData.ElementAt(3).Substring(4, 7)))
            //        {
            //            if (getData.ElementAt(4).IndexOf(dt_history.ElementAt(i).Substring(dt_history.ElementAt(i).IndexOf(" ") + 1).Trim()) != -1)
            //                listBox1.Items.Add(dt_history[i].ToString() + "     中");
            //            else
            //                listBox1.Items.Add(dt_history[i].ToString() + "     掛");
            //        }
            //        else
            //            listBox1.Items.Add(dt_history[i].ToString() + "     停");
            //    }
            comboBox1.DataSource = new BindingSource(Items.Where(x => x.Key > int.Parse(getData.ElementAt(3).Substring(8))), null);
            comboBox2.DataSource = new BindingSource(Items.Where(x => x.Key > (int.Parse(getData.ElementAt(3).Substring(8))) + 1), null);
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
            if (checkedListBox1.SelectedValue != null)
            {
                frm_Note frm = new frm_Note((int)checkedListBox1.SelectedValue);
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
    }
}
