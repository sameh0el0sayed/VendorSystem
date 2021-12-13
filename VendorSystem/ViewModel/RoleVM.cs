using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorSystem.ViewModel
{
    public class RoleVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public bool IsActive { get; set; }
    }
}