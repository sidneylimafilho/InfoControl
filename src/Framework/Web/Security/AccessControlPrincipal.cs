using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Security;
using System.Security.AccessControl;
using System.Web;
using System.Data.Linq;


using InfoControl.Runtime;


namespace InfoControl.Web.Security
{
    /// <summary>
    /// Represents security information for the current HTTP request, defined for Access Control
    /// </summary>
    [Serializable]
    public partial class AccessControlPrincipal : IPrincipal, System.Web.SessionState.IRequiresSessionState
    {
        private DataEntities.User _user;
        private IIdentity _identity;

        #region IPrincipal Members
        public DataEntities.User Identity
        {
            get { return _user; }
        }
        IIdentity IPrincipal.Identity
        {
            get { return _identity; }
        }
        #endregion

        #region Properties

        private Dictionary<string, AccessControlActions> _permissions;
        public Dictionary<string, AccessControlActions> Permissions
        {
            get
            {
                if (_permissions == null)
                    using (MembershipManager manager = new MembershipManager(null))
                        _permissions = manager.GetPermissions(_user.UserId);

                return _permissions;
            }
        }

        private List<string> _roles;
        public List<string> Roles
        {
            get
            {
                if (_roles == null)
                    using (var manager = new MembershipManager(null))
                        _roles = manager.GetRoles(_user.UserId);

                return _roles;
            }
        }

        private Hashtable _items;
        public Hashtable Personalization
        {
            get
            {
                if (_items == null)
                {
                    _items = new Hashtable();

                    if (_user != null && !String.IsNullOrEmpty(_user.PersonalizationRaw))
                        _items = _user.PersonalizationRaw.Deserialize<Hashtable>();
                }

                return _items;
            }
        }


        // Summary:
        //     Gets the type of authentication used.
        //
        // Returns:
        //     The type of authentication used to identify the user.
        public string AuthenticationType { get { return _identity == null ? "" : _identity.AuthenticationType; } }
        //
        // Summary:
        //     Gets a value that indicates whether the user has been authenticated.
        //
        // Returns:
        //     true if the user was authenticated; otherwise, false.
        public bool IsAuthenticated { get { return _identity == null ? false : _identity.IsAuthenticated; } }
        //
        // Summary:
        //     Gets the name of the current user.
        //
        // Returns:
        //     The name of the user on whose behalf the code is running.
        public string Name { get { return _user.Profile.Name ?? ""; } }

        #endregion

        #region Methods
        public void RefreshCredentials()
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session["_permissions"] = null;
                HttpContext.Current.Session["_roles"] = null;
            }
            _permissions = null;
            _roles = null;
        }

        public bool IsInRole(string role)
        {
            return Roles.Contains(role);
        }

        /// <summary> Indicate if a user has access to view the funcionality
        /// </summary>
        /// <param name="token">Token that labeled a funcionality</param>
        /// <returns></returns>
        public bool CanView(string token)
        {
            return Permissions.ContainsKey(token);
        }

        /// <summary> Indicate if a user has access to change the data in funcionality
        /// </summary>
        /// <param name="token">Token that labeled a funcionality</param>
        /// <returns></returns>
        public bool CanChange(string token)
        {
            if (Permissions.ContainsKey(token))
            {
                return Permissions[token] == AccessControlActions.Change;
            }
            return false;
        }
        #endregion

        public AccessControlPrincipal(DataEntities.User user, IIdentity identity)
        {
            _user = user ?? new DataEntities.User();
            _user.Profile = _user.Profile ?? new DataEntities.Profile();
            //_user.Profile.Name = (user != null ? _user.Profile.Name : identity.Name);

            _identity = identity;
        }

       
    }
}
