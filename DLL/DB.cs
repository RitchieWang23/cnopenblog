using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DLL
{
    public class DB
    {
        public static string ConnString
        {//@"server=.\SQLEXPRESS;database=openblog;user id=cnopenblog;password=cnopenblog.com;"; 
            get { return System.Configuration.ConfigurationManager.AppSettings["connstring"]; }
        }

        /// <summary>
        /// ִ�д洢���̷���intֵ
        /// </summary>
        /// <param name="proc">�洢������</param>
        /// <param name="pars">��������</param>
        /// <returns></returns>
        public static int Procedure(string proc, SqlParameter[] pars)
        {
            object obj = Procedure2(proc, pars);
            int i = 0;
            if (obj != null)
                int.TryParse(obj.ToString(), out i);
            return i;
        }

        public static object Procedure2(string proc, SqlParameter[] pars)
        { 
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlCommand cmd = new SqlCommand(proc, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter par in pars)
                    cmd.Parameters.Add(par);
                conn.Open();

                object obj = null;
                try { obj = cmd.ExecuteScalar(); }
                catch { obj = null; }
                finally { conn.Close(); }

                return obj;
            }
        }

        /// <summary>
        /// �Ӵ洢���̻�ȡtable
        /// </summary>
        /// <param name="proc">�洢����</param>
        /// <param name="pars">��������</param>
        /// <returns></returns>
        public static DataTable TableFromProcedure(string proc, SqlParameter[] pars)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlCommand cmd = new SqlCommand(proc, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter par in pars)
                    cmd.Parameters.Add(par);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                DataSet ds = new DataSet();

                try { da.Fill(ds); }
                catch { return new DataTable(); }

                if (ds.Tables.Count > 0) return ds.Tables[0];
                else return new DataTable();
            }
        }

        public static DataSet GetDataSet(string sql)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        public static DataTable GetTable(string sql)
        {
            DataSet ds = GetDataSet(sql);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            else return new DataTable();
        }

        public static bool Execute(string sql)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                try { cmd.ExecuteNonQuery(); return true; }
                catch { return false; }
                finally { conn.Close(); }
            }
        }

        public static object GetValue(string sql)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                object obj = "";
                conn.Open();
                try { obj = cmd.ExecuteScalar(); return obj; }
                catch { return ""; }
                finally { conn.Close(); }
            }
        }
    }
}
