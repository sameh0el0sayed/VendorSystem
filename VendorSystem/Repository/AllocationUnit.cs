using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendorSystem.Models;
using VendorSystem.Models.Model1;
using VendorSystem.ViewModel;

namespace VendorSystem.Repository
{
    public class AllocationUnit
    {
        BayanEntities DB;
        public AllocationUnit(BayanEntities _Db)
        {
            DB = _Db;
        }
        public AllocationUnit()
        {
            DB = new BayanEntities();
        }

        public IQueryable<Fun_GetAllocationData_Result> GetAllocationData(int RegionID, int? TerritoryID, int? RouteID, DateTime ExpectedDeliveryDate, string Vendor_CompanyID)
        {
            return DB.Fun_GetAllocationData(RegionID, TerritoryID, RouteID, ExpectedDeliveryDate, Vendor_CompanyID);
        }

        public string Save(int RegionID, int? TerritoryID, int? RouteID, DateTime ExpectedDeliveryDate, string Vendor_CompanyID, List<AllocationVM> AllocationVMLst)
        {
            using (var contxt = new BayanEntities())
            {
                using (var db_contextTransaction = contxt.Database.BeginTransaction())
                {
                    try
                    {
                        if (AllocationVMLst == null)
                            return "Done";

                        foreach (var item in AllocationVMLst)
                        {
                            contxt.Proc_ReAllocateQty(RegionID, TerritoryID, RouteID, ExpectedDeliveryDate, Vendor_CompanyID, item.InternalCode, item.Barcode, item.ShippedQty, item.TotalNeededQty);
                            contxt.SaveChanges();
                        }
                        db_contextTransaction.Commit();
                        return "Done";

                    }
                    catch (Exception ex)
                    {
                        db_contextTransaction.Rollback();
                        return ErrorUnit.RetriveExceptionMsg(ex);
                    }
                }
            }
        }
    }
}