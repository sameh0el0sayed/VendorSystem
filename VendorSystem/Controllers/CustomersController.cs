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
    public class CustomersController : MyBaseController
    {
        CustomerUnit CustomerUnit = new CustomerUnit();

        [AuthorizeShow(PageName = "Customers", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            LockUpUnit LookUpUnit = new LockUpUnit();
            CustomerUnit CustomerUnit = new CustomerUnit();

            if (Lang == "ar-SA")
            {
                ViewBag.Country = new SelectList(LookUpUnit.GetAllCountries().Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.StoreType = new SelectList(LookUpUnit.GetAllStoreType(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.StoreCategory = new SelectList(LookUpUnit.GetAllStoreCategory(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.StoreClassification = new SelectList(LookUpUnit.GetAllStoreClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");

                ViewBag.Customer = CustomerUnit.GetAllCustomersVM(Vendor_CompanyID).Select(w => new CustomerVM()
                {
                    Address = w.Address,
                    Mobile = w.Mobile,
                    ID = w.ID,
                    StoreName = w.StoreName,
                    CompanyName = w.CompanyName,
                    CityName = w.CityName,
                    IsActive = w.IsActive,
                    CustomerCode = w.CustomerCode
                }).ToList();
            }
            else
            {
                ViewBag.Country = new SelectList(LookUpUnit.GetAllCountries().Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.StoreType = new SelectList(LookUpUnit.GetAllStoreType(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.StoreCategory = new SelectList(LookUpUnit.GetAllStoreCategory(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.StoreClassification = new SelectList(LookUpUnit.GetAllStoreClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");

                ViewBag.Customer = CustomerUnit.GetAllCustomersVM(Vendor_CompanyID).Select(w => new CustomerVM()
                {
                    Address = w.Address,
                    Mobile = w.Mobile,
                    ID = w.ID,
                    StoreName = w.StoreNameEng,
                    CompanyName = w.CompanyNameEng,
                    CityName = w.CityNameEng,
                    IsActive = w.IsActive,
                    CustomerCode = w.CustomerCode
                }).ToList();

            }

            return View();
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Customers", TypeButton = TypeButton.Save)]
        public JsonResult Save(CustomerVM CustomerVM)
        {

            int? UserId = Session["UserID"] as int?;
            var Result = CustomerUnit.Save(CustomerVM, UserId.Value);
            return Json(Result);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Customers", TypeButton = TypeButton.Search)]
        public JsonResult GetCustomerByID(int ID)
        {
            var Customer = CustomerUnit.GetCustomerVM_ByID(ID);
            return Json(Customer);
        }

    }
}