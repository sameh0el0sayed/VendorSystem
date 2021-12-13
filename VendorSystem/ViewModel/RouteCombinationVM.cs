using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class RouteCombinationVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public int? DistributorID { get; set; }
        public int? RouteID { get; set; }
        public List<int?> CustomersDtlLst { get; set; }

        public string Note { get; set; }
        public bool? IsActive { get; set; }

        public string DistributorName { get; set; }
        public string DistributorNameEng { get; set; }

        public string RouteName { get; set; }
        public string RouteNameEng { get; set; }

        public List<int?> FirstClassificationLst { get; set; }
        public List<int?> SecondClassificationLst { get; set; }



    }
}