using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSaleModel.ViewModel
{
    public class ProductDetail
    {

        /// <summary>
        /// 商品代號
        /// </summary>
        public List<int> ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public List<string> ProductName { get; set; }

        [Range(0, double.MaxValue)]
        /// <summary>
        /// 單價
        /// </summary>
        public List<int> UnitPrice { get; set; }
        
        /// <summary>
        /// 數量
        /// </summary>
        public List<int> Qty { get; set; }

        /// <summary>
        /// 小計
        /// </summary>
        public List<int> Sum { get; set; }
        /// <summary>
        /// 折價
        /// </summary>
        public List<int> Discount { get; set; }
        /// <summary>
        /// 折價
        /// </summary>
        public _S _S { get; set; }
    }
    public class ProductDetailWithId
    {

        public int id { get; set; }
        public List<ProductDetail> items { get; set; }
    }

    public class _S
    {

        public List<ProductDetail> _arrayOfAllItems { get; set; }
    }
    
}
