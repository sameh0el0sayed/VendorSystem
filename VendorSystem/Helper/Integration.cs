using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using VendorSystem.Models.Model1;
using VendorSystem.Models.Model2;
using VendorSystem.Repository;
using VendorSystem.ViewModel;

namespace VendorSystem.Helper
{
    public class Integration
    {
        string Pass;
        public Integration(string _Pass)
        {
            Pass = _Pass;
        }

        public void UploadData()
        {
            try
            {
                BayanEntities DB = new BayanEntities();

                try { UploadProductAndProductAttAndProdVsVendor(); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__UploadProductAndProductAttAndProdVsVendor", ErrorUnit.RetriveExceptionMsg(ex)); }
                try { UploadProductVsDistributor(); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__UploadProductVsDistributor", ErrorUnit.RetriveExceptionMsg(ex)); }
                try { UploadDistributorVsVendor(); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__UploadDistributorVsVendor", ErrorUnit.RetriveExceptionMsg(ex)); }

                var VendorLst = DB.Tbl_Company.Where(w => w.IsActive == true).ToList();
                foreach (var item in VendorLst)
                {
                    try { new CompensationUnit().CalcCustomerCompensation(item.Vendor_CompanyID); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__CalcCustomerCompensation: ( ", item + " ) " + ErrorUnit.RetriveExceptionMsg(ex)); }
                }
                try { UploadCustomerRebateAmount(); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__UploadCustomerRebateAmount", ErrorUnit.RetriveExceptionMsg(ex)); }
            }
            catch (Exception ex)
            { ErrorUnit.InsertStatus("Error__UploadData", ErrorUnit.RetriveExceptionMsg(ex)); }

        }
        private void UploadProductAndProductAttAndProdVsVendor()
        {
            BayanEntities DB = new BayanEntities();
            var _LastUpdate = DateTime.Now.AddYears(-50);

            // We use this because of  support team can change in product table  in centalized database
            var ProductLastUpdate = DB.Tbl_LastUpdateFromCenteralDB.Where(a => a.TableName == "Product").FirstOrDefault();
            if (ProductLastUpdate != null)
            {
                _LastUpdate = ProductLastUpdate.LastUpdate.Value.AddSeconds(1);
            }
            else
            {
                DB.Tbl_LastUpdateFromCenteralDB.Add(new Tbl_LastUpdateFromCenteralDB() { LastUpdate = _LastUpdate, TableName = "Product" });
                DB.SaveChanges();
            }
            var Result = DB.Tbl_Product.Where(w => w.LastUpdate > _LastUpdate).OrderBy(w => w.LastUpdate).ToList();
            _LastUpdate = DateTime.Now;

            using (var CenterDB = new CentralizedEntities())
            {
                using (var db_contextTransaction = CenterDB.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in Result)
                        {
                            var ExistProduct = CenterDB.Products.Where(w => w.Barcode == item.SmallBarcode).FirstOrDefault();
                            if (ExistProduct != null)
                            {
                                ExistProduct.PiecesInCatron = item.PiecesInCatron;
                                ExistProduct.Weight = item.Weight;
                                ExistProduct.Lastupdate = item.LastUpdate;
                                CenterDB.SaveChanges();

                            }
                            else
                            {
                                #region Product
                                var Product = new Product()
                                {
                                    Active = 1,
                                    Barcode = item.SmallBarcode,
                                    CatronInBox = 1,
                                    CreatedDate = DateTime.Now,
                                    IsNegativeSales = false,
                                    IsReplenishment = true,
                                    IsSupplierOffer = false,
                                    IsWeight = false,
                                    ItemTypeID = 1,
                                    Lastupdate = DateTime.Now,
                                    OldBarcode = item.BigBarcode,
                                    Pdt_Type = 0,
                                    PiecesInCatron = item.PiecesInCatron,
                                    Pos_Name = item.Name,
                                    Product_Name = item.Name,
                                    Product_NameEn = item.NameEng,
                                    Receipt_Name = item.Name,
                                    Serial_Num = item.SmallBarcode,
                                    Updated = 1,
                                    Weight = item.Weight,
                                    CompanyID = "cnt"
                                };

                                CenterDB.Products.Add(Product);
                                CenterDB.SaveChanges();
                                #endregion

                                #region SmallProductAttribut
                                var SmallProd = new ProductAttribute()
                                {
                                    Active = 1,
                                    BarCode = item.SmallBarcode,
                                    CreatedDate = DateTime.Now,
                                    Lastupdate = DateTime.Now,
                                    OldBarCode = item.SmallBarcode,
                                    ParentId = null,
                                    Product_Id = Product.Product_Id,
                                    UOM = 0,
                                    Updated = 1,
                                    CompanyID = "cnt"
                                };
                                CenterDB.ProductAttributes.Add(SmallProd);
                                CenterDB.SaveChanges();
                                #endregion

                                #region BigProductAttribut
                                if (item.BigBarcode != null)
                                {
                                    var BigProd = new ProductAttribute()
                                    {
                                        Active = 1,
                                        BarCode = item.BigBarcode,
                                        CreatedDate = DateTime.Now,
                                        Lastupdate = DateTime.Now,
                                        OldBarCode = item.BigBarcode,
                                        ParentId = SmallProd.Seq,
                                        Product_Id = Product.Product_Id,
                                        UOM = 1,
                                        Updated = 1,
                                        CompanyID = "cnt"
                                    };
                                    CenterDB.ProductAttributes.Add(BigProd);
                                    CenterDB.SaveChanges();

                                }
                                #endregion
                            }

                            var OldSmallProductVendor = CenterDB.ProductVsVendors.Where(w => w.VendorCode == item.Vendor_CompanyID && w.ProductAttrBarcode == item.SmallBarcode).FirstOrDefault();
                            if (OldSmallProductVendor != null)
                            {
                                OldSmallProductVendor.Active = item.IsActive; OldSmallProductVendor.LastUpdate = DateTime.Now; CenterDB.SaveChanges();
                            }
                            else
                            {
                                CenterDB.ProductVsVendors.Add(new ProductVsVendor()
                                {
                                    Active = item.IsActive,
                                    LastUpdate = DateTime.Now,
                                    VendorCode = item.Vendor_CompanyID,
                                    ProductAttrBarcode = item.SmallBarcode,
                                    CompanyID = "cnt"
                                });
                                CenterDB.SaveChanges();
                            }

                            if (item.BigBarcode != null)
                            {
                                var OldBigProductVendor = CenterDB.ProductVsVendors.Where(w => w.VendorCode == item.Vendor_CompanyID && w.ProductAttrBarcode == item.BigBarcode).FirstOrDefault();
                                if (OldBigProductVendor != null) { OldBigProductVendor.Active = item.IsActive; OldBigProductVendor.LastUpdate = DateTime.Now; CenterDB.SaveChanges(); }
                                else
                                {
                                    CenterDB.ProductVsVendors.Add(new ProductVsVendor()
                                    {
                                        Active = item.IsActive,
                                        LastUpdate = DateTime.Now,
                                        VendorCode = item.Vendor_CompanyID,
                                        ProductAttrBarcode = item.BigBarcode,
                                        CompanyID = "cnt"
                                    });
                                    CenterDB.SaveChanges();
                                }
                            }

                            item.IsLocked = true;
                        }
                        db_contextTransaction.Commit();
                        DB.SaveChanges();

                        ProductLastUpdate = DB.Tbl_LastUpdateFromCenteralDB.Where(a => a.TableName == "Product").FirstOrDefault();
                        if (ProductLastUpdate != null)
                        {
                            ProductLastUpdate.LastUpdate = _LastUpdate;
                            DB.SaveChanges();
                        }

                    }

                    catch (Exception ex)
                    {
                        db_contextTransaction.Rollback();

                        ErrorUnit.InsertStatus("Error__UploadProductAndProductAttAndProdVsVendor_Internal", ErrorUnit.RetriveExceptionMsg(ex));
                    }
                }
            }
        }
        private void UploadProductVsDistributor()
        {
            BayanEntities DB = new BayanEntities();

            var _LastUpdate = DateTime.Now.AddYears(-50);

            // We use this because of  more than on DistributorCode in centalized database
            var ProductVsDistributorLastUpdate = DB.Tbl_LastUpdateFromCenteralDB.Where(a => a.TableName == "ProdVsDist").FirstOrDefault();
            if (ProductVsDistributorLastUpdate != null)
            {
                _LastUpdate = ProductVsDistributorLastUpdate.LastUpdate.Value.AddSeconds(1);
            }
            else
            {
                DB.Tbl_LastUpdateFromCenteralDB.Add(new Tbl_LastUpdateFromCenteralDB()
                {
                    LastUpdate = _LastUpdate,
                    TableName = "ProdVsDist"
                });
                DB.SaveChanges();
            }
            var Result = DB.Tbl_ProductVsDistributor.Where(w => w.LastUpdate > _LastUpdate).OrderBy(w => w.LastUpdate).ToList();
            _LastUpdate = DateTime.Now;

            using (var CenterDB = new CentralizedEntities())
            {
                using (var db_contextTransaction = CenterDB.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in Result)
                        {

                            var ExistObj = CenterDB.ProductVsDistributors.Where(w => w.ProductAttrBarcode == item.Barcode && w.PartnerCode == item.DistributorCode).FirstOrDefault();
                            if (ExistObj != null)
                            {
                                ExistObj.Active = item.Active;
                                ExistObj.InternalCode = item.InternalCode;
                                ExistObj.LastUpdate = DateTime.Now;
                                CenterDB.SaveChanges();
                            }
                            else
                            {
                                var ProductVsDistributo = new ProductVsDistributor()
                                {
                                    Active = item.Active,
                                    ProductAttrBarcode = item.Barcode,
                                    InternalCode = item.InternalCode,
                                    LastUpdate = DateTime.Now,
                                    PartnerCode = item.DistributorCode,
                                    CompanyID = "cnt"
                                };

                                CenterDB.ProductVsDistributors.Add(ProductVsDistributo);
                                CenterDB.SaveChanges();
                            }
                        }

                        db_contextTransaction.Commit();
                        ProductVsDistributorLastUpdate = DB.Tbl_LastUpdateFromCenteralDB.Where(a => a.TableName == "ProdVsDist").FirstOrDefault();
                        if (ProductVsDistributorLastUpdate != null)
                        {
                            ProductVsDistributorLastUpdate.LastUpdate = _LastUpdate;
                            DB.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        db_contextTransaction.Rollback();

                        ErrorUnit.InsertStatus("Error__UploadProductVsDistributor_Internal", ErrorUnit.RetriveExceptionMsg(ex));
                    }
                }
            }
        }
        private void UploadDistributorVsVendor()
        {
            BayanEntities DB = new BayanEntities();
            var Cntr = new CentralizedEntities();

            var _LastUpdate = DateTime.Now.AddYears(-50);

            var DistributorVsVendorsLastUpdate = Cntr.DistributorVsVendors.OrderByDescending(w => w.LastUpdate).FirstOrDefault();
            if (DistributorVsVendorsLastUpdate != null)
            {
                _LastUpdate = DistributorVsVendorsLastUpdate.LastUpdate.Value.AddSeconds(1);
            }

            var Result = DB.Tbl_Distributor.Where(w => w.LastUpdate > _LastUpdate).OrderBy(w => w.LastUpdate).ToList();
            using (var CenterDB = new CentralizedEntities())
            {
                using (var db_contextTransaction = CenterDB.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in Result)
                        {
                            var ExistObj = CenterDB.DistributorVsVendors.Where(w => w.PartnerCode == item.Code && w.VendorCompanyCode == item.Vendor_CompanyID).FirstOrDefault();
                            if (ExistObj != null)
                            {
                                ExistObj.IsActive = item.IsActive;
                                ExistObj.LastUpdate = DateTime.Now;
                                CenterDB.SaveChanges();
                            }
                            else
                            {
                                var DistributorVsVendor = new DistributorVsVendor()
                                {
                                    IsActive = item.IsActive,
                                    PartnerCode = item.Code,
                                    VendorCompanyCode = item.Vendor_CompanyID,
                                    CreationDate = DateTime.Now,
                                    LastUpdate = DateTime.Now
                                };
                                CenterDB.DistributorVsVendors.Add(DistributorVsVendor);
                                CenterDB.SaveChanges();
                            }
                        }

                        db_contextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        db_contextTransaction.Rollback();
                        ErrorUnit.InsertStatus("Error__UploadDistributorVsVendor_Internal", ErrorUnit.RetriveExceptionMsg(ex));
                    }
                }
            }
        }
        private void UploadCustomerRebateAmount()
        {
            BayanEntities DB = new BayanEntities();
            var Cntr = new CentralizedEntities();

            var _LastUpdate = DateTime.Now.AddYears(-50);
            var CustomerRebateAmountsLastUpdate = Cntr.CustomerRebateAmounts.OrderByDescending(w => w.LastUpdate).FirstOrDefault();

            if (CustomerRebateAmountsLastUpdate != null)
            {
                _LastUpdate = CustomerRebateAmountsLastUpdate.LastUpdate.Value.AddSeconds(1);
            }

            var Result = (from CustMstr in DB.Tbl_CustomerMaster
                          from CustDtl in DB.Tbl_CustomerDtl
                          where CustDtl.CustomerMasterID == CustMstr.ID
                          && CustDtl.Active == true
                          && CustDtl.LastUpdate > _LastUpdate
                          select new
                          {
                              StoreID = CustMstr.StoreID,
                              CompanyID = CustMstr.CompanyID,
                              RebateAmount = CustDtl.RebateAmount,
                              Vendor_CompanyID = CustDtl.Vendor_CompanyID
                          }).ToList();


            using (var CenterDB = new CentralizedEntities())
            {
                using (var db_contextTransaction = CenterDB.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in Result)
                        {
                            var ExistObj = CenterDB.CustomerRebateAmounts.Where(w => w.StoreID == item.StoreID && w.CompanyID == item.CompanyID && w.VendorCompanyCode == item.Vendor_CompanyID).FirstOrDefault();
                            if (ExistObj != null)
                            {
                                ExistObj.OldRebateAmount = ExistObj.NewRebateAmount;
                                ExistObj.NewRebateAmount = item.RebateAmount;
                                ExistObj.LastUpdate = DateTime.Now;
                                CenterDB.SaveChanges();
                            }
                            else
                            {
                                var CustomerRebateAmount = new CustomerRebateAmount()
                                {
                                    LastUpdate = DateTime.Now,
                                    OldRebateAmount = 0,
                                    NewRebateAmount = item.RebateAmount,
                                    VendorCompanyCode = item.Vendor_CompanyID,
                                    CompanyID = item.CompanyID,
                                    Active = true,
                                    StoreID = item.StoreID
                                };
                                CenterDB.CustomerRebateAmounts.Add(CustomerRebateAmount);
                                CenterDB.SaveChanges();
                            }
                        }
                        db_contextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        db_contextTransaction.Rollback();
                        ErrorUnit.InsertStatus("Error__UploadCustomerRebateAmount_Internal", ErrorUnit.RetriveExceptionMsg(ex));
                    }
                }
            }
        }


        public void DownloadAllData()
        {
            try
            {
                var context = new BayanEntities();
                var VendorIdLst = context.Tbl_Company.Where(w => w.IsActive == true).Select(w => w.Vendor_CompanyID).ToList();
                var Customers = context.Tbl_CustomerMaster.Where(w => w.IsActive == true).ToList();

                try { DownloadCustomer(); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__DownloadCustomer", ErrorUnit.RetriveExceptionMsg(ex)); }
                try { DownloadCustomer_CustomerCode(); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__DownloadCustomer", ErrorUnit.RetriveExceptionMsg(ex)); }


                try { GetReplenishmentMstr(Customers, Pass); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__GetReplenishmentMstr", ErrorUnit.RetriveExceptionMsg(ex)); }

                foreach (var item in VendorIdLst)
                {
                    try { GetReplenishmentDetails(Customers, Pass, item); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__GetReplenishmentDetails", ErrorUnit.RetriveExceptionMsg(ex)); }

                    try { GetOrderManagementMstr(Customers, Pass, item); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__GetOrderManagementMstr", ErrorUnit.RetriveExceptionMsg(ex)); }
                    try { GetOrderManagementDetails(Customers, Pass, item); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__GetOrderManagementDetails", ErrorUnit.RetriveExceptionMsg(ex)); }

                }

                try { DownloadCentralizedLookUp(); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__DownloadCentralizedLookUp", ErrorUnit.RetriveExceptionMsg(ex)); }
                try { DownloadCustomer(); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__DownloadCustomer", ErrorUnit.RetriveExceptionMsg(ex)); }
                try { DownloadCustomer_CustomerCode(); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__DownloadCustomer", ErrorUnit.RetriveExceptionMsg(ex)); }

            }
            catch (Exception ex)
            { ErrorUnit.InsertStatus("Error__DownloadAllData", ErrorUnit.RetriveExceptionMsg(ex)); }

        }
        private void GetReplenishmentMstr(List<Tbl_CustomerMaster> _Customers, string _Password)
        {
            var Customers = _Customers.Select(w => new { URL = w.URL, CompanyID = w.CompanyID }).Distinct().ToList();
            string Status = "";
            foreach (var Customer in Customers)
            {

                try
                {
                    Thread.Sleep(500);
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(Customer.URL);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var context = new BayanEntities();

                    var LastRecord = context.Replenish_Master.Where(w => w.Company_Id == Customer.CompanyID).OrderByDescending(w => w.LastUpdate).FirstOrDefault();
                    var _LastUpdate = DateTime.Now.AddYears(-50);
                    if (LastRecord != null)
                        _LastUpdate = LastRecord.LastUpdate.Value.AddSeconds(1);

                    var Response = client.PostAsJsonAsync(
                   "VendorSync/RetriveReplenishMasterData", new
                   {
                       LastUpdate = _LastUpdate,
                       Password = _Password
                   }).Result;

                    var Result = Response.Content.ReadAsAsync<List<Replenish_MasterVM>>().Result.OrderBy(w => w.LastUpdate).ToList();
                    foreach (var RepMstrItem in Result)
                    {

                        var OldObj = context.Replenish_Master.Where(w => w.Company_Id == RepMstrItem.Company_Id && w.MarketID == RepMstrItem.Id).FirstOrDefault();
                        if (OldObj == null)
                        {
                            var Obj = new Replenish_Master()
                            {
                                Company_Id = RepMstrItem.Company_Id,
                                DateFrom = DateTime.ParseExact(RepMstrItem.DateFrom, "MM/dd/yyyy", null),
                                DateTo = DateTime.ParseExact(RepMstrItem.DateTo, "MM/dd/yyyy", null),
                                LastUpdate = DateTime.ParseExact(RepMstrItem.LastUpdate, "MM/dd/yyyy H:mm:ss", null),
                                User_Id = RepMstrItem.User_Id,
                                MarketID = RepMstrItem.Id
                            };
                            context.Replenish_Master.Add(Obj);
                            context.SaveChanges();
                        }

                    }
                }
                catch (Exception ex)
                {
                    Status += Customer.URL + "  " + ErrorUnit.RetriveExceptionMsg(ex) + ", ";
                }
            }

            if (Status != "")
            {
                ErrorUnit.InsertStatus("Error__GetReplenishmentMstr outer", Status);
            }
        }
        private void GetReplenishmentDetails(List<Tbl_CustomerMaster> Customers, string _Password, string _VendorCompanyCode)
        {
            string Status = "";
            foreach (var Customer in Customers)
            {

                try
                {
                    Thread.Sleep(500);
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(Customer.URL);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var context = new BayanEntities();

                    var LastRecord = context.Replenish_Details.Where(w => w.Company_Id == Customer.CompanyID && w.Sub_WH_Id == Customer.StoreID && w.Vendor_CompanyID == _VendorCompanyCode).OrderByDescending(w => w.LastUpdate).FirstOrDefault();
                    var _LastUpdate = DateTime.Now.AddYears(-50);
                    if (LastRecord != null)
                    {
                        _LastUpdate = LastRecord.LastUpdate.Value.AddSeconds(1);
                    }

                    var Response = client.PostAsJsonAsync(
                   "VendorSync/RetriveReplenishDetailsData", new
                   {
                       LastUpdate = _LastUpdate,
                       Password = _Password,
                       StoreID = Customer.StoreID,
                       CompanyID = Customer.CompanyID,
                       VendorCompanyCode = _VendorCompanyCode
                   }).Result;


                    var Result = Response.Content.ReadAsAsync<List<Replenish_DetailsVM>>().Result;

                    if (Result.Count == 0) continue;

                    var MrktMstrIDLst = Result.Select(w => new { MstrID = w.Replenish_Master_Id, Company_Id = w.Company_Id }).Distinct().OrderBy(w => w.MstrID).ToList();
                    foreach (var Mrkt in MrktMstrIDLst)
                    {
                        var VendRepMstrObj = context.Replenish_Master.Where(w => w.MarketID == Mrkt.MstrID && w.Company_Id == Mrkt.Company_Id).FirstOrDefault();
                        if (VendRepMstrObj == null)
                        {
                            ErrorUnit.InsertStatus("Error__GetReplenishmentDetails inner", Customer.URL + "  Replenishment Mstr with MarketID =  " + Mrkt.MstrID + " Not Exist");
                            break;
                        }

                        else
                        {
                            var VendRepMstrID = VendRepMstrObj.ID;
                            var Replenish_DetailsLst = Result.Where(w => w.Replenish_Master_Id == Mrkt.MstrID && w.Company_Id == Mrkt.Company_Id).Select(w => new Replenish_Details()
                            {
                                Company_Id = w.Company_Id,
                                Replenish_Master_Id = VendRepMstrID,
                                Avg_Po_Return = w.Avg_Po_Return,
                                Barcode = w.Barcode,
                                Current_Stock = w.Current_Stock,
                                First_Visit = w.First_Visit,
                                Forecasted_Po_Qty = w.Forecasted_Po_Qty,
                                Net_Sales = w.Net_Sales,
                                ProductAtrrSeq = w.ProductAtrrSeq,
                                Second_Visit = w.Second_Visit,
                                Sub_WH_Id = w.Sub_WH_Id,
                                Supplier_Code = w.Supplier_Code,
                                Third_Visit = w.Third_Visit,
                                Total_Po_Qty = w.Total_Po_Qty,
                                Total_Po_Return_Qty = w.Total_Po_Return_Qty,
                                LastUpdate = DateTime.ParseExact(w.LastUpdate, "MM/dd/yyyy H:mm:ss", null),
                                MarktID = w.ID,
                                InternalCode = w.InternalCode,
                                Vendor_CompanyID = _VendorCompanyCode, //w.Vendor_CompanyID
                                AvgSales = w.AvgSales,
                                FifthPO_Received = w.FifthPO_Received,
                                FifthPO_Return = w.FifthPO_Return,
                                FirstPO_Received = w.FirstPO_Received,
                                FirstPO_Return = w.FirstPO_Return,
                                FourthPO_Received = w.FourthPO_Received,
                                FourthPO_Return = w.FourthPO_Return,
                                Min_Qty = w.Min_Qty,
                                SecondPO_Received = w.SecondPO_Received,
                                SecondPO_Return = w.SecondPO_Return,
                                SixthPO_Received = w.SixthPO_Received,
                                SixthPO_Return = w.SixthPO_Return,
                                ThirdPO_Received = w.ThirdPO_Received,
                                ThirdPO_Return = w.ThirdPO_Return,
                                NormalNet_PO_Qty = w.NormalNet_PO_Qty,
                                NormalRecieved_PO_Qty = w.NormalRecieved_PO_Qty,
                                NormalReturn_PO_Qty = w.NormalReturn_PO_Qty,
                                Total_Cost = w.Total_Cost
                            }).OrderBy(w => w.LastUpdate).ToList();
                            context.Replenish_Details.AddRange(Replenish_DetailsLst);
                            context.SaveChanges();
                        }
                    }
                }

                catch (Exception ex)
                {
                    Status += Customer.URL + "  " + ErrorUnit.RetriveExceptionMsg(ex) + ", ";
                }
            }

            if (Status != "")
            {
                ErrorUnit.InsertStatus("Error__GetReplenishmentDetails outer", Status);
            }
        }
        private void GetOrderManagementMstr(List<Tbl_CustomerMaster> Customers, string _Password, string _VendorCompanyCode)
        {
            string Status = "";
            foreach (var Customer in Customers)
            {
                try
                {
                    Thread.Sleep(500);
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(Customer.URL);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var context = new BayanEntities();

                    var LastRecord = context.OrderManagementMasters.Where(w => w.CompanyID == Customer.CompanyID && w.StoreId == Customer.StoreID && w.Vendor_CompanyID == _VendorCompanyCode).OrderByDescending(w => w.Lastupdate).FirstOrDefault();
                    var _LastUpdate = DateTime.Now.AddYears(-50);
                    if (LastRecord != null)
                    {
                        _LastUpdate = LastRecord.Lastupdate.Value.AddSeconds(1);
                    }
                    var Response = client.PostAsJsonAsync(
                   "VendorSync/RetriveOrderManagementMasterData", new
                   {
                       LastUpdate = _LastUpdate,
                       Password = _Password,
                       StoreID = Customer.StoreID,
                       CompanyID = Customer.CompanyID,
                       VendorCompanyCode = _VendorCompanyCode
                   }).Result;

                    var Result = Response.Content.ReadAsAsync<List<OrderManagementMasterVM>>().Result.OrderBy(w => w.Lastupdate).ToList();
                    var OrderManagMstr = new List<ReplenishmentMstrResultVM>();
                    foreach (var OrderManagItem in Result)
                    {
                        decimal ReplenishmentMasterID = 0;
                        if (OrderManagItem.ReplenishmentMasterID != null && OrderManagItem.ReplenishmentMasterID > 0)
                        {
                            var ReplMstr = context.Replenish_Master.Where(w => w.Company_Id == OrderManagItem.CompanyID && w.MarketID == OrderManagItem.ReplenishmentMasterID).FirstOrDefault();
                            if (ReplMstr != null)
                            {
                                ReplenishmentMasterID = ReplMstr.ID;
                            }
                            else
                            {
                                ErrorUnit.InsertStatus("Error__GetOrderManagementMstr inner", Customer.URL + "  Order Mstr with MarketID =  " + OrderManagItem.ReplenishmentMasterID + " Not Exist");
                                break;
                            }
                        }
                        var OldObj = context.OrderManagementMasters.Where(w => w.CompanyID == OrderManagItem.CompanyID && w.MarketID == OrderManagItem.ID).FirstOrDefault();
                        if (OldObj != null)
                        {
                            if (OrderManagItem.Lastupdate == "") { OldObj.Lastupdate = null; } else { OldObj.Lastupdate = DateTime.ParseExact(OrderManagItem.Lastupdate, "MM/dd/yyyy H:mm:ss", null); }
                            if (OrderManagItem.RecievedDate == "") { OldObj.RecievedDate = null; } else { OldObj.RecievedDate = DateTime.ParseExact(OrderManagItem.RecievedDate, "MM/dd/yyyy H:mm:ss", null); }

                            OldObj.DocumentNumber = OrderManagItem.DocumentNumber;
                            OldObj.Active = OrderManagItem.Active;
                            OldObj.BuyItemCount = OrderManagItem.BuyItemCount;
                            OldObj.BuyTotalAfterDiscount = OrderManagItem.BuyTotalAfterDiscount;
                            OldObj.BuyTotalAfterTax = OrderManagItem.BuyTotalAfterTax;
                            OldObj.BuyTotalBeforeDiscount = OrderManagItem.BuyTotalBeforeDiscount;
                            OldObj.BuyTotalDiscount = OrderManagItem.BuyTotalDiscount;
                            OldObj.BuyTotalTax = OrderManagItem.BuyTotalTax;
                            OldObj.Card1No = OrderManagItem.Card1No;
                            OldObj.FreeItemCount = OrderManagItem.FreeItemCount;
                            OldObj.InvoiceDiscount = OrderManagItem.InvoiceDiscount;
                            OldObj.ItemCount = OrderManagItem.ItemCount;
                            OldObj.PaidAmount = OrderManagItem.PaidAmount;
                            OldObj.Pay1amt = OrderManagItem.Pay1amt;
                            OldObj.Pay2amt = OrderManagItem.Pay2amt;
                            OldObj.Pay2Id = OrderManagItem.Pay2Id;
                            OldObj.RemainAmount = OrderManagItem.RemainAmount;
                            OldObj.ReturnItemCount = OrderManagItem.ReturnItemCount;
                            OldObj.ReturnTotalAfterDiscount = OrderManagItem.ReturnTotalAfterDiscount;
                            OldObj.ReturnTotalAfterTax = OrderManagItem.ReturnTotalAfterTax;
                            OldObj.ReturnTotalBeforeDiscount = OrderManagItem.ReturnTotalBeforeDiscount;
                            OldObj.ReturnTotalDiscount = OrderManagItem.ReturnTotalDiscount;
                            OldObj.ReturnTotalTax = OrderManagItem.ReturnTotalTax;
                            OldObj.Status = OrderManagItem.Status;
                            OldObj.SubTotal = OrderManagItem.SubTotal;
                            OldObj.Taxes = OrderManagItem.Taxes;
                            OldObj.Total = OrderManagItem.Total;
                            OldObj.TotalAfterDiscount = OrderManagItem.TotalAfterDiscount;
                            OldObj.TotalAfterTax = OrderManagItem.TotalAfterTax;
                            OldObj.TotalBeforeDiscount = OrderManagItem.TotalBeforeDiscount;
                            OldObj.TotalDiscount = OrderManagItem.TotalDiscount;
                            OldObj.TotalTax = OrderManagItem.TotalTax;
                            OldObj.ReplenishmentMasterID = ReplenishmentMasterID;
                            context.SaveChanges();

                            var Cust = context.Tbl_CustomerMaster.Where(w => w.StoreID == OrderManagItem.StoreId && w.CompanyID == OrderManagItem.CompanyID).FirstOrDefault();
                            var NotifcationObj = new Notifcation()
                            {
                                Date = DateTime.Now,
                                NotifcationMessage = "Order with Documnet Number: " + OrderManagItem.DocumentNumber + " from ( " + Cust.CompanyNameEng + " - " + Cust.StoreNameEng + " ) had been delivered",
                                Seen = false,
                                Url = "/OrderManagement/Delivered",
                                Vendor_CompanyID = OrderManagItem.Vendor_CompanyID
                            };
                            context.Notifcations.Add(NotifcationObj);
                            context.SaveChanges();
                        }
                        else
                        {
                            var Obj = new OrderManagementMaster()
                            {
                                PartnerCode = OrderManagItem.PartnerCode,
                                Active = OrderManagItem.Active,
                                BuyItemCount = OrderManagItem.BuyItemCount,
                                BuyTotalAfterDiscount = OrderManagItem.BuyTotalAfterDiscount,
                                BuyTotalAfterTax = OrderManagItem.BuyTotalAfterTax,
                                BuyTotalBeforeDiscount = OrderManagItem.BuyTotalBeforeDiscount,
                                BuyTotalDiscount = OrderManagItem.BuyTotalDiscount,
                                BuyTotalTax = OrderManagItem.BuyTotalTax,
                                Card1No = OrderManagItem.Card1No,
                                CompanyID = OrderManagItem.CompanyID,
                                DocumentNumber = OrderManagItem.DocumentNumber,
                                FreeItemCount = OrderManagItem.FreeItemCount,
                                InvoiceDiscount = OrderManagItem.InvoiceDiscount,
                                ItemCount = OrderManagItem.ItemCount,
                                PaidAmount = OrderManagItem.PaidAmount,
                                PartnerId = OrderManagItem.PartnerId,
                                Pay1amt = OrderManagItem.Pay1amt,
                                Pay2amt = OrderManagItem.Pay2amt,
                                Pay2Id = OrderManagItem.Pay2Id,
                                RemainAmount = OrderManagItem.RemainAmount,
                                ReturnItemCount = OrderManagItem.ReturnItemCount,
                                ReturnTotalAfterDiscount = OrderManagItem.ReturnTotalAfterDiscount,
                                ReturnTotalAfterTax = OrderManagItem.ReturnTotalAfterTax,
                                ReturnTotalBeforeDiscount = OrderManagItem.ReturnTotalBeforeDiscount,
                                ReturnTotalDiscount = OrderManagItem.ReturnTotalDiscount,
                                ReturnTotalTax = OrderManagItem.ReturnTotalTax,
                                SentDate = DateTime.Now,
                                Status = OrderManagItem.Status,
                                StoreId = OrderManagItem.StoreId,
                                SubTotal = OrderManagItem.SubTotal,
                                Taxes = OrderManagItem.Taxes,
                                Total = OrderManagItem.Total,
                                TotalAfterDiscount = OrderManagItem.TotalAfterDiscount,
                                TotalAfterTax = OrderManagItem.TotalAfterTax,
                                TotalBeforeDiscount = OrderManagItem.TotalBeforeDiscount,
                                TotalDiscount = OrderManagItem.TotalDiscount,
                                TotalTax = OrderManagItem.TotalTax,
                                IsFromReplenishment = OrderManagItem.IsFromReplenishment,
                                MarketID = OrderManagItem.ID,
                                Lastupdate = DateTime.ParseExact(OrderManagItem.Lastupdate, "MM/dd/yyyy H:mm:ss", null),
                                CreationDate = DateTime.ParseExact(OrderManagItem.CreationDate, "MM/dd/yyyy H:mm:ss", null),
                                Vendor_CompanyID = _VendorCompanyCode // OrderManagItem.Vendor_CompanyID
                            };

                            if (OrderManagItem.RecievedDate == "") { Obj.RecievedDate = null; } else { Obj.RecievedDate = DateTime.ParseExact(OrderManagItem.RecievedDate, "MM/dd/yyyy H:mm:ss", null); }
                            if (OrderManagItem.ShippedDate == "") { Obj.ShippedDate = null; } else { Obj.ShippedDate = DateTime.ParseExact(OrderManagItem.ShippedDate, "MM/dd/yyyy H:mm:ss", null); }
                            if (OrderManagItem.ExpectedDeliveryDate == "") { Obj.ExpectedDeliveryDate = null; } else { Obj.ExpectedDeliveryDate = DateTime.ParseExact(OrderManagItem.ExpectedDeliveryDate, "MM/dd/yyyy H:mm:ss", null); }

                            context.OrderManagementMasters.Add(Obj);
                            context.SaveChanges();

                            var Cust = context.Tbl_CustomerMaster.Where(w => w.StoreID == OrderManagItem.StoreId && w.CompanyID == OrderManagItem.CompanyID).FirstOrDefault();
                            var NotifcationObj = new Notifcation()
                            {
                                Date = DateTime.Now,
                                NotifcationMessage = "New Order with Documnet Number: " + OrderManagItem.DocumentNumber + " from ( " + Cust.CompanyNameEng + " - " + Cust.StoreNameEng + " )",
                                Seen = false,
                                Url = "/OrderManagement/Received",
                                Vendor_CompanyID = OrderManagItem.Vendor_CompanyID
                            };
                            context.Notifcations.Add(NotifcationObj);
                            context.SaveChanges();

                            OrderManagMstr.Add(new ReplenishmentMstrResultVM() { MasterID = OrderManagItem.ID, RequestID = Obj.ID });
                        }
                    }

                    if (OrderManagMstr.Count > 0)
                    {
                        var Response2 = client.PostAsJsonAsync(
                        "VendorSync/UpdateDirectPoMasterIDs", new
                        {
                            DirectPoMstrResultVMLst = OrderManagMstr,
                            Password = _Password,
                            LastUpdate = DateTime.ParseExact(Result[0].Lastupdate, "MM/dd/yyyy H:mm:ss", null)
                        }).Result;

                        foreach (var Item in OrderManagMstr)
                        {
                            var OldObj = context.OrderManagementMasters.Where(w => w.ID == Item.MasterID).FirstOrDefault();
                            if (OldObj != null)
                            {
                                OldObj.Lastupdate = DateTime.Now;
                                context.SaveChanges();
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Status += Customer.URL + "  " + ErrorUnit.RetriveExceptionMsg(ex) + ", ";
                }
            }

            if (Status != "")
            {
                ErrorUnit.InsertStatus("Error__GetOrderManagementMstr outer", Status);
            }
        }
        private void GetOrderManagementDetails(List<Tbl_CustomerMaster> Customers, string _Password, string _VendorCompanyCode)
        {
            string Status = "";
            foreach (var Customer in Customers)
            {
                Thread.Sleep(500);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Customer.URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var context = new BayanEntities();
                try
                {
                    var LastRecord = (from Mstr in context.OrderManagementMasters
                                      from Dtl in context.OrderManagementDetails
                                      where Dtl.MasterID == Mstr.ID
                                      && Mstr.StoreId == Customer.StoreID
                                      && Mstr.CompanyID == Customer.CompanyID
                                      && Dtl.Vendor_CompanyID == _VendorCompanyCode
                                      select Dtl
                                     ).OrderByDescending(w => w.Lastupdate).FirstOrDefault();
                    var _LastUpdate = DateTime.Now.AddYears(-50);
                    if (LastRecord != null)
                    {
                        _LastUpdate = LastRecord.Lastupdate.Value.AddSeconds(1);
                    }
                    var Response = client.PostAsJsonAsync(
                   "VendorSync/RetriveOrderManagementDetailsData", new
                   {
                       LastUpdate = _LastUpdate,
                       Password = _Password,
                       StoreID = Customer.StoreID,
                       CompanyID = Customer.CompanyID,
                       VendorCompanyCode = _VendorCompanyCode
                   }).Result;

                    var Result = Response.Content.ReadAsAsync<List<OrderManagementDetailsVM>>().Result.OrderBy(w => w.Lastupdate).ToList();
                    var OrderManagementDetailLst = new List<OrderManagementDetail>();


                    var OldOrderMstrIDs = Result.Select(w => w.MasterID).Distinct().ToList();
                    var OldDtl = context.OrderManagementDetails.Where(w => OldOrderMstrIDs.Contains(w.MasterID)).ToList();
                    context.OrderManagementDetails.RemoveRange(OldDtl);
                    context.SaveChanges();


                    var NewLst = Result.Select(w => new OrderManagementDetail()
                    {
                        Lastupdate = DateTime.ParseExact(w.Lastupdate, "MM/dd/yyyy H:mm:ss", null),
                        IsActive = w.IsActive,
                        SubTotal = w.SubTotal,
                        Taxes = w.Taxes,
                        Total = w.Total,
                        TotalAfterDiscount = w.TotalAfterDiscount,
                        TotalAfterTax = w.TotalAfterTax,
                        TotalBeforeDiscount = w.TotalBeforeDiscount,
                        ActualRecivedQty = w.ActualRecivedQty,
                        MasterID = w.MasterID,
                        ApprovedQty = w.ApprovedQty,
                        Barcode = w.Barcode,
                        BonusQTY = w.BonusQTY,
                        CommericalDiscount = w.CommericalDiscount,
                        DifferancePercent = w.DifferancePercent,
                        DifferanceValue = w.DifferanceValue,
                        Discount = w.Discount,
                        ExpiryDate = w.ExpiryDate,
                        IsFree = w.IsFree,
                        JomlaDiscount = w.JomlaDiscount,
                        MarketUnitPrice = w.MarketUnitPrice,
                        ReturnQty = w.ReturnQty,
                        SellPrice = w.SellPrice,
                        ShippedQty = w.ShippedQty,
                        SystemQty = w.SystemQty,
                        TaxValue = w.TaxValue,
                        VendorUnitPrice = w.VendorUnitPrice,
                        InternalCode = w.InternalCode,
                        Vendor_CompanyID = _VendorCompanyCode // w.Vendor_CompanyID
                    }).ToList();
                    context.OrderManagementDetails.AddRange(NewLst);
                    context.SaveChanges();
                }

                catch (Exception ex)
                {
                    Status += Customer.URL + "  " + ErrorUnit.RetriveExceptionMsg(ex) + ", ";
                }
            }

            if (Status != "")
            {
                ErrorUnit.InsertStatus("Error__GetOrderManagementDetails outer", Status);
            }
        }
        private void DownloadCentralizedLookUp()
        {
            BayanEntities DB = new BayanEntities();
            CentralizedEntities CenterDB = new CentralizedEntities();

            var _LastUpdate = DateTime.Now.AddYears(-50);
            var LookUpLastUpdate = DB.Tbl_CentralizedLockUp.OrderByDescending(w => w.LastUpdate).FirstOrDefault();
            if (LookUpLastUpdate != null)
            {
                _LastUpdate = LookUpLastUpdate.LastUpdate.Value.AddSeconds(1);
            }

            var Result = CenterDB.CentralizedLockUps.Where(w => w.Lastupdate > _LastUpdate).OrderBy(w => w.Lastupdate).ToList();

            using (var Contxt = new BayanEntities())
            {
                using (var db_contextTransaction = Contxt.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in Result)
                        {
                            var ExistObj = Contxt.Tbl_CentralizedLockUp.Where(w => w.ID == item.ID).FirstOrDefault();
                            if (ExistObj != null)
                            {
                                ExistObj.IsActive = item.Active;
                                ExistObj.LastUpdate = item.Lastupdate;
                                ExistObj.Name = item.NameAr;
                                ExistObj.NameEng = item.NameEn;
                                ExistObj.ParentId = item.ParentId;
                                ExistObj.Updatedby = item.updatedby;
                                Contxt.SaveChanges();
                            }
                            else
                            {
                                var LookUpTbl = new Tbl_CentralizedLockUp()
                                {
                                    LastUpdate = _LastUpdate,
                                    IsActive = item.Active,
                                    Createdby = item.Createdby,
                                    CreatedDate = item.CreatedDate,
                                    Name = item.NameAr,
                                    NameEng = item.NameEn,
                                    ParentId = item.ParentId,
                                    Updatedby = item.updatedby,
                                    ID = item.ID
                                };
                                Contxt.Tbl_CentralizedLockUp.Add(LookUpTbl);
                                Contxt.SaveChanges();
                            }
                        }

                        Contxt.SaveChanges();
                        db_contextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        db_contextTransaction.Rollback();
                        ErrorUnit.InsertStatus("Error__DownloadCentralizedLookUp Internal", ErrorUnit.RetriveExceptionMsg(ex));
                    }
                }
            }
        }
        private void DownloadCustomer()
        {
            BayanEntities DB = new BayanEntities();
            CentralizedEntities CenterDB = new CentralizedEntities();
            var _LastUpdate = DateTime.Now.AddYears(-50);
            var VendorCompanyIdLst = DB.Tbl_Company.Where(w => w.IsActive == true).Select(w => w.Vendor_CompanyID).ToList();

            // We use this because of vendor system and markt system can chnage on lastupdate in Customer table
            var CustomerLastUpdate = DB.Tbl_LastUpdateFromCenteralDB.Where(a => a.TableName == "Customer").FirstOrDefault();

            if (CustomerLastUpdate != null)
            {
                _LastUpdate = CustomerLastUpdate.LastUpdate.Value.AddSeconds(1);
            }
            else
            {
                DB.Tbl_LastUpdateFromCenteralDB.Add(new Tbl_LastUpdateFromCenteralDB() { LastUpdate = _LastUpdate, TableName = "Customer" });
                DB.SaveChanges();
            }

            var Result = CenterDB.Customer_URL.Where(w => w.LastUpdate > _LastUpdate).OrderBy(w => w.LastUpdate).ToList();
            _LastUpdate = DateTime.Now;

            using (var Contxt = new BayanEntities())
            {
                using (var db_contextTransaction = Contxt.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in Result)
                        {
                            var ExistObj = Contxt.Tbl_CustomerMaster.Where(w => w.StoreID == item.StoreID && w.CompanyID == item.CompanyID).FirstOrDefault();
                            if (ExistObj != null)
                            {
                                ExistObj.Address = item.Address;
                                ExistObj.CityID = item.CityID;
                                ExistObj.Commercial_Registration = item.Commercial_Registration;
                                ExistObj.CompanyName = item.CompanyName;
                                ExistObj.CompanyNameEng = item.CompanyNameEng;
                                ExistObj.CountryID = item.CountryID;
                                ExistObj.Mobile = item.Mobile;
                                ExistObj.Phone = item.Phone;
                                ExistObj.ProvinceID = item.ProvinceID;
                                ExistObj.RegionID = item.RegionID;
                                ExistObj.StoreName = item.StoreName;
                                ExistObj.StoreNameEng = item.StoreNameEng;
                                ExistObj.TerritoryID = item.TerritoryID;
                                ExistObj.URL = item.URL;
                                ExistObj.VAT_Registration = item.VAT_Registration;
                                ExistObj.LastSalesSyncDate = item.LastSalesSyncDate;
                                ExistObj.LastOpenDay = item.LastOpenDay;
                                ExistObj.IsActive = item.Active;
                            }
                            else
                            {
                                var NewCustMstr = new Tbl_CustomerMaster()
                                {
                                    CompanyID = item.CompanyID,
                                    StoreID = item.StoreID,
                                    Address = item.Address,
                                    CityID = item.CityID,
                                    Commercial_Registration = item.Commercial_Registration,
                                    CompanyName = item.CompanyName,
                                    CompanyNameEng = item.CompanyNameEng,
                                    CountryID = item.CountryID,
                                    Mobile = item.Mobile,
                                    Phone = item.Phone,
                                    ID = item.ID,
                                    ProvinceID = item.ProvinceID,
                                    RegionID = item.RegionID,
                                    StoreName = item.StoreName,
                                    StoreNameEng = item.StoreNameEng,
                                    TerritoryID = item.TerritoryID,
                                    URL = item.URL,
                                    VAT_Registration = item.VAT_Registration,
                                    LastSalesSyncDate = item.LastSalesSyncDate,
                                    LastOpenDay = item.LastOpenDay,
                                    IsActive = item.Active
                                };

                                Contxt.Tbl_CustomerMaster.Add(NewCustMstr);
                                Contxt.SaveChanges();

                                int CustMstrID = NewCustMstr.ID;
                                foreach (var VendorCode in VendorCompanyIdLst)
                                {
                                    var CustDtl = new Tbl_CustomerDtl() { CustomerMasterID = CustMstrID, Active = true, LastUpdate = DateTime.Now, Vendor_CompanyID = VendorCode };
                                    Contxt.Tbl_CustomerDtl.Add(CustDtl);
                                    Contxt.SaveChanges();
                                }

                            }
                        }
                        CustomerLastUpdate = Contxt.Tbl_LastUpdateFromCenteralDB.Where(a => a.TableName == "Customer").FirstOrDefault();
                        CustomerLastUpdate.LastUpdate = _LastUpdate;
                        Contxt.SaveChanges();
                        db_contextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        db_contextTransaction.Rollback();
                        ErrorUnit.InsertStatus("Error__DownloadCustomer Internal", ErrorUnit.RetriveExceptionMsg(ex));
                    }
                }
            }
        }
        private void DownloadCustomer_CustomerCode()
        {
            BayanEntities DB = new BayanEntities();
            CentralizedEntities CenterDB = new CentralizedEntities();

            var VendorIdLst = DB.Tbl_Company.Where(w => w.IsActive == true).Select(w => w.Vendor_CompanyID).ToList();

            var _LastUpdate = DateTime.Now.AddYears(-50);

            // We use this because of vendor system and markt system can chnage on lastupdate in Customer Details table
            var CustomerLastUpdate = DB.Tbl_LastUpdateFromCenteralDB.Where(a => a.TableName == "CustomerDtl").FirstOrDefault();

            if (CustomerLastUpdate != null)
            {
                _LastUpdate = CustomerLastUpdate.LastUpdate.Value.AddSeconds(1);
            }
            else
            {
                DB.Tbl_LastUpdateFromCenteralDB.Add(new Tbl_LastUpdateFromCenteralDB() { LastUpdate = _LastUpdate, TableName = "CustomerDtl" });
                DB.SaveChanges();
            }

            var Result = CenterDB.StoreVsDistributors.Where(w => w.Lastupdate > _LastUpdate).OrderBy(w => w.Lastupdate).ToList();
            _LastUpdate = DateTime.Now;

            using (var Contxt = new BayanEntities())
            {
                using (var db_contextTransaction = Contxt.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in Result)
                        {
                            var MstrObj = Contxt.Tbl_CustomerMaster.Where(w => w.StoreID == item.StoreID && w.CompanyID == item.CompanyID).FirstOrDefault();
                            if (MstrObj == null) continue;

                            var ExistObj = Contxt.Tbl_CustomerDtl.Where(w => w.CustomerMasterID == MstrObj.ID && w.Vendor_CompanyID == item.VendorCompanyCode).FirstOrDefault();
                            if (ExistObj != null)
                            {
                                ExistObj.CustoemrCode = item.Code;
                                ExistObj.LastUpdate = DateTime.Now;
                                Contxt.SaveChanges();
                            }
                        }

                        CustomerLastUpdate = Contxt.Tbl_LastUpdateFromCenteralDB.Where(a => a.TableName == "CustomerDtl").FirstOrDefault();
                        CustomerLastUpdate.LastUpdate = _LastUpdate;
                        Contxt.SaveChanges();
                        db_contextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        db_contextTransaction.Rollback();
                        ErrorUnit.InsertStatus("Error__DownloadCustomer Internal", ErrorUnit.RetriveExceptionMsg(ex));
                    }
                }
            }
        }
        public void DownloadOrderOnly()
        {
            try
            {
                var context = new BayanEntities();
                var LastJob = context.Tbl_Jobs.Where(w => w.Description.StartsWith("StartDownload")).OrderByDescending(w => w.FiringDate).FirstOrDefault();
                var LastComplete = context.Tbl_Jobs.Where(w => w.Description.StartsWith("Completed") || w.Description.Contains("Successfully__DownloadAllData")).OrderByDescending(w => w.FiringDate).FirstOrDefault();
                if (LastJob != null && LastComplete != null && LastJob.FiringDate > LastComplete.FiringDate && LastJob.FiringDate.AddMinutes(5) > DateTime.Now)
                {
                    Thread.Sleep(30000);
                    DownloadOrderOnly();
                    return;
                }
                ErrorUnit.InsertStatus("StartDownload", "");


                var Customers = (from CustMstr in context.Tbl_CustomerMaster
                                 where CustMstr.IsActive == true
                                 select CustMstr).ToList();

                var VendorIdLst = context.Tbl_Company.Where(w => w.IsActive == true).Select(w => w.Vendor_CompanyID).ToList();
                foreach (var item in VendorIdLst)
                {
                    try { GetOrderManagementMstr(Customers, Pass, item); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__GetOrderManagementMstr", ErrorUnit.RetriveExceptionMsg(ex)); }
                    try { GetOrderManagementDetails(Customers, Pass, item); } catch (Exception ex) { ErrorUnit.InsertStatus("Error__GetOrderManagementDetails", ErrorUnit.RetriveExceptionMsg(ex)); }
                }
                ErrorUnit.InsertStatus("Completed", "");
            }
            catch (Exception ex)
            { ErrorUnit.InsertStatus("Error__DownloadOrderOnly", ErrorUnit.RetriveExceptionMsg(ex)); }

        }
    }
}