using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;

namespace WinFormsApp1
{
    public partial class frm_VersionCheck : Form
    {
        public frm_VersionCheck()
        {
            //確認是否已經過了維護時間
            int NowDate = Int32.Parse(DateTime.Now.ToString("u").Replace("Z", "").Replace(":", "").Substring(10, 5));
            if (NowDate < 10 || NowDate > 2355)
            {
                MessageBox.Show("目前维护中请于12:10分后使用");
                this.Close();
                Environment.Exit(0);
                //return;               
            }
            InitializeComponent();
            this.ControlBox = false;

            bkgVersionCheck.WorkerReportsProgress = true;//啟動回報進度
            //bkgVersionCheck.DoWork += bkgVersionCheck_DoWork;
            //bkgVersionCheck.RunWorkerCompleted += bkgVersionCheck_RunWorkerCompleted;
            //bkgVersionCheck.ProgressChanged += bkgVersionCheck_ProgressChanged;

            Thread.Sleep(2000); 
            bkgVersionCheck.RunWorkerAsync();
            pgbshow.Minimum = 0;
            pgbshow.Maximum = 5;
        }

        private void bkgVersionCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            bool isNeedUpate = checkVersionFormDb();

            if (isNeedUpate)
            {
                //要更新
                bkgVersionCheck.ReportProgress(4);
                Process.Start(string.Format(@"{0}\{1}", Application.StartupPath, "CheckUpdateVersion.exe"));
                Environment.Exit(0);
            }
            else
            {
                //無更新
                bkgVersionCheck.ReportProgress(5);
            }
        }

        private bool checkVersionFormDb()
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";

            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
            con = new SqlConnection(connetionString);
            string date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Substring(0, 10).Replace("/", "");
            string Sqlstr = "";
            try
            {
                con.Open();
                Sqlstr = "Select * from ProgramVer Order by [PV_updatetime] desc";
                SqlDataAdapter da = new SqlDataAdapter(Sqlstr, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                con.Close();

                string VersionNumber = ds.Tables[0].Rows[0]["PV_version"].ToString();
                string ProVersion = this.GetType().Assembly.GetName().Version.ToString();

                //查看
               // MessageBox.Show(ProVersion);

                if (VersionNumber == ProVersion)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        private void bkgVersionCheck_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            pgbshow.Value = e.ProgressPercentage;

            switch (pgbshow.Value)
            {
                case 4:
                    //MessageBox.Show("不符合最新版本...自动更新中请稍候");
                    lbVersionCheckDesc.Text = "不符合最新版本...自动更新中请稍候";
                    break;
                case 5:
                    //MessageBox.Show("目前是最新版本...软件启动中请稍候");
                    lbVersionCheckDesc.Text = "目前是最新版本...软件启动中请稍候";
                    break;
            }
        }

        private void bkgVersionCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Thread.Sleep(1000);
            this.Close();
        }
    }
}
