using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace  VendorSystem.ViewModel
{
    public class RoleViewModel
    {
        public decimal ID{ get; set;}
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Requird")]
        public string Name { get; set; }
        public int? FormType { get; set; }
        public bool Active { get; set; }
    }
}