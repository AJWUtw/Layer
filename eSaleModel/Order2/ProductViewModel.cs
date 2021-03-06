﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSaleModel.Order2
{
    public class ProductViewModel
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public bool Discount { get; set; }
        public int? Qty { get; set; }
        public int? Sum { get; set; }
    }
}
