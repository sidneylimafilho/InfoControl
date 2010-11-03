using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using InfoControl.Data;
using InfoControl.Web.Auditing;
using InfoControl.Web.Configuration;
using InfoControl.Properties;
using InfoControl.Web.Security;

namespace InfoControl.Web.UI
{ 
    /// <summary>
    /// Represents a InfoControl.Web.UI.Page with capabilities of the data access and maintenance of transactions in context, through of a DataManager
    /// </summary>      
    public class DataPage : Page, IDataAccessor, IRequiresSessionState
    {
        private DataManager _DataManager;
        private Dictionary<string, object> _dataMembers = new Dictionary<string, object>();
        private SessionPageStatePersister _pageStatePersister;
        private Dictionary<string, IList<ISerializable>> _viewStateLists;
        private DataSet _viewStateTables;
        private Dictionary<Type, BusinessManager> managersList = new Dictionary<Type, BusinessManager>();

        /// <summary>
        /// Store a dataset in View State
        /// </summary>
        public DataSet ViewStateTables
        {
            get
            {
                if (_viewStateTables == null)
                    _viewStateTables = new DataSet();

                if (ViewState["__VIEWSTATE_TABLES"] != null)
                    _viewStateTables = ViewState["__VIEWSTATE_TABLES"] as DataSet;

                ViewState["__VIEWSTATE_TABLES"] = _viewStateTables;

                return _viewStateTables;
            }
        }

        /// <summary>
        /// Store a List in View State
        /// </summary>
        public Dictionary<string, IList<ISerializable>> ViewStateLists
        {
            get
            {
                if (_viewStateLists == null)
                    _viewStateLists = new Dictionary<string, IList<ISerializable>>();

                if (ViewState["__VIEWSTATE_LISTS"] != null)
                    _viewStateLists = (Dictionary<string, IList<ISerializable>>)ViewState["__VIEWSTATE_LISTS"];

                return _viewStateLists;
            }
        }

        public Dictionary<string, object> DataMembers
        {
            get { return _dataMembers; }
        }

        /// <summary>
        /// Get the information about the user making the page request
        /// </summary>
        public new AccessControlPrincipal User
        {
            get
            {
                if (!(Context.User is AccessControlPrincipal))
                    throw new InvalidCastException(Resources.VivinaRoleProviderNotConfigured);

                return Context.User as AccessControlPrincipal;
            }
        }

        public Hashtable Customization
        {
            get
            {
                if (User.Personalization[Page.GetType().FullName] == null)
                    User.Personalization[Page.GetType().FullName] = new Hashtable();

                return User.Personalization[Page.GetType().FullName] as Hashtable;
            }
        }

        /// <summary>
        /// Get the information about the current application
        /// </summary>
        public new Application Application
        {
            get { return Application.Current; }
        }

        /// <summary>
        /// Get the application state for the current web request
        /// </summary>
        public HttpApplicationState ApplicationState
        {
            get { return HttpContext.Current.Application; }
        }

        protected override PageStatePersister PageStatePersister
        {
            get
            {
                if (_pageStatePersister == null)
                    _pageStatePersister = new SessionPageStatePersister(this);

                return _pageStatePersister;
            }
        }

        /// <summary>
        /// Gets a reference to the InfoControl.Web.UI.DataPage instance that contains the server
        ///     control.
        /// </summary>
        public new DataPage Page
        {
            get { return this; }
        }

        #region IDataAccessor Members

        /// <summary>
        /// Provides helper functions that implements data access
        /// </summary>
        public DataManager DataManager
        {
            get
            {
                //
                // Check if exists a DataManager in context
                //
                if (_DataManager == null)
                {
                    if (HttpContext.Current.Items[Resources.CurrentDataManagerKey] != null)
                    {
                        _DataManager = HttpContext.Current.Items[Resources.CurrentDataManagerKey] as DataManager;
                    }
                    else
                    {
                        _DataManager = new DataManager();
                        HttpContext.Current.Items[Resources.CurrentDataManagerKey] = _DataManager;
                    }
                }

                return _DataManager;
            }
        }

        #endregion

        public T GetManager<T>(IDataAccessor acessor) where T : BusinessManager
        {
            return managersList[typeof(T)] as T ?? (managersList[typeof(T)] = (BusinessManager)Activator.CreateInstance(typeof(T), acessor)) as T;
        }

        public T GetManager<T>() where T : BusinessManager
        {
            return GetManager<T>(null);
        }

        protected override void OnPreInit(EventArgs e)
        {
            Trace.Warn("DataPage", "Begin OnPreInit");
            base.OnPreInit(e);
            Trace.Warn("DataPage", "End OnPreInit");
        }

        protected override void OnLoad(EventArgs e)
        {
            Trace.Warn("DataPage", "OnLoad");
            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            try
            {
                Trace.Warn("DataPage", "OnUnload");
                DataManager.Commit();
            }
            catch (Exception ex)
            {
                Context.AddError(ex);
            }

            
            base.OnUnload(e);
        }

        protected override void OnError(EventArgs e)
        {
            try
            {
                Trace.Warn("DataPage.OnError");
                DataManager.Rollback();
            }
            catch (Exception ex)
            {
                Context.AddError(ex);
            }

            base.OnError(e);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Trace.Warn("Page Init");
        }

        private bool CanAccess()
        {
            object[] permList = GetType().GetCustomAttributes(typeof(PermissionRequiredAttribute), true);
            object[] roleList = GetType().GetCustomAttributes(typeof(RoleRequiredAttribute), true);

            //
            // Check the page has permissions or role validators
            //
            if (permList.Length == 0 && roleList.Length == 0)
            {
                return true;
            }

            //
            // Check Permissions
            //
            foreach (object perm in permList)
            {
                if (User.Permissions.ContainsKey((perm as PermissionRequiredAttribute).PermissionName))
                {
                    return true;
                }
            }

            // 
            // Check Roles
            //
            foreach (object role in roleList)
            {
                if (User.Roles.Contains((role as RoleRequiredAttribute).RoleName))
                {
                    return true;
                }
            }

            return false;
        }

        protected override void InitializeCulture()
        {
            if (!String.IsNullOrEmpty(Request["cultureInfo"]))
                Session["cultureInfo"] = Request["cultureInfo"];

            if (Session["cultureInfo"] != null)
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["cultureInfo"].ToString());

            base.InitializeCulture();
        }


        #region Events

        public event EventHandler LoadingData;

        /// <summary>
        /// Disparado antes de carregar os dados nos controles
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLoadingData(EventArgs e)
        {
            if (LoadingData != null)
                LoadingData(this, e);
        }

        public event EventHandler LoadedData;

        /// <summary>
        /// Disparado depois de carregar os dados nos controles
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLoadedData(EventArgs e)
        {
            if (LoadedData != null)
                LoadedData(this, e);
        }

        #endregion
    }

    
}