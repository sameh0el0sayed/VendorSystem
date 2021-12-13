using System;
using System.Collections.Generic;
using System.Linq;
using VendorSystem.Models.Model1;
using VendorSystem.ViewModel;

namespace VendorSystem.Repository
{
    public class LockUpUnit
    {
        BayanEntities DB;
        public LockUpUnit(BayanEntities _Db)
        {
            DB = _Db;
        }

        public LockUpUnit()
        {
            DB = new BayanEntities();
        }


        public IQueryable<Tbl_CentralizedLockUp> GetAllCountries()
        {
            return GetChildrenByParentIDFromCentralized(1);
        }

        public IQueryable<LockUp> GetAllFirstClassification(string Vendor_CompanyID)
        {
            return GetChildrenByParentID(1, Vendor_CompanyID);
        }

        public IQueryable<LockUp> GetAllStoreType(string Vendor_CompanyID)
        {
            return GetChildrenByParentID(2, Vendor_CompanyID);
        }

        public IQueryable<LockUp> GetAllStoreCategory(string Vendor_CompanyID)
        {
            return GetChildrenByParentID(3, Vendor_CompanyID);
        }

        public IQueryable<LockUp> GetAllStoreClassification(string Vendor_CompanyID)
        {
            return GetChildrenByParentID(4, Vendor_CompanyID);
        }

        public IQueryable<LockUp> GetAllCurrencies(string Vendor_CompanyID)
        {
            return GetChildrenByParentID(5, Vendor_CompanyID);
        }

        public IQueryable<LockUp> GetAllCards(string Vendor_CompanyID)
        {
            return GetChildrenByParentID(6, Vendor_CompanyID);
        }

        public IQueryable<LockUp> GetChildrenByParentID(int ParentID, string Vendor_CompanyID)
        {
            return DB.LockUps.Where(w => w.Active == true && w.ParentId == ParentID && (w.Vendor_CompanyID == "cnt" || w.Vendor_CompanyID == Vendor_CompanyID));
        }

        public IQueryable<Tbl_CentralizedLockUp> GetChildrenByParentIDFromCentralized(int ParentID)
        {
            return DB.Tbl_CentralizedLockUp.Where(w => w.IsActive == true && w.ParentId == ParentID);
        }


        public IQueryable<LockUp> GetChildrenByParentLst(List<int?> ParentIdLst, string Vendor_CompanyID)
        {
            return DB.LockUps.Where(w => w.Active == true && ParentIdLst.Contains(w.ParentId) && (w.Vendor_CompanyID == "cnt" || w.Vendor_CompanyID == Vendor_CompanyID));
        }

        public IQueryable<Tbl_CentralizedLockUp> GetChildrenByParentIdLstFromCentralized(List<int?> ParentIdLst)
        {
            return DB.Tbl_CentralizedLockUp.Where(w => w.IsActive == true && ParentIdLst.Contains(w.ParentId));
        }

        public IQueryable<LockUp> GetAllLockup(string Vendor_CompanyID)
        {
            return DB.LockUps.Where(w => w.Vendor_CompanyID == "cnt" || w.Vendor_CompanyID == Vendor_CompanyID);
        }

        public IQueryable<LockUp> GetAllActiveParentRefrences()
        {
            return DB.LockUps.Where(w => w.Active == true && w.ParentId == null);
        }

        public IQueryable<LockUp> GetAllActiveRefrences(string Vendor_CompanyID)
        {
            return DB.LockUps.Where(w => (w.ParentId == null || w.Vendor_CompanyID == Vendor_CompanyID) && w.Active == true);
        }

        public IQueryable<V_GetRegionByRoutes> GetRegionByRoutes(string Vendor_CompanyID)
        {
            return DB.V_GetRegionByRoutes.Where(w => w.Vendor_CompanyID == Vendor_CompanyID);
        }

        public LockUp GetLockUpByID(int id)
        {
            return DB.LockUps.Where(w => w.ID == id).FirstOrDefault();
        }

        public string Remove(int _ID)
        {
            try
            {
                var ExistChildrn = DB.LockUps.Where(w => w.ParentId == _ID).Count();
                if (ExistChildrn > 0)
                {
                    return "Exist";
                }

                var OldObj = DB.LockUps.Where(w => w.ID == _ID).FirstOrDefault();
                if (OldObj.ParentId == null)
                {
                    return CheckUnit.RetriveCorrectMsg("عفوا لا يمكن حذف هذا المرجع", "Sorry, You couldn't Delete  this reference!");
                }
                DB.LockUps.Remove(OldObj);
                DB.SaveChanges();

                return "Done";
            }
            catch (Exception ex)
            {
                return ErrorUnit.RetriveExceptionMsg(ex);
            }
        }

        public string Save(TreeViewNode selectedItems, string Vendor_CompanyID)
        {
            int _ID = int.Parse(selectedItems.id);

            var ExistObj = DB.LockUps.Where(w => w.ID == _ID && selectedItems.IsEdit == true).FirstOrDefault();
            if (ExistObj != null && ExistObj.ParentId == null)
            {
                return CheckUnit.RetriveCorrectMsg("عفوا لا يمكن التعديل على هذا المرجع", "Sorry, You couldn't edit on this reference!");
            }

            int? ParentID = null;
            try
            {
                ParentID = int.Parse(selectedItems.parent);
            }
            catch
            {
                return CheckUnit.RetriveCorrectMsg("عفوا لابد من اختيارالمرجع الاساسي", "Sorry, You must choose basic Reference");
            }


            var OldNameArLst = DB.LockUps.Where(w => w.Name == selectedItems.text && w.Vendor_CompanyID == Vendor_CompanyID).ToList();
            if (OldNameArLst.Count > 1 || (OldNameArLst.Count == 1 && OldNameArLst[0].ID != _ID) || (OldNameArLst.Count == 1 && selectedItems.IsEdit == false))
            {
                return "ErrorArabicName";
            }


            var OldNameEnLst = DB.LockUps.Where(w => w.NameEng == selectedItems.textEn && w.Vendor_CompanyID == Vendor_CompanyID).ToList();
            if (OldNameEnLst.Count > 1 || (OldNameEnLst.Count == 1 && OldNameEnLst[0].ID != _ID) || (OldNameEnLst.Count == 1 && selectedItems.IsEdit == false))
            {
                return "ErrorEngName";
            }

            try
            {
                if (selectedItems.IsEdit == false)
                {
                    LockUp LockUp = new LockUp()
                    {
                        NameEng = selectedItems.textEn,
                        Active = selectedItems.Active,
                        CreatedDate = DateTime.Now,
                        Lastupdate = DateTime.Now,
                        Name = selectedItems.text,
                        ParentId = _ID,
                        Vendor_CompanyID = Vendor_CompanyID
                    };
                    DB.LockUps.Add(LockUp);
                    DB.SaveChanges();
                }
                else
                {
                    int TempParent = ParentID.Value;
                    bool IsValid = true;

                    var OldParent = DB.LockUps.Where(w => w.ID == TempParent).FirstOrDefault();
                    while (OldParent != null && OldParent.ParentId != null)
                    {
                        TempParent = OldParent.ParentId.Value;
                        if (TempParent == _ID)
                        {
                            IsValid = false;
                            break;
                        }
                        OldParent = DB.LockUps.Where(w => w.ID == TempParent).FirstOrDefault();
                    }
                    if (IsValid == false || ParentID == _ID)
                    {
                        return CheckUnit.RetriveCorrectMsg("عفوا لابد من اختيار مرجع رئيسى مختلف", "Sorry, You must Choose a Differnet basic Reference!");
                    }

                    var OldObj = DB.LockUps.Where(w => w.ID == _ID).FirstOrDefault();
                    OldObj.NameEng = selectedItems.textEn;
                    OldObj.Active = selectedItems.Active;
                    OldObj.CreatedDate = DateTime.Now;
                    OldObj.Lastupdate = DateTime.Now;
                    OldObj.Name = selectedItems.text;
                    OldObj.ParentId = ParentID;
                    DB.SaveChanges();
                }

                return "Done";
            }
            catch (Exception ex)
            {
                return ErrorUnit.RetriveExceptionMsg(ex);
            }
        }
    }
}