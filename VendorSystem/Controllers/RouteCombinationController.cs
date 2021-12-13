using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using VendorSystem.Authorize;
using VendorSystem.Repository;
using VendorSystem.ViewModel;

namespace VendorSystem.Controllers
{
    public class RouteCombinationController : MyBaseController
    {
        RouteCombinationUnit RouteCombinationUnit = new RouteCombinationUnit();

        [AuthorizeShow(PageName = "RouteCombination", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            DistributorUnit DistributorUnit = new DistributorUnit();
            RouteUnit RouteUnit = new RouteUnit();
            LockUpUnit LookUpUnit = new LockUpUnit();
            CustomerUnit CustomerUnit = new CustomerUnit();

            if (Lang == "ar-SA")
            {
                ViewBag.Distributor = new SelectList(DistributorUnit.GetAllActiveDistributors(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.Route = new SelectList(RouteUnit.GetAllActiveRoutes(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.FirstClassification = new SelectList(LookUpUnit.GetAllFirstClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.Customers = new SelectList(CustomerUnit.GetAllActiveCustomersVM(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.CompanyName + " - " + w.StoreName }).ToList(), "ID", "Name");

                ViewBag.RouteCombination = RouteCombinationUnit.GetAllRouteCombinationsVM(Vendor_CompanyID).Select(w => new RouteCombinationVM()
                {
                    Name = w.Name,
                    DistributorName = w.DistributorName,
                    RouteName = w.RouteName,
                    Note = w.Note,
                    ID = w.ID,
                    IsActive = w.IsActive
                }).ToList();
            }
            else
            {
                ViewBag.Distributor = new SelectList(DistributorUnit.GetAllActiveDistributors(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.Route = new SelectList(RouteUnit.GetAllActiveRoutes(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.FirstClassification = new SelectList(LookUpUnit.GetAllFirstClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.Customers = new SelectList(CustomerUnit.GetAllActiveCustomersVM(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.CompanyNameEng + " - " + w.StoreNameEng }).ToList(), "ID", "Name");

                ViewBag.RouteCombination = RouteCombinationUnit.GetAllRouteCombinationsVM(Vendor_CompanyID).Select(w => new RouteCombinationVM()
                {
                    Name = w.NameEng,
                    DistributorName = w.DistributorNameEng,
                    RouteName = w.RouteNameEng,
                    Note = w.Note,
                    ID = w.ID,
                    IsActive = w.IsActive
                }).ToList();
            }
            return View();
        }

        [HttpPost]
        [AuthorizeShow(PageName = "RouteCombination", TypeButton = TypeButton.Save)]
        public JsonResult Save(RouteCombinationVM RouteCombinationVM)
        {
            int? UserId = Session["UserID"] as int?;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            if (RouteCombinationVM.CustomersDtlLst == null)
                RouteCombinationVM.CustomersDtlLst = new List<int?>();

            if (RouteCombinationVM.FirstClassificationLst == null)
                RouteCombinationVM.FirstClassificationLst = new List<int?>();

            if (RouteCombinationVM.SecondClassificationLst == null)
                RouteCombinationVM.SecondClassificationLst = new List<int?>();

            var Result = RouteCombinationUnit.Save(RouteCombinationVM, UserId.Value, Vendor_CompanyID);
            return Json(Result);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "RouteCombination", TypeButton = TypeButton.Search)]
        public JsonResult GetRouteCombinationByID(int ID)
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var RouteCombination = RouteCombinationUnit.GetAllRouteCombinations(Vendor_CompanyID).Where(w => w.ID == ID).Select(w => new RouteCombinationVM
            {
                NameEng = w.NameEng,
                DistributorID = w.DistributorID,
                ID = w.ID,
                IsActive = w.IsActive,
                Name = w.Name,
                Note = w.Note,
                RouteID = w.RouteID
            }).FirstOrDefault();

            var CombinationDtl = RouteCombinationUnit.GetAllActiveCustomersByRouteCombinationID(ID).ToList();
            var Customers = CombinationDtl.Select(w => w.CustDtl_ID).Distinct().ToList();
            var FirstClassLst = CombinationDtl.Select(w => w.FirstClassID).Distinct().ToList();
            var SecondClassLst = CombinationDtl.Select(w => w.SecondClassID).Distinct().ToList();

            RouteCombination.CustomersDtlLst = Customers;
            RouteCombination.FirstClassificationLst = FirstClassLst;
            RouteCombination.SecondClassificationLst = SecondClassLst;

            return Json(RouteCombination);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "RouteCombination", TypeButton = TypeButton.Save)]
        public JsonResult UploadAndSave()
        {
            int? UserID = Session["UserID"] as int?;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Result = RouteCombinationUnit.UploadAndSave(Server, Request, UserID.Value, Vendor_CompanyID);
            return Json(Result);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "RouteCombination", TypeButton = TypeButton.Show)]
        public JsonResult DownloadCurrentStatus()
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
            string Path = "";
            FileVM Result = new FileVM();

            RouteCombinationUnit.DownloadCurrentStatus(Server, Result, Vendor_CompanyID);

            if (Result.Status == "Done")
            {
                int Count = Result.FilePath.Length - 2;
                Path = "/" + Result.FilePath.Substring(2, Count);
                Result.FilePath = Path;
            }
            return Json(Result);
        }

    }
}