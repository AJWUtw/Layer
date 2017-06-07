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
            string sql = @"SELECT * FROM Sales.OrderDetails a JOIN Production.Products b ON a.ProductID = b.ProductID WHERE OrderId=@OrderId ";
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
        /// 取得所有訂單資料
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetOrder()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT * FROM Sales.Orders AS a JOIN Sales.Customers AS b ON a.CustomerID =b.CustomerID ";
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
        /// 依篩選條件取得訂單資料
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetOrderByCondition(eSaleModel.Order condition)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT * FROM Sales.Orders AS a JOIN Sales.Customers AS b ON a.CustomerID =b.CustomerID WHERE 
                                                            (CAST(OrderId as CHAR) LIKE @OrderId OR @OrderId IS NULL) AND 
                                                            (b.CompanyName LIKE @CustName OR @CustName IS NULL ) AND
                                                            (EmployeeId = @EmpId OR @EmpId = 0 ) AND
                                                            (ShipperId=@ShipperId OR @ShipperId = 0 ) AND
                                                            (OrderDate=@OrderDate OR @OrderDate = '1780-01-01' ) AND
                                                            (RequiredDate=@RequiredDate OR @RequiredDate = '1780-01-01' ) AND
                                                            (ShippedDate=@ShippedDate OR @ShippedDate = '1780-01-01' ) ";
            using (SqlConnection conn = new SqlConnection(this.DbConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", condition.O_OrderId == null ? string.Empty : condition.O_OrderId));
                cmd.Parameters.Add(new SqlParameter("@CustName", condition.CustName == null ? string.Empty : condition.CustName));
                cmd.Parameters.Add(new SqlParameter("@EmpId", condition.EmpId == null ? 0 : condition.EmpId));
                cmd.Parameters.Add(new SqlParameter("@ShipperId", condition.ShipperId == null ? 0 : condition.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@OrderDate", condition.OrderDate == null ? Convert.ToDateTime("1780-01-01") : condition.OrderDate));
                cmd.Parameters.Add(new SqlParameter("@RequiredDate", condition.RequiredDate == null ? Convert.ToDateTime("1780-01-01") : condition.RequiredDate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", condition.ShippedDate == null ? Convert.ToDateTime("1780-01-01") : condition.ShippedDate));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// 新增訂單資料
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
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
							@CustomerID,@EmployeeID,@OrderDate,@Requireddate,@Shippeddate,@Shipperid,@Freight,
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
                cmd.Parameters.Add(new SqlParameter("@OrderDate", order.OrderDate));
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
                    if (orderDetail.Qty > 0)
                    {
                        var aa = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    var aaa = e;
                }
            }
            return dt;
        }

        /// <summary>
        /// 新增訂單資料 SqlTransaction.Rollback
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int InsertOrderInSqlTrans(eSaleModel.Order2.OrderDetailViewModel order)
        {
            string sql_1 = @" Insert INTO Sales.Orders
                        (
							CustomerID,EmployeeID,orderdate,requireddate,shippeddate,shipperid,freight,
							shipname,shipaddress,shipcity,shipregion,shippostalcode,shipcountry
						)
                    OUTPUT INSERTED.OrderID 
						VALUES
						(
							@CustomerID,@EmployeeID,@Orderdate,@RequiredDate,@Shippeddate,@Shipperid,@Freight,
							@Shipname,@Shipaddress,@Shipcity,@Shipregion,@Shippostalcode,@Shipcountry
						)";
            string sql_2 = @" Insert INTO Sales.OrderDetails
						 (
							OrderID,ProductID,UnitPrice,Qty,Discount
						)
						VALUES
						(
							@OrderID,@ProductID,@UnitPrice,@Qty,@Discount
						)";
            int orderId=0;
            using (SqlConnection conn = new SqlConnection(this.DbConn))
            {
                conn.Open();

                SqlCommand command = conn.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = conn.BeginTransaction("SampleTransaction");

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = conn;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = sql_1;
                    command.Parameters.Add(new SqlParameter("@CustomerID", order.CustId));
                    command.Parameters.Add(new SqlParameter("@EmployeeID", order.EmpId));
                    command.Parameters.Add(new SqlParameter("@OrderDate", order.OrderDate));
                    command.Parameters.Add(new SqlParameter("@RequiredDate", order.RequiredDate));
                    command.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate));
                    command.Parameters.Add(new SqlParameter("@ShipperId", order.ShipperId ));
                    command.Parameters.Add(new SqlParameter("@Freight", order.Freight));
                    command.Parameters.Add(new SqlParameter("@Shipname", "aa"));
                    command.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress));
                    command.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity));
                    command.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion));
                    command.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode));
                    command.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry));
                    
                    object newId = command.ExecuteScalar();
                    orderId = Convert.ToInt32(newId);

                    command.CommandText = sql_2;

                    if ( order.Products!=null)
                    {
                        for (int i = 0; i < order.Products.Count; i++)
                        {
                            if (order.Products[i].Qty > 0)
                            {
                                command.Parameters.Clear();
                                command.Parameters.Add(new SqlParameter("@OrderId", orderId));
                                command.Parameters.Add(new SqlParameter("@ProductID", order.Products[i].ProductName.ProductId));
                                command.Parameters.Add(new SqlParameter("@UnitPrice", order.Products[i].UnitPrice));
                                command.Parameters.Add(new SqlParameter("@Qty", order.Products[i].Qty));
                                command.Parameters.Add(new SqlParameter("@Discount", order.Products[i].Discount == null ? false : order.Products[i].Discount));

                                command.ExecuteNonQuery();
                            }
                        }
                    }

                            // Attempt to commit the transaction.
                    transaction.Commit();
                    Console.WriteLine("Both records are written to database.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
                
                conn.Close();
            }
            return orderId;

        }

        /// <summary>
        /// 修改訂單資料
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int UpdateOrder(eSaleModel.Order order)
        {
            string sql = @" Update Sales.Orders SET
							    CustomerID=@CustomerID,
                                EmployeeID=@EmployeeID,
                                orderdate=@OrderDate,
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
                cmd.Parameters.Add(new SqlParameter("@OrderDate", order.OrderDate));
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
                    var aa = cmd.ExecuteNonQuery();
                    return aa;
               
            }

        }


        /// <summary>
        /// 新增訂單資料 SqlTransaction.Rollback
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Boolean UpdateOrderInSqlTrans(eSaleModel.Order2.OrderDetailViewModel order)
        {
            string sql_1 = @" Update Sales.Orders SET
							    CustomerID=@CustomerID,
                                EmployeeID=@EmployeeID,
                                orderdate=@OrderDate,
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
            string sql_2 = @"DELETE FROM Sales.OrderDetails WHERE OrderId=@OrderId";
            string sql_3 = @" Insert INTO Sales.OrderDetails
						 (
							OrderID,ProductID,UnitPrice,Qty,Discount
						)
						VALUES
						(
							@OrderID,@ProductID,@UnitPrice,@Qty,@Discount
						)";
            int orderId = 0;
            using (SqlConnection conn = new SqlConnection(this.DbConn))
            {
                conn.Open();

                SqlCommand command = conn.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = conn.BeginTransaction("SampleTransaction");

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = conn;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = sql_1;
                    command.Parameters.Add(new SqlParameter("@CustomerID", order.CustId));
                    command.Parameters.Add(new SqlParameter("@EmployeeID", order.EmpId));
                    command.Parameters.Add(new SqlParameter("@OrderDate", order.OrderDate));
                    command.Parameters.Add(new SqlParameter("@RequireDdate", order.RequiredDate));
                    command.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate));
                    command.Parameters.Add(new SqlParameter("@ShipperId", order.ShipperId));
                    command.Parameters.Add(new SqlParameter("@Freight", order.Freight));
                    command.Parameters.Add(new SqlParameter("@Shipname", order.ShipName));
                    command.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress));
                    command.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity));
                    command.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion));
                    command.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode));
                    command.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry));
                    command.Parameters.Add(new SqlParameter("@OrderId", order.OrderId));

                    command.ExecuteScalar();


                    command.CommandText = sql_2;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@OrderId", order.OrderId));
                    command.ExecuteScalar();


                    command.CommandText = sql_3;

                    if (order.Products.Count > 0)
                    {
                        for (int i = 0; i < order.Products.Count; i++)
                        {
                            if (order.Products[i].Qty > 0)
                            {
                                command.Parameters.Clear();
                                command.Parameters.Add(new SqlParameter("@OrderId", order.OrderId));
                                command.Parameters.Add(new SqlParameter("@ProductID", order.Products[i].ProductName.ProductId));
                                command.Parameters.Add(new SqlParameter("@UnitPrice", order.Products[i].UnitPrice));
                                command.Parameters.Add(new SqlParameter("@Qty", order.Products[i].Qty));
                                command.Parameters.Add(new SqlParameter("@Discount", order.Products[i].Discount == null ? false : order.Products[i].Discount));

                                command.ExecuteNonQuery();
                            }
                        }
                    }

                    // Attempt to commit the transaction.
                    transaction.Commit();
                    Console.WriteLine("Both records are written to database.");

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                        return false;

                    }

                    return false;
                }

                conn.Close();
            }

        }

        /// <summary>
        /// 刪除訂單資料
        /// </summary>
        /// <param name="orderDetail"></param>
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
        /// <summary>
        /// 刪除訂單明細
        /// </summary>
        /// <param name="orderDetail"></param>
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


        /// <summary>
        /// 刪除訂單資料
        /// </summary>
        /// <param name="orderDetail"></param>
        public Boolean DeleteOrderInSqlTran(eSaleModel.Order2.OrderDetailViewModel order)
        {
            string sql_1 = @"DELETE FROM Sales.OrderDetails WHERE OrderId=@OrderId";
            string sql_2 = @"DELETE FROM Sales.Orders WHERE OrderId=@OrderId";

            int orderId = 0;
            using (SqlConnection conn = new SqlConnection(this.DbConn))
            {
                conn.Open();

                SqlCommand command = conn.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = conn.BeginTransaction("SampleTransaction");

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = conn;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = sql_1;
                    command.Parameters.Add(new SqlParameter("@OrderId", order.OrderId));
                    command.ExecuteScalar();

                    command.CommandText = sql_2;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@OrderId", order.OrderId));
                    command.ExecuteScalar();

                    // Attempt to commit the transaction.
                    transaction.Commit();
                    Console.WriteLine("Both records are written to database.");

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                        return false;

                    }

                    return false;
                }

                conn.Close();
            }

        }


    }
}
