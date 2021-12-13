using System;
using System.Linq;
using System.Web.Mvc;
using VendorSystem.ViewModel;
using VendorSystem.Authorize;
using System.Globalization;
using VendorSystem.Repository;
using System.Threading;

namespace VendorSystem.Controllers
{
    public class RoleController : MyBaseController
    {
        RoleUnit RoleUnit = new RoleUnit();


        [AuthorizeShow(PageName = "Role", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            RoleUnit RoleUnit = new RoleUnit();
            DistributorUnit DistributorUnit = new DistributorUnit();

            if (Lang == "ar-SA")
            {

                ViewBag.Roles = RoleUnit.GetAllRolesVM(Vendor_CompanyID).Select( w => new RoleVM()
                {
                    IsActive = w.IsActive,
                    ID = w.ID,
                    Name = w.Name
                }).ToList();
            }
            else
            {
                ViewBag.Roles = new SelectList(RoleUnit.GetAllActiveRoles(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.Distributors = new SelectList(DistributorUnit.GetAllActiveDistributors(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");

                ViewBag.Roles = RoleUnit.GetAllRolesVM(Vendor_CompanyID).Select(w => new RoleVM()
                {
                    IsActive = w.IsActive,
                    ID = w.ID,
                    Name = w.NameEng
                }).ToList();

            }

            return View();
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Role", TypeButton = TypeButton.Save)]
        public JsonResult Save(RoleVM RoleVM)
        {
            int? UserID = Session["UserID"] as int?;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Result = RoleUnit.Save(RoleVM, UserID.Value, Vendor_CompanyID);
            return Json(Result);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Role", TypeButton = TypeButton.Search)]
        public JsonResult GetRoleByID(int ID)
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Distributor = RoleUnit.GetAllRoles(Vendor_CompanyID).Where(w => w.ID == ID).FirstOrDefault();
            return Json(Distributor);
        }
    }
}