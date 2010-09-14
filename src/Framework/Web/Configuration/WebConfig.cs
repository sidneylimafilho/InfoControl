using System;
using System.ServiceModel;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Net.Configuration;
using System.Web.Configuration.Internal;
using System.Web.Configuration;
using System.Text;

using InfoControl.Web;
using InfoControl.Web.Personalization;
using System.ServiceModel.Configuration;

namespace InfoControl.Web.Configuration
{
    public class WebConfig : InfoControl.Configuration.AppConfig
    {
        #region InfoControl.
        //private static PersonalizationSection _personalization;
        //public static PersonalizationSection Personalization
        //{
        //    get
        //    {
        //        if (_personalization == null)
        //            _personalization = GetSection("InfoControl/Personalization") as PersonalizationSection;
        //        return _personalization;
        //    }
        //}
        #endregion

        public static class Web
        {
            private static AnonymousIdentificationSection _anonymousIdentification;
            public static AnonymousIdentificationSection AnonymousIdentification
            {
                get
                {
                    if (_anonymousIdentification == null)
                        _anonymousIdentification = (AnonymousIdentificationSection)GetSection("system.web/anonymousIdentification");
                    return _anonymousIdentification;
                }
            }

            private static AuthenticationSection _authentication;
            public static AuthenticationSection Authentication
            {
                get
                {
                    if (_authentication == null)
                        _authentication = (AuthenticationSection)GetSection("system.web/authentication");
                    return _authentication;
                }
            }

            private static AuthorizationSection _authorization;
            public static AuthorizationSection Authorization
            {
                get
                {
                    if (_authorization == null)
                        _authorization = (AuthorizationSection)GetSection("system.web/authorization");
                    return _authorization;
                }
            }

            private static CacheSection _cache;
            public static CacheSection Cache
            {
                get
                {
                    if (_cache == null)
                        _cache = (CacheSection)GetSection("system.web/caching/cache");
                    return _cache;
                }
            }

            private static ClientTargetSection _clientTarget;
            public static ClientTargetSection ClientTarget
            {
                get
                {
                    if (_clientTarget == null)
                        _clientTarget = (ClientTargetSection)GetSection("system.web/clientTarget");
                    return _clientTarget;
                }
            }

            private static CompilationSection _compilation;
            public static CompilationSection Compilation
            {
                get
                {
                    if (_compilation == null)
                        _compilation = (CompilationSection)GetSection("system.web/compilation");
                    return _compilation;
                }
            }

            private static CustomErrorsSection _customErrors;
            public static CustomErrorsSection CustomErrors
            {
                get
                {
                    if (_customErrors == null)
                        _customErrors = (CustomErrorsSection)GetSection("system.web/customErrors");
                    return _customErrors;
                }
            }

            private static DeploymentSection _deployment;
            public static DeploymentSection Deployment
            {
                get
                {
                    if (_deployment == null)
                        _deployment = (DeploymentSection)GetSection("system.web/deployment");
                    return _deployment;
                }
            }

            private static GlobalizationSection _globalization;
            public static GlobalizationSection Globalization
            {
                get
                {
                    if (_globalization == null)
                        _globalization = (GlobalizationSection)GetSection("system.web/globalization");
                    return _globalization;
                }
            }

            private static HealthMonitoringSection _healthMonitoring;
            public static HealthMonitoringSection HealthMonitoring
            {
                get
                {
                    if (_healthMonitoring == null)
                        _healthMonitoring = (HealthMonitoringSection)GetSection("system.web/healthMonitoring");
                    return _healthMonitoring;
                }
            }

            private static HostingEnvironmentSection _hostingEnvironment;
            public static HostingEnvironmentSection HostingEnvironment
            {
                get
                {
                    if (_hostingEnvironment == null)
                        _hostingEnvironment = (HostingEnvironmentSection)GetSection("system.web/hostingEnvironment");
                    return _hostingEnvironment;
                }
            }

            private static HttpCookiesSection _httpCookies;
            public static HttpCookiesSection HttpCookies
            {
                get
                {
                    if (_httpCookies == null)
                        _httpCookies = (HttpCookiesSection)GetSection("system.web/httpCookies");
                    return _httpCookies;
                }
            }

            private static HttpHandlersSection _httpHandlers;
            public static HttpHandlersSection HttpHandlers
            {
                get
                {
                    if (_httpHandlers == null)
                        _httpHandlers = (HttpHandlersSection)GetSection("system.web/httpHandlers");
                    return _httpHandlers;
                }
            }

            private static HttpModulesSection _httpModules;
            public static HttpModulesSection HttpModules
            {
                get
                {
                    if (_httpModules == null)
                        _httpModules = (HttpModulesSection)GetSection("system.web/httpModules");
                    return _httpModules;
                }
            }

            private static HttpRuntimeSection _httpRuntime;
            public static HttpRuntimeSection HttpRuntime
            {
                get
                {
                    if (_httpRuntime == null)
                        _httpRuntime = (HttpRuntimeSection)GetSection("system.web/httpRuntime");
                    return _httpRuntime;
                }
            }

            private static IdentitySection _identity;
            public static IdentitySection Identity
            {
                get
                {
                    if (_identity == null)
                        _identity = (IdentitySection)GetSection("system.web/identity");
                    return _identity;
                }
            }

            private static MachineKeySection _machineKey;
            public static MachineKeySection MachineKey
            {
                get
                {
                    if (_machineKey == null)
                        _machineKey = (MachineKeySection)GetSection("system.web/machineKey");
                    return _machineKey;
                }
            }

            private static MembershipSection _membership;
            public static MembershipSection Membership
            {
                get
                {
                    if (_membership == null)
                        _membership = (MembershipSection)GetSection("system.web/membership");
                    return _membership;
                }
            }

            private static OutputCacheSection _outputCache;
            public static OutputCacheSection OutputCache
            {
                get
                {
                    if (_outputCache == null)
                        _outputCache = (OutputCacheSection)GetSection("system.web/caching/outputCache");
                    return _outputCache;
                }
            }

            private static OutputCacheSettingsSection _outputCacheSettings;
            public static OutputCacheSettingsSection OutputCacheSettings
            {
                get
                {
                    if (_outputCacheSettings == null)
                        _outputCacheSettings = (OutputCacheSettingsSection)GetSection("system.web/caching/outputCacheSettings");
                    return _outputCacheSettings;
                }
            }

            private static PagesSection _pages;
            public static PagesSection Pages
            {
                get
                {
                    if (_pages == null)
                        _pages = (PagesSection)GetSection("system.web/pages");
                    return _pages;
                }
            }

            private static ProcessModelSection _processModel;
            public static ProcessModelSection ProcessModel
            {
                get
                {
                    if (_processModel == null)
                        _processModel = (ProcessModelSection)GetSection("system.web/processModel");
                    return _processModel;
                }
            }

            private static ProfileSection _profile;
            public static ProfileSection Profile
            {
                get
                {
                    if (_profile == null)
                        _profile = (ProfileSection)GetSection("system.web/profile");
                    return _profile;
                }
            }

            private static RoleManagerSection _roleManager;
            public static RoleManagerSection RoleManager
            {
                get
                {
                    if (_roleManager == null)
                        _roleManager = (RoleManagerSection)GetSection("system.web/roleManager");
                    return _roleManager;
                }
            }

            private static SecurityPolicySection _securityPolicy;
            public static SecurityPolicySection SecurityPolicy
            {
                get
                {
                    if (_securityPolicy == null)
                        _securityPolicy = (SecurityPolicySection)GetSection("system.web/securityPolicy");
                    return _securityPolicy;
                }
            }



            private static SessionPageStateSection _sessionPageState;
            public static SessionPageStateSection SessionPageState
            {
                get
                {
                    if (_sessionPageState == null)
                        _sessionPageState = (SessionPageStateSection)GetSection("system.web/sessionPageState");
                    return _sessionPageState;
                }
            }

            private static SessionStateSection _sessionState;
            public static SessionStateSection SessionState
            {
                get
                {
                    if (_sessionState == null)
                        _sessionState = (SessionStateSection)GetSection("system.web/sessionState");
                    return _sessionState;
                }
            }

            private static SiteMapSection _siteMap;
            public static SiteMapSection SiteMap
            {
                get
                {
                    if (_siteMap == null)
                        _siteMap = (SiteMapSection)GetSection("system.web/siteMap");
                    return _siteMap;
                }
            }

            private static SmtpSection _smtp;
            public static SmtpSection Smtp
            {
                get
                {
                    if (_smtp == null)
                        _smtp = (SmtpSection)GetSection("system.net/mailSettings/smtp");
                    return _smtp;
                }
            }

            private static SqlCacheDependencySection _sqlCacheDependency;
            public static SqlCacheDependencySection SqlCacheDependency
            {
                get
                {
                    if (_sqlCacheDependency == null)
                        _sqlCacheDependency = (SqlCacheDependencySection)GetSection("system.web/caching/sqlCacheDependency");
                    return _sqlCacheDependency;
                }
            }

            private static TraceSection _trace;
            public static TraceSection Trace
            {
                get
                {
                    if (_trace == null)
                        _trace = (TraceSection)GetSection("system.web/trace");
                    return _trace;
                }
            }

            private static TrustSection _trust;
            public static TrustSection Trust
            {
                get
                {
                    if (_trust == null)
                        _trust = (TrustSection)GetSection("system.web/trust");
                    return _trust;
                }
            }

            private static UrlMappingsSection _urlMappings;
            public static UrlMappingsSection UrlMappings
            {
                get
                {
                    if (_urlMappings == null)
                        _urlMappings = (UrlMappingsSection)GetSection("system.web/urlMappings");
                    return _urlMappings;
                }
            }

            private static WebControlsSection _webControls;
            public static WebControlsSection WebControls
            {
                get
                {
                    if (_webControls == null)
                        _webControls = (WebControlsSection)GetSection("system.web/webControls");
                    return _webControls;
                }
            }

            private static WebPartsSection _webParts;
            public static WebPartsSection WebParts
            {
                get
                {
                    if (_webParts == null)
                        _webParts = (WebPartsSection)GetSection("system.web/webParts");
                    return _webParts;
                }
            }

            private static XhtmlConformanceSection _xhtmlConformance;
            public static XhtmlConformanceSection XhtmlConformance
            {
                get
                {
                    if (_xhtmlConformance == null)
                        _xhtmlConformance = (XhtmlConformanceSection)GetSection("system.web/xhtmlConformance");
                    return _xhtmlConformance;
                }
            }
        }

        public static class ServiceModel
        {
            private static BindingsSection _binding;
            public static BindingsSection Bindings
            {
                get
                {
                    return _binding ?? (_binding = (BindingsSection)GetSection("system.serviceModel/client"));
                }
            }

            private static ClientSection _client;
            public static ClientSection Clients
            {
                get
                {
                    if (_client == null)
                        _client = (ClientSection)GetSection("system.serviceModel/client");
                    return _client;
                }
            }
        }
    }
}
