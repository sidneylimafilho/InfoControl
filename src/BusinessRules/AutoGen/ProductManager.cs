using System;
using System.Linq;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class ProductManager : BusinessManager<InfoControlDataContext>
    {
        public ProductManager(IDataAccessor container)
            : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Products.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Product> GetAllProducts()
        {
            return DbContext.Products.Where(products => products.IsTemp == false || !products.IsTemp.HasValue);
        }

        /// <summary>
        /// This method gets all temporary products
        /// </summary>
        /// <returns></returns>
        public IQueryable<Product> GetAllTempProducts()
        {
            return DbContext.Products.Where(products => products.IsTemp == true);
        }

        /// <summary>
        /// This method gets record counts of all Products.
        /// Do not change this method.
        /// </summary>
        public int GetAllProductsCount()
        {
            return GetAllProducts().Count();
        }


        /// <summary>
        /// This method gets a single temporary product
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public Product GetTempProduct(Int32 productId, Int32 companyId)
        {
            return
                GetAllTempProducts().Where(tp => tp.ProductId == productId && tp.CompanyId == companyId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves Product by Company.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<Product> GetProductsByCompany(Int32 CompanyId)
        {
            return GetAllProducts().Where(x => x.CompanyId == CompanyId);
        }


        /// <summary>
        /// This method retrieves Product by Category.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CategoryId>CategoryId</param>
        public IQueryable<Product> GetProductsByCategory(Int32 CategoryId)
        {
            return GetAllProducts().Where(x => x.CategoryId == CategoryId);
        }


        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<Product> GetFilteredProducts(string tableName, Int32 Company_CompanyId,
                                                        Int32 Composite_CompositeId, Int32 Category_CategoryId,
                                                        Int32 ProductStatu_ProductStatusId)
        {
            switch (tableName)
            {
                case "Company_Products":
                    return GetProductsByCompany(Company_CompanyId);
                case "Category_Products":
                    return GetProductsByCategory(Category_CategoryId);
                default:
                    return GetAllProducts();
            }
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Product entity)
        {
            //GetAllProducts().Attach(entity);
            DbContext.Products.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(Product entity)
        {
            entity.RequiresAuthorization = entity.RequiresAuthorization ?? false;

            entity.IsTemp = entity.IsTemp ?? false;

            entity.CreatedDate = entity.ModifiedDate = DateTime.Now;
            DbContext.Log = Console.Out;
            DbContext.Products.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }
    }
}