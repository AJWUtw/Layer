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

        [HttpGet]
        public JsonResult GetOrder(eSaleModel.Order condition)
        {
            var result = new eSaleModel.Order();
            var orderService = new eSalesService.OrderService(this.GetDBConnectionString());
            var store = new eSaleModel.Store();

            result.OrderId = 0;
            result.CustName = "%"+ null+ "%";
            result.EmpId = 0;
            result.ShipperId = 0;
            result.Orderdate = null;
            result.RequireDdate = null;
            result.ShippedDate = null;

            store.identifier = "OrderId";
            store.items = orderService.GetOrderByCondition(result);

            return this.Json(store, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 取得訂單資訊
        /// </summary>
        /// <param name="condition">篩選條件</param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetOrderByCondition(eSaleModel.Order condition)
        {
            var result = new eSaleModel.Order();
            var orderService = new eSalesService.OrderService(this.GetDBConnectionString());
            var store = new eSaleModel.Store();

            result.OrderId = condition.OrderId;
            result.CustName = "%" + (condition.CustName)+ "%" ;
            result.EmpId = Int32.Parse(condition.EmpName);
            result.ShipperId = Int32.Parse(condition.ShipperName);
            result.Orderdate = condition.Orderdate;
            result.RequireDdate = condition.RequireDdate;
            result.ShippedDate = condition.ShippedDate;


            store.identifier = "OrderId";
            store.items = orderService.GetOrderByCondition(result);

            return this.Json(store, JsonRequestBehavior.AllowGet);
        }
    }
}