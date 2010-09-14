using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using InfoControl.Security;

namespace InfoControl.Web.UrlRewriting
{
    public class UrlRewriterModule : HttpDataModule
    {
        private const string OriginalPath = "OriginalRequestPathInfo";
        private static bool _enabled;
        private readonly UrlRulesSectionCollection _rules = (ConfigurationManager.GetSection("InfoControl/UrlRewriting") as UrlRewriterSectionGroup).Rules;
        private string _newUrl;

        #region Properties

        public static bool Enabled
        {
            get { return _enabled; }
        }

        #endregion

        public override void Init(HttpApplication context)
        {
            base.Init(context);


            /* 
             * FormsAuthenticationModule  >  AuthenticateRequest 
             * Determines if the user is authenticated using forms authentication. If not, 
             * the user is automatically redirected to the specified logon page. 
             * 
             * FileAuthorizationMoudle    >  AuthorizeRequest 
             * When using Windows authentication, this HTTP module checks to ensure that the 
             * 
             * UrlAuthorizationModule     >  AuthorizeRequest 
             * Checks to make sure the requestor can access the specified URL. URL authorization 
             * is specified through the <authorization> and <location> elements in the Web.config file.
             * 
             */

            context.ResolveRequestCache += ContextResolveRequestCache;
            context.EndRequest += ContextPreRequestHandlerExecute;

            _enabled = true;
        }

        protected virtual void LookForNewUrl(HttpContext context, string requestedUrl)
        {
            string query = context.Request.Url.Query.Replace("?", "");

            if (!File.Exists(Context.Server.MapPath(requestedUrl)))
                foreach (UrlRulesSection rule in _rules)
                {
                    string lookFor = ResolveUrl(Context.Request.ApplicationPath, rule.From);
                    var reg = new Regex(lookFor, RegexOptions.IgnoreCase);
                    if (reg.IsMatch(requestedUrl))
                    {
                        _newUrl = reg.Replace(requestedUrl, ResolveUrl(Context.Request.ApplicationPath, rule.To.Replace("^", "&")));

                        if (_newUrl.IndexOf("?") > 0)
                        {
                            query = _newUrl.Substring(_newUrl.IndexOf("?") + 1) + query;
                            _newUrl = _newUrl.Substring(0, _newUrl.IndexOf("?"));
                        }

                        // rewrite the path..
                        if (File.Exists(Context.Server.MapPath(_newUrl)))
                        {
                            // query.Trim('&') because DataPager WebControl bug when trailing & in queryString
                            RewritePath(_newUrl, String.Empty, query.Trim('&'));
                            break;
                        }
                    }
                }
        }

        protected void RewritePath(string filePath, string pathInfo, string queryString)
        {
            Context.Items[OriginalPath] = Context.Request.Path;
            Context.RewritePath(filePath, pathInfo, queryString);
        }

        #region Events

        private void ContextResolveRequestCache(object sender, EventArgs e)
        {
            Context.Trace.Warn("UrlRewriter", "Begin OnBeginRequest");

            if (IsRequestValid())
            {
                string requestedUrl = ResolveUrl(Context.Request.ApplicationPath, Context.Request.Url.AbsolutePath);
                LookForNewUrl(Context, requestedUrl);
            }

            Context.Trace.Warn("UrlRewriter", "End OnBeginRequest");
        }

        private static void ContextPreRequestHandlerExecute(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app == null) return;

            var originalPath = app.Context.Items[OriginalPath] as String;
            if (!string.IsNullOrEmpty(originalPath))
                app.Context.RewritePath(originalPath, true);
        }

        #endregion

        #region Utility Functions

        private static string ResolveUrl(String appPath, String url)
        {
            // String is Empty, just return Url
            if (url.Length == 0)
                return url;

            // String does not contain a ~, so just return Url
            if (url.StartsWith("~") == false)
                return url;

            // There is just the ~ in the Url, return the appPath
            if (url.Length == 1)
                return appPath;

            if (url.ToCharArray()[1] == '/' || url.ToCharArray()[1] == '\\')
            {
                // Url looks like ~/ or ~\
                if (appPath.Length > 1)
                    return appPath + "/" + url.Substring(2);

                return "/" + url.Substring(2);
            }
            // Url look like ~something
            if (appPath.Length > 1)
                return appPath + "/" + url.Substring(1);

            return appPath + url.Substring(1);
        }

        private bool IsRequestValid()
        {
            // URL validation 
            // check for ".." escape characters commonly used by hackers to traverse the folder tree on the server
            // the application should always use the exact relative location of the resource it is requesting
            String strUrl = Context.Request.Url.AbsolutePath;
            String strDoubleDecodeUrl = Context.Server.UrlDecode(Context.Server.UrlDecode(Context.Request.RawUrl));
            if (Regex.Match(strUrl, @"[\\/]\.\.[\\/]").Success || Regex.Match(strDoubleDecodeUrl, @"[\\/]\.\.[\\/]").Success)
                throw new HttpException(404, "Not Found");

            return true;
        }

        #endregion
    }
}