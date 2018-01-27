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
    public partial class Form3 : Form
    {
        int[] arraySno;
        int[] arrayCno;
        int selectSno;
        int selectCno;


        public Form3()
        {
            InitializeComponent();
        }

        //private string g_strsno = "";
        //public string strSno
        //{
        //    get
        //    {
        //        return this.g_strsno;
        //    }
        //    set { this.g_strsno = value; }
        //}

        //private string g_strconn = "";
        //public string strConn
        //{
        //    get
        //    {
        //        return this.g_strconn;
        //    }
        //    set { this.g_strconn = value; }
        //}

        private void LoadDepartmentFromDB()
        {
            arraySno = new int[100];
            arrayCno = new int[100];

            int i = 0;
            string sqlstr = "SELECT sno,sname FROM Student";

            SqlDataReader reader = GlobalVar.getReader(sqlstr);
            while (reader.Read())
            {
                comboBox_name.Items.Add(reader.GetString(1));
                arraySno[i++] = reader.GetInt32(0);
            }

            comboBox_name.Items.Add("All");
            GlobalVar.closeConnection();
            
            reader.Close();

            i = 0;
            sqlstr = "SELECT cno,cname FROM Course";
            reader = GlobalVar.getReader(sqlstr);
            while (reader.Read())
            {
                comboBox_course.Items.Add(reader.GetString(1));
                arrayCno[i++] = reader.GetInt32(0);
            }

            comboBox_course.Items.Add("All");

            reader.Close();
            GlobalVar.closeConnection();

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadDepartmentFromDB();
            LoadCurrentStudentScore();
        }

        private void SearchResultShow(string sno, string cno)
        {

        }

        private void LoadCurrentStudentScore()
        {
            string sqlStr = "SELECT Student.sno, sname, Course.cno, cname, score FROM SC, Student, Course WHERE SC.sno = Student.sno AND Course.cno = SC.cno AND SC.sno = '" + GlobalVar.g_strsno + "'";
            dataGridView1.DataSource = GlobalVar.getDataset(sqlStr);

            SqlDataReader reader = GlobalVar.getReader(sqlStr);
            if (reader.Read())
            {
                comboBox_name.Text = reader.GetString(1);
                comboBox_course.Text = reader.GetString(3);
                textBox_score.Text = reader.GetInt32(4).ToString();
            }

            GlobalVar.closeConnection();
        }

        private void UpdateSCDataGrid()
        {
            string sqlStr = "SELECT Student.sno, sname, Course.cno, cname, score FROM SC, Student, Course WHERE SC.sno = Student.sno AND Course.cno = SC.cno AND SC.sno = '"+arraySno[comboBox_name.SelectedIndex]+"' ";
            dataGridView1.DataSource = GlobalVar.getDataset(sqlStr);
            GlobalVar.closeConnection();
        }

        private bool IsItemRepeat()
        {
            string studentname = "%" + comboBox_name.Text.Trim() + "%";
            string coursename = "%" + comboBox_course.Text.Trim() + "%";

            string sqlStr = "SELECT Student.sno, sname, Course.cno, cname, score FROM SC, Student, Course WHERE SC.sno = Student.sno AND Course.cno = SC.cno " +
                "AND cname LIKE '" + coursename + "' AND sname LIKE '" + studentname + "'";
            SqlDataReader reader = GlobalVar.getReader(sqlStr);

            if (reader.Read())
            {
                GlobalVar.closeConnection();
                return true;
            }

            GlobalVar.closeConnection();

            return false;
        }

        private void button_add_Click(object sender, EventArgs e)
        {

            if (IsItemRepeat())
            {
                MessageBox.Show("该记录已经存在!");
                return;
            }

            string sqlstr = "INSERT INTO SC(sno, cno, score) VALUES('" + arraySno[comboBox_name.SelectedIndex] + "', '" + arrayCno[comboBox_course.SelectedIndex] + "', '" + textBox_score.Text + "')";

            int result = GlobalVar.getCommand(sqlstr);

            if (result > 0)
                MessageBox.Show("成绩添加成功!");
            UpdateSCDataGrid();

        }

        private void button_search_Click(object sender, EventArgs e)
        {
            string studentname = "%" + comboBox_name.Text.Trim() + "%";
            string coursename = "%" + comboBox_course.Text.Trim() + "%";

            if (comboBox_name.Text.Trim() == "All")
                studentname = "%";

            if (comboBox_course.Text.Trim() == "All")
                coursename = "%";

            string sqlStr = "SELECT Student.sno, sname, Course.cno, cname, score FROM SC, Student, Course WHERE SC.sno = Student.sno AND Course.cno = SC.cno " +
                "AND cname LIKE '" + coursename + "' AND sname LIKE '" + studentname + "'";

            dataGridView1.DataSource = GlobalVar.getDataset(sqlStr);

            GlobalVar.closeConnection();
        }

        private void button_update_Click(object sender, EventArgs e)
        {

            string sqlStr = "UPDATE SC SET score = '" + textBox_score.Text + "' WHERE sno = '" + selectSno + "' AND cno = '" + selectCno + "'";
            int result = GlobalVar.getCommand(sqlStr);

            if (result > 0)
                MessageBox.Show("成绩更新成功!");            
            
            UpdateSCDataGrid();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            string sqlStr = "DELETE FROM SC WHERE sno = '" + selectSno + "' AND cno = '" + selectCno + "'";
            int result = GlobalVar.getCommand(sqlStr);

            if (result > 0)
                MessageBox.Show("成绩删除成功!");
            UpdateSCDataGrid();
        }       

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            comboBox_name.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            comboBox_course.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            textBox_score.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();

            selectSno = Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            selectCno = Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
        }

        private void button_quit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            string message = "确定要退出吗?";
            string caption = "系统对话框";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(this, message, caption, buttons,
            MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }

        }

    }

}
