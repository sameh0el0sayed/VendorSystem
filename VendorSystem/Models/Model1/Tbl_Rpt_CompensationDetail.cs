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
    
    public partial class Tbl_Rpt_CompensationDetail
    {
        public decimal ID { get; set; }
        public Nullable<int> MasterID { get; set; }
        public Nullable<int> CustDtl_ID { get; set; }
        public string StoreID { get; set; }
        public string CompanyID { get; set; }
        public Nullable<decimal> TotalRecievedOrderValue { get; set; }
        public Nullable<decimal> CompensationPercentage { get; set; }
        public Nullable<decimal> RevenueCompensationPercenatge { get; set; }
        public Nullable<int> RecievedOrdersCount { get; set; }
        public Nullable<decimal> CompensationValue { get; set; }
        public Nullable<decimal> RevenueCompensationValue { get; set; }
        public Nullable<decimal> TotalRevenueCompensation { get; set; }
        public Nullable<decimal> TotalCreatedOrderValue { get; set; }
        public Nullable<decimal> KPI_Percenatge { get; set; }
        public Nullable<decimal> Revenue_KPI_Percenatge { get; set; }
        public Nullable<int> CreaatedOrderCount { get; set; }
        public Nullable<decimal> KPI_Value { get; set; }
        public Nullable<decimal> Revenue_KPI_Value { get; set; }
        public Nullable<decimal> TotalRevenueKPI { get; set; }
        public Nullable<decimal> MonthlyFixedValue { get; set; }
        public Nullable<decimal> Net { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public string Vendor_CompanyID { get; set; }
    }
}
