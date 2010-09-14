using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InfoControl;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.WebUI.InfoControl._systemframework.Security
{
    public class InfoControlMembershipProvider : VivinaMembershipProvider
    {

        public override bool ValidateUser(string username, string password, bool checkIsOnline)
        {
            var isValidUser = base.ValidateUser(username, password, checkIsOnline);

            if (!isValidUser)
                return isValidUser;

            using (var companyManager = new CompanyManager(null))
            {
                var user = companyManager.GetUserByUserName(username);

                return user.CompanyUsers.Any() || user.Customers.Any();
            }

        }

    }
}