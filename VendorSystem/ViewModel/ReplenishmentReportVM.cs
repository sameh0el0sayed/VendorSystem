using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class ReplenishmentReportVM
    {
        public string CompanyName { get; set; }
        public string StoreName { get; set; }
        public Nullable<System.DateTime> LastSalesSyncDate { get; set; }
        public Nullable<System.DateTime> LastOpenDay { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public Nullable<System.DateTime> ReplenishmentDate { get; set; }

        public Nullable<int> OrdersCount { get; set; }
        public Nullable<int> OrderCountPerRep { get; set; }

        public decimal NegativeStock { get; set; }
        public decimal PositiveStock { get; set; }
        public decimal NetStock { get; set; }

        public decimal NegativeSales { get; set; }
        public decimal PositiveSales { get; set; }
        public decimal NetSales { get; set; }

        public decimal Total_Po_Qty { get; set; }
        public decimal Total_Po_Return_Qty { get; set; }
        public decimal NetPurchaseOrder { get; set; }

        public decimal NegativeForecast { get; set; }
        public decimal PositiveForecast { get; set; }
        public decimal NetForecast { get; set; }

        public decimal? Total_Cost { get; set; }
        public int FirstOrder { get; set; }
        public Nullable<int> SecondOrder { get; set; }
        public Nullable<int> ThirdOrder { get; set; }

        
    }
}