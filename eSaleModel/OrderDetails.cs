using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSaleModel
{
    /// <summary>
    /// 定義 OrderDetails 資料型別
    /// </summary>
    public class OrderDetails
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 商品編號
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public int Discount { get; set; }
    }
}
