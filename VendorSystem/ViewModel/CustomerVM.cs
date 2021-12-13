using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class CustomerVM
    {
        public int ID { get; set; }
        public string StoreName { get; set; }
        public string StoreNameEng { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameEng { get; set; }
        public string CommercialRegistration { get; set; }
        public string VatNumber { get; set; }
        public string Phone { get; set; }
        public int? StoreCategoryID { get; set; }
        public int? StoreClassificationID { get; set; }
        public int? StoreTypeID { get; set; }
        public int? VisitScheduleID { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public int? CountryID { get; set; }
        public int? ProvinceID { get; set; }
        public int? CityID { get; set; }
        public int? RegionID { get; set; }
        public int? TerritoryID { get; set; }
        public bool? IsActive { get; set; }

        public string StoreID { get; set; }
        public string CompanyID { get; set; }
        public string URL { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }

        public string CityName { get; set; }
        public string CityNameEng { get; set; }
        public string CustomerCode { get; set; }
        public string AssingRoutes { get; set; }
    }
}