using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class OrderConfigVM
    {
        public decimal ID { get; set; } 
        public string Name { get; set; }
        public string NameEng { get; set; }
        public int? VisitScheduleTypeID { get; set; }
        public int? VisitScheduleCycleCount { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreationDate { get; set; } 
        public int CreatedBy { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int? UpdatedBy { get; set; } 
        public string Vendor_CompanyID { get; set; }

        public List<OrderConfig_VisitScheduleVM> VisitSchedule_Data { get; set; }
        public List<int> CustomersDtlLst { get; set; }


    }
}