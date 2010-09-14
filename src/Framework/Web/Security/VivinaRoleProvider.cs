using System;
using System.Data;
using System.Web;

using System.Web.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Configuration.Provider;

using System.Linq;
using System.Data.Linq;

using InfoControl.Web.Configuration;
using InfoControl.Web.Security.DataEntities;

namespace InfoControl.Web.Security
{
    public class VivinaRoleProvider : System.Web.Security.RoleProvider
    {

        private static string connectionString;

        #region Properties
        private string _applicationName;

        public override string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }


        private string _permissionDeniedUrl;

        public string PermissionDeniedUrl
        {
            get { return _permissionDeniedUrl; }
            set { _permissionDeniedUrl = value; }
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

        public override void Initialize(string name, NameValueCollection config)
        {
            //
            // Initialize values from web.config.
            //
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "VivinaRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Vivina Role Provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);


            //
            // Initialize the Application Name
            //
            _applicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);

            //
            // Initialize the Application Name
            //
            _permissionDeniedUrl = GetConfigValue(config["permissionDeniedUrl"], "default.aspx?WithoutPermission");


            //
            // Initialize connectionString.
            //
            if (config["connectionStringName"] == null || config["connectionStringName"].Trim() == "")
            {
                config["connectionStringName"] = WebConfig.DataAccess.ConnectionStringName;
            }

            connectionString = WebConfig.ConnectionStrings[config["connectionStringName"]].ConnectionString;

        }

        #region System.Web.Security.RoleProvider methods.

        public override void AddUsersToRoles(string[] usernames, string[] rolenames)
        {
            throw new NotImplementedException("AddUsersToRoles not implemented.");
            //foreach (string rolename in rolenames)
            //{
            //    if (!RoleExists(rolename))
            //    {
            //        throw new ProviderException("Role name not found.");
            //    }
            //}

            //foreach (string username in usernames)
            //{
            //    if (username.Contains(","))
            //    {
            //        throw new ArgumentException("User names cannot contain commas.");
            //    }

            //    User user = User.GetUser(username);

            //    foreach (string rolename in rolenames)
            //    {
            //        if (IsUserInRole(username, rolename))
            //        {
            //            throw new ProviderException("User is already in role.");
            //        }
            //        else
            //        {
            //            UsersInRoles usersInRoles = new UsersInRoles();
            //            usersInRoles.UserId = user.UserId;
            //            usersInRoles.RoleId = Roles.GetRoles(rolename).RoleId;

            //            MembershipDataContext context = CreateDataContext();
            //            context.UsersInRoles.Add(usersInRoles);
            //            context.SubmitChanges();
            //        }
            //    }
            //}



        }

        public override void CreateRole(string rolename)
        {
            if (rolename.Contains(","))
            {
                throw new ArgumentException("Role names cannot contain commas.");
            }

            if (RoleExists(rolename))
            {
                throw new ProviderException("Role name already exists.");
            }

            Role role = new Role();
            role.Name = rolename;
            role.LastUpdatedDate = DateTime.Now;
            role.ApplicationId = Configuration.Application.Current.ApplicationId;

            MembershipDataContext context = CreateDataContext();
            context.Roles.InsertOnSubmit(role);
            context.SubmitChanges();
        }

        public override bool DeleteRole(string rolename, bool throwOnPopulatedRole)
        {
            if (!RoleExists(rolename))
            {
                throw new ProviderException("Role does not exist.");
            }

            if (throwOnPopulatedRole && GetUsersInRole(rolename).Length > 0)
            {
                throw new ProviderException("Cannot delete a populated role.");
            }

            MembershipDataContext context = CreateDataContext();
            context.Roles.DeleteAllOnSubmit(context.Roles.Where(r => r.Name == rolename));
            context.SubmitChanges();

            return true;
        }

        public override string[] GetAllRoles()
        {
            string tmpRoleNames = "";
            MembershipDataContext context = CreateDataContext();
            foreach (Role role in context.Roles)
            {
                tmpRoleNames += role.Name + ",";
            }

            if (tmpRoleNames.Length > 0)
            {
                // Remove trailing comma.
                tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
                return tmpRoleNames.Split(',');
            }

            return new string[0];
        }

        public override string[] GetRolesForUser(string username)
        {
            MembershipDataContext context = CreateDataContext();

            IQueryable<DataEntities.Role> query = from role in context.Roles
                                                   //join userRoles in context.UsersInRoles on role.RoleId equals userRoles.RoleId
                                                   //join user in context.Users on userRoles.UserId equals user.UserId
                                                   //where user.UserName == username
                                                   select role;

            string roles = "";
            foreach (DataEntities.Role role in query)
            {
                roles += role.Name + ",";
            }

            if (roles.Length > 0)
            {
                roles = roles.Substring(0, roles.Length - 1);
                return (roles.Split(','));
            }

            return new string[0];
        }

        public override string[] GetUsersInRole(string rolename)
        {
            MembershipDataContext context = CreateDataContext();

            IQueryable<User> query = from role in context.Roles
                                     join userRoles in context.UsersInRoles on role.RoleId equals userRoles.RoleId
                                     join user in context.Users on userRoles.UserId equals user.UserId
                                     where role.Name == rolename
                                     select user;

            string users = "";
            foreach (User user in query)
            {
                users += user.UserName + ",";
            }

            if (users.Length > 0)
            {
                users = users.Substring(0, users.Length - 1);
                return (users.Split(','));
            }

            return new string[0];
        }

        public override bool IsUserInRole(string username, string rolename)
        {
            MembershipDataContext context = CreateDataContext();
            IQueryable<User> query = from role in context.Roles
                                     join userRoles in context.UsersInRoles on role.RoleId equals userRoles.RoleId
                                     join user in context.Users on userRoles.UserId equals user.UserId
                                     where role.Name == rolename && user.UserName == username
                                     select user;
            return query.ToList().Count > 0;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] rolenames)
        {
            MembershipDataContext context = CreateDataContext();
            context.ExecuteCommand(@"
                DELETE FROM UsersInRoles
                where exists 
                    (
                    select UsersInRoles.UserId 
                    from UsersInRoles as UR INNER JOIN
	                     Users ON UsersInRoles.UserId = Users.UserId INNER JOIN
                         Roles ON UsersInRoles.RoleId = Roles.RoleId INNER JOIN
	                     UsersInRoles ON UR.UserId = UsersInRoles.UserId AND UR.RoleId = UsersInRoles.RoleId
                    where 
                         Users.UserName IN ('') AND Roles.Name IN ('')
                    )
                ",
                 String.Join(",", usernames),
                 String.Join(",", rolenames));
        }

        public override bool RoleExists(string rolename)
        {
            MembershipDataContext context = CreateDataContext();
            return context.Roles.Where(role => role.Name == rolename).FirstOrDefault() != null;
        }

        public override string[] FindUsersInRole(string rolename, string usernameToMatch)
        {
            return GetUsersInRole(rolename);
        }
        #endregion

        /// <summary>
        /// Create a <see cref="T:InfoControl.Web.Security.MembershipDataContext"/> that implements Data access control
        /// </summary>
        public static DataEntities.MembershipDataContext CreateDataContext()
        {
            //
            // Sets the application name, only for the Membership initialize the connection string
            //
            string app = System.Web.Security.Roles.ApplicationName;
            if (String.IsNullOrEmpty(connectionString))
            {
                throw new ProviderException("VivinaRoleProvider not implemented!");
            }
            return new MembershipDataContext(connectionString);
        }


    }
}
