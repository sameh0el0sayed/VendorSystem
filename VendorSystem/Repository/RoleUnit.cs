using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendorSystem.Models;
using VendorSystem.Models.Model1;
using VendorSystem.ViewModel;

namespace VendorSystem.Repository
{
    public class RoleUnit
    {
        BayanEntities DB;
        public RoleUnit()
        {
            DB = new BayanEntities();
        }
        public RoleUnit(BayanEntities _DB)
        {
            DB = _DB;
        }

        public IQueryable<Role> GetAllRoles(string Vendor_CompanyID)
        {
            return DB.Roles.Where(w => w.Vendor_CompanyID == Vendor_CompanyID);
        }

        public IQueryable<Role> GetAllActiveRoles(string Vendor_CompanyID)
        {
            return GetAllRoles(Vendor_CompanyID).Where(w => w.IsActive == true);
        }

        public List<RoleVM> GetAllRolesVM(string Vendor_CompanyID)
        {
            var Qry = GetAllRoles(Vendor_CompanyID).Select(w => new RoleVM
            {
                IsActive = w.IsActive,
                Name = w.Name,
                NameEng = w.NameEng,
                ID = w.ID
            });

            return Qry.ToList();
        }

        public string Save(RoleVM RoleVM, int UserID, string Vendor_CompanyID)
        {
            try
            {
                var CheckArNameAndEngName = DB.Roles.Where(w => w.ID != RoleVM.ID && w.Vendor_CompanyID == Vendor_CompanyID && (w.Name == RoleVM.Name && w.NameEng == RoleVM.NameEng)).FirstOrDefault();
                if (CheckArNameAndEngName != null)
                {
                    return CheckUnit.RetriveCorrectMsg("الاسم العربى او الاسم الانجليزى موجود من قبل ", "Arabic Name Or English Name Is Exist Befor");
                }
                var OldObj = DB.Roles.Where(w => w.ID == RoleVM.ID).FirstOrDefault();

                if (OldObj == null)
                {
                    var NewRole = new Role()
                    {
                        CreationDate = DateTime.Now,
                        CreatedBy = UserID,
                        Vendor_CompanyID = Vendor_CompanyID
                    };
                    FillRole(RoleVM, NewRole, UserID);
                    DB.Roles.Add(NewRole);
                }
                else
                {
                    FillRole(RoleVM, OldObj, UserID);
                }
                DB.SaveChanges();
                return "Done";
            }
            catch (Exception ex)
            {
                return ErrorUnit.RetriveExceptionMsg(ex);
            }
        }

        private void FillRole(RoleVM RoleVM, Role Role, int UserID)
        {
            Role.IsActive = RoleVM.IsActive;
            Role.LastUpdate = DateTime.Now;
            Role.Name = RoleVM.Name;
            Role.NameEng = RoleVM.NameEng;
            Role.UpdateBy = UserID;
        }


    }
}