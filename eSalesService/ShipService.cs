using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web.Mvc;

namespace eSalesService
{
    public class ShipService
    {
        /// <summary>
        /// Database 連線字串
        /// </summary>
        public string DbConn { get; set; }
        /// <summary>
        /// EmpService 建構子
        /// </summary>
        /// <param name="dbConn">Database 連線字串</param>
        public ShipService(string dbConn)
        {
            this.DbConn = dbConn;
        }

        /// <summary>
        /// 取得員工資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetShipperNameData()
        {

            List<SelectListItem> result = new List<SelectListItem>();
            eSaleDao.ShipDao shipDao = new eSaleDao.ShipDao(this.DbConn);
            var datalist = shipDao.GetShipData();


            return this.MapShipNameSelectListItem(datalist);
        }
        private List<SelectListItem> MapShipNameSelectListItem(DataTable empData)
        {
            List<SelectListItem> result = new List<SelectListItem>();


            foreach (DataRow row in empData.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Value = row["ShipperID"].ToString(),
                    Text = row["CompanyName"].ToString(),

                });
            }
            return result;
        }
    }
}
