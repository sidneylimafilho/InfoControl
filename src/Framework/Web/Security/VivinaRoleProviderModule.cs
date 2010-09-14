using System;
using System.IO;
using System.Configuration;
using System.Web;
using System.Threading;
using System.Web.Security;
using System.Collections;
using System.Collections.Generic;
using System.Text;


using InfoControl.Data;
using InfoControl.IO.Compression;
using InfoControl.Runtime;
using InfoControl.Web.UI;
using InfoControl.Web.Security;
using InfoControl.Security.Cryptography;

namespace InfoControl.Web.Security
{
    public class VivinaRoleProviderModule : HttpDataModule
    {

        #region IHttpModule Members

        public override void Dispose()
        {

        }

        public override void Init(HttpApplication context)
        {
            base.Init(context);


            context.PostAcquireRequestState += new EventHandler(context_PostAcquireRequestState);
            context.EndRequest += new EventHandler(context_ReleaseRequestState);
        }

        void context_ReleaseRequestState(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;

            if (context.User is IDisposable)
                (context.User as IDisposable).Dispose();


            if (Roles.CacheRolesInCookie)
                if (context.User != null && context.User is AccessControlPrincipal && (context.User as AccessControlPrincipal).IsAuthenticated)
                {
                    if (Roles.CookieRequireSSL && !context.Request.IsSecureConnection)
                    {
                        if (context.Request.Cookies[Roles.CookieName] != null)
                            Roles.DeleteCookie();
                    }
                    else
                    {
                        AccessControlPrincipal principal = (AccessControlPrincipal)context.User;
                        if (context.Request.Browser.Cookies)
                        {
                            HttpCookie cookie = new HttpCookie(Roles.CookieName, HttpUtility.UrlEncode(principal.Serialize()));
                            cookie.HttpOnly = true;
                            cookie.Path = Roles.CookiePath;
                            cookie.Domain = Roles.Domain;

                            if (!Roles.CreatePersistentCookie)
                            {
                                cookie.Expires = (principal as AccessControlPrincipal).Identity.LastActivityDate.AddMinutes(20);
                            }

                            if (Roles.CookieProtectionValue == CookieProtection.Encryption)
                            {
                                cookie.Value = cookie.Value.Encrypt().Compress();
                            }

                            cookie.Secure = Roles.CookieRequireSSL;
                            context.Response.Cookies.Add(cookie);
                        }
                    }
                }
        }

        void context_PostAcquireRequestState(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            MembershipManager membershipManager = new MembershipManager(null);
            AccessControlPrincipal principal = null;
            DataEntities.User user = null;
            System.Security.Principal.IIdentity identity = null;

            if (IsValid)
            {
                context.Trace.Warn("Role Module Begin");

                #region Cookies

                if ((!Roles.CookieRequireSSL || context.Request.IsSecureConnection))
                {
                    if (Roles.CacheRolesInCookie)
                    {
                        HttpCookie cookie = context.Request.Cookies[Roles.CookieName];

                        if (cookie != null && cookie.Value != null)
                        {
                            if (!string.IsNullOrEmpty(Roles.CookiePath) && (Roles.CookiePath != "/"))
                                cookie.Path = Roles.CookiePath;

                            if (Roles.CookieProtectionValue == CookieProtection.Encryption)
                                cookie.Value = cookie.Value.Decrypt().Decompress();

                            cookie.Domain = Roles.Domain;
                            context.User = HttpUtility.UrlDecode(cookie.Value).Deserialize<AccessControlPrincipal>();
                        }
                        else { Roles.DeleteCookie(); }
                    }
                    else { Roles.DeleteCookie(); }

                }
                else { Roles.DeleteCookie(); }

                #endregion

                identity = context.User.Identity;
                principal = new AccessControlPrincipal(user, identity);

                if (context.Session != null && context.Session[context.Session.SessionID] != null && identity.IsAuthenticated)
                    principal = context.Session[context.Session.SessionID] as AccessControlPrincipal;


                if (String.IsNullOrEmpty(principal.Name) && !String.IsNullOrEmpty(identity.Name))
                {
                    user = membershipManager.GetUserByName(identity.Name);
                    if (user != null)
                    {
                        bool timeoutExpired = DateTime.Now.Subtract(user.LastActivityDate).Minutes > System.Web.Security.Membership.UserIsOnlineTimeWindow;
                        user.LastActivityDate = DateTime.Now;
                        user.IsOnline = !timeoutExpired;

                        membershipManager.DbContext.SubmitChanges();
                        membershipManager.DataManager.Commit();
                    }

                    principal = new AccessControlPrincipal(user, identity);
                }


                if (context.Application["Session_End"] == null)
                    context.Application["Session_End"] = new EventHandler(OnSessionEnd);

                //
                // Cache the user in session to dont query database
                //
                if (context.Session != null && identity.IsAuthenticated)
                    context.Session[context.Session.SessionID] = principal;

                System.Threading.Thread.CurrentPrincipal = principal;
                context.User = principal;

                context.Trace.Warn("Role Module End");
            }
        }


        void OnSessionEnd(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            System.Web.SessionState.HttpSessionState session = application.Session;
            
            using (MembershipManager membershipManager = new MembershipManager(null))
            {
                AccessControlPrincipal principal = session[session.SessionID] as AccessControlPrincipal;
                if (principal != null && principal.Identity != null)
                    membershipManager.Logoff(principal);
            }
            session.RemoveAll();
        }
        #endregion

        private bool IsValid
        {
            get
            {
                return IsDotNetRequest && Roles.Enabled;
            }
        }
    }
}
