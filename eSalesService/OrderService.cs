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
        public eSaleModel.Order GetOrderById(string id)
        {
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            return orderDao.GetOrderById(id);
        }
        
        
        public List<eSaleModel.Order> GetOrderByCondition(eSaleModel.Order condition)
        {
            List<eSaleModel.Order> result = new List<eSaleModel.Order>();
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            var datalist = orderDao.GetOrderByCondition(condition);
            
            return this.MapOrderStore(datalist);

        }
        private List<eSaleModel.Order> MapOrderStore(DataTable empData)
        {
            List<eSaleModel.Order> result = new List<eSaleModel.Order>();


            foreach (DataRow row in empData.Rows)
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
        /// 依訂單id取得訂單資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int InsertOrder(eSaleModel.Order data)
        {
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            return orderDao.InsertOrder(data);
        }

        /// <summary>
        /// 依訂單id取得訂單資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void InsertOrderDetail(eSaleModel.OrderDetails data)
        {
            eSaleDao.OrderDao orderDao = new eSaleDao.OrderDao(this.DbConn);
            orderDao.InsertOrderDetail(data);
            
        }


    }
}
