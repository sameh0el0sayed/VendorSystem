using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using VendorSystem.Authorize;
using VendorSystem.Models.Model1;
using VendorSystem.Repository;
using VendorSystem.ViewModel;

namespace VendorSystem.Controllers
{
    public class OrderConfigController : Controller
    {
        OrderConfigUnit _orderConfigUnit = new OrderConfigUnit();
        CustomerUnit _customerUnit = new CustomerUnit();

        [AuthorizeShow(PageName = "Order Configuration", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            LockUpUnit LookUpUnit = new LockUpUnit();

            if (Lang == "ar-SA")
            {
                ViewBag.ScheduleType = new SelectList(_orderConfigUnit.GetAllScheduleType().Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.WeekDays = new SelectList(_orderConfigUnit.GetAllWeekDays().Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.Customers = new SelectList(_customerUnit.GetAllActiveCustomersVM(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.CompanyName + " - " + w.StoreName }).ToList(), "ID", "Name");
            }
            else
            {
                ViewBag.ScheduleType = new SelectList(_orderConfigUnit.GetAllScheduleType().Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.WeekDays = new SelectList(_orderConfigUnit.GetAllWeekDays().Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.Customers = new SelectList(_customerUnit.GetAllActiveCustomersVM(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.CompanyNameEng + " - " + w.StoreNameEng }).ToList(), "ID", "Name");
            }

            return View();
        }

        [HttpPost]
        [AuthorizeShow(PageName = "ReplenishmentConfiguration", TypeButton = TypeButton.Save)]
        public JsonResult Save(OrderConfigVM _orderConfigVM)
        {
            int? UserId = Session["UserID"] as int?;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            if (_orderConfigVM.VisitSchedule_Data == null)
            {
                _orderConfigVM.VisitSchedule_Data = new List<OrderConfig_VisitScheduleVM>();
            }
            var Result = _orderConfigUnit.Save(_orderConfigVM, UserId.Value, Vendor_CompanyID);
            return Json(Result);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "ReplenishmentConfiguration", TypeButton = TypeButton.Search)]
        public JsonResult GetAllOrderConfig()
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var List = _orderConfigUnit.GetAllOrderConfig(Vendor_CompanyID);

            try
            {
                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                string Lang = currentCulture.Name;
                if (Lang == "ar-SA")
                {
                    var Result = List.Select(w => new { ID = w.ID, Name = w.Name, Status = w.IsActive == true ? "نشط" : "غير نشط" }).ToList();
                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Result = List.Select(w => new { ID = w.ID, Name = w.NameEng, Status = w.IsActive == true ? "Active" : "Not Active" }).ToList();
                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        [AuthorizeShow(PageName = "ReplenishmentConfiguration", TypeButton = TypeButton.Save)]
        public JsonResult GetOrderConfigByID(decimal _ID)
        {
     
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
 

            try
            {
                var _orderConfigVM = _orderConfigUnit.GetOrderConfigByID(Vendor_CompanyID, _ID).Select(w => new OrderConfigVM
                {
                        ID = w.ID,
                        Name = w.Name,
                        NameEng = w.NameEng,
                        VisitScheduleTypeID = w.VisitScheduleTypeID, 
                        VisitScheduleCycleCount = w.VisitScheduleCycleCount,
                        IsActive = w.IsActive,
                }).FirstOrDefault();
                _orderConfigVM.CustomersDtlLst = _orderConfigUnit.GetOrderConfigCustomersByID(_ID).ToList();




                return Json(_orderConfigVM, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }

    }
}