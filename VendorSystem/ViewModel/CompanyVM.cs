using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace  VendorSystem.ViewModel
{
    public class CompanyVM
    {
        public string Company_Id { get; set; }
        public string Company_Name { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> ProvinceId { get; set; }
        public Nullable<int> Region { get; set; }
        public Nullable<int> TerritoryId { get; set; }
        public Nullable<int> CityId { get; set; }
        public string Company_Address { get; set; }
        public string Phone1 { get; set; }
        public string Mobile { get; set; }
        public string Website { get; set; }
        public string Logo { get; set; }
    }
}