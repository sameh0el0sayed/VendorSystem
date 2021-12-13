using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class AllocationVM
    {
        public string Barcode { get; set; }
        public string InternalCode { get; set; }
        public string Name { get; set; }
        public decimal? TotalNeededQty { get; set; }
        public decimal? ShippedQty { get; set; }

    }
}