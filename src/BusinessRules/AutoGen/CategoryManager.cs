using System;
using System.Collections;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class CategoryManager : BusinessManager<InfoControlDataContext>
    {
        public CategoryManager(IDataAccessor container)
            : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Categories.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Category> GetAllCategories()
        {
            return DbContext.Categories.OrderBy(cat => cat.Name);
        }

        /// <summary>
        /// This method gets record counts of all Categories.
        /// Do not change this method.
        /// </summary>
        public int GetAllCategoriesCount()
        {
            return GetAllCategories().Count();
        }

        /// <summary>
        /// This method retrieves a single Category.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=categoryId>categoryId</param>
        public Category GetCategory(int? categoryId)
        {
            return DbContext.Categories.Where(x => x.CategoryId == categoryId).FirstOrDefault();
        }


       

        /// <summary>
        /// This method retrieves a single Category.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=CategoryId>CategoryId</param>
        public Category GetCategoryByExternalSource(int companyId, string categoryId)
        {
            return DbContext.Categories.FirstOrDefault(x => x.CompanyId == companyId && x.ExternalSourceId == categoryId);
        }

        /// <summary>
        /// This method pages and sorts over all Categories.
        /// Do not change this method.
        /// </summary>
        /// <param name=sortExpression>sortExpression</param>
        /// <param name=startRowIndex>startRowIndex</param>
        /// <param name=maximumRows>maximumRows</param>
        public IList GetAllCategories(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetAllCategories().SortAndPage(sortExpression, startRowIndex, maximumRows, "CategoryId").ToList();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Category entity)
        {
            // 
            //  DbContext.Categories.Attach(entity);
            DbContext.Categories.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(Category entity)
        {
            entity.ModifiedDate = DateTime.Now;
            DbContext.Categories.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(Category original_entity, Category entity)
        {
            //
            //DbContext.Categories.Attach(original_entity);
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }
    }
}