using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web.Mvc;

namespace eSalesService
{
    public class ProductService
    {
        /// <summary>
        /// Database 連線字串
        /// </summary>
        public string DbConn { get; set; }
        /// <summary>
        /// ProductService 建構子
        /// </summary>
        /// <param name="dbConn">Database 連線字串</param>
        public ProductService(string dbConn)
        {
            this.DbConn = dbConn;
        }
        /// <summary>
        /// 依訂單id取得商品資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<eSaleModel.Order2.ProductViewModel> GetProductById(int id)
        {
            eSaleDao.ProductDao productDao = new eSaleDao.ProductDao(this.DbConn);
            var datalist = productDao.GetProductById(id);
            return this.MapProduct(datalist);
        }
        /// <summary>
        /// 取得商品清單2
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<eSaleModel.ProductList> GetProductList()
        {
            List<eSaleModel.Product> result = new List<eSaleModel.Product>();
            eSaleDao.ProductDao productDao = new eSaleDao.ProductDao(this.DbConn);
            var datalist = productDao.GetProductNameList();

            return this.MapProductList(datalist);
        }

        /// <summary>
        /// 取得商品清單
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<eSaleModel.FilteringSelect> GetProductNameList()
        {
            List<eSaleModel.Product> result = new List<eSaleModel.Product>();
            eSaleDao.ProductDao productDao = new eSaleDao.ProductDao(this.DbConn);
            var datalist = productDao.GetProductNameList();

            return this.MapProductNameList(datalist);
        }

        /// <summary>
        /// 依條件篩選商品資料
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<eSaleModel.Product> GetProductByCondition(eSaleModel.Product condition)
        {
            List<eSaleModel.Product> result = new List<eSaleModel.Product>();
            eSaleDao.ProductDao productDao = new eSaleDao.ProductDao(this.DbConn);
            var datalist = productDao.GetProductByCondition(condition);
            
            return this.MapProductStore(datalist);

        }
        private List<eSaleModel.ProductList> MapProductList(DataTable productData)
        {
            List<eSaleModel.ProductList> result = new List<eSaleModel.ProductList>();

            foreach (DataRow row in productData.Rows)
            {
                result.Add(new eSaleModel.ProductList()
                {
                    ProductName = row["ProductName"].ToString(),
                    ProductId = (int)row["ProductId"]
                });
            }
            return result;
        }

        private List<eSaleModel.Order2.ProductViewModel> MapProduct(DataTable productData)
        {
            List<eSaleModel.Order2.ProductViewModel> result = new List<eSaleModel.Order2.ProductViewModel>();

            foreach (DataRow row in productData.Rows)
            {
                result.Add(new eSaleModel.Order2.ProductViewModel()
                {
                    ProductName = row["ProductName"].ToString(),
                    ProductId = (int)row["ProductId"],
                    UnitPrice = (decimal)row["UnitPrice"],
                    Qty = 0,
                    Sum = 0
                });
            }
            return result;
        }


        private List<eSaleModel.FilteringSelect> MapProductNameList(DataTable productData)
        {
            List<eSaleModel.FilteringSelect> result = new List<eSaleModel.FilteringSelect>();

            foreach (DataRow row in productData.Rows)
            {
                result.Add(new eSaleModel.FilteringSelect()
                {
                    name = row["ProductName"].ToString(),
                    id = (int)row["ProductId"]
                });
            }
            return result;
        }

        private List<eSaleModel.FilteringSelect> MapProductNaList(DataTable productData)
        {
            List<eSaleModel.FilteringSelect> result = new List<eSaleModel.FilteringSelect>();

            foreach (DataRow row in productData.Rows)
            {
                result.Add(new eSaleModel.FilteringSelect()
                {
                    name = row["ProductName"].ToString(),
                    id = (int)row["ProductId"]
                });
            }
            return result;
        }
        private List<eSaleModel.Product> MapProductStore(DataTable productData)
        {
            List<eSaleModel.Product> result = new List<eSaleModel.Product>();


            foreach (DataRow row in productData.Rows)
            {
                result.Add(new eSaleModel.Product()
                {
                    ProductName = row["ProductName"].ToString(),
                    ProductId = (int)row["ProductId"],
                    UnitPrice = (decimal)row["UnitPrice"],
                    SupplierId = (int)row["SupplierId"],
                    CategoryId = (int)row["CategoryId"],
                    Count = 0,
                    Sum = 0

                });
            }
            return result;
        }
    }
}
