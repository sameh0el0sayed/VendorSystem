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
    
    public partial class Tbl_OrdConfig_VisitScheduleType
    {
        public decimal ID { get; set; }
        public Nullable<decimal> ConfigMstrID { get; set; }
        public Nullable<int> OrderTypeID { get; set; }
        public Nullable<int> DayNum { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
    }
}
