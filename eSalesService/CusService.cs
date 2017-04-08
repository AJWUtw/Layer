using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace eSalesService
{
    public class CusService
    {
        /// <summary>
        /// Database 連線字串
        /// </summary>
        public string DbConn { get; set; }
        /// <summary>
        /// EmpService 建構子
        /// </summary>
        /// <param name="dbConn">Database 連線字串</param>
        public CusService(string dbConn)
        {
            this.DbConn = dbConn;
        }
        /// <summary>
        /// 取得客戶資料
        /// </summary>
        /// <returns></returns>
        public List<eSaleModel.Cus> GetCusData()
        {
            
            List<eSaleModel.Cus> result = new List<eSaleModel.Cus>();
            eSaleDao.CusDao cusDao = new eSaleDao.CusDao(this.DbConn);
            var datalist = cusDao.GetCusData();


            return this.MapCusDataToList(datalist);
        }
        /// <summary>
        /// Map客戶資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetCusNameData()
        {

            List<SelectListItem> result = new List<SelectListItem>();
            eSaleDao.CusDao cusDao = new eSaleDao.CusDao(this.DbConn);
            var datalist = cusDao.GetCusData();


            return this.MapCusNameSelectListItem(datalist);
        }
        private List<SelectListItem> MapCusNameSelectListItem(DataTable cusData)
        {
            List<SelectListItem> result = new List<SelectListItem>();


            foreach (DataRow row in cusData.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Value = row["CustomerId"].ToString(),
                    Text = row["ContactTitle"].ToString() +" "+ row["ContactName"].ToString(),

                });
            }
            return result;
        }

        private List<eSaleModel.Cus> MapCusDataToList(DataTable cusData)
        {
            List<eSaleModel.Cus> result = new List<eSaleModel.Cus>();


            foreach (DataRow row in cusData.Rows)
            {
                result.Add(new eSaleModel.Cus()
                {
                    CusId = (int)row["CusId"],
                    CompanyName = row["CompanyName"].ToString(),
                    ContactName = row["ContactName"].ToString(),
                    ContactTTitle = row["ContactTTitle"].ToString(),
                    City = row["City"].ToString(),
                    Region = row["Region"].ToString(),
                    PostalCode = row["PostalCode"].ToString(),
                    Country = row["Country"].ToString(),
                    Phone = row["Phone"].ToString(),
                    
                });
            }
            return result;
        }
    }
}
