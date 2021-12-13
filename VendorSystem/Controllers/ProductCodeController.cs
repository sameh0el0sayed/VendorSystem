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
    public class ProductCodeController : MyBaseController
    {
        ProductCodeUnit ProductCodeUnit = new ProductCodeUnit();

        [AuthorizeShow(PageName = "Product Code", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [AuthorizeShow(PageName = "Product Code", TypeButton = TypeButton.Show)]
        public JsonResult DownloadCurrentStatus()
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
            var DistributorCode = Session["DistributorCode"] as string;
            string Path = "";
            FileVM Result = new FileVM();

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            if (Lang == "ar-SA")
            {
                ProductCodeUnit.DownloadCurrentStatus(DistributorCode, Server, Result, Vendor_CompanyID, true);
            }
            else
            {
                ProductCodeUnit.DownloadCurrentStatus(DistributorCode, Server, Result, Vendor_CompanyID, false);
            }

           

            if (Result.Status == "Done")
            {
                int Count = Result.FilePath.Length - 2;
                Path = "/" + Result.FilePath.Substring(2, Count);
                Result.FilePath = Path;
            }
            return Json(Result);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Product Code", TypeButton = TypeButton.Save)]
        public JsonResult UploadAndSaveInternalCode()
        {
            int? UserID = Session["UserID"] as int?;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            string DistributorCode = Session["DistributorCode"].ToString();

            var Result = ProductCodeUnit.UploadAndSaveInternalCode(Server, Request, DistributorCode, UserID.Value, Vendor_CompanyID);
            return Json(Result);
        }
    }
}