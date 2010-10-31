using System;
using System.Web;
using InfoControl.Web.Auditing;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using User = InfoControl.Web.Security.DataEntities.User;

namespace Vivina.Erp.SystemFramework.Auditing
{
    public class CustomerCallNotifier : ExceptionManager
    {
        public override InfoControl.Web.Auditing.Event LogErrorInDatabase(Exception ex)
        {

            var eventEntity = base.LogErrorInDatabase(ex);
            HttpContext context = HttpContext.Current;

            if (context != null && context.Session != null)
                using (var customerManager = new CustomerManager(null))
                using (var companyManager = new CompanyManager(null))
                using (var membershipManager = new MembershipManager(null))
                {
                    Company hostCompany = companyManager.GetHostCompany();
                    Company company = companyManager.GetCompanyByContext(context);

                    Customer customer = customerManager.GetCustomerByLegalEntityProfile(hostCompany.CompanyId, company.LegalEntityProfileId);

                    if (customer != null)
                    {
                        var customerCall = new CustomerCall();
                        customerCall.EventId = eventEntity.EventId;

                        customerCall.Subject = (ex.Message.Length > 100 ? ex.Message.Substring(0, 90) + " ..." : ex.Message);

                        customerCall.CallNumber = Util.GenerateUniqueID();
                        customerCall.CompanyId = hostCompany.CompanyId;
                        customerCall.CustomerId = customer.CustomerId;
                        customerCall.OpenedDate = customerCall.ModifiedDate = DateTime.Now.Date;
                        customerCall.Description = String.Empty;

                        customerCall.CustomerCallTypeId = CustomerCallType.ERROR;
                        customerCall.CustomerCallStatusId = CustomerCallStatus.New;
                        customerCall.Rating = 5;

                        customerCall.CallNumberAssociated = context.Request.RawUrl;
                        customerCall.Sector = Convert.ToString(context.Session["_lastPageTitle"]);

                        if (context.User != null)
                            if (context.User.Identity != null)
                                if (context.User.Identity.IsAuthenticated)
                                {
                                    User user = membershipManager.GetUserByEmail(context.User.Identity.Name);
                                    if (user != null)
                                        customerCall.UserId = user.UserId;
                                }

                        customerManager.InsertCustomerCall(customerCall, null, null, null);
                    }
                }
            return eventEntity;
        }




    }
}