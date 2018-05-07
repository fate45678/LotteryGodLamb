using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class frm_Note : Form
    {
        Connection con = new Connection();
        int global_pid = 0;
        /// <summary>
        /// 不帶參數建構子
        /// </summary>
        public frm_Note()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 修改備註
        /// </summary>
        /// <param name="pid"></param>
        public frm_Note(int pid)
        {
            InitializeComponent();
            global_pid = pid;
        }

        private void frm_Note_Load(object sender, EventArgs e)
        {
            if (global_pid == 0)
            {
                if (string.IsNullOrEmpty(frmGameMain.globalMessageTemp))
                {
                    Dictionary<int, string> dic = new Dictionary<int, string>();
                    dic.Add(0, "p_note");
                    var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select top 1 * from Upplan where p_account = '" + frmGameMain.globalUserAccount + "' order by p_id desc", dic);
                    richTextBox1.Text = dt.ElementAt(0).ToString();
                }
                else
                    richTextBox1.Text = frmGameMain.globalMessageTemp;
            }
            else
            {
                Dictionary<int, string> dic = new Dictionary<int, string>();
                dic.Add(0, "p_note");
                var dt = con.ConSQLtoLT("43.252.208.201, 1433\\SQLEXPRESS", "lottery", "select top 1 * from Upplan where p_id = '" + global_pid + "' order by p_id desc", dic);
                richTextBox1.Text = dt.ElementAt(0).ToString();
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmGameMain.globalMessageTemp = richTextBox1.Text;
            MessageBox.Show("儲存成功。");
        }
    }
}
