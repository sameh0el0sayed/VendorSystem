using VendorSystem.Authorize;
using VendorSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendorSystem.Models.Model1;

namespace VendorSystem.Controllers
{
    public class NotifcationController : MyBaseController
    {
        BayanEntities context = new BayanEntities();

        [AuthorizeShow(PageName = "Notifcation", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Notifcations = context.Notifcations.Where(w=> w.Vendor_CompanyID == Vendor_CompanyID).OrderByDescending(w=> w.ID).ToList();
            return View(Notifcations);
        }


        [HttpPost]
        public JsonResult ChangeNotifcation(decimal ID)
        {
            var Notfcation = context.Notifcations.Where(v => v.ID == ID).FirstOrDefault();
            Notfcation.Seen = true;
            context.SaveChanges();

            return Json("");
        }
    }
}