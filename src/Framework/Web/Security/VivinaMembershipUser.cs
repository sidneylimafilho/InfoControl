using System;
using System.Web.Security;

namespace InfoControl.Web.Security
{
    [Serializable]
    public class VivinaMembershipUser : MembershipUser
    {
        public VivinaMembershipUser(string providername,
                                  string username,
                                  object providerUserKey,
                                  string email,
                                  string passwordQuestion,
                                  string comment,
                                  bool isApproved,
                                  bool isLockedOut,
                                  DateTime creationDate,
                                  DateTime lastLoginDate,
                                  DateTime lastActivityDate,
                                  DateTime lastPasswordChangedDate,
                                  DateTime lastLockedOutDate)
            :
                                  base(providername,
                                       username,
                                       providerUserKey,
                                       email,
                                       passwordQuestion,
                                       comment,
                                       isApproved,
                                       isLockedOut,
                                       creationDate,
                                       lastLoginDate,
                                       lastActivityDate,
                                       lastPasswordChangedDate,
                                       lastLockedOutDate)
        {

        }

        public VivinaMembershipUser(DataEntities.User user)
            : base(
                System.Web.Security.Membership.Provider.Name,
                user.UserName,
                user.UserId,
                user.Email,
                user.PasswordQuestion,
                "",
                user.IsActive,
                user.IsLockedOut,
                user.CreationDate,
                user.LastLoginDate,
                DateTime.Now,
                user.LastPasswordChangedDate,
                user.LastLockoutDate)
        {
            _hasChangePassword = user.HasChangePassword;
        }


        public int UserId
        {
            get
            {
                return Convert.ToInt32(this.ProviderUserKey);
            }
        }


        public bool HasChangePassword
        {
            get
            {
                return _hasChangePassword;
            }
        }
        private bool _hasChangePassword;




    }
}