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
        /// 取得所有 PorductName 和 PorductID
        /// </summary>
        /// <returns></returns>
        public JsonResult ReadPorductList()
        {
            var productService = new eSalesService.ProductService(this.GetDBConnectionString());
            
            var data =  productService.GetProductList();
            
            return this.Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得所有 PorductName 和 PorductID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ReadPorductById(int id)
        {
            var productService = new eSalesService.ProductService(this.GetDBConnectionString());

            var data = productService.GetProductById(id);

            return this.Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得所有 PorductName 和 PorductID
        /// </summary>
        /// <returns></returns>
        public JsonResult ReadCustomerList()
        {
            eSalesService.CusService cusService = new eSalesService.CusService(this.GetDBConnectionString());
            
            var data = cusService.GetCusNameData();

            return this.Json(data, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 取得所有 PorductName 和 PorductID
        /// </summary>
        /// <returns></returns>
        public JsonResult ReadEmployeeList()
        {
            eSalesService.EmpService empService = new eSalesService.EmpService(this.GetDBConnectionString());
            
            var data = empService.GetEmpNameData();

            return this.Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 取得所有 PorductName 和 PorductID
        /// </summary>
        /// <returns></returns>
        public JsonResult ReadShipperList()
        {
            eSalesService.ShipService shipService = new eSalesService.ShipService(this.GetDBConnectionString());
            
            var data = shipService.GetShipperNameData();

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
            result.OrderDate = condition.OrderDate;
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
            var orderService = new eSalesService.Order2Service(this.GetDBConnectionString());

            var data = orderService.GetOrderDetailById(id);

            return this.Json(data, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 新增訂單資訊
        /// </summary>
        /// <param name="orderData">訂單資訊</param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult InsertOrder(OrderDetailViewModel orderData)
        {
            var orderService = new eSalesService.Order2Service(this.GetDBConnectionString());
            try
            {
                var result = new OrderDetailViewModel();

                result.CustId = Int32.Parse(orderData.CustName);
                result.EmpId = Int32.Parse(orderData.EmpName);
                result.OrderDate = orderData.OrderDate;
                result.RequiredDate = orderData.RequiredDate;
                result.ShippedDate = orderData.ShippedDate;
                result.ShipperId = Int32.Parse(orderData.ShipperName);
                result.Freight = orderData.Freight == null ? 0 : orderData.Freight;
                result.ShipCountry = orderData.ShipCountry == null ? string.Empty : orderData.ShipCountry;
                result.ShipCity = orderData.ShipCity == null ? string.Empty : orderData.ShipCity;
                result.ShipRegion = orderData.ShipRegion == null ? string.Empty : orderData.ShipRegion;
                result.ShipPostalCode = orderData.ShipPostalCode == null ? string.Empty : orderData.ShipPostalCode;
                result.ShipAddress = orderData.ShipAddress == null ? string.Empty : orderData.ShipAddress ;
                result.Products = orderData.Products;

                var error = new eSaleModel.ViewModel.ErrorMsg();
                error.Orderid = orderService.InsertOrder(result);
                error.State = true;

                return this.Json(error, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var error = new eSaleModel.ViewModel.ErrorMsg();
                error.Describe = "尚未填寫完成";
                error.State = false;

                return this.Json(error, JsonRequestBehavior.AllowGet);
            }

        }




        /// <summary>
        /// 修改訂單資訊
        /// </summary>
        /// <param name="orderData">訂單資訊</param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult UpdateOrder(OrderDetailViewModel orderData)
        {
            var orderService = new eSalesService.Order2Service(this.GetDBConnectionString());
            try
            {
                var result = new OrderDetailViewModel();

                result.CustId = Int32.Parse(orderData.CustName);
                result.EmpId = Int32.Parse(orderData.EmpName);
                result.OrderDate = orderData.OrderDate;
                result.RequiredDate = orderData.RequiredDate;
                result.ShippedDate = orderData.ShippedDate;
                result.ShipperId = Int32.Parse(orderData.ShipperName);
                result.OrderId = orderData.OrderId;
                result.Freight = orderData.Freight == null ? 0 : orderData.Freight;
                result.ShipCountry = orderData.ShipCountry == null ? string.Empty : orderData.ShipCountry;
                result.ShipCity = orderData.ShipCity == null ? string.Empty : orderData.ShipCity;
                result.ShipRegion = orderData.ShipRegion == null ? string.Empty : orderData.ShipRegion;
                result.ShipPostalCode = orderData.ShipPostalCode == null ? string.Empty : orderData.ShipPostalCode;
                result.ShipAddress = orderData.ShipAddress == null ? string.Empty : orderData.ShipAddress;
                result.ShipName = orderData.ShipName == null ? string.Empty : orderData.ShipName;
                result.Products = orderData.Products;
                
                var msg = orderService.UpdateOrder(result);

                return this.Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var error = new eSaleModel.ViewModel.ErrorMsg();
                error.Describe = "尚未填寫完成";
                error.State = false;

                return this.Json(error, JsonRequestBehavior.AllowGet);
            }

        }


        /// <summary>
        /// 修改訂單資訊
        /// </summary>
        /// <param name="orderData">訂單資訊</param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteOrder(OrderDetailViewModel orderData)
        {
            var orderService = new eSalesService.Order2Service(this.GetDBConnectionString());
            try
            {
                var result = new OrderDetailViewModel();
                
                result.OrderId = orderData.OrderId;

                var msg = orderService.DeleteOrder(result);

                return this.Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var error = new eSaleModel.ViewModel.ErrorMsg();
                error.Describe = "尚未填寫完成";
                error.State = false;

                return this.Json(error, JsonRequestBehavior.AllowGet);
            }

        }





    }
}