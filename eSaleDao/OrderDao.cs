using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace eSaleDao
{
    public class OrderDao
    {
        /// <summary>
        /// Database 連線字串
        /// </summary>
        public string DbConn { get; set; }
        /// <summary>
        /// OrderDao 建構子
        /// </summary>
        /// <param name="dbConn"></param>
        public OrderDao(string dbConn)
        {
            this.DbConn = dbConn;
        }
        /// <summary>
        /// 依訂單id取得訂單資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public eSaleModel.Order GetOrderById(string id)
        {
            
            return new eSaleModel.Order()
            { OrderId = 001, CustId = "C002", CustName = "王小明" };
        }

        public DataTable GetOrderByCondition(eSaleModel.Order condition)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT * FROM Sales.Orders AS a JOIN Sales.Customers AS b ON a.CustomerID =b.CustomerID WHERE 
                                                            (OrderId=@OrderId OR @OrderId = 0 ) AND 
                                                            (b.CompanyName LIKE @CustName OR @CustName IS NULL ) AND
                                                            (EmployeeId = @EmpId OR @EmpId = 0 ) AND
                                                            (ShipperId=@ShipperId OR @ShipperId = 0 ) AND
                                                            (Orderdate=@Orderdate OR @Orderdate = '1780-01-01' ) AND
                                                            (RequiredDate=@RequireDdate OR @RequireDdate = '1780-01-01' ) AND
                                                            (ShippedDate=@ShippedDate OR @ShippedDate = '1780-01-01' ) ";

            using (SqlConnection conn = new SqlConnection(this.DbConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", condition.OrderId == null ? 0 : condition.OrderId));
                cmd.Parameters.Add(new SqlParameter("@CustName", condition.CustName == null ? string.Empty : condition.CustName));
                cmd.Parameters.Add(new SqlParameter("@EmpId", condition.EmpId == null ? 0 : condition.EmpId));
                cmd.Parameters.Add(new SqlParameter("@ShipperId", condition.ShipperId == null ? 0 : condition.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@Orderdate", condition.Orderdate == null ? Convert.ToDateTime("1780-01-01") : condition.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@RequireDdate", condition.RequireDdate == null ? Convert.ToDateTime("1780-01-01") : condition.RequireDdate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", condition.ShippedDate == null ? Convert.ToDateTime("1780-01-01") : condition.ShippedDate));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return dt;
        }
    }
}
