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
    public class EmpService
    {
        /// <summary>
        /// Database 連線字串
        /// </summary>
        public string DbConn { get; set; }
        /// <summary>
        /// EmpService 建構子
        /// </summary>
        /// <param name="dbConn">Database 連線字串</param>
        public EmpService(string dbConn)
        {
            this.DbConn = dbConn;
        }
        /// <summary>
        /// 取得員工資料
        /// </summary>
        /// <returns></returns>
        public List<eSaleModel.Emp> GetEmpData()
        {
            
            List<eSaleModel.Emp> result = new List<eSaleModel.Emp>();
            eSaleDao.EmpDao empDao = new eSaleDao.EmpDao(this.DbConn);
            var datalist = empDao.GetEmpData();


            return this.MapEmpDataToList(datalist);
        }
        /// <summary>
        /// 取得員工資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetEmpNameData()
        {

            List<SelectListItem> result = new List<SelectListItem>();
            eSaleDao.EmpDao empDao = new eSaleDao.EmpDao(this.DbConn);
            var datalist = empDao.GetEmpData();


            return this.MapEmpNameSelectListItem(datalist);
        }
        private List<SelectListItem> MapEmpNameSelectListItem(DataTable empData)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            

            foreach (DataRow row in empData.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Value = row["EmployeeId"].ToString(),
                    Text = row["LastName"].ToString() + row["FirstName"].ToString(),

                });
            }
            return result;
        }

        private List<eSaleModel.Emp> MapEmpDataToList(DataTable empData)
        {
            List<eSaleModel.Emp> result = new List<eSaleModel.Emp>();


            foreach (DataRow row in empData.Rows)
            {
                result.Add(new eSaleModel.Emp()
                {
                    EmpId = (int)row["EmployeeId"],
                    EmpName = row["LastName"].ToString()+ row["FirstName"].ToString(),
                    Title = row["Title"].ToString(),
                    TitleOfCourtesy = row["TitleOfCourtesy"].ToString(),
                    BirthDate = (DateTime)row["BirthDate"],
                    HireDate = (DateTime)row["HireDate"],
                    Address = row["Address"].ToString(),
                    City = row["City"].ToString(),
                    Region = row["Region"].ToString(),
                    PostalCode = row["PostalCode"].ToString(),
                    Country = row["Country"].ToString(),
                    Phone = row["Phone"].ToString(),
                    ManagerID = (int)row["MnangerID"] ,
                    
                });
            }
            return result;
        }
    }
}
