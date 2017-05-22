using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSaleModel
{
    /// <summary>
    /// 定義 ProductName 資料型別
    /// </summary>
    public class ProductList
    {
        /// <summary>
        /// 商品代號
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; }

    }
}
