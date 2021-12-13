using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class ProductVM
    {
        public string BigBarcode { get; set; }
        public string SmallBarcode { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public int PiecesInCatron { get; set; }
        public decimal? Weight { get; set; }
        public int? FirstClassification { get; set; }
        public int? SecondClassification { get; set; }
        public int? ThirdClassification { get; set; }
        public int? FourthClassification { get; set; }
        public bool? IsActive { get; set; }
        public int ID { get; set; }
        public bool? IsLocked { get; set; }
        public decimal? ReturnPercentage { get; set; }
        public decimal? MinQty { get; set; }
        public int? ShelfLifeID { get; set; }

    }
}