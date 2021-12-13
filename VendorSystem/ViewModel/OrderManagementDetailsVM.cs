using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class OrderManagementDetailsVM
    {
        public Nullable<decimal> MasterID { get; set; }
        public string Barcode { get; set; }
        public Nullable<decimal> SystemQty { get; set; }
        public Nullable<decimal> ApprovedQty { get; set; }
        public Nullable<decimal> ShippedQty { get; set; }
        public Nullable<decimal> ActualRecivedQty { get; set; }
        public Nullable<decimal> MarketUnitPrice { get; set; }
        public Nullable<decimal> VendorUnitPrice { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> Taxes { get; set; }
        public string CompanyID { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<decimal> DifferanceValue { get; set; }
        public Nullable<decimal> DifferancePercent { get; set; }
        public Nullable<decimal> BonusQTY { get; set; }
        public Nullable<decimal> TotalBeforeDiscount { get; set; }
        public Nullable<decimal> TotalAfterDiscount { get; set; }
        public Nullable<decimal> TaxValue { get; set; }
        public Nullable<decimal> TotalAfterTax { get; set; }
        public Nullable<decimal> CommericalDiscount { get; set; }
        public Nullable<bool> IsFree { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string Lastupdate { get; set; }
        public Nullable<decimal> JomlaDiscount { get; set; }
        public Nullable<decimal> SellPrice { get; set; }
        public Nullable<decimal> ReturnQty { get; set; }
        public string InternalCode { get; set; }
        public string Vendor_CompanyID { get; set; }
    }
}