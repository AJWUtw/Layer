using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Layer.Controllers
{
    public class OrderController : Controller
    {
        /// <summary>
        /// 取得 DataBase 連線
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }
        // GET: Order
        public ActionResult Index()
        {
            eSalesService.OrderService orderService = new eSalesService.OrderService(this.GetDBConnectionString());
            eSalesService.EmpService empService = new eSalesService.EmpService(this.GetDBConnectionString());
            eSalesService.ShipService shipService = new eSalesService.ShipService(this.GetDBConnectionString());

            var tmp = orderService.GetOrderById("id");
            ViewBag.data = tmp.CustId+" "+tmp.CustName+" "+tmp.OrderId;
            ViewBag.EmpNameData = empService.GetEmpNameData();
            ViewBag.ShipperNameData = shipService.GetShipperNameData();
            return View();
        }
        [HttpPost()]
        public ActionResult Index(eSaleModel.Order order)
        {

            return View();
        }

        /// <summary>
        /// 取得訂單資訊
        /// </summary>
        /// <param name="condition">篩選條件</param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult GetOrderByCondition(eSaleModel.Order condition)
        {
            var result = new eSaleModel.Order();
            var orderService = new eSalesService.OrderService(this.GetDBConnectionString());
            result.OrderId = condition.OrderId;
            result.CustName = "%" + (condition.CustName)+ "%" ;
            result.EmpId = Int32.Parse(condition.EmpName);
            result.ShipperId = Int32.Parse(condition.ShipperName);
            result.Orderdate = condition.Orderdate;
            result.RequireDdate = condition.RequireDdate;
            result.ShippedDate = condition.ShippedDate;
            //return result;

            //return Content(condition.CustName, "text/html");
            return this.Json(orderService.GetOrderByCondition(result), JsonRequestBehavior.AllowGet);
        }
    }
}