using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace VendorSystem.ViewModel
{
    public class ReplenishConfig_VisitScheduleVM
    {
        public int WeekNumber { get; set; }
        public int DayNumber { get; set; }
        public string WeekName { get; set; }
        public string DayName { get; set; }
        public decimal OrderPercentage { get; set; }

    }
}