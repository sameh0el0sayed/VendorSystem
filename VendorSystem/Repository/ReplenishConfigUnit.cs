using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendorSystem.Models.Model1;
using VendorSystem.ViewModel;

namespace VendorSystem.Repository
{
    public class ReplenishConfigUnit
    {
        BayanEntities DB;
        public ReplenishConfigUnit(BayanEntities _Db)
        {
            DB = _Db;
        }
        public ReplenishConfigUnit()
        {
            DB = new BayanEntities();
        }

        public IQueryable<Tbl_CalculationType> GetAllCalculationType()
        {
            return DB.Tbl_CalculationType;
        }

        public IQueryable<Tbl_ScheduleType> GetAllScheduleType()
        {
            return DB.Tbl_ScheduleType;
        }

        public IQueryable<Tbl_WeekDay> GetAllWeekDays()
        {
            return DB.Tbl_WeekDay;
        }

        public List<ReplenishConfigVM> GetAllReplenishConfig(string Vendor_CompanyID)
        {
            return DB.Tbl_RepConfig_Mstr.Where(m => m.Vendor_CompanyID == Vendor_CompanyID).Select(m=> new ReplenishConfigVM {  ID=  m.ID  ,Name=m.Name, NameEng = m.NameEng, IsActive = m.IsActive }).ToList();
        }

        public List<Tbl_RepConfig_Mstr> GetReplenishConfigByID(string Vendor_CompanyID, decimal ID)
        {
            return DB.Tbl_RepConfig_Mstr.Where(m => m.Vendor_CompanyID == Vendor_CompanyID && m.ID == ID).ToList();
        }
        public string Save(ReplenishConfigVM VM, int UserID, string Vendor_CompanyID)
        {
            try
            {

                var OldRepConfigMstr = DB.Tbl_RepConfig_Mstr.Where(w => w.ID == VM.ID).FirstOrDefault();
                if (OldRepConfigMstr == null)
                {
                    #region check of arabic name 
                    var ExistObj = DB.Tbl_RepConfig_Mstr.Where(w => w.ID != VM.ID && w.Name == VM.Name && w.Vendor_CompanyID == Vendor_CompanyID && w.IsActive == true).FirstOrDefault();
                    if (ExistObj != null)
                    {
                        return CheckUnit.RetriveCorrectMsg("الاسم موجود من قبل", " Name is already exist before");
                    }
                    #endregion

                    #region check of English name 
                    ExistObj = DB.Tbl_RepConfig_Mstr.Where(w => w.ID != VM.ID && w.NameEng == VM.NameEng && w.Vendor_CompanyID == Vendor_CompanyID && w.IsActive == true).FirstOrDefault();
                    if (ExistObj != null)
                    {
                        return CheckUnit.RetriveCorrectMsg("الاسم باللغة الانجليزية موجود من قبل", "English Name is already exist before");
                    }
                    #endregion

                    #region check if first to fourth class is exsit 
                    ExistObj = DB.Tbl_RepConfig_Mstr.Where(w => w.ID != VM.ID && w.FirstClassID == VM.FirstClassificationID
                            && w.SecondClassID == VM.SecondClassificationID && w.ThirdClassID == VM.ThirdClassificationID && w.FourthClassID == VM.FourthClassificationID
                            && w.Vendor_CompanyID == Vendor_CompanyID && w.IsActive == true).FirstOrDefault();
                    if (ExistObj != null)
                    {
                        return CheckUnit.RetriveCorrectMsg("  :نفس التصنيف الاول والثانى والثالث والرابع موجود من قبل كود   " + ExistObj.ID + "", "The same first and second and third and fourth classificatin exist befor Code: " + ExistObj.ID + "");
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
        
        private void SaveInDB(ReplenishConfigVM VM, int UserID, string Vendor_CompanyID, BayanEntities contxt, bool SaveFromFile)
        {
            decimal MastrID;

            #region Rep Config Master
            var OldRepConfigMstr = contxt.Tbl_RepConfig_Mstr.Where(w => w.ID == VM.ID).FirstOrDefault();
            if (OldRepConfigMstr != null)
            {
                MastrID = OldRepConfigMstr.ID;
                OldRepConfigMstr.IsActive = VM.IsActive;
                OldRepConfigMstr.LastUpdate = DateTime.Now;
                OldRepConfigMstr.UpdatedBy = UserID;
                //  FillReplenishConfig(VM, OldRepConfigMstr, UserID, SaveFromFile);
            }
            else
            {
                var NewRepConfigMstr = new Tbl_RepConfig_Mstr() { CreatedBy = UserID, CreationDate = DateTime.Now, Vendor_CompanyID = Vendor_CompanyID };
                FillReplenishConfig(VM, NewRepConfigMstr, UserID, SaveFromFile);
                contxt.Tbl_RepConfig_Mstr.Add(NewRepConfigMstr);
                contxt.SaveChanges();
                MastrID = NewRepConfigMstr.ID;
            } 
            #endregion

            #region Old Rep Schedule Type

            var OldRepScheduleType = contxt.Tbl_RepConfig_RepScheduleType.Where(w => w.ConfigMstrID == MastrID && w.IsActive == true).ToList();
            foreach (var item in OldRepScheduleType)
            {
                item.IsActive = false; item.LastUpdate = DateTime.Now;
            }
            contxt.SaveChanges();
            if (VM.IsActive == true)
            {
                foreach (var item in VM.RepSchedule_Data)
                {
                    var Exist = contxt.Tbl_RepConfig_RepScheduleType.Where(w => w.ConfigMstrID == MastrID && w.OrderTypeID == item.WeekNumber && w.DayNum == item.DayNumber).FirstOrDefault();
                    if (Exist != null)
                    {
                        Exist.IsActive = true;
                    }
                    else
                    {
                        var NewObj = new Tbl_RepConfig_RepScheduleType()
                        {
                            IsActive = true,
                            ConfigMstrID = MastrID,
                            DayNum = item.DayNumber,
                            LastUpdate = DateTime.Now,
                            OrderTypeID = item.WeekNumber
                        };
                        contxt.Tbl_RepConfig_RepScheduleType.Add(NewObj);
                    }
                }
                contxt.SaveChanges();
            }
            #endregion

            #region Old Visit Schedule Type

            //var OldVisitScheduleType = contxt.Tbl_RepConfig_VisitScheduleType.Where(w => w.ConfigMstrID == MastrID && w.IsActive == true).ToList();
            //foreach (var item in OldVisitScheduleType)
            //{
            //    item.IsActive = false; item.LastUpdate = DateTime.Now;
            //}
            //contxt.SaveChanges();

            //if (VM.IsActive == true)
            //{
            //    foreach (var item in VM.VisitSchedule_Data)
            //    {
            //        var Exist = contxt.Tbl_RepConfig_VisitScheduleType.Where(w => w.ConfigMstrID == MastrID && w.OrderTypeID == item.WeekNumber && w.DayNum == item.DayNumber).FirstOrDefault();
            //        if (Exist != null)
            //        {
            //            Exist.IsActive = true; Exist.Percentage = item.OrderPercentage;
            //        }
            //        else
            //        {
            //            var NewObj = new Tbl_RepConfig_VisitScheduleType()
            //            {
            //                IsActive = true,
            //                ConfigMstrID = MastrID,
            //                DayNum = item.DayNumber,
            //                LastUpdate = DateTime.Now,
            //                OrderTypeID = item.WeekNumber,
            //                Percentage = item.OrderPercentage
            //            };
            //            contxt.Tbl_RepConfig_VisitScheduleType.Add(NewObj);
            //        }
            //    }
            //    contxt.SaveChanges();
            //}
            #endregion

        }
        private void FillReplenishConfig(ReplenishConfigVM VM, Tbl_RepConfig_Mstr RepConfigMstr, int UserID, bool SaveFromFile)
        {
            RepConfigMstr.Name = VM.Name;
            RepConfigMstr.NameEng = VM.NameEng;
            RepConfigMstr.CalcTypeID = VM.CalcTypeID;
            RepConfigMstr.RepScheduleTypeID = VM.Rep_ScheduleTypeID;
            RepConfigMstr.VisitScheduleTypeID = VM.Visit_ScheduleTypeID;
            RepConfigMstr.IsIncludeMinQty = VM.IsIncludeMinQty;
            RepConfigMstr.FirstClassID = VM.FirstClassificationID;
            RepConfigMstr.SecondClassID = VM.SecondClassificationID;
            RepConfigMstr.ThirdClassID = VM.ThirdClassificationID;
            RepConfigMstr.FourthClassID = VM.FourthClassificationID;
            RepConfigMstr.IsActive = VM.IsActive;
            RepConfigMstr.LastUpdate = DateTime.Now;
            RepConfigMstr.UpdatedBy = UserID;
            RepConfigMstr.RepScheduleCycleCount = VM.RepSchedule_Data.Count;
            RepConfigMstr.VisitScheduleCycleCount = 0;
        }


    }
}