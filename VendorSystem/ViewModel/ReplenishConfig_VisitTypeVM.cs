using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class ReplenishConfig_VisitTypeVM
    {
        public int OrderID { get; set; }
        public int DayNum { get; set; }
        public decimal OrderPercentage { get; set; }
        public string OrderName { get; set; }
        public string DayName { get; set; }
    }
}