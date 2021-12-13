using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendorSystem.Models.Model1;
using VendorSystem.ViewModel;

namespace VendorSystem.Repository
{
    public class ReplenishmentReportUnit
    {
        BayanEntities DB;
        public ReplenishmentReportUnit()
        {
            DB = new BayanEntities();
        }

        public ReplenishmentReportUnit(BayanEntities _DB)
        {
            DB = _DB;
        }
        //sameh code edit
        public void Download_Excel(HttpServerUtilityBase Server, FileVM _Result, DateTime? DateFrom, DateTime? DateTO, int? RegionID, int? TerritoryID, int? RouteID, int? CustDtlID, string Vendor_CompanyID, bool IsArabicLang)
        {
            try
            {
                var headerRow = new List<string[]>() { new string[] { "Company Name", "Store Name", "Last Sales Sync Date", "Last Open Day", "Date From", "Date To",
                    "Replenishment Date", "Orders Count Per Vendor", "Orders Count Per Replenish", "Negative Stock", "Positive Stock", "Net Stock", "Negative Sales",
                    "Positive Sales",  "Net Sales", "Total_Po_Qty", "Total_Po_Return_Qty", "Net Purchase Order", "Negative Forecast", "Positive Forecast", "Net Forecast",
                    "Total_Cost","No of Items in First Order",   "No of Items in Second Order",   "No of Items in Third Order" } };

                if (IsArabicLang)
                {
                    var Result = DB.Fun_GetReplenishmentReportData(RegionID, TerritoryID, RouteID, CustDtlID, DateFrom, DateTO, Vendor_CompanyID)
                        .Select(w => new ReplenishmentReportVM()
                        {
                            DateFrom = w.DateFrom,
                            CompanyName = w.CompanyName,
                            DateTo = w.DateTo,
                            FirstOrder = w.FirstOrder,
                            LastOpenDay = w.LastOpenDay,
                            LastSalesSyncDate = w.LastSalesSyncDate,
                            NegativeForecast = w.NegativeForecast,
                            NegativeSales = w.NegativeSales,
                            NegativeStock = w.NegativeStock,
                            OrdersCount = w.OrdersCount,
                            PositiveForecast = w.PositiveForecast,
                            PositiveSales = w.PositiveSales,
                            PositiveStock = w.PositiveStock,
                            ReplenishmentDate = w.ReplenishmentDate,
                            SecondOrder = w.SecondOrder,
                            StoreName = w.StoreName,
                            ThirdOrder = w.ThirdOrder,
                            Total_Po_Qty = w.Total_Po_Qty,
                            Total_Po_Return_Qty = w.Total_Po_Return_Qty,
                            OrderCountPerRep = w.FirstVisit + w.SecondVisit + w.ThirdVisit,
                            NetStock = w.PositiveStock + w.NegativeStock,
                            NetSales = w.PositiveSales + w.NegativeSales,
                            NetPurchaseOrder = w.Total_Po_Qty - w.Total_Po_Return_Qty,
                            NetForecast = w.NegativeForecast + w.PositiveForecast,
                            Total_Cost = w.Total_Cost
                        }).ToList();

                    new FileManager().ExportExcel("Replenishment Report", headerRow, Result, Server, _Result);
                }
                else
                {
                    var Result = DB.Fun_GetReplenishmentReportData(RegionID, TerritoryID, RouteID, CustDtlID, DateFrom, DateTO, Vendor_CompanyID)
                         .Select(w => new ReplenishmentReportVM()
                         {
                             DateFrom = w.DateFrom,
                             CompanyName = w.CompanyNameEng,
                             DateTo = w.DateTo,
                             FirstOrder = w.FirstOrder,
                             LastOpenDay = w.LastOpenDay,
                             LastSalesSyncDate = w.LastSalesSyncDate,
                             NegativeForecast = w.NegativeForecast,
                             NegativeSales = w.NegativeSales,
                             NegativeStock = w.NegativeStock,
                             OrdersCount = w.OrdersCount,
                             PositiveForecast = w.PositiveForecast,
                             PositiveSales = w.PositiveSales,
                             PositiveStock = w.PositiveStock,
                             ReplenishmentDate = w.ReplenishmentDate,
                             SecondOrder = w.SecondOrder,
                             StoreName = w.StoreNameEng,
                             ThirdOrder = w.ThirdOrder,
                             Total_Po_Qty = w.Total_Po_Qty,
                             Total_Po_Return_Qty = w.Total_Po_Return_Qty,
                             OrderCountPerRep = w.FirstVisit + w.SecondVisit + w.ThirdVisit,
                             NetStock = w.PositiveStock + w.NegativeStock,
                             NetSales = w.PositiveSales + w.NegativeSales,
                             NetPurchaseOrder = w.Total_Po_Qty - w.Total_Po_Return_Qty,
                             NetForecast = w.NegativeForecast + w.PositiveForecast,
                             Total_Cost = w.Total_Cost

                         }).ToList();

                    new FileManager().ExportExcel("Replenishment Report", headerRow, Result, Server, _Result);
                }
            }
            catch (Exception ex)
            {
                _Result.Status = "Error";
                _Result.FilePath = ErrorUnit.RetriveExceptionMsg(ex);
            }
        }
    }
}