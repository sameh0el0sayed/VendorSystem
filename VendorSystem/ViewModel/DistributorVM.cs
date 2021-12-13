using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class DistributorVM
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public int? CountryID { get; set; }
        public int? ProvinceID { get; set; }
        public int? CityID { get; set; }
        public int? RegionID { get; set; }
        public int? TerritoryID { get; set; }
        public bool? IsActive { get; set; }

        public string CityName { get; set; }
        public string CityNameEng { get; set; }
        public int? RoutesCount { get; set; }
    }
}