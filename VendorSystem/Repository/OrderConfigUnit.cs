using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using VendorSystem.Models.Model1;
using VendorSystem.ViewModel;

namespace VendorSystem.Repository
{

    public class OrderConfigUnit
    {
        BayanEntities DB;

        public OrderConfigUnit()
        {
            DB = new BayanEntities();
        }

        public IQueryable<Tbl_ScheduleType> GetAllScheduleType()
        {
            return DB.Tbl_ScheduleType;
        }

        public IQueryable<Tbl_WeekDay> GetAllWeekDays()
        {
            return DB.Tbl_WeekDay;
        }

        public List<OrderConfigVM> GetAllOrderConfig(string Vendor_CompanyID)
        {
            return DB.Tbl_OrdConfig_Mstr.Where(m => m.Vendor_CompanyID == Vendor_CompanyID).Select(m => new OrderConfigVM { ID = m.ID, Name = m.Name, NameEng = m.NameEng, IsActive = m.IsActive }).ToList();
        }

        public List<Tbl_OrdConfig_Mstr> GetOrderConfigByID(string Vendor_CompanyID, decimal OrdConfigID)
        {
            return DB.Tbl_OrdConfig_Mstr.Where(m => m.Vendor_CompanyID == Vendor_CompanyID && m.ID == OrdConfigID).ToList();
        }
        public List<int> GetOrderConfigCustomersByID(decimal OrdConfigID)
        {
            return DB.Tbl_OrdConfigCustomer.Where(m => m.OrdConfig_Mstr_ID == OrdConfigID).Select(m => m.CustomerDtl_ID).ToList();
        }

        public string Save(OrderConfigVM VM, int UserID, string Vendor_CompanyID)
        {
            try
            {

                var OldOrderConfigMstr = DB.Tbl_OrdConfig_Mstr.Where(w => w.ID == VM.ID).FirstOrDefault();
                if (OldOrderConfigMstr == null)
                {
                    #region check of arabic name 
                    var ExistObj = DB.Tbl_OrdConfig_Mstr.Where(w => w.ID != VM.ID && w.Name == VM.Name && w.Vendor_CompanyID == Vendor_CompanyID && w.IsActive == true).FirstOrDefault();
                    if (ExistObj != null)
                    {
                        return CheckUnit.RetriveCorrectMsg("الاسم موجود من قبل", " Name is already exist before");
                    }
                    #endregion

                    #region check of English name 
                    ExistObj = DB.Tbl_OrdConfig_Mstr.Where(w => w.ID != VM.ID && w.NameEng == VM.NameEng && w.Vendor_CompanyID == Vendor_CompanyID && w.IsActive == true).FirstOrDefault();
                    if (ExistObj != null)
                    {
                        return CheckUnit.RetriveCorrectMsg("الاسم باللغة الانجليزية موجود من قبل", "English Name is already exist before");
                    }
                    #endregion
 
                }
                using (var contxt = new BayanEntities())
                {
                    using (var db_contextTransaction = contxt.Database.BeginTransaction())
                    {
                        try
                        {
                            SaveInDB(VM, UserID, Vendor_CompanyID, contxt, false);
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
            catch (Exception ex)
            {
                return ErrorUnit.RetriveExceptionMsg(ex);
            }
        }

        private void SaveInDB(OrderConfigVM VM, int UserID, string Vendor_CompanyID, BayanEntities contxt, bool SaveFromFile)
        {
            decimal MastrID;

            #region Order Config Master
            var OldOrderConfigMstr = contxt.Tbl_OrdConfig_Mstr.Where(w => w.ID == VM.ID).FirstOrDefault();
            if (OldOrderConfigMstr != null)
            {
                MastrID = OldOrderConfigMstr.ID;
                OldOrderConfigMstr.IsActive = VM.IsActive;
                OldOrderConfigMstr.LastUpdate = DateTime.Now;
                OldOrderConfigMstr.UpdatedBy = UserID;
                //  FillReplenishConfig(VM, OldRepConfigMstr, UserID, SaveFromFile);
            }
            else
            {
                var NewOrderConfigMstr = new Tbl_OrdConfig_Mstr() { CreatedBy = UserID, CreationDate = DateTime.Now, Vendor_CompanyID = Vendor_CompanyID };
                FillOrderConfig(VM, NewOrderConfigMstr, UserID, SaveFromFile);
                contxt.Tbl_OrdConfig_Mstr.Add(NewOrderConfigMstr);
                contxt.SaveChanges();
                MastrID = NewOrderConfigMstr.ID;
            }
            #endregion

            #region Order Customer add and edit
            List<Tbl_OrdConfigCustomer> ordConfigCustomers = new List<Tbl_OrdConfigCustomer>();

            var OrdrCustExist = contxt.Tbl_OrdConfigCustomer.Where(w => w.OrdConfig_Mstr_ID == MastrID).ToList();
            if (OrdrCustExist.Count() > 0)
            {
                contxt.Tbl_OrdConfigCustomer.RemoveRange(OrdrCustExist);
                contxt.SaveChanges();
            }
            foreach (var item in VM.CustomersDtlLst)
            {
                ordConfigCustomers.Add(new Tbl_OrdConfigCustomer { OrdConfig_Mstr_ID = MastrID, CustomerDtl_ID = item });
            }
            contxt.Tbl_OrdConfigCustomer.AddRange(ordConfigCustomers);
            contxt.SaveChanges(); 
            #endregion

            #region Old Visit Schedule Type

            var OldVisitScheduleType = contxt.Tbl_OrdConfig_VisitScheduleType.Where(w => w.ConfigMstrID == MastrID && w.IsActive == true).ToList();
            foreach (var item in OldVisitScheduleType)
            {
                item.IsActive = false; item.LastUpdate = DateTime.Now;
            }
            contxt.SaveChanges();

            if (VM.IsActive == true)
            {
                foreach (var item in VM.VisitSchedule_Data)
                {
                    var Exist = contxt.Tbl_OrdConfig_VisitScheduleType.Where(w => w.ConfigMstrID == MastrID && w.OrderTypeID == item.WeekNumber && w.DayNum == item.DayNumber).FirstOrDefault();
                    if (Exist != null)
                    {
                        Exist.IsActive = true; Exist.Percentage = item.OrderPercentage;
                    }
                    else
                    {
                        var NewObj = new Tbl_OrdConfig_VisitScheduleType()
                        {
                            IsActive = true,
                            ConfigMstrID = MastrID,
                            DayNum = item.DayNumber,
                            OrderTypeID = item.WeekNumber,
                            LastUpdate = DateTime.Now,
                            Percentage = item.OrderPercentage, 
                        };
                        contxt.Tbl_OrdConfig_VisitScheduleType.Add(NewObj);
                    }
                }
                contxt.SaveChanges();
            }
            #endregion

        }
        private void FillOrderConfig(OrderConfigVM VM, Tbl_OrdConfig_Mstr OrderConfigMstr, int UserID, bool SaveFromFile)
        {
            OrderConfigMstr.Name = VM.Name;
            OrderConfigMstr.NameEng = VM.NameEng;
            OrderConfigMstr.VisitScheduleTypeID = VM.VisitScheduleTypeID;
            OrderConfigMstr.IsActive = VM.IsActive;
            OrderConfigMstr.LastUpdate = DateTime.Now;
            OrderConfigMstr.UpdatedBy = UserID; 
            OrderConfigMstr.VisitScheduleCycleCount = VM.VisitSchedule_Data.Count;
        }

    }


}