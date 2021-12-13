using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class Replenish_VM
    {
        public Nullable<decimal> Replenish_Master_Id { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public string Barcode { get; set; }
        public string Product_Name { get; set; }
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public string CompanyName { get; set; }
        public string CustoemrCode { get; set; }
        public string RoutCode { get; set; }
        public string RouteName { get; set; }
        public Nullable<decimal> Current_Stock { get; set; }
        public Nullable<decimal> Net_Sales { get; set; }
        public Nullable<decimal> Total_Po_Qty { get; set; }
        public Nullable<decimal> Total_Po_Return_Qty { get; set; }
        public Nullable<decimal> FirstPO_Received { get; set; }
        public Nullable<decimal> FirstPO_Return { get; set; }
        public Nullable<decimal> SecondPO_Received { get; set; }
        public Nullable<decimal> SecondPO_Return { get; set; }
        public Nullable<decimal> ThirdPO_Received { get; set; }
        public Nullable<decimal> ThirdPO_Return { get; set; }
        public Nullable<decimal> FourthPO_Received { get; set; }
        public Nullable<decimal> FourthPO_Return { get; set; }
        public Nullable<decimal> FifthPO_Received { get; set; }
        public Nullable<decimal> FifthPO_Return { get; set; }
        public Nullable<decimal> SixthPO_Received { get; set; }
        public Nullable<decimal> SixthPO_Return { get; set; }


        public Nullable<decimal> Avg_Po_Return { get; set; }
        public Nullable<decimal> Forecasted_Po_Qty { get; set; }
        public Nullable<decimal> First_Visit { get; set; }
        public Nullable<decimal> Second_Visit { get; set; }
        public Nullable<decimal> Third_Visit { get; set; }
    }
}