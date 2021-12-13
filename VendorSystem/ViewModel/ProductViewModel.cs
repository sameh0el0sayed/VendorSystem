using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VendorSystem.ViewModel
{
    public class ProductViewModel
    {
        public decimal Product_Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Requird")]
        public string Serial_Num { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Requird")]
        public string Product_NameEn { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Requird")]
        public string Product_Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Requird")]
        public string Barcode { get; set; }
        public string OldBarcode { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Requird")]
        public string Pos_Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Requird")]
        public string Receipt_Name { get; set; }
        public Nullable<decimal> Main_Grp_Id { get; set; }
        public Nullable<decimal> Sub_Grp_Id { get; set; }
        // [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Requird")]
        public decimal? SubCategoryId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Requird")]
        public decimal PiecesInCatron { get; set; }
        //[Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Requird")]
        public decimal? CatronInBox { get; set; }
        public decimal? ModelID { get; set; }
        public decimal? ItemDirectCost { get; set; }
        public decimal? ShippingClassfication { get; set; }
        public decimal? ReplacementItemId { get; set; }
        public decimal? BasicClassification { get; set; }
        public decimal? AdvancedClassification { get; set; }
        public decimal? ItemOverHead { get; set; }
        public decimal? BrandId { get; set; }
        public bool? IsReplenishment { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Requird")]
        public decimal? CategoryId { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string Photo_Name { get; set; }
        // [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Requird")]
        public int? Purchase_Uom_Id { get; set; }
        public decimal? Pdt_Type { get; set; }
        public Nullable<decimal> Currency_Id { get; set; }
        public Nullable<decimal> seasonId { get; set; }
        public Nullable<decimal> FabricGroupId { get; set; }
        public Nullable<decimal> FabricTypeId { get; set; }
        public string Year { get; set; }
        public Nullable<int> TreatmentType { get; set; }
        public string PrintName { get; set; }
        /// ///////////////////////////////////////////////////////////////////////
        public int? FormType { get; set; }
        public bool Active { get; set; }
        public HttpPostedFileBase Photo { get; set; }
        public string Code { get; set; }
        public bool? IsSupplierOffer { get; set; }
        public bool IsNegativeSale { get; set; }
        public bool IsWeight { get; set; }
        public List<VendorLink> VendorLinkLst { get; set; }

    }

    public class VendorLink  
    {
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
    }
}