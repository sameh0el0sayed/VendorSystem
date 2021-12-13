using System.Linq;
using System.Web.Mvc;
using VendorSystem.ViewModel;
using System.Globalization;
using VendorSystem.Authorize;
using System.Threading;
using VendorSystem.Repository;

namespace VendorSystem.Controllers
{

    public class ProductController : MyBaseController
    {
        ProductUnit ProductUnit = new ProductUnit();

        [AuthorizeShow(PageName = "Products", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            LockUpUnit LookUpUnit = new LockUpUnit();
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            if (Lang == "ar-SA")
            {
                ViewBag.FirstClassification = new SelectList(LookUpUnit.GetAllFirstClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.ShelfLife = new SelectList(ProductUnit.GetAllShelfLife().Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
            }
            else
            {
                ViewBag.FirstClassification = new SelectList(LookUpUnit.GetAllFirstClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.ShelfLife = new SelectList(ProductUnit.GetAllShelfLife().Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
            }

            return View();
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Products", TypeButton = TypeButton.Search)]
        public JsonResult RetriveAllProducts()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            LockUpUnit LookUpUnit = new LockUpUnit();

            if (Lang == "ar-SA")
            {
                var Rslt = ProductUnit.GetAllProductsVM(Vendor_CompanyID).Select(w => new
                {
                    IsActive = w.IsActive,
                    BigBarcode = w.BigBarcode == null ? "   " : w.BigBarcode,
                    Name = w.Name,
                    SmallBarcode = w.SmallBarcode,
                    ID = w.ID
                }).ToList();

                return Json(Rslt);
            }
            else
            {
                var Rslt = ProductUnit.GetAllProductsVM(Vendor_CompanyID).Select(w => new
                {
                    IsActive = w.IsActive,
                    BigBarcode = w.BigBarcode == null ? "  " : w.BigBarcode,
                    Name = w.NameEng,
                    SmallBarcode = w.SmallBarcode,
                    ID = w.ID
                }).ToList();

                return Json(Rslt);
            }
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Products", TypeButton = TypeButton.Save)]
        public JsonResult Save(ProductVM ProductVM)
        {
            int? UserId = Session["UserID"] as int?;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Result = ProductUnit.Save(ProductVM, UserId.Value, Vendor_CompanyID);
            return Json(Result);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Products", TypeButton = TypeButton.Search)]
        public JsonResult GetProductByID(int ID)
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Product = ProductUnit.GetAllProducts(Vendor_CompanyID).Where(w => w.ID == ID).Select(w => new ProductVM
            {
                ID = w.ID,
                BigBarcode = w.BigBarcode,
                FirstClassification = w.FirstClassification,
                FourthClassification = w.FourthClassification,
                IsActive = w.IsActive,
                IsLocked = w.IsLocked,
                Name = w.Name,
                NameEng = w.NameEng,
                PiecesInCatron = w.PiecesInCatron,
                SecondClassification = w.SecondClassification,
                SmallBarcode = w.SmallBarcode,
                ThirdClassification = w.ThirdClassification,
                Weight = w.Weight,
                ShelfLifeID = w.ShelfLifeID,
                MinQty = w.MinQty,
                ReturnPercentage = w.ReturnPercentage
            }).FirstOrDefault();

            return Json(Product);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Products", TypeButton = TypeButton.Show)]
        public JsonResult DownloadCurrentStatus()
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
            string Path = "";
            FileVM Result = new FileVM();

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            if (Lang == "ar-SA")
            {
                ProductUnit.DownloadCurrentStatus(Server, Result, Vendor_CompanyID, true);
            }
            else
            {
                ProductUnit.DownloadCurrentStatus(Server, Result, Vendor_CompanyID, false);
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
        [AuthorizeShow(PageName = "Products", TypeButton = TypeButton.Save)]
        public JsonResult UploadAndSave()
        {
            int? UserID = Session["UserID"] as int?;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Result = ProductUnit.UploadAndSave(Server, Request, UserID.Value, Vendor_CompanyID);
            return Json(Result);
        }

    }
}