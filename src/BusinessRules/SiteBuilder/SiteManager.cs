using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules.WebSites
{
    public class SiteManager : BusinessManager<InfoControlDataContext>
    {
        public SiteManager(IDataAccessor container)
            : base(container)
        {
        }

        #region Site

        public void Save(WebPage page, string tags)
        {
            WebPage originalPage = GetWebPage(page.CompanyId, page.WebPageId).Detach();
            page.ModifiedDate = originalPage.ModifiedDate = DateTime.Now;

            //
            // Save
            //
            if (originalPage.WebPageId == -404)
                DbContext.WebPages.InsertOnSubmit(page);
            else
                originalPage.CopyPropertiesFrom(page);

            DbContext.SubmitChanges();

            string[] list = null;

            //
            // Tags
            //
            if (!String.IsNullOrEmpty(tags))
            {
                list = tags.Split(',');
                DbContext.PageTags.DeleteAllOnSubmit(DbContext.PageTags.Where(p => p.WebPageId == page.WebPageId));
                DbContext.SubmitChanges();

                foreach (string item in list)
                    if (!String.IsNullOrEmpty(item))
                        DbContext.PageTags.InsertOnSubmit(new PageTag
                                                              {
                                                                  CompanyId = page.CompanyId,
                                                                  WebPageId = page.WebPageId,
                                                                  Name = item.Trim()
                                                              });
                DbContext.SubmitChanges();
            }


            //
            // Categories
            //
            //if (!String.IsNullOrEmpty(categories))
            //{
            //    list = categories.Split(',');
            //    DbContext.PageCategories.DeleteAllOnSubmit(
            //        DbContext.PageCategories.Where(p => p.WebPageId == page.WebPageId));
            //    DbContext.SubmitChanges();

            //    foreach (string item in list)
            //        if (!String.IsNullOrEmpty(item))
            //            DbContext.PageCategories.InsertOnSubmit(new PageCategory
            //                                                        {
            //                                                            CompanyId = page.CompanyId,
            //                                                            WebPageId = page.WebPageId,
            //                                                            Name = item.Trim()
            //                                                        });
            //    DbContext.SubmitChanges();
            //}

            //
            // Rollup tree with configs
            //
            RollupConfig(originalPage);
        }

        private void RollupConfig(WebPage page)
        {
            WebPage parentPage;

            if ((parentPage = page).IsInMenu)
                while ((parentPage = parentPage.WebPage1) != null)
                    parentPage.IsInMenu = true;

            if ((parentPage = page).IsPublished)
                while ((parentPage = parentPage.WebPage1) != null)
                    parentPage.IsPublished = true;

            DbContext.SubmitChanges();
        }

        public void Delete(WebPage entity)
        {
            foreach (WebPage page in entity.WebPages)
                Delete(page);

            DbContext.WebPages.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        public IQueryable<WebPage> GetChildPages(int? companyId, int? parentId, bool recursive)
        {
            if (recursive)
                return DbContext.GetChildPages(companyId, parentId);

            IQueryable<WebPage> query = DbContext.WebPages.Where(page => page.CompanyId == companyId);
            query = parentId.HasValue
                        ? query.Where(page => page.ParentPageId == parentId)
                        : query.Where(page => !page.ParentPageId.HasValue);

            return query.OrderBy(x => x.ParentPageId).OrderByDescending(x => x.PublishedDate);
        }

        /// <summary>
        /// This method returns all webPages in db
        /// </summary>
        /// <returns></returns>
        public IQueryable<WebPage> GetAllWebPages()
        {
            return DbContext.WebPages;
        }

        public IQueryable<WebPage> GetWebPages(int companyId)
        {
            return GetChildPages(companyId, null, false);
        }

        public WebPage GetWebPage(Int32 companyId, Int32 pageId)
        {
            return DbContext.WebPages.Where(page => page.CompanyId == companyId &&
                                                    page.WebPageId.Equals(pageId)).FirstOrDefault()
                   ?? WebPageNotFound();
        }

        public WebPage GetMainPage(Int32 companyId)
        {
            return GetWebPages(companyId).Where(page => page.IsMainPage.Value).FirstOrDefault()
                   ?? WebPageNotFound();
        }

        public DataTable GetChildPagesAsTable(int? companyId, int? parentId, bool recursive)
        {
            return GetChildPages(companyId, parentId, recursive).ToDataTable();
        }

        public DataTable GetMenu(Int32 companyId)
        {
            return GetWebPages(companyId).Where(pg => pg.IsInMenu).ToDataTable();
        }

        public WebPage WebPageNotFound()
        {
            return new WebPage
                       {
                           WebPageId = -404,
                           Name = @"Página não encontrada!",
                           Description = @"Página não encontrada!",
                           PublishedDate = DateTime.Now,
                           CanComment = false,
                           IsInMenu = false,
                           IsPublished = true,
                           IsMainPage = false
                       };
        }

        public IEnumerable<string> GetMasterPages(Company company)
        {
            return Directory.GetFiles(HttpContext.Current.Server.MapPath(company.GetHomePath()))
                .Where(f => f.ToLower().EndsWith(".master"))
                .Select(f => Path.GetFileNameWithoutExtension(f));
        }

        #endregion

        #region Domain in Google

        private void CreateDomain(string domain)
        {
            //http://www.google.com/a/cpanel/standard/selectDomain?existingDomain=observatiorioimsuerj.com.br&newDomain=false&ownDomain=true
        }

        #endregion

        #region Tags
        /// <summary>
        /// Retrieve a set of tags and count for hierarchy
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="webpageId"></param>
        /// <returns></returns>
        public IQueryable GetTagsByPage(int companyid, int webpageId)
        {
            var query = from page in GetChildPages(companyid, webpageId, true)
                        join tags in DbContext.PageTags on page.WebPageId equals tags.WebPageId
                        where tags.Name != "" && tags.Name != null && page.IsPublished
                        group tags by tags.Name into gTags
                        select new { TagName = gTags.Key, Count = gTags.Count() };
            return query;

        }

        #endregion

        #region Categories
        /// <summary>
        /// Retrieve a set of PageCategories and count for hierarchy
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="webpageId"></param>
        /// <returns></returns>
        public IQueryable GetPageCategoriesByPage(int companyid, int webpageId)
        {
            var query = from page in GetChildPages(companyid, webpageId, true)
                        join cat in DbContext.PageCategories on page.WebPageId equals cat.WebPageId
                        where cat.Name != "" && cat.Name != null && page.IsPublished
                        group cat by cat.Name into gTags
                        select new { Name = gTags.Key, Count = gTags.Count() };
            return query;

        }
        #endregion
    }
}