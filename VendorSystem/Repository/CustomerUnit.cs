using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendorSystem.Models;
using VendorSystem.Models.Model1;
using VendorSystem.ViewModel;

namespace VendorSystem.Repository
{
    public class CustomerUnit
    {
        BayanEntities DB;
        public CustomerUnit(BayanEntities _Db)
        {
            DB = _Db;
        }
        public CustomerUnit()
        {
            DB = new BayanEntities();
        }

        public List<CustomerVM> GetAllCustomersVM(string Vendor_CompanyID)
        {
            var Qry = DB.V_GetAllCustomersVM.Where(w => w.Vendor_CompanyID == Vendor_CompanyID).Select(w => new CustomerVM
            {
                Address = w.Address,
                CityName = w.CityName,
                StoreName = w.StoreName,
                CompanyName = w.CompanyName,
                CityNameEng = w.CityName,
                ID = w.CustID,
                IsActive = w.Active,
                StoreNameEng = w.StoreNameEng,
                CompanyNameEng = w.CompanyNameEng,
                Mobile = w.Mobile,
                CustomerCode = w.CustoemrCode,
            });

            return Qry.ToList();
        }

        public List<CustomerVM> GetAllActiveCustomersVM(string Vendor_CompanyID)
        {
            var Qry = DB.V_GetAllCustomersVM.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.Active == true).Select(w => new CustomerVM
            {
                Address = w.Address,
                CityName = w.CityName,
                StoreName = w.StoreName,
                CompanyName = w.CompanyName,
                CityNameEng = w.CityName,
                ID = w.CustID,
                IsActive = w.Active,
                StoreNameEng = w.StoreNameEng,
                CompanyNameEng = w.CompanyNameEng,
                Mobile = w.Mobile,
                CustomerCode = w.CustoemrCode,
                CityID = w.CityID,
                AssingRoutes = "",
                CommercialRegistration = w.Commercial_Registration,
                CompanyID = w.CompanyID,
                CountryID = w.CountryID,
                LastUpdate = w.LastUpdate,
                Phone = w.Phone,
                ProvinceID = w.ProvinceID,
                RegionID = w.RegionID,
                StoreCategoryID = w.RegionID,
                StoreClassificationID = w.StoreClass,
                StoreID = w.StoreID,
                StoreTypeID = w.StoreType,
                URL = w.URL,
                TerritoryID = w.TerritoryID,
                VatNumber = w.VAT_Registration,
                VisitScheduleID = w.VisitScheduleID
            });

            return Qry.ToList();
        }

        public CustomerVM GetCustomerVM_ByID(int CustDtlID)
        {
            var Qry = DB.V_GetAllCustomersVM.Where(w => w.CustID == CustDtlID).Select(w => new CustomerVM
            {
                Address = w.Address,
                CityID = w.CityID,
                CommercialRegistration = w.Commercial_Registration,
                CompanyName = w.CompanyName,
                CompanyNameEng = w.CompanyNameEng,
                CountryID = w.CountryID,
                ID = w.CustID,
                IsActive = w.Active,
                Mobile = w.Mobile,
                Phone = w.Phone,
                ProvinceID = w.ProvinceID,
                RegionID = w.RegionID,
                StoreCategoryID = w.StoreCategory,
                StoreClassificationID = w.StoreClass,
                StoreName = w.StoreName,
                StoreNameEng = w.StoreNameEng,
                StoreTypeID = w.StoreType,
                TerritoryID = w.TerritoryID,
                VatNumber = w.VAT_Registration,
                VisitScheduleID = w.VisitScheduleID,
                StoreID = w.StoreID,
                CompanyID = w.CompanyID,
                CityName = w.CityName,
                CityNameEng = w.CityNameEng,
                LastUpdate = w.LastUpdate,
                URL = w.URL,
                CustomerCode = w.CustoemrCode
            }).FirstOrDefault();

            var AssignRoutes = DB.Fun_GetAssingRoutesByCustID(CustDtlID).ToList();
            if (AssignRoutes.Count > 0)
            {
                Qry.AssingRoutes = AssignRoutes[0].Name;
                for (int i = 1; i < AssignRoutes.Count; i++)
                {
                    Qry.AssingRoutes += " , " + AssignRoutes[i].Name;
                }
            }
            return Qry;
        }

        public List<CustomerVM> GetCustomersVM_ByRouteID(int RouteID, string Vendor_CompanyID)
        {
            var Rslt = (from RoutMstr in DB.Tbl_RouteCombinMaster
                        from RoutDtlCust in DB.Tbl_RouteCombinDtlCustomer
                        from CustDtl in DB.Tbl_CustomerDtl
                        from w in DB.Tbl_CustomerMaster
                        where RoutMstr.RouteID == RouteID
                        && RoutMstr.IsActive == true
                        && RoutDtlCust.MstrID == RoutMstr.ID
                        && RoutDtlCust.IsActive == true
                        && CustDtl.ID == RoutDtlCust.CustID
                        && CustDtl.Active == true
                        && CustDtl.Vendor_CompanyID == Vendor_CompanyID
                        && CustDtl.CustomerMasterID == w.ID
                        select new CustomerVM()
                        {
                            ID = CustDtl.ID,
                            CompanyName = w.CompanyName,
                            CompanyNameEng = w.CompanyNameEng,
                            CustomerCode = CustDtl.CustoemrCode,
                            StoreName = w.StoreName,
                            StoreNameEng = w.StoreNameEng
                        }
                      ).ToList();

            return Rslt;
        }

        public string Save(CustomerVM CustomerVM, int UserID)
        {
            try
            {
                var OldCustomer = DB.Tbl_CustomerDtl.Where(w => w.ID == CustomerVM.ID).FirstOrDefault();
                FillCustomer(CustomerVM, OldCustomer, UserID);
                DB.SaveChanges();
                return "Done";
            }
            catch (Exception ex)
            {
                return ErrorUnit.RetriveExceptionMsg(ex);
            }
        }

        private void FillCustomer(CustomerVM CustomerVM, Tbl_CustomerDtl Customer, int UserID)
        {
            Customer.VisitScheduleID = CustomerVM.VisitScheduleID;
            Customer.StoreType = CustomerVM.StoreTypeID;
            Customer.StoreCategory = CustomerVM.StoreCategoryID;
            Customer.StoreClass = CustomerVM.StoreClassificationID;
            Customer.Active = CustomerVM.IsActive;
        }
    }

}