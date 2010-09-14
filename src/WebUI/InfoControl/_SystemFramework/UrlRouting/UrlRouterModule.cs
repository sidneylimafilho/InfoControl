using System;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using InfoControl.Web;
using InfoControl.Web.Configuration;

using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.SystemFramework.Web.UrlRouting
{
    public class UrlRouterModule : InfoControl.Web.UrlRewriting.UrlRewriterModule
    {
        protected override void LookForNewUrl(HttpContext context, string requestedUrl)
        {
            base.LookForNewUrl(context, requestedUrl);

            if (IsUserLogged)
            {
                string query = context.Request.Url.Query.Replace("?", "");

                using (CompanyManager manager = new CompanyManager(null))
                {
                    Company company = manager.GetCompanyByContext(Context);
                    if (company != null)
                    {
                        string newUrl;

                        //
                        // Try the company website
                        //
                        string webSite = ("" + company.LegalEntityProfile.Website).ToLower().Replace("http://", "").Replace("www.", "");
                        if (!String.IsNullOrEmpty(webSite) && !requestedUrl.Contains(webSite.ToLower()))
                        {
                            newUrl = "~/infocontrol/_companies/" + webSite + "/" + requestedUrl.ToLower().Replace("infocontrol/", "");

                            // rewrite the path..
                            if (newUrl != requestedUrl && System.IO.File.Exists(Context.Server.MapPath(newUrl)))
                            {
                                RewritePath(newUrl, String.Empty, query);
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
