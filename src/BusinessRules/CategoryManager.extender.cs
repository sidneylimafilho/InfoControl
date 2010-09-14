using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using InfoControl.Data;
using InfoControl;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class CategoryManager
    {
        /// <summary>
        /// Return all categories of a company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
#warning Esse método não está nos padrões de nomenclatura -> GetCategoriesAsList
        public List<Category> GetCategoriesByCompanyAsList(int companyId, string sortExpression, int startRowIndex,
                                                           int maximumRows)
        {
            IQueryable<Category> x = GetCategoriesByCompany(companyId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "CategoryId").ToList();
        }

        /// <summary>
        /// Return all categories of a company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<Category> GetCategoriesByCompany(Int32 companyId)
        {
            return GetChildCategories(companyId, null, true);
        }


        public IQueryable<Category> GetChildCategories(Int32 companyId, int? parentId, bool recursive)
        {
            if (recursive)
                return DbContext.GetChildCategories(companyId, parentId);

            IQueryable<Category> query = GetAllCategories().Where(c => c.CompanyId == companyId);
            query = parentId.HasValue
                        ? query.Where(cat => cat.ParentId == parentId)
                        : query.Where(cat => !cat.ParentId.HasValue);

            return query.OrderBy(x => x.Name);
        }

        public DataTable GetCategoriesByCompanyAsDataTable(Int32 companyId)
        {
            return GetCategoriesByCompany(companyId).ToDataTable();
        }



        /// <summary>
        /// This method retrieves a single Category.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=categoryId>categoryId</param>
        public Category GetCategory(int companyId, string categoryPath)
        {
            Category lastCategory = null;
            string[] categoryNames = categoryPath.Split('/');
            var categories = DbContext.Categories.Where(cat => cat.CompanyId == companyId && !cat.ParentId.HasValue);

            foreach (var categoryName in categoryNames)
                foreach (var category in categories)
                    if (category.Name.RemoveSpecialChars() == categoryName)
                    {
                        lastCategory = category;
                        categories = category.Categories.AsQueryable();
                        break;
                    }

            return lastCategory;
        }

    }
}