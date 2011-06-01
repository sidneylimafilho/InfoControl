using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Data.Linq;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.AccessControl;
using System.Text;
using System.Web;
using System.Web.Security;
using InfoControl.Data;
using InfoControl.Runtime;
using InfoControl.Security.Cryptography;
using InfoControl.Web.Configuration;
using InfoControl.Web.Security.DataEntities;

namespace InfoControl.Web.Security
{
    public class MembershipManager : BusinessManager<MembershipDataContext>
    {
        public MembershipManager(IDataAccessor acessor)
            : base(acessor) { }

        /// <summary>
        /// This method retrieves all Users. 
        /// </summary>
        /// <returns></returns>
        public IQueryable<User> GetAllUsers()
        {
            return DbContext.Users;
        }

        /// <summary>
        /// Decrypts or leaves the password clear based on the password format
        /// </summary>
        /// <param name="encodedPassword"></param>
        /// <returns></returns>
        internal string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (Membership.Provider.PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password = password.Decrypt(WebConfig.Web.MachineKey.DecryptionKey);
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }

        /// <summary>
        /// Encrypts, Hashes or leaves the password clear based on the password format
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        internal string EncodePassword(string password)
        {
            password = password ?? "123456";
            switch (Membership.Provider.PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;

                case MembershipPasswordFormat.Encrypted:
                    password = password.Encrypt(Algorithm.TripleDES, WebConfig.Web.MachineKey.ValidationKey, EncodingType.BinHex);
                    break;

                case MembershipPasswordFormat.Hashed:
                    password = password.Encrypt(Algorithm.SHA1, WebConfig.Web.MachineKey.ValidationKey, EncodingType.BinHex);
                    break;

                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public string GetPassword(string username, string answer)
        {
            if (Membership.Provider.PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw new ProviderException("Cannot retrieve Hashed passwords.");
            }

            string password;

            User user = GetUserByName(username);
            if (user != null)
            {
                if (user.IsLockedOut)
                    throw new MembershipPasswordException("The supplied user is locked out.");

                password = user.Password;
            }
            else
                throw new MembershipPasswordException("The supplied user name is not found.");

            if (Membership.Provider.PasswordFormat == MembershipPasswordFormat.Encrypted)
                password = password.Decrypt(WebConfig.Web.MachineKey.DecryptionKey);

            return password;
        }

        // This method gets record counts of all Users.
        // Do not change this method.
        public int GetAllUsersCount()
        {
            return GetAllUsers().Count();
        }

        // This method retrieves a single User.
        // Change this method to alter how that record is received.
        public User GetUser(Int32 UserId)
        {
            return DbContext.Users.Where(x => x.UserId == UserId).FirstOrDefault();
        }

        // This method retrieves a single User.
        // Change this method to alter how that record is received.
        public User GetUserByName(string userName)
        {
            return DbContext.Users.Where(x => x.UserName == userName).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves a single User.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User GetUserByEmail(string email)
        {
            return DbContext.Users.Where(x => x.Email == email).FirstOrDefault();
        }

        // This method pages and sorts over all Users.
        // Do not change this method.
        public IQueryable<User> GetAllUsers(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetAllUsers().SortAndPage(sortExpression, startRowIndex, maximumRows, "UserId");
        }

        /// <summary>
        /// This method retrieves all the permissions for user
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, AccessControlActions> GetPermissions(Int32 userId)
        {
            DataManager.Parameters.Add("@UserId", userId);
            DataReader dataReader = DataManager.ExecuteReader(@"
                    SELECT     
                        Functions.CodeName, MAX(PermissionType.PermissionTypeId) AS Expr1
                    FROM         
                        Functions INNER JOIN
                        Permissions ON Functions.FunctionId = Permissions.FunctionId INNER JOIN
                        PermissionType ON Permissions.PermissionTypeId = PermissionType.PermissionTypeId INNER JOIN
                        UsersInRoles ON Permissions.RoleId = UsersInRoles.RoleId 
                    WHERE     
                        UsersInRoles.UserId = @UserId AND 
                        UsersInRoles.CompanyID IN (
	                        SELECT TOP 1 CompanyUser.CompanyID
	                        FROM CompanyUser 
	                        WHERE CompanyUser.UserId = UsersInRoles.UserId  AND CompanyUser.IsMain = 1)
                    GROUP BY 
                        Functions.CodeName");
            var list = new Dictionary<string, AccessControlActions>();
            while (dataReader.Read())
            {
                list.Add(dataReader.GetString(0), (AccessControlActions)dataReader.GetInt32(1));
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetRoles(Int32 userId)
        {
            DataManager.Parameters.Add("@UserId", userId);
            string query = @"
                SELECT Roles.Name
                FROM Roles INNER JOIN 
                     UsersInRoles ON Roles.RoleId = UsersInRoles.RoleId 
                WHERE UsersInRoles.UserId = @UserId AND UsersInRoles.CompanyID IN (
	                SELECT TOP 1 CompanyUser.CompanyID
	                FROM CompanyUser 
	                WHERE CompanyUser.UserId = UsersInRoles.UserId  AND CompanyUser.IsMain = 1)";

            var list = new List<string>();
            DataReader reader = DataManager.ExecuteReader(query);
            while (reader.Read())
                list.Add(Convert.ToString(reader[0]));

            return list;
        }

        // This method deletes a record in the table.
        // Change this method to alter how records are deleted.
        public int Delete(User x)
        {
            DbContext.Users.DeleteOnSubmit(x);
            DbContext.SubmitChanges();
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        public void Delete(string userName)
        {
            DbContext.Users.DeleteAllOnSubmit(DbContext.Users.Where(u => u.UserName == userName));
            DbContext.SubmitChanges();
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
        public MembershipUser Insert(User user, out MembershipCreateStatus status, bool requireValidEmail)
        {
            User usr;

            usr = GetUserByEmail(user.Email);
            if (Membership.Provider.RequiresUniqueEmail && usr != null)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            usr = GetUserByName(user.UserName);
            if (usr != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            if (user.Password.Length < Membership.MinRequiredPasswordLength)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            user.Password = EncodePassword(user.Password);
            user.PasswordAnswer = EncodePassword(user.PasswordAnswer);
            user.IsActive = true;
            user.IsLockedOut = false;
            user.CreationDate = DateTime.Now;
            user.LastPasswordChangedDate = DateTime.Now;
            user.LastLoginDate = DateTime.Now;
            user.LastLockoutDate = DateTime.Now;
            user.LastActivityDate = DateTime.Now;
            user.FailedPasswordAttemptCount = 0;
            user.HasChangePassword = false;

            try
            {
                DbContext.Users.InsertOnSubmit(user);
                DbContext.SubmitChanges();

                if (requireValidEmail)
                {
                    var mail = new MailMessage(WebConfig.Web.Smtp.From, user.Email);
                    mail.IsBodyHtml = true;
                    mail.Headers.Add("Disposition-Notification-To", WebConfig.Web.Smtp.From);

                    mail.Subject = "Confirmação de Cadastro no InfoControl!";
                    mail.Body = user.Profile != null
                                    ? user.Profile.Name + @", seu"
                                    : "Seu";
                    mail.Body += @" cadastro foi recebido com sucesso, para sua maior segurança, 
                                sua conta precisa ser ativada para entrar em vigor.  Acesse o site do <b>InfoControl</b> e
                                clique em <b>ativação de cadastro</b> e informe o seguinte código: <b>" + user.UserId.EncryptToHex() + "</b>";
                    mail.BodyEncoding = Encoding.UTF8;

                    var smtp = new SmtpClient();
                    smtp.EnableSsl = (WebConfig.Web.Smtp.Network.Port == 587);
                    smtp.Credentials = new NetworkCredential(WebConfig.Web.Smtp.Network.UserName, WebConfig.Web.Smtp.Network.Password);
                    smtp.Send(mail);
                }
            }
            catch (Exception)
            {
                DataManager.Rollback();
                throw;
            }

            status = MembershipCreateStatus.Success;

            return new VivinaMembershipUser(GetUserByName(user.UserName));
        }

        // This method updates a record in the table.
        // Change this method to alter how records are updated.
        public int Update(User original_x, User x)
        {
            //original_x = DbContext.Users.GetOriginalEntityState(original_x);
            //DbContext.Users.Attach(original_x);
            if (!String.IsNullOrEmpty(x.Password))
                original_x.Password = EncodePassword(x.Password);
            original_x.UserName = x.UserName;
            original_x.Email = x.Email;
            original_x.LastLoginDate = x.LastLoginDate;
            original_x.LastPasswordChangedDate = x.LastPasswordChangedDate;
            original_x.LastLockoutDate = x.LastLockoutDate;
            original_x.CreationDate = x.CreationDate;
            original_x.FailedPasswordAttemptCount = x.FailedPasswordAttemptCount;
            original_x.IsLockedOut = x.IsLockedOut;
            original_x.IsActive = x.IsActive;
            original_x.HasChangePassword = x.HasChangePassword;
            original_x.PasswordQuestion = x.PasswordQuestion;
            if (!String.IsNullOrEmpty(x.PasswordAnswer))
                original_x.PasswordAnswer = EncodePassword(x.PasswordAnswer);
            DbContext.SubmitChanges();
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        internal void UpdateMembershipUser(MembershipUser user)
        {
            User u = DbContext.Users.Where(us => us.UserName == user.UserName).FirstOrDefault();
            u.Email = user.Email;
            u.UserName = user.UserName;
            u.CreationDate = user.CreationDate;
            u.LastActivityDate = user.LastActivityDate;
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public bool ChangePassword(string userName, string newPwd)
        {
            //
            // Updates the user password
            //

            User u = DbContext.Users.Where(x => x.UserName == userName).FirstOrDefault();
            u.Password = EncodePassword(newPwd);
            u.LastPasswordChangedDate = DateTime.Now;
            u.HasChangePassword = false;
            DbContext.SubmitChanges();
            return true;
        }

        /// <summary>Resets a user's password to a new, automatically generated password.</summary>
        /// <returns>The new password for the specified user.</returns>
        /// <param name="username">The user to reset the password for. </param>
        /// <param name="answer">The password answer for the specified user. </param>
        public string ResetPassword(string username, string answer)
        {
            if (!Membership.EnablePasswordReset)
            {
                throw new NotSupportedException("Password reset is not enabled.");
            }

            if (answer == null && Membership.RequiresQuestionAndAnswer)
            {
                UpdateFailureCount(username);

                throw new ProviderException("Password answer required for password reset.");
            }

            string passwordAnswer;
            string newPassword = Membership.GeneratePassword(Membership.MinRequiredPasswordLength, Membership.MinRequiredNonAlphanumericCharacters);

            User user = GetUserByName(username);
            if (user != null)
            {
                if (user.IsLockedOut)
                    throw new MembershipPasswordException("The supplied user is locked out.");

                passwordAnswer = user.PasswordAnswer;
            }
            else
            {
                throw new MembershipPasswordException("The supplied user name is not found.");
            }

            if (Membership.RequiresQuestionAndAnswer && (EncodePassword(answer) == passwordAnswer))
            {
                UpdateFailureCount(username);

                throw new MembershipPasswordException("Incorrect password answer.");
            }

            user.Password = EncodePassword(newPassword);
            user.LastPasswordChangedDate = DateTime.Now;
            DbContext.SubmitChanges();
            return newPassword;
        }

        /// <summary>
        /// Change the Q & A 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="newPwdQuestion"></param>
        /// <param name="newPwdAnswer"></param>
        /// <returns></returns>
        public bool ChangePasswordQuestionAndAnswer(string username,
                                                    string newPwdQuestion,
                                                    string newPwdAnswer)
        {
            User u = DbContext.Users.Where(x => x.UserName == username).FirstOrDefault();
            u.PasswordQuestion = newPwdQuestion;
            u.PasswordAnswer = EncodePassword(newPwdAnswer);
            DbContext.SubmitChanges();
            return true;
        }

        public bool UnlockUser(string username)
        {
            User user = GetUserByName(username);
            user.IsLockedOut = false;
            user.LastLockoutDate = DateTime.Now;
            user.FailedPasswordAttemptCount = 0;
            DbContext.SubmitChanges();
            return true;
        }

        public void UpdateFailureCount(string username)
        {
            User user = GetUserByName(username);

            int failureCount = user.FailedPasswordAttemptCount;

            if (failureCount++ >= Membership.MaxInvalidPasswordAttempts)
            {
                user.IsLockedOut = true;
                user.LastLockoutDate = DateTime.Now;
            }
            else
            {
                user.FailedPasswordAttemptCount = failureCount;
            }

            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user</param>
        /// <returns></returns>
        public bool ValidateUser(string username, string password, bool processLogin)
        {
            User user = GetUserByName(username);
            if (user != null)
                if (user.Password == EncodePassword(password) || user.PasswordAnswer == EncodePassword(password))
                {
                    if (!user.IsLockedOut || user.IsActive)
                    {
                        if (!processLogin)
                            return user.IsActive;

                        //
                        // Case the solution in javascript dont work, then uses above
                        //
                        //Int32 onlineTime = System.Web.Security.Membership.UserIsOnlineTimeWindow;
                        //if (!user.IsOnline || user.LastActivityDate < DateTime.Now.AddMinutes(-onlineTime))
                        string currentHost = (HttpContext.Current.Request.Cookies[WebConfig.Web.AnonymousIdentification.CookieName] ?? new HttpCookie("")).Value;
                        bool isSameTerminal = (user.LastRemoteHost == null || user.LastRemoteHost == currentHost);
                        bool timeoutExpired = DateTime.Now.Subtract(user.LastActivityDate).TotalMinutes > Membership.UserIsOnlineTimeWindow;
                        //if (!user.IsOnline || isSameTerminal || timeoutExpired)
                        //{
                        user.LastLoginDate = DateTime.Now;
                        user.LastRemoteHost = currentHost;
                        user.IsOnline = true;
                        user.FailedPasswordAttemptCount = 0;
                        DbContext.SubmitChanges();
                        return true;
                        //}
                    }
                }
                else
                {
                    UpdateFailureCount(username);
                }

            return false;
        }

        public void Logoff(AccessControlPrincipal principal)
        {
            if (principal != null && principal.Identity != null)
            {
                var user = GetUser(principal.Identity.UserId);

                if (user != null)
                {
                    //
                    // Trace the User Activity
                    //
                    DbContext.UserActivityLogs.InsertOnSubmit(new UserActivityLog()
                    {
                        LoginDate = user.LastLoginDate,
                        LogoffDate = DateTime.Now,
                        UserId = user.UserId
                    });
                    DbContext.SubmitChanges();



                    user.IsOnline = false;
                    user.LastRemoteHost = null;
                    user.LastActivityDate = DateTime.Now;
                    user.LastLoginDate = DateTime.Now;
                    user.PersonalizationRaw = principal.Personalization.SerializeToString();
                    DbContext.SubmitChanges();
                }
            }
        }
    }

    public class FunctionManager : BusinessManager<MembershipDataContext>
    {
        public FunctionManager(IDataAccessor acessor)
            : base(acessor) { }

        // This method retrieves all Functions.
        // Change this method to alter how records are retrieved.
        public IQueryable<Function> GetAllFunctions()
        {
            return DbContext.Functions;
        }

        // This method gets record counts of all Functions.
        // Do not change this method.
        public int GetAllFunctionsCount()
        {
            return GetAllFunctions().Count();
        }

        // This method retrieves a single Function.
        // Change this method to alter how that record is received.
        public Function GetFunction(Int32 FunctionId)
        {
            return DbContext.Functions.Where(x => x.FunctionId == FunctionId).FirstOrDefault();
        }

        // This method retrieves a single Function.
        // Change this method to alter how that record is received.
        public Function GetFunction(string name)
        {
            return DbContext.Functions.Where(x => x.Name == name).FirstOrDefault();
        }

        // This method retrieves a single Function.
        // Change this method to alter how that record is received.
        public Function GetFunctionByCodeName(string codeName)
        {
            return DbContext.Functions.Where(x => x.CodeName == codeName).FirstOrDefault();
        }

        // This method pages and sorts over all Functions.
        // Do not change this method.
        public IQueryable<Function> GetAllFunctions(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetAllFunctions().SortAndPage(sortExpression, startRowIndex, maximumRows, "FunctionId");
        }

        // This method deletes a record in the table.
        // Change this method to alter how records are deleted.
        public int Delete(Function x)
        {
            DbContext.Functions.DeleteAllOnSubmit(DbContext.Functions.Where(f => f.FunctionId == x.FunctionId));
            DbContext.SubmitChanges();
            return 1;
        }

        // This method inserts a new record in the table.
        // Change this method to alter how records are inserted.
        public int Insert(Function entity)
        {
            DbContext.Functions.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
            return 1;
        }

        /// <summary>
        /// Basic Insert Method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isPublic"></param>
        //public void Insert(Function entity, bool isPublic)
        //{
        //    DbContext.Functions.InsertOnSubmit(entity);
        //    DbContext.SubmitChanges();
        //    if (isPublic)
        //    {
        //        RolesManager rManager = new RolesManager(this);
        //        List<Role> roles = rManager.GetRolesByName("Admin").ToList();
        //        foreach (Role role in roles)
        //        {
        //            Permission permission = new Permission();
        //            permission.CompanyId = role.CompanyId;
        //            permission.FunctionId = entity.FunctionId;
        //            permission.PermissionTypeId = (int)System.Security.AccessControl.AccessControlActions.Change;
        //            permission.RoleId = role.RoleId;
        //            new PermissionsManager(this).Insert(permission);
        //        }
        //    }
        //    DbContext.SubmitChanges();
        //}
        // This method updates a record in the table.
        // Change this method to alter how records are updated.
        public int Update(Function original_x, Function x)
        {
            DbContext.Functions.Attach(original_x);
            original_x.Name = x.Name;
            original_x.Description = x.Description;
            DbContext.SubmitChanges();
            return 1;
        }
    }

    public class RolesManager : BusinessManager<MembershipDataContext>
    {
        public RolesManager(IDataAccessor acessor)
            : base(acessor) { }

        // This method retrieves all Roles.
        // Change this method to alter how records are retrieved.
        public IQueryable<Role> GetAllRoles()
        {
            return DbContext.Roles;
        }

        // This method gets record counts of all Roles.
        // Do not change this method.
        public int GetAllRolesCount()
        {
            return GetAllRoles().Count();
        }

        // This method retrieves a single Roles.
        // Change this method to alter how that record is received.
        public Role GetRole(Int32 RoleId)
        {
            return DbContext.Roles.Where(x => x.RoleId == RoleId).FirstOrDefault();
        }

        // This method retrieves a single Roles.
        // Change this method to alter how that record is received.
        public Role GetRole(string roleName)
        {
            return GetRolesByName(roleName).FirstOrDefault();
        }

        public IQueryable<Role> GetRolesByName(string roleName)
        {
            return DbContext.Roles.Where(x => x.Name.Contains(roleName));
        }

        /// <summary>
        /// This method retrieves all the permissions for user
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, AccessControlActions> GetPermissions(Int32 roleId)
        {
            DataManager.Parameters.Add("@RoleId", roleId);
            DataReader dataReader = DataManager.ExecuteReader(@"
                    SELECT     
                        Functions.Name, MAX(ISNULL(PermissionType.PermissionTypeId, 0)) AS Expr1
                    FROM         
                        Functions LEFT OUTER JOIN
                        Permissions ON Functions.FunctionId = Permissions.FunctionId LEFT OUTER JOIN
                        PermissionType ON Permissions.PermissionTypeId = PermissionType.PermissionTypeId 
                    WHERE     
                        (Permissions.RoleId = @RoleId)
                    GROUP BY 
                        Functions.Name");
            var list = new Dictionary<string, AccessControlActions>();
            while (dataReader.Read())
            {
                list.Add(dataReader.GetString(0), (AccessControlActions)dataReader.GetInt32(1));
            }
            return list;
        }

        /// <summary>
        /// This method retrieves all the permissions for user
        /// </summary>
        /// <returns></returns>
        public DataReader GetAllFunctionsWithPermissions(Int32 roleId)
        {
            DataManager.Parameters.Add("@RoleId", roleId);
            return DataManager.ExecuteReader(@"
                SELECT     
                    Functions.FunctionId, Functions.Name, ISNULL(Perm.PermissionTypeId, 0) AS PermissionTypeId
                FROM         
                    Functions LEFT OUTER JOIN
                      (SELECT     FunctionId, PermissionTypeId, RoleId
                        FROM          Permissions
                        WHERE      (RoleId = @RoleId)) AS Perm ON Functions.FunctionId = Perm.FunctionId
                ORDER BY 
                    Functions.Name
            ");
        }

        // This method pages and sorts over all Roles.
        // Do not change this method.
        public IQueryable<Role> GetAllRoles(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetAllRoles().SortAndPage(sortExpression, startRowIndex, maximumRows, "RoleId");
        }

        // This method deletes a record in the table.
        // Change this method to alter how records are deleted.
        public int Delete(Role x)
        {
            DbContext.Roles.Attach(x);
            DbContext.Roles.DeleteOnSubmit(x);
            DbContext.SubmitChanges();
            return 1;
        }

        // This method inserts a new record in the table.
        // Change this method to alter how records are inserted.
        public int Insert(Role x)
        {
            DbContext.Roles.InsertOnSubmit(x);
            DbContext.SubmitChanges();
            return x.RoleId;
        }

        // This method updates a record in the table.
        // Change this method to alter how records are updated.
        public int Update(Role original_x, Role x)
        {
            DbContext.Roles.Attach(original_x);
            original_x.ParentRoleId = x.ParentRoleId;
            original_x.ApplicationId = x.ApplicationId;
            original_x.LastUpdatedDate = x.LastUpdatedDate;
            original_x.Name = x.Name;
            original_x.Description = x.Description;
            DbContext.SubmitChanges();
            return 1;
        }
    }

    public class UsersInRolesManager : BusinessManager<MembershipDataContext>
    {
        public UsersInRolesManager(IDataAccessor acessor)
            : base(acessor) { }

        // This method retrieves all UsersInRoles.
        // Change this method to alter how records are retrieved.
        public Table<UsersInRole> GetAllUsersInRoles()
        {
            return DbContext.UsersInRoles;
        }

        // This method gets record counts of all UsersInRoles.
        // Do not change this method.
        public int GetAllUsersInRolesCount()
        {
            return GetAllUsersInRoles().Count();
        }

        // This method retrieves a single UsersInRoles.
        // Change this method to alter how that record is received.
        public UsersInRole GetUsersInRoles(Int32 RoleId, Int32 UserId)
        {
            return DbContext.UsersInRoles.Where(x => x.RoleId == RoleId && x.UserId == UserId).FirstOrDefault();
        }

        // This method deletes a record in the table.
        // Change this method to alter how records are deleted.
        public int Delete(UsersInRole x)
        {
            DbContext.UsersInRoles.Attach(x);
            DbContext.UsersInRoles.DeleteOnSubmit(x);
            DbContext.SubmitChanges();
            return 1;
        }

        // This method inserts a new record in the table.
        // Change this method to alter how records are inserted.
        public int Insert(UsersInRole x)
        {
            DbContext.UsersInRoles.InsertOnSubmit(x);
            DbContext.SubmitChanges();
            return 1;
        }

        // This method updates a record in the table.
        // Change this method to alter how records are updated.
        public int Update(UsersInRole original_x, UsersInRole x)
        {
            DbContext.UsersInRoles.Attach(original_x);
            DbContext.SubmitChanges();
            return 1;
        }
    }

    public class PermissionsManager : BusinessManager<MembershipDataContext>
    {
        public PermissionsManager(IDataAccessor acessor)
            : base(acessor) { }

        // This method retrieves all UsersOnlines.
        // Change this method to alter how records are retrieved.
        public IQueryable<Permission> GetAllPermissions()
        {
            return DbContext.Permissions;
        }

        // This method gets record counts of all UsersOnlines.
        // Do not change this method.
        public int GetAllPermissionsCount()
        {
            return GetAllPermissions().Count();
        }

        // This method retrieves a single UsersOnline.
        // Change this method to alter how that record is received.
        public Permission GetPermission(Int32 roleId, Int16 functionId, Int32 permissionTypeId)
        {
            return DbContext.Permissions.Where(x => x.RoleId == roleId && x.FunctionId == functionId && x.PermissionTypeId == permissionTypeId).FirstOrDefault();
        }

        // This method deletes a record in the table.
        // Change this method to alter how records are deleted.
        public int Delete(Permission x)
        {
            DbContext.Permissions.Attach(x);
            DbContext.Permissions.DeleteOnSubmit(x);
            DbContext.SubmitChanges();
            return 1;
        }

        // This method inserts a new record in the table.
        // Change this method to alter how records are inserted.
        public int Insert(Permission x)
        {
            DbContext.Permissions.InsertOnSubmit(x);
            DbContext.SubmitChanges();
            return 1;
        }

        // This method updates a record in the table.
        // Change this method to alter how records are updated.
        public int Update(Permission original_x, Permission x)
        {
            DbContext.Permissions.Attach(original_x);

            DbContext.SubmitChanges();
            return 1;
        }
    }
}