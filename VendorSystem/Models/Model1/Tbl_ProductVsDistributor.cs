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
    
    public partial class Tbl_ProductVsDistributor
    {
        public int ID { get; set; }
        public string DistributorCode { get; set; }
        public string Barcode { get; set; }
        public string InternalCode { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string Vendor_CompanyID { get; set; }
    }
}
