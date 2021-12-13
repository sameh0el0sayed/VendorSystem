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
    
    public partial class Fun_GetReplenishmentReportData_Result
    {
        public string CompanyNameEng { get; set; }
        public string StoreNameEng { get; set; }
        public string CompanyName { get; set; }
        public string StoreName { get; set; }
        public Nullable<System.DateTime> LastSalesSyncDate { get; set; }
        public Nullable<System.DateTime> LastOpenDay { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public Nullable<System.DateTime> ReplenishmentDate { get; set; }
        public Nullable<int> OrdersCount { get; set; }
        public int FirstVisit { get; set; }
        public int SecondVisit { get; set; }
        public int ThirdVisit { get; set; }
        public decimal NegativeStock { get; set; }
        public decimal PositiveStock { get; set; }
        public decimal NegativeSales { get; set; }
        public decimal PositiveSales { get; set; }
        public decimal Total_Po_Qty { get; set; }
        public decimal NormalRecieved_PO_Qty { get; set; }
        public decimal NormalReturn_PO_Qty { get; set; }
        public decimal Total_Po_Return_Qty { get; set; }
        public decimal NegativeForecast { get; set; }
        public decimal PositiveForecast { get; set; }
        public int FirstOrder { get; set; }
        public Nullable<int> SecondOrder { get; set; }
        public Nullable<int> ThirdOrder { get; set; }
        public decimal Total_Cost { get; set; }
    }
}
