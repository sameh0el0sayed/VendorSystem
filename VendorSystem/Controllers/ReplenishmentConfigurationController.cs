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
    public class ReplenishmentConfigurationController : MyBaseController
    {
        ReplenishConfigUnit ReplenishConfigUnit = new ReplenishConfigUnit();


       


        [AuthorizeShow(PageName = "ReplenishmentConfiguration", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            LockUpUnit LookUpUnit = new LockUpUnit();
             
            if (Lang == "ar-SA")
            {
                ViewBag.FirstClassification = new SelectList(LookUpUnit.GetAllFirstClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.CalcType = new SelectList(ReplenishConfigUnit.GetAllCalculationType().Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.ScheduleType = new SelectList(ReplenishConfigUnit.GetAllScheduleType().Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.WeekDays = new SelectList(ReplenishConfigUnit.GetAllWeekDays().Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
            }
            else
            {
                ViewBag.FirstClassification = new SelectList(LookUpUnit.GetAllFirstClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.CalcType = new SelectList(ReplenishConfigUnit.GetAllCalculationType().Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.ScheduleType = new SelectList(ReplenishConfigUnit.GetAllScheduleType().Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.WeekDays = new SelectList(ReplenishConfigUnit.GetAllWeekDays().Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
            }

            return View();
        }

        [HttpPost]
        [AuthorizeShow(PageName = "ReplenishmentConfiguration", TypeButton = TypeButton.Save)]
        public JsonResult Save(ReplenishConfigVM ReplenishConfigVM)
        {
            int? UserId = Session["UserID"] as int?;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            if(ReplenishConfigVM.RepSchedule_Data == null)
            {
                ReplenishConfigVM.RepSchedule_Data = new List<ReplenishConfig_RepScheduleVM>();
            } 
            //if (ReplenishConfigVM.VisitSchedule_Data == null)
            //{
            //    ReplenishConfigVM.VisitSchedule_Data = new List<ReplenishConfig_VisitScheduleVM>();
            //}
            var Result = ReplenishConfigUnit.Save(ReplenishConfigVM, UserId.Value, Vendor_CompanyID);
            return Json(Result);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "ReplenishmentConfiguration", TypeButton = TypeButton.Search)]
        public JsonResult GetAllReplenishConfig()
        { 
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
             
            var List = ReplenishConfigUnit.GetAllReplenishConfig(Vendor_CompanyID);
            
            try
            {
                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture; 
                string Lang = currentCulture.Name;
                if (Lang == "ar-SA")
                {
                    var Result = List.Select(w => new  { ID = w.ID, Name = w.Name, Status =  w.IsActive==true ? "نشط" : "غير نشط"  }).ToList();
                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Result = List.Select(w => new  { ID = w.ID, Name = w.NameEng, Status = w.IsActive == true ? "Active" : "Not Active" }).ToList();
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
        public JsonResult GetReplenishConfigByID(decimal _ID)
        { 
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
             
            var List = ReplenishConfigUnit.GetReplenishConfigByID(Vendor_CompanyID, _ID);

            try
            {
                var Result = List.Select(w => new {
                    ID = w.ID,
                    Name = w.Name,
                    NameEng = w.NameEng,
                    CalcTypeID = w.CalcTypeID,
                    RepScheduleTypeID = w.RepScheduleTypeID,
                    IsActive = w.IsActive,
                    IsIncludeMinQty = w.IsIncludeMinQty,
                    FirstClassID = w.FirstClassID,
                    SecondClassID = w.SecondClassID,
                    ThirdClassID = w.ThirdClassID,
                    FourthClassID = w.FourthClassID
                }).ToList();
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }

    }
}