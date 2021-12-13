using VendorSystem.Models;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using VendorSystem.Authorize;
using VendorSystem.Models.Model1;
using VendorSystem.Helper;
using VendorSystem.Repository;

namespace VendorSystem.Controllers
{
    public class HomeController : MyBaseController
    {
        BayanEntities _context = new BayanEntities();


        [AuthorizeShow(PageName = "Home", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangeLanguage(string lang)
        {
            string action = (string)TempData["action"];
            //TempData["lang"] = lang;
            new SiteLanguage().SetLanguage(lang);
            return RedirectToAction(action, "Home");
        }

        [HttpGet]
        public ActionResult Login()
        {
            HttpCookie myCookie = Request.Cookies["Vendor_CompanyID_Cookie"];
            if (myCookie != null)
                ViewBag.Vendor_CompanyID = myCookie.Value;
            else
                ViewBag.Vendor_CompanyID = "";

            return View();
        }


        [HttpPost]
        public ActionResult Login(string userName, string password, string companyId)
        {


            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(companyId))
            {
                TempData["Error"] = CheckUnit.RetriveCorrectMsg("من فضلك أدخل البيانات المطلوبه", "Please Fill in Required Fields");
                return RedirectToAction("Login");
            }

            password = HashPassword.Hash(password);

            var user = _context.Users.FirstOrDefault(c => c.User_Name == userName && c.Password == password && c.Vendor_CompanyID == companyId);
            if (user != null)
            {
                if (user.IsActive == true)
                {
                    Session["UserID"] = user.ID;
                    Session["RoleID"] = user.RoleId;
                    Session["DistributorCode"] = user.DistributorCode;
                    Session["Vendor_CompanyID"] = user.Vendor_CompanyID;
                    var Company = _context.Tbl_Company.FirstOrDefault(w => w.Vendor_CompanyID == user.Vendor_CompanyID);

                    Session["CompanyName"] = CheckUnit.RetriveCorrectMsg(Company.Name, Company.NameEng);
                    Session["LogURL"] = Company.LogURL;


                    // UserInfo Cookie
                    AddUserInfoCookie(user);

                    string URL = Session["URL"] as string;
                    string url = string.IsNullOrEmpty(URL) ? "/Home/Index" : URL;

                    return Redirect(url);
                }
                else
                {
                    TempData["Error"] = CheckUnit.RetriveCorrectMsg("هذا الحساب تم الغاؤه", "This account is deactivated");
                    return RedirectToAction("Login");
                }
            }
            else
            {

                TempData["Error"] = CheckUnit.RetriveCorrectMsg("تاكد من اسم المستخدم و كلمه السر وكود الشركة", "Invalid username, password and company code");
                return RedirectToAction("Login");
            }
        }

        public ActionResult Error404()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            return RedirectToAction("Login");
        }

        [AuthorizeShow(PageName = "Home", TypeButton = TypeButton.Show)]
        public void DownloadTheOrders()
        {
            Thread Th = new Thread(Download);
            Th.Start();
        }

        private void Download()
        {
            string Pass = "QBS@2020"; // it will be configuration and get from database depending on every Company ID
            Integration Integration = new Integration(Pass);
            Integration.DownloadOrderOnly();
        }


        private void AddUserInfoCookie(User user)
        {

            HttpCookie userInfoCookie = new HttpCookie("UserInfo");
            userInfoCookie.Values.Add("UserName", user.Name);
            userInfoCookie.Values.Add("CompanyId", user.Vendor_CompanyID);
            userInfoCookie.Expires = DateTime.Now.AddYears(1);
            System.Web.HttpContext.Current.Response.Cookies.Add(userInfoCookie);
        }

    }
}