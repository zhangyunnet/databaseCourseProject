using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace InfoManage
{
    public class GlobalVar
    {
        public static string g_strsno;
        public static string g_strconn = "Data Source=.;Initial Catalog=studentinfo;Integrated Security=True";
        public static SqlConnection myConn;
        public static bool g_isConnect = false;
        public static SqlConnection getConnection()
        {
            myConn = new SqlConnection(g_strconn);            

            if (myConn.State != ConnectionState.Open)
            {
                myConn.Open();
            }
            
            if (myConn.State == ConnectionState.Open)
            {
                g_isConnect = true;
            }

            return myConn;
        }

        public static void closeConnection()
        {
            if (myConn.State == ConnectionState.Open)
            {
                myConn.Close();
                myConn.Dispose();
            }
                
        }

        public static SqlDataReader getReader(string strSql)
        {
            getConnection();
            SqlCommand cmd = new SqlCommand(strSql, myConn);
            SqlDataReader read = cmd.ExecuteReader();
            return read;
        }

        public static int getCommand(string strSql)
        {
            getConnection();
            SqlCommand cmd = new SqlCommand(strSql, myConn);
            int result = cmd.ExecuteNonQuery();
            cmd.Dispose();
            closeConnection();

            return result;
        }

        public static DataView getDataset(string strSql)
        {
            getConnection();
            SqlDataAdapter myda = new SqlDataAdapter(strSql, myConn);
            DataSet myds = new DataSet();
            myda.Fill(myds);

            return myds.Tables[0].DefaultView;
        }
    }
}
