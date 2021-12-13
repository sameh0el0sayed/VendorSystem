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
    public class RouteCombinationUnit
    {
        BayanEntities DB;
        FileManager FileManager = new FileManager();
        public RouteCombinationUnit(BayanEntities _Db)
        {
            DB = _Db;
        }
        public RouteCombinationUnit()
        {
            DB = new BayanEntities();
        }

        public IQueryable<Tbl_RouteCombinMaster> GetAllRouteCombinations(string Vendor_CompanyID)
        {
            return DB.Tbl_RouteCombinMaster.Where(w=> w.Vendor_CompanyID == Vendor_CompanyID);
        }

        public IQueryable<Tbl_RouteCombinDtl> GetAllActiveCustomersByRouteCombinationID(int RouteCombinationID)
        {
            return DB.Tbl_RouteCombinDtl.Where(w => w.MasterID == RouteCombinationID && w.Active == true);
        }

        public IQueryable<Tbl_RouteCombinMaster> GetAllActiveRouteCombinations(string Vendor_CompanyID)
        {
            return DB.Tbl_RouteCombinMaster.Where(w => w.IsActive == true && w.Vendor_CompanyID == Vendor_CompanyID);
        }

        public List<RouteCombinationVM> GetAllRouteCombinationsVM(string Vendor_CompanyID)
        {
            var Qry = DB.V_GetAllRouteCombinationsVM.Where(w=> w.Vendor_CompanyID == Vendor_CompanyID).Select(w => new RouteCombinationVM
            {
                Name = w.Name,
                NameEng = w.NameEng,
                DistributorName = w.DistributorName,
                DistributorNameEng = w.DistributorNameEng,
                RouteName = w.RouteName,
                RouteNameEng = w.RouteNameEng,
                Note = w.Note,
                ID = w.ID,
                IsActive = w.IsActive
            });

            return Qry.ToList();
        }

        public string Save(RouteCombinationVM VM, int UserID, string Vendor_CompanyID)
        {
            try
            {

                #region Region To Validate That One Route Is Used for customer 
                var ExistObj = DB.Tbl_RouteCombinMaster.Where(w => w.ID != VM.ID && w.RouteID == VM.RouteID).FirstOrDefault();
                if (ExistObj != null)
                {
                    return CheckUnit.RetriveCorrectMsg("هذا المسار موجود من قبل", "This  Route aready exist befor");
                }
                #endregion

                ExistObj = DB.Tbl_RouteCombinMaster.Where(w => w.ID != VM.ID && w.Name == VM.Name && w.Vendor_CompanyID == Vendor_CompanyID).FirstOrDefault();
                if (ExistObj != null)
                {
                    return CheckUnit.RetriveCorrectMsg("الاسم موجود من قبل", " Name is aready exist befor");
                }

                ExistObj = DB.Tbl_RouteCombinMaster.Where(w => w.ID != VM.ID && w.NameEng == VM.NameEng && w.Vendor_CompanyID == Vendor_CompanyID).FirstOrDefault();
                if (ExistObj != null)
                {
                    return CheckUnit.RetriveCorrectMsg("الاسم باللغة الانجليزية موجود من قبل", "English Name is aready exist befor");
                }

                ExistObj = DB.Tbl_RouteCombinMaster.Where(w => w.ID != VM.ID && w.RouteID == VM.RouteID && w.DistributorID == VM.DistributorID && w.Vendor_CompanyID == Vendor_CompanyID).FirstOrDefault();
                if (ExistObj != null)
                {
                    return CheckUnit.RetriveCorrectMsg("هذا الموزع مع نفس المسار موجود من قبل", "This distributor with the same route aready exist befor");
                }

                using (var contxt = new BayanEntities())
                {
                    using (var db_contextTransaction = contxt.Database.BeginTransaction())
                    {
                        try
                        {
                            SaveInDB(VM, UserID, Vendor_CompanyID, contxt, false);
                            db_contextTransaction.Commit();
                            DeleteOldData();
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

        private void SaveInDB(RouteCombinationVM VM, int UserID, string Vendor_CompanyID, BayanEntities contxt, bool SaveFromFile)
        {
            int MastrID;
            var ExistFirstFromSecondClass = contxt.LockUps.Where(w => VM.SecondClassificationLst.Contains(w.ID)).Select(w => w.ParentId).Distinct().ToList();
            var RemainingFirstClass = VM.FirstClassificationLst.Where(w => !(ExistFirstFromSecondClass.Contains(w))).ToList();


            var OldRouteCombination = contxt.Tbl_RouteCombinMaster.Where(w => w.ID == VM.ID).FirstOrDefault();
            if (OldRouteCombination != null)
            {
                MastrID = OldRouteCombination.ID;
                FillRouteCombination(VM, OldRouteCombination, UserID, SaveFromFile);
            }
            else
            {
                var NewRouteCombination = new Tbl_RouteCombinMaster() { CreatedBy = UserID, CreationDate = DateTime.Now, Vendor_CompanyID = Vendor_CompanyID };
                FillRouteCombination(VM, NewRouteCombination, UserID, SaveFromFile);
                contxt.Tbl_RouteCombinMaster.Add(NewRouteCombination);
                contxt.SaveChanges();
                MastrID = NewRouteCombination.ID;
            }

            #region Set Old Data To be InActive
            var OldDtlLst = contxt.Tbl_RouteCombinDtl.Where
                                      (w => w.Active == true &&
                                          (
                                             w.MasterID == MastrID ||
                                             ((VM.CustomersDtlLst.Contains(w.CustDtl_ID))
                                             // &&  (RemainingFirstClass.Contains(w.FirstClassID) || VM.SecondClassificationLst.Contains(w.SecondClassID))
                                             )
                                         )
                                     ).ToList();

            foreach (var item in OldDtlLst)
            {
                item.Active = false; item.LastUpdate = DateTime.Now;
            }
            contxt.SaveChanges();
            #endregion


            #region Old Combination Dtl Customer
            var OLodCustDtl = OldDtlLst.Select(w => new { MasterID = w.MasterID, CustDtl_ID = w.CustDtl_ID }).Distinct().ToList();
            foreach (var item in OLodCustDtl)
            {
                var IsStillOjectActive = contxt.Tbl_RouteCombinDtl.Where(w => w.CustDtl_ID == item.CustDtl_ID && w.MasterID == item.MasterID && w.Active == true).FirstOrDefault();

                if (IsStillOjectActive != null) continue;

                var OldCombinCustomer = contxt.Tbl_RouteCombinDtlCustomer.Where(w => w.CustID == item.CustDtl_ID && w.MstrID == item.MasterID && w.IsActive == true).FirstOrDefault();
                if (OldCombinCustomer != null)
                {
                    OldCombinCustomer.IsActive = false; OldCombinCustomer.LastUpdate = DateTime.Now;
                    contxt.SaveChanges();
                }
            }
            #endregion

            #region Old Combination Dtl First Class
            var OldFirstClassDtl = OldDtlLst.Select(w => new { MasterID = w.MasterID, FirstClassID = w.FirstClassID }).Distinct().ToList();
            foreach (var item in OldFirstClassDtl)
            {
                var IsStillOjectActive = contxt.Tbl_RouteCombinDtl.Where(w => w.FirstClassID == item.FirstClassID && w.MasterID == item.MasterID && w.Active == true).FirstOrDefault();
                if (IsStillOjectActive != null) continue;

                var OldCominFirstClass = contxt.Tbl_RouteCombinDtlFirstClass.Where(w => w.FirstClassID == item.FirstClassID && w.MstrID == item.MasterID && w.IsActive == true).FirstOrDefault();
                if (OldCominFirstClass != null)
                {
                    OldCominFirstClass.IsActive = false; OldCominFirstClass.LastUpdate = DateTime.Now;
                    contxt.SaveChanges();
                }
            }
            #endregion

            #region Old Combination Dtl Second
            var OldSecondClassDtl = OldDtlLst.Where(w => w.SecondClassID != null).Select(w => new { MasterID = w.MasterID, SecondClassID = w.SecondClassID }).Distinct().ToList();
            foreach (var item in OldSecondClassDtl)
            {
                var IsStillOjectActive = contxt.Tbl_RouteCombinDtl.Where(w => w.SecondClassID == item.SecondClassID && w.MasterID == item.MasterID && w.Active == true).FirstOrDefault();
                if (IsStillOjectActive != null) continue;

                var OldCominSecondClass = contxt.Tbl_RouteCombinDtlSecondClass.Where(w => w.SecondClassID == item.SecondClassID && w.MstrID == item.MasterID && w.IsActive == true).FirstOrDefault();
                if (OldCominSecondClass != null)
                {
                    OldCominSecondClass.IsActive = false; OldCominSecondClass.LastUpdate = DateTime.Now;
                    contxt.SaveChanges();
                }
            }
            #endregion


            #region New Combination Dtl Customer
            foreach (var item in VM.CustomersDtlLst)
            {
                var ExistCombinCustomer = contxt.Tbl_RouteCombinDtlCustomer.Where(w => w.CustID == item && w.MstrID == MastrID).FirstOrDefault();
                if (ExistCombinCustomer != null)
                {
                    ExistCombinCustomer.IsActive = true; ExistCombinCustomer.LastUpdate = DateTime.Now;
                }
                else
                {
                    contxt.Tbl_RouteCombinDtlCustomer.Add(new Tbl_RouteCombinDtlCustomer()
                    {
                        CustID = item,
                        IsActive = true,
                        LastUpdate = DateTime.Now,
                        MstrID = MastrID
                    });
                }

                contxt.SaveChanges();
            }
            #endregion

            #region New Combination Dtl FirstClass

            foreach (var item in RemainingFirstClass)
            {
                var ExistCombinFirstClass = contxt.Tbl_RouteCombinDtlFirstClass.Where(w => w.FirstClassID == item && w.MstrID == MastrID).FirstOrDefault();
                if (ExistCombinFirstClass != null)
                {
                    ExistCombinFirstClass.IsActive = true; ExistCombinFirstClass.LastUpdate = DateTime.Now;
                }
                else
                {
                    contxt.Tbl_RouteCombinDtlFirstClass.Add(new Tbl_RouteCombinDtlFirstClass()
                    {
                        FirstClassID = item,
                        IsActive = true,
                        LastUpdate = DateTime.Now,
                        MstrID = MastrID
                    });
                }

                contxt.SaveChanges();
            }
            #endregion


            #region New Combination Dtl Second Class

            foreach (var item in VM.SecondClassificationLst)
            {
                var ExistCombinSecondClass = contxt.Tbl_RouteCombinDtlSecondClass.Where(w => w.SecondClassID == item && w.MstrID == MastrID).FirstOrDefault();
                if (ExistCombinSecondClass != null)
                {
                    ExistCombinSecondClass.IsActive = true; ExistCombinSecondClass.LastUpdate = DateTime.Now;
                }
                else
                {
                    contxt.Tbl_RouteCombinDtlSecondClass.Add(new Tbl_RouteCombinDtlSecondClass()
                    {
                        SecondClassID = item,
                        IsActive = true,
                        LastUpdate = DateTime.Now,
                        MstrID = MastrID,
                    });
                }
                contxt.SaveChanges();
            }
            #endregion

            foreach (var Cust in VM.CustomersDtlLst)
            {
                #region CustomersDtlLst

                var OldDtlObj = contxt.Tbl_RouteCombinDtl.Where(w => w.MasterID == MastrID && w.CustDtl_ID == Cust).FirstOrDefault();
                if (OldDtlObj != null)
                {
                    OldDtlObj.Active = true; OldDtlObj.LastUpdate = DateTime.Now;
                }
                else
                {
                    var NewDtl = new Tbl_RouteCombinDtl()
                    {
                        Active = true,
                        CustDtl_ID = Cust,
                        FirstClassID = null,
                        LastUpdate = DateTime.Now,
                        MasterID = MastrID,
                        SecondClassID = null
                    };
                    contxt.Tbl_RouteCombinDtl.Add(NewDtl);
                }

                contxt.SaveChanges();
                #endregion

                #region First Classification
                //foreach (var FirstClass in RemainingFirstClass)
                //{
                //     OldDtlObj = contxt.Tbl_RouteCombinDtl.Where(w => w.MasterID == MastrID && w.CustDtl_ID == Cust && w.FirstClassID == FirstClass && w.SecondClassID == null).FirstOrDefault();
                //    if (OldDtlObj != null)
                //    {
                //        OldDtlObj.Active = true; OldDtlObj.LastUpdate = DateTime.Now;
                //    }
                //    else
                //    {
                //        var NewDtl = new Tbl_RouteCombinDtl()
                //        {
                //            Active = true,
                //            CustDtl_ID = Cust,
                //            FirstClassID = FirstClass.Value,
                //            LastUpdate = DateTime.Now,
                //            MasterID = MastrID,
                //            SecondClassID = null
                //        };
                //        contxt.Tbl_RouteCombinDtl.Add(NewDtl);
                //    }

                //    contxt.SaveChanges();
                //}
                #endregion

                #region Second Classififcation
                //var SecondLst = contxt.LockUps.Where(w => w.Active == true && VM.SecondClassificationLst.Contains(w.ID)).Select(w => new { ID = w.ID, ParentId = w.ParentId }).ToList();
                //foreach (var item in SecondLst)
                //{
                //     OldDtlObj = contxt.Tbl_RouteCombinDtl.Where(w => w.MasterID == MastrID && w.CustDtl_ID == Cust && w.FirstClassID == item.ParentId && w.SecondClassID == item.ID).FirstOrDefault();
                //    if (OldDtlObj != null)
                //    {
                //        OldDtlObj.Active = true; OldDtlObj.LastUpdate = DateTime.Now;
                //    }
                //    else
                //    {
                //        var NewDtl = new Tbl_RouteCombinDtl()
                //        {
                //            Active = true,
                //            CustDtl_ID = Cust,
                //            FirstClassID = item.ParentId.Value,
                //            LastUpdate = DateTime.Now,
                //            MasterID = MastrID,
                //            SecondClassID = item.ID
                //        };
                //        contxt.Tbl_RouteCombinDtl.Add(NewDtl);
                //    }
                //    contxt.SaveChanges();
                //}
                #endregion
            }
        }

        private void DeleteOldData()
        {
            try
            {
                var LastWeekDate = DateTime.Now.AddDays(-7);
                var OldInActiveData = DB.Tbl_RouteCombinDtl.Where(w => w.Active == false && w.LastUpdate < LastWeekDate).ToList();
                DB.Tbl_RouteCombinDtl.RemoveRange(OldInActiveData);
                DB.SaveChanges();

                var OldInActiveDataCustmr = DB.Tbl_RouteCombinDtlCustomer.Where(w => w.IsActive == false && w.LastUpdate < LastWeekDate).ToList();
                DB.Tbl_RouteCombinDtlCustomer.RemoveRange(OldInActiveDataCustmr);
                DB.SaveChanges();

                var OldInActiveDataFirst = DB.Tbl_RouteCombinDtlFirstClass.Where(w => w.IsActive == false && w.LastUpdate < LastWeekDate).ToList();
                DB.Tbl_RouteCombinDtlFirstClass.RemoveRange(OldInActiveDataFirst);
                DB.SaveChanges();

                var OldInActiveDataSecond = DB.Tbl_RouteCombinDtlSecondClass.Where(w => w.IsActive == false && w.LastUpdate < LastWeekDate).ToList();
                DB.Tbl_RouteCombinDtlSecondClass.RemoveRange(OldInActiveDataSecond);
                DB.SaveChanges();
            }
            catch
            {

            }
        }

        public string UploadAndSave(HttpServerUtilityBase Server, HttpRequestBase Request, int UserID, string Vendor_CompanyID)
        {
            ExcelSheetRouteCombination ResultCombination = new ExcelSheetRouteCombination();
            string Path = FileManager.CreateNewFileFromRequist(Server, Request, 0, "");
            ExcelSheetRouteCombination Result = new ExcelSheetRouteCombination();
            ResultCombination = ReadFromSpecificFile(Result, Path, Vendor_CompanyID);
            string Stats = "Done";
            if (ResultCombination.Status != 0)
            {
                using (var contxt = new BayanEntities())
                {
                    using (var db_contextTransaction = contxt.Database.BeginTransaction())
                    {
                        try
                        {
                            var Lst_MasterCombination = ResultCombination.Result.Select(w => new { w.RouteID, w.DistributorID, w.CombinationName, w.CombinationNameEng, w.MasterID }).Distinct().ToList();
                            foreach (var item in Lst_MasterCombination)
                            {
                                var RouteCombinationVM = new RouteCombinationVM()
                                {
                                    DistributorID = item.DistributorID,
                                    RouteID = item.RouteID,
                                    ID = item.MasterID,
                                    DistributorName = "",
                                    DistributorNameEng = "",
                                    IsActive = true,
                                    Name = item.CombinationName,
                                    NameEng = item.CombinationNameEng,
                                    Note = "",
                                    RouteName = "",
                                    RouteNameEng = "",
                                };
                                RouteCombinationVM.CustomersDtlLst = ResultCombination.Result.Where(w => w.DistributorID == item.DistributorID && w.RouteID == item.RouteID).Select(w => w.CustomerDtlID).Distinct().ToList();
                                //RouteCombinationVM.FirstClassificationLst = ResultCombination.Result.Where(w => w.DistributorID == item.DistributorID && w.RouteID == item.RouteID).Select(w => w.FirstClassID).Distinct().ToList();
                                //RouteCombinationVM.SecondClassificationLst = ResultCombination.Result.Where(w => w.DistributorID == item.DistributorID && w.RouteID == item.RouteID).Select(w => w.SecondClassID).Distinct().ToList();
                                RouteCombinationVM.FirstClassificationLst = new List<int?>();
                                RouteCombinationVM.SecondClassificationLst = new List<int?>();

                                SaveInDB(RouteCombinationVM, UserID, Vendor_CompanyID, contxt, true);
                            }

                            db_contextTransaction.Commit();
                            DeleteOldData();
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
            else
            {
                Stats = "";
                foreach (var item in ResultCombination.Result)
                {
                    Stats += item.CombinationName + " ";
                }
            }

            return Stats;
        }

        private ExcelSheetRouteCombination ReadFromSpecificFile(ExcelSheetRouteCombination Result, string path, string Vendor_CompanyID)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;

            Result.Status = 1;
            Result.Result = new List<RouteCombinationFile>();
            OfficeOpenXml.ExcelWorksheet worksheet;
            int rows, columns;
            FileManager.PrepareForReadingFromSpecificFile(path, out worksheet, out rows, out columns);

            object _CombinationName;
            object _CombinationNameEng;
            object _DistributorCode;
            object _RouteCode;
            //object _FirstClassName;
            //object _SecondClassName;
            object _CustomerCode;


            string CombinationName;
            string CombinationNameEng;
            string DistributorCode;
            string RouteCode;
            //string FirstClassName;
            //string SecondClassName;
            string CustomerCode;

            for (int i = 3; i <= rows; i++)
            {

                //_SecondClassName = worksheet.Cells[i, 7].Value;
                //_FirstClassName = worksheet.Cells[i, 6].Value;
                _CustomerCode = worksheet.Cells[i, 1].Value;
                _RouteCode = worksheet.Cells[i, 2].Value;
                _DistributorCode = worksheet.Cells[i, 3].Value;
                _CombinationNameEng = worksheet.Cells[i, 4].Value;
                _CombinationName = worksheet.Cells[i, 5].Value;



                #region CustomerCode
                if (_CustomerCode == null)
                {
                    InsertError(Result, Lang, i, "Customer ID is empty", "كود العميل فارغ");
                    continue;
                }
                CustomerCode = _CustomerCode.ToString();
                #endregion

                #region SecondClassName
                //if (_SecondClassName == null)
                //{
                //    SecondClassName = null;
                //}
                //else
                //{
                //    SecondClassName = _SecondClassName.ToString();
                //}
                #endregion

                #region FirstClassName
                //if (_FirstClassName == null)
                //{
                //    InsertError(Result, Lang, i, "First Class Name is empty", " التصنيف الاول فارغ");
                //    continue;
                //}

                //FirstClassName = _FirstClassName.ToString();

                #endregion

                #region RouteCode
                if (_RouteCode == null)
                {
                    InsertError(Result, Lang, i, "Route Code is empty", "كود المسار فارغ");
                    continue;
                }
                RouteCode = _RouteCode.ToString();
                #endregion

                #region DistributorCode
                if (_DistributorCode == null)
                {
                    InsertError(Result, Lang, i, "Distributor Code is empty", "كود الموزع فارغ");
                    continue;
                }
                DistributorCode = _DistributorCode.ToString();
                #endregion

                #region CombinationNameEng
                if (_CombinationNameEng == null)
                {
                    InsertError(Result, Lang, i, "Combination Name Eng  is empty", "الاسم باللغة الانجليزية فارغ");
                    continue;
                }
                CombinationNameEng = _CombinationNameEng.ToString();
                #endregion

                #region CombinationName
                if (_CombinationName == null)
                {
                    InsertError(Result, Lang, i, "Combination Name is empty", "الاسم باللغة العربية فارغ");
                    continue;
                }
                CombinationName = _CombinationName.ToString();
                #endregion

                var RouteCombinationFile = new RouteCombinationFile()
                {
                    CombinationName = CombinationName,
                    CombinationNameEng = CombinationNameEng,
                    CustomerCode = CustomerCode,
                    DistributorCode = DistributorCode,
                    RouteCode = RouteCode
                    //FirstClassName = FirstClassName,
                    //SecondClassName = SecondClassName
                };
                if (Result.Status != 0)
                {
                    Result.Result.Add(RouteCombinationFile);
                }
            }

            if (Result.Status != 0)
            {

                var DublicatedData = Result.Result.Select(w => new { w.RouteCode, w.DistributorCode, w.CustomerCode,/* w.FirstClassID, w.SecondClassID */}).Distinct().ToList();
                if (DublicatedData.Count < Result.Result.Count)
                {
                    InsertError(Result, Lang, 0, "You try to upload Doblicated data)", "عفوا هناك بيانات مكرره");
                    return Result;
                }

                if (Result.Status != 0)
                {

                    int i = 2;
                    foreach (var item in Result.Result)
                    {
                        i++;
                        int MastrID = 0;

                        #region Route
                        var Route = DB.Tbl_Route.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.Code == item.RouteCode && w.IsActive == true).FirstOrDefault();
                        if (Route == null)
                        {
                            InsertError(Result, Lang, i, "Route not found", "المسار غير موجود");
                            continue;
                        }
                        else
                        {
                            item.RouteID = Route.ID;
                        }
                        #endregion


                        var ExistRecombination = DB.Tbl_RouteCombinMaster.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.RouteID == item.RouteID).FirstOrDefault();
                        if (ExistRecombination != null)
                        {
                            MastrID = ExistRecombination.ID;
                            item.MasterID = ExistRecombination.ID;
                        }

                        ExistRecombination = DB.Tbl_RouteCombinMaster.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.ID != MastrID && w.Name == item.CombinationName).FirstOrDefault();
                        if (ExistRecombination != null)
                        {
                            InsertError(Result, Lang, i, "Arabic Name aready exist before", "الاسم باللغة العربية موجود من قبل");
                            continue;
                        }

                        ExistRecombination = DB.Tbl_RouteCombinMaster.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.ID != MastrID && w.NameEng == item.CombinationNameEng).FirstOrDefault();
                        if (ExistRecombination != null)
                        {
                            InsertError(Result, Lang, i, "English Name aready exist before", "الاسم باللغة الانجليزية موجود من قبل");
                            continue;
                        }


                        #region Distributor
                        var Distrib = DB.Tbl_Distributor.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.Code == item.DistributorCode && w.IsActive == true).FirstOrDefault();
                        if (Distrib == null)
                        {
                            InsertError(Result, Lang, i, "Distributor not found", "الموزع غير موجود");
                            continue;
                        }
                        else
                        {
                            item.DistributorID = Distrib.ID;
                        }
                        #endregion



                        ExistRecombination = DB.Tbl_RouteCombinMaster.Where(w => w.RouteID == Route.ID && w.Vendor_CompanyID == Vendor_CompanyID && w.ID != item.MasterID).FirstOrDefault();
                        if (ExistRecombination != null)
                        {
                            InsertError(Result, Lang, i, "This Route aready exist befor", "هذا المسار موجود من قبل");
                            continue;

                        }

                        ExistRecombination = DB.Tbl_RouteCombinMaster.Where(w => w.RouteID == Route.ID && w.DistributorID == Distrib.ID && w.Vendor_CompanyID == Vendor_CompanyID && w.ID != item.MasterID).FirstOrDefault();
                        if (ExistRecombination != null)
                        {
                            InsertError(Result, Lang, i, "This distributor with the same route aready exist befor", "هذا الموزع مع نفس المسار موجود من قبل");
                            continue;

                        }

                        #region First Class
                        //var FirstClass = DB.LockUps.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && (w.Name == item.FirstClassName || w.NameEng == item.FirstClassName) && w.Active != true && w.ParentId == 1).FirstOrDefault();
                        //if (FirstClass == null)
                        //{
                        //    InsertError(Result, Lang, i, "First Class Name  found", "التصنيف الاول غير موجود");
                        //    continue;
                        //}
                        //else
                        //{
                        //    item.FirstClassID = FirstClass.ID;
                        //}
                        #endregion

                        #region Second Class

                        //if (item.SecondClassName != null)
                        //{
                        //    var SecondClass = DB.LockUps.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && (w.Name == item.SecondClassName || w.NameEng == item.SecondClassName) && w.Active != true && w.ParentId == FirstClass.ID).FirstOrDefault();
                        //    if (SecondClass == null)
                        //    {
                        //        InsertError(Result, Lang, i, "Second Class Name  found", "التصنيف الثانى غير موجود");
                        //        continue;
                        //    }
                        //    else
                        //    {
                        //        item.SecondClassID = SecondClass.ID;
                        //    }
                        //}
                        //else
                        //{
                        //    item.SecondClassID = null;
                        //}


                        #endregion

                        #region Customer Code
                        var Cust = DB.Tbl_CustomerDtl.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.CustoemrCode == item.CustomerCode && w.Active == true).FirstOrDefault();
                        if (Cust == null)
                        {
                            InsertError(Result, Lang, i, "Customer Not found", "العميل غير موجود ");
                            continue;
                        }
                        else
                        {
                            item.CustomerDtlID = Cust.ID;
                        }
                        #endregion

                    }
                }
            }
            return Result;
        }

        private void InsertError(ExcelSheetRouteCombination Result, string Lang, int i, string EngMsg, string ArMsg)
        {
            if (Result.Status == 1)
            {
                Result.Result = new List<RouteCombinationFile>();
            }

            if (Lang == "ar-SA")
            {
                Result.Result.Add(new RouteCombinationFile() { CombinationName = " <br> " + " في الصف " + i.ToString() + " " + ArMsg });
            }
            else
            {
                Result.Result.Add(new RouteCombinationFile() { CombinationName = " <br> " + " at row " + i.ToString() + " " + EngMsg });
            }

            Result.Status = 0;
        }

        private void FillRouteCombination(RouteCombinationVM RouteCombinationVM, Tbl_RouteCombinMaster RouteCombination, int UserID, bool SaveFromFile)
        {
            RouteCombination.Name = RouteCombinationVM.Name;
            RouteCombination.NameEng = RouteCombinationVM.NameEng;
            RouteCombination.DistributorID = RouteCombinationVM.DistributorID;
            RouteCombination.RouteID = RouteCombinationVM.RouteID;
            if (!SaveFromFile)
            {
                RouteCombination.Note = RouteCombinationVM.Note;
            }
            RouteCombination.IsActive = RouteCombinationVM.IsActive;
            RouteCombination.UpdateBy = UserID;
            RouteCombination.LastUpdate = DateTime.Now;


        }

        public void DownloadCurrentStatus(HttpServerUtilityBase Server, FileVM _Result, string Vendor_CompanyID)
        {
            try
            {
                var headerRow = new List<string[]>() { new string[] {"Customer Code", "Route Code", "Distributor Code","Combination Name Eng",  "Combination Name"

                } };

                var Result = DB.Fun_GetRouteCombinationCurrentStatus(Vendor_CompanyID).Select(w => new RouteCombinationCurrentStatus()
                {
                    CombinationName = w.Name,
                    CombinationNameEng = w.NameEng,
                    CustomerCode = w.CustoemrCode,
                    DistributorCode = w.DistCode,
                    RouteCode = w.Code
                }).ToList();

                FileManager.ExportExcel("RouteCombination", headerRow, Result, Server, _Result);
            }
            catch (Exception ex)
            {
                _Result.Status = "Error";
                _Result.FilePath = ErrorUnit.RetriveExceptionMsg(ex);
            }
        }

        class ExcelSheetRouteCombination
        {
            public int Status { get; set; }
            public List<RouteCombinationFile> Result { get; set; }
        }

        class RouteCombinationFile
        {
            public string CombinationName { get; set; }
            public string CombinationNameEng { get; set; }
            public string DistributorCode { get; set; }
            public string RouteCode { get; set; }
            //public string FirstClassName { get; set; }
            //public string SecondClassName { get; set; }
            public string CustomerCode { get; set; }

            public int DistributorID { get; set; }
            public int RouteID { get; set; }
            //public int? FirstClassID { get; set; }
            //public int? SecondClassID { get; set; }
            public int? CustomerDtlID { get; set; }
            public int MasterID { get; set; }
        }

        class RouteCombinationCurrentStatus
        {
            public string CustomerCode { get; set; }
            public string RouteCode { get; set; }
            public string DistributorCode { get; set; }
            public string CombinationNameEng { get; set; }
            public string CombinationName { get; set; }

        }
    }

}