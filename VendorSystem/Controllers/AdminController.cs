
using System.Web.Mvc;
using VendorSystem.Models;
using VendorSystem.Authorize;
using VendorSystem.Models.Model1;

namespace VendorSystem.Controllers
{
    public class AdminController : Controller
    {
        BayanEntities context = new BayanEntities();

        public ActionResult ChangeLanguage(string lang)
        {
            string action = (string)TempData["action"];
            string Controller = (string)TempData["Controller"];
            new SiteLanguage().SetLanguage(lang);
            return RedirectToAction(action, Controller);
        } 
    }
}
