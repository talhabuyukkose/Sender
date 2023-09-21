using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender.Core.Models.TsoftModels
{
    public class TsoftProductModel : TsoftBaseModel
    {
        public List<Data> data { get; set; } = new List<Data>();
        public class Data
        {
            public string ProductId { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string DefaultCategoryId { get; set; }
            public string DefaultCategoryName { get; set; }
            public string DefaultCategoryPath { get; set; }
            public string SupplierProductCode { get; set; }
            public string Barcode { get; set; }
            public string Stock { get; set; }
            public string IsActive { get; set; }
            public string ComparisonSites { get; set; }
            public string IsApproved { get; set; }
            public string HasSubProducts { get; set; }
            public string HasImages { get; set; }
            public string Vat { get; set; }
            public string CurrencyId { get; set; }
            public string BuyingPrice { get; set; }
            public string SellingPrice { get; set; }
            public string SellingPriceVatIncludedNoDiscount { get; set; }
            public float SellingPriceVatIncluded { get; set; }
            public string DiscountedSellingPrice { get; set; }
            public string SeoLink { get; set; }
            public string StockUnit { get; set; }
            public string StockUnitId { get; set; }
            public string SearchKeywords { get; set; }
            public string DisplayOnHomepage { get; set; }
            public string IsNewProduct { get; set; }
            public string OnSale { get; set; }
            public string IsDisplayProduct { get; set; }
            public string VendorDisplayOnly { get; set; }
            public string DisplayWithVat { get; set; }
            public string Brand { get; set; }
            public string BrandId { get; set; }
            public string BrandLink { get; set; }
            public string Model { get; set; }
            public string ModelId { get; set; }
            public string SupplierId { get; set; }
            public string CustomerGroupDisplay { get; set; }
            public string Additional1 { get; set; }
            public string Additional2 { get; set; }
            public string Additional3 { get; set; }
            public string ImageUrl { get; set; }
            public string Magnifier { get; set; }
            public string MemberMinOrder { get; set; }
            public string MemberMaxOrder { get; set; }
            public string VendorMinOrder { get; set; }
            public string VendorMaxOrder { get; set; }
            public string FreeDeliveryMember { get; set; }
            public string FreeDeliveryVendor { get; set; }
            public string FreeDeliveryForProduct { get; set; }
            public string ShortDescription { get; set; }
            public string SavingDate { get; set; }
            public string CreateDateTimeStamp { get; set; }
            public DateTime CreateDate { get; set; }
            public string FilterGroupId { get; set; }
            public string ListNo { get; set; }
            public string OwnerId { get; set; }
            public string StockUpdateDate { get; set; }
            public string StockUpdatePlatform { get; set; }
            public string PriceUpdateDate { get; set; }
            public string PriceUpdatePlatform { get; set; }
            public string IsActiveUpdateDate { get; set; }
            public string IsActiveUpdatePlatform { get; set; }
            public string Gender { get; set; }
            public string OpportunityProduct { get; set; }
            public string OpportunityStart { get; set; }
            public string OpportunityFinish { get; set; }
            public object DisablePaymentTypes { get; set; }
            public string AgeGroup { get; set; }
            public string Point { get; set; }
            public string Numeric1 { get; set; }
            public string DefaultSubProductId { get; set; }
            public string DisableCargoCompany { get; set; }
            public string HomepageSort { get; set; }
            public string MostSoldSort { get; set; }
            public string NewestSort { get; set; }
            public string EftRate { get; set; }
            public string RelatedProductsBlock1 { get; set; }
            public string RelatedProductsBlock2 { get; set; }
            public string RelatedProductsBlock3 { get; set; }
            public string CatalogModelCode { get; set; }
            public string ShowInListPage { get; set; }
            public string SearchRank { get; set; }
            public Label1 Label1 { get; set; }
            public Label2 Label2 { get; set; }
            public Label3 Label3 { get; set; }
            public Label4 Label4 { get; set; }
            public Label5 Label5 { get; set; }
            public Label6 Label6 { get; set; }
            public Label7 Label7 { get; set; }
            public Label8 Label8 { get; set; }
            public Label9 Label9 { get; set; }
            public Label10 Label10 { get; set; }
            public string DefaultCategoryCode { get; set; }
            public string Currency { get; set; }
            public string Supplier { get; set; }
            public float DiscountedSellingPriceTL { get; set; }
            public Category[] Categories { get; set; }
            public string Details { get; set; }
            public string Width { get; set; }
            public string Height { get; set; }
            public string Depth { get; set; }
            public string Weight { get; set; }
            public string CBM { get; set; }
            public string WarrantyInfo { get; set; }
            public string DeliveryInfo { get; set; }
            public string DeliveryTime { get; set; }
            public string ProductNote { get; set; }
            public string Document { get; set; }
            public string Warehouse { get; set; }
            public string PersonalizationId { get; set; }
            public string SeoTitle { get; set; }
            public string SeoKeywords { get; set; }
            public string SeoDescription { get; set; }
            public string SeoSettingsId { get; set; }
            public string Gtip { get; set; }
            public string CountryOfOrigin { get; set; }
            public object[] PackageProducts { get; set; }
            public object DiscountedPrice { get; set; }
            public Discountedprice[] DiscountedPrices { get; set; }
            public object[] MultipleDiscounts { get; set; }
            public Imageurl[] ImageUrls { get; set; }
        }

        public class Label1
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        public class Label2
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        public class Label3
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        public class Label4
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        public class Label5
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        public class Label6
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        public class Label7
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        public class Label8
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        public class Label9
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        public class Label10
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        public class Category
        {
            public string CategoryId { get; set; }
            public string CategoryCode { get; set; }
        }

        public class Discountedprice
        {
            public string CustomerGroupCode { get; set; }
            public string DiscountedPrice { get; set; }
            public int DiscountedPriceVatIncluded { get; set; }
        }

        public class Imageurl
        {
            public string ImageUrl { get; set; }
            public string ListNo { get; set; }
            public string PropertyId { get; set; }
        }

    }
}
