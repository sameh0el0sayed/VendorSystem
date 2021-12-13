using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class Replenish_MasterVM
    {
        public decimal Id { get; set; }
        public Nullable<decimal> User_Id { get; set; }
        public string Company_Id { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string LastUpdate { get; set; }
    }
}