using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eSaleModel.Order2;

namespace Layer.Controllers
{
    public class Order2Controller : Controller
    {
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }
        // GET: Order2
        public ActionResult Index()
        {
            eSalesService.OrderService orderService = new eSalesService.OrderService(this.GetDBConnectionString());
            eSalesService.EmpService empService = new eSalesService.EmpService(this.GetDBConnectionString());
            eSalesService.CusService cusService = new eSalesService.CusService(this.GetDBConnectionString());
            eSalesService.ShipService shipService = new eSalesService.ShipService(this.GetDBConnectionString());


            ViewBag.EmpNameData = empService.GetEmpNameData();
            ViewBag.CustNameData = cusService.GetCusNameData();
            ViewBag.ShipperNameData = shipService.GetShipperNameData();
            return View();
        }

        public JsonResult Read()
        {
            var orderService = new eSalesService.Order2Service(this.GetDBConnectionString());
            

            var data = orderService.GetOrder();

            return this.Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 依據篩選條件取得訂單資訊
        /// </summary>
        /// <param name="condition">篩選條件</param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetOrderByCondition(eSaleModel.Order condition)
        {
            var result = new eSaleModel.Order();
            var orderService = new eSalesService.Order2Service(this.GetDBConnectionString());
            var store = new eSaleModel.Store();

            result.O_OrderId = "%" + Convert.ToString(condition.OrderId) + "%";
            result.CustName = "%" + (condition.CustName) + "%";
            result.EmpId = condition.EmpName == null ? 0 : Int32.Parse(condition.EmpName);
            result.ShipperId = condition.ShipperName == null ? 0 : Int32.Parse(condition.ShipperName);
            result.Orderdate = condition.Orderdate;
            result.RequiredDate = condition.RequiredDate;
            result.ShippedDate = condition.ShippedDate;


            var data = orderService.GetOrderByCondition(result);

            return this.Json(data, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 依OrderId取得訂單明細
        /// </summary>
        /// <param name="condition">篩選條件</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetOrderDetailById(int id)
        {
            var orderService = new eSalesService.OrderService(this.GetDBConnectionString());
            var store = new eSaleModel.Store();

            var data = orderService.GetOrderDetailById(id);

            return this.Json(data, JsonRequestBehavior.AllowGet);
        }







    }
}