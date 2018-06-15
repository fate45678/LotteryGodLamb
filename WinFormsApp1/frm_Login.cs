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
    public partial class frm_Login : Form
    {
        public frm_Login()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                //檢查DB是否有重複帳號
                Dictionary<int, string> dic_field = new Dictionary<int, string>();
                dic_field.Add(0, "name");
                Connection con = new Connection();
                var consql = con.ConSQLtoLT("43.252.208.201,1433\\SQLEXPRESS", "lottery", "select * from userData where account = '" + textBox1.Text + "' and password ='" + textBox2.Text + "'", dic_field);
                if (consql.Count > 0)
                {
                    DialogResult dr = MessageBox.Show("登入成功。");
                    frmGameMain.globalUserAccount = textBox1.Text;
                    frmGameMain.globalUserName = consql.ElementAt(0);
                    if (dr == DialogResult.OK)
                    {
                        this.Close();
                        frm_PlanUpload.loginButtonType = 1;
                        frm_PlanAgent.loginButtonType = 1;
                        frm_PlanUpload.isFirst = false;
                    }

                }
                else if(consql.Count==0)
                   MessageBox.Show("帳號或密碼錯誤，請重新登入。");
            }
            else
            {
                MessageBox.Show("請輸入所有資料");
            }
        }
    }

}
