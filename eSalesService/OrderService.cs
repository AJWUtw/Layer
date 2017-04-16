using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web.Mvc;

namespace eSalesService
{
    public class OrderService
    {
        /// <summary>
        /// Database 連線字串
        /// </summary>
        public string DbConn { get; set; }
        /// <summary>
        /// OrderService 建構子
        /// </summary>
        /// <param name="dbConn">Database 連線字串</param>
        public OrderService(string dbConn)
        {
            this.DbConn = dbConn;
        }
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
        public List<eSaleModel.OrderDetails> GetOrderDetailById(int id)
        {
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            var datalist = orderDao.GetOrderDetailById(id);

            return this.MapOrderDetailData(datalist);
        }


        public List<eSaleModel.Order> GetOrderByCondition(eSaleModel.Order condition)
        {
            List<eSaleModel.Order> result = new List<eSaleModel.Order>();
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            var datalist = orderDao.GetOrderByCondition(condition);
            
            return this.MapOrderStore(datalist);

        }
        private List<eSaleModel.Order> MapOrderData(DataTable orderData)
        {
            List<eSaleModel.Order> result = new List<eSaleModel.Order>();

            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new eSaleModel.Order()
                {
                    OrderId = (int)row["OrderId"],
                    CustId = (int)row["CustomerId"],
                    CustName = row["CompanyName"].ToString(),
                    EmpId = (int)row["EmployeeId"],
                    O_Orderdate = row["Orderdate"].ToString(),
                    O_ShippedDate = row["ShippedDate"].ToString(),
                    O_RequiredDate = row["RequiredDate"].ToString(),
                    Orderdate = (DateTime)row["Orderdate"],
                    ShippedDate = (DateTime)row["ShippedDate"],
                    RequiredDate = (DateTime)row["RequiredDate"],
                    ShipperId = (int)row["ShipperId"],
                    Freight = (decimal)row["Freight"],

                    ShipName = String.IsNullOrEmpty(row["ShipName"].ToString()) ? "" : row["ShipName"].ToString(),
                    ShipAddress = row["ShipAddress"].ToString(),
                    ShipCity = row["ShipCity"].ToString(),
                    ShipRegion = row["ShipRegion"].ToString(),
                    ShipPostalCode = row["ShipPostalCode"].ToString(),
                    ShipCountry = row["ShipCountry"].ToString()

                });
            }
            var aa = result;
            return result;
        }

        private List<eSaleModel.OrderDetails> MapOrderDetailData(DataTable orderDetailData)
        {
            List<eSaleModel.OrderDetails> result = new List<eSaleModel.OrderDetails>();

            foreach (DataRow row in orderDetailData.Rows)
            {
                result.Add(new eSaleModel.OrderDetails()
                {
                    OrderId = (int)row["OrderId"],
                    ProductName= (int)row["ProductId"],
                    UnitPrice = (decimal)row["UnitPrice"],
                    Qty = (Int16)row["Qty"],
                    Sum = (Int16)row["Qty"] * (decimal)row["UnitPrice"]

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
                    OrderId = (int)row["OrderId"],
                    CustName = row["CompanyName"].ToString(),
                    O_Orderdate = row["Orderdate"].ToString(),
                    O_ShippedDate = row["ShippedDate"].ToString(),

                });
            }
            return result;
        }

        /// <summary>
        /// 新增訂單資料
        /// </summary>
        /// <param name="data">訂單資料</param>
        /// <returns></returns>
        public int InsertOrder(eSaleModel.Order data)
        {
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            return orderDao.InsertOrder(data);
        }

        /// <summary>
        /// 新增訂單明細
        /// </summary>
        /// <param name="data">訂單明細</param>
        /// <returns></returns>
        public void InsertOrderDetail(eSaleModel.OrderDetails data)
        {
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            orderDao.InsertOrderDetail(data);
            
        }

        /// <summary>
        /// 新增訂單資料
        /// </summary>
        /// <param name="data">訂單資料</param>
        /// <returns></returns>
        public int UpdateOrder(eSaleModel.Order data)
        {
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            return orderDao.UpdateOrder(data);

        }

        /// <summary>
        /// 新增訂單明細
        /// </summary>
        /// <param name="data">訂單明細</param>
        /// <returns></returns>
        public void DeleteOrder(eSaleModel.OrderDetails data)
        {
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            orderDao.DeleteOrder(data);

        }
        /// <summary>
        /// 新增訂單明細
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
