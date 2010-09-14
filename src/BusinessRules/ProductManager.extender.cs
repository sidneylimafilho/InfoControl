using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;
using InfoControl.Security.Cryptography;

namespace Vivina.Erp.BusinessRules
{
    public partial class ProductManager
    {
        #region ProductPart

        /// <summary>
        /// Get a single product part
        /// </summary>
        /// <param name="productPartId"></param>
        /// <returns></returns>
        public ProductPart GetProductPart(int productPartId)
        {
            return DbContext.ProductParts.Where(x => x.ProductPartId == productPartId).FirstOrDefault();
        }

        /// <summary>
        /// Return all product parts of a company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<ProductPart> GetProductPartsByCompany(int companyId)
        {
            return from productPart in DbContext.ProductParts
                   join product in GetAllProducts() on productPart.ProductId equals product.ProductId
                   where product.CompanyId == companyId
                   select productPart;
        }

        /// <summary>
        /// Basic inser method
        /// </summary>
        /// <param name="entity"></param>
        public void InsertProductPart(ProductPart entity)
        {
            DbContext.ProductParts.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Basic delete method
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteProductPart(ProductPart entity)
        {
            DbContext.ProductParts.Attach(entity);
            DbContext.ProductParts.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        #endregion

        #region Composite

        /// <summary>
        /// Returns all composite products with the same parent product
        /// </summary>
        /// <param name="parentProductId"></param>
        /// <returns></returns>
        public IQueryable<CompositeProduct> GetChildCompositeProducts(int compositeProductId)
        {
            return DbContext.CompositeProducts.Where(x => x.CompositeProductId == compositeProductId);
        }

        /// <summary>
        /// Returns all composite products with the same parent product
        /// </summary>
        /// <param name="parentProductId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<CompositeProduct> GetChildCompositeProducts(Int32 compositeProductId, string sortExpression,
                                                                      int startRowIndex, int maximumRows)
        {
            IQueryable<CompositeProduct> query = from compositeProducts in DbContext.CompositeProducts
                                                 join products in GetAllProducts() on compositeProducts.ProductId equals
                                                     products.ProductId
                                                 where compositeProducts.CompositeProductId == compositeProductId
                                                 select compositeProducts;

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "CompositeId");
        }

        public Int32 GetChildCompositeProductsCount(Int32 parentProductId, string sortExpression, int startRowIndex,
                                                    int maximumRows)
        {
            return
                GetChildCompositeProducts(parentProductId, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>()
                    .Count();
        }

        /// <summary>
        /// Get all composite of a company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="parentProductId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IList GetCompositeByCompany(Int32 companyId, Int32 compositeProductId, string sortExpression,
                                           int startRowIndex, int maximumRows)
        {
            var query =
                from products in GetAllProducts()
                join cProducts in DbContext.CompositeProducts on products.ProductId equals cProducts.ProductId
                join manuf in DbContext.Manufacturers on products.ManufacturerId equals manuf.ManufacturerId
                where (products.CompanyId == companyId && cProducts.CompositeProductId == compositeProductId)
                select new
                           {
                               products,
                               ManufacturerName = manuf.Name
                           };
            return query.ToList();
        }

        /// <summary>
        /// This method insert a new composite product
        /// </summary>
        /// <param name="entity"></param>
        public void AddComposite(CompositeProduct entity)
        {
            DbContext.CompositeProducts.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(CompositeProduct entity)
        {
            DbContext.CompositeProducts.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method remove a composite product
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="parentProductsId"></param>
        /// <param name="productsId"></param>
        public void RemoveComposite(Int32 compositeId)
        {
            CompositeProduct cProduct =
                DbContext.CompositeProducts.Where(x => x.CompositeId == compositeId).FirstOrDefault();
            Delete(cProduct);
        }

        #endregion

        #region product

        //
        // Método utilizado para fazer uma busca por qualquer parâmetro na tabela de produtos
        //
        public DataTable SearchProducts(Hashtable ht)
        {
            DataManager.Parameters.Add("@CompanyId", ht["CompanyId"]);

            var sql = new StringBuilder();
            sql.Append(
                @"SELECT prod.ProductId, prod.Name, prod.ProductCode, prod.CompanyId, prod.BarCode,
                                  prod.CategoryId, prod.IsActive, prod.ManufacturerId,inv.Quantity, inv.MinimumRequired,
                                  inv.SupplierId, inv.DepositId, cat.Name as Category,
                                  dep.Name as Deposit, Manuf.Name as Manufacturer
                           FROM   Product AS prod LEFT OUTER JOIN
                                  Inventory AS inv ON prod.ProductId = inv.ProductId LEFT OUTER JOIN 
                                  Manufacturer AS manuf ON manuf.ManufacturerId = prod.ManufacturerId LEFT OUTER JOIN
                                  Deposit AS dep ON dep.DepositId = inv.DepositId INNER JOIN 
                                  Category AS Cat ON prod.CategoryId = Cat.CategoryId ");

            var where = new StringBuilder();
            where.Append(" WHERE prod.CompanyId = @CompanyId AND");

            if (!string.IsNullOrEmpty(ht["Name"] as String))
            {
                DataManager.Parameters.Add("@Name", "%" + ht["Name"] + "%");
                where.Append(" prod.Name LIKE @Name AND");
            }
            if (!string.IsNullOrEmpty(ht["ProductCode"] as String))
            {
                DataManager.Parameters.Add("@ProductCode", "%" + ht["ProductCode"] + "%");
                where.Append(" prod.ProductCode LIKE @ProductCode AND");
            }
            if (!string.IsNullOrEmpty(ht["ManufacturerId"] as String))
            {
                DataManager.Parameters.Add("@ManufacturerId", ht["ManufacturerId"]);
                where.Append(" prod.ManufacturerId = @ManufacturerId AND");
            }
            if (!string.IsNullOrEmpty(ht["IsActive"] as String))
            {
                DataManager.Parameters.Add("@IsActive", ht["IsActive"]);
                where.Append(" prod.IsActive = @IsActive AND");
            }
            if (!string.IsNullOrEmpty(ht["QuantityStart"] as String))
            {
                DataManager.Parameters.Add("@QuantityStart", ht["QuantityStart"]);
                where.Append(" inv.Quantity >= @QuantityStart AND");
            }
            if (!string.IsNullOrEmpty(ht["QuantityEnd"] as String))
            {
                DataManager.Parameters.Add("@QuantityEnd", ht["QuantityEnd"]);
                where.Append(" inv.Quantity <= @QuantityEnd AND");
            }
            if (!string.IsNullOrEmpty(ht["MinimumStart"] as String))
            {
                DataManager.Parameters.Add("@MinimumStart", ht["MinimumStart"]);
                where.Append(" inv.MinimumRequired >= @MinimumStart AND");
            }
            if (!string.IsNullOrEmpty(ht["MinimumEnd"] as String))
            {
                DataManager.Parameters.Add("@MinimumEnd", ht["MinimumEnd"]);
                where.Append(" inv.MinimumRequired <= @MinimumEnd AND");
            }
            if (!string.IsNullOrEmpty(ht["DepositId"] as String))
            {
                DataManager.Parameters.Add("@DepositId", ht["DepositId"]);
                where.Append(" inv.DepositId = @DepositId AND");
            }
            if (where.ToString() != "")
            {
                sql.Append(where.Remove(where.Length - 3, 3));
            }
            return DataManager.ExecuteDataTable(sql.ToString());
        }

        /// <summary>
        /// this method search the products 
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable SearchProducts(Hashtable ht, string sortExpression, int startRowIndex, int maximumRows)
        {
            var queryProducts = from products in GetAllProducts()
                                where products.CompanyId == Convert.ToInt32(ht["CompanyId"])
                                join categories in DbContext.Categories on products.CategoryId equals
                                    categories.CategoryId
                                join inventories in DbContext.Inventories on products.ProductId equals
                                    inventories.ProductId into gInventories
                                from inventory in gInventories.DefaultIfEmpty()
                                join manufacturers in DbContext.Manufacturers on products.ManufacturerId.Value equals
                                    manufacturers.ManufacturerId into gManufacturers
                                from manufacturer in gManufacturers.DefaultIfEmpty()
                                join deposits in DbContext.Deposits on inventory.DepositId equals deposits.DepositId
                                    into gDeposits
                                from deposit in gDeposits.DefaultIfEmpty()
                                select new
                                           {
                                               products.Name,
                                               CategoryName = categories.Name,
                                               categories.CategoryId,
                                               ManufacturerName = manufacturer != null
                                                                      ? manufacturer.Name
                                                                      : "",
                                               DepositName = deposit != null
                                                                 ? deposit.Name
                                                                 : "",
                                               products.ProductId,
                                               products.ProductCode,
                                               ManufacturerId = manufacturer != null
                                                                    ? manufacturer.ManufacturerId
                                                                    : 0,
                                               products.IsActive,
                                               Quantity = inventory != null
                                                              ? inventory.Quantity
                                                              : 0,
                                               QuantityInReserve = inventory != null
                                                                       ? inventory.QuantityInReserve
                                                                       : 0,
                                               MinimumRequired = inventory != null
                                                                     ? inventory.MinimumRequired
                                                                     : 0,
                                               DepositId = deposit != null
                                                               ? deposit.DepositId
                                                               : 0
                                           };

            if (!string.IsNullOrEmpty(ht["CategoryId"].ToString()))
            {
                queryProducts = queryProducts.Where(p => p.CategoryId == Convert.ToInt32(ht["CategoryId"].ToString()));
            }

            if (!string.IsNullOrEmpty(ht["Name"].ToString()))
            {
                queryProducts = queryProducts.Where(p => p.Name.Contains(ht["Name"].ToString()));
            }
            if (!string.IsNullOrEmpty(ht["ProductCode"].ToString()))
            {
                queryProducts = queryProducts.Where(p => p.ProductCode.Contains(ht["ProductCode"].ToString()));
            }
            if (!string.IsNullOrEmpty(ht["ManufacturerId"].ToString()))
            {
                queryProducts =
                    queryProducts.Where(p => p.ManufacturerId == Convert.ToInt32(ht["ManufacturerId"].ToString()));
            }
            if (!string.IsNullOrEmpty(ht["IsActive"].ToString()))
            {
                queryProducts = queryProducts.Where(p => p.IsActive == Convert.ToBoolean((ht["IsActive"])));
            }
            if (!string.IsNullOrEmpty(ht["QuantityStart"].ToString()))
            {
                queryProducts = queryProducts.Where(p => p.Quantity >= Convert.ToInt32(ht["QuantityStart"]));
            }
            if (!string.IsNullOrEmpty(ht["QuantityEnd"].ToString()))
            {
                queryProducts = queryProducts.Where(p => p.Quantity <= Convert.ToInt32(ht["QuantityEnd"]));
            }
            if (!string.IsNullOrEmpty(ht["MinimumStart"].ToString()))
            {
                queryProducts = queryProducts.Where(p => p.MinimumRequired >= Convert.ToInt32(ht["MinimumStart"]));
            }
            if (!string.IsNullOrEmpty(ht["MinimumEnd"].ToString()))
            {
                queryProducts = queryProducts.Where(p => p.MinimumRequired <= Convert.ToInt32(ht["MinimumEnd"]));
            }
            if (!string.IsNullOrEmpty(ht["DepositId"].ToString()))
            {
                queryProducts = queryProducts.Where(p => p.DepositId == Convert.ToInt32(ht["DepositId"]));
            }

            return queryProducts.SortAndPage(sortExpression, startRowIndex, maximumRows, "ProductId");
        }

        /// <summary>
        /// this method return total rows of SearchProducts
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 SearchProductsCount(Hashtable ht, string sortExpression, int startRowIndex, int maximumRows)
        {
            return SearchProducts(ht, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        //
        // Método utilizado para para preencher o auto-complete de produtos que estão no estoque
        // Retorna o nome completo.
        //
        public IQueryable<Recognizable> SearchProductInInventory(Int32 userId, string name, Int32 maximumRows)
        {
            string methodName = MethodBase.GetCurrentMethod().ToString();
            //if (DataManager.CacheCommands[methodName] == null)
            //{
            DataManager.CacheCommands[methodName] =
         CompiledQuery.Compile<InfoControlDataContext, int, string, int, IQueryable<Recognizable>>(
             (ctx, _userId, _name, _maximumRows) => (from cUsers in DbContext.CompanyUsers
                                                     join dep in DbContext.Deposits on cUsers.DepositId equals dep.DepositId
                                                     join inv in DbContext.Inventories on dep.DepositId equals inv.DepositId
                                                     join product in GetAllProducts() on inv.ProductId equals product.ProductId
                                                     where (cUsers.UserId == userId && cUsers.IsMain &&
                                                            (product.Name.Contains(name) || product.ProductCode.Contains(name)))
                                                     select new Recognizable(product.ProductId.EncryptToHex(), product.Name)).Take(maximumRows));

            var method = (Func<InfoControlDataContext, int, string, int, IQueryable<Recognizable>>)
        DataManager.CacheCommands[methodName];

            return method(DbContext, userId, name, maximumRows);


            //IQueryable<string> query =
            //    from cUsers in DbContext.CompanyUsers
            //    join dep in DbContext.Deposits on cUsers.DepositId equals dep.DepositId
            //    join inv in DbContext.Inventories on dep.DepositId equals inv.DepositId
            //    join product in GetAllProducts() on inv.ProductId equals product.ProductId
            //    where (cUsers.UserId == userId && cUsers.IsMain &&
            //           (product.Name.Contains(name) || product.ProductCode.Contains(name)))
            //    select product.Name;

            //  DataManager.CacheCommands[methodName] = DbContext.GetCommand(query.Take(maximumRows));
            //}

            //DataReader reader = DataManager.ExecuteCachedQuery(methodName, userId, "%" + name + "%", "%" + name + "%");
            //var list = new List<string>();
            //while (reader.Read())
            //{
            //    list.Add(reader.GetString(0));
            //}
            //return list.ToArray();
        }

        //
        // Método utilizado para para preencher o auto-complete de produtos que estão na tabela de Produtos
        // Retorna o nome completo.
        //
        public IQueryable<Recognizable> SearchProduct(Int32 companyId, string name, Int32 maximumRows)
        {
            return SearchProductAsList(companyId, name, maximumRows);
        }

        public IQueryable<Recognizable> SearchProductAsList(Int32 companyId, string name, Int32 maximumRows)
        {
            string methodName = MethodBase.GetCurrentMethod().ToString();

            if (DataManager.CacheCommands[methodName] == null)
            {
                DataManager.CacheCommands[methodName] = CompiledQuery.Compile
                    <InfoControlDataContext, int, string, int, IQueryable<Recognizable>>(
                    (ctx, _companyId, _name, _maximumRows) => (
                                                                  from product in GetAllProducts()
                                                                  join productManufacturer in
                                                                      DbContext.ProductManufacturers on
                                                                      product.ProductId equals
                                                                      productManufacturer.ProductId into gMan
                                                                  from productManufacturer in gMan.DefaultIfEmpty()
                                                                  join productPackages in DbContext.ProductPackages on
                                                                      product.ProductId equals productPackages.ProductId
                                                                      into gPkg
                                                                  from productPackages in gPkg.DefaultIfEmpty()
                                                                  where (product.CompanyId == _companyId &&
                                                                         (product.Name.Contains(_name) ||
                                                                          product.ProductCode.Contains(_name)))
                                                                  orderby product.Name, productPackages.Name
                                                                  select new Recognizable(product.ProductId.EncryptToHex(),
                                                                         (product.ProductCode ?? "") + " | " +
                                                                         product.Name + " | " +
                                                                         (productPackages.Name ?? "") + " | " +
                                                                         (productManufacturer.Name ?? "")))
                                                                  .Take(_maximumRows));
            }

            var method =
                (Func<InfoControlDataContext, int, string, int, IQueryable<Recognizable>>)
                DataManager.CacheCommands[methodName];

            return method(DbContext, companyId, name, maximumRows);
        }

        //
        // Pega todos os produtos daquela company, e exclui ele próprio
        //
        public IQueryable<Product> GetProductByCompanySelfExcludent(Int32 productId, Int32 companyId)
        {
            return GetAllProducts().Where(x => x.CompanyId == companyId && x.ProductId != productId);
        }

        /// <summary>
        /// This method retrieves a single Product.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=ProductId>ProductId</param>
        /// <param name=CompanyId>CompanyId</param>
        public Product GetProduct(Int32 productId, Int32 companyId)
        {
            return GetProducts(companyId, null, false, null, null, null, false, null, null, productId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieve a product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Product GetProduct(Int32 productId)
        {
            return DbContext.Products.Where(product => product.ProductId == productId).FirstOrDefault();
        }

        //
        // Seleciona o produto, por categoria ou todos, seta a paginação e a ordenação e devolve os dados 
        // para um DataTable, ideal para gridView com Ordenação por categoria. Tem de ser utilizado com
        // o método COUNT abaixo
        //
        public IList GetProductsByCategory(int? categoryId, int companyId, string sortExpression, int startRowIndex,
                                           int maximumRows)
        {
            var query = from product in GetAllProducts()
                        join category in DbContext.GetChildCategories(companyId, categoryId) on product.CategoryId
                            equals category.CategoryId
                        join manufacturer in DbContext.Manufacturers on product.ManufacturerId equals
                            manufacturer.ManufacturerId into gManufactory
                        from manufacturer in gManufactory.DefaultIfEmpty()
                        where product.CompanyId == companyId
                        select new
                                   {
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
                                       ManufacturerName = gManufactory.FirstOrDefault().Name,
                                       CategoryName = category.Name
                                   };

            if (categoryId != null)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "productId").ToList();
        }

        /// <summary>
        /// This method returns products with parameters in filter of product
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="companyId"></param>
        /// <param name="categoriesRecursive"></param>
        /// <param name="manufacturerId"></param>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <param name="depositId"></param>
        /// <param name="isTemp"></param>
        /// <param name="initialLetter"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IQueryable<Product> GetProducts(
            Int32 companyId,
            Int32? categoryId,
            bool categoriesRecursive,
            Int32? manufacturerId,
            String description,
            String name,
            bool? isTemp,
            String initialLetter,
            int? depositId,
            int? productId)
        {


            IQueryable<Inventory> inventories = DbContext.Inventories.AsQueryable();
            if (depositId.HasValue && depositId != 0)
                inventories = inventories.Where(inv => inv.DepositId == depositId);

            var query = from product in GetProductsByCompany(companyId)
                        join inventory in inventories on product.ProductId equals inventory.ProductId into gInv
                        from inventory in gInv.DefaultIfEmpty()
                        select new { product, inventory };

            if (manufacturerId.HasValue && manufacturerId != 0)
                query = query.Where(p => p.product.ManufacturerId == manufacturerId);

            if (productId.HasValue && productId != 0)
                query = query.Where(p => p.product.ProductId == productId);

            if (categoryId.HasValue && categoryId != 0)
            {
                var categoryManager = new CategoryManager(this);
                var categories = categoryManager.GetChildCategories(companyId, categoryId, true);
                query = query.Where(p => p.product.CategoryId == categoryId || categories.Any(c => c.CategoryId == p.product.CategoryId));
            }

            if (!String.IsNullOrEmpty(name))
                query =
                    query.Where(
                        p =>
                        p.product.Name.Contains(name) || p.product.Description.Contains(name) ||
                        p.product.Manufacturer.Name.Contains(name) || p.product.Category.Name.Contains(name));

            if (!String.IsNullOrEmpty(initialLetter))
                query = query.Where(p => p.product.Name.StartsWith(initialLetter));

            if (!String.IsNullOrEmpty(description))
                query = query.Where(p => p.product.Description.Contains(description));

            query = isTemp.HasValue && isTemp.Value
                        ? query.Where(p => p.product.IsTemp == true)
                        : query.Where(p => p.product.IsTemp == false || !p.product.IsTemp.HasValue);

            //if (categoryId.HasValue)
            //    query = query.Where(p => p.product.CategoryId == categoryId);

            var query2 = from q in query
                         select q.product;

            return query2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="categoryId"></param>
        /// <param name="categoriesRecursive"></param>
        /// <param name="manufacturerId"></param>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <param name="isTemp"></param>
        /// <param name="initialLetter"></param>
        /// <param name="depositId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<Product> GetProducts(
            Int32 companyId,
            Int32? categoryId,
            bool categoriesRecursive,
            Int32? manufacturerId,
            String description,
            String name,
            bool? isTemp,
            String initialLetter,
            int? depositId,
            int? productId,
            string sortExpression,
            int startRowIndex,
            int maximumRows)
        {
            return
                GetProducts(companyId, categoryId, categoriesRecursive, manufacturerId, description, name, isTemp,
                            initialLetter, depositId, productId)
                            .ToList().AsQueryable().SortAndPage(sortExpression, startRowIndex, maximumRows,
                                                                             "Name");
        }

        public int GetProductsCount(
            Int32 companyId,
            Int32? categoryId,
            bool categoriesRecursive,
            Int32? manufacturerId,
            String description,
            String name,
            bool? isTemp,
            String initialLetter,
            int? depositId,
            int? productId,
            string sortExpression,
            int startRowIndex,
            int maximumRows)
        {
            return
                GetProducts(companyId, categoryId, categoriesRecursive, manufacturerId, description, name, isTemp,
                            initialLetter, depositId, productId).Count();
        }

        /// <summary>
        /// This method returns the quantity that GetProducts(in respective overload) method returns
        /// </summary>
        /// <param name="companyId">cannot be null</param>
        /// <param name="depositId">cannot be null</param>
        /// <param name="categoryId"></param>
        /// <param name="categoriesRecursive"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetProductsCount(int companyId, int depositId, int? categoryId, bool categoriesRecursive,
                                      string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetProducts(companyId, categoryId, categoriesRecursive, null, null, null, false, null, depositId, null).
                    Cast<object>().Count();
        }


        public IQueryable GetProducts(Int32 companyId, Int32? categoryId, Int32 manufacturerId,
                                      String description, String name, Boolean? isTemp, string sortExpression,
                                      int startRowIndex,
                                      int maximumRows)
        {
            return GetProducts(companyId, categoryId, false, manufacturerId, description, name, isTemp, null, null, null,
                               sortExpression, startRowIndex, maximumRows);
        }

        public IQueryable GetProducts(Int32 companyId, Int32? categoryId, Int32 manufacturerId,
                                      String description, String name, Boolean? isTemp, String initialLetter,
                                      string sortExpression, int startRowIndex,
                                      int maximumRows)
        {
            return GetProducts(companyId, categoryId, false, manufacturerId, description, name, isTemp, initialLetter,
                               null, null, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// Count method for paging when selecting elements by it's initial Letter 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="companyId"></param>
        /// <param name="manufacturerId"></param>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <param name="isTemp"></param>
        /// <param name="initialLetter"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetProductsCount(Int32 companyId, Int32? categoryId, Int32 manufacturerId, String description,
                                      String name, Boolean? isTemp, String initialLetter, string sortExpression,
                                      int startRowIndex, int maximumRows)
        {
            return
                GetProducts(companyId, categoryId, false, manufacturerId, description, name, isTemp, initialLetter, null,
                            null).Cast<object>().Count();
        }


        public Int32 GetProductsCount(Int32? categoryId, Int32 companyId, Int32 manufacturerId, String description,
                                      String name, Boolean? isTemp, string sortExpression, int startRowIndex,
                                      int maximumRows)
        {
            return
                GetProducts(companyId, categoryId, false, manufacturerId, description, name, isTemp, null, null, null,
                            sortExpression, startRowIndex, maximumRows).Cast<object>().Count();
        }

        //
        // Metodo auxiliar para funcionamento da paginação manual da GridView, 
        // funciona em conjunto com o Método GetProductsByCategory
        //
        public int GetProductsCount(int? categoryId, int companyId, string sortExpression, int startRowIndex,
                                    int maximumRows)
        {
            if (categoryId != null)
            {
                IQueryable<Product> y = GetProductsByCategory(Convert.ToInt16(categoryId));
                return y.Count();
            }
            else
            {
                IQueryable<Product> x = GetProductsByCompany(companyId);
                return x.Count();
            }
        }

        //
        // Pega todos os produtos de uma company, e seta a ordenação e paginação manual ...
        // Também deve ser utilizado com o método COUNT acima
        //
        public DataTable GetProducts(int companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            var sql = new StringBuilder();
            DataManager.Parameters.Add("@companyId", companyId);

            sql.AppendFormat(
                @"
                SELECT * FROM (
                    SELECT 
                        TOP {0}
                        Products.*, 
                        ISNULL(Manufacturer.Name,'Sem Fabricante') as ManufacturerName, 
                        Categories.Name as CategoryName,
                        ROW_NUMBER() OVER (ORDER BY {2}) as IDX
                    FROM   
                        Products  LEFT OUTER JOIN                                  
                        Manufacturer  ON Manufacturer.ManufacturerId = Products.ManufacturerId INNER JOIN 
                        Categories  ON Products.CategoryId = Categories.CategoryId 
                    WHERE  
                        Products.CompanyId = @companyId                         
                    ORDER BY {2}
                ) as test
                WHERE IDX >= {1} ",
                maximumRows < 1
                    ? 10
                    : maximumRows, startRowIndex, sortExpression ?? "Products.Name");

            return DataManager.ExecuteDataTable(sql.ToString());
        }

        //
        // Pega o código de barras de um produto que se encontra no estoque
        //
        public DataReader GetProductCodeBarInIneventory(int productId, int depositId)
        {
            DataManager.Parameters.Add("@productId", productId);
            DataManager.Parameters.Add("@depositId", depositId);

            string sql =
                @"
                    SELECT 
                        Products.Name, 
                        Products.ProductCode, 
                        Products.BarCode,
                        Inventory.Quantity,
                        Products.BarCodeTypeId
                    FROM   
                        Products  INNER JOIN                                  
                        Inventory  ON Inventory.ProductId = Products.ProductId                        
                    WHERE  
                        Products.ProductId = @productId AND
                        Inventory.DepositId = @depositId";
            return DataManager.ExecuteReader(sql);
        }

        /// <summary>
        /// This method retrieves Product by Category.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CategoryId>CategoryId</param>
        public DataTable GetProductsByBudget(Int32 budgetId)
        {
            var query = from bi in DbContext.BudgetItems
                        join prod in GetAllProducts() on bi.ProductId equals prod.ProductId into gProduct
                        from prod in gProduct.DefaultIfEmpty()
                        where bi.BudgetId == budgetId
                        select new
                                   {
                                       prod,
                                       bi
                                   };
            return query.ToDataTable();
        }

        public IQueryable<Product> GetProductsInBudget(Int32 companyId, Int32 budgetId)
        {
            IQueryable<Product> products = from product in GetProductsByCompany(companyId)
                                           join budgetItem in
                                               DbContext.BudgetItems.Where(bItem => bItem.BudgetId == budgetId) on
                                               product.ProductId equals budgetItem.ProductId
                                           select product;
            return products;
        }

        /// <summary>
        /// Return a single product
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Product GetProductByExternalSource(int companyId, string id)
        {
            return GetProductsByCompany(companyId).Where(x => x.ExternalSourceProductId.Contains(id)).FirstOrDefault();
        }

        /// <summary>
        /// Return a single product
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Product GetProductByName(int companyId, string name)
        {
            return GetAllProducts().Where(x => x.CompanyId == companyId && x.Name.Contains(name)).FirstOrDefault();
        }

        /// <summary>
        /// Return a single product
        /// </summary>
        /// <param name="code"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Product GetProductByCode(string code, int companyId)
        {
            return GetAllProducts().Where(x => x.CompanyId == companyId && x.ProductCode == code).FirstOrDefault();
        }

        /// <summary>
        /// Return All Categories of a company, organized by the parentId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataSet GetCategoriesTree(int companyId)
        {
            DataManager.Parameters.Add("@CompanyId", companyId);

            string sql =
                @" WITH ROOT AS (SELECT CategoryId, 
                                                 Name, 
                                                 ParentId,
                                                 CompanyId                                                  
                            FROM Categories)
            SELECT r.CategoryId, r.Name, r.ParentId, p.
            FROM ROOT AS r LEFT OUTER JOIN
            Categories AS c ON r.ParentId = c.CategoryId
            WHERE (r.CompanyId = @CompanyId)";

            return DataManager.ExecuteDataSet(sql);
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public int InsertRetrievingProductsId(Product entity)
        {
            if (CheckIfProductCodeExists(entity.CompanyId, entity.ProductId, entity.ProductCode))
            {
                return 0;
            }
            else
            {
                DbContext.Products.InsertOnSubmit(entity);
                DbContext.SubmitChanges();
                return entity.ProductId;
            }
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public int Update(Product original_entity, Product entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            entity.ModifiedDate = DateTime.Now;
            DbContext.SubmitChanges();
            return entity.ProductId;
        }

        /// <summary>
        /// this method return true if the productCode Exist in DataBase and ProductId isn't
        /// the same of parameter
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="productId"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public bool CheckIfProductCodeExists(int companyId, int productId, string productCode)
        {
            IQueryable<Product> query = from products in GetAllProducts()
                                        where products.CompanyId == companyId && products.ProductCode == productCode
                                              && products.ProductId != productId
                                        select products;

            if (query.Count() != 0)
                return true;
            else return false;
        }

        /// <summary>
        /// this method return true if ProductCode exist in DataBase
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public bool CheckIfProductCodeExists(int companyId, string productCode)
        {
            IQueryable<Product> query = from product in GetAllProducts()
                                        where product.CompanyId == companyId && product.ProductCode == productCode
                                        select product;
            if (query.Count() != 0)
                return true;

            return false;
        }

        public DataTable RetrievingShortageProducts(int companyId)
        {
            var queryProducts = from products in GetAllProducts()
                                join inventory in DbContext.Inventories on products.ProductId equals inventory.ProductId
                                where products.CompanyId == companyId && inventory.Quantity <= inventory.MinimumRequired
                                select new
                                           {
                                               products.Name,
                                               products.ProductCode,
                                               products.Description,
                                               inventory.Quantity
                                           };
            return queryProducts.ToDataTable();
        }

        /// <summary>
        /// this method return the products where Quantity lower or equals MinimumRequired
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<Product> RetrievingShortageProducts(Int32 companyId, string sortExpression, int startRowIndex,
                                                              int maximumRows)
        {
            IQueryable<Product> query = from product in GetAllProducts()
                                        join inventory in DbContext.Inventories on product.ProductId equals
                                            inventory.ProductId
                                        where
                                            inventory.Quantity <= inventory.MinimumRequired &&
                                            product.CompanyId == companyId
                                        select product;
            return query.AsQueryable().SortAndPage(sortExpression, startRowIndex, maximumRows, "ProductId");
        }

        /// <summary>
        /// this method make the ABC Curve from product and deposit
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="depositId"></param>
        /// <returns></returns>
        [Obsolete("This method is obsolete, use the GetProductsRankByDeposit method")]
        public IList GetProductsRankByDeposit(Int32 companyId, Int32? depositId, string sortExpression,
                                              DateTime startDate, DateTime endDate)
        {
            var dateTimeInterval = new DateTimeInterval(startDate, endDate);

            return GetProductsRankByDeposit(companyId, depositId, sortExpression, dateTimeInterval);
        }

        /// <summary>
        /// this method make the ABC Curve from product and deposi
        /// </summary>
        /// <param name="companyId">can't be null</param>
        /// <param name="depositId">can be null</param>
        /// <param name="sortExpression">can't be null</param>
        /// <param name="dateTimeInterval">can't be null</param>
        /// <returns>an IList of products</returns>
        public IList GetProductsRankByDeposit(Int32 companyId, Int32? depositId, string sortExpression,
                                              DateTimeInterval dateTimeInterval)
        {
            var pQuery = from products in GetAllProducts()
                         join saleItem in DbContext.SaleItems on products.ProductId equals saleItem.ProductId
                         join sale in DbContext.Sales on saleItem.SaleId equals sale.SaleId
                         where
                             products.CompanyId == companyId &&
                             (sale.SaleDate >= dateTimeInterval.BeginDate.Date &&
                              sale.SaleDate <= dateTimeInterval.EndDate.Date) && saleItem.UnitPrice > 0 &&
                             saleItem.UnitCost > 0
                         select new
                                    {
                                        productName = products.Name,
                                        quantity = saleItem.Quantity,
                                        unitPrice = saleItem.UnitPrice,
                                        depositId = sale.DepositId,
                                        productId = saleItem.ProductId,
                                        subTotal = saleItem.Quantity * saleItem.UnitPrice,
                                        subTotalCost = saleItem.Quantity * saleItem.UnitCost,
                                    };
            if (depositId != null)
                pQuery = pQuery.Where(x => x.depositId == depositId);

            Decimal? sumTotalQuery = pQuery.Sum(x => x.subTotal);
            var sumQuery = from query in pQuery
                           group query by new
                                              {
                                                  query.productId,
                                                  query.productName,
                                                  query.depositId
                                              }
                               into gQuery
                               select new
                                          {
                                              gQuery.Key.productName,
                                              quantity = gQuery.Sum(x => x.quantity),
                                              unitPrice = gQuery.Average(x => x.unitPrice),
                                              gQuery.Key.depositId,
                                              gQuery.Key.productId,
                                              sumSubtotal = gQuery.Sum(x => x.subTotal),
                                              sumSubTotalCost = gQuery.Sum(x => x.subTotalCost),
                                              sumTotal = sumTotalQuery,
                                              Percentage = (gQuery.Sum(x => x.subTotal) / sumTotalQuery) * 100,
                                              profit = (gQuery.Sum(x => x.subTotal) - gQuery.Sum(x => x.subTotalCost)),
                                              profitMargin =
                               ((gQuery.Sum(x => x.subTotal) - gQuery.Sum(x => x.subTotalCost)) /
                                gQuery.Sum(x => x.subTotalCost)) * 100
                                          };

            return sumQuery.OrderByDescending(x => x.sumSubtotal).Sort(sortExpression).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductBySale(int companyId)
        {
            var query = from sales in DbContext.Sales
                        where sales.CompanyId == companyId
                        join saleItems in DbContext.SaleItems on sales.SaleId equals saleItems.SaleId
                        group saleItems by saleItems.CompanyId
                            into gSaleItems
                            select new
                                       {
                                           media =
                            ((gSaleItems.Sum(x => x.UnitPrice * x.Quantity) - gSaleItems.Sum(x => x.Sale.Discount)) /
                             gSaleItems.Sum(x => x.Quantity))
                                       };
            return query.ToDataTable();
        }

        #endregion

        #region ProductImage

        /// <summary>
        /// Insert product images
        /// </summary>
        /// <param name="entity"></param>
        public void InsertProductImage(ProductImage entity)
        {
            DbContext.ProductImages.InsertOnSubmit(entity); //insert the productImage
            DbContext.SubmitChanges();
        }


        /// <summary>
        /// this method insert the productImage
        /// </summary>
        /// <param name="productImage"></param>
        public void InsertProductImage(Company company, ProductImage entity, HttpPostedFile file)
        {
            string virtualPath = company.GetFilesDirectory();
            string fileName = Path.GetFileName(file.FileName);
            entity.ImageUrl = virtualPath + fileName; // set the ImageUrl
            file.SaveAs(HttpContext.Current.Server.MapPath(entity.ImageUrl)); //save the file

            InsertProductImage(entity);
        }

        /// <summary>
        /// This method deletes a product image from data base and the file from the directory
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="productImageId"></param>
        public void DeleteProductImage(Int32 companyId, Int32 productImageId)
        {
            String virtualPath; //contains the physical url
            ProductImage image =
                DbContext.ProductImages.Where(im => im.ProductImageId == productImageId).FirstOrDefault();

            try
            {
                //set the logical url
                virtualPath = HttpContext.Current.Server.MapPath(image.ImageUrl);
                File.Delete(virtualPath);
                DbContext.ProductImages.DeleteOnSubmit(image);
                DbContext.SubmitChanges();
            }
            catch (Exception)
            {
                DataManager.Rollback();
            }
        }

        public IQueryable<ProductImage> GetProductImages(Int32 productId)
        {
            return DbContext.ProductImages.Where(pi => pi.ProductId == productId);
        }

        /// <summary>
        /// this method return one Iqueryable of producImages
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IQueryable<ProductImage> GetProductImages(Int32 productId, string sortExpression, int startRowIndex,
                                                         int maximumRows)
        {
            return GetProductImages(productId).SortAndPage(sortExpression, startRowIndex, maximumRows, "ProductId");
        }

        /// <summary>
        /// this method return the number of rows in GetProductImages
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetProductImagesCount(Int32 productId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetProductImages(productId, sortExpression, startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// This method returns the products with yours respectives images
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public IQueryable GetProductsWithImages(Int32 categoryId)
        {
            var query = from product in GetAllProducts()
                        join productImage in DbContext.ProductImages on product.ProductId equals productImage.ProductId
                        select new
                                   {
                                       product.Name,
                                       productImage.ImageUrl
                                   };

            return query;
        }

        ///// <summary>
        ///// This method returns the products with yours respectives images
        ///// </summary>
        ///// <param name="CategoryId"></param>
        ///// <returns></returns>
        //public IQueryable GetProductsWithImages(Int32 categoryId)
        //{
        //    return GetProductsByCategory(categoryId).Join(DbContext.ProductImages, product => product.ProductId,
        //        productImage => productImage.ProductId, (product, productImage) => new { product.Name, productImage.ImageUrl });
        //}

        #endregion

        #region ProductCertificate

        /// <summary>
        /// This method insert a new certificate to the product which the ProductId makes reference
        /// </summary>
        /// <param name="productCertificate"></param>
        /// <param name="ProductId"></param>
        public void InsertProductCertificate(int productId, ProductCertificate productCertificate)
        {
            //
            // binds a certificate to a product
            //
            productCertificate.ProductId = productId;
            DbContext.ProductCertificates.InsertOnSubmit(productCertificate);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Deletes a Certificate of a product
        /// </summary>
        /// <param name="productCertificate"></param>
        public void DeleteProductCertificate(int productCertificateId)
        {
            DbContext.ProductCertificates.DeleteAllOnSubmit(
                DbContext.ProductCertificates.Where(pc => pc.ProductCertificateId == productCertificateId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Returns one Certification by it's ID
        /// </summary>
        /// <param name="productCertificateId"></param>
        /// <returns></returns>
        public ProductCertificate GetProductCertificate(int productCertificateId)
        {
            return
                DbContext.ProductCertificates.Where(x => x.ProductCertificateId == productCertificateId).FirstOrDefault();
        }

        /// <summary>
        /// Returns all certifications of one product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<ProductCertificate> GetProductCertificates(int productId, string sortExpression,
                                                                     int startRowIndex, int maximumRows)
        {
            return DbContext.ProductCertificates.Where(x => x.ProductId == productId);
        }

        #endregion

        #region ProductPackage

        public ProductPackage GetProductPackage(int productPackageId)
        {
            return DbContext.ProductPackages.Where(x => x.ProductPackageId == productPackageId).FirstOrDefault();
        }

        /// <summary>
        /// resturns all ProductPackage from one specific product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<ProductPackage> GetProductPackages(int productId, string sortExpression, int startRowIndex,
                                                             int maximumRows)
        {
            return DbContext.ProductPackages.Where(pp => pp.ProductId == productId);
        }

        /// <summary>
        /// This method insert a new Package to the product which the ProductId makes reference
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productPackage"></param>
        /// <returns></returns>
        public void InsertProductPackage(int productId, ProductPackage productPackage)
        {
            //
            // Binds a Package to a product
            //
            productPackage.ProductId = productId;
            DbContext.ProductPackages.InsertOnSubmit(productPackage);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Deletes a package from the product package list
        /// </summary>
        /// <param name="productPackageId"></param>
        public void DeleteProductPackage(int productPackageId)
        {
            DbContext.ProductPackages.DeleteOnSubmit(
                DbContext.ProductPackages.Where(pp => pp.ProductPackageId == productPackageId).First());
            DbContext.SubmitChanges();
        }

        #endregion

        #region ProductManufacturer

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productManufacturerId"></param>
        /// <returns></returns>
        public ProductManufacturer GetProductManufacturer(int productManufacturerId)
        {
            return
                DbContext.ProductManufacturers.Where(x => x.ProductManufacturerId == productManufacturerId).
                    FirstOrDefault();
        }

        /// <summary>
        /// Returns all manufacturers of the product being updated
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IQueryable<ProductManufacturer> GetProductManufacturers(int productId, string sortExpression,
                                                                       int startRowIndex, int maximumRows)
        {
            return DbContext.ProductManufacturers.Where(m => m.ProductId == productId);
        }

        /// <summary>
        /// Insert a new manufacturer to the product which productId makes reference
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productManufacturer"></param>
        public void InsertProductManufacturer(int productId, ProductManufacturer productManufacturer)
        {
            //
            // binds a manufacturer to a product
            //
            productManufacturer.ProductId = productId;
            DbContext.ProductManufacturers.InsertOnSubmit(productManufacturer);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Deletes a manufacturer from the product manufacturer list
        /// </summary>
        /// <param name="productManufacturerId"></param>
        public void DeleteProductManufacturer(int productManufacturerId)
        {
            DbContext.ProductManufacturers.DeleteOnSubmit(
                DbContext.ProductManufacturers.Where(m => m.ProductManufacturerId == productManufacturerId).First());
            DbContext.SubmitChanges();
        }

        #endregion
    }
}