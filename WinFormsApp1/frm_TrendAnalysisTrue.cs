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
    public partial class frm_TrendAnalysisTrue : Form
    {
        public frm_TrendAnalysisTrue()
        {
            InitializeComponent();
        }

        private void connectDb()
        {
            string serverIP = "43.252.208.201, 1433\\SQLEXPRESS", DB = "lottery";

            string connetionString = null;
            SqlConnection con;
            connetionString = "Data Source=" + serverIP + ";Initial Catalog = " + DB + "; USER ID = 4winform; Password=sasa";
            con = new SqlConnection(connetionString);
            string SelectNowDate = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss").Substring(0, 10);
            try
            {
                con.Open();

                string Sqlstr = @"SELECT * from HistoryNumber";
                SqlDataAdapter da = new SqlDataAdapter(Sqlstr, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                con.Close();
                //return ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //return null;
            }
        }
    }
}
