//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VendorSystem.Models.Model2
{
    using System;
    using System.Collections.Generic;
    
    public partial class DistributorVsVendor
    {
        public int ID { get; set; }
        public string VendorCompanyCode { get; set; }
        public string PartnerCode { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
