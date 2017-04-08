using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace eSaleDao
{
    public class CusDao
    {
        /// <summary>
        /// Database 連線字串
        /// </summary>
        public string DbConn { get; set; }
        /// <summary>
        /// CusDao 建構子
        /// </summary>
        /// <param name="dbConn"></param>
        public CusDao(string dbConn)
        {
            this.DbConn = dbConn;
        }
        public DataTable GetCusData()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT * FROM Sales.Customers";
            using (SqlConnection conn = new SqlConnection(this.DbConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return dt;
        }

    }
}
