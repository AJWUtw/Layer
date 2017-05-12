using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSaleModel.Order2
{
    /// <summary>
    /// 定義 Product 資料型別
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 商品代號
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; }

        [Range(0, double.MaxValue)]
        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 供應商代號
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// 類別代號
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public Boolean Discontinued { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 小計
        /// </summary>
        public int Sum { get; set; }
    }
}
