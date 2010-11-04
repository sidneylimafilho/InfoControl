using System.Linq;
using InfoControl;

namespace Vivina.Erp.DataClasses
{
    public partial class Product
    {
        public string ImageUrl { get { return ProductImages.Any() ? ProductImages.First().ImageUrl : ""; } }
        public string ManufacturerName { get { return Manufacturer != null ? Manufacturer.Name : ""; } }
        public string CategoryName { get { return Category != null ? Category.Name : ""; } }
        public decimal? UnitPrice { get { return Inventories.Any() ? Inventories.Min(i => i.UnitPrice) : default(decimal?); } }
        public int? Quantity { get { return Inventories.Any() ? Inventories.Min(i => i.Quantity) : default(int?); } }
        public int? InventoryId { get { return Inventories.Any() ? Inventories.Min(i => i.InventoryId) : default(int?); } }

        public int? QuantitySold { get { return SaleItems.Any() ? SaleItems.Min(i => i.Quantity) : default(int?); } }


        /// <summary>
        /// Url from e-commerce product
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public string Url
        {
            get
            {
                return this.Category.Url + this.Name.RemoveSpecialChars() + "," + this.ProductId + ".aspx";
            }
        }
    }
}