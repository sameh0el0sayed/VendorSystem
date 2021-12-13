using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendorSystem.Models.Model1;
using VendorSystem.ViewModel;

namespace VendorSystem.Repository
{
    public class CompensationUnit
    {
        BayanEntities DB;
        public CompensationUnit(BayanEntities _Db)
        {
            DB = _Db;
        }

        public CompensationUnit()
        {
            DB = new BayanEntities();
        }

        public void DownloadCompensationData(int? CustomerID, DateTime DateFrom, DateTime DateTo, HttpServerUtilityBase Server, FileVM _Result, string vendor_CompanyID)
        {
            FileManager FileManager = new FileManager();
            var DT = DateTime.Now;
            DB.Proc_RemoveOld_Rpt_CompensationData(DT.AddDays(-2));
            int OperationNumber = -1;
            DateTime TheNextMonth;
            do
            {
                TheNextMonth = DateFrom.AddMonths(1).AddDays(-(DateFrom.Day));

                if (DateTo < TheNextMonth)
                {
                    TheNextMonth = DateTo;
                }
                Tbl_Rpt_CompensationMstr Mstr = new Tbl_Rpt_CompensationMstr()
                {
                    DateFrom = DateFrom,
                    DateTo = TheNextMonth,
                    LastUpdate = DT,
                    Month = DateFrom.Month,
                    Year = DateFrom.Year,
                    OperationNumber = OperationNumber,
                    Vendor_CompanyID = vendor_CompanyID

                };
                DB.Tbl_Rpt_CompensationMstr.Add(Mstr);
                DB.SaveChanges();

                if (OperationNumber == -1)
                {
                    OperationNumber = Mstr.ID;
                    Mstr.OperationNumber = OperationNumber;
                    DB.Entry(Mstr).State = System.Data.Entity.EntityState.Modified;
                    DB.SaveChanges();
                }
                DB.Proc_Calc_Rpt_Compensation(Mstr.ID, vendor_CompanyID);

                DateFrom = TheNextMonth.AddDays(1);
            } while (DateFrom < DateTo);

            try
            {
                var Result = DB.V_GetCompensationData.Where(w =>
                                                                (CustomerID == null || w.CustID == CustomerID)
                                                                && w.OperationNumber == OperationNumber

                                                          ).Select(w => new
                                                          {
                                                              Year = w.Year,
                                                              Month = w.Month,
                                                              CompanyName = w.CompanyName,
                                                              CompensationPercentage = w.CompensationPercentage,
                                                              CompensationValue = w.CompensationValue,
                                                              CreaatedOrderCount = w.CreaatedOrderCount,
                                                              KPI_Percenatge = w.KPI_Percenatge,
                                                              KPI_Value = w.KPI_Value,
                                                              MonthlyFixedValue = w.MonthlyFixedValue,
                                                              Net = w.Net,
                                                              RecievedOrdersCount = w.RecievedOrdersCount,
                                                              RevenueCompensationPercenatge = w.RevenueCompensationPercenatge,
                                                              RevenueCompensationValue = w.RevenueCompensationValue,
                                                              Revenue_KPI_Percenatge = w.Revenue_KPI_Percenatge,
                                                              Revenue_KPI_Value = w.Revenue_KPI_Value,
                                                              StoreName = w.StoreName,
                                                              TotalCreatedOrderValue = w.TotalCreatedOrderValue,
                                                              TotalRecievedOrderValue = w.TotalRecievedOrderValue,
                                                              TotalRevenueCompensation = w.TotalRevenueCompensation,
                                                              TotalRevenueKPI = w.TotalRevenueKPI,
                                                              DateFrom = w.DateFrom,
                                                              DateTo = w.DateTo,
                                                              CustomerID = w.CustoemrCode,
                                                              RegionName = w.RegionName,
                                                              RouteNumber = w.RoutCode,
                                                              TerritoryName = w.TerritoryName
                                                          }).ToList().Select(w => new CompensationData()
                                                          {
                                                              CalculatedMonth = w.Year + " - " + w.Month.Value.ToString("00"),
                                                              CompanyName = w.CompanyName,
                                                              CompensationPercentage = w.CompensationPercentage,
                                                              CompensationValue = w.CompensationValue,
                                                              CreaatedOrderCount = w.CreaatedOrderCount,
                                                              KPI_Percenatge = w.KPI_Percenatge,
                                                              KPI_Value = w.KPI_Value,
                                                              MonthlyFixedValue = w.MonthlyFixedValue,
                                                              Net = w.Net,
                                                              RecievedOrdersCount = w.RecievedOrdersCount,
                                                              RevenueCompensationPercenatge = w.RevenueCompensationPercenatge,
                                                              RevenueCompensationValue = w.RevenueCompensationValue,
                                                              Revenue_KPI_Percenatge = w.Revenue_KPI_Percenatge,
                                                              Revenue_KPI_Value = w.Revenue_KPI_Value,
                                                              CustomerName = w.StoreName,
                                                              TotalCreatedOrderValue = w.TotalCreatedOrderValue,
                                                              TotalRecievedOrderValue = w.TotalRecievedOrderValue,
                                                              TotalRevenueCompensation = w.TotalRevenueCompensation,
                                                              TotalRevenueKPI = w.TotalRevenueKPI,
                                                              DateFrom = w.DateFrom.Value.ToShortDateString(),
                                                              DateTo = w.DateTo.Value.ToShortDateString(),
                                                              CustomerID = w.CustomerID,
                                                              RegionName = w.RegionName,
                                                              RouteNumber = w.RouteNumber,
                                                              TerritoryName = w.TerritoryName
                                                          }).ToList();


                var test = Result.Select(r => r.CalculatedMonth).ToList();

                var headerRow = new List<string[]>() { new string[] { "Calculated Month", "Date From","Date To", "Customer Name", "Company Name", "Custoemr ID", "Route Number", "Sector Name", "Unit Name",  "Total Recieved Order Value", "Compensation Percentage %",
                                                                      "Compensation Percenatge Value",  "Recieved Orders Count", "Compensation Value", "Compensation_Value Value", "Total Compensation",
                                                                      "Total Created Order Value",  "KPI Percenatge",  "KPI Percenatge Value", "Creaated Order Count", "KPI Value", "KPI_Value Value",
                                                                      "Total KPI Value", "Monthly Fixed Value", "Net" } };
                FileManager.ExportExcel("CompensationData", headerRow, Result, Server, _Result);
            }
            catch (Exception ex)
            {
                _Result.Status = "Error";
                _Result.FilePath = ErrorUnit.RetriveExceptionMsg(ex);
            }
        }
        //Sameh Code
        public void DownloadCompensationGrowthData(int? CustomerID, DateTime DateFrom, DateTime DateTo, HttpServerUtilityBase Server, FileVM _Result, string vendor_CompanyID)
        {
            FileManager FileManager = new FileManager();
            var DT = DateTime.Now;
            DB.Proc_RemoveOld_Rpt_CompensationData(DT.AddDays(-2));
            int OperationNumber = -1;
            DateTime TheNextMonth;
            do
            {
                TheNextMonth = DateFrom.AddMonths(1).AddDays(-(DateFrom.Day));

                if (DateTo < TheNextMonth)
                {
                    TheNextMonth = DateTo;
                }
                Tbl_Rpt_CompensationMstr Mstr = new Tbl_Rpt_CompensationMstr()
                {
                    DateFrom = DateFrom,
                    DateTo = TheNextMonth,
                    LastUpdate = DT,
                    Month = DateFrom.Month,
                    Year = DateFrom.Year,
                    OperationNumber = OperationNumber,
                    Vendor_CompanyID = vendor_CompanyID

                };
                DB.Tbl_Rpt_CompensationMstr.Add(Mstr);
                DB.SaveChanges();

                if (OperationNumber == -1)
                {
                    OperationNumber = Mstr.ID;
                    Mstr.OperationNumber = OperationNumber;
                    DB.Entry(Mstr).State = System.Data.Entity.EntityState.Modified;
                    DB.SaveChanges();
                }
                DB.Proc_Calc_Rpt_Compensation(Mstr.ID, vendor_CompanyID);

                DateFrom = TheNextMonth.AddDays(1);
            } while (DateFrom < DateTo);

            try
            {
                var Result = DB.V_GetCompensationData.Where(w =>
                                                                (CustomerID == null || w.CustID == CustomerID)
                                                                && w.OperationNumber == OperationNumber

                                                          ).OrderBy(m => new { m.CompanyName, m.Year, m.Month }).Select(w => new
                                                          {
                                                              Year = w.Year,
                                                              Month = w.Month,
                                                              CompanyName = w.CompanyName,
                                                              CompensationPercentage = w.CompensationPercentage,
                                                              CompensationValue = w.CompensationValue,
                                                              CreaatedOrderCount = w.CreaatedOrderCount,
                                                              KPI_Percenatge = w.KPI_Percenatge,
                                                              KPI_Value = w.KPI_Value,
                                                              MonthlyFixedValue = w.MonthlyFixedValue,
                                                              Net = w.Net,
                                                              RecievedOrdersCount = w.RecievedOrdersCount,
                                                              RevenueCompensationPercenatge = w.RevenueCompensationPercenatge,
                                                              RevenueCompensationValue = w.RevenueCompensationValue,
                                                              Revenue_KPI_Percenatge = w.Revenue_KPI_Percenatge,
                                                              Revenue_KPI_Value = w.Revenue_KPI_Value,
                                                              StoreName = w.StoreName,
                                                              TotalCreatedOrderValue = w.TotalCreatedOrderValue,
                                                              TotalRecievedOrderValue = w.TotalRecievedOrderValue,
                                                              TotalRevenueCompensation = w.TotalRevenueCompensation,
                                                              TotalRevenueKPI = w.TotalRevenueKPI,
                                                              DateFrom = w.DateFrom,
                                                              DateTo = w.DateTo,
                                                              CustomerID = w.CustoemrCode,
                                                              RegionName = w.RegionName,
                                                              RouteNumber = w.RoutCode,
                                                              TerritoryName = w.TerritoryName,

                                                          }).ToList().Select(w => new CompensationData()
                                                          {
                                                              CalculatedMonth = w.Year + " - " + w.Month.Value.ToString("00"),
                                                              CompanyName = w.CompanyName,
                                                              CompensationPercentage = w.CompensationPercentage,
                                                              CompensationValue = w.CompensationValue,
                                                              CreaatedOrderCount = w.CreaatedOrderCount,
                                                              KPI_Percenatge = w.KPI_Percenatge,
                                                              KPI_Value = w.KPI_Value,
                                                              MonthlyFixedValue = w.MonthlyFixedValue,
                                                              Net = w.Net,
                                                              RecievedOrdersCount = w.RecievedOrdersCount,
                                                              RevenueCompensationPercenatge = w.RevenueCompensationPercenatge,
                                                              RevenueCompensationValue = w.RevenueCompensationValue,
                                                              Revenue_KPI_Percenatge = w.Revenue_KPI_Percenatge,
                                                              Revenue_KPI_Value = w.Revenue_KPI_Value,
                                                              CustomerName = w.StoreName,
                                                              TotalCreatedOrderValue = w.TotalCreatedOrderValue,
                                                              TotalRecievedOrderValue = w.TotalRecievedOrderValue,
                                                              TotalRevenueCompensation = w.TotalRevenueCompensation,
                                                              TotalRevenueKPI = w.TotalRevenueKPI,
                                                              DateFrom = w.DateFrom.Value.ToShortDateString(),
                                                              DateTo = w.DateTo.Value.ToShortDateString(),
                                                              CustomerID = w.CustomerID,
                                                              RegionName = w.RegionName,
                                                              RouteNumber = w.RouteNumber,
                                                              TerritoryName = w.TerritoryName
                                                          }).ToList();

                 
                var newResult = new List<CompensationData>();
                int firstMonth = 0;
                decimal? oldTotalRecievedOrderValue = 0;
               
                for (int i=0;i<Result.Count;i++)
                {
                    if (!newResult.Exists(x => x.CompanyName == Result[i].CompanyName))
                    {
                        foreach (var w in Result)
                        {

                            if (Result[i].CompanyName == w.CompanyName)
                            {
                                if (firstMonth == 0)
                                {
                                    newResult.Add(new CompensationData
                                    {
                                        CalculatedMonth = w.CalculatedMonth,
                                        CompanyName = w.CompanyName,
                                        CompensationPercentage = w.CompensationPercentage,
                                        CompensationValue = w.CompensationValue,
                                        CreaatedOrderCount = w.CreaatedOrderCount,
                                        KPI_Percenatge = w.KPI_Percenatge,
                                        KPI_Value = w.KPI_Value,
                                        MonthlyFixedValue = w.MonthlyFixedValue,
                                        Net = w.Net,
                                        RecievedOrdersCount = w.RecievedOrdersCount,
                                        RevenueCompensationPercenatge = w.RevenueCompensationPercenatge,
                                        RevenueCompensationValue = w.RevenueCompensationValue,
                                        Revenue_KPI_Percenatge = w.Revenue_KPI_Percenatge,
                                        Revenue_KPI_Value = w.Revenue_KPI_Value,
                                        CustomerName = w.CustomerName,
                                        TotalCreatedOrderValue = w.TotalCreatedOrderValue,
                                        TotalRecievedOrderValue = w.TotalRecievedOrderValue,
                                        TotalRevenueCompensation = w.TotalRevenueCompensation,
                                        TotalRevenueKPI = w.TotalRevenueKPI,
                                        DateFrom = w.DateFrom,
                                        DateTo = w.DateTo,
                                        CustomerID = w.CustomerID,
                                        RegionName = w.RegionName,
                                        RouteNumber = w.RouteNumber,
                                        TerritoryName = w.TerritoryName,
                                        GrowthRate = w.TotalRecievedOrderValue
                                    });

                                    oldTotalRecievedOrderValue = w.TotalRecievedOrderValue;
                                }
                                if (firstMonth > 0)
                                {
                                    if (oldTotalRecievedOrderValue == 0) { oldTotalRecievedOrderValue =1; }
                                    newResult.Add(new CompensationData
                                    {
                                        CalculatedMonth = w.CalculatedMonth,
                                        CompanyName = w.CompanyName,
                                        CompensationPercentage = w.CompensationPercentage,
                                        CompensationValue = w.CompensationValue,
                                        CreaatedOrderCount = w.CreaatedOrderCount,
                                        KPI_Percenatge = w.KPI_Percenatge,
                                        KPI_Value = w.KPI_Value,
                                        MonthlyFixedValue = w.MonthlyFixedValue,
                                        Net = w.Net,
                                        RecievedOrdersCount = w.RecievedOrdersCount,
                                        RevenueCompensationPercenatge = w.RevenueCompensationPercenatge,
                                        RevenueCompensationValue = w.RevenueCompensationValue,
                                        Revenue_KPI_Percenatge = w.Revenue_KPI_Percenatge,
                                        Revenue_KPI_Value = w.Revenue_KPI_Value,
                                        CustomerName = w.CustomerName,
                                        TotalCreatedOrderValue = w.TotalCreatedOrderValue,
                                        TotalRecievedOrderValue = w.TotalRecievedOrderValue,
                                        TotalRevenueCompensation = w.TotalRevenueCompensation,
                                        TotalRevenueKPI = w.TotalRevenueKPI,
                                        DateFrom = w.DateFrom,
                                        DateTo = w.DateTo,
                                        CustomerID = w.CustomerID,
                                        RegionName = w.RegionName,
                                        RouteNumber = w.RouteNumber,
                                        TerritoryName = w.TerritoryName,
                                        GrowthRate = ((w.TotalRecievedOrderValue - oldTotalRecievedOrderValue) / oldTotalRecievedOrderValue)
                                    });
                                    oldTotalRecievedOrderValue = w.TotalRecievedOrderValue;
                                }

                                firstMonth += 1;

                            }
                            else
                            {
                                oldTotalRecievedOrderValue = 0;
                                firstMonth = 0;

                            }



                        }
                    }
                }

                var headerRow = new List<string[]>() { new string[] { "Calculated Month", "Date From","Date To", "Customer Name", "Company Name", "Custoemr ID", "Route Number", "Sector Name", "Unit Name",  "Total Recieved Order Value", "Compensation Percentage %",
                                                                      "Compensation Percenatge Value",  "Recieved Orders Count", "Compensation Value", "Compensation_Value Value", "Total Compensation",
                                                                      "Total Created Order Value",  "KPI Percenatge",  "KPI Percenatge Value", "Creaated Order Count", "KPI Value", "KPI_Value Value",
                                                                      "Total KPI Value", "Monthly Fixed Value", "Net","GrowthRate" } };
                FileManager.ExportExcel("CompensationGrowthData", headerRow, newResult, Server, _Result);
            }
            catch (Exception ex)
            {
                _Result.Status = "Error";
                _Result.FilePath = ErrorUnit.RetriveExceptionMsg(ex);
            }
        }

        private DateTime RetriveTheEndOfMonth(DateTime _DF)
        {
            return _DF.AddMonths(1).AddDays(-(_DF.Day));
        }

        private int Insert_New_Month_Into_Customer_Compensation_Tables(DateTime DF, DateTime DT, BayanEntities DB, string Vendor_CompanyID)
        {
            #region Mstr
            var MstrObj = new Tbl_CustomerCompensationMstr()
            {
                DateFrom = DF,
                DateTo = DT,
                LastUpdate = DateTime.Now,
                Month = DF.Month,
                Year = DF.Year,
                Vendor_CompanyID = Vendor_CompanyID
            };

            DB.Tbl_CustomerCompensationMstr.Add(MstrObj);
            DB.SaveChanges();
            #endregion

            #region Dtl
            CustomerUnit CustomerUnit = new CustomerUnit(DB);

            var Customers = CustomerUnit.GetAllActiveCustomersVM(Vendor_CompanyID);
            var CustCompensationDtlLst = Customers.Select(w => new Tbl_CustomerCompensationDetail()
            {
                CompanyID = w.CompanyID,
                CustDtl_ID = w.ID,
                DateFrom = DF,
                LastUpdate = DateTime.Now,
                MasterID = MstrObj.ID,
                Net = 0,
                StoreID = w.StoreID,
                Vendor_CompanyID = Vendor_CompanyID
            }).ToList();
            DB.Tbl_CustomerCompensationDetail.AddRange(CustCompensationDtlLst);
            DB.SaveChanges();
            #endregion

            var Un_Using_Customers = DB.Tbl_CustomerDtl.Where(w => w.Active == true && w.RebateAmount == null).ToList();
            foreach (var item in Un_Using_Customers)
            {
                item.RebateAmount = 0;
            }
            DB.SaveChanges();

            return MstrObj.ID;
        }

        private void Update_Exist_Month_Into_Customer_Compensation_Tables(DateTime? DF, DateTime? DT, BayanEntities DB, Tbl_CustomerCompensationMstr MstrObj, string Vendor_CompanyID)
        {

            CustomerUnit CustomerUnit = new CustomerUnit(DB);
            MstrObj.DateTo = DT;
            MstrObj.LastUpdate = DateTime.Now;
            DB.SaveChanges();

            #region Dtl
            var Un_Using_Customers = DB.Tbl_CustomerDtl.Where(w => w.Active == true && w.RebateAmount == null).ToList();

            var Customers = CustomerUnit.GetAllActiveCustomersVM(Vendor_CompanyID);
            var CustCompensationDtlLst = Customers.Select(w => new Tbl_CustomerCompensationDetail()
            {
                CompanyID = w.CompanyID,
                CustDtl_ID = w.ID,
                DateFrom = DF,
                LastUpdate = DateTime.Now,
                MasterID = MstrObj.ID,
                Net = 0,
                StoreID = w.StoreID,
                Vendor_CompanyID = Vendor_CompanyID
            }).ToList();
            DB.Tbl_CustomerCompensationDetail.AddRange(CustCompensationDtlLst);
            DB.SaveChanges();
            #endregion

            foreach (var item in Un_Using_Customers)
            {
                item.RebateAmount = 0;
            }
            DB.SaveChanges();

        }

        private void UpdateCustomerLastUpdate(BayanEntities DB)
        {
            var AllCustomer = DB.Tbl_CustomerDtl.Where(w => w.Active == true).ToList();
            var Date = DateTime.Now;
            foreach (var item in AllCustomer)
            {
                item.LastUpdate = Date;
            }
            DB.SaveChanges();
        }

        public void CalcCustomerCompensation(string Vendor_CompanyID)
        {
            using (var Contxt = new BayanEntities())
            {
                using (var db_contextTransaction = Contxt.Database.BeginTransaction())
                {
                    try
                    {
                        int MastrID = 0;
                        var DT = new DateTime(2020, 11, 30);

                        var MaxExistMstrObj = Contxt.Tbl_CustomerCompensationMstr.OrderByDescending(w => w.ID).FirstOrDefault();
                        if (MaxExistMstrObj == null)
                        {
                            var DF = new DateTime(2020, 11, 1);
                            MastrID = Insert_New_Month_Into_Customer_Compensation_Tables(DF, DT, Contxt, Vendor_CompanyID);
                        }
                        else
                        {

                            DT = MaxExistMstrObj.DateTo.Value.AddDays(1);
                            if (DT.Date == DateTime.Now.Date)
                            {
                                return;
                            }

                            if (DT.Month == MaxExistMstrObj.DateTo.Value.Month)
                            {
                                MastrID = MaxExistMstrObj.ID;
                                Update_Exist_Month_Into_Customer_Compensation_Tables(MaxExistMstrObj.DateFrom, DT, Contxt, MaxExistMstrObj, Vendor_CompanyID);
                            }
                            else
                            {
                                var DF = DT;

                                DT = RetriveTheEndOfMonth(DF);
                                if (DT.Date > DateTime.Now.AddDays(-1).Date)
                                {
                                    DT = DateTime.Now.AddDays(-1).Date;
                                }
                                MastrID = Insert_New_Month_Into_Customer_Compensation_Tables(DF, DT, Contxt, Vendor_CompanyID);
                            }
                        }

                        UpdateCustomerLastUpdate(Contxt);
                        db_contextTransaction.Commit();
                        Contxt.Proc_Calc_CustomerCompensation(MastrID, DT);
                        Contxt.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        db_contextTransaction.Rollback();
                        ErrorUnit.InsertStatus("CalcCustomerCompensation Internal: ( ", Vendor_CompanyID + " ) " + ErrorUnit.RetriveExceptionMsg(ex));
                    }

                }
            }
        }
    }

    public class CompensationData
    {
        public string CalculatedMonth { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
        public string CustomerID { get; set; }
        public string RouteNumber { get; set; }
        public string RegionName { get; set; }
        public string TerritoryName { get; set; }
        public decimal? TotalRecievedOrderValue { get; set; }
        public decimal? CompensationPercentage { get; set; }
        public decimal? RevenueCompensationPercenatge { get; set; }
        public Nullable<int> RecievedOrdersCount { get; set; }
        public decimal? CompensationValue { get; set; }
        public decimal? RevenueCompensationValue { get; set; }
        public decimal? TotalRevenueCompensation { get; set; }
        public decimal? TotalCreatedOrderValue { get; set; }
        public decimal? KPI_Percenatge { get; set; }
        public decimal? Revenue_KPI_Percenatge { get; set; }
        public Nullable<int> CreaatedOrderCount { get; set; }
        public decimal? KPI_Value { get; set; }
        public decimal? Revenue_KPI_Value { get; set; }
        public decimal? TotalRevenueKPI { get; set; }
        public decimal? MonthlyFixedValue { get; set; }
        public decimal? Net { get; set; }
        public decimal? GrowthRate { get; set; }

    }
}