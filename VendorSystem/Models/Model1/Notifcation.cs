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
    
    public partial class Notifcation
    {
        public int ID { get; set; }
        public string NotifcationMessage { get; set; }
        public Nullable<bool> Seen { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Url { get; set; }
        public string Vendor_CompanyID { get; set; }
    }
}