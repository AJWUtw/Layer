using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace eSaleDao
{
    public class ShipDao
    {
        /// <summary>
        /// Database 連線字串
        /// </summary>
        public string DbConn { get; set; }
        /// <summary>
        /// ShipDao 建構子
        /// </summary>
        /// <param name="dbConn"></param>
        public ShipDao(string dbConn)
        {
            this.DbConn = dbConn;
        }
        public DataTable GetShipData()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT * FROM Sales.Shippers";
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
