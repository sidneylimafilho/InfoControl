using System;
using System.Collections;
using System.Data;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;
//imports javax.io.*;

namespace Vivina.Erp.BusinessRules
{
    public partial class InventoryManager : BusinessManager<InfoControlDataContext>
    {
        public InventoryManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Inventories.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Inventory> GetAllInventories()
        {
            return DbContext.Inventories;
        }

        /// <summary>
        /// This method gets record counts of all Inventories.
        /// Do not change this method.
        /// </summary>
        public int GetAllInventoriesCount()
        {
            return GetAllInventories().Count();
        }

        /// <summary>
        /// This method retrieves a single Inventory.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=ProductId>ProductId</param>
        /// <param name=CompanyId>CompanyId</param>
        public Inventory GetInventory(Int32 CompanyId, Int32 ProductId, Int32 DepositId)
        {
            return
                DbContext.Inventories.Where(
                    x => x.CompanyId == CompanyId && x.DepositId == DepositId && x.ProductId == ProductId).
                    FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves Inventory by Company.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<Inventory> GetInventoryByCompany(Int32 CompanyId)
        {
            return DbContext.Inventories.Where(x => x.CompanyId == CompanyId);
        }

        /// <summary>
        /// This method retrieves Inventory by Supplier.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=SupplierId>SupplierId</param>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<Inventory> GetInventoryBySupplier(Int32 SupplierId, Int32 CompanyId)
        {
            return DbContext.Inventories.Where(x => x.SupplierId == SupplierId && x.CompanyId == CompanyId);
        }

        /// <summary>
        /// This method retrieves Inventory by Product.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=ProductId>ProductId</param>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<Inventory> GetInventoryByProduct(Int32 ProductId, Int32 CompanyId)
        {
            return DbContext.Inventories.Where(x => x.ProductId == ProductId && x.CompanyId == CompanyId);
        }

        /// <summary>
        /// This method retrieves Inventory by CurrencyRate.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CurrencyRateId>CurrencyRateId</param>
        public IQueryable<Inventory> GetInventoryByCurrencyRate(Int32 CurrencyRateId)
        {
            return DbContext.Inventories.Where(x => x.CurrencyRateId == CurrencyRateId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all Inventories filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetInventories(string tableName, Int32 Company_CompanyId, Int32 Supplier_SupplierId,
                                    Int32 Supplier_CompanyId, Int32 Product_ItemId, Int32 Product_CompanyId,
                                    Int32 CurrencyRate_CurrencyRateId, string sortExpression, int startRowIndex,
                                    int maximumRows)
        {
            IQueryable<Inventory> x = GetFilteredInventories(tableName, Company_CompanyId, Supplier_SupplierId,
                                                             Supplier_CompanyId, Product_ItemId, Product_CompanyId,
                                                             CurrencyRate_CurrencyRateId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "ProductId").ToList();
        }


        //
        // Seleciona o produto, por categoria ou todos, seta a paginação e a ordenação e devolve os dados 
        // para um DataTable, ideal para gridView com Ordenação por categoria. Tem de ser utilizado com
        // o método COUNT abaixo
        //
        public IList GetInventories(int companyId, int depositId, int? categoryId, bool categoriesRecursive,
                                    string sortExpression, int startRowIndex, int maximumRows)
        {
            var productManager = new ProductManager(this);
            var categoryManager = new CategoryManager(this);

            var query = from inventory in DbContext.Inventories
                        join product in productManager.GetAllProducts() on inventory.ProductId equals product.ProductId
                        join category in categoryManager.GetChildCategories(companyId, categoryId, categoriesRecursive)
                            on product.CategoryId equals category.CategoryId
                        join manufacturer in DbContext.Manufacturers on product.ManufacturerId equals
                            manufacturer.ManufacturerId into gManufactory
                        from manufacturer in gManufactory.DefaultIfEmpty()
                        where product.CompanyId == companyId && inventory.DepositId == depositId
                        select new
                                   {
                                       inventory,
                                       product.ProductId,
                                       product.Name,
                                       product.ProductCode,
                                       product.ModifiedDate,
                                       product.CompanyId,
                                       product.BarCode,
                                       product.CategoryId,
                                       product.IsActive,
                                       product.ManufacturerId,
                                       product.BarCodeTypeId,
                                       product.Description,
                                       product.DropCompositeInStock,
                                       product.AddCustomerEquipmentInSale,
                                       product.AllowNegativeStock,
                                       product.AllowSaleBelowCost,
                                       product.IPI,
                                       product.ICMS,
                                       product.FiscalClass,
                                       product.WarrantyDays,
                                       product.IdentificationOrPlaca,
                                       product.PatrimonioOrRenavam,
                                       product.SerialNumberOrChassi,
                                       ImageUrl =
                            product.ProductImages.Count > 0 ? product.ProductImages.FirstOrDefault().ImageUrl : "",
                                       ManufacturerName = manufacturer.Name ?? "",
                                       CategoryName = category.Name
                                   };

            //if (categoryId.HasValue)
            //    query = query.Where(x => x.CategoryId == categoryId);

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "Name").ToList();
        }


        /// <summary>
        /// This method retrieve the quantity of registers that GetInventories method returns
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="depositId"></param>
        /// <param name="categoryId"></param>
        /// <param name="categoriesRecursive"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetInventoriesCount(Int32 companyId, Int32 depositId, Int32? categoryId, bool categoriesRecursive,
                                         String sortExpression, Int32 startRowIndex, Int32 maximumRows)
        {
            return
                GetInventories(companyId, depositId, categoryId, categoriesRecursive, sortExpression, startRowIndex,
                               maximumRows).Count;
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<Inventory> GetFilteredInventories(string tableName, Int32 Company_CompanyId,
                                                             Int32 Supplier_SupplierId, Int32 Supplier_CompanyId,
                                                             Int32 Product_ItemId, Int32 Product_CompanyId,
                                                             Int32 CurrencyRate_CurrencyRateId)
        {
            switch (tableName)
            {
                case "Company_Inventories":
                    return GetInventoryByCompany(Company_CompanyId);
                case "Supplier_Inventories":
                    return GetInventoryBySupplier(Supplier_SupplierId, Supplier_CompanyId);
                case "Product_Inventories":
                    return GetInventoryByProduct(Product_ItemId, Product_CompanyId);
                case "CurrencyRate_Inventories":
                    return GetInventoryByCurrencyRate(CurrencyRate_CurrencyRateId);
                default:
                    return GetAllInventories();
            }
        }

        /// <summary>
        /// This method gets records counts of all Inventories filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetInventoriesCount(string tableName, Int32 Company_CompanyId, Int32 Supplier_SupplierId,
                                       Int32 Supplier_CompanyId, Int32 Product_ItemId, Int32 Product_CompanyId,
                                       Int32 CurrencyRate_CurrencyRateId)
        {
            IQueryable<Inventory> x = GetFilteredInventories(tableName, Company_CompanyId, Supplier_SupplierId,
                                                             Supplier_CompanyId, Product_ItemId, Product_CompanyId,
                                                             CurrencyRate_CurrencyRateId);
            return x.Count();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Inventory entity)
        {
            DbContext.Inventories.Attach(entity);
            DbContext.Inventories.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        private void Insert(Inventory entity)
        {
            entity.EntryDate = entity.ModifiedDate = DateTime.Now;

            DbContext.Inventories.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(Inventory original_entity, Inventory entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            original_entity.ModifiedDate = DateTime.Now;
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method checks the Product quantity
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool CheckIfCanTransfer(Int32 CompanyId, int productId, Int32 depositId, int quantity)
        {
            // Get the data of the respectively inventory
            Inventory inventory = GetProductInventory(CompanyId, productId, depositId);
            //
            // Compare the Inventory Quantity wich the Transfer quantity, if the Transfer Quantity is 
            // less than the Inventory, the Can transfer flag will be true, else the Transfer will be canceled
            //
            if (quantity <= inventory.Quantity)
                return true;
            else
                return false;
        }

        #region InventoryHistory

        /// <summary>
        /// this method return all InventoryHistories
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="DepositId"></param>
        /// <returns></returns>
        public DataTable GetInventoryHistoriesByProduct(Int32 productId, Int32 depositId)
        {
            var query = from inventoryHistories in DbContext.InventoryHistories
                        where inventoryHistories.ProductId == productId && inventoryHistories.DepositId == depositId
                        select new
                                   {
                                       inventoryHistories.DummyId,
                                       inventoryHistories.ProductId,
                                       inventoryHistories.CurrencyRateId,
                                       inventoryHistories.UnitPrice,
                                       inventoryHistories.Localization,
                                       inventoryHistories.Quantity,
                                       inventoryHistories.LogDate,
                                       inventoryHistories.MinimumRequired,
                                       inventoryHistories.RealCost,
                                       inventoryHistories.Profit,
                                       inventoryHistories.SupplierId,
                                       inventoryHistories.DestinationDepositId,
                                       inventoryHistories.DepositId,
                                       inventoryHistories.FiscalNumber,
                                       inventoryHistories.InventoryDropTypeId,
                                       inventoryHistories.InventoryEntryTypeId,
                                       inventoryHistories.CompanyId,
                                       inventoryHistories.QuantityInReserve,
                                       inventoryHistories.AverageCosts,
                                       inventoryHistories.SaleId,
                                       inventoryHistories.UserId,
                                       userName = inventoryHistories.User.Profile.Name
                                   };
            return query.OrderByDescending(x => x.LogDate).ToDataTable();
        }

        /// <summary>
        /// this method rollback inventoryHistory
        /// </summary>
        /// <param name="entity"></param>
        public void RollbackInventoryHistory(InventoryHistory entity)
        {
            //retrieve the product of Inventory
            Inventory inventory = GetProductInventory(entity.CompanyId, entity.ProductId, entity.DepositId);

            //rollback set the properties of inventoryHistory in inventory
            inventory.UnitPrice = entity.UnitPrice;
            inventory.Localization = entity.Localization;
            inventory.MinimumRequired = entity.MinimumRequired;
            inventory.RealCost = entity.RealCost;
            inventory.Profit = entity.Profit;
            inventory.FiscalNumber = entity.FiscalNumber;
            inventory.Quantity = entity.Quantity;
            inventory.AverageCosts = entity.AverageCosts.Value;
            DbContext.SubmitChanges();

            //delete the last row of inventoryHistory
            DbContext.InventoryHistories.Attach(entity);
            DbContext.InventoryHistories.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        #endregion
    }
}