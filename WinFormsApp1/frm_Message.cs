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
    public partial class frm_Message : Form
    {
        public frm_Message()
        {
            InitializeComponent();
            loadUrlAdPicture();
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

                string Url = string.Format("http://43.252.208.201:81/Upload/{0}/4/4.jpg", User);
                var request = WebRequest.Create(Url);
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    this.BackgroundImage = Bitmap.FromStream(stream);
                    this.Click += new EventHandler(pic_Click);
                }
            }
        }

        string clickUrl = "";
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

        private void button1_Click(object sender, EventArgs e)
        {            
            //frmGameMain fGameMain = new frmGameMain();
            frmGameMain.openMessage = false;
            this.Dispose();
            this.Close();
        }

        private void frm_Message_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmGameMain.clsIcon = 0;
        }
    }
}
