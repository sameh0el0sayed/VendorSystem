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
    
    public partial class V_GetActiveReplenishConfiguration
    {
        public decimal ID { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public Nullable<bool> IsIncludeMinQty { get; set; }
        public string Vendor_CompanyID { get; set; }
        public string CalcName { get; set; }
        public string CalcNameEng { get; set; }
        public string RepSchedName { get; set; }
        public string RepSchedNameEng { get; set; }
        public string VisitSchedName { get; set; }
        public string VisitSchedNameEng { get; set; }
        public string FirstClassName { get; set; }
        public string FirstClassNameEng { get; set; }
    }
}
