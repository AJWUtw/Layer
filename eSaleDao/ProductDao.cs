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
    public class ProductDao
    {
        /// <summary>
        /// Database 連線字串
        /// </summary>
        public string DbConn { get; set; }
        /// <summary>
        /// ProductDao 建構子
        /// </summary>
        /// <param name="dbConn"></param>
        public ProductDao(string dbConn)
        {
            this.DbConn = dbConn;
        }
        /// <summary>
        /// 依訂單id取得商品資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetProductById(int id)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT ProductName,ProductId,UnitPrice FROM Production.Products WHERE ProductId=@ProductId";

            using (SqlConnection conn = new SqlConnection(this.DbConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@ProductId", id == null ? -1 : id));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// 取得商品清單
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetProductNameList()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT ProductName,ProductId FROM Production.Products";

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
        /// <summary>
        /// 依條件篩選商品資料
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetProductByCondition(eSaleModel.Product condition)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT * FROM Production.Products WHERE  (ProductId=@ProductId OR @ProductId = 0 ) AND 
                                                            (ProductName LIKE @ProductName OR @ProductName IS NULL ) AND
                                                            (SupplierId=@SupplierId OR @SupplierId = 0 ) AND
                                                            (CategoryId=@CategoryId OR @CategoryId = 0 ) ";

            using (SqlConnection conn = new SqlConnection(this.DbConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@ProductId", condition.ProductId == null ? 0 : condition.ProductId));
                cmd.Parameters.Add(new SqlParameter("@ProductName", condition.ProductName == null ? string.Empty : condition.ProductName));
                cmd.Parameters.Add(new SqlParameter("@SupplierId", condition.SupplierId == null ? 0 : condition.SupplierId));
                cmd.Parameters.Add(new SqlParameter("@CategoryId", condition.CategoryId == null ? 0 : condition.CategoryId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return dt;
        }
    }
}
