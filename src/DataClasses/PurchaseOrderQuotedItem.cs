using System;

namespace Vivina.Erp.DataClasses
{
    [Serializable]
    public class PurchaseOrderQuotedItem
    {
        #region Properties
        public String Name { get; set; }
        public Decimal Price { get; set; }
        public Int32 Quantity { get; set; }
        public Int32 QuantityReceived { get; set; }
        public String SupplierName { get; set; }
        public Int32 ProductId { get; set; }
        public Int32 ProductPackageId { get; set; }
        public Int32? ProductManufacturerId { get; set; }
        public Int32 PurchaseRequestItemId { get; set; }
        public Int32 PurchaseOrderItemId { get; set; }
        public DateTime? DeliveryDate { get; set; }

        #endregion

        public PurchaseOrderQuotedItem(String name, Int32 quantity, Int32 quantityReceived, String supplierName, Decimal price, Int32 productId, Int32 productPackageId, Int32? productManufacturerId, Int32 purchaseRequestItemId, DateTime? deliveryDate, int purchaseOrderItemId)
        {
            Name = name;
            Quantity = quantity;
            QuantityReceived = quantityReceived;
            SupplierName = supplierName;
            Price = price;
            ProductId = productId;
            ProductManufacturerId = productManufacturerId;
            ProductPackageId = productPackageId;
            PurchaseRequestItemId = purchaseRequestItemId;
            DeliveryDate = deliveryDate;
            PurchaseOrderItemId = purchaseOrderItemId;
        }
    }
}