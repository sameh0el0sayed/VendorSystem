//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VendorSystem.Models.Model1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_CustomerDtl
    {
        public int ID { get; set; }
        public int CustomerMasterID { get; set; }
        public string Vendor_CompanyID { get; set; }
        public Nullable<int> VisitScheduleID { get; set; }
        public Nullable<int> StoreType { get; set; }
        public Nullable<int> StoreCategory { get; set; }
        public Nullable<int> StoreClass { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public Nullable<decimal> RebateAmount { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<bool> Active { get; set; }
        public string CustoemrCode { get; set; }
    }
}
