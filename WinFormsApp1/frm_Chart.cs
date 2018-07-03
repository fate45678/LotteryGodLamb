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
    public partial class frm_Chart : Form
    {

        public static string strGameKindSelect = ""; //五星
        public static string strLineSelect = ""; //辅助线
        public static string strMissSelect = ""; //遗漏
        public static string strMissBarSelect = ""; //遗漏条
        public static string strChartSelect = ""; //走势
        public static string strPeriodSelect = ""; //最近30期

        public frm_Chart()
        {
            InitializeComponent();
            lblGameKind5Star.Click += new System.EventHandler(btnGameKind_Click);
            lblGameKind4Star.Click += new System.EventHandler(btnGameKind_Click);
            lblGameKindPre3.Click += new System.EventHandler(btnGameKind_Click);
            lblGameKindMid3.Click += new System.EventHandler(btnGameKind_Click);
            lblGameKindBack3.Click += new System.EventHandler(btnGameKind_Click);
            lblGameKindPre2.Click += new System.EventHandler(btnGameKind_Click);
            lblGameKindBack2.Click += new System.EventHandler(btnGameKind_Click);
            lblMenuLast30.Click += new System.EventHandler(btnPeriod_Click);
            lblMenuLast50.Click += new System.EventHandler(btnPeriod_Click);
            lblMenuLast100.Click += new System.EventHandler(btnPeriod_Click);
        }

        private void frm_Chart_Load(object sender, EventArgs e)
        {
        }

        #region Click事件
        private void btnGameKind_Click(object sender, EventArgs e)
        {
            if (strGameKindSelect == ((Label)(sender)).Text)
                return;
            ResetAllGameKind(); //重設選單
            //DisposeForm(HD_MenuSelect.Text);
            strGameKindSelect = ((Label)(sender)).Text;

            switch (strGameKindSelect)
            {
                case "五星":
                    lblGameKind5Star.BackColor = Color.DarkMagenta;
                    lblGameKind5Star.ForeColor = Color.White;
                    lblGameKind5Star.Refresh();
                    break;
                case "四星":
                    lblGameKind4Star.BackColor = Color.DarkMagenta;
                    lblGameKind4Star.ForeColor = Color.White;
                    lblGameKind4Star.Refresh();
                    break;
                case "前三":
                    lblGameKindPre3.BackColor = Color.DarkMagenta;
                    lblGameKindPre3.ForeColor = Color.White;
                    lblGameKindPre3.Refresh();
                    break;
                case "中三":
                    lblGameKindMid3.BackColor = Color.DarkMagenta;
                    lblGameKindMid3.ForeColor = Color.White;
                    lblGameKindMid3.Refresh();
                    break;
                case "后三":
                    lblGameKindBack3.BackColor = Color.DarkMagenta;
                    lblGameKindBack3.ForeColor = Color.White;
                    lblGameKindBack3.Refresh();
                    break;
                case "前二":
                    lblGameKindPre2.BackColor = Color.DarkMagenta;
                    lblGameKindPre2.ForeColor = Color.White;
                    lblGameKindPre2.Refresh();
                    break;
                case "后二":
                    lblGameKindBack2.BackColor = Color.DarkMagenta;
                    lblGameKindBack2.ForeColor = Color.White;
                    lblGameKindBack2.Refresh();
                    break;
            }
            PassToUserControl();
        }

        private void btnPeriod_Click(object sender, EventArgs e)
        {
            if (strPeriodSelect == ((Label)(sender)).Text)
                return;
            ResetAllPeriod(); //重設選單
            strPeriodSelect = ((Label)(sender)).Text;

            switch (strPeriodSelect)
            {
                case "最近30期":
                    lblMenuLast30.BackColor = Color.RoyalBlue;
                    lblMenuLast30.ForeColor = Color.White;
                    lblMenuLast30.Refresh();
                    break;
                case "最近50期":
                    lblMenuLast50.BackColor = Color.RoyalBlue;
                    lblMenuLast50.ForeColor = Color.White;
                    lblMenuLast50.Refresh();
                    break;
                case "最近100期":
                    lblMenuLast100.BackColor = Color.RoyalBlue;
                    lblMenuLast100.ForeColor = Color.White;
                    lblMenuLast100.Refresh();
                    break;
            }
            PassToUserControl();
        }

        private void lblMenuLine_Click(object sender, EventArgs e)
        {
            if (lblMenuLine.BackColor == Color.Gainsboro)
            {
                lblMenuLine.BackColor = Color.ForestGreen; lblMenuLine.ForeColor = Color.White; lblMenuLine.Refresh();
                strLineSelect = "辅助线";
            }
            else 
            { 
                lblMenuLine.BackColor = Color.Gainsboro; lblMenuLine.ForeColor = Color.Black; lblMenuLine.Refresh();
                strLineSelect = "";
            }
            PassToUserControl();
        }

        private void lblMenuMiss_Click(object sender, EventArgs e)
        {
            if (lblMenuMiss.BackColor == Color.Gainsboro)
            {
                lblMenuMiss.BackColor = Color.ForestGreen; lblMenuMiss.ForeColor = Color.White; lblMenuMiss.Refresh();
                strMissSelect = "遗漏";
            }
            else
            {
                lblMenuMiss.BackColor = Color.Gainsboro; lblMenuMiss.ForeColor = Color.Black; lblMenuMiss.Refresh();
                strMissSelect = "";
            }
            PassToUserControl();
        }

        private void lblMenuMissBar_Click(object sender, EventArgs e)
        {
            if (lblMenuMissBar.BackColor == Color.Gainsboro)
            {
                lblMenuMissBar.BackColor = Color.ForestGreen; lblMenuMissBar.ForeColor = Color.White; lblMenuMissBar.Refresh();
                strMissBarSelect = "遗漏条";
            }
            else
            {
                lblMenuMissBar.BackColor = Color.Gainsboro; lblMenuMissBar.ForeColor = Color.Black; lblMenuMissBar.Refresh();
                strMissBarSelect = "";
            }
            PassToUserControl();
        }

        private void lblMenuChart_Click(object sender, EventArgs e)
        {
            if (lblMenuChart.BackColor == Color.Gainsboro)
            {
                lblMenuChart.BackColor = Color.ForestGreen; lblMenuChart.ForeColor = Color.White; lblMenuChart.Refresh();
                strChartSelect = "走势";
            }
            else
            {
                lblMenuChart.BackColor = Color.Gainsboro; lblMenuChart.ForeColor = Color.Black; lblMenuChart.Refresh();
                strChartSelect = "";
            }
            PassToUserControl();
        }
        #endregion

        //重設選單
        private void ResetAllGameKind()
        {
            lblGameKind5Star.BackColor = Color.Gainsboro; lblGameKind5Star.ForeColor = Color.Black; lblGameKind5Star.Refresh();
            lblGameKind4Star.BackColor = Color.Gainsboro; lblGameKind4Star.ForeColor = Color.Black; lblGameKind4Star.Refresh();
            lblGameKindPre3.BackColor = Color.Gainsboro; lblGameKindPre3.ForeColor = Color.Black; lblGameKindPre3.Refresh();
            lblGameKindMid3.BackColor = Color.Gainsboro; lblGameKindMid3.ForeColor = Color.Black; lblGameKindMid3.Refresh();
            lblGameKindBack3.BackColor = Color.Gainsboro; lblGameKindBack3.ForeColor = Color.Black; lblGameKindBack3.Refresh();
            lblGameKindPre2.BackColor = Color.Gainsboro; lblGameKindPre2.ForeColor = Color.Black; lblGameKindPre2.Refresh();
            lblGameKindBack2.BackColor = Color.Gainsboro; lblGameKindBack2.ForeColor = Color.Black; lblGameKindBack2.Refresh();

        }

        //重設選單
        private void ResetAllPeriod()
        {
            lblMenuLast30.BackColor = Color.Gainsboro; lblMenuLast30.ForeColor = Color.Black; lblMenuLast30.Refresh();
            lblMenuLast50.BackColor = Color.Gainsboro; lblMenuLast50.ForeColor = Color.Black; lblMenuLast50.Refresh();
            lblMenuLast100.BackColor = Color.Gainsboro; lblMenuLast100.ForeColor = Color.Black; lblMenuLast100.Refresh();

        }

        //取得歷史開獎
        private void UpdateHistory()
        {
            
        }        

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateHistory();
        }

        //傳參數
        private void PassToUserControl()
        {
            if (strGameKindSelect != "" && strPeriodSelect != "" )
                userControl11.DrawChart(strGameKindSelect, strLineSelect, strMissSelect, strMissBarSelect, strChartSelect, strPeriodSelect);
        }

        private void lbRefrash_Click(object sender, EventArgs e)
        {
            PassToUserControl();
        }

    }
}
