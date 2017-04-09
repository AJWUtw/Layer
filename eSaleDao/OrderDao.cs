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
        public DataTable GetOrderById(int id)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT * FROM Sales.Orders AS a JOIN Sales.Customers AS b ON a.CustomerID =b.CustomerID 
                                                                                WHERE OrderId=@OrderId ";
            using (SqlConnection conn = new SqlConnection(this.DbConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", id));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// 依訂單id取得訂單資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetOrderDetailById(int id)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT * FROM Sales.OrderDetails WHERE OrderId=@OrderId ";
            using (SqlConnection conn = new SqlConnection(this.DbConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", id));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return dt;
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
                                                            (RequiredDate=@RequiredDate OR @RequiredDate = '1780-01-01' ) AND
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
                cmd.Parameters.Add(new SqlParameter("@RequiredDate", condition.RequiredDate == null ? Convert.ToDateTime("1780-01-01") : condition.RequiredDate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", condition.ShippedDate == null ? Convert.ToDateTime("1780-01-01") : condition.ShippedDate));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return dt;
        }
        public int InsertOrder(eSaleModel.Order order)
        {
            string sql = @" Insert INTO Sales.Orders
                        (
							CustomerID,EmployeeID,orderdate,requireddate,shippeddate,shipperid,freight,
							shipname,shipaddress,shipcity,shipregion,shippostalcode,shipcountry
						)
                    OUTPUT INSERTED.OrderID 
						VALUES
						(
							@CustomerID,@EmployeeID,@Orderdate,@Requireddate,@Shippeddate,@Shipperid,@Freight,
							@Shipname,@Shipaddress,@Shipcity,@Shipregion,@Shippostalcode,@Shipcountry
						)
						";
            int orderId;
            using (SqlConnection conn = new SqlConnection(this.DbConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@CustomerID", order.CustId));
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", order.EmpId));
                cmd.Parameters.Add(new SqlParameter("@Orderdate", order.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@RequireDdate", order.RequiredDate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@ShipperId", order.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@Freight", order.Freight));
                cmd.Parameters.Add(new SqlParameter("@Shipname", "aa"));
                cmd.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry));

                Console.WriteLine(cmd);
                object aa = cmd.ExecuteScalar();
                orderId = Convert.ToInt32(aa);

                conn.Close();
            }
            return orderId;

        }

        public DataTable InsertOrderDetail(eSaleModel.OrderDetails orderDetail)
        {
            DataTable dt = new DataTable();
            string sql = @" Insert INTO Sales.OrderDetails
						 (
							OrderID,ProductID,UnitPrice,Qty,Discount
						)
						VALUES
						(
							@OrderID,@ProductID,@UnitPrice,@Qty,@Discount
						)
						";
            using (SqlConnection conn = new SqlConnection(this.DbConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", orderDetail.OrderId));
                cmd.Parameters.Add(new SqlParameter("@ProductID", orderDetail.ProductId));
                cmd.Parameters.Add(new SqlParameter("@UnitPrice", orderDetail.UnitPrice));
                cmd.Parameters.Add(new SqlParameter("@Qty", orderDetail.Qty));
                cmd.Parameters.Add(new SqlParameter("@Discount", orderDetail.Discount == null ? 0 : orderDetail.Discount));
                //SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                //sqlAdapter.Fill(dt);
                try
                {
                    var aa = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    var aaa = e;
                }
            }
            return dt;
        }


        public int UpdateOrder(eSaleModel.Order order)
        {
            string sql = @" Update Sales.Orders SET
							    CustomerID=@CustomerID,
                                EmployeeID=@EmployeeID,
                                orderdate=@Orderdate,
                                requireddate=@Requireddate,
                                shippeddate=@Shippeddate,
                                shipperid=@Shipperid,
                                freight=@Freight,
							    shipname=@Shipname,
                                shipaddress=@Shipaddress,
                                shipcity=@Shipcity,
                                shipregion=@Shipregion,
                                shippostalcode=@Shippostalcode,
                                shipcountry=@Shipcountry 
                                WHERE OrderId=@OrderId ";
            int orderId;
            using (SqlConnection conn = new SqlConnection(this.DbConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@CustomerID", order.CustId));
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", order.EmpId));
                cmd.Parameters.Add(new SqlParameter("@Orderdate", order.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@RequireDdate", order.RequiredDate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@ShipperId", order.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@Freight", order.Freight));
                cmd.Parameters.Add(new SqlParameter("@Shipname", order.ShipName));
                cmd.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion == null ? "" : order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry));
                cmd.Parameters.Add(new SqlParameter("@OrderId", order.OrderId));

                Console.WriteLine(cmd);
                try
                {
                    var aa = cmd.ExecuteNonQuery();
                    return aa;
                }
                catch (Exception e)
                {
                    var aaa = e;
                    return 99;
                }
               
            }

        }

        public void DeleteOrder(eSaleModel.OrderDetails orderDetail)
        {
            string sqlD = "DELETE FROM Sales.Orders WHERE OrderId=@OrderId";
            using (SqlConnection conn = new SqlConnection(this.DbConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlD, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", orderDetail.OrderId));
                try
                {
                    var aa = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    var aaa = e;
                }
                
                conn.Close();
            }
            
        }

        public void DeleteOrderDetail(eSaleModel.OrderDetails orderDetail)
        {
            string sqlD = "DELETE FROM Sales.OrderDetails WHERE OrderId=@OrderId";
            using (SqlConnection conn = new SqlConnection(this.DbConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlD, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", orderDetail.OrderId));
                try
                {
                    var aa = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    var aaa = e;
                }

                conn.Close();
            }

        }
    }
}
