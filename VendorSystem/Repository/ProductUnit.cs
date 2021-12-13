using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using VendorSystem.Models;
using VendorSystem.Models.Model1;
using VendorSystem.ViewModel;

namespace VendorSystem.Repository
{
    public class ProductUnit
    {
        BayanEntities DB;
        FileManager FileManager = new FileManager();

        public ProductUnit(BayanEntities _Db)
        {
            DB = _Db;
        }

        public ProductUnit()
        {
            DB = new BayanEntities();
        }

        public IQueryable<Tbl_Product> GetAllProducts(string Vendor_CompanyID)
        {
            return DB.Tbl_Product.Where(w => w.Vendor_CompanyID == Vendor_CompanyID);
        }

        public IQueryable<Tbl_Product> GetAllActiveProducts(string Vendor_CompanyID)
        {
            return DB.Tbl_Product.Where(w => w.IsActive == true && w.Vendor_CompanyID == Vendor_CompanyID);
        }

        public IQueryable<Tbl_ShelfLife> GetAllShelfLife()
        {
            return DB.Tbl_ShelfLife;
        }

        public List<ProductVM> GetAllProductsVM(string Vendor_CompanyID)
        {
            var Qry = (from w in DB.Tbl_Product
                       where w.Vendor_CompanyID == Vendor_CompanyID
                       select new ProductVM
                       {
                           IsActive = w.IsActive,
                           BigBarcode = w.BigBarcode,
                           Name = w.Name,
                           NameEng = w.NameEng,
                           SmallBarcode = w.SmallBarcode,
                           ID = w.ID
                       });

            return Qry.ToList();
        }

        public string Save(ProductVM ProductVM, int UserID, string Vendor_CompanyID)
        {
            using (var contxt = new BayanEntities())
            {
                using (var db_contextTransaction = contxt.Database.BeginTransaction())
                {
                    try
                    {
                        if (ProductVM.BigBarcode == ProductVM.SmallBarcode)
                        {
                            return CheckUnit.RetriveCorrectMsg("يجب ادخال باركود للوحده الكبيره مختلف عن باركود الوحده الصغيرة", "Larg Barcode Must Be Differnt From Small Barcode");
                        }

                        var OtherProducts = contxt.Tbl_Product.Where(w => w.ID != ProductVM.ID && w.Vendor_CompanyID == Vendor_CompanyID && (w.SmallBarcode == ProductVM.SmallBarcode || w.BigBarcode == ProductVM.SmallBarcode || (ProductVM.BigBarcode != null && (w.BigBarcode == ProductVM.BigBarcode || w.SmallBarcode == ProductVM.BigBarcode)))).FirstOrDefault();
                        if (OtherProducts != null)
                        {
                            return CheckUnit.RetriveCorrectMsg("باركود الوحده الكبيره او الصغيره موجود من قبل مع منتج اخر", "Big Barcoe Or Small Barcoe exist befor with another product");
                        }


                        var OldProduct = contxt.Tbl_Product.Where(w => w.ID == ProductVM.ID).FirstOrDefault();


                        if (OldProduct == null)
                        {

                            var NewProduct = new Tbl_Product()
                            {
                                BigBarcode = ProductVM.BigBarcode,
                                CreatedBy = UserID,
                                CreationDate = DateTime.Now,
                                IsLocked = false,
                                Vendor_CompanyID = Vendor_CompanyID
                            };
                            FillProduct(ProductVM, NewProduct, UserID);
                            contxt.Tbl_Product.Add(NewProduct);
                            contxt.SaveChanges();
                            FillProductAttribute(ProductVM, NewProduct, contxt, Vendor_CompanyID);
                            contxt.SaveChanges();
                        }
                        else
                        {
                            if (OldProduct.IsLocked == false && (OldProduct.BigBarcode != ProductVM.BigBarcode || OldProduct.SmallBarcode != ProductVM.SmallBarcode))
                            {
                                int ProductID = OldProduct.ID;
                                var OldData = contxt.Tbl_ProductAttribute.Where(w => w.ProductID == ProductID).ToList();
                                contxt.Tbl_ProductAttribute.RemoveRange(OldData);
                                contxt.SaveChanges();
                                FillProductAttribute(ProductVM, OldProduct, contxt, Vendor_CompanyID);
                            }

                            FillProduct(ProductVM, OldProduct, UserID);
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

        private void FillProductAttribute(ProductVM ProductVM, Tbl_Product NewProduct, BayanEntities Contxt, string Vendor_CompanyID)
        {
            if (ProductVM.BigBarcode == null)
            {
                var SmallProdAttribut = new Tbl_ProductAttribute() { Barcode = ProductVM.SmallBarcode, ChildrenBarcode = null, ProductID = NewProduct.ID, UOM = 0, LastUpdate = DateTime.Now, Vendor_CompanyID = Vendor_CompanyID };
                Contxt.Tbl_ProductAttribute.Add(SmallProdAttribut);
            }
            else
            {
                var BigProdAttribut = new Tbl_ProductAttribute() { Barcode = ProductVM.BigBarcode, ChildrenBarcode = ProductVM.SmallBarcode, ProductID = NewProduct.ID, UOM = 1, LastUpdate = DateTime.Now, Vendor_CompanyID = Vendor_CompanyID };
                var SmallProdAttribut = new Tbl_ProductAttribute() { Barcode = ProductVM.SmallBarcode, ChildrenBarcode = null, ProductID = NewProduct.ID, UOM = 0, LastUpdate = DateTime.Now, Vendor_CompanyID = Vendor_CompanyID };

                Contxt.Tbl_ProductAttribute.Add(BigProdAttribut);
                Contxt.Tbl_ProductAttribute.Add(SmallProdAttribut);
            }
        }

        private void FillProduct(ProductVM ProductVM, Tbl_Product Product, int UserID)
        {
            if (Product.IsLocked == false)
            {
                Product.SmallBarcode = ProductVM.SmallBarcode;
                Product.BigBarcode = ProductVM.BigBarcode;
            }
            Product.FirstClassification = ProductVM.FirstClassification;
            Product.FourthClassification = ProductVM.FourthClassification;
            Product.IsActive = ProductVM.IsActive;
            Product.LastUpdate = DateTime.Now;
            Product.PiecesInCatron = ProductVM.PiecesInCatron;
            Product.Name = ProductVM.Name;
            Product.NameEng = ProductVM.NameEng;
            Product.SecondClassification = ProductVM.SecondClassification;
            Product.ThirdClassification = ProductVM.ThirdClassification;
            Product.UpdatedBy = UserID;
            Product.Weight = ProductVM.Weight;
            Product.ReturnPercentage = ProductVM.ReturnPercentage;
            Product.MinQty = ProductVM.MinQty;
            Product.ShelfLifeID = ProductVM.ShelfLifeID;
        }

        public void DownloadCurrentStatus(HttpServerUtilityBase Server, FileVM _Result, string Vendor_CompanyID, bool IsArabicLang)
        {
            try
            {
                var headerRow = new List<string[]>() { new string[] {
                    "Ar Name   * ", "Eng Name  * ", "Big Barocde", "Small Barocde   * ", "Weight", "Piece In Carton   * ", "First Class   * ", "Second Class",
                    "Third Class",  "Fourth Class", "Return %   * ", "Min Qty %   * ", "Shelf Life   * "
                    } };

                if (IsArabicLang)
                {
                    var Result = DB.V_GetProductCurrentStatus.Where(w => w.Vendor_CompanyID == Vendor_CompanyID).Select(w => new ProductCurrentStatus()
                    {
                        Name = w.Name,
                        BigBarocde = w.BigBarcode,
                        NameEng = w.NameEng,
                        FirstClass = w.FirstClassName,
                        FourthClass = w.FourthClassName,
                        PieceInCarton = w.PiecesInCatron,
                        SecondClass = w.SecondClassName,
                        SmallBarocde = w.SmallBarcode,
                        ThirdClass = w.ThirdClassName,
                        Weight = w.Weight,
                        ReturnPercentage = w.ReturnPercentage,
                        MinQtyPercentage = w.MinQty,
                        ShelfLife = w.ShelfName
                    }).ToList();

                    FileManager.ExportExcel("Products", headerRow, Result, Server, _Result);
                }
                else
                {
                    var Result = DB.V_GetProductCurrentStatus.Where(w => w.Vendor_CompanyID == Vendor_CompanyID).Select(w => new ProductCurrentStatus()
                    {
                        Name = w.Name,
                        BigBarocde = w.BigBarcode,
                        NameEng = w.NameEng,
                        FirstClass = w.FirstClassNameEng,
                        FourthClass = w.FourthClassNameEng,
                        PieceInCarton = w.PiecesInCatron,
                        SecondClass = w.SecondClassNameEng,
                        SmallBarocde = w.SmallBarcode,
                        ThirdClass = w.ThirdClassNameEng,
                        Weight = w.Weight,
                        ReturnPercentage = w.ReturnPercentage,
                        MinQtyPercentage = w.MinQty,
                        ShelfLife = w.ShelfNameEng
                    }).ToList();

                    FileManager.ExportExcel("Products", headerRow, Result, Server, _Result);
                }
            }
            catch (Exception ex)
            {
                _Result.Status = "Error";
                _Result.FilePath = ErrorUnit.RetriveExceptionMsg(ex);
            }
        }

        public string UploadAndSave(HttpServerUtilityBase Server, HttpRequestBase Request, int UserID, string Vendor_CompanyID)
        {
            ExcelSheetProduct ResultItem = new ExcelSheetProduct();
            string Path = FileManager.CreateNewFileFromRequist(Server, Request, 0, "");
            ExcelSheetProduct Result = new ExcelSheetProduct();
            ResultItem = ReadFromSpecificFile(Result, Path, Vendor_CompanyID);
            string Stats = "Done";
            if (ResultItem.Status != 0)
            {
                using (var contxt = new BayanEntities())
                {
                    using (var db_contextTransaction = contxt.Database.BeginTransaction())
                    {
                        try
                        {

                            #region First Class
                            var NewFirstClassLst = ResultItem.Result.Where(w => w.FirstClassID == null).Select(w => new { ParentID = 1, Name = w.FirstClassName }).Distinct().ToList();
                            foreach (var item in NewFirstClassLst)
                            {
                                var NewFirstClass = new LockUp()
                                {
                                    Active = true,
                                    ParentId = item.ParentID,
                                    Createdby = UserID,
                                    CreatedDate = DateTime.Now,
                                    Deletedby = null,
                                    Lastupdate = DateTime.Now,
                                    Name = item.Name,
                                    NameEng = item.Name,
                                    Vendor_CompanyID = Vendor_CompanyID
                                };
                                contxt.LockUps.Add(NewFirstClass);
                                contxt.SaveChanges();

                                var UodateLst = ResultItem.Result.Where(w => w.FirstClassName == item.Name).ToList();
                                foreach (var NewFirst in UodateLst)
                                {
                                    NewFirst.FirstClassID = NewFirstClass.ID;
                                }
                            }
                            #endregion

                            #region Second Class
                            var NewSecondClassLst = ResultItem.Result.Where(w => w.SecondClassID == null).Select(w => new { ParentID = w.FirstClassID, Name = w.SecondClassName }).Distinct().ToList();
                            foreach (var item in NewSecondClassLst)
                            {
                                var NewSecondClass = new LockUp()
                                {
                                    Active = true,
                                    ParentId = item.ParentID,
                                    Createdby = UserID,
                                    CreatedDate = DateTime.Now,
                                    Deletedby = null,
                                    Lastupdate = DateTime.Now,
                                    Name = item.Name,
                                    NameEng = item.Name,
                                    Vendor_CompanyID = Vendor_CompanyID
                                };
                                contxt.LockUps.Add(NewSecondClass);
                                contxt.SaveChanges();

                                var UodateLst = ResultItem.Result.Where(w => w.SecondClassName == item.Name).ToList();
                                foreach (var NewSecond in UodateLst)
                                {
                                    NewSecond.SecondClassID = NewSecondClass.ID;
                                }
                            }
                            #endregion

                            #region Third Class
                            var NewThirdClassLst = ResultItem.Result.Where(w => w.ThirdClassID == null).Select(w => new { ParentID = w.SecondClassID, Name = w.ThirdClassName }).Distinct().ToList();
                            foreach (var item in NewThirdClassLst)
                            {
                                var NewThirdClass = new LockUp()
                                {
                                    Active = true,
                                    ParentId = item.ParentID,
                                    Createdby = UserID,
                                    CreatedDate = DateTime.Now,
                                    Deletedby = null,
                                    Lastupdate = DateTime.Now,
                                    Name = item.Name,
                                    NameEng = item.Name,
                                    Vendor_CompanyID = Vendor_CompanyID
                                };
                                contxt.LockUps.Add(NewThirdClass);
                                contxt.SaveChanges();

                                var UodateLst = ResultItem.Result.Where(w => w.ThirdClassName == item.Name).ToList();
                                foreach (var NewThird in UodateLst)
                                {
                                    NewThird.ThirdClassID = NewThirdClass.ID;
                                }
                            }
                            #endregion

                            #region Fourth Class
                            var NewFourthClassLst = ResultItem.Result.Where(w => w.FourthClassID == null).Select(w => new { ParentID = w.ThirdClassID, Name = w.FourthClassName }).Distinct().ToList();
                            foreach (var item in NewFourthClassLst)
                            {
                                var NewFourthClass = new LockUp()
                                {
                                    Active = true,
                                    ParentId = item.ParentID,
                                    Createdby = UserID,
                                    CreatedDate = DateTime.Now,
                                    Deletedby = null,
                                    Lastupdate = DateTime.Now,
                                    Name = item.Name,
                                    NameEng = item.Name,
                                    Vendor_CompanyID = Vendor_CompanyID
                                };
                                contxt.LockUps.Add(NewFourthClass);
                                contxt.SaveChanges();

                                var UodateLst = ResultItem.Result.Where(w => w.FourthClassName == item.Name).ToList();
                                foreach (var NewFourth in UodateLst)
                                {
                                    NewFourth.FourthClassID = NewFourthClass.ID;
                                }
                            }
                            #endregion

                            foreach (var item in ResultItem.Result)
                            {
                                ProductVM ProductVM = new ProductVM()
                                {
                                    ID = item.ID,
                                    BigBarcode = item.BigBarCode,
                                    FirstClassification = item.FirstClassID,
                                    FourthClassification = item.FourthClassID,
                                    IsActive = true,
                                    Name = item.Name,
                                    NameEng = item.NameEng,
                                    PiecesInCatron = item.PieceInCarton,
                                    SecondClassification = item.SecondClassID,
                                    SmallBarcode = item.SmallBarCode,
                                    ThirdClassification = item.ThirdClassID,
                                    Weight = item.Weight,
                                    ShelfLifeID = item.ShelfLifeID,
                                    MinQty = item.MinQtyPercentage,
                                    ReturnPercentage = item.ReturnPercentage
                                };

                                var OldProduct = contxt.Tbl_Product.Where(w => w.ID == item.ID).FirstOrDefault();
                                if (OldProduct == null)
                                {

                                    var NewProduct = new Tbl_Product()
                                    {
                                        BigBarcode = item.BigBarCode,
                                        CreatedBy = UserID,
                                        CreationDate = DateTime.Now,
                                        IsLocked = false,
                                        Vendor_CompanyID = Vendor_CompanyID
                                    };
                                    FillProduct(new ProductVM { }, NewProduct, UserID);
                                    contxt.Tbl_Product.Add(NewProduct);
                                    contxt.SaveChanges();
                                    FillProductAttribute(ProductVM, NewProduct, contxt, Vendor_CompanyID);
                                    contxt.SaveChanges();
                                }
                                else
                                {
                                    if (OldProduct.IsLocked == false && (OldProduct.BigBarcode != ProductVM.BigBarcode || OldProduct.SmallBarcode != ProductVM.SmallBarcode))
                                    {
                                        int ProductID = OldProduct.ID;
                                        var OldData = contxt.Tbl_ProductAttribute.Where(w => w.ProductID == ProductID).ToList();
                                        contxt.Tbl_ProductAttribute.RemoveRange(OldData);
                                        contxt.SaveChanges();
                                        FillProductAttribute(ProductVM, OldProduct, contxt, Vendor_CompanyID);
                                    }
                                    FillProduct(ProductVM, OldProduct, UserID);
                                    contxt.SaveChanges();
                                }
                            }
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
                    Stats += item.SmallBarCode + " ";
                }
            }

            return Stats;
        }

        private ExcelSheetProduct ReadFromSpecificFile(ExcelSheetProduct Result, string path, string Vendor_CompanyID)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string Lang = currentCulture.Name;

            var ShelfLifeLst = DB.Tbl_ShelfLife.ToList().Select(w => new Tbl_ShelfLife() { ID = w.ID, Name = w.Name, NameEng = w.NameEng.ToLower() }).ToList();
            Tbl_ShelfLife ShelfObj;

            Result.Status = 1;
            Result.Result = new List<ProductFile>();
            OfficeOpenXml.ExcelWorksheet worksheet;
            int rows, columns;
            FileManager.PrepareForReadingFromSpecificFile(path, out worksheet, out rows, out columns);

            object _Name;
            object _NameEng;
            object _BigBarocde;
            object _SmallBarocde;
            object _Weight;
            object _PieceInCarton;
            object _FirstClassName;
            object _SecondClassName;
            object _ThirdClassName;
            object _FourthClassName;
            object _ReturnPercentage;
            object _MinQtyPercentage;
            object _ShelfLife;


            string Name;
            string NameEng;
            string BigBarocde;
            string SmallBarocde;
            decimal? Weight;
            int PieceInCarton;
            string FirstClassName;
            string SecondClassName;
            string ThirdClassName;
            string FourthClassName;
            decimal ReturnPercentage;
            decimal MinQtyPercentage;
            int ShelfLifeID;
            string ShelfLifeName;
            for (int i = 2; i <= rows; i++)
            {

                _Name = worksheet.Cells[i, 1].Value;
                _NameEng = worksheet.Cells[i, 2].Value;
                _BigBarocde = worksheet.Cells[i, 3].Value;
                _SmallBarocde = worksheet.Cells[i, 4].Value;
                _Weight = worksheet.Cells[i, 5].Value;
                _PieceInCarton = worksheet.Cells[i, 6].Value;
                _FirstClassName = worksheet.Cells[i, 7].Value;
                _SecondClassName = worksheet.Cells[i, 8].Value;
                _ThirdClassName = worksheet.Cells[i, 9].Value;
                _FourthClassName = worksheet.Cells[i, 10].Value;
                _ReturnPercentage = worksheet.Cells[i, 11].Value;
                _MinQtyPercentage = worksheet.Cells[i, 12].Value;
                _ShelfLife = worksheet.Cells[i, 13].Value;

                #region Name
                if (_Name == null || _Name.ToString().Trim().Length < 3)
                {
                    InsertError(Result, Lang, i, "Ar Name should be 3 character at least ", "الاسم باللغة العربية يجب ان يكون 3 احرف على الاقل");
                    continue;
                }
                Name = _Name.ToString().Trim();
                #endregion
                #region NameEng
                if (_NameEng == null || _NameEng.ToString().Trim().Length < 3)
                {
                    InsertError(Result, Lang, i, "Eng Name should be 3 character at least ", "الاسم باللغة الانجليزية يجب ان يكون 3 احرف على الاقل");
                    continue;
                }
                NameEng = _NameEng.ToString().Trim();
                #endregion
                #region SmallBarocde
                if (_SmallBarocde == null || _SmallBarocde.ToString().Trim().Length < 3)
                {
                    InsertError(Result, Lang, i, "Small Barocde should be 3 character at least", "باركود الوحدة الصغيرة يجب ان يكون 3 احرف على الاقل");
                    continue;
                }
                SmallBarocde = _SmallBarocde.ToString().Trim();
                #endregion
                #region BigBarocde
                if (_BigBarocde != null)
                {
                    BigBarocde = _BigBarocde.ToString().Trim();
                    if (BigBarocde == SmallBarocde)
                    {
                        InsertError(Result, Lang, i, "big Barocde and small barcode is same", "باركود الوحدة الصغيرة وباركود الوحده الكبيرة متشابهان");
                        continue;
                    }
                    if (BigBarocde.Length < 3)
                    {
                        InsertError(Result, Lang, i, "big Barocde should be null or 3 character at least", "باركود الوحدة الكبيرة يجب ان يكون فارغ او 3 احرف على الاقل");
                        continue;
                    }
                }
                else
                {
                    BigBarocde = null;
                }
                #endregion
                #region Weight
                if (_Weight != null)
                {
                    try
                    {
                        Weight = Convert.ToDecimal(_Weight.ToString());
                        if (Weight <= 0)
                        {
                            InsertError(Result, Lang, i, "Weight Value must be gretaer than zero", "قيمة الوزن  يجب ان تكون اكبر من  صفر");
                            continue;
                        }
                    }
                    catch
                    {
                        InsertError(Result, Lang, i, "Invalide Weight Value", "قيمة الوزن غير صحيحة");
                        continue;
                    }
                }
                else
                {
                    Weight = null;
                }
                #endregion
                #region PieceInCarton
                if (_PieceInCarton == null)
                {
                    InsertError(Result, Lang, i, "Piece In Carton is empty", "معامل التحويل من الوحدة الكبيرة للصغيرة فارغ");
                    continue;
                }
                try
                {
                    PieceInCarton = Convert.ToInt32(_PieceInCarton.ToString());
                    if (PieceInCarton < 1)
                    {
                        InsertError(Result, Lang, i, "Piece In Carton Value must be greater than or equall to one", "قيمة معامل التحويل يجب ان تكون اكبر من او تساوى الواحد");
                        continue;
                    }
                }
                catch
                {
                    InsertError(Result, Lang, i, "Invalide Piece In Carton Value", "قيمة معامل التحويل غير صحيحة");
                    continue;
                }
                #endregion
                #region FirstClass
                if (_FirstClassName == null || _FirstClassName.ToString().Trim().Length < 3)
                {
                    InsertError(Result, Lang, i, "First Classification should be 3 character at least", "التصنيف الاول يجب ان يكون 3 احرف على الاقل");
                    continue;
                }
                FirstClassName = _FirstClassName.ToString();
                #endregion
                #region SecondClass
                if (_SecondClassName != null && _SecondClassName.ToString().Trim().Length >= 3)
                {
                    SecondClassName = _SecondClassName.ToString();
                }
                else
                {
                    SecondClassName = null;
                }
                #endregion
                #region ThirdClass
                if (_ThirdClassName != null && _ThirdClassName.ToString().Trim().Length >= 3)
                {
                    ThirdClassName = _ThirdClassName.ToString();
                }
                else
                {
                    ThirdClassName = null;
                }
                #endregion
                #region FourthClass
                if (_FourthClassName != null && _FourthClassName.ToString().Trim().Length >= 3)
                {
                    FourthClassName = _FourthClassName.ToString();
                }
                else
                {
                    FourthClassName = null;
                }
                #endregion
                #region ReturnPercentage
                if (_ReturnPercentage == null)
                {
                    InsertError(Result, Lang, i, "Return Percentage is empty", "نسبة المرتجعات فارغ");
                    continue;
                }
                try
                {
                    ReturnPercentage = Convert.ToDecimal(_ReturnPercentage.ToString());
                    if (ReturnPercentage < 0)
                    {
                        InsertError(Result, Lang, i, "Return Percentage Value must be greater than or equall to zero", "قيمة نسبة المرتجعات يجب ان تكون اكبر من او تساوى الصفر");
                        continue;
                    }
                }
                catch
                {
                    InsertError(Result, Lang, i, "Invalide Return Percentage Value", "قيمة نسبة المرتجعات غير صحيحة");
                    continue;
                }
                #endregion
                #region MinQtyPercentage
                if (_MinQtyPercentage == null)
                {
                    InsertError(Result, Lang, i, "Min Qty Percentage is empty", "نسبه كميه الحد الادنى فارغ");
                    continue;
                }
                try
                {
                    MinQtyPercentage = Convert.ToDecimal(_MinQtyPercentage.ToString());
                    if (MinQtyPercentage < 0)
                    {
                        InsertError(Result, Lang, i, "Min Qty Percentage Value must be greater than or equall to zero", "قيمة نسبة كميه الحد الادنى يجب ان تكون اكبر من او تساوى الصفر");
                        continue;
                    }
                }
                catch
                {
                    InsertError(Result, Lang, i, "Invalide Min Qty Percentage Value", "قيمة نسبة كميه الحد الادنى غير صحيحة");
                    continue;
                }
                #endregion
                #region ShelfLife
                if (_ShelfLife == null)
                {
                    InsertError(Result, Lang, i, "Shelf Life is empty", "نوع الصلاحية فارغ");
                    continue;
                }
                ShelfLifeName = _ShelfLife.ToString().Trim().ToLower();
                ShelfObj = ShelfLifeLst.Where(w => w.Name == ShelfLifeName || w.NameEng == ShelfLifeName).FirstOrDefault();
                if (ShelfObj != null)
                {
                    ShelfLifeID = ShelfObj.ID;
                }
                else
                {
                    InsertError(Result, Lang, i, "Incorrect Shelf Life", "نوع الصلاحيه غير موجود");
                    continue;
                }
                #endregion
                var ProductFile = new ProductFile()
                {
                    BigBarCode = BigBarocde,
                    SmallBarCode = SmallBarocde,
                    Name = Name,
                    NameEng = NameEng,
                    Weight = Weight,
                    PieceInCarton = PieceInCarton,
                    FirstClassName = FirstClassName,
                    SecondClassName = SecondClassName,
                    ThirdClassName = ThirdClassName,
                    FourthClassName = FourthClassName,
                    ReturnPercentage = ReturnPercentage,
                    MinQtyPercentage = MinQtyPercentage,
                    ShelfLifeID = ShelfLifeID
                };
                if (Result.Status != 0)
                {
                    Result.Result.Add(ProductFile);
                }
            }
            if (Result.Status != 0)
            {
                int i = 2;
                foreach (var item in Result.Result)
                {
                    i++;

                    int ProductID = -1;
                    var ExistProd = DB.Tbl_Product.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.SmallBarcode == item.SmallBarCode).FirstOrDefault();
                    if (ExistProd != null)
                    {
                        ProductID = ExistProd.ID;
                        item.ID = ProductID;
                    }

                    #region Big Barocde Check
                    if (item.BigBarCode != null)
                    {
                        ExistProd = DB.Tbl_Product.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.BigBarcode == item.BigBarCode && w.ID != ProductID).FirstOrDefault();
                        if (ExistProd != null)
                        {
                            InsertError(Result, Lang, i, "Big BarCode aready exist before with another product", "باركود الوحدة الكبيرة موجود من قبل مع منتج اخر");
                            continue;
                        }
                    }
                    #endregion

                    #region Name Ar Check
                    ExistProd = DB.Tbl_Product.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.Name == item.Name && w.ID != ProductID).FirstOrDefault();
                    if (ExistProd != null)
                    {
                        InsertError(Result, Lang, i, "Arabic Name aready exist before with another product", "الاسم باللغة العربية موجود من قبل مع منتج اخر");
                        continue;
                    }
                    #endregion

                    #region Name Eng Check
                    ExistProd = DB.Tbl_Product.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && w.NameEng == item.NameEng && w.ID != ProductID).FirstOrDefault();
                    if (ExistProd != null)
                    {
                        InsertError(Result, Lang, i, "Eng Name aready exist before with another product", "الاسم باللغة الانجليزية موجود من قبل مع منتج اخر");
                        continue;
                    }
                    #endregion


                    #region First Class Check
                    var FirstClassObj = DB.LockUps.Where(w => (w.NameEng == item.FirstClassName || w.Name == item.FirstClassName) && w.Vendor_CompanyID == Vendor_CompanyID).FirstOrDefault();
                    if (FirstClassObj == null)
                    {
                        item.FirstClassID = null;
                    }
                    else
                    {

                        if (FirstClassObj.ParentId != 1)
                        {
                            InsertError(Result, Lang, i, "First Classification Name exist befor but with another classificaton", "اسم التصنيف الاول موجود من قبل ولكن مع تصنيف مختلف");
                            continue;
                        }
                        else if (FirstClassObj.Active == false)
                        {
                            InsertError(Result, Lang, i, "First Classification Not active", " التصنيف الاول غير نشط");
                            continue;
                        }
                        else
                        {
                            item.FirstClassID = FirstClassObj.ID;
                        }
                    }

                    #endregion

                    #region SecondClass Check
                    if (item.SecondClassName == null) continue;

                    var SecondClassObj = DB.LockUps.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && (w.NameEng == item.SecondClassName || w.Name == item.SecondClassName)).FirstOrDefault();
                    if (SecondClassObj == null)
                    {
                        item.SecondClassID = null;
                    }
                    else
                    {
                        if (FirstClassObj == null || SecondClassObj.ParentId != FirstClassObj.ID)
                        {
                            InsertError(Result, Lang, i, "Second Classification Aready exist but with different parent ", "اسم التصنيف الثانى موجود من قبل ولكن مع تصنيف مختلف");
                            continue;
                        }
                        else if (SecondClassObj.Active == false)
                        {
                            InsertError(Result, Lang, i, "Second Classification Not active ", " التصنيف الثانى غير نشط");
                            continue;
                        }
                        else
                        {
                            item.SecondClassID = SecondClassObj.ID;
                        }
                    }
                    #endregion

                    #region ThirdClass Check
                    if (item.ThirdClassName == null) continue;

                    var ThirdClassObj = DB.LockUps.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && (w.NameEng == item.ThirdClassName || w.Name == item.ThirdClassName)).FirstOrDefault();
                    if (ThirdClassObj == null)
                    {
                        item.ThirdClassID = null;
                    }
                    else
                    {
                        if (SecondClassObj == null || ThirdClassObj.ParentId != SecondClassObj.ID)
                        {
                            InsertError(Result, Lang, i, "Third Classification Aready exist but with different parent ", "اسم التصنيف الثالث موجود من قبل ولكن مع تصنيف مختلف");
                            continue;
                        }
                        else if (ThirdClassObj.Active == false)
                        {
                            InsertError(Result, Lang, i, "Third Classification Not active ", " التصنيف الثالث غير نشط");
                            continue;
                        }
                        else
                        {
                            item.ThirdClassID = ThirdClassObj.ID;
                        }

                    }
                    #endregion

                    #region FourthClass Check
                    if (item.FourthClassName == null) continue;
                    var FourthClassObj = DB.LockUps.Where(w => w.Vendor_CompanyID == Vendor_CompanyID && (w.NameEng == item.FourthClassName || w.Name == item.FourthClassName)).FirstOrDefault();
                    if (FourthClassObj == null)
                    {
                        item.FourthClassID = null;
                    }
                    else
                    {
                        if (ThirdClassObj == null || FourthClassObj.ParentId != ThirdClassObj.ID)
                        {
                            InsertError(Result, Lang, i, "Fourth Classification Aready exist but with different parent ", "اسم التصنيف الرابع موجود من قبل ولكن مع تصنيف مختلف");
                            continue;
                        }
                        else if (FourthClassObj.Active == false)
                        {
                            InsertError(Result, Lang, i, "Fourth Classification Not active ", " التصنيف الرابع غير نشط");
                            continue;
                        }
                        else
                        {
                            item.FourthClassID = FourthClassObj.ID;
                        }

                    }
                    #endregion
                }
            }

            return Result;
        }

        private void InsertError(ExcelSheetProduct Result, string Lang, int i, string EngMsg, string ArMsg)
        {
            if (Result.Status == 1)
            {
                Result.Result = new List<ProductFile>();
            }

            if (Lang == "ar-SA")
            {
                Result.Result.Add(new ProductFile() { SmallBarCode = " <br> " + " في الصف " + i.ToString() + " " + ArMsg });
            }
            else
            {
                Result.Result.Add(new ProductFile() { SmallBarCode = " <br> " + " at row " + i.ToString() + " " + EngMsg });
            }

            Result.Status = 0;
        }

        class ExcelSheetProduct
        {
            public int Status { get; set; }
            public List<ProductFile> Result { get; set; }
        }

        class ProductCurrentStatus
        {
            public string Name { get; set; }
            public string NameEng { get; set; }
            public string BigBarocde { get; set; }
            public string SmallBarocde { get; set; }
            public string Weight { get; set; }
            public string PieceInCarton { get; set; }
            public string FirstClass { get; set; }
            public string SecondClass { get; set; }
            public string ThirdClass { get; set; }
            public string FourthClass { get; set; }
            public decimal? ReturnPercentage { get; set; }
            public decimal? MinQtyPercentage { get; set; }
            public string ShelfLife { get; set; }
        }

        class ProductFile
        {
            public int ID { get; set; }
            public int? FourthClassID { get; set; }
            public int? ThirdClassID { get; set; }
            public int? SecondClassID { get; set; }
            public int? FirstClassID { get; set; }
            public int PieceInCarton { get; set; }
            public decimal? Weight { get; set; }
            public string SmallBarCode { get; set; }
            public string BigBarCode { get; set; }
            public string NameEng { get; set; }
            public string Name { get; set; }
            public string FirstClassName { get; set; }
            public string SecondClassName { get; set; }
            public string ThirdClassName { get; set; }
            public string FourthClassName { get; set; }
            public decimal? ReturnPercentage { get; set; }
            public decimal? MinQtyPercentage { get; set; }
            public int? ShelfLifeID { get; set; }
        }

    }

}