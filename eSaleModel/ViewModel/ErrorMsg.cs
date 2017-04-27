using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSaleModel.ViewModel
{
    public class ErrorMsg
    {

        /// <summary>
        /// 訊息狀態
        /// </summary>
        public Boolean State { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 訂單id
        /// </summary>
        public int Orderid { get; set; }
        /// <summary>
        /// 訂單資訊
        /// </summary>
        public eSaleModel.Order Order { get; set; }
        
    }
    
}
