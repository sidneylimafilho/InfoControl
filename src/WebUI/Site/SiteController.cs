using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using InfoControl.Web.Services;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Comments;
using Vivina.Erp.BusinessRules.Services;
using Vivina.Erp.SystemFramework;

namespace Vivina.Erp.WebUI
{
    public class SiteController : DataServiceBase 
    {
        #region WebPages

        [JavaScriptSerializer]
        public ActionResult GetComments()
        {
            return ClientResponse(() =>
                                  {
                                      using (var manager = new CommentsManager(null))
                                          return manager.GetPageCommentsByCompany(Company.CompanyId).ToArray();
                                  }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region E-Shop
        [JavaScriptSerializer]
        public ActionResult GetServiceOrdersByCustomer(Hashtable Params, Hashtable FormData)
        {
            return ClientResponse(
                () =>
                {
                    var manager = new ServicesManager(this);
                    var customerManager = new CustomerManager(this);

                    var customer = customerManager.GetCustomerByUserName(Company.CompanyId,
                                                                     User.Identity.UserName);
                    return manager.GetServiceOrdersByCustomer(customer.CustomerId, "", 0, Int32.MaxValue);

                });
        }

        #endregion
    }
}