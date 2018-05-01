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
            else if(!string.IsNullOrEmpty(rtxtHistory.Text) && dt_history.ElementAt(dt_history.Count()-1).IndexOf(frmGameMain.jArr[0]["Issue"].ToString())==-1)
            {
                rtxtHistory.Text += frmGameMain.jArr[0]["Issue"].ToString() + "  " + frmGameMain.jArr[0]["Number"].ToString().Replace(",", "") + "\r\n";
                dt_history.Add(frmGameMain.jArr[0]["Issue"].ToString() + "     " + frmGameMain.jArr[0]["Number"].ToString().Replace(",", ""));
                //檢查資料庫 沒有就新增
            }
        }
        private void updatecheckboxlist1()
        {
            checkedListBox1.Items.Clear();
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "p_name");
            var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_account = '" + frmGameMain.globalUserAccount + "'", dic);
            for (int i = 0; i < dt.Count; i++)
            {
                checkedListBox1.Items.Add(dt.ElementAt(i).ToString());

            }
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
                updatecheckboxlist1();
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
                //Dictionary<int, string> dic = new Dictionary<int, string>();
                //dic.Add(0, "p_curNum");
                string planName = label24.Text.Replace("重庆时时彩  ", "");
                //var plancount = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select case when max(p_curNum) is null then 0 else max(p_curNum) end as 'p_curNum' from Upplan where p_account ='" + frmGameMain.globalUserAccount + "'", dic);
                //int planNUmber = int.Parse(plancount.ElementAt(0)) + 1;
                con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "Insert into Upplan(p_name, p_account, p_start, p_end, p_rule,p_curNum, p_note) values('" + label4.Text + "重慶時時彩" + planName+"','" + frmGameMain.globalUserAccount + "','" + cbGamePlan.Text + "','" + cbGameCycle.Text + "','" + richTextBox2.Text + "','0','"+frmGameMain.globalMessageTemp+"')");

                MessageBox.Show("上傳成功。");
                updatecheckboxlist1();
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
                cbGameDirect.SelectedIndex = -1;
                cbGameDirect.Enabled = false;
            }
            else
            {
                cbGameDirect.SelectedIndex = 0;
                cbGameDirect.Enabled = true ;

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


        /// <summary>
        /// 除錯按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            string[] temp;
            string data = "";
            if (richTextBox2.Text.IndexOf(" ") != -1 && richTextBox2.Text.IndexOf(",") != -1)
                data = richTextBox2.Text.Replace(" ", ",");
            else if (richTextBox2.Text.IndexOf(" ")!=-1)
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
                    }
                }
                MessageBox.Show("已清除重複及錯誤資料。");
                label21.Text = "共"+labelCount+"注";
            }


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


           
            //MatchCollection mc;
            //Regex r = new Regex(" ");
            //mc = r.Matches(richTextBox2.Text);
            //label21.Text = "共" + mc.Count + "注";
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
                for(int i = 0; i < checkedListBox1.Items.Count; i++)
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
                con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "delete from Upplan where p_name = '" + checkedItem.ToString() + "'");
            MessageBox.Show("刪除成功。");
            updatecheckboxlist1();
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
                //取得帳號
                Dictionary<int, string> dic = new Dictionary<int, string>();
                dic.Add(0, "account");
                var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from userData where account = '" + frmGameMain.globalUserAccount + "'", dic);
                if (getData.Count != 0)
                {
                    con.ExecSQL("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "Insert into Upplan(p_name ,p_account ,p_start ,p_end ,p_rule) values('" + label16.Text + "續傳" + "','" + getData.ElementAt(0) + "','" + comboBox1.Text + "','" + comboBox2.Text + "','" + richTextBox1.Text + "," + "')");
                    updatecheckboxlist1();
                }
                else
                    MessageBox.Show("該計畫帳號不存在。");
            }
        }
        private void button11_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
                dic.Add(0, "p_name");
            var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_account = '" + frmGameMain.globalUserAccount + "' order by p_id desc", dic);
            checkedListBox1.Items.Clear();
            for (int i = 0; i < dt.Count; i++)
                checkedListBox1.Items.Add(dt.ElementAt(i).ToString());
        }
        private void button10_Click(object sender, EventArgs e)
        {
            upodatelsbSentbyHitTimes();
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
            var getData = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select * from Upplan where p_name = '" + checkedListBox1.SelectedItem + "'", dic);
            for (int i = 0; i < getData.Count; i++)
            {

                if (i == 0)
                    label16.Text = getData.ElementAt(i).Substring(getData.ElementAt(i).IndexOf(" ") + 1);
                else if (i == 2)
                    label17.Text = "已上传: 第" + getData.ElementAt(i) + "~" + getData.ElementAt(i + 1) + "期";
                else if (i == 4)
                {
                    richTextBox1.Text = getData.ElementAt(i).Substring(0, getData.ElementAt(i).Length);
                    string strReplace = getData.ElementAt(i).Replace(",", "");
                    int times = (getData.ElementAt(i).Length - strReplace.Length) / 1;
                    label15.Text = "共" + times + "注";
                }
            }
            listBox1.Items.Clear();
            //檢查期數
            for (int i = 0; i < dt_history.Count; i++)
            {
                if (int.Parse(dt_history.ElementAt(i).Substring(4, 7)) >= int.Parse(getData.ElementAt(2).Substring(4, 7)) && int.Parse(dt_history.ElementAt(i).Substring(4, 7)) <= int.Parse(getData.ElementAt(3).Substring(4, 7)))
                {
                    if (getData.ElementAt(4).IndexOf(dt_history.ElementAt(i).Substring(dt_history.ElementAt(i).IndexOf(" ") + 1).Trim()) != -1)
                        listBox1.Items.Add(dt_history[i].ToString() + "     中");
                    else
                        listBox1.Items.Add(dt_history[i].ToString() + "     掛");
                }
                else
                    listBox1.Items.Add(dt_history[i].ToString() + "     停");
            }
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
    }
}
