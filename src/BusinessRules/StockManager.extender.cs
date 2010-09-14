using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;



namespace Vivina.Erp.BusinessRules
{
    public enum InventoryStatus
    {
        NegativeQuantity,
        Success
    }

    public enum DropType
    {
        Defect = 1,
        Miscarriage = 2,
        Use = 3,
        Sell = 4,
        Transfer = 5,
        RMA = 9
    }

    public enum EntryType
    {
        Purchase = 1,
        Transfer = 2,
        Devolution = 3,
        RMA = 5
    }

    public partial class InventoryManager
    {

        /// <summary>
        /// This method execute a stockDeposit 
        /// </summary>
        /// <param name="currentInventory"></param>
        /// <param name="supplierId"></param>
        public void StockDeposit(Inventory currentInventory, int? supplierId, Int32? userId)
        {
            Inventory originalInventory = GetProductInventory(currentInventory.CompanyId,
                                                              currentInventory.ProductId,
                                                              currentInventory.DepositId);


            if (originalInventory != null && originalInventory.DepositId == currentInventory.DepositId)
            {
                originalInventory.SupplierId = currentInventory.SupplierId;
                originalInventory.Quantity += currentInventory.Quantity;

                if (userId.HasValue)
                    InsertHistoryFromInventory(originalInventory, null, null, userId);


                if ((originalInventory.Quantity + currentInventory.Quantity) != 0)
                {
                    originalInventory.AverageCosts = ((originalInventory.Quantity * originalInventory.RealCost) +
                                                      (currentInventory.Quantity * currentInventory.RealCost))
                                                     / (originalInventory.Quantity + currentInventory.Quantity);
                }
                DbContext.SubmitChanges();
            }
            else
            {
                currentInventory.SupplierId = supplierId;
                currentInventory.AverageCosts = currentInventory.RealCost;
                Insert(currentInventory);
            }
        }


        /// <summary>
        /// This method adds quantity of specified product in deposit
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="productId"></param>
        /// <param name="depositId"></param>
        /// <param name="quantity"></param>
        public void AddQuantityInDeposit(Int32 companyId, Int32? userId, Int32 productId, Int32 depositId, Int32 quantity)
        {
            var inventoryProduct = GetProductInventory(companyId, productId, depositId);

            inventoryProduct.Quantity += quantity;
            DbContext.SubmitChanges();

            //
            // Create history from inventory 
            //

            if (userId.HasValue)
                InsertHistoryFromInventory(inventoryProduct, null, null, userId);
        }

        /// <summary>
        /// This method creates a history for inventory modified
        /// </summary>
        /// <param name="inventory">the modified inventory </param>
        public void InsertHistoryFromInventory(Inventory inventory, Int32? dropTypeId, Int32? saleId, Int32? userId)
        {
            var inventoryHistory = new InventoryHistory();

            inventoryHistory.InventoryDropTypeId = dropTypeId;
            inventoryHistory.SaleId = saleId;
            inventoryHistory.UserId = userId;

            inventoryHistory.CompanyId = inventory.CompanyId;
            inventoryHistory.AverageCosts = inventory.AverageCosts;
            inventoryHistory.CurrencyRateId = inventory.CurrencyRateId;
            inventoryHistory.DepositId = inventory.DepositId;
            inventoryHistory.FiscalNumber = inventory.FiscalNumber;

            inventoryHistory.Localization = inventory.Localization;
            inventoryHistory.ProductId = inventory.ProductId;
            inventoryHistory.Profit = inventory.Profit;
            inventoryHistory.MinimumRequired = inventory.MinimumRequired;
            inventoryHistory.RealCost = inventory.RealCost;

            inventoryHistory.SupplierId = inventory.SupplierId;
            inventoryHistory.UnitPrice = inventory.UnitPrice;
            inventoryHistory.LogDate = DateTime.Now;
            inventoryHistory.Quantity = inventory.Quantity;

            InsertHistory(inventoryHistory);
        }

        public void InventoryDrop(Inventory currentInventory, int? quantity, Int32 dropTypeId, Int32? saleId)
        {
            Inventory originalInventory = GetProductInventory(currentInventory.CompanyId, currentInventory.ProductId,
                                                              currentInventory.DepositId);

            var inventoryManager = new InventoryManager(this);
            if (dropTypeId != (int)DropType.Sell &&
                !inventoryManager.CheckIfCanTransfer(currentInventory.CompanyId, currentInventory.ProductId,
                                                     currentInventory.DepositId, quantity.Value))
                throw new InvalidOperationException();

            //
            //insert one row in InventoryHistory
            //

            InsertHistoryFromInventory(originalInventory, dropTypeId, saleId, null);

            originalInventory.Quantity -= Convert.ToInt32(quantity);
           
            //
            //drop StockComposite
            //
           
            var productManager = new ProductManager(this);

            Product product = productManager.GetProduct(originalInventory.ProductId);

            if (product.DropCompositeInStock)
            {
                List<CompositeProduct> listCompositProduct =
                    productManager.GetChildCompositeProducts(product.ProductId).ToList();
                for (int i = 0; i < listCompositProduct.Count; i++)
                {
                    Inventory compositeInventory = GetProductInventory(currentInventory.CompanyId,
                                                                       listCompositProduct[i].ProductId,
                                                                       originalInventory.DepositId);
                    if (compositeInventory != null)
                        InventoryDrop(compositeInventory, 1, dropTypeId, saleId);
                }
            }

            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Retorna uma linha da tabela Inventory onde o produto tenha DepositId e o ProductId
        /// iguais aos parâmetros da função
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="depositId"></param>
        /// <returns></returns>
        public Inventory GetProductInventory(Int32 companyId, Int32 productId, Int32 depositId)
        {
            return GetProductInventory(companyId, productId, null, depositId);
        }

        /// <summary>
        /// Retorna uma linha da tabela Inventory onde o produto tenha DepositId e o ProductId
        /// iguais aos parâmetros da função
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="depositId"></param>
        /// <returns></returns>
        public Inventory GetProductInventory(Int32 companyId, Int32 productId, int? packageId, Int32 depositId)
        {
            IQueryable<Inventory> query = DbContext.Inventories.Where(i => i.CompanyId == companyId &&
                                                                           i.ProductId == productId &&
                                                                           i.DepositId == depositId);

            //if(packageId.HasValue)
            //    query = query.Where(x => x.ProductPackageId == packageId.Value);

            return query.FirstOrDefault();
        }

        /// <summary>
        /// This method is a selector for 3 distinct functions
        /// Returns a single Deposit
        /// Returns all deposits for a specific CompanyId
        /// Or returns all deposits of all companys related by ReferenceCompanyID
        /// </summary>
        /// <param name="depositId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable GetInventoriesByDeposit(string depositId, int companyId)
        {
            switch (depositId)
            {
                case "Company":
                    return GetProductsByDepositInThisCompany(companyId);

                case "Matrix":
                    return GetProductsByDepositInMatrix(companyId);

                default:
                    return GetProductsByDeposit(Convert.ToInt16(depositId));
            }
        }

        /// <summary>
        /// This method returns all products of a deposit
        /// </summary>
        /// <param name="depositId"></param>
        /// <returns></returns>
        public DataTable GetProductsByDeposit(Int32 depositId)
        {
            var query =
                from prod in DbContext.Products
                join inv in DbContext.Inventories on prod.ProductId equals inv.ProductId
                join comp in DbContext.Companies on inv.CompanyId equals comp.CompanyId
                join dep in DbContext.Deposits on inv.DepositId equals dep.DepositId
                where (inv.DepositId == depositId)
                select new { prod.Name, prod.ProductId, inv.Quantity, inv.UnitPrice, inv.AverageCosts, inv.Localization };
            return query.ToDataTable();
        }

        public Int32 GetProductsByDepositCount(Int32 companyId, Int32? categoryId, Int32? manufacturerId,
                                               Int32 depositId, String description, String name, string sortExpression,
                                               int startRowIndex, int maximumRows)
        {
            return
                GetProductsByDeposit(companyId, categoryId, manufacturerId, depositId, description, name, sortExpression,
                                     startRowIndex, maximumRows).Cast<Object>().Count();
        }


        /// <summary>
        /// This method returns products in inventories related with a specific deposit
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="categoryId"></param>
        /// <param name="manufacturerId"></param>
        /// <param name="depositId"></param>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetProductsByDeposit(Int32 companyId, Int32? categoryId, Int32? manufacturerId,
                                               Int32 depositId, String description, String name, string sortExpression,
                                               int startRowIndex, int maximumRows)
        {
            var query = from product in DbContext.Products
                        join inventory in GetAllInventories() on product.ProductId equals inventory.ProductId
                        where inventory.DepositId == depositId && inventory.CompanyId == companyId
                        select new
                                   {
                                       product.Name,
                                       product.ProductCode,
                                       product.ProductId,
                                       product.CategoryId,
                                       product.ManufacturerId,
                                       product.Description,
                                       inventory.Quantity,
                                       inventory.UnitPrice,
                                       inventory.InventoryId
                                   };

            if (categoryId.HasValue)
                query = query.Where(product => product.CategoryId == categoryId);

            if (manufacturerId.HasValue)
                query = query.Where(product => product.ManufacturerId == manufacturerId);

            if (!String.IsNullOrEmpty(name))
                query = query.Where(product => product.Name.Contains(name));

            if (!String.IsNullOrEmpty(description))
                query = query.Where(product => product.Description.Contains(description));

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "ProductId");
        }

        /// <summary>
        /// Get all products of all deposits of all companies related with the matrix company
        /// </summary>
        /// <param name="matrixId"></param>
        /// <returns></returns>
        public DataTable GetProductsByDepositInMatrix(Int32 matrixId)
        {
            string sql =
                @"SELECT
                                (SUM(t2.AverageCosts) / Count(*)) AS AverageCosts, SUM(t2.MinimumRequired) AS MinimumRequired, 
                                SUM(t2.Quantity) AS Quantity, AVG(t2.UnitPrice) AS UnitPrice, 
                                t3.ProductId, t3.Name, t3.ProductCode
                           FROM Company AS t0 
                           INNER JOIN Deposit AS t1 ON t0.CompanyId = t1.CompanyId 
                           INNER JOIN Inventory AS t2 ON t1.DepositId = t2.DepositId 
                           INNER JOIN Product AS t3 ON t2.ProductId = t3.ProductId
                           WHERE (t0.MatrixId = @MatrixId)
                           GROUP BY t3.ProductId, t3.Name, t3.ProductCode";

            DataManager.Parameters.Add("@MatrixId", matrixId);
            return DataManager.ExecuteDataTable(sql);
        }

        /// <summary>
        /// Return all products of all deposits of a company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable GetProductsByDepositInThisCompany(Int32 companyId)
        {
            string sql =
                @"SELECT
                                (SUM(t2.AverageCosts) / Count(*)) as AverageCosts, SUM(t2.MinimumRequired) AS MinimumRequired, 
                                SUM(t2.Quantity) AS Quantity, AVG(t2.UnitPrice) AS UnitPrice, 
                                t3.ProductId, t3.Name, t3.ProductCode
                           FROM Company AS t0 
                           INNER JOIN Deposit AS t1 ON t0.CompanyId = t1.CompanyId 
                           INNER JOIN Inventory AS t2 ON t1.DepositId = t2.DepositId 
                           INNER JOIN Product AS t3 ON t2.ProductId = t3.ProductId
                           WHERE (t0.CompanyId = @companyId)
                           GROUP BY t3.ProductId, t3.Name, t3.ProductCode";

            DataManager.Parameters.Add("@companyId", companyId);
            return DataManager.ExecuteDataTable(sql);
        }

        /// <summary>
        /// Retorna um Datatable contendo os dados do produto e sua quantidade
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="depositId"></param>
        /// <returns></returns>
        public DataTable GetProductsInInventory(int productId, string depositId, int companyId)
        {
            var query = from product in DbContext.Products
                        join inv in DbContext.Inventories on product.ProductId equals inv.ProductId
                        where product.ProductId == productId && inv.DepositId == Convert.ToInt16(depositId)
                        select new
                                   {
                                       product.Name,
                                       product.ProductCode,
                                       product.BarCode,
                                       inv.Quantity,
                                       product.BarCodeTypeId
                                   };
            return query.ToDataTable();
        }

        /// <summary>
        /// This function allow the user to change all inventory prices or all company prices
        /// by passing a companyId. If you pass the CompanyId the function will change all the company values
        /// If you pass the MatrixId the function will change all values
        /// </summary>
        /// <param name="companyId"></param>
        public void AdjustInventoryPrice(int companyId, decimal unitPrice, decimal profitMargin, int productId)
        {
            string sql =
                @"UPDATE Inventory
                           SET UnitPrice = @unitPrice, Profit = @profitMargin 
                           FROM Company Comp 
                           INNER JOIN Deposit Dep ON Comp.CompanyId = Dep.CompanyId
                           INNER JOIN Inventory Inv ON Inv.CompanyId = Dep.CompanyId
                           WHERE ((Comp.CompanyId = @companyId) OR (Comp.MatrixId = @companyId)) 
                           AND (Inv.ProductId = @productId) ";

            DataManager.Parameters.Add("@productId", productId);
            DataManager.Parameters.Add("@unitPrice", unitPrice);
            DataManager.Parameters.Add("@profitMargin", profitMargin);
            DataManager.Parameters.Add("@companyId", companyId);
            DataManager.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// Método para inserir um RMA (Defeito/Troca) na tabela, qualquer dado nesta tabela, consta como
        /// se o produto estivesse em trânsito, aguardando uma confirmação da troca pelo usuário
        /// </summary>
        /// <param name="entity"></param>
        public void InsertStockRMA(InventoryRMA entity)
        {
            DbContext.InventoryRMAs.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Retorna uma DataTable contendo os dados do produto, fornecedor e do RMA.
        /// Verifica tanto os RMA que já foram trocados, quanto os que aguardam a troca
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="DepositId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable RetrieveRMA(int companyId, int depositId, string sortExpression, int startRowIndex,
                                      int maximumRows)
        {
            var query = from rma in DbContext.InventoryRMAs
                        join prod in DbContext.Products on rma.ProductId equals prod.ProductId
                        join sup in DbContext.Suppliers on rma.SupplierId equals sup.SupplierId
                        where
                            (
                                rma.CompanyId == companyId &&
                                rma.DepositId == depositId
                            )
                        select new
                                   {
                                       rma.InventoryRMAId,
                                       rma.ModifiedDate,
                                       rma.Quantity,
                                       ProductName = prod.Name,
                                       prod.ProductId,
                                       SupplierName = sup.Profile.Name
                                   };
            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "InventoryRMAId");
        }

        /// <summary>
        /// This method count registers returned by RetrieveRMA
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="DepositId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 RetrieveRMACount(int companyId, int depositId, string sortExpression, int startRowIndex,
                                      int maximumRows)
        {
            return
                RetrieveRMA(companyId, depositId, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method delete RMA
        /// </summary>
        /// <param name="RMAId"></param>
        public void DeleteRMA(int RMAId)
        {
            /*
             * This method performs all functions 
             * necessary for the exclusion of an RMA
            */

            var inventory = new Inventory();
            var rma = new InventoryRMA();
            var inventoryManager = new InventoryManager(this);
            var history = new InventoryHistory();

            rma = inventoryManager.RetrieveRMA(RMAId); //retrieve the RMA by RMAId
            inventory = inventoryManager.GetProductInventory(rma.CompanyId, rma.ProductId, rma.DepositId);
            //Retrieve The Inventory
            //set InventotyHistory
            history.CompanyId = inventory.CompanyId;
            history.CurrencyRateId = inventory.CurrencyRateId;
            history.DepositId = rma.DepositId;
            history.FiscalNumber = inventory.FiscalNumber;
            history.InventoryEntryTypeId = (int)EntryType.RMA;
            history.Localization = inventory.Localization;
            history.LogDate = DateTime.Now;
            history.MinimumRequired = inventory.MinimumRequired;
            history.ProductId = rma.ProductId;
            history.Profit = inventory.Profit;
            history.Quantity = rma.Quantity;
            history.RealCost = inventory.RealCost;
            history.SupplierId = inventory.SupplierId;
            history.UnitPrice = inventory.UnitPrice;

            //set quantity and AverageCost in inventory
            inventory.Quantity += rma.Quantity;
            inventory.AverageCosts = (((inventory.Quantity * inventory.RealCost) + (rma.Quantity * inventory.RealCost)) /
                                      (inventory.Quantity + rma.Quantity));
            //applay the changes
            inventoryManager.InsertHistory(history);
            DbContext.InventoryRMAs.DeleteOnSubmit(rma);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Retorna uma linha da tabela de RMA
        /// </summary>
        /// <param name="inventoryRMAId"></param>
        /// <returns></returns>
        public InventoryRMA RetrieveRMA(int inventoryRMAId)
        {
            return DbContext.InventoryRMAs.Where(x => x.InventoryRMAId == inventoryRMAId).FirstOrDefault();
        }

        /// <summary>
        /// Método que insere uma linha no histórico de tranferência
        /// </summary>
        /// <param name="entity"></param>
        public void InsertHistory(InventoryHistory entity)
        {
            DbContext.InventoryHistories.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        #region InventorySerial

        /// <summary>
        /// this method return Iqueryable of InventorySerials
        /// </summary>
        /// <param name="depositId"></param>
        /// <param name="productId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<InventorySerial> GetInventorySerials(Int32 inventoryId, string sortExpression,
                                                               int startRowIndex, int maximumRows)
        {
            return DbContext.InventorySerials.Where(s => s.InventoryId == inventoryId).SortAndPage(sortExpression,
                                                                                                   startRowIndex,
                                                                                                   maximumRows,
                                                                                                   "InventorySerialId");
        }

        /// <summary>
        /// this method return the number of rows of GetInventorySerials
        /// </summary>
        /// <param name="depositId"></param>
        /// <param name="productId"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetInventorySerialsCount(Int32 inventoryId, string sortExpression, int startRowIndex,
                                              int maximumRows)
        {
            return GetInventorySerials(inventoryId, sortExpression, startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// this method return one DataTable of InventorySerials
        /// </summary>
        /// <param name="depositId"></param>
        /// <param name="productId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable GetInventorySerials(Int32 depositId, Int32 productId, Int32 companyId)
        {
            return
                DbContext.InventorySerials.Where(
                    s => s.CompanyId == companyId && s.Inventory.ProductId == productId && s.DepositId == depositId).
                    ToDataTable();
        }

        /// <summary>
        /// this method Insert the InventorySerial
        /// </summary>
        /// <param name="entity"></param>
        public void InsertInventorySerial(InventorySerial entity)
        {
            DbContext.InventorySerials.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method delete the InventorySerial 
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteInventorySerial(Int32 inventorySerialId)
        {
            DbContext.InventorySerials.DeleteOnSubmit(GetInventorySerial(inventorySerialId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method retrieve a specific invetorySerial entity
        /// </summary>
        /// <param name="inventorySerialId"></param>
        /// <returns></returns>
        public InventorySerial GetInventorySerial(Int32 inventorySerialId)
        {
            return DbContext.InventorySerials.Where(inventorySerial => inventorySerial.InventorySerialId == inventorySerialId).FirstOrDefault();
        }

        #endregion

        #region Tranfers

        /// <summary>
        /// this method execute all functions Necessary to Moviment a Product,
        /// the functions are InsertHistory(insert one row in InventoryHistory),InventoryDrop(Drop the product from inventory)
        ///  and InventoryMoviments(insert onr row in InventoryMoviments)
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="inventoryMoviment"></param>
        /// <param name="quantity"></param>
        public void InsertStockMovement(Inventory inventory, InventoryMoviment inventoryMoviment, Int32 quantity,
                                        Int32 userId)
        {
            Inventory inventoryData;

            //insert one row in InventoryHistory
            inventoryData = GetProductInventory(inventory.CompanyId, inventory.ProductId, inventory.DepositId);

            //drop the inventory(product and quantity)
            InventoryDrop(inventory, quantity, (int)DropType.Transfer, null);

            //insert the moviments
            DbContext.InventoryMoviments.InsertOnSubmit(inventoryMoviment);

            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Retorna linhas da tabela de movimentação daquela company.
        /// Somente linhas que não foram recusadas pela company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="destinationCompanyId"></param>
        /// <param name="destinationDepositId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable RetrievePendingTransfers(int companyId, int destinationCompanyId, int destinationDepositId,
                                                   string sortExpression, int startRowIndex, int maximumRows)
        {
            var query = from mov in DbContext.InventoryMoviments
                        join prod in DbContext.Products on mov.ProductId equals prod.ProductId
                        where
                            (
                                mov.Refused != true &&
                                mov.CompanyId == companyId &&
                                mov.CompanyDestinationId == destinationCompanyId &&
                                mov.DepositDestinationId == destinationDepositId
                            )
                        select new
                                   {
                                       mov,
                                       mov.InventoryMovementId,
                                       mov.CompanyDestinationId,
                                       mov.Quantity,
                                       mov.ModifiedDate,
                                       prod.Name,
                                       prod.ProductId
                                   };
            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "InventoryMovementId");
        }

        /// <summary>
        /// This method count registers returned by retrievePendingTransfers
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="destinationCompanyId"></param>
        /// <param name="destinationDepositId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 RetrievePendingTransfersCount(int companyId, int destinationCompanyId, int destinationDepositId,
                                                   string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                RetrievePendingTransfers(companyId, destinationCompanyId, destinationDepositId, sortExpression,
                                         startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// Retorna todas as linhas da tabela de movimento, inclusive as que foram recusadas
        /// </summary>
        /// <param name="companyId">companyId</param>
        /// <returns></returns>
        public IQueryable RetrieveTransfers(int companyId, int DepositId, string sortExpression, int startRowIndex,
                                            int maximumRows)
        {
            var query = from mov in DbContext.InventoryMoviments
                        join prod in DbContext.Products on mov.ProductId equals prod.ProductId
                        join comp in DbContext.Companies on mov.CompanyDestinationId equals comp.CompanyId
                        where (mov.CompanyId == companyId && mov.DepositId == DepositId)
                        select new
                                   {
                                       mov.InventoryMovementId,
                                       mov.CompanyDestinationId,
                                       mov.Quantity,
                                       mov.ModifiedDate,
                                       mov.Refused,
                                       ProductName = prod.Name,
                                       prod.ProductId,
                                       comp.LegalEntityProfile.CompanyName
                                   };
            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "InventoryMovementId");
        }

        /// <summary>
        /// This method count registers returned by RetrieveTransfers
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="DepositId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 RetrieveTransfersCount(int companyId, int DepositId, string sortExpression, int startRowIndex,
                                            int maximumRows)
        {
            return
                RetrieveTransfers(companyId, DepositId, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().
                    Count();
        }

        /// <summary>
        /// Retorna uma linha específica da tabela de movimentação, faz a busca pelo ID
        /// </summary>
        /// <param name="inventoryMovementId">inventoryMovementId</param>
        /// <returns></returns>
        public InventoryMoviment RetrievePendingTransfers(int inventoryMovementId)
        {
            IQueryable<InventoryMoviment> query =
                from mov in DbContext.InventoryMoviments
                where (mov.InventoryMovementId == inventoryMovementId)
                select mov;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Retorna o nome das Companhias que tem items "em trânsito"
        /// </summary>
        /// <param name="companyId">CompanyId</param>
        /// <returns></returns>
        public DataTable RetrieveCompanyTransferName(int companyId)
        {
            var sql = from comp in DbContext.Companies
                      join inventory in DbContext.InventoryMoviments on comp.CompanyId equals inventory.CompanyId
                      join legaProfile in DbContext.LegalEntityProfiles on comp.LegalEntityProfileId equals
                          legaProfile.LegalEntityProfileId
                      where (comp.CompanyId == inventory.CompanyId && inventory.CompanyDestinationId == companyId)
                      orderby comp.CompanyId
                      select new { comp.CompanyId, Name = legaProfile.CompanyName };

            return sql.ToDataTable();

            //            string sql = @" SELECT 
            //                                Company.Name, Company.CompanyId
            //                            FROM 
            //                                Company, InventoryMoviment
            //                            WHERE
            //                                Company.CompanyId = InventoryMoviment.CompanyId AND
            //                                InventoryMoviment.CompanyDestinationId = @companyId
            //                            GROUP BY
            //                                Company.Name, Company.CompanyId";

            //            DataManager.Parameters.Add("@companyId", companyId);
            //            return DataManager.ExecuteDataTable(sql);
        }

        /// <summary>
        /// this method transfer a product Between two deposit
        /// </summary>
        /// <param name="moviment"></param>
        public void TransferStockDeposit(InventoryMoviment moviment, Int32 userId)
        {
            //company Source Inventory Moviment
            InventoryMoviment sourceInventoryMoviment = RetrievePendingTransfers(moviment.InventoryMovementId);

            //company Souce Inventory
            Inventory sourceInventory = GetProductInventory(moviment.CompanyId, moviment.ProductId, moviment.DepositId);

            //company targeting Inventory
            Inventory destinationInventory = GetProductInventory(moviment.CompanyId, moviment.ProductId,
                                                                 moviment.DepositDestinationId);

            //insert Inventory History
            var history = new InventoryHistory();
            history.Quantity = sourceInventory.Quantity;
            history.CurrencyRateId = sourceInventory.CurrencyRateId;
            history.FiscalNumber = sourceInventory.FiscalNumber;
            history.Localization = sourceInventory.Localization;
            history.MinimumRequired = sourceInventory.MinimumRequired;
            history.ProductId = sourceInventory.ProductId;
            history.Profit = sourceInventory.Profit;
            history.RealCost = sourceInventory.RealCost;
            history.SupplierId = sourceInventory.SupplierId;
            history.UnitPrice = sourceInventory.UnitPrice;
            history.CompanyId = moviment.CompanyId;
            history.DepositId = sourceInventory.DepositId;
            history.DestinationDepositId = moviment.DepositDestinationId;
            history.InventoryEntryTypeId = (int)EntryType.Transfer;
            history.LogDate = DateTime.Now;
            history.UserId = userId;
            //insert History
            InsertHistory(history);

            if (destinationInventory != null)
            {
                //set the new AverageCost
                destinationInventory.AverageCosts =
                    ((destinationInventory.Quantity * destinationInventory.UnitPrice
                      + sourceInventoryMoviment.Quantity * sourceInventory.UnitPrice) /
                     (sourceInventoryMoviment.Quantity + destinationInventory.Quantity));

                //set the new Quantity
                destinationInventory.Quantity += sourceInventoryMoviment.Quantity;
            }
            else
            {
                //insert a Inventory if the product not exists in the target inventory
                //set the new depositId and companyId
                var newInventory = new Inventory();
                newInventory.CopyPropertiesFrom(sourceInventory);
                newInventory.DepositId = moviment.DepositDestinationId;
                newInventory.CompanyId = moviment.CompanyId;
                Insert(newInventory);
            }


            //delete Moviment
            DeleteMovement(moviment);

            //submit changes
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Método que faz a entrada do estoque dos produtos "em trânsito"
        /// Quando o usuário aceita a transferência, esse método é utilizado
        /// </summary>
        /// <param name="DestinationDepositId"></param>
        /// <param name="SourceDepositId"></param>
        /// <param name="ProductId"></param>
        /// <param name="Quantity"></param>
        /// <param name="CompanyId"></param>
        public void TransferStockDeposit(int DestinationDepositId, int SourceDepositId, int ProductId, int Quantity,
                                         int CompanyId)
        {
            //InventoryManager inventoryManager = new InventoryManager(this);
            //Inventory destinationInventory = inventoryManager.GetProductInventory(ProductId, DestinationDepositId);
            //Inventory inventory = new Inventory();

            //if (destinationInventory == null)
            //{
            //    Inventory sourceInventory = inventoryManager.GetProductInventory(ProductId, SourceDepositId);
            //    inventory.CopyPropertiesFrom(sourceInventory);

            //    inventory.DepositId = DestinationDepositId;
            //    inventory.EntryDate = DateTime.Now;
            //    inventory.ModifiedDate = DateTime.Now;
            //    inventory.QuantityInReserve = 0;
            //    inventory.CompanyId = CompanyId;
            //    inventory.Quantity = Quantity;
            //    inventory.SupplierId = sourceInventory.SupplierId;
            //    //inventory.SupplierId = null;
            //    Insert(inventory);
            //}
            //else
            //{
            //    inventory = inventoryManager.GetProductInventory(ProductId, DestinationDepositId);
            //    inventory.Quantity += Quantity;
            //    //set the new AverageCost
            //    inventory.AverageCosts = ((destinationInventory.Quantity * destinationInventory.UnitPrice + inventory.Quantity * inventory.UnitPrice) / (destinationInventory.Quantity + inventory.Quantity));
            //    Update(destinationInventory, inventory);
            //}

            //InventoryHistory history = new InventoryHistory();
            //history.Quantity = Quantity;
            //history.CurrencyRateId = inventory.CurrencyRateId;
            //history.FiscalNumber = inventory.FiscalNumber;
            //history.Localization = inventory.Localization;
            //history.MinimumRequired = inventory.MinimumRequired;
            //history.ProductId = inventory.ProductId;
            //history.Profit = inventory.Profit;
            //history.RealCost = inventory.RealCost;
            //history.SupplierId = inventory.SupplierId;
            //history.UnitPrice = inventory.UnitPrice;
            //history.CompanyId = CompanyId;
            //history.DepositId = DestinationDepositId;
            //history.InventoryEntryTypeId = (int)EntryType.Transfer;
            //history.LogDate = DateTime.Now;
            //history.SupplierId = null;
            //InsertHistory(history);
        }

        /// <summary>
        /// Método para apagar um registro da tabela de movimento, ou seja retirá-lo do status "em trânsito"
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteMovement(InventoryMoviment entity)
        {
            //DbContext.InventoryMoviments.Attach(entity);
            DbContext.InventoryMoviments.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Método usado para se recusar um recebimento, este método não apaga a linha da tabela,
        /// Mas seta o parametro Refused, como true
        /// </summary>
        /// <param name="inventoryMovementId"></param>
        public void RefuseMovement(int inventoryMovementId)
        {
            IQueryable<InventoryMoviment> query =
                from mov in DbContext.InventoryMoviments
                where (mov.InventoryMovementId == inventoryMovementId)
                select mov;
            query.FirstOrDefault().Refused = true;
            DbContext.SubmitChanges();
        }

        #endregion
    }
}