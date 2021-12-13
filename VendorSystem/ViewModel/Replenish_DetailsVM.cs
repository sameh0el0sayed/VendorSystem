using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class Replenish_DetailsVM
    {
        public decimal ID { get; set; }
        public decimal Replenish_Master_Id { get; set; }
        public string Barcode { get; set; }
        public decimal ProductAtrrSeq { get; set; }
        public string Sub_WH_Id { get; set; }
        public string Supplier_Code { get; set; }
        public Nullable<decimal> Current_Stock { get; set; }
        public Nullable<decimal> Net_Sales { get; set; }
        public Nullable<decimal> Total_Po_Qty { get; set; }
        public Nullable<decimal> Total_Po_Return_Qty { get; set; }
        public Nullable<decimal> First_Visit { get; set; }
        public Nullable<decimal> Second_Visit { get; set; }
        public Nullable<decimal> Third_Visit { get; set; }
        public string Company_Id { get; set; }
        public Nullable<decimal> Avg_Po_Return { get; set; }
        public Nullable<decimal> Forecasted_Po_Qty { get; set; }
        public string LastUpdate { get; set; }
        public string InternalCode { get; set; }
        public string Vendor_CompanyID { get; set; }

        public Nullable<decimal> ReturnPercentage { get; set; }
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
        public Nullable<decimal> AvgSales { get; set; }
        public Nullable<decimal> Min_Qty { get; set; }
        public decimal? NormalNet_PO_Qty { get;  set; }
        public decimal? NormalRecieved_PO_Qty { get;  set; }
        public decimal? NormalReturn_PO_Qty { get; set; }
        public decimal? Total_Cost { get; set; }
    }
}