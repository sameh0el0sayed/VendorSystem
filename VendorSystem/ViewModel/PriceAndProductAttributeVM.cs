using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace  VendorSystem.ViewModel
{
    public class PriceAndProductAttributeVM
    {
        public decimal? SellPrice { get; set; }
        public string InternalCode { get; set; }
        public string ProductName { get; set; }
        public decimal?  UOM { get; set; }
    }
}