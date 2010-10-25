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
    public class AccountingController : DataServiceBase
    {
        [JavaScriptSerializer]
        public ActionResult GetCashFlowByYear(Hashtable Params, Hashtable FormData)
        {

            using (var manager = new AccountManager(null))
                return ClientResponse(() => manager.GetCashFlowByYear(Company.CompanyId,
                                                                      Convert.ToInt32(Params["accountPlanId"]),
                                                                      Convert.ToInt32(Params["year"])));
        }
    }
}

