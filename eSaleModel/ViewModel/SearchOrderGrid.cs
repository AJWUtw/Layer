using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSaleModel.ViewModel
{
    public class SearchOrderGrid
    {

        /// <summary>
        /// 商品代號
        /// </summary>
        public Boolean State { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public eSaleModel.Order Order { get; set; }
        
    }
    
}
