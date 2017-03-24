using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSaleModel
{
    /// <summary>
    /// 定義 Emp 資料型別
    /// </summary>
    public class Emp
    {
        /// <summary>
        /// 業務(員工)代號
        /// </summary>
        public int EmpId { get; set; }

        /// <summary>
        /// 業務(員工)名稱
        /// </summary>
        public string EmpName { get; set; }

        /// <summary>
        /// 職稱
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 稱謂
        /// </summary>
        public string TitleOfCourtesy { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// 任職日期
        /// </summary>
        public DateTime? HireDate { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 地區
        /// </summary>Region,PostalCode,Country,Phone
        public string Region { get; set; }
        /// <summary>
        /// 郵遞區號
        /// </summary>
        public string PostalCode { get; set; }
        /// <summary>
        /// 國籍
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 主管員工代號
        /// </summary>
        public int ManagerID { get; set; }



    }
}
