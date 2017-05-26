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
        /// GET  Order
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
        /// <summary>
        /// 取得新增視窗
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInsertDialog()
        {
            
            eSalesService.EmpService empService = new eSalesService.EmpService(this.GetDBConnectionString());
            eSalesService.CusService cusService = new eSalesService.CusService(this.GetDBConnectionString());
            eSalesService.ShipService shipService = new eSalesService.ShipService(this.GetDBConnectionString());
            
            ViewBag.EmpNameData = empService.GetEmpNameData();
            ViewBag.CustNameData = cusService.GetCusNameData();
            ViewBag.ShipperNameData = shipService.GetShipperNameData();

            return PartialView();
        }
        /// <summary>
        /// 取得修改視窗
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUpdateDialog(int id)
        {


            eSalesService.OrderService orderService = new eSalesService.OrderService(this.GetDBConnectionString());
            eSalesService.EmpService empService = new eSalesService.EmpService(this.GetDBConnectionString());
            eSalesService.CusService cusService = new eSalesService.CusService(this.GetDBConnectionString());
            eSalesService.ShipService shipService = new eSalesService.ShipService(this.GetDBConnectionString());
            eSaleModel.Order orderData = new eSaleModel.Order();

            orderData = orderService.GetOrderById(id);

            ViewBag.OrderId = orderData.OrderId;
            ViewBag.OrderDate = orderData.OrderDate;
            ViewBag.RequiredDate = orderData.RequiredDate;
            ViewBag.ShippedDate = orderData.ShippedDate;
            ViewBag.Freight = orderData.Freight;
            ViewBag.ShipCountry = orderData.ShipCountry;
            ViewBag.ShipCity = orderData.ShipCity;
            ViewBag.ShipRegion = orderData.ShipRegion;
            ViewBag.ShipPostalCode = orderData.ShipPostalCode;
            ViewBag.ShipAddress = orderData.ShipAddress;
            ViewBag.ShipName = orderData.ShipAddress;

            //ViewBag.OrderId = "updateProductGrid" + id;

            ViewBag.EmpNameData = new SelectList(empService.GetEmpNameData(), "Value", "Text", orderData.EmpId);
            ViewBag.CustNameData = new SelectList(cusService.GetCusNameData(), "Value", "Text", orderData.CustId);
            ViewBag.ShipperNameData = new SelectList(shipService.GetShipperNameData(), "Value", "Text", orderData.ShipperId);

            return PartialView();
        }
        /// <summary>
        /// 取得商品清單
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public JsonResult GetProductList()
        {
            var result = new eSaleModel.Product();
            var productService = new eSalesService.ProductService(this.GetDBConnectionString());
            var store = new eSaleModel.Store();

            result.ProductId = 0;
            result.ProductName = "%" + null + "%";
            result.SupplierId = 0;
            result.CategoryId = 0;
            
            return this.Json(productService.GetProductByCondition(result), JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 取得商品名稱清單
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetProductNameList()
        {
            var productService = new eSalesService.ProductService(this.GetDBConnectionString());
            var store = new eSaleModel.Store();

            store.identifier = "id";
            store.items = productService.GetProductNameList();

            return this.Json(store, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 取得 dojo store 預設格式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetInsertRow()
        {
            List<eSaleModel.Product> item = new List<eSaleModel.Product>();
            var productService = new eSalesService.ProductService(this.GetDBConnectionString());
            var store = new eSaleModel.Store();
            
            
            item.Add(new eSaleModel.Product()
            {
                ProductName = "Product HHYDP",
                UnitPrice = (decimal)18.00,
                ProductId = 1,
                Count = 1,
                Sum = 1
            });
            

            store.identifier = "ProductId";
            store.items = item;
            return this.Json(store, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 取得訂單資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetOrder()
        {
            var result = new eSaleModel.Order();
            var orderService = new eSalesService.OrderService(this.GetDBConnectionString());
            var store = new eSaleModel.Store();

            result.OrderId = 0;
            result.CustName = "%" + null + "%";
            result.EmpId = 0;
            result.ShipperId = 0;
            result.OrderDate = null;
            result.RequiredDate = null;
            result.ShippedDate = null;

            store.identifier = "OrderId";
            store.items = orderService.GetOrderByCondition(result);
            return this.Json(store, JsonRequestBehavior.AllowGet );

        }
        /// <summary>
        /// 依OrderId取得訂單資訊
        /// </summary>
        /// <param name="condition">篩選條件</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetOrderById(int id)
        {
            var result = new eSaleModel.Order();
            var orderService = new eSalesService.OrderService(this.GetDBConnectionString());
            
            result = orderService.GetOrderById(id);

            return this.Json(result, JsonRequestBehavior.AllowGet);
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
            
            store.identifier = "ProductName";
            store.items = orderService.GetOrderDetailById(id);

            return this.Json(store, JsonRequestBehavior.AllowGet);
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
            var orderService = new eSalesService.OrderService(this.GetDBConnectionString());
            var store = new eSaleModel.Store();

            result.O_OrderId = "%" + Convert.ToString(condition.OrderId) + "%";
            result.CustName = "%" + (condition.CustName)+ "%" ;
            result.EmpId = condition.EmpName == null ? 0 : Int32.Parse(condition.EmpName) ;
            result.ShipperId = condition.ShipperName == null ? 0 : Int32.Parse(condition.ShipperName);
            result.OrderDate = condition.OrderDate;
            result.RequiredDate = condition.RequiredDate;
            result.ShippedDate = condition.ShippedDate;


            store.identifier = "OrderId";
            store.items = orderService.GetOrderByCondition(result);

            return this.Json(store, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新增訂單資訊
        /// </summary>
        /// <param name="orderData">訂單資訊</param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult InsertOrder(eSaleModel.Order orderData)
        {
            var orderService = new eSalesService.OrderService(this.GetDBConnectionString());
            try
            {
                var result = new eSaleModel.Order();
                
                result.CustId = Int32.Parse(orderData.CustName);
                result.EmpId = Int32.Parse(orderData.EmpName);
                result.OrderDate = orderData.OrderDate;
                result.RequiredDate = orderData.RequiredDate;
                result.ShippedDate = orderData.ShippedDate;
                result.ShipperId = Int32.Parse(orderData.ShipperName);
                result.Freight = orderData.Freight;
                result.ShipCountry = orderData.ShipCountry;
                result.ShipCity = orderData.ShipCity;
                result.ShipRegion = orderData.ShipRegion;
                result.ShipPostalCode = orderData.ShipPostalCode;
                result.ShipAddress = orderData.ShipAddress;

                var error = new eSaleModel.ViewModel.ErrorMsg();
                error.Orderid = orderService.InsertOrder(result);
                error.State = true;

                return this.Json(error, JsonRequestBehavior.AllowGet);
            }catch (Exception e)
            {
                var error = new eSaleModel.ViewModel.ErrorMsg();
                error.Describe = "尚未填寫完成";
                error.State = false;

                return this.Json(error, JsonRequestBehavior.AllowGet);
            }
            
        }
        /// <summary>
        /// 新增訂單明細
        /// </summary>
        /// <param name="data">訂單明細</param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult InsertOrderDetail(eSaleModel.ViewModel.ProductDetailWithId data)
        {
            var orderService = new eSalesService.OrderService(this.GetDBConnectionString());
            eSaleModel.ViewModel.SearchOrderGrid searchOrderGrid = new eSaleModel.ViewModel.SearchOrderGrid();
            eSaleModel.Order orderData = new eSaleModel.Order();

            try
            {
                eSaleModel.OrderDetails orderDetail = new eSaleModel.OrderDetails();
                orderDetail.OrderId = data.id;
                orderDetail.ProductId = Convert.ToInt16( data.items[0].ProductName[0]);
                orderDetail.UnitPrice = data.items[0].UnitPrice[0];
                orderDetail.Qty = data.items[0].Qty[0];
                orderService.InsertOrderDetail(orderDetail);
                if (data.items.Count > 1)
                {
                    for (int i = 1; i < data.items.Count; i++)
                    {
                        var orderService2 = new eSalesService.OrderService(this.GetDBConnectionString());
                        eSaleModel.OrderDetails orderDetail2 = new eSaleModel.OrderDetails();
                        orderDetail2.OrderId = data.id;
                        orderDetail2.ProductId = Convert.ToInt16(data.items[0]._S._arrayOfAllItems[i].ProductName[0]);
                        orderDetail2.UnitPrice = data.items[0]._S._arrayOfAllItems[i].UnitPrice[0];
                        orderDetail2.Qty = data.items[0]._S._arrayOfAllItems[i].Qty[0];
                        orderService2.InsertOrderDetail(orderDetail2);
                    }
                }

                
                searchOrderGrid.State = true;
                searchOrderGrid.Order = orderService.GetOrderById(data.id);

                return this.Json(searchOrderGrid);
            }
            catch (Exception e)
            {
                var error = new eSaleModel.ViewModel.ErrorMsg();
                error.Orderid = data.id;
                error.State = false;

                return this.Json(error);
            }
        }

        /// <summary>
        /// 修改訂單資訊
        /// </summary>
        /// <param name="orderData">訂單資訊</param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult UpdateOrder(eSaleModel.Order orderData)
        {
            var orderService = new eSalesService.OrderService(this.GetDBConnectionString());
            try
            {
                var result = new eSaleModel.Order();

                result.OrderId = orderData.OrderId;
                result.CustId = Int32.Parse(orderData.CustName);
                result.EmpId = Int32.Parse(orderData.EmpName);
                result.OrderDate = orderData.OrderDate;
                result.RequiredDate = orderData.RequiredDate;
                result.ShippedDate = orderData.ShippedDate;
                result.ShipperId = Int32.Parse(orderData.ShipperName);
                result.Freight = orderData.Freight;
                result.ShipCountry = orderData.ShipCountry;
                result.ShipCity = orderData.ShipCity;
                result.ShipRegion = orderData.ShipRegion;
                result.ShipPostalCode = orderData.ShipPostalCode;
                result.ShipAddress = orderData.ShipAddress;
                result.ShipName = orderData.ShipName;
                var error = new eSaleModel.ViewModel.ErrorMsg();
                error.State = true;
                error.Orderid = orderService.UpdateOrder(result);
                //var aa = orderService.UpdateOrder(result);
                return this.Json(error, JsonRequestBehavior.AllowGet);
            }
           catch (Exception e)
            {
                var error = new eSaleModel.ViewModel.ErrorMsg();
                //var msg = Convert.ToString(e);
                //var start = msg.IndexOf("參數化查詢");
                //var end = msg.IndexOf("必須有參數");
                //error.Describe = msg.Substring(start+8,end-start-9);
                error.Describe = "尚未填寫完成";
                error.State = false;
                return this.Json(error, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// 修改訂單明細
        /// </summary>
        /// <param name="data">訂單明細</param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult UpdateOrderDetail(eSaleModel.ViewModel.ProductDetailWithId data)
        {
            var orderService = new eSalesService.OrderService(this.GetDBConnectionString());
            try
            {
                //orderService.InsertOrderDetail(data);

                eSaleModel.OrderDetails orderDetail = new eSaleModel.OrderDetails();
                //productDetail = data.items[0];
                orderDetail.OrderId = data.id;
                orderService.DeleteOrderDetail(orderDetail);
                if (data.items.Count > 0)
                {
                    orderDetail.ProductId = Convert.ToInt16(data.items[0].ProductName[0]);
                    orderDetail.UnitPrice = (decimal)data.items[0].UnitPrice[0];
                    orderDetail.Qty = data.items[0].Qty[0];
                    orderService.InsertOrderDetail(orderDetail);
                }
                    
                if (data.items.Count > 1)
                {
                    for (int i = data.items[0]._S._arrayOfAllItems.Count-1 ; i > (data.items[0]._S._arrayOfAllItems.Count - data.items.Count) ; i--)
                    {
                        var orderService2 = new eSalesService.OrderService(this.GetDBConnectionString());
                        eSaleModel.OrderDetails orderDetail2 = new eSaleModel.OrderDetails();
                        orderDetail2.OrderId = data.id;
                        orderDetail2.ProductId = Convert.ToInt16(data.items[0]._S._arrayOfAllItems[i].ProductName[0]);
                        orderDetail2.UnitPrice = Convert.ToInt16(data.items[0]._S._arrayOfAllItems[i].UnitPrice[0]);
                        orderDetail2.Qty = data.items[0]._S._arrayOfAllItems[i].Qty[0];
                        orderService2.InsertOrderDetail(orderDetail2);
                    }
                }


                return this.Json(true);
            }
            catch (Exception e)
            {
                var ee = e;

                return this.Json(false);
            }
        }


        /// <summary>
        /// 刪除訂單明細
        /// </summary>
        /// <param name="data">訂單明細</param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteOrder(eSaleModel.ViewModel.ProductDetailWithId data)
        {
            var orderService = new eSalesService.OrderService(this.GetDBConnectionString());
            try
            {
                //orderService.InsertOrderDetail(data);

                eSaleModel.OrderDetails orderDetail = new eSaleModel.OrderDetails();
                //productDetail = data.items[0];
                orderDetail.OrderId = data.id;

                orderService.DeleteOrderDetail(orderDetail);
                orderService.DeleteOrder(orderDetail);


                return this.Json(true);
            }
            catch (Exception e)
            {
                var ee = e;

                return this.Json(false);
            }
        }

    }
}