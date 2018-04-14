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
    public partial class frm_Register : Form
    {
        public frm_Register()
        {
            InitializeComponent();
        }

        #region UI事件
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void btReset_Click(object sender, EventArgs e)
        {
            tbAccount.Text = "";
            tbPassword.Text = "";
            tbPasswordCheck.Text = "";
            tbUserName.Text = "";
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            
            if (!string.IsNullOrEmpty(tbAccount.Text) && !string.IsNullOrEmpty(tbPassword.Text)
                && !string.IsNullOrEmpty(tbPasswordCheck.Text) && !string.IsNullOrEmpty(tbUserName.Text))
            {
                if (!tbPassword.Text.Equals(tbPasswordCheck.Text))
                    MessageBox.Show("密碼不一致");
                else
                {
                    //檢查DB是否有重複帳號
                    Connection con = new Connection();
                    con.SqlInsertUpdateDelete("43.229.154.156", "lottery","select * from userData");
                    
                }
            }
            else
            {
                MessageBox.Show("請輸入所有資料");
            }
        }

        #endregion


    }
}
