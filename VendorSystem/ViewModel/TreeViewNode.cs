using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace  VendorSystem.ViewModel
{
    public class TreeViewNode
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public string textEn { get; set; }
        public bool IsEdit { get; set; }
        public bool Active { get; set; }
        public string Code { get; set; }
        public Nullable<decimal> IsClothes { get; set; }
        public bool IsNegativeSale { get; set; }
    }
}