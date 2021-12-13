using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace VendorSystem.Models
{
    public partial class BayanEntities
    {
        public BayanEntities() : this(1)
        {
            //var objectContext = (this as IObjectContextAdapter).ObjectContext;
            //objectContext.CommandTimeout = 300;
            //this.Configuration.ProxyCreationEnabled = false;

        }
    }
}


