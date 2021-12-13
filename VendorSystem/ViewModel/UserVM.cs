using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class UserVM
    {
        public decimal ID { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public int RoleID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsManufacture { get; set; }
        public string RoleName { get; set; }
        public string RoleNameEng { get; set; }

        public string DistributorCode { get; set; }

    }
}