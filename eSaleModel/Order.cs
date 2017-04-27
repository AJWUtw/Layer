using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSaleModel
{
    /// <summary>
    /// 定義 Order 資料型別
    /// </summary>
    public class Order
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        [DisplayName("訂單編號")]
        [Required()]
        public int OrderId { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        [DisplayName("訂單編號")]
        [Required()]
        public string O_OrderId { get; set; }

        /// <summary>
        /// 客戶代號
        /// </summary>
        [DisplayName("客戶代號")]
        [Required()]
        public int CustId { get; set; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        [DisplayName("客戶名稱")]
        [Required()]
        public string CustName { get; set; }

        /// <summary>
        /// 業務(員工)代號
        /// </summary>
        [DisplayName("負責員工編號")]
        [Required()]
        public int EmpId { get; set; }

        /// <summary>
        /// 業務(員工姓名)
        /// </summary>
        [DisplayName("負責員工名稱")]
        [Required()]
        public string EmpName { get; set; }

        /// <summary>
        /// 訂單日期
        /// </summary>
        [DisplayName("訂單日期")]
        [Required()]
        public DateTime? Orderdate { get; set; }

        /// <summary>
        /// 需要日期
        /// </summary>
        [DisplayName("需要日期")]
        [Required()]
        public DateTime? RequiredDate { get; set; }

        /// <summary>
        /// 出貨日期
        /// </summary>
        [DisplayName("出貨日期")]
        [Required()]
        public DateTime? ShippedDate { get; set; }

        /// <summary>
        /// 訂單日期
        /// </summary>
        [DisplayName("訂單日期")]
        [Required()]
        public string O_Orderdate { get; set; }

        /// <summary>
        /// 需要日期
        /// </summary>
        [DisplayName("需要日期")]
        [Required()]
        public string O_RequiredDate { get; set; }

        /// <summary>
        /// 出貨日期
        /// </summary>
        [DisplayName("出貨日期")]
        [Required()]
        public string O_ShippedDate { get; set; }
        /// <summary>
        /// 出貨公司代號
        /// </summary>
        [DisplayName("出貨公司代號")]
        [Required()]
        public int ShipperId { get; set; }

        /// <summary>
        /// 出貨公司名稱
        /// </summary>
        [DisplayName("出貨公司名稱")]
        [Required()]
        public string ShipperName { get; set; }


        /// <summary>
        /// 運費
        /// </summary>
        [Range(0, int.MaxValue)]
        [DisplayName("運費")]
        [Required()]
        public decimal Freight { get; set; }

        /// <summary>
        /// 出貨說明
        /// </summary>
        [DisplayName("出貨說明")]
        [Required()]
        public string ShipName { get; set; }

        /// <summary>
        /// 出貨地址
        /// </summary>
        [DisplayName("出貨地址")]
        [Required()]
        public string ShipAddress { get; set; }

        /// <summary>
        /// 出貨程式
        /// </summary>
        [DisplayName("出貨城市")]
        [Required()]
        public string ShipCity { get; set; }

        /// <summary>
        /// 出貨地區
        /// </summary>
        [DisplayName("出貨地區")]
        [Required()]
        public string ShipRegion { get; set; }

        /// <summary>
        /// 郵遞區號
        /// </summary>
        [DisplayName("郵遞區號")]
        [Required()]
        public string ShipPostalCode { get; set; }

        /// <summary>
        /// 出貨國家
        /// </summary>
        [DisplayName("出貨國家")]
        [Required()]
        public string ShipCountry { get; set; }
    }
}
