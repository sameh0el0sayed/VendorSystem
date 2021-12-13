using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class UserPreview
    {
        public decimal ID { get; set; }
        public string Name { get; set; }
        public string User_Name { get; set; }
        public Nullable<decimal> RoleId { get; set; }
        public string Password { get; set; }
        public int? FormType { get; set; }
        public bool Active { get; set; }
        public string RuleName { get; set; }
        public string MainwarehouseId { get; set; }

    }
}