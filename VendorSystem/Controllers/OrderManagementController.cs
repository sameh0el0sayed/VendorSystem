using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Mvc;
using VendorSystem.Authorize;
using VendorSystem.ViewModel;
using VendorSystem.Repository;
using VendorSystem.Models.Model1;
using Microsoft.Reporting.WebForms;

namespace VendorSystem.Controllers
{
    public class OrderManagementController : MyBaseController
    {
        BayanEntities DB = new BayanEntities();
        string Password = "QBS@2020";


        #region View Pages
        [AuthorizeShow(PageName = "Receive Purchase Order", TypeButton = TypeButton.Show)]
        public ActionResult Received()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            LockUpUnit LookUpUnit = new LockUpUnit();
            if (Lang == "ar-SA")
            {
                ViewBag.SubWareHouse = new SelectList(DB.Tbl_CustomerMaster.Select(w => new { StoreID = w.StoreID + w.CompanyID, StoreName = w.CompanyName + "-" + w.StoreName }).ToList(), "StoreID", "StoreName");
                ViewBag.Catogaries = new SelectList(LookUpUnit.GetAllFirstClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.Cards = new SelectList(LookUpUnit.GetAllCards(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
            }
            else
            {
                ViewBag.SubWareHouse = new SelectList(DB.Tbl_CustomerMaster.Select(w => new { StoreID = w.StoreID + w.CompanyID, StoreName = w.CompanyNameEng + "-" + w.StoreNameEng }).ToList(), "StoreID", "StoreName");
                ViewBag.Catogaries = new SelectList(LookUpUnit.GetAllFirstClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.Cards = new SelectList(LookUpUnit.GetAllCards(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");

            }
            List<ID_NameVM> Products = new List<ID_NameVM>();
            ViewBag.Products = new SelectList(Products, "ID", "Name");

            List<ID_NameVM> SubCatogaries = new List<ID_NameVM>();
            ViewBag.SubCatogaries = new SelectList(SubCatogaries, "ID", "Name");
            return View();
        }

        [AuthorizeShow(PageName = "Shipped Purchase Order", TypeButton = TypeButton.Show)]
        public ActionResult Shipped()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            LockUpUnit LookUpUnit = new LockUpUnit();
            if (Lang == "ar-SA")
            {
                ViewBag.SubWareHouse = new SelectList(DB.Tbl_CustomerMaster.Select(w => new { StoreID = w.StoreID + w.CompanyID, StoreName = w.CompanyName + "-" + w.StoreName }).ToList(), "StoreID", "StoreName");
                ViewBag.Catogaries = new SelectList(LookUpUnit.GetAllFirstClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.Cards = new SelectList(LookUpUnit.GetAllCards(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
            }
            else
            {
                ViewBag.SubWareHouse = new SelectList(DB.Tbl_CustomerMaster.Select(w => new { StoreID = w.StoreID + w.CompanyID, StoreName = w.CompanyNameEng + "-" + w.StoreNameEng }).ToList(), "StoreID", "StoreName");
                ViewBag.Catogaries = new SelectList(LookUpUnit.GetAllFirstClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.Cards = new SelectList(LookUpUnit.GetAllCards(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");

            }

            List<ID_NameVM> Products = new List<ID_NameVM>();
            ViewBag.Products = new SelectList(Products, "ID", "Name");

            List<ID_NameVM> SubCatogaries = new List<ID_NameVM>();
            ViewBag.SubCatogaries = new SelectList(SubCatogaries, "ID", "Name");
            return View();
        }

        [AuthorizeShow(PageName = "Delivered Purchase Order", TypeButton = TypeButton.Show)]
        public ActionResult Delivered()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            LockUpUnit LookUpUnit = new LockUpUnit();
            if (Lang == "ar-SA")
            {
                ViewBag.SubWareHouse = new SelectList(DB.Tbl_CustomerMaster.Select(w => new { StoreID = w.StoreID + w.CompanyID, StoreName = w.CompanyName + "-" + w.StoreName }).ToList(), "StoreID", "StoreName");
                ViewBag.Catogaries = new SelectList(LookUpUnit.GetAllFirstClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.Cards = new SelectList(LookUpUnit.GetAllCards(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
            }
            else
            {
                ViewBag.SubWareHouse = new SelectList(DB.Tbl_CustomerMaster.Select(w => new { StoreID = w.StoreID + w.CompanyID, StoreName = w.CompanyNameEng + "-" + w.StoreNameEng }).ToList(), "StoreID", "StoreName");
                ViewBag.Catogaries = new SelectList(LookUpUnit.GetAllFirstClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.Cards = new SelectList(LookUpUnit.GetAllCards(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");

            }

            List<ID_NameVM> Products = new List<ID_NameVM>();
            ViewBag.Products = new SelectList(Products, "ID", "Name");

            List<ID_NameVM> SubCatogaries = new List<ID_NameVM>();
            ViewBag.SubCatogaries = new SelectList(SubCatogaries, "ID", "Name");
            return View();
        }

        [AuthorizeShow(PageName = "Rejected Purchase Order", TypeButton = TypeButton.Show)]
        public ActionResult Rejected()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;


            LockUpUnit LookUpUnit = new LockUpUnit();
            if (Lang == "ar-SA")
            {
                ViewBag.SubWareHouse = new SelectList(DB.Tbl_CustomerMaster.Select(w => new { StoreID = w.StoreID + w.CompanyID, StoreName = w.CompanyName + "-" + w.StoreName }).ToList(), "StoreID", "StoreName");
                ViewBag.Catogaries = new SelectList(LookUpUnit.GetAllFirstClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
                ViewBag.Cards = new SelectList(LookUpUnit.GetAllCards(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList(), "ID", "Name");
            }
            else
            {
                ViewBag.SubWareHouse = new SelectList(DB.Tbl_CustomerMaster.Select(w => new { StoreID = w.StoreID + w.CompanyID, StoreName = w.CompanyNameEng + "-" + w.StoreNameEng }).ToList(), "StoreID", "StoreName");
                ViewBag.Catogaries = new SelectList(LookUpUnit.GetAllFirstClassification(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
                ViewBag.Cards = new SelectList(LookUpUnit.GetAllCards(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList(), "ID", "Name");
            }

            List<ID_NameVM> Products = new List<ID_NameVM>();
            ViewBag.Products = new SelectList(Products, "ID", "Name");

            List<ID_NameVM> SubCatogaries = new List<ID_NameVM>();
            ViewBag.SubCatogaries = new SelectList(SubCatogaries, "ID", "Name");
            return View();
        }
        #endregion

        //Sameh Code
        [HttpPost]
        [AuthorizeShow(PageName = "Receive Purchase Order", TypeButton = TypeButton.Search)]
        public JsonResult GetPOsPerStatusID(decimal StatusID)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            if (Lang == "ar-SA")
            {
                var AllPO = (from DPO in DB.OrderManagementMasters
                             from Cust in DB.Tbl_CustomerMaster
                             from CustDtl in DB.Tbl_CustomerDtl
                             where Cust.StoreID == DPO.StoreId
                             && Cust.CompanyID == DPO.CompanyID
                             && DPO.Active == true
                             && DPO.Status == StatusID
                             && DPO.Vendor_CompanyID == Vendor_CompanyID
                             && CustDtl.CustomerMasterID == Cust.ID
                             select new
                             {
                                 DocumentNumber = DPO.DocumentNumber,
                                 ID = DPO.ID,
                                 SentDate = DPO.SentDate,
                                 StoreName = Cust.StoreName + "-" + Cust.CompanyName,
                                 ShippedDate = DPO.ShippedDate,
                                 RecievedDate = DPO.RecievedDate,
                                 ExpectedDeliveredDate = DPO.ExpectedDeliveryDate,
                                 CustomerCode = CustDtl.CustoemrCode

                             }).OrderByDescending(w => w.ID).ToList().Select(w => new
                             {
                                 DocumentNumber = w.DocumentNumber,
                                 ID = w.ID,
                                 SentDate = w.SentDate == null ? "" : w.SentDate.Value.ToShortDateString(),
                                 StoreName = w.StoreName,
                                 ShippedDate = w.ShippedDate == null ? "" : w.ShippedDate.Value.ToShortDateString(),
                                 RecievedDate = w.RecievedDate == null ? "" : w.RecievedDate.Value.ToShortDateString(),
                                 ExpectedDeliveredDate = w.ExpectedDeliveredDate == null ? "" : w.ExpectedDeliveredDate.Value.ToShortDateString(),
                                 CustomerCode = w.CustomerCode

                             }).ToList();
                return Json(AllPO);
            }

            else
            {
                var AllPO = (from DPO in DB.OrderManagementMasters
                             from Cust in DB.Tbl_CustomerMaster
                             from CustDtl in DB.Tbl_CustomerDtl 
                             where Cust.StoreID == DPO.StoreId
                             && Cust.CompanyID == DPO.CompanyID
                             && DPO.Active == true
                             && DPO.Status == StatusID
                             && DPO.Vendor_CompanyID == Vendor_CompanyID 
                             && CustDtl.CustomerMasterID == Cust.ID
                             select new
                             {
                                 DocumentNumber = DPO.DocumentNumber,
                                 ID = DPO.ID,
                                 SentDate = DPO.SentDate,
                                 StoreName = Cust.StoreNameEng + "-" + Cust.CompanyNameEng,
                                 ShippedDate = DPO.ShippedDate,
                                 RecievedDate = DPO.RecievedDate,
                                 ExpectedDeliveredDate = DPO.ExpectedDeliveryDate,
                                 CustomerCode = CustDtl.CustoemrCode

                             }).OrderByDescending(w => w.ID).ToList().Select(w => new
                             {
                                 DocumentNumber = w.DocumentNumber,
                                 ID = w.ID,
                                 SentDate = w.SentDate == null ? "" : w.SentDate.Value.ToString("dd/MM/yyyy"),
                                 StoreName = w.StoreName,
                                 ShippedDate = w.ShippedDate == null ? "" : w.ShippedDate.Value.ToString("dd/MM/yyyy"),
                                 RecievedDate = w.RecievedDate == null ? "" : w.RecievedDate.Value.ToString("dd/MM/yyyy"),
                                 ExpectedDeliveredDate = w.ExpectedDeliveredDate == null ? "" : w.ExpectedDeliveredDate.Value.ToString("dd/MM/yyyy"),
                                 CustomerCode = w.CustomerCode

                             }).ToList();
                return Json(AllPO);
            }
        }

        //Sameh Code
        [HttpPost]
        [AuthorizeShow(PageName = "Receive Purchase Order", TypeButton = TypeButton.Search)]
        public JsonResult GetPoByMasterID(decimal _MasterID)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            if (Lang == "ar-SA")
            {

                var OrderDetails_Lst = (from Dtl in DB.OrderManagementDetails
                                        from mstr in DB.OrderManagementMasters
                                        from ProdAttr in DB.Tbl_ProductAttribute
                                        from Prod in DB.Tbl_Product
                                        from Cust in DB.Tbl_CustomerMaster
                                        from CustDtl in DB.Tbl_CustomerDtl 
                                        where mstr.ID == Dtl.MasterID
                                        && Cust.StoreID == mstr.StoreId
                                        && Cust.CompanyID == mstr.CompanyID
                                        && CustDtl.CustomerMasterID == Cust.ID
                                        && Dtl.MasterID == _MasterID
                                        && Prod.ID == ProdAttr.ProductID
                                        && ProdAttr.Barcode == Dtl.Barcode
                                        && Dtl.IsActive == true
                                        select new
                                        {
                                            ApprovedQty = Dtl.ApprovedQty,
                                            BarCode = Dtl.Barcode,
                                            ProductNa = Prod.Name,
                                            ShippedQty = Dtl.ShippedQty == null ? 0 : Dtl.ShippedQty,
                                            SystemQty = Dtl.SystemQty,
                                            VendorUnitPrice = Dtl.VendorUnitPrice,
                                            DocumentNumber = mstr.DocumentNumber,
                                            StoreId = mstr.StoreId + mstr.CompanyID,
                                            ExpectedDeliveryDate = mstr.ExpectedDeliveryDate,
                                            MarketUnitPrice = Dtl.MarketUnitPrice,
                                            DeliveredQty = Dtl.ActualRecivedQty,
                                            InternalCode = Dtl.InternalCode,
                                            CustomerCode = CustDtl.CustoemrCode,
                                            CustomerMasterId = Cust.ID

                                        }).ToList().Select(w => new PODetailsVM()
                                        {
                                            ApprovedQty = w.ApprovedQty,
                                            BarCode = w.BarCode,
                                            ProductNa = w.ProductNa,
                                            ShippedQty = w.ShippedQty,
                                            SystemQty = w.SystemQty,
                                            VendorUnitPrice = w.VendorUnitPrice,
                                            DocumentNumber = w.DocumentNumber,
                                            StoreId = w.StoreId,
                                            ExpectedDeliveredDate = w.ExpectedDeliveryDate == null ? "" : w.ExpectedDeliveryDate.Value.ToString("dd/MM/yyyy"),
                                            MarketUnitPrice = w.MarketUnitPrice,
                                            DeliveredQty = w.DeliveredQty,
                                            InternalCode = w.InternalCode,
                                            CustomerCode = w.CustomerCode,
                                            CustomerMasterId = w.CustomerMasterId
                                        }).ToList();

                var custMID = OrderDetails_Lst.FirstOrDefault().CustomerMasterId; 

                var customerMasterCenterLocal = (from Cust in DB.Tbl_CustomerMaster
                                                 where Cust.ID == custMID
                                                 select new
                                                 {
                                                     RegionID = Cust.RegionID,
                                                     TerritoryID = Cust.TerritoryID,
                                                      
                                                 }).ToList().Select(c => new CustomerVM()
                                                 {

                                                    RegionID = c.RegionID,
                                                    TerritoryID = c.TerritoryID,

                                                 }).FirstOrDefault();


                var RouteName = (from RCust in DB.Tbl_RouteCombinDtlCustomer
                                 from RouteCombinMaster in DB.Tbl_RouteCombinMaster
                                 from Cust in DB.Tbl_CustomerDtl
                                 where RCust.CustID == Cust.ID
                                 && RouteCombinMaster.ID == RCust.MstrID
                                 && Cust.CustomerMasterID == custMID
                                 && Cust.Vendor_CompanyID == Vendor_CompanyID
                                 select new
                                 {
                                     RouteCombinMaster.Name

                                 }).FirstOrDefault()?.Name;

                var CLockup_list= (from CLockup in DB.Tbl_CentralizedLockUp 
                                   select new {CLockup.ID,CLockup.Name}).ToList();

                var RegionName = CLockup_list.Where(m => m.ID == customerMasterCenterLocal.RegionID).Select(m=>m.Name).FirstOrDefault()  ;
                var TerritoryName = CLockup_list.Where(m => m.ID == customerMasterCenterLocal.TerritoryID).Select(m => m.Name).FirstOrDefault();

                var OrderDetails_Lst_With_Route = OrderDetails_Lst.Select(w => new
                { 
                    ApprovedQty = w.ApprovedQty,
                    BarCode = w.BarCode,
                    ProductNa = w.ProductNa,
                    ShippedQty = w.ShippedQty,
                    SystemQty = w.SystemQty,
                    VendorUnitPrice = w.VendorUnitPrice,
                    DocumentNumber = w.DocumentNumber,
                    StoreId = w.StoreId,
                    ExpectedDeliveredDate = w.ExpectedDeliveredDate,
                    MarketUnitPrice = w.MarketUnitPrice,
                    DeliveredQty = w.DeliveredQty,
                    InternalCode = w.InternalCode,
                    CustomerCode = w.CustomerCode,
                    CustomerMasterId = w.CustomerMasterId,
                    RouteName = RouteName ,
                    RegionName = RegionName ,
                    TerritoryName = TerritoryName  ,
               }).ToList();


                return Json(OrderDetails_Lst_With_Route);
            }
            else
            {
                var OrderDetails_Lst = (from Dtl in DB.OrderManagementDetails
                                        from mstr in DB.OrderManagementMasters
                                        from ProdAttr in DB.Tbl_ProductAttribute
                                        from Prod in DB.Tbl_Product
                                        from CustDtl in DB.Tbl_CustomerDtl
                                        from Cust in DB.Tbl_CustomerMaster 
                                        where mstr.ID == Dtl.MasterID
                                        && Dtl.MasterID == _MasterID
                                        && Cust.StoreID == mstr.StoreId
                                        && Cust.CompanyID == mstr.CompanyID
                                        && CustDtl.CustomerMasterID == Cust.ID && Prod.ID == ProdAttr.ProductID
                                        && ProdAttr.Barcode == Dtl.Barcode
                                        && Dtl.IsActive == true
                                        select new
                                        {
                                            ApprovedQty = Dtl.ApprovedQty,
                                            BarCode = Dtl.Barcode,
                                            ProductNa = Prod.NameEng,
                                            ShippedQty = Dtl.ShippedQty == null ? 0 : Dtl.ShippedQty,
                                            SystemQty = Dtl.SystemQty,
                                            VendorUnitPrice = Dtl.VendorUnitPrice,
                                            DocumentNumber = mstr.DocumentNumber,
                                            StoreId = mstr.StoreId + mstr.CompanyID,
                                            ExpectedDeliveryDate = mstr.ExpectedDeliveryDate,
                                            MarketUnitPrice = Dtl.MarketUnitPrice,
                                            DeliveredQty = Dtl.ActualRecivedQty,
                                            InternalCode = Dtl.InternalCode,
                                            CustomerCode = CustDtl.CustoemrCode,
                                            CustomerMasterId = Cust.ID

                                        }).ToList().Select(w => new PODetailsVM()
                                        {
                                            ApprovedQty = w.ApprovedQty,
                                            BarCode = w.BarCode,
                                            ProductNa = w.ProductNa,
                                            ShippedQty = w.ShippedQty,
                                            SystemQty = w.SystemQty,
                                            VendorUnitPrice = w.VendorUnitPrice,
                                            DocumentNumber = w.DocumentNumber,
                                            StoreId = w.StoreId,
                                            ExpectedDeliveredDate = w.ExpectedDeliveryDate == null ? "" : w.ExpectedDeliveryDate.Value.ToString("dd/MM/yyyy"),
                                            MarketUnitPrice = w.MarketUnitPrice,
                                            DeliveredQty = w.DeliveredQty,
                                            InternalCode = w.InternalCode,
                                            CustomerCode = w.CustomerCode,
                                            CustomerMasterId = w.CustomerMasterId


                                        }).ToList();


                var custMID = OrderDetails_Lst.FirstOrDefault().CustomerMasterId; 

                var customerMasterCenterLocal = (from Cust in DB.Tbl_CustomerMaster
                                                 where Cust.ID == custMID
                                                 select new
                                                 {
                                                     RegionID = Cust.RegionID,
                                                     TerritoryID = Cust.TerritoryID,
                                                 }).ToList().Select(c => new CustomerVM()
                                                 {
                                                     RegionID = c.RegionID,
                                                     TerritoryID = c.TerritoryID,
                                                 }).FirstOrDefault(); 

                var RouteName = (from RCust in DB.Tbl_RouteCombinDtlCustomer
                                 from RouteCombinMaster in DB.Tbl_RouteCombinMaster
                                 from Cust in DB.Tbl_CustomerDtl
                                 where RCust.CustID == Cust.ID
                                 && RouteCombinMaster.ID == RCust.MstrID
                                 && Cust.CustomerMasterID == custMID
                                 && Cust.Vendor_CompanyID == Vendor_CompanyID
                                 select new
                                 { RouteCombinMaster.NameEng }).FirstOrDefault()?.NameEng;

                var CLockup_list = (from CLockup in DB.Tbl_CentralizedLockUp
                                    select new
                                    {CLockup.ID, CLockup.NameEng }).ToList();

                var RegionName = CLockup_list.Where(m => m.ID == customerMasterCenterLocal.RegionID).Select(m => m.NameEng).FirstOrDefault()  ;
                var TerritoryName = CLockup_list.Where(m => m.ID == customerMasterCenterLocal.TerritoryID).Select(m => m.NameEng).FirstOrDefault() ;


                var OrderDetails_Lst_With_Route = OrderDetails_Lst.Select(w => new PODetailsVM()
                {

                    ApprovedQty = w.ApprovedQty,
                    BarCode = w.BarCode,
                    ProductNa = w.ProductNa,
                    ShippedQty = w.ShippedQty,
                    SystemQty = w.SystemQty,
                    VendorUnitPrice = w.VendorUnitPrice,
                    DocumentNumber = w.DocumentNumber,
                    StoreId = w.StoreId,
                    ExpectedDeliveredDate = w.ExpectedDeliveredDate,
                    MarketUnitPrice = w.MarketUnitPrice,
                    DeliveredQty = w.DeliveredQty,
                    InternalCode = w.InternalCode,
                    CustomerCode = w.CustomerCode,
                    CustomerMasterId = w.CustomerMasterId,
                    RouteName = RouteName  ,
                    RegionName = RegionName  ,
                    TerritoryName = TerritoryName ,
                }).ToList();


                return Json(OrderDetails_Lst_With_Route);
            }
        }


        [HttpPost]
        [AuthorizeShow(PageName = "Receive Purchase Order", TypeButton = TypeButton.Search)]
        public JsonResult GetPriceAndProductAttributeVM(string Barcode)
        {
            var DistributorCode = Session["DistributorCode"] as string;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Qry = (from ProdAtt in DB.Tbl_ProductAttribute
                       from prod in DB.Tbl_Product
                       where prod.ID == ProdAtt.ProductID
                       && ProdAtt.Barcode == Barcode
                       && prod.Vendor_CompanyID == Vendor_CompanyID
                       select new { ProdAtt, prod }).FirstOrDefault();
            if (Qry != null)
            {

                PriceAndProductAttributeVM PriceAndProductAttributeVM = new PriceAndProductAttributeVM()
                {
                    SellPrice = 1,
                    InternalCode = DB.Tbl_ProductVsDistributor.Where(w => w.DistributorCode == DistributorCode && w.Barcode == Barcode && w.Vendor_CompanyID == Vendor_CompanyID).Select(w => w.InternalCode).FirstOrDefault()
                };
                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                string Lang = currentCulture.Name;
                if (PriceAndProductAttributeVM.InternalCode == null) { PriceAndProductAttributeVM.InternalCode = " "; }
                if (Lang == "ar-SA")
                {
                    PriceAndProductAttributeVM.ProductName = Qry.prod.Name;
                }

                else
                {
                    PriceAndProductAttributeVM.ProductName = Qry.prod.NameEng;
                }

                return Json(PriceAndProductAttributeVM, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [AuthorizeShow(PageName = "Receive Purchase Order", TypeButton = TypeButton.Search)]
        public JsonResult GetProducts(int? CategoryId)
        {
            try
            {
                var Qry = (from ProdAtt in DB.Tbl_ProductAttribute
                           from Prod in DB.Tbl_Product
                           where Prod.ID == ProdAtt.ProductID
                           && Prod.IsActive == true
                           && Prod.FirstClassification == CategoryId
                           select new { ProdAtt, Prod });

                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                string Lang = currentCulture.Name;
                if (Lang == "ar-SA")
                {
                    var Products = Qry.Select(w => new Code_NameVM { ID = w.ProdAtt.Barcode, Name = w.Prod.Name + "-" + w.ProdAtt.Barcode }).ToList();
                    return Json(Products, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    var Products = Qry.Select(w => new Code_NameVM { ID = w.ProdAtt.Barcode, Name = w.Prod.NameEng + "-" + w.ProdAtt.Barcode }).ToList();
                    return Json(Products, JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                return Json(2, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Receive Purchase Order", TypeButton = TypeButton.Save)]
        public JsonResult Save(List<PODetailsVM> OrderVM)
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
            var Response = new Response();
            var MastrId = OrderVM[0].MasterID;
            var ExistOrderMstr = DB.OrderManagementMasters.Where(w => w.ID == MastrId).FirstOrDefault();

            #region Reject The PO
            if (OrderVM[0].IsRejected == true)
            {
                Response = RejectedPo(ExistOrderMstr, Vendor_CompanyID);
                if (Response.StatusID == 1)
                {
                    ExistOrderMstr.ShippedDate = DateTime.Now;
                    DB.SaveChanges();
                    return Json("Done");
                }
                else
                {
                    return Json("Error While Rejecting   The PO : " + Response.StatusMsg);
                }
            }
            #endregion
            DateTime? ExpectedDeliveryDate = null;
            if (OrderVM[0].ExpectedDeliveredDate != "")
            {
                try
                { ExpectedDeliveryDate = DateTime.ParseExact(OrderVM[0].ExpectedDeliveredDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); }
                catch { }
            }
            ExistOrderMstr.ExpectedDeliveryDate = ExpectedDeliveryDate;
            var DistributorCode = DB.OrderManagementMasters.Where(i => i.ID == MastrId).Select(m => m.PartnerCode).FirstOrDefault();
            foreach (var item in OrderVM)
            {
                var internalCode = DB.Tbl_ProductVsDistributor.Where(d => d.DistributorCode == DistributorCode && d.Barcode == item.BarCode).Select(i => i.InternalCode).FirstOrDefault();
                
                var ExistObj = DB.OrderManagementDetails.Where(w => w.MasterID == MastrId && w.Barcode == item.BarCode).FirstOrDefault();
                if (ExistObj != null) { ExistObj.VendorUnitPrice = item.VendorUnitPrice; ExistObj.ShippedQty = item.ShippedQty; ExistObj.InternalCode = internalCode; }
                else
                {
                    item.InternalCode = internalCode;
                    OrderManagementDetail NewObj = FillNewOrderDetails(item);
                     
                    DB.OrderManagementDetails.Add(NewObj);
                }
                DB.SaveChanges();
            }


            if (OrderVM[0].IsShipping == true)
            {
                Response = ShippPo(ExistOrderMstr, DB.OrderManagementDetails.Where(w => w.MasterID == MastrId).ToList(), Vendor_CompanyID);
                if (Response.StatusID == 1)
                {
                    Session["OrderManagID"] = MastrId;
                    DB.SaveChanges();
                    return Json("Done");
                }
                else
                {
                    return Json("Error While Shipping  The PO : " + Response.StatusMsg);
                }
            }

            return Json("Done");
        }

        private Response ShippPo(OrderManagementMaster ExistOrderMstr, List<OrderManagementDetail> OrderDtl, string Vendor_CompanyID)
        {
            ExistOrderMstr.Status = 4; // shipped
            ExistOrderMstr.ShippedDate = DateTime.Now;
            try
            {
                string URL = DB.Tbl_CustomerMaster.Where(w => w.CompanyID == ExistOrderMstr.CompanyID && w.StoreID == ExistOrderMstr.StoreId).FirstOrDefault().URL;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Response = client.PostAsJsonAsync(
                       "DirectPo/ShippPo", new
                       {
                           PoMst = ExistOrderMstr,
                           PoDtlLst = OrderDtl,
                           Password = Password
                       }).Result;

                return Response.Content.ReadAsAsync<Response>().Result;

            }
            catch (Exception ex)
            {
                return new Response() { StatusID = -1, StatusMsg = "ShipPO Function" + ErrorUnit.RetriveExceptionMsg(ex) };
            }
        }

        private OrderManagementDetail FillNewOrderDetails(PODetailsVM item)
        {
            return new OrderManagementDetail()
            {
                ShippedQty = item.ShippedQty,
                IsActive = true,
                ActualRecivedQty = 0,
                ApprovedQty = 0,
                Barcode = item.BarCode,
                BonusQTY = 0,
                CommericalDiscount = 0,
                DifferancePercent = 0,
                DifferanceValue = 0,
                Discount = 0,
                ExpiryDate = null,
                IsFree = false,
                JomlaDiscount = 0,
                Lastupdate = DateTime.Now,
                MarketUnitPrice = item.VendorUnitPrice,
                VendorUnitPrice = item.VendorUnitPrice,
                MasterID = item.MasterID,
                ReturnQty = 0,
                SellPrice = 0,
                SubTotal = 0,
                SystemQty = 0,
                Taxes = 0,
                TaxValue = 0,
                Total = 0,
                TotalAfterDiscount = 0,
                TotalAfterTax = 0,
                TotalBeforeDiscount = 0,
                InternalCode = item.InternalCode
            };
        }

        private Response RejectedPo(OrderManagementMaster ExistOrderMstr, string Vendor_CompanyID)
        {
            ExistOrderMstr.Status = 6;
            try
            {
                string URL = DB.Tbl_CustomerMaster.Where(w => w.CompanyID == ExistOrderMstr.CompanyID && w.StoreID == ExistOrderMstr.StoreId).FirstOrDefault().URL;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Response = client.PostAsJsonAsync(
                       "DirectPo/RejectPo", new
                       {
                           DocumnetNumber = ExistOrderMstr.DocumentNumber,
                           Password = Password,
                           CompanyID = ExistOrderMstr.CompanyID
                       }).Result;
                return Response.Content.ReadAsAsync<Response>().Result;
            }
            catch (Exception ex)
            {
                return new Response() { StatusID = -1, StatusMsg = "Reject Po Function" + ErrorUnit.RetriveExceptionMsg(ex) };
            }
        }


        public ActionResult PrintShippingOrder()
        {
            decimal? MasterID = Session["OrderManagID"] as decimal?;
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            string ReportName = "ShippedOrderAr.rdlc";
            if (Lang != "ar-SA")
            {
                ReportName = "ShippedOrderEng.rdlc";
            }
            return Print(MasterID.Value, "ShippedOrder", false, ReportName);
        }
        //sameh code
        public ActionResult PrintReceivedOrder(decimal MasterID)
        { 
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            string ReportName = "ReceivedOrderAr.rdlc";
            if (Lang != "ar-SA")
            {
                ReportName = "ReceivedOrderEng.rdlc";
            }
            return Print(MasterID, "ReceivedOrder", false, ReportName);
        }
        
        public ActionResult PrintShippedOrder(decimal MasterID)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            string ReportName = "ShippedOrderAr.rdlc";
            if (Lang != "ar-SA")
            {
                ReportName = "ShippedOrderEng.rdlc";
            }
            return Print(MasterID, "ShippedOrder", false, ReportName);
        }

        public ActionResult PrintDelivereddOrder(decimal MasterID)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            string ReportName = "DeliveredOrderAr.rdlc";
            if (Lang != "ar-SA")
            {
                ReportName = "DeliveredOrderEng.rdlc";
            }
            return Print(MasterID, "DeliveredOrder", true, ReportName);
        }

        public ActionResult Print(decimal MasterID, string FileName, bool IsDeliveredReport, string ReportName)
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
            var MasterData = DB.Fun_GetOrderInvoiceMasterData(MasterID, Vendor_CompanyID).FirstOrDefault();
            var Qry = DB.Fun_PrintShippedOrder(MasterData.ID, Vendor_CompanyID).ToList();



            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var InvoiceDataDetails = new List<OrdeRManagementRptVM>();
            if (Lang == "ar-SA")
            {
                InvoiceDataDetails = Qry.Select(w => new OrdeRManagementRptVM()
                {
                    BarCode = w.BarCode,
                    InternalCode = w.InternalCode,
                    ApprovedQty = w.ApprovedQty,
                    DeliveredQty = w.DeliveredQty,
                    DeliveredSubTotal = w.DeliveredSubTotal,
                    MarketPrice = w.MarketUnitPrice,
                    RepQty = w.RepQty,
                    ShippedQty = w.ShippedQty,
                    ShippedSubTotal = w.ShippedSubTotal,
                    VendorPrice = w.VendorUnitPrice,
                    ProductName = w.ProductName
                }).ToList();
            }
            else
            {
                InvoiceDataDetails = Qry.Select(w => new OrdeRManagementRptVM()
                {
                    BarCode = w.BarCode,
                    InternalCode = w.InternalCode,
                    ApprovedQty = w.ApprovedQty,
                    DeliveredQty = w.DeliveredQty,
                    DeliveredSubTotal = w.DeliveredSubTotal,
                    MarketPrice = w.MarketUnitPrice,
                    RepQty = w.RepQty,
                    ShippedQty = w.ShippedQty,
                    ShippedSubTotal = w.ShippedSubTotal,
                    VendorPrice = w.VendorUnitPrice,
                    ProductName = w.ProductNameEng
                }).ToList();
            }
           
            LocalReport LR = new LocalReport();
            string U = GetImageURL();
            LR.EnableExternalImages = true;
            string CustomerName = "";
            string VendorName = "";
            string DocumnetNumber = MasterData.DocumentNumber;

            //sameh code
            string Date = "";
            if (MasterData.CreationDate.HasValue)
            {
                Date = MasterData.CreationDate.Value.Date.ToString("yyyy-MM-dd");
            }
            string ExpectedDeliveryDate = "";
            if (MasterData.ExpectedDeliveryDate.HasValue)
            {
                ExpectedDeliveryDate= MasterData.ExpectedDeliveryDate.Value.Date.ToString("yyyy-MM-dd");
            }
            string RoutName = "";
            string CustomerCode = MasterData.CustoemrCode;
            string RegionName = "";
            string TerritoryName = "";
            LR.ReportPath = Server.MapPath("~/Reports/" + ReportName + "");

            if (Lang == "ar-SA")
            {
                CustomerName = MasterData.StoreName;
                VendorName = MasterData.VendorNameAr;
                RoutName = MasterData.RoutName;
                RegionName = MasterData.RegionName;
                TerritoryName = MasterData.TerritoryName;
            }
            else
            {
                CustomerName = MasterData.StoreNameEng;
                VendorName = MasterData.VendorNameEng;
                RoutName = MasterData.RoutNameEng;
                RegionName = MasterData.RegionNameEng;
                TerritoryName = MasterData.TerritoryNameEng;
            }

            ReportParameter[] param = new ReportParameter[9];
            param[0] = new ReportParameter("CustomerName", CustomerName, false);
            param[1] = new ReportParameter("VendorName", VendorName, false);
            param[2] = new ReportParameter("DocumnetNumber", DocumnetNumber, false);
            param[3] = new ReportParameter("Date", Date, false);
            param[4] = new ReportParameter("RoutName", RoutName, false);
            param[5] = new ReportParameter("CustomerCode", CustomerCode, false);
            param[6] = new ReportParameter("RegionName", RegionName, false);
            param[7] = new ReportParameter("TerritoryName", TerritoryName, false);
            param[8] = new ReportParameter("ImageUrl", U, false);
            param[8] = new ReportParameter("Date", Date, false);
            param[8] = new ReportParameter("ExpectedDeliveryDate", ExpectedDeliveryDate, false);
            LR.SetParameters(param);
            string ExtraFileName = FileManager.MakeValidFileName("_" + CustomerCode + "_" + CustomerName);
            ReportDataSource RDS = new ReportDataSource();
            //dataset name
            RDS.Name = "DataSet1";
            //session name
            RDS.Value = InvoiceDataDetails;
            //assign report data source
            LR.DataSources.Add(RDS);
            //
            string FileNameExtension;
            FileNameExtension = "pdf";
            string[] streams;
            Warning[] Warning;
            byte[] renderByte;
            string mimetype;
            string encoding;
            string deviceInfo = "<DeviceInfo>" +
             "  <OutputFormat>" + "Excel" + "</OutputFormat>" +
             "  <PageWidth>28cm</PageWidth>" +
             "  <PageHeight>29cm</PageHeight>" +
             "  <MarginTop>0.2cm</MarginTop>" +
             "  <MarginLeft>0.0cm</MarginLeft>" +
             "  <MarginRight>0.0cm</MarginRight>" +
             "  <MarginBottom>0.1cm</MarginBottom>" +
             "</DeviceInfo>";
            //
            renderByte = LR.Render(FileNameExtension, deviceInfo, out mimetype, out encoding, out FileNameExtension, out streams, out Warning);
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName+ ExtraFileName + "." + "pdf");
            return File(renderByte, FileNameExtension);

        }

        private string GetImageURL()
        {
            string Url = Session["LogURL"] as string;
            string U = Server.MapPath(Url);
            U = new Uri(U).AbsoluteUri;
            return U;
        }

    }
}