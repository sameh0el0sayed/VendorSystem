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
    
    public partial class CentralizedLockUp
    {
        public int ID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> Lastupdate { get; set; }
        public Nullable<int> Createdby { get; set; }
        public Nullable<int> updatedby { get; set; }
        public Nullable<bool> Active { get; set; }
        public string CompanyID { get; set; }
    }
}