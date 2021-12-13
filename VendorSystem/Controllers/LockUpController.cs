using VendorSystem.Authorize;
using VendorSystem.Models;
using VendorSystem.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VendorSystem.Repository;

namespace VendorSystem.Controllers
{
    public class LockUpController : MyBaseController
    {
        LockUpUnit LockUpUnit = new LockUpUnit();


        [AuthorizeShow(PageName = "LockUp", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;

            var LockUp = LockUpUnit.GetAllLockup(Vendor_CompanyID).ToList();
            List<TreeViewNode> nodes = new List<TreeViewNode>();

            foreach (var type in LockUp)
            {
                if (Lang == "ar-SA")
                {
                    if (type.ParentId == null)
                        nodes.Add(new TreeViewNode { id = type.ID.ToString(), parent = "#", text = type.Name, textEn = type.NameEng });
                    else
                        nodes.Add(new TreeViewNode { id = /*type.ParentId + "-" +*/type.ID.ToString(), parent = type.ParentId.ToString(), text = type.Name, textEn = type.NameEng });
                }
                else
                {
                    if (type.ParentId == null)
                        nodes.Add(new TreeViewNode { id = type.ID.ToString(), parent = "#", text = type.NameEng, textEn = type.NameEng });
                    else
                        nodes.Add(new TreeViewNode { id = /*type.ParentId + "-" +*/type.ID.ToString(), parent = type.ParentId.ToString(), text = type.NameEng, textEn = type.NameEng });
                }

            }

            if (Lang == "ar-SA")
            {
                ViewBag.Refrences = new SelectList(LockUpUnit.GetAllActiveParentRefrences().Select(w => new { ID = w.ID, Name = w.Name }), "ID", "Name");
                ViewBag.AllRefrences = new SelectList(LockUpUnit.GetAllActiveRefrences(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }), "ID", "Name");
            }
            else
            {
                ViewBag.Refrences = new SelectList(LockUpUnit.GetAllActiveParentRefrences().Select(w => new { ID = w.ID, Name = w.NameEng }), "ID", "Name");
                ViewBag.AllRefrences = new SelectList(LockUpUnit.GetAllActiveRefrences(Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }), "ID", "Name");

            }


            //Serialize to JSON string.
            ViewBag.Json = nodes;// (new JavaScriptSerializer()).Serialize(nodes);
            return View();
        }


        [HttpPost]
        [AuthorizeShow(PageName = "LockUp", TypeButton = TypeButton.Save)]
        public JsonResult Save(TreeViewNode selectedItems)
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            var Rslt = LockUpUnit.Save(selectedItems, Vendor_CompanyID);
            return Json(Rslt);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "LockUp", TypeButton = TypeButton.Save)]
        public JsonResult Remove(int _ID)
        {
            var Rslt = LockUpUnit.Remove(_ID);
            return Json(Rslt);
        }

        [HttpPost]
        [AuthorizeShow(PageName = "LockUp", TypeButton = TypeButton.Search)]
        public JsonResult GetLockUp(int id)
        {
            var Ref = LockUpUnit.GetLockUpByID(id);
            TreeViewNode TreeViewNode = new TreeViewNode();
            TreeViewNode.text = Ref.Name;
            TreeViewNode.textEn = Ref.NameEng;
            TreeViewNode.parent = Ref.ParentId.HasValue ? Ref.ParentId.ToString() : "";
            TreeViewNode.Active = Ref.Active;
            return Json(TreeViewNode);
        }


        [HttpPost]
        [AuthorizeShow(PageName = "Home", TypeButton = TypeButton.Search)]
        public JsonResult GetChildrenDataByParentID(int ParentID, bool? IsFromCentralizedLookUp)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            if (Lang == "ar-SA")
            {

                if (IsFromCentralizedLookUp == true)
                {
                    return Json(LockUpUnit.GetChildrenByParentIDFromCentralized(ParentID).Select(w => new { ID = w.ID, Name = w.Name }).ToList());
                }
                else
                {
                    return Json(LockUpUnit.GetChildrenByParentID(ParentID, Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList());
                }
            }
            else
            {
                if (IsFromCentralizedLookUp == true)
                {
                    return Json(LockUpUnit.GetChildrenByParentIDFromCentralized(ParentID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList());
                }
                else
                {
                    return Json(LockUpUnit.GetChildrenByParentID(ParentID, Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList());
                }
            }
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Home", TypeButton = TypeButton.Search)]
        public JsonResult GetChildrensbyPaentLst(List<int?> ParentIdLst, bool? IsFromCentralizedLookUp)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;

            if (Lang == "ar-SA")
            {

                if (IsFromCentralizedLookUp == true)
                {
                    return Json(LockUpUnit.GetChildrenByParentIdLstFromCentralized(ParentIdLst).Select(w => new { ID = w.ID, Name = w.Name }).ToList());
                }
                else
                {
                    return Json(LockUpUnit.GetChildrenByParentLst(ParentIdLst, Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.Name }).ToList());
                }
            }
            else
            {
                if (IsFromCentralizedLookUp == true)
                {
                    return Json(LockUpUnit.GetChildrenByParentIdLstFromCentralized(ParentIdLst).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList());
                }
                else
                {
                    return Json(LockUpUnit.GetChildrenByParentLst(ParentIdLst, Vendor_CompanyID).Select(w => new { ID = w.ID, Name = w.NameEng }).ToList());
                }
            }
        }
    }
}