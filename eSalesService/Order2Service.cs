using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web.Mvc;
using eSaleModel.Order2;

namespace eSalesService
{
    public class Order2Service
    {
        /// <summary>
        /// Database 連線字串
        /// </summary>
        public string DbConn { get; set; }
        /// <summary>
        /// OrderService 建構子
        /// </summary>
        /// <param name="dbConn">Database 連線字串</param>
        public Order2Service(string dbConn)
        {
            this.DbConn = dbConn;
        }

        /*
         取得 DataTable 資料
        */
        /// <summary>
        /// 依訂單id取得訂單資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public eSaleModel.Order GetOrderById(int id)
        {
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            var datalist = orderDao.GetOrderById(id);

            return this.MapOrderData(datalist).FirstOrDefault();
        }

        /// <summary>
        /// 依訂單id取得訂單資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<OrderProductViewModel> GetOrderDetailById(int id)
        {
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            var datalist = orderDao.GetOrderDetailById(id);

            return this.MapOrderDetailData(datalist);
        }

        public List<eSaleModel.Order> GetOrder()
        {
            List<eSaleModel.Order> result = new List<eSaleModel.Order>();
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            var datalist = orderDao.GetOrder();

            return this.MapOrderData(datalist);

        }
        public List<eSaleModel.Order> GetOrderByCondition(eSaleModel.Order condition)
        {
            List<eSaleModel.Order> result = new List<eSaleModel.Order>();
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            var datalist = orderDao.GetOrderByCondition(condition);
            
            return this.MapOrderStore(datalist);

        }
        /*
         新增資料
        */

        /// <summary>
        /// 新增訂單資料
        /// </summary>
        /// <param name="data">訂單資料</param>
        /// <returns></returns>
        public int InsertOrder(OrderDetailViewModel data)
        {
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            return orderDao.InsertOrderInSqlTrans(data);
        }



        /*
         整理 DataTable 資料
        */
        private List<eSaleModel.Order> MapOrderData(DataTable orderData)
        {
            List<eSaleModel.Order> result = new List<eSaleModel.Order>();

            foreach (DataRow row in orderData.Rows)
            {
                try
                {
                    result.Add(new eSaleModel.Order()
                    {
                        OrderId = (int)row["OrderId"] == null ? 0 : (int)row["OrderId"],
                        CustId = (int)row["CustomerId"] == null ? 0 : (int)row["CustomerId"],
                        CustName = String.IsNullOrEmpty(row["CompanyName"].ToString()) ? "" : row["CompanyName"].ToString(),
                        EmpId = (int)row["EmployeeId"] == null ? 0 : (int)row["EmployeeId"],
                        OrderDate = row["OrderDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["Orderdate"],
                        ShippedDate = row["ShippedDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["ShippedDate"],
                        RequiredDate = row["RequiredDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["RequiredDate"],
                        ShipperId = (int)row["ShipperId"] == null ? 0 : (int)row["ShipperId"],
                        Freight = (decimal)row["Freight"] == null ? 0 : (decimal)row["Freight"],

                        ShipName = String.IsNullOrEmpty(row["ShipName"].ToString()) ? "" : row["ShipName"].ToString(),
                        ShipAddress = String.IsNullOrEmpty(row["ShipAddress"].ToString()) ? "" : row["ShipAddress"].ToString(),
                        ShipCity = String.IsNullOrEmpty(row["ShipCity"].ToString()) ? "" : row["ShipCity"].ToString(),
                        ShipRegion = String.IsNullOrEmpty(row["ShipRegion"].ToString()) ? "" : row["ShipRegion"].ToString(),
                        ShipPostalCode = String.IsNullOrEmpty(row["ShipPostalCode"].ToString()) ? "" : row["ShipPostalCode"].ToString(),
                        ShipCountry = String.IsNullOrEmpty(row["ShipCountry"].ToString()) ? "" : row["ShipCountry"].ToString()

                    });
                }
                catch (Exception e)
                {
                    var aa = e;

                }
                
            }
            return result;
        }

        private List<OrderProductViewModel> MapOrderDetailData(DataTable orderDetailData)
        {
            List<OrderProductViewModel> result = new List<OrderProductViewModel>();

            foreach (DataRow row in orderDetailData.Rows)
            {
                PName ProductName = new PName();
                ProductName.ProductId = (int)row["ProductId"];
                ProductName.ProductName = row["ProductName"].ToString();
                result.Add(new OrderProductViewModel()
                {
                    
                    ProductName = ProductName,
                    UnitPrice = (decimal)row["UnitPrice"],
                    Qty = (Int16)row["Qty"],
                    Sum = (Int16)row["Qty"] * (int)(decimal)row["UnitPrice"]

                });
            }
            return result;
        }
        private List<eSaleModel.Order> MapOrderStore(DataTable orderData)
        {
            List<eSaleModel.Order> result = new List<eSaleModel.Order>();

            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new eSaleModel.Order()
                {
                    OrderId = (int)row["OrderId"] == null ? 0 : (int)row["OrderId"],
                    CustId = (int)row["CustomerId"] == null ? 0 : (int)row["CustomerId"],
                    CustName = String.IsNullOrEmpty(row["CompanyName"].ToString()) ? "" : row["CompanyName"].ToString(),
                    EmpId = (int)row["EmployeeId"] == null ? 0 : (int)row["EmployeeId"],
                    OrderDate = row["OrderDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["Orderdate"],
                    ShippedDate = row["ShippedDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["ShippedDate"],
                    RequiredDate = row["RequiredDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["RequiredDate"],
                    ShipperId = (int)row["ShipperId"] == null ? 0 : (int)row["ShipperId"],
                    Freight = (decimal)row["Freight"] == null ? 0 : (decimal)row["Freight"],

                    ShipName = String.IsNullOrEmpty(row["ShipName"].ToString()) ? "" : row["ShipName"].ToString(),
                    ShipAddress = String.IsNullOrEmpty(row["ShipAddress"].ToString()) ? "" : row["ShipAddress"].ToString(),
                    ShipCity = String.IsNullOrEmpty(row["ShipCity"].ToString()) ? "" : row["ShipCity"].ToString(),
                    ShipRegion = String.IsNullOrEmpty(row["ShipRegion"].ToString()) ? "" : row["ShipRegion"].ToString(),
                    ShipPostalCode = String.IsNullOrEmpty(row["ShipPostalCode"].ToString()) ? "" : row["ShipPostalCode"].ToString(),
                    ShipCountry = String.IsNullOrEmpty(row["ShipCountry"].ToString()) ? "" : row["ShipCountry"].ToString()

                });
            }
            return result;
        }
        

        /// <summary>
        /// 修改訂單資料
        /// </summary>
        /// <param name="data">訂單資料</param>
        /// <returns></returns>
        public Boolean UpdateOrder(OrderDetailViewModel data)
        {
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);

            return orderDao.UpdateOrderInSqlTrans(data);

        }

        /// <summary>
        /// 刪除訂單
        /// </summary>
        /// <param name="data">訂單明細</param>
        /// <returns></returns>
        public Boolean DeleteOrder(OrderDetailViewModel data)
        {
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            return orderDao.DeleteOrderInSqlTran(data);
            
        }
        /// <summary>
        /// 刪除訂單明細
        /// </summary>
        /// <param name="data">訂單明細</param>
        /// <returns></returns>
        public void DeleteOrderDetail(eSaleModel.OrderDetails data)
        {
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            orderDao.DeleteOrderDetail(data);

        }


    }
}
