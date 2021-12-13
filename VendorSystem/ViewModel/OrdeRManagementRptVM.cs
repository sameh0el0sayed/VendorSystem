using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class OrdeRManagementRptVM
    {
        public string BarCode { get; set; }
        public string ProductName { get; set; }
        public string InternalCode { get; set; }
        public decimal? RepQty { get; set; }
        public decimal? ApprovedQty { get; set; }
        public decimal? ShippedQty { get; set; }
        public decimal? DeliveredQty { get; set; }
        public decimal? VendorPrice { get; set; }
        public decimal? MarketPrice { get; set; }
        public decimal? ShippedSubTotal { get; set; }
        public decimal? DeliveredSubTotal { get; set; }
    }
}