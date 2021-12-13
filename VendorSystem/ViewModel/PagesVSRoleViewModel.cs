using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class PagesVSRoleViewModel
    {
        public decimal ID { get; set; }
        public decimal RoleId { get; set; }
        public decimal PageId { get; set; }
        public string CompanyID { get; set; }
        public bool save { get; set; }
        public bool saveandpost { get; set; }
        public bool Show { get; set; }
        public bool search { get; set; }
        public bool Print { get; set; }
        public bool Active { get; set; }
        public string PageName { get; set; }
        public int? FormType { get; set; }
    }
}