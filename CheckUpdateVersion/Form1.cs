using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;
using System.Diagnostics;
using Ionic.Zip;
using System.IO;

namespace CheckUpdateVersion
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pgbshow.Minimum = 0;
            pgbshow.Maximum = 4;
            bkgCheckVersion.WorkerReportsProgress = true;//啟動回報進度
            bkgCheckVersion.RunWorkerAsync();
        }

        string errorDesc = "";
        string NewVersion = "";
        private bool ConnectDbGetHistoryNumber()
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";

            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
            con = new SqlConnection(connetionString);
            string date = DateTime.Now.ToString("u").Substring(0, 10).Replace("-", "");
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


                //MessageBox.Show(ProVersion);

                if (VersionNumber == ProVersion)
                    return false;
                else
                { 
                    NewVersion = VersionNumber;
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorDesc = ex.ToString();
                return false;
            }
        }
        int iCheckProcess = 1;
        private void bkgCheckVersion_DoWork(object sender, DoWorkEventArgs e)
        {
            bkgCheckVersion.ReportProgress(iCheckProcess);
            iCheckProcess++;

            bool isNeedToUpdate = ConnectDbGetHistoryNumber();
            if (errorDesc != "")
            {
                MessageBox.Show("请洽客服人员," + errorDesc);
                Application.Exit();
                return;
            }

            if (isNeedToUpdate)
            {
                string LoaclPath = Application.StartupPath.ToString();
                string FileName = NewVersion + ".zip";
                //下载档案中
                bkgCheckVersion.ReportProgress(iCheckProcess);
                iCheckProcess++;

                WebClient client = new WebClient();
                client.DownloadFile("http://43.252.208.201:81/ShengDnnZip/" + FileName, LoaclPath + @"/" + FileName);

                //解压缩档案中
                bkgCheckVersion.ReportProgress(iCheckProcess);
                iCheckProcess++;
                //取出檔案
                using (ZipFile zip = ZipFile.Read(LoaclPath + @"/" + FileName))
                {
                    //第三步驟在這裡
                    //复盖原本的档案
                    bkgCheckVersion.ReportProgress(iCheckProcess);
                    iCheckProcess++;

                    foreach (ZipEntry entry in zip)
                    {
                        //相同檔案覆蓋                       
                        entry.Extract(LoaclPath, ExtractExistingFileAction.OverwriteSilently);
                    }
                }

                //複製檔案且覆蓋
                CopyFiles(LoaclPath + @"/ShengDneng", LoaclPath);

                //删除下载的档案
                bkgCheckVersion.ReportProgress(iCheckProcess);
                iCheckProcess++;
                File.Delete(LoaclPath + @"/" + FileName);
                Directory.Delete(LoaclPath + @"/ShengDneng", true);

                //更新完畢開始啟動程式
                Process.Start(
                    string.Format(@"{0}\{1}", Application.StartupPath, "WinFormsApp1.exe"));
                Application.Exit();
            }
            else
            {
                Process.Start(
                    string.Format(@"{0}\{1}", Application.StartupPath, "WinFormsApp1.exe"));
                Application.Exit();
            }
        }

        //首先取得原始檔案夾路徑下的所有檔案名稱
        private FileInfo[] GetFileList(string path)
        {
            FileInfo[] fileList = null;
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                fileList = di.GetFiles();
            }

            return fileList;
        }

        //複製檔案
        private void CopyFiles(string remotePath, string localPath)
        {
            FileInfo[] file = GetFileList(remotePath);
            for (int i = 0; i < file.Length; i++)
            {
                string fileName = remotePath + @"\" + file.GetValue(i).ToString();
                string desFileName = localPath + @"\" + file.GetValue(i).ToString();
                File.Copy(fileName, desFileName, true);
                System.Threading.Thread.Sleep(500);
            }
        }

        private void bkgCheckVersion_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgbshow.Value = e.ProgressPercentage;

            if (pgbshow.Value == 1)
            {
                lbCheckVersionDesc.Text = "检查版本中...请稍候";
            }
            else if (pgbshow.Value == 2)
            {
                lbCheckVersionDesc.Text = "下载档案中...请稍候";
            }
            else if (pgbshow.Value == 3)
            {
                lbCheckVersionDesc.Text = "解压缩档案中...请稍候";
            }
            else if (pgbshow.Value == 4)
            {
                lbCheckVersionDesc.Text = "复盖原本的档案...请稍候";
            }
            else if (pgbshow.Value == 5)
            {
                lbCheckVersionDesc.Text = "删除下载的档案...请稍候";
            }
        }
    }
}
