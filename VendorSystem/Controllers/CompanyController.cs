using VendorSystem.Authorize;
using VendorSystem.ViewModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using VendorSystem.Repository;
using VendorSystem.Models.Model1;

namespace VendorSystem.Controllers
{
    public class CompanyController : MyBaseController
    {
        BayanEntities _context = new BayanEntities();
        #region Upload Image
        public JsonResult Upload()
        {
            string Path = "../images/" + CreateNewFileFromRequist(Server, Request, 0);
            // Path = new Uri(Path).AbsoluteUri;
            Session["LogURL"] =  Path;
            return Json(Path, JsonRequestBehavior.AllowGet);
        }
        public string CreateNewFileFromRequist(HttpServerUtilityBase Server, HttpRequestBase Request, int fileIndex)
        {
            HttpPostedFileBase Uploadfile = Request.Files[fileIndex];
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
            var Rlt = Request.Files[fileIndex].ContentType.Split('/');
            var Extention = Rlt[Rlt.Length - 1];
            var Dt = DateTime.Now;
            string FileName ="CompID= "+Vendor_CompanyID+ "  "+ Dt.Year+"-"+ Dt.Month+"-"+ Dt.Day+"___"+ Dt.Hour+"-"+ Dt.Minute+"-"+ Dt.Second+"-"+ Dt.Millisecond  + "."+ Extention;

            string path = Path.GetFullPath(Server.MapPath("~/images/" + FileName));
            Uploadfile.SaveAs(path);
            return FileName;
        }
        #endregion

        [AuthorizeShow(PageName = "Company Settings", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
            var Company = _context.Tbl_Company.Where(w => w.Vendor_CompanyID == Vendor_CompanyID).Select(w =>
              new CompanyVM()
              {
                  Company_Address = w.Address,
                  Company_Id = w.Vendor_CompanyID,
                  Company_Name = w.Name,
                  Mobile = w.Mobile,
                  Phone1 = w.Phone1,
                  Website = w.Website,
                  CountryId = w.CountryID,
                  ProvinceId = w.ProvinceID,
                  TerritoryId = w.TerritoryID,
                  CityId = w.CityID,
                  Logo = w.LogURL,
                  Region = w.RegionID
              }
             ).FirstOrDefault();
            Session["LogURL"] = Company.Logo;
            ViewBag.Company = Company;

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;

            LockUpUnit LookUpUnit = new LockUpUnit();
            if (Lang == "ar-SA")
            {
                ViewBag.Country = new SelectList(LookUpUnit.GetAllCountries().Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
            }
            else
            {
                ViewBag.Country = new SelectList(LookUpUnit.GetAllCountries().Select(w => new { ID = w.ID, NameEn = w.NameEng }).ToList(), "ID", "NameEn");
            }
            return View();
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Company Settings", TypeButton = TypeButton.Save)]
        public JsonResult Save(CompanyVM CompanyVM)
        {
            var UserID = Session["UserID"] as int?;
            if (ModelState.IsValid)
            {

                #region Edit
                var Company = _context.Tbl_Company.Find(CompanyVM.Company_Id);
                Company.Name = CompanyVM.Company_Name;
                Company.Address = CompanyVM.Company_Address;
                Company.Mobile = CompanyVM.Mobile;
                Company.Phone1 = CompanyVM.Phone1;
                Company.Website = CompanyVM.Website;
                Company.Updatedby = UserID;
                Company.Lastupdate = DateTime.Now;
                Company.CountryID = CompanyVM.CountryId;
                Company.ProvinceID = CompanyVM.ProvinceId;
                Company.CityID = CompanyVM.CityId;
                Company.RegionID = CompanyVM.Region;
                Company.TerritoryID = CompanyVM.TerritoryId;
                Company.LogURL = Session["LogURL"] as string;
                _context.SaveChanges();
                #endregion
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}