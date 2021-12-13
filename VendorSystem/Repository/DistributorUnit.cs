using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendorSystem.Models;
using VendorSystem.Models.Model1;
using VendorSystem.ViewModel;

namespace VendorSystem.Repository
{
    public class DistributorUnit
    {
        BayanEntities DB;
        public DistributorUnit(BayanEntities _Db)
        {
            DB = _Db;
        }
        public DistributorUnit()
        {
            DB = new BayanEntities();
        }

        public IQueryable<Tbl_Distributor> GetAllDistributorS(string Vendor_CompanyID)
        {
            return DB.Tbl_Distributor.Where(w => w.Vendor_CompanyID == Vendor_CompanyID);
        }

        public IQueryable<Tbl_Distributor> GetAllActiveDistributors(string Vendor_CompanyID)
        {
            return DB.Tbl_Distributor.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.IsActive == true);
        }

        public DistributorVM GitDistriutorsByCode(string Vendor_CompanyID, string Code)
        {
            var Distributor = DB.Tbl_Distributor.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.Code == Code).FirstOrDefault();

            var Rslt = new DistributorVM()
            {
                Code = Distributor.Code,
                Address = Distributor.Address,
                CityID = Distributor.CityID,
                CountryID = Distributor.CountryID,
                IsActive = Distributor.IsActive,
                Mobile = Distributor.Mobile,
                Name = Distributor.Name,
                NameEng = Distributor.NameEng,
                ProvinceID = Distributor.ProvinceID,
                RegionID = Distributor.RegionID,
                TerritoryID = Distributor.TerritoryID
            };

            Rslt.RoutesCount = DB.Tbl_RouteCombinMaster.Where(w => w.DistributorID == Distributor.ID && w.IsActive == true).Count();

            return Rslt;
        }

        public List<DistributorVM> GetAllDistributorsVM(string Vendor_CompanyID)
        {
            var Qry = DB.V_GetAllDistributorsVM.Where(w => w.Vendor_CompanyID == Vendor_CompanyID).Select(w =>
                        new DistributorVM
                        {
                            Address = w.Address,
                            CityName = w.CityName,
                            Name = w.Name,
                            CityNameEng = w.CityName,
                            Code = w.Code,
                            IsActive = w.IsActive,
                            NameEng = w.NameEng,
                            Mobile = w.Mobile,
                            RoutesCount = w.RoutCount
                        });

            return Qry.ToList();
        }

        public string Save(DistributorVM DistributorVM, int UserID, string Vendor_CompanyID)
        {
            try
            {
                var OldDistributor = DB.Tbl_Distributor.Where(w => w.Code == DistributorVM.Code && w.Vendor_CompanyID == Vendor_CompanyID).FirstOrDefault();
                if (OldDistributor == null)
                {
                    int MaxCode = 1;
                    var Obj = DB.Tbl_Distributor.OrderByDescending(w => w.ID).FirstOrDefault();
                    if (Obj != null)
                    {
                        MaxCode = Obj.ID + 1;
                    }
                    var NewDistributor = new Tbl_Distributor() { Code = "VED_" + MaxCode.ToString(), ID = MaxCode, CreatedBy = UserID, CreationDate = DateTime.Now, Vendor_CompanyID = Vendor_CompanyID };
                    FillDistibutor(DistributorVM, NewDistributor, UserID);
                    DB.Tbl_Distributor.Add(NewDistributor);
                }
                else
                {
                    FillDistibutor(DistributorVM, OldDistributor, UserID);
                }
                DB.SaveChanges();
                return "Done";
            }
            catch (Exception ex)
            {
                return ErrorUnit.RetriveExceptionMsg(ex);
            }
        }

        private void FillDistibutor(DistributorVM DistributorVM, Tbl_Distributor Distributor, int UserID)
        {
            Distributor.IsActive = DistributorVM.IsActive;
            Distributor.Address = DistributorVM.Address;
            Distributor.CityID = DistributorVM.CityID;
            Distributor.CountryID = DistributorVM.CountryID;
            Distributor.Email = "";
            Distributor.Name = DistributorVM.Name;
            Distributor.NameEng = DistributorVM.NameEng;
            Distributor.Mobile = DistributorVM.Mobile;
            Distributor.ProvinceID = DistributorVM.ProvinceID;
            Distributor.RegionID = DistributorVM.RegionID;
            Distributor.TerritoryID = DistributorVM.TerritoryID;
            Distributor.UpdateBy = UserID;
            Distributor.LastUpdate = DateTime.Now;
        }
    }

}