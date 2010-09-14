using System;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using System.Web.Hosting;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Data;

using InfoControl.Runtime;

namespace InfoControl.Web.Personalization.WebParts
{
    public class CookiesPersonalizationProvider : PersonalizationProvider
    {
        #region Members
        string cookiesName;
        int cookiesTimeout;
        bool createPersistentCookie;
        string xmlFile;
        #endregion

        #region Properties
        public override string ApplicationName
        {
            get
            {
                return Configuration.Application.Current.Name;
            }
            set
            {
                // Do nothing
            }
        }
        #endregion

        #region UtilityMethods
        private DataTable CreatePersonalizationBlobs()
        {
            DataTable currentTable = new DataTable(cookiesName);
            currentTable.Columns.Add("Path");
            currentTable.Columns.Add("Blob");
            return currentTable;
        }
        protected byte[] GetPersonalizationBlob(DataTable table, string pagePath)
        {
            foreach (DataRow r in table.Rows)
            {
                if ((string)r["Path"] == pagePath)
                {
                    return Convert.FromBase64String((string)r["Blob"]);
                }
            }
            return Convert.FromBase64String(string.Empty);
        }
        protected void SetPersonalizationBlob(DataTable table, string pagePath, string blob)
        {
            foreach (DataRow r in table.Rows)
            {
                if ((string)r["Path"] == pagePath)
                {
                    r["Blob"] = blob;
                    return;
                }
            }
            DataRow row = table.NewRow();
            row["Path"] = pagePath;
            row["Blob"] = blob;
            table.Rows.Add(row);
            table.AcceptChanges();
        }
        private DataTable GetPersonalizationBlobs(WebPartManager webPartManager, string username)
        {
            //
            // Verifies if still has a DataTable loaded
            //
            DataTable personalizationBlobs = null;
            if (HttpContext.Current.Items[cookiesName] != null)
                return HttpContext.Current.Items[cookiesName] as DataTable;

            switch (GetScope(webPartManager, username))
            {
                case PersonalizationScope.User:
                    HttpCookie cookie = webPartManager.Page.Request.Cookies[cookiesName];
                    if (cookie != null)
                    {
                        personalizationBlobs = cookie.Value.Deserialize() as DataTable;
                    }
                    break;
                case PersonalizationScope.Shared:
                    personalizationBlobs = webPartManager.Page.Cache[cookiesName] as DataTable;
                    if (personalizationBlobs == null)
                    {
                        if (File.Exists(xmlFile))
                        {
                            DataSet ds = new DataSet();
                            ds.ReadXml(xmlFile);
                            personalizationBlobs = ds.Tables[cookiesName];
                            ds.Tables.Remove(personalizationBlobs); ;
                        }
                    }
                    break;
            }


            if (personalizationBlobs == null)
                personalizationBlobs = CreatePersonalizationBlobs();

            HttpContext.Current.Items[cookiesName] = personalizationBlobs;
            return personalizationBlobs;
        }

        private PersonalizationScope GetScope(WebPartManager webPartManager, string userName)
        {

            //
            // Checks if the scope will Initial or not
            //            
            if (String.IsNullOrEmpty(userName))
            {
                return webPartManager.Personalization.InitialScope;
            }
            else
            {
                return PersonalizationScope.User;
            }            
        }
        #endregion

        #region Methods

        protected override void LoadPersonalizationBlobs(
            WebPartManager webPartManager,
            string path,
            string userName,
            ref byte[] sharedDataBlob,
            ref byte[] userDataBlob)
        {

            DataTable personalizationBlobs = GetPersonalizationBlobs(webPartManager, userName);
            sharedDataBlob = GetPersonalizationBlob(personalizationBlobs, path);
            if (!String.IsNullOrEmpty(userName))
                userDataBlob = sharedDataBlob;
        }

        
        protected override void ResetPersonalizationBlob(
            WebPartManager webPartManager,
            string path,
            string userName)
        {
            switch (webPartManager.Personalization.Scope)
            {
                case PersonalizationScope.User:
                    HttpCookie cookie = new HttpCookie(cookiesName);
                    cookie.Value = String.Empty;
                    if (createPersistentCookie)
                    {
                        cookie.Expires = DateTime.MaxValue;
                    }
                    else
                    {
                        cookie.Expires = DateTime.Now.AddMinutes(cookiesTimeout);
                    }
                    webPartManager.Page.Response.Cookies.Add(cookie);
                    break;
                case PersonalizationScope.Shared:
                    File.Delete(xmlFile);
                    webPartManager.Page.Cache.Remove(cookiesName);
                    break;
            }
        }

        
        protected override void SavePersonalizationBlob(
            WebPartManager webPartManager,
            string path,
            string userName,
            byte[] dataBlob)
        {
            DataTable personalizationBlobs = GetPersonalizationBlobs(webPartManager, userName);
            SetPersonalizationBlob(personalizationBlobs, path, Convert.ToBase64String(dataBlob));

            switch (GetScope(webPartManager, userName))
            {
                case PersonalizationScope.User:
                    HttpCookie cookie = new HttpCookie(cookiesName);
                    cookie.Value = personalizationBlobs.SerializeToString();
                    if (createPersistentCookie)
                    {
                        cookie.Expires = DateTime.Now.AddYears(999);
                    }
                    else
                    {
                        cookie.Expires = DateTime.Now.AddMinutes(cookiesTimeout);
                    }
                    webPartManager.Page.Response.Cookies.Add(cookie);

                    break;
                case PersonalizationScope.Shared:
                    personalizationBlobs.WriteXml(xmlFile);
                    webPartManager.Page.Cache.Insert(cookiesName, personalizationBlobs, new System.Web.Caching.CacheDependency(xmlFile));
                    break;
            }
        }

        public override PersonalizationScope DetermineInitialScope(WebPartManager webPartManager, PersonalizationState loadedState)
        {
            PersonalizationScope scope = base.DetermineInitialScope(webPartManager, loadedState);
            return scope;
        }
       

        public override int GetCountOfState(
          PersonalizationScope scope,
          PersonalizationStateQuery query)
        {
            return 0;
        }

        public override PersonalizationStateInfoCollection FindState(
          PersonalizationScope scope,
          PersonalizationStateQuery query,
          int pageIndex,
          int pageSize,
          out int totalRecords)
        {
            totalRecords = 0;
            return null;
        }

        public override int ResetState(
          PersonalizationScope scope,
          string[] paths,
          string[] usernames)
        {
            return 0;
        }

        public override int ResetUserState(
          string path,
          DateTime userInactiveSinceDate)
        {
            return 0;
        }
        #endregion

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {

            base.Initialize(name, config);


            cookiesName = Convert.ToString(config["cookiesName"]);
            cookiesTimeout = Convert.ToInt32(config["cookiesTimeout"]);
            createPersistentCookie = Convert.ToBoolean(config["createPersistentCookie"]);
            xmlFile = System.Web.Hosting.HostingEnvironment.MapPath(config["xmlFile"]);
        }
    }


}

