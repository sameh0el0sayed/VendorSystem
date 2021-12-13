using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using VendorSystem.Models;
using VendorSystem.Models.Model1;
using VendorSystem.ViewModel;

namespace VendorSystem.Repository
{
    public class ProductCodeUnit
    {
        BayanEntities DB;

        FileManager FileManager = new FileManager();

        public ProductCodeUnit() : this(new BayanEntities())
        {
        }

        public ProductCodeUnit(BayanEntities _DB)
        {
            DB = _DB;
        }

        public void DownloadCurrentStatus(string DistributorCode, HttpServerUtilityBase Server, FileVM _Result, string Vendor_CompanyID, bool IsArabicLang)
        {
            try
            {

                var headerRow = new List<string[]>() { new string[] { "Product Name", "Barcode", "InternalCode" } };

                if (IsArabicLang)
                {
                    var Result = DB.V_ProductInternalCode.Where(w => w.DistributorCode == DistributorCode && w.Vendor_CompanyID == Vendor_CompanyID).Select(w => new ProductCodeVM()
                    {
                        Barcode = w.Barcode,
                        InternalCode = w.InternalCode,
                        Name = w.Name
                    }).ToList();

                    FileManager.ExportExcel("ProductCode", headerRow, Result, Server, _Result);
                }
                else
                {
                    var Result = DB.V_ProductInternalCode.Where(w => w.DistributorCode == DistributorCode && w.Vendor_CompanyID == Vendor_CompanyID).Select(w => new ProductCodeVM()
                    {
                        Barcode = w.Barcode,
                        InternalCode = w.InternalCode,
                        Name = w.NameEng
                    }).ToList();

                    FileManager.ExportExcel("ProductCode", headerRow, Result, Server, _Result);
                }
            }
            catch (Exception ex)
            {
                _Result.Status = "Error";
                _Result.FilePath = ErrorUnit.RetriveExceptionMsg(ex);
            }
        }

        public string UploadAndSaveInternalCode(HttpServerUtilityBase Server, HttpRequestBase Request, string DistributorCode, int UserID, string Vendor_CompanyID)
        {
            ExcelSheetProductInernalCode ResultItem = new ExcelSheetProductInernalCode();
            string Path = FileManager.CreateNewFileFromRequist(Server, Request, 0, "");
            ExcelSheetProductInernalCode Result = new ExcelSheetProductInernalCode();
            ResultItem = ReadFromSpecificFile(Result, Path, Vendor_CompanyID);
            string Stats = "Done";
            if (ResultItem.Status != 0)
            {
                List<ProductCodeVM> ProductCodeLst = new List<ProductCodeVM>();
                using (var contxt = new BayanEntities())
                {
                    using (var db_contextTransaction = contxt.Database.BeginTransaction())
                    {
                        try
                        {
                            int index = 1;
                            foreach (var item in ResultItem.Result)
                            {
                                index += 1;
                                var OldObj = contxt.Tbl_ProductVsDistributor.Where(w => w.Barcode == item.Barcode && w.DistributorCode == DistributorCode && w.Vendor_CompanyID == Vendor_CompanyID).FirstOrDefault();
                                var OtherObjs = contxt.Tbl_ProductVsDistributor.Where(w => w.DistributorCode == DistributorCode && w.Vendor_CompanyID == Vendor_CompanyID && (item.InternalCode != "" && w.InternalCode == item.InternalCode)).ToList();

                                if (OtherObjs.Count > 1 || (OtherObjs.Count == 1 && OldObj.Barcode != OtherObjs[0].Barcode))
                                {
                                    if (Stats == "Done")
                                    {
                                        Stats = "";
                                    }
                                    Stats += CheckUnit.RetriveCorrectMsg("  " + System.Environment.NewLine + "  " + "كود المنتج  فى السطر رقم " + index.ToString() + "  موجود من قبل", "  " + System.Environment.NewLine + "  interanl code in row # " + index.ToString() + " exist Befor");
                                    continue;
                                }

                                if (OldObj == null)
                                {
                                    var NewObj = new Tbl_ProductVsDistributor()
                                    {
                                        DistributorCode = DistributorCode,
                                        Barcode = item.Barcode,
                                        Active = true,
                                        CreatedBy = UserID,
                                        CreationDate = DateTime.Now,
                                        InternalCode = item.InternalCode,
                                        LastUpdate = DateTime.Now,
                                        UpdateBy = UserID,
                                        Vendor_CompanyID = Vendor_CompanyID
                                    };

                                    contxt.Tbl_ProductVsDistributor.Add(NewObj);
                                    contxt.SaveChanges();
                                }
                                else if (OldObj.InternalCode != item.InternalCode)
                                {
                                    OldObj.InternalCode = item.InternalCode;
                                    OldObj.LastUpdate = DateTime.Now;
                                    OldObj.UpdateBy = UserID;
                                    contxt.SaveChanges();
                                }
                            }
                            if (Stats == "Done")
                            {
                                db_contextTransaction.Commit();
                            }
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
                    Stats += item.Barcode + System.Environment.NewLine;
                }
            }

            return Stats;
        }

        private ExcelSheetProductInernalCode ReadFromSpecificFile(ExcelSheetProductInernalCode Result, string path, string Vendor_CompanyID)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;

            string Barcode = "";
            string Internalcode = "";
            Result.Status = 1;
            Result.Result = new List<ProductCodeVM>();
            OfficeOpenXml.ExcelWorksheet worksheet;
            int rows, columns;
            FileManager.PrepareForReadingFromSpecificFile(path, out worksheet, out rows, out columns);

            // loop through the worksheet rows and columns
            object _Barcode;
            object _Internalcode;
            for (int i = 2; i <= rows; i++)
            {

                if (worksheet.Cells[i, 2].Value == null && worksheet.Cells[i, 3].Value == null)
                    continue;

                _Barcode = worksheet.Cells[i, 2].Value;
                _Internalcode = worksheet.Cells[i, 3].Value;

                if (_Barcode == null)
                {
                    if (Result.Status == 1)
                    {
                        Result.Result = new List<ProductCodeVM>();
                    }

                    if (Lang == "ar-SA")
                    {
                        Result.Result.Add(new ProductCodeVM() { Barcode = " <br> " + " في الصف " + i.ToString() + "يوجد قيمة فارغة  " });
                    }
                    else
                    {
                        Result.Result.Add(new ProductCodeVM() { Barcode = " <br> " + " at row " + i.ToString() + " there are empty value  " });
                    }


                    Result.Status = 0;
                }
                Barcode = _Barcode.ToString();
                if (_Internalcode == null)
                {
                    Internalcode = "";
                }
                else
                {
                    Internalcode = _Internalcode.ToString();
                }

                var productAtt = DB.Tbl_ProductAttribute.Where(w => w.Barcode == Barcode && w.Vendor_CompanyID == Vendor_CompanyID).FirstOrDefault();
                if (productAtt == null)
                {
                    if (Result.Status == 1)
                    {
                        Result.Result = new List<ProductCodeVM>();
                    }

                    if (Lang == "ar-SA")
                    {
                        Result.Result.Add(new ProductCodeVM() { Barcode = " <br> " + " في الصف " + i.ToString() + "باركود غير صحيح " });
                    }
                    else
                    {
                        Result.Result.Add(new ProductCodeVM() { Barcode = " <br> " + " at row " + i.ToString() + " invalid Barcode " });
                    }

                    Result.Status = 0;
                }

                if (Result.Status != 0)
                {
                    Result.Result.Add(new ProductCodeVM() { Barcode = Barcode, InternalCode = Internalcode });
                }
            }
            return Result;
        }

        public class ExcelSheetProductInernalCode
        {
            public int Status { get; set; }
            public List<ProductCodeVM> Result { get; set; }
        }
        public class ProductCodeVM
        {
            public string Name { get; set; }
            public string Barcode { get; set; }
            public string InternalCode { get; set; }

        }
    }

    

}