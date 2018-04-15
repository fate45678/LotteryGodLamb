using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
    public partial class frm_PlanCycle : Form
    {
        public frm_PlanCycle()
        {
            InitializeComponent();

            txtGameNum.ForeColor = Color.LightGray;
            txtGameNum.Text = "请输入奖金号";
            this.txtGameNum.Leave += new System.EventHandler(this.txtGameNum_Leave);
            this.txtGameNum.Enter += new System.EventHandler(this.txtGameNum_Enter);

            txtTimes.ForeColor = Color.LightGray;
            txtTimes.Text = "请输入倍数";
            this.txtTimes.Leave += new System.EventHandler(this.txtTimes_Leave);
            this.txtTimes.Enter += new System.EventHandler(this.txtTimes_Enter);

            cbMoney.SelectedIndex = 0;
            cbGameKind.SelectedIndex = 0;
            cbGameDirect.SelectedIndex = 0;
            cbGamePlus.SelectedIndex = 0;
            //cbGamePlan.SelectedIndex = 0;
            cbGameCycle.SelectedIndex = 0;
        }

        #region TextBox的提示
        private void txtGameNum_Leave(object sender, EventArgs e)
        {
            if (txtGameNum.Text == "")
            {
                txtGameNum.Text = "请输入奖金号";
                txtGameNum.ForeColor = Color.Gray;
            }
        }
        private void txtGameNum_Enter(object sender, EventArgs e)
        {
            if (txtGameNum.Text == "请输入奖金号")
            {
                txtGameNum.Text = "";
                txtGameNum.ForeColor = Color.Black;
            }
        }
        private void txtTimes_Leave(object sender, EventArgs e)
        {
            if (txtTimes.Text == "")
            {
                txtTimes.Text = "请输入倍数";
                txtTimes.ForeColor = Color.Gray;
            }
        }
        private void txtTimes_Enter(object sender, EventArgs e)
        {
            if (txtTimes.Text == "请输入倍数")
            {
                txtTimes.Text = "";
                txtTimes.ForeColor = Color.Black;
            }
        }
        #endregion

        #region ComboBox的切換處理
        private void cbGameKind_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (cbGameKind.SelectedItem.ToString())
            {
                case "五星":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("五星组合");
                    cbGameDirect.SelectedIndex = 0;
                    cbGamePlus.Items.Clear();
                    cbGamePlus.Items.Add("30000+");
                    cbGamePlus.Items.Add("40000+");
                    cbGamePlus.Items.Add("50000+");
                    cbGamePlus.SelectedIndex = 0;
                    break;
                case "四星":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("四星组合");
                    cbGameDirect.SelectedIndex = 0;
                    cbGamePlus.Items.Clear();
                    cbGamePlus.Items.Add("3000+");
                    cbGamePlus.Items.Add("4000+");
                    cbGamePlus.Items.Add("5000+");
                    cbGamePlus.SelectedIndex = 0;
                    break;
                case "前三":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("前三组合");
                    cbGameDirect.Items.Add("直选和值");
                    cbGameDirect.Items.Add("直选跨度");
                    cbGameDirect.SelectedIndex = 0;
                    cbGamePlus.Items.Clear();
                    cbGamePlus.Items.Add("300+");
                    cbGamePlus.Items.Add("400+");
                    cbGamePlus.Items.Add("500+");
                    cbGamePlus.SelectedIndex = 0;
                    break;
                case "中三":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("中三组合");
                    cbGameDirect.Items.Add("直选和值");
                    cbGameDirect.Items.Add("直选跨度");
                    cbGameDirect.SelectedIndex = 0;
                    cbGamePlus.Items.Clear();
                    cbGamePlus.Items.Add("300+");
                    cbGamePlus.Items.Add("400+");
                    cbGamePlus.Items.Add("500+");
                    cbGamePlus.SelectedIndex = 0;
                    break;
                case "后三":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("后三组合");
                    cbGameDirect.Items.Add("直选和值");
                    cbGameDirect.Items.Add("直选跨度");
                    cbGameDirect.SelectedIndex = 0;
                    cbGamePlus.Items.Clear();
                    cbGamePlus.Items.Add("300+");
                    cbGamePlus.Items.Add("400+");
                    cbGamePlus.Items.Add("500+");
                    cbGamePlus.SelectedIndex = 0;
                    break;
                case "前二":
                case "后二":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("直选和值");
                    cbGameDirect.Items.Add("直选跨度");
                    cbGameDirect.SelectedIndex = 0;
                    cbGamePlus.Items.Clear();
                    cbGamePlus.Items.Add("30+");
                    cbGamePlus.Items.Add("40+");
                    cbGamePlus.Items.Add("50+");
                    cbGamePlus.SelectedIndex = 0;
                    break;
                case "定位胆":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("定位胆");
                    //todo: 定位胆的處理
                    cbGameDirect.SelectedIndex = 0;
                    cbGamePlus.Items.Clear();
                    break;
                default:
                    break;
            }
        }

        private void cbGamePlus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //switch (cbGamePlus.SelectedItem.ToString().Substring(0, 1))
            //{
            //    case "3":
            //        cbGameCycle.Items.Clear();
            //        cbGameCycle.Items.Add("三期一周");
            //        cbGameCycle.Items.Add("二期一周");
            //        cbGameCycle.Items.Add("一期一周");
            //        cbGameCycle.SelectedIndex = 0;
            //        break;
            //    case "4":
            //        cbGameCycle.Items.Clear();
            //        cbGameCycle.Items.Add("二期一周");
            //        cbGameCycle.Items.Add("一期一周");
            //        cbGameCycle.SelectedIndex = 0;
            //        break;
            //    case "5":
            //        cbGameCycle.Items.Clear();
            //        cbGameCycle.Items.Add("二期一周");
            //        cbGameCycle.Items.Add("一期一周");
            //        cbGameCycle.SelectedIndex = 0;
            //        break;
            //    default:
            //        break;
            //}
        }
        #endregion

        private void btnViewResult_Click(object sender, EventArgs e)
        {
            if (txtGameNum.Text == "" || txtGameNum.Text == "请输入奖金号" ||
                txtTimes.Text == "" || txtTimes.Text == "请输入倍数" ||
                (ckRegularCycle.Checked == false && ckWinToNextCycle.Checked == false) ||
                cbGamePlus.SelectedItem == null ||
                cbGamePlan.SelectedItem == null)
            {
                MessageBox.Show("所有欄位都必須輸入");
                return;
            }

            pnlShowPlan.Visible = true;
            //todo: 開始計算
            //每期注數 共?元
            lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString(".###");
            //總投注額?元
            lblSumBetsMoney.Text = (Convert.ToDecimal(lblBetsMoney.Text) * Convert.ToDecimal(lblSumBetsCycle.Text)).ToString();
            //獎金?元
            lblWinMoney.Text = (Convert.ToDecimal(txtGameNum.Text) * Convert.ToDecimal(txtTimes.Text)).ToString();
            rtxtPlanCycle.ReadOnly = true;

        }

        private void cbCycleResult1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Pen str = new Pen(Color.Black);
            //Pen bg = new Pen(Color.White);
            switch (e.Index)
            {
                case 2: str = new Pen(Color.Red);
                    break;
            }
            if (e.Index != -1)
            {
                //e.Graphics.FillRectangle(bg.Brush, e.Bounds);
                e.Graphics.DrawString((string)this.cbCycleResult1.Items[e.Index], this.Font, str.Brush, e.Bounds);
            }
        }
        //取得歷史開獎
        private void UpdateHistory()
        {
            if (rtxtHistory.Text == "") //無資料就全寫入
            {
                for (int i = 0; i < frmGameMain.jArr.Count; i++)
                {
                    if (i == 120) break; //寫120筆就好
                    rtxtHistory.Text += frmGameMain.jArr[i]["Issue"].ToString() + "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "\r\n";
                }
            }
            else //有資料先判斷
            {
                if ((rtxtHistory.Text.Substring(0, 11) != frmGameMain.jArr[0]["Issue"].ToString()) && (frmGameMain.strHistoryNumberOpen != "?")) //有新資料了
                {
                    rtxtHistory.Text = "";
                    for (int i = 0; i < frmGameMain.jArr.Count; i++)
                    {
                        if (i == 120) break; //寫120筆就好
                        rtxtHistory.Text += frmGameMain.jArr[i]["Issue"].ToString() + "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "\r\n";
                    }
                }
            }
        }

        private void picAD1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.zhcw.com/");
        }

        private void picAD2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.cqcp.net/");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateHistory();
        }

        private void txtGameNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            //只能輸入數字
            if (e.KeyChar != '\b') //後退鍵以外的字元
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9')) //0~9
                {
                    e.Handled = true;
                }
            }
        }

        private void txtTimes_KeyPress(object sender, KeyPressEventArgs e)
        {
            //只能輸入數字
            if (e.KeyChar != '\b') //後退鍵以外的字元
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9')) //0~9
                {
                    e.Handled = true;
                }
            }
        }

        private void ckRegularCycle_CheckedChanged(object sender, EventArgs e)
        {
            if (ckRegularCycle.Checked == true)
                ckWinToNextCycle.Checked = false;
        }

        private void ckWinToNextCycle_CheckedChanged(object sender, EventArgs e)
        {
            if (ckWinToNextCycle.Checked == true)
                ckRegularCycle.Checked = false;
        }

        private void btnEditPlanNumber_Click(object sender, EventArgs e)
        {
            rtxtPlanCycle.ReadOnly = false;
        }

        private void btnCopyPlanNumber_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(rtxtPlanCycle.Text);
            MessageBox.Show("已复制到剪贴簿");
            //Random R = new Random();

            //string[] temp = new string[450];

            //Random rand = new Random(Guid.NewGuid().GetHashCode());

            //List<int> listLinq = new List<int>(Enumerable.Range(0, 999));
            //listLinq = listLinq.OrderBy(num => rand.Next()).ToList<int>();

            //for (int i = 0; i < 450; i++)
            //{
            //    temp[i]= listLinq[i].ToString("000");
            //}
            //Array.Sort(temp);

            //rtxtPlanCycle.Text = "";
            //for (int i = 0; i < 450; i++)
            //{
            //    rtxtPlanCycle.Text += temp[i] + ", ";
            //} 
        }
    }
}
