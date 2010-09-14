using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Configuration;
using System.Linq;
using System.Data.Linq;
using System.Data.Odbc;
using System.Data;
using System.Diagnostics;
using System.Globalization;

using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;
using System.Web;

using InfoControl.Data;
using InfoControl.Security;
using InfoControl.Web.Configuration;
using InfoControl.Web.Security.DataEntities;

namespace InfoControl.Web.Security 
{

    public class VivinaMembershipProvider : MembershipProvider
    {
        #region Members
        private static string connectionString;
        #endregion

        #region Properties

        MembershipManager _membershipManager;
        public MembershipManager MembershipManager
        {
            get
            {
                return new MembershipManager(null);                
            }
        }

        /// <summary>
        /// Optional string to identity the application: defaults to Application Metabase path
        /// </summary>
        public override string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }
        private string _applicationName;

        /// <summary>
        /// Should the provider support password resets
        /// </summary>
        public override bool EnablePasswordReset
        {
            get { return _enablePasswordReset; }
        }
        private bool _enablePasswordReset;


        /// <summary>
        /// Should the provider support password retrieve
        /// </summary>
        public override bool EnablePasswordRetrieval
        {
            get { return _enablePasswordRetrieval; }
        }
        private bool _enablePasswordRetrieval;


        /// <summary>
        /// Should the provider require Q & A
        /// </summary>
        public override bool RequiresQuestionAndAnswer
        {
            get { return _requiresQuestionAndAnswer; }
        }
        private bool _requiresQuestionAndAnswer;


        /// <summary>
        /// Should the provider require a unique email to be specified
        /// </summary>
        public override bool RequiresUniqueEmail
        {
            get { return _requiresUniqueEmail; }
        }
        private bool _requiresUniqueEmail;

        /// <summary>
        /// Should the provider require a valid email to be specified
        /// </summary>
        public bool RequiresValidEmail
        {
            get { return _requiresValidEmail; }
        }
        private bool _requiresValidEmail;

        /// <summary>
        /// The maximum number a attempts of the logon, after the user is lockdown
        /// </summary>
        public override int MaxInvalidPasswordAttempts
        {
            get { return _maxInvalidPasswordAttempts; }
        }
        private int _maxInvalidPasswordAttempts;

        public override int PasswordAttemptWindow
        {
            get { return 999999; }
        }

        /// <summary>
        /// Storage format for the password: Hashed (SHA1), Clear or Encrypted (Triple-DES)
        /// </summary>
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return pPasswordFormat; }
        }
        private MembershipPasswordFormat pPasswordFormat;

        /// <summary>
        /// The minimum number of non-alphanumeric characters
        /// </summary>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _minRequiredNonAlphanumericCharacters; }
        }
        private int _minRequiredNonAlphanumericCharacters;

        /// <summary>
        /// The minimum password length
        /// </summary>
        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }
        private int _minRequiredPasswordLength;

        public override string PasswordStrengthRegularExpression
        {
            get { return ""; }
        }

        #endregion

        #region Support Methods

        /// <summary>
        /// A helper function to retrieve config values fro the configuration file
        /// </summary>
        /// <param name="configValue">A value of the configuration key</param>
        /// <param name="defaultValue">A default value case the configValue is null or empty</param>
        /// <returns></returns>
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        #endregion

        #region Override Methods


        /// <summary>
        /// Initialize the provider
        /// </summary>
        /// <param name="name">The friendly name of the provider</param>
        /// <param name="config">
        /// A collection of the name/value pairs representing the provider-specific 
        /// attributes specified in the configuration for this provider
        /// </param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "VivinaMembershipProvider";


            // Initialize the abstract base class.
            base.Initialize(name, config);


            _applicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);

            _maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));

            _minRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));

            _minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));

            _enablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));

            _enablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));

            _requiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));

            _requiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));

            _requiresValidEmail = Convert.ToBoolean(GetConfigValue(config["requiresValidEmail"], "true"));


            switch (GetConfigValue(config["passwordFormat"], "Encrypted").ToLower())
            {
                case "hashed":
                    pPasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "encrypted":
                    pPasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "clear":
                    pPasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            //
            // Initialize connectionString.
            //
            if (config["connectionStringName"] == null || config["connectionStringName"].Trim() == "")
            {
                config["connectionStringName"] = WebConfig.DataAccess.ConnectionStringName;
            }

            connectionString = WebConfig.ConnectionStrings[config["connectionStringName"]].ConnectionString;

            if (WebConfig.Web.MachineKey.ValidationKey.Contains("AutoGenerate"))
                if (PasswordFormat != MembershipPasswordFormat.Clear)
                    throw new ProviderException("Hashed or Encrypted passwords " +
                                                "are not supported with auto-generated keys.");
        }

        /// <summary>
        /// Change the password 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public override bool ChangePassword(string username, string oldPwd, string newPwd)
        {
            //
            // Validate user to permits change the password
            //
            if (!ValidateUser(username, oldPwd, false))
                return false;

            //
            // Fires event validating password
            //
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPwd, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Change password canceled due to new password validation failure.");


            //
            // Updates the user password
            //
            return MembershipManager.ChangePassword(username, newPwd);
        }

        /// <summary>
        /// Change the Q & A 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="newPwdQuestion"></param>
        /// <param name="newPwdAnswer"></param>
        /// <returns></returns>
        public override bool ChangePasswordQuestionAndAnswer(string username,
                      string password,
                      string newPwdQuestion,
                      string newPwdAnswer)
        {
            if (!ValidateUser(username, password, false))
                return false;

            //
            // Updates the user password
            //
            return MembershipManager.ChangePasswordQuestionAndAnswer(username, newPwdQuestion, newPwdAnswer);

        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns></returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            MembershipManager.Delete(username);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public override string GetUserNameByEmail(string email)
        {
            return MembershipManager.GetUserByEmail(email).UserName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            User user = MembershipManager.GetUserByName(username);
            return user == null ? null : new VivinaMembershipUser(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerUserKey"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            User user = MembershipManager.GetUser(Convert.ToInt32(providerUserKey));
            return user == null ? null : new VivinaMembershipUser(user);
        }

        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="providerUserKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override MembershipUser CreateUser(string username,
                 string password,
                 string email,
                 string passwordQuestion,
                 string passwordAnswer,
                 bool isApproved,
                 object providerUserKey,
                 out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);

            User user = new User();
            user.UserName = username;
            user.Password = password;
            user.Email = email;
            user.PasswordQuestion = passwordQuestion;
            user.PasswordAnswer = passwordAnswer;

            return MembershipManager.Insert(user, out status, _requiresValidEmail);
        }

        /// <summary>
        /// Get number of the users is online
        /// </summary>
        /// <returns></returns>
        public override int GetNumberOfUsersOnline()
        {
            return MembershipManager.GetAllUsers().Where(usr => usr.IsOnline == true).Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval)
            {
                throw new ProviderException("Password Retrieval Not Enabled.");
            }

            if (PasswordFormat == System.Web.Security.MembershipPasswordFormat.Hashed)
            {
                return ResetPassword(username, answer);
            }

            return MembershipManager.GetPassword(username, answer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public override bool UnlockUser(string username)
        {
            return MembershipManager.UnlockUser(username);
        }



        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user</param>
        /// <returns></returns>
        public override bool ValidateUser(string username, string password)
        {
            return ValidateUser(username, password, true);
        }

        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user</param>
        /// <returns></returns>
        public virtual bool ValidateUser(string username, string password, bool checkIsOnline)
        {
            return MembershipManager.ValidateUser(username, password, checkIsOnline);
        }



        /// <summary>Resets a user's password to a new, automatically generated password.</summary>
        /// <returns>The new password for the specified user.</returns>
        /// <param name="username">The user to reset the password for. </param>
        /// <param name="answer">The password answer for the specified user. </param>
        public override string ResetPassword(string username, string answer)
        {

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, answer, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Reset password canceled due to password validation failure.");

            return MembershipManager.ResetPassword(username, answer);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public override void UpdateUser(MembershipUser user)
        {
            MembershipManager.UpdateMembershipUser(user);
        }


        /// <summary>Gets a collection of all the users in the SQL Server membership database.</summary>
        /// <returns>A <see cref="T:System.Web.Security.MembershipUserCollection"></see> of <see cref="T:System.Web.Security.MembershipUser"></see> objects representing all the users in the database for the configured <see cref="P:System.Web.Security.SqlMembershipProvider.ApplicationName"></see>.</returns>
        /// <param name="totalRecords">The total number of users.</param>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <exception cref="T:System.ArgumentException">pageIndex is less than zero.- or -pageSize is less than one.- or -pageIndex multiplied by pageSize plus pageSize minus one exceeds <see cref="F:System.Int32.MaxValue"></see>.</exception>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            //
            // Calculate the first and last records
            //
            int firstUserIndex = pageIndex * pageSize;
            int lastUserIndex = ((pageIndex * pageSize) + pageSize) - 1;

            if (pageIndex < 0)
            {
                throw new ArgumentException(Properties.Resources.PageIndex_bad, "pageIndex");
            }
            if (pageSize < 1)
            {
                throw new ArgumentException(Properties.Resources.PageSize_bad, "pageSize");
            }
            if (lastUserIndex > 0x7fffffff)
            {
                throw new ArgumentException(Properties.Resources.PageIndex_PageSize_bad, "pageIndex and pageSize");
            }


            //
            // Retrieve a users collection
            //
            System.Collections.Generic.IList<User> users = MembershipManager.GetAllUsers().ToList();
            totalRecords = users.Count;

            lastUserIndex = Math.Min(totalRecords, lastUserIndex);

            //
            // Enumerate the Membership.User and convert to MembershipUserCollection
            //
            MembershipUserCollection list = new MembershipUserCollection();
            for (int idx = firstUserIndex; idx < lastUserIndex; idx++)
            {
                list.Add(new VivinaMembershipUser(users[idx]));
            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="usernameToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {

            //
            // Calculate the first and last records
            //
            int firstUserIndex = pageIndex * pageSize;
            int lastUserIndex = ((pageIndex * pageSize) + pageSize) - 1;

            if (pageIndex < 0)
            {
                throw new ArgumentException(Properties.Resources.PageIndex_bad, "pageIndex");
            }
            if (pageSize < 1)
            {
                throw new ArgumentException(Properties.Resources.PageSize_bad, "pageSize");
            }
            if (lastUserIndex > 0x7fffffff)
            {
                throw new ArgumentException(Properties.Resources.PageIndex_PageSize_bad, "pageIndex and pageSize");
            }

            //
            // Retrieve a users collection
            //
            MembershipDataContext membership = new MembershipDataContext(connectionString);
            System.Collections.Generic.IList<User> users = membership.Users.Where(us => us.UserName == usernameToMatch).ToList();
            totalRecords = users.Count;

            //
            // Enumerate the Membership.User and convert to MembershipUserCollection
            //
            MembershipUserCollection list = new MembershipUserCollection();
            for (int idx = firstUserIndex; idx <= lastUserIndex; idx++)
            {
                list.Add(new VivinaMembershipUser(users[idx]));
            }

            return list;
        }


        /// <summary>Returns a collection of membership users for which the e-mail address field contains the specified e-mail address.</summary>
        /// <returns>A <see cref="T:System.Web.Security.MembershipUserCollection"></see> that contains a page of pageSize<see cref="T:System.Web.Security.MembershipUser"></see> objects beginning at the page specified by pageIndex.</returns>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="emailToMatch">The e-mail address to search for.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <exception cref="T:System.ArgumentException">emailToMatch is longer than 256 characters.- or -pageIndex is less than zero.- or -pageSize is less than one.- or -pageIndex multiplied by pageSize plus pageSize minus one exceeds <see cref="F:System.Int32.MaxValue"></see>.</exception>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {

            //
            // Calculate the first and last records
            //
            int firstUserIndex = pageIndex * pageSize;
            int lastUserIndex = ((pageIndex * pageSize) + pageSize) - 1;

            if (pageIndex < 0)
            {
                throw new ArgumentException(Properties.Resources.PageIndex_bad, "pageIndex");
            }
            if (pageSize < 1)
            {
                throw new ArgumentException(Properties.Resources.PageSize_bad, "pageSize");
            }
            if (lastUserIndex > 0x7fffffff)
            {
                throw new ArgumentException(Properties.Resources.PageIndex_PageSize_bad, "pageIndex and pageSize");
            }

            //
            // Retrieve a users collection
            //
            MembershipDataContext membership = new MembershipDataContext(connectionString);
            System.Collections.Generic.IList<User> users = membership.Users.Where(us => us.Email == emailToMatch).ToList();
            totalRecords = users.Count;

            //
            // Enumerate the Membership.User and convert to MembershipUserCollection
            //
            MembershipUserCollection list = new MembershipUserCollection();
            for (int idx = firstUserIndex; idx <= lastUserIndex; idx++)
            {
                list.Add(new VivinaMembershipUser(users[idx]));
            }

            return list;
        }
        #endregion

        #region IDataAccessor Members

        public DataManager DataManager
        {
            get { return new DataManager(false); }
        }

        #endregion
    }
}