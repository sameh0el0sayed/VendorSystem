namespace VendorSystem.ViewModel
{
    public class PODetailsVM
    {
        public decimal? ApprovedQty { get; set; }
        public string BarCode { get; set; }
        public string DocumentNumber { get; set; }
        public string ProductNa { get; set; }
        public decimal? ShippedQty { get; set; }
        public string StoreId { get; set; }
        public decimal? SystemQty { get; set; }
        public decimal? DeliveredQty { get; set; }
        public decimal? VendorUnitPrice { get; set; }
        public decimal? MarketUnitPrice { get; set; }
        public string ExpectedDeliveredDate { get; set; }
        public bool? IsShipping { get; set; }
        public bool? IsRejected { get; set; }
        public decimal? MasterID { get; set; }
        public string InternalCode { get; set; }

        //sameh code
        public string CustomerCode { get; set; } 
        public int CustomerMasterId { get; set; }
        public string RouteName { get; set; }
        public string RegionName { get; set; }
        public string TerritoryName { get; set; }
    }
}