using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSaleModel.Order2
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string CustName { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        
    }
}
