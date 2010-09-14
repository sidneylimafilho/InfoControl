using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System;
using System.IO;
using System.Threading;
using InfoControl;

namespace Vivina.Erp.DataClasses
{
    public partial class Company : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private EntityRef<Company> _company;

        [Association(Storage = "_company", ThisKey = "ReferenceCompanyId", IsForeignKey = true)]
        public Company ReferenceCompany
        {
            get { return _company.Entity; }            
        }

        public string GetMasterPagePath(WebPage page)
        {
            string baseDirectory = Thread.GetDomain().BaseDirectory;

            string masterPage = "Site";
            if (page != null && !String.IsNullOrEmpty(page.MasterPage))
                masterPage = page.MasterPage;


            if (page != null && page.WebPageId > 0)
                masterPage = page.Company.GetHomePath() + "/" + masterPage + ".master";
            else
                masterPage = GetHomePath() + "/" + masterPage + ".master";

            string localPath = baseDirectory + masterPage.Replace("~", "").Replace("/", "\\");

            if (!File.Exists(localPath))
                masterPage = "~/site/1/site.master";

            return masterPage;
        }

        public string GetMasterPagePath()
        {
            return GetMasterPagePath( null);
        }

        public string GetHomePath()
        {
            string filePath = "~/Site/";
            string appPath = Thread.GetDomain().BaseDirectory;
            string localPath = "";
            string tempPath = "";

          
            if (LegalEntityProfile == null)
                throw new NullReferenceException();

            if (CompanyId > 1)
            {
                string tempPathById = filePath + CompanyId + "/";
                string localPathById = appPath + tempPathById.Replace("~", "").Replace("/", "\\");

                string tempPathByDomain = filePath + LegalEntityProfile.Website.RemoveDpnInUrl() + "/";
                string localPathByDomain = appPath + tempPathByDomain.Replace("~", "").Replace("/", "\\");

                if (!String.IsNullOrEmpty(LegalEntityProfile.Website))
                {
                    if (Directory.Exists(localPathById) && !Directory.Exists(localPathByDomain))
                        Directory.Move(localPathById, localPathByDomain);

                    localPath = localPathByDomain;
                    tempPath = tempPathByDomain;
                }
                else
                {
                    localPath = localPathById;
                    tempPath = tempPathById;
                }

                if (!Directory.Exists(localPath))
                    Directory.CreateDirectory(localPath);

                return tempPath;
            }

            return filePath + "1/";
        }

        private string GetCompanyDirectory()
        {
            return GetCompanyDirectory(null);
        }

        private string GetCompanyDirectory(string path)
        {
            path = GetHomePath() + path + "/";
            string tempPath = Thread.GetDomain().BaseDirectory + path.Replace("~", "").Replace("/", "\\");
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            return path;
        }

        /// <summary>
        /// Return the TEMPLATES folder in Home Path
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GetDocumentTemplateDirectory()
        {
            string virtualPath = GetFilesDirectory() + "templates/";
            string fisicalPath = Thread.GetDomain().BaseDirectory + virtualPath.Replace("~", "").Replace("/", "\\");
            if (!Directory.Exists(fisicalPath))
                Directory.CreateDirectory(fisicalPath);
            return virtualPath;
        }

        public string GetBudgetsDirectory()
        {
            string virtualPath = GetFilesDirectory() + "budgets/";
            string fisicalPath = Thread.GetDomain().BaseDirectory + virtualPath.Replace("~", "").Replace("/", "\\");
            if (!Directory.Exists(fisicalPath))
                Directory.CreateDirectory(fisicalPath);
            return virtualPath;
        }

        public string GetFilesDirectory()
        {
            return GetCompanyDirectory("files");
        }


    }
}
