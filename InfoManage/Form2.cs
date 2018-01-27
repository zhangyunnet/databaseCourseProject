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
    public partial class Form2 : Form
    {
        int[] arrayDno;
   
        
        public Form2()
        {
            InitializeComponent();
        }

        private void LoadDepartmentFromDB()
        { 
            arrayDno = new int[20];
            int i = 0;
            string sqlstr = "SELECT dno,dname FROM Dept";

            SqlDataReader reader = GlobalVar.getReader(sqlstr);
            while (reader.Read())
            {
                comboBox_dept.Items.Add(reader.GetString(1));
                arrayDno[i++] = reader.GetInt32(0);
            }

            GlobalVar.closeConnection();       
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {            
            LoadDepartmentFromDB();
        }

        private bool CheckInsertItems()
        {
            bool flag = true;

            string inputErrors = "";

            if (textBox_password.Text.Length < 6)
            {
                inputErrors = inputErrors + "密码长度小于6!";
                textBox_password.Text = "";
                textBox_password2.Text = "";
                flag = false;
            }

            if (textBox_password.Text != textBox_password2.Text)
            {
                inputErrors = inputErrors + "两次密码输入不一致!";
                textBox_password.Text = "";
                textBox_password2.Text = "";
                flag = false;
            }

            if (inputErrors.Trim().Length > 0)
                MessageBox.Show(inputErrors);

            return flag;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!CheckInsertItems())
                return;            

            string sqlstr = "INSERT INTO Student(sno, password, sname, gender, birth, address, dno) VALUES('" + textBox_sno.Text + "', '" + textBox_password.Text + "', '" + textBox_name.Text + "', '" + comboBox_gender.Text + "',  '" + dateTimePicker1.Value + "', '" + textBox_address.Text + "', '"+ arrayDno[comboBox_dept.SelectedIndex] +"')";
            
            int result = GlobalVar.getCommand(sqlstr);

            if (result > 0)
                MessageBox.Show("添加学生成功!");

            GlobalVar.closeConnection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox_address.Text = "";
            textBox_name.Text = "";
            textBox_password.Text = "";
            textBox_password2.Text = "";
            textBox_sno.Text = "";
            comboBox_gender.Text = "";
            comboBox_dept.Text = "";
        }   

    }

}
