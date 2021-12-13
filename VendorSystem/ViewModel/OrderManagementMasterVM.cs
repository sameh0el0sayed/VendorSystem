using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class OrderManagementMasterVM
    {
        public decimal ID { get; set; }
        public string DocumentNumber { get; set; }
        public string CreationDate { get; set; }
        public string SentDate { get; set; }
        public string ShippedDate { get; set; }
        public string RecievedDate { get; set; }
        public Nullable<int> PartnerId { get; set; }
        public Nullable<int> ItemCount { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<decimal> InvoiceDiscount { get; set; }
        public Nullable<decimal> Taxes { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> Pay1amt { get; set; }
        public Nullable<decimal> Pay2amt { get; set; }
        public Nullable<int> Pay2Id { get; set; }
        public string Card1No { get; set; }
        public string StoreId { get; set; }
        public Nullable<int> Status { get; set; }
        public string CompanyID { get; set; }
        public string Lastupdate { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public Nullable<decimal> RemainAmount { get; set; }
        public Nullable<decimal> TotalBeforeDiscount { get; set; }
        public Nullable<decimal> TotalAfterDiscount { get; set; }
        public Nullable<decimal> TotalTax { get; set; }
        public Nullable<decimal> TotalAfterTax { get; set; }
        public Nullable<decimal> TotalDiscount { get; set; }
        public Nullable<decimal> BuyTotalBeforeDiscount { get; set; }
        public Nullable<decimal> BuyTotalAfterDiscount { get; set; }
        public Nullable<decimal> BuyTotalTax { get; set; }
        public Nullable<decimal> BuyTotalAfterTax { get; set; }
        public Nullable<decimal> BuyTotalDiscount { get; set; }
        public Nullable<int> BuyItemCount { get; set; }
        public Nullable<int> FreeItemCount { get; set; }
        public string PartnerCode { get; set; }
        public Nullable<decimal> ReturnTotalBeforeDiscount { get; set; }
        public Nullable<decimal> ReturnTotalAfterDiscount { get; set; }
        public Nullable<decimal> ReturnTotalTax { get; set; }
        public Nullable<decimal> ReturnTotalAfterTax { get; set; }
        public Nullable<decimal> ReturnTotalDiscount { get; set; }
        public Nullable<decimal> ReturnItemCount { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public bool? IsFromReplenishment { get; set; }
        public string Vendor_CompanyID { get; set; }
        public Nullable<decimal> ReplenishmentMasterID { get; set; }

    }
}