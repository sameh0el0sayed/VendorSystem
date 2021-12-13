//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VendorSystem.Models.Model2
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public decimal Product_Id { get; set; }
        public string Serial_Num { get; set; }
        public string Product_NameEn { get; set; }
        public string Product_Name { get; set; }
        public string Barcode { get; set; }
        public string OldBarcode { get; set; }
        public string Pos_Name { get; set; }
        public string Receipt_Name { get; set; }
        public Nullable<decimal> Main_Grp_Id { get; set; }
        public Nullable<decimal> Sub_Grp_Id { get; set; }
        public Nullable<int> UOM { get; set; }
        public Nullable<decimal> SubCategoryId { get; set; }
        public decimal PiecesInCatron { get; set; }
        public decimal CatronInBox { get; set; }
        public Nullable<decimal> ModelID { get; set; }
        public Nullable<decimal> ItemTypeID { get; set; }
        public Nullable<decimal> ItemDirectCost { get; set; }
        public Nullable<decimal> ShippingClassfication { get; set; }
        public Nullable<decimal> ReplacementItemId { get; set; }
        public Nullable<decimal> BasicClassification { get; set; }
        public Nullable<decimal> AdvancedClassification { get; set; }
        public Nullable<decimal> ItemOverHead { get; set; }
        public Nullable<decimal> BrandId { get; set; }
        public Nullable<bool> HasReplacement { get; set; }
        public Nullable<bool> IsReplenishment { get; set; }
        public Nullable<decimal> CategoryId { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string Photo_Name { get; set; }
        public Nullable<decimal> Lead_Time { get; set; }
        public decimal Pdt_Type { get; set; }
        public Nullable<decimal> Currency_Id { get; set; }
        public Nullable<decimal> seasonId { get; set; }
        public Nullable<decimal> FabricGroupId { get; set; }
        public Nullable<decimal> FabricTypeId { get; set; }
        public string Year { get; set; }
        public Nullable<int> TreatmentType { get; set; }
        public string PrintName { get; set; }
        public string CompanyID { get; set; }
        public decimal Updated { get; set; }
        public decimal Active { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> Lastupdate { get; set; }
        public Nullable<int> Createdby { get; set; }
        public Nullable<int> updatedby { get; set; }
        public Nullable<int> Deletedby { get; set; }
        public Nullable<bool> IsSupplierOffer { get; set; }
        public Nullable<bool> IsNegativeSales { get; set; }
        public Nullable<bool> IsWeight { get; set; }
        public string BigUnitNameAr { get; set; }
        public string BigUnitNameEn { get; set; }
    }
}
