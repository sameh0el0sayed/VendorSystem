using System.Linq;
using System.Web.Mvc;
using VendorSystem.Models;
using VendorSystem.Authorize;
using VendorSystem.ViewModel;
using System.Globalization;
using VendorSystem.Repository;
using System.Threading;
using VendorSystem.Models.Model1;

namespace VendorSystem.Controllers
{
    public class UserController : MyBaseController
    {
        BayanEntities _context = new BayanEntities();
        UserUnit UserUnit = new UserUnit();


        [AuthorizeShow(PageName = "User", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            RoleUnit RoleUnit = new RoleUnit();
            DistributorUnit DistributorUnit = new DistributorUnit();

            if (Lang == "ar-SA")
            {
                ViewBag.Roles = new SelectList(RoleUnit.GetAllActiveRoles(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.Distributors = new SelectList(DistributorUnit.GetAllActiveDistributors(Vendor_CompanyID).Select(w => new { Code = w.Code, Name = w.Name }).ToList(), "Code", "Name");

                ViewBag.Users = UserUnit.GetAllUsersVM(Vendor_CompanyID).Select(w => new UserVM()
                {
                    IsActive = w.IsActive,
                    ID = w.ID,
                    Name = w.Name,
                    Mobile = w.Mobile,
                    Password = w.Password,
                    UserName = w.UserName,
                    RoleName = w.RoleName,
                    DistributorCode = w.DistributorCode
                }).ToList();
            }
            else
            {
                ViewBag.Roles = new SelectList(RoleUnit.GetAllActiveRoles(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.Distributors = new SelectList(DistributorUnit.GetAllActiveDistributors(Vendor_CompanyID).Select(w => new { Code = w.Code, Name = w.NameEng }).ToList(), "Code", "Name");

                ViewBag.Users = UserUnit.GetAllUsersVM(Vendor_CompanyID).Select(w => new UserVM()
                {
                    IsActive = w.IsActive,
                    ID = w.ID,
                    Name = w.NameEng,
                    Mobile = w.Mobile,
                    Password = w.Password,
                    UserName = w.UserName,
                    RoleName = w.RoleNameEng,
                    DistributorCode = w.DistributorCode
                }).ToList();

            }

            return View();
        }

        [HttpPost]
        [AuthorizeShow(PageName = "User", TypeButton = TypeButton.Save)]
        public JsonResult Save(UserVM UserVM)
        {
            int? UserId = Session["UserID"] as int?;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Result = UserUnit.Save(UserVM, UserId.Value, Vendor_CompanyID);
            return Json(Result);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "User", TypeButton = TypeButton.Search)]
        public JsonResult GetUserByID(int ID)
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Distributor = UserUnit.GetAllUsers(Vendor_CompanyID).Where(w => w.ID == ID).FirstOrDefault();
            return Json(Distributor);
        }
    }
}