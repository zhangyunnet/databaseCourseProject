using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InfoManage
{
    public partial class Form1 : Form
    {   
        public Form1()
        {
            InitializeComponent();            
        }

        private void check_username_password()
        {
            GlobalVar.g_strsno = textBox_sno.Text;

            string sqlstr = "SELECT sno, password FROM Student WHERE sno='" + textBox_sno.Text + "' AND password='" + textBox_pass.Text + "'";
            SqlDataReader reader = GlobalVar.getReader(sqlstr);

            if (reader.Read())
            {
                MessageBox.Show("祝贺你,登陆成功!");
                this.Hide();

                Form3 form = new Form3();  
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("用户名/密码,登陆失败!");
                textBox_sno.Clear();
                textBox_pass.Clear();
                label_name.Text = "";

                textBox_sno.Focus();
            }

            GlobalVar.closeConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            check_username_password();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GlobalVar.getConnection();
            if (GlobalVar.g_isConnect)
                MessageBox.Show("数据库连接成功!");
            else
                MessageBox.Show("数据库连接失败!");

        }

        private void textBox_sno_Leave(object sender, EventArgs e)
        { 
            string sqlstr = "SELECT sname FROM Student WHERE sno = '" + textBox_sno.Text + "'";
            SqlDataReader reader = GlobalVar.getReader(sqlstr);
            if (reader.Read())
            {
                label_name.Text = reader.GetString(0);               
            }
            else
            {
                label_name.Text = "学号不存在";
            }

            GlobalVar.closeConnection();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            textBox_sno.Focus();
        }

        private void textBox_pass_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                check_username_password();
            }
        }

        private void textBox_sno_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
