using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class RouteVM
    {
        public int ID { get; set; }
        public string RouteCode { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public int? CountryID { get; set; }
        public int? ProvinceID { get; set; }
        public int? CityID { get; set; }
        public int? RegionID { get; set; }
        public int? TerritoryID { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }

        public string CountryName { get; set; }
        public string CountryNameEng { get; set; }

        public string CityName { get; set; }
        public string CityNameEng { get; set; }

    }
}