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
    public class RouteUnit
    {
        BayanEntities DB;
        FileManager FileManager = new FileManager();

        public RouteUnit(BayanEntities _Db)
        {
            DB = _Db;
        }
        public RouteUnit()
        {
            DB = new BayanEntities();
        }

        public IQueryable<Tbl_Route> GetAllRoutes(string Vendor_CompanyID)
        {
            return DB.Tbl_Route.Where(w => w.Vendor_CompanyID == Vendor_CompanyID);
        }

        public IQueryable<Tbl_Route> GetAllActiveRoutes(string Vendor_CompanyID)
        {
            return DB.Tbl_Route.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.IsActive == true);
        }

        public IQueryable<Tbl_Route> GetRouteByTerritoryID(int TerritoryID, string Vendor_CompanyID)
        {
            return DB.Tbl_Route.Where(w => w.TerritoryID == TerritoryID && w.Vendor_CompanyID == Vendor_CompanyID);
        }

        public List<RouteVM> GetAllRoutesVM(string Vendor_CompanyID)
        {
            var Qry = DB.V_GetAllRoutesVM.Where(w => w.Vendor_CompanyID == Vendor_CompanyID).Select(w => new RouteVM
            {
                Name = w.Name,
                NameEng = w.NameEng,
                CountryName = w.CountryName,
                CountryNameEng = w.CountryNameEng,
                CityName = w.CityName,
                CityNameEng = w.CityNameEng,
                Note = w.Note,
                ID = w.RouteID,
                IsActive = w.IsActive,
                RouteCode = w.RouteCode
            });

            return Qry.ToList();
        }

        public string Save(RouteVM RouteVM, int UserID, string Vendor_CompanyID)
        {
            try
            {
                var ExistArabic = DB.Tbl_Route.Where(w => w.ID != RouteVM.ID && w.Vendor_CompanyID == Vendor_CompanyID && w.Name == RouteVM.Name).FirstOrDefault();
                if (ExistArabic != null)
                {
                    return CheckUnit.RetriveCorrectMsg("الاسم موجود من قبل", " Name is aready exist befor");
                }

                var ExistEng = DB.Tbl_Route.Where(w => w.ID != RouteVM.ID && w.Vendor_CompanyID == Vendor_CompanyID && w.NameEng == RouteVM.NameEng).FirstOrDefault();
                if (ExistEng != null)
                {
                    return CheckUnit.RetriveCorrectMsg("الاسم باللغة الانجليزية موجود من قبل", "English Name is aready exist befor");
                }

                var ExistCode = DB.Tbl_Route.Where(w => w.ID != RouteVM.ID && w.Vendor_CompanyID == Vendor_CompanyID && w.Code == RouteVM.RouteCode).FirstOrDefault();
                if (ExistCode != null)
                {
                    return CheckUnit.RetriveCorrectMsg("الكود موجود من قبل", "The Code is aready exist befor");
                }

                var OldRoute = DB.Tbl_Route.Where(w => w.ID == RouteVM.ID).FirstOrDefault();
                if (OldRoute != null)
                {
                    FillRoute(RouteVM, OldRoute, UserID);
                }
                else
                {
                    var NewRoute = new Tbl_Route()
                    {
                        CreatedBy = UserID,
                        CreationDate = DateTime.Now,
                        Vendor_CompanyID = Vendor_CompanyID
                    };
                    FillRoute(RouteVM, NewRoute, UserID);
                    DB.Tbl_Route.Add(NewRoute);
                }
                DB.SaveChanges();
                return "Done";
            }
            catch (Exception ex)
            {
                return ErrorUnit.RetriveExceptionMsg(ex);
            }
        }

        private void FillRoute(RouteVM RouteVM, Tbl_Route Route, int UserID)
        {
            Route.Name = RouteVM.Name;
            Route.NameEng = RouteVM.NameEng;
            Route.CountryID = RouteVM.CountryID;
            Route.ProvinceID = RouteVM.ProvinceID;
            Route.CityID = RouteVM.CityID;
            Route.RegionID = RouteVM.RegionID;
            Route.TerritoryID = RouteVM.TerritoryID;
            Route.Note = RouteVM.Note;
            Route.IsActive = RouteVM.IsActive;
            Route.UpdateBy = UserID;
            Route.LastUpdate = DateTime.Now;
            Route.Code = RouteVM.RouteCode;
        }

        public string UploadAndSave(HttpServerUtilityBase Server, HttpRequestBase Request, int UserID, string Vendor_CompanyID)
        {
            ExcelSheetRoute ResultItem = new ExcelSheetRoute();
            string Path = FileManager.CreateNewFileFromRequist(Server, Request, 0, "");
            ExcelSheetRoute Result = new ExcelSheetRoute();
            ResultItem = ReadFromSpecificFile(Result, Path, Vendor_CompanyID);
            string Stats = "Done";
            if (ResultItem.Status != 0)
            {
                List<RouteVM> RouteLst = new List<RouteVM>();
                using (var contxt = new BayanEntities())
                {
                    using (var db_contextTransaction = contxt.Database.BeginTransaction())
                    {
                        try
                        {
                            var Rslt = ResultItem.Result.Select(w => new Tbl_Route()
                            {
                                TerritoryID = w.TerritoryID,
                                CityID = w.CityID,
                                Code = w.RouteCode,
                                CountryID = w.CountryID,
                                CreatedBy = UserID,
                                CreationDate = DateTime.Now,
                                IsActive = true,
                                LastUpdate = DateTime.Now,
                                Name = w.Name,
                                NameEng = w.NameEng,
                                Note = "",
                                ProvinceID = w.ProvinceID,
                                RegionID = w.RegionID,
                                UpdateBy = UserID,
                                Vendor_CompanyID = Vendor_CompanyID
                            });
                            contxt.Tbl_Route.AddRange(Rslt);
                            contxt.SaveChanges();
                            db_contextTransaction.Commit();
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
                foreach (var item in ResultItem.Result)
                {
                    Stats += item.RouteCode + " ";
                }
            }

            return Stats;
        }

        private ExcelSheetRoute ReadFromSpecificFile(ExcelSheetRoute Result, string path, string Vendor_CompanyID)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;

            Result.Status = 1;
            Result.Result = new List<RouteVM>();
            OfficeOpenXml.ExcelWorksheet worksheet;
            int rows, columns;
            FileManager.PrepareForReadingFromSpecificFile(path, out worksheet, out rows, out columns);

            object _RouteCode;
            object _Name;
            object _NameEng;
            object _CountryName;
            object _ProvinceName;
            object _CityName;
            object _RegionName;
            object _TerritoryName;

            string RouteCode;
            string Name;
            string NameEng;
            string CountryName;
            string ProvinceName;
            string CityName;
            string RegionName;
            string TerritoryName;

            for (int i = 3; i <= rows; i++)
            {

                _RouteCode = worksheet.Cells[i, 8].Value;
                _Name = worksheet.Cells[i, 7].Value;
                _NameEng = worksheet.Cells[i, 6].Value;
                _CountryName = worksheet.Cells[i, 5].Value;
                _ProvinceName = worksheet.Cells[i, 4].Value;
                _CityName = worksheet.Cells[i, 3].Value;
                _RegionName = worksheet.Cells[i, 2].Value;
                _TerritoryName = worksheet.Cells[i, 1].Value;


                #region Route Code
                if (_RouteCode == null)
                {
                    InsertError(Result, Lang, i, "Route Code is empty", "كود المسار فارغ");
                    continue;
                }
                RouteCode = _RouteCode.ToString();
                #endregion

                #region Route Name
                if (_Name == null)
                {
                    InsertError(Result, Lang, i, "Route Code is empty", "كود المسار فارغ");
                    continue;
                }
                Name = _Name.ToString();
                #endregion

                #region Route Name Eng
                if (_NameEng == null)
                {
                    InsertError(Result, Lang, i, "Route Eng Name is empty", "اسم المسار باللغة الانجليزية فارغ");
                    continue;
                }
                NameEng = _NameEng.ToString();
                #endregion

                #region CountryName
                if (_CountryName == null)
                {
                    InsertError(Result, Lang, i, "Country Name is empty", "اسم الدولة فارغ");
                    continue;
                }
                CountryName = _CountryName.ToString();
                #endregion

                #region ProvinceName
                if (_ProvinceName == null)
                {
                    InsertError(Result, Lang, i, "Area Name is empty", "اسم الاقليم فارغ");
                    continue;
                }
                ProvinceName = _ProvinceName.ToString();
                #endregion

                #region CityName
                if (_CityName != null)
                {
                    CityName = _CityName.ToString();
                }
                else
                {
                    CityName = null;
                }
                #endregion

                #region RegionName
                if (_RegionName != null)
                {
                    RegionName = _RegionName.ToString();
                }
                else
                {
                    RegionName = null;
                }
                #endregion

                #region TerritoryName
                if (_TerritoryName != null)
                {
                    TerritoryName = _TerritoryName.ToString();
                }
                else
                {
                    TerritoryName = null;
                }
                #endregion

                #region Route Code Check
                var ExistRoute = DB.Tbl_Route.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.Code == RouteCode).FirstOrDefault();
                if (ExistRoute != null)
                {
                    InsertError(Result, Lang, i, "Route Code aready exist before", "كود المسار موجود من قبل");
                    continue;
                }
                #endregion

                #region Route Name Ar Check
                ExistRoute = DB.Tbl_Route.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.Name == Name).FirstOrDefault();
                if (ExistRoute != null)
                {
                    InsertError(Result, Lang, i, "Route Name aready exist before", "اسم المسار موجود من قبل");
                    continue;
                }
                #endregion

                #region Route Name Eng Check
                ExistRoute = DB.Tbl_Route.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.NameEng == NameEng).FirstOrDefault();
                if (ExistRoute != null)
                {
                    InsertError(Result, Lang, i, "Route Eng Name aready exist before", "اسم المسار باللغة الانجليزية موجود من قبل");
                    continue;
                }
                #endregion

                #region Country Check
                var Country = DB.Tbl_CentralizedLockUp.Where(w => w.IsActive == true && (w.NameEng == CountryName || w.Name == CountryName) && w.ParentId == 1).FirstOrDefault();
                if (Country == null)
                {
                    InsertError(Result, Lang, i, "Country Name not exist", "اسم الدولة غير موجود");
                    continue;
                }
                #endregion

                #region Province Check
                var Province = DB.Tbl_CentralizedLockUp.Where(w => w.IsActive == true && (w.NameEng == ProvinceName || w.Name == ProvinceName) && w.ParentId == Country.ID).FirstOrDefault();
                if (Province == null)
                {
                    InsertError(Result, Lang, i, "Province Name not exist or not related to this country", "اسم الاقليم غير موجود او غير مرتبط بهذه الدوله");
                    continue;
                }
                #endregion

                var NewRoute = new RouteVM();
                NewRoute.RouteCode = RouteCode;
                NewRoute.Name = Name;
                NewRoute.NameEng = NameEng;
                NewRoute.CountryID = Country.ID;
                NewRoute.ProvinceID = Province.ID;

                #region City Check
                if (CityName == null) continue;

                var City = DB.Tbl_CentralizedLockUp.Where(w => w.IsActive == true && (w.NameEng == CityName || w.Name == CityName) && w.ParentId == Province.ID).FirstOrDefault();
                if (City == null)
                {
                    InsertError(Result, Lang, i, "City Name not exist or not related to this Province", "اسم المدينة غير موجود او غير مرتبط بهذا الاقليم");
                    continue;
                }
                NewRoute.CityID = City.ID;
                #endregion

                #region Region Check
                if (RegionName == null) continue;

                var Region = DB.Tbl_CentralizedLockUp.Where(w => w.IsActive == true && (w.NameEng == RegionName || w.Name == RegionName) && w.ParentId == City.ID).FirstOrDefault();
                if (Region == null)
                {
                    InsertError(Result, Lang, i, "Unit Name not exist or not related to this city", "اسم المنطقه غير موجود او غير مرتبط بهذه المدينة");
                    continue;
                }
                NewRoute.RegionID = Region.ID;
                #endregion

                #region Teriority Check
                if (TerritoryName == null) continue;

                var Sector = DB.Tbl_CentralizedLockUp.Where(w => w.IsActive == true && (w.NameEng == TerritoryName || w.Name == TerritoryName) && w.ParentId == Region.ID).FirstOrDefault();
                if (Sector == null)
                {
                    InsertError(Result, Lang, i, "Sector Name not exist or not related to this region", "اسم القطاع غير موجود او غير مرتبط بهذه المنطقة");
                    continue;
                }
                NewRoute.TerritoryID = Sector.ID;
                #endregion

                if (Result.Status != 0)
                {
                    Result.Result.Add(NewRoute);
                }
            }
            return Result;
        }

        private void InsertError(ExcelSheetRoute Result, string Lang, int i, string EngMsg, string ArMsg)
        {
            if (Result.Status == 1)
            {
                Result.Result = new List<RouteVM>();
            }

            if (Lang == "ar-SA")
            {
                Result.Result.Add(new RouteVM() { RouteCode = " <br> " + " في الصف " + i.ToString() + " " + ArMsg });
            }
            else
            {
                Result.Result.Add(new RouteVM() { RouteCode = " <br> " + " at row " + i.ToString() + " " + EngMsg });
            }

            Result.Status = 0;
        }

        class ExcelSheetRoute
        {
            public int Status { get; set; }
            public List<RouteVM> Result { get; set; }
        }
    }
}