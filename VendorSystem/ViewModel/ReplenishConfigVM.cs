using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class ReplenishConfigVM
    {
        public decimal ID { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public int CalcTypeID { get; set; }
        public int Rep_ScheduleTypeID { get; set; }
        public int Visit_ScheduleTypeID { get; set; }
        public bool? IsIncludeMinQty { get; set; }
        public bool? IsActive { get; set; } 

        public List<ReplenishConfig_RepScheduleVM> RepSchedule_Data { get; set; }

       // public List<ReplenishConfig_VisitScheduleVM> VisitSchedule_Data { get; set; }

        public int? FirstClassificationID { get; set; }
        public int? SecondClassificationID { get; set; }
        public int? ThirdClassificationID { get; set; }
        public int? FourthClassificationID { get; set; }

        public string Vendor_CompanyID { get; set; }

    }
}