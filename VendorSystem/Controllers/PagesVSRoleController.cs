using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendorSystem.Models;
using VendorSystem.ViewModel;
using System.Globalization;
using System.Threading;
using Microsoft.Reporting.WebForms;
using VendorSystem.Authorize;
using VendorSystem.Models.Model1;

namespace VendorSystem.Controllers
{
    public class PagesVSRoleController : MyBaseController
    {
        BayanEntities _context = new BayanEntities();

        [AuthorizeShow(PageName = "Assign Roles", TypeButton = TypeButton.Show)]
        public ActionResult Index()
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
            ViewBag.Roles = new SelectList(_context.Roles.Where(x => x.IsActive == true && x.Vendor_CompanyID == Vendor_CompanyID).Select(w => new { Name = w.Name, ID = w.ID }).ToList(), "ID", "Name");
            ViewBag.AppPages = _context.Pages.Where(x => x.IsActive == true).ToList();
            return View();
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Assign Roles", TypeButton = TypeButton.Save)]
        public JsonResult Save(PagesVSRoleListViewModel PagesVSRoleListViewModel)
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
            var UserID = Session["UserID"] as int?;

            using (var context = new BayanEntities())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var PagesVSRoleViewModel in PagesVSRoleListViewModel.PagesVSRoleViewModelData)
                        {
                            if (PagesVSRoleViewModel.ID == 0)
                            {
                                bool Print = false, search = false, save = false, saveandpost = false, Show = false;
                                if (PagesVSRoleViewModel.Print)
                                {
                                    Print = true;
                                }
                                if (PagesVSRoleViewModel.save)
                                {
                                    save = true;
                                }
                                if (PagesVSRoleViewModel.saveandpost)
                                {
                                    saveandpost = true;
                                }
                                if (PagesVSRoleViewModel.Show)
                                {
                                    Show = true;
                                }
                                if (PagesVSRoleViewModel.search)
                                {
                                    search = true;
                                }
                                PageVSRole PageVSRole = new PageVSRole()
                                {
                                    PageId = PagesVSRoleViewModel.PageId,
                                    Print = Print,
                                    RoleId = PagesVSRoleViewModel.RoleId,
                                    Save = save,
                                    SaveAndPost = saveandpost,
                                    Search = search,
                                    Show = Show,
                                    Active = true,
                                    UpdateBy = UserID,
                                    LastUpdate = DateTime.Now,
                                    Vendor_CompanyID = Vendor_CompanyID
                                };
                                PageVSRole = context.PageVSRoles.Add(PageVSRole);
                                context.SaveChanges();
                            }
                            else
                            {
                                var PagesVSRole = context.PageVSRoles.Find(PagesVSRoleViewModel.ID);
                                PagesVSRole.PageId = PagesVSRoleViewModel.PageId;
                                PagesVSRole.Print = PagesVSRoleViewModel.Print;
                                PagesVSRole.RoleId = PagesVSRoleViewModel.RoleId;
                                PagesVSRole.Save = PagesVSRoleViewModel.save;
                                PagesVSRole.SaveAndPost = PagesVSRoleViewModel.saveandpost;
                                PagesVSRole.Search = PagesVSRoleViewModel.search;
                                PagesVSRole.Show = PagesVSRoleViewModel.Show;
                                PagesVSRole.Active = true;
                                PagesVSRole.UpdateBy = UserID;
                                PagesVSRole.LastUpdate = DateTime.Now;
                                context.SaveChanges();
                            }
                        }
                        dbContextTransaction.Commit();
                        return Json(1);
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                        return Json(2);
                    }
                }
            }
        }

        [HttpPost]
        [AuthorizeShow(PageName = "Assign Roles", TypeButton = TypeButton.Search)]
        public JsonResult GetPageVsRoleForEdit(int RoleId)
        {
            var Vendor_CompanyID = Session["Vendor_CompanyID"] as string;
            var AssignRole = _context.Proc_GetPageVsRole(RoleId, Vendor_CompanyID).ToList();
            return Json(AssignRole);
        }
    }
}

