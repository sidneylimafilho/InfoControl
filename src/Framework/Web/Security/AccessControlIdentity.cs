using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Principal;
using System.Security.AccessControl;

namespace InfoControl.Web.Security
{
    /// <summary>
    /// Represents a user identity authenticated in Access Control
    /// </summary>
    [Serializable]
    public partial class AccessControlIdentity : VivinaMembershipUser, IIdentity
    {
        internal IIdentity identity;


        public AccessControlIdentity(DataEntities.User user, IIdentity ident)
            :
            base(user)
        {
            identity = ident;


        }

        #region IIdentity Members

        string IIdentity.AuthenticationType
        {
            get { return identity.AuthenticationType; }
        }

        bool IIdentity.IsAuthenticated
        {
            get { return identity.IsAuthenticated; }
        }

        string IIdentity.Name
        {
            get { return identity.Name; }
        }

        #endregion


    }
}
