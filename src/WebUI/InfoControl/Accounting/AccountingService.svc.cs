using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using InfoControl;
using InfoControl.Web;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Reports;
using Vivina.Erp.SystemFramework;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using Vivina.Erp.DataClasses;
using System.Web.Mvc;
using InfoControl.Web.Services;
using Vivina.Erp.BusinessRules.Services;

namespace Vivina.Erp.WebUI.InfoControl.Accounting
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AccountingService : DataServiceBase
    {
        [OperationContract, JavaScriptSerializer]
        public ClientResponse GetCashFlowByYear(int? accountPlanId, int year)
        {
            return new ClientResponse(() =>
            {
                using (var manager = new AccountManager(null))
                    return manager.GetCashFlowByYear(Company.CompanyId, accountPlanId, year);
            });
        }
    }
}

