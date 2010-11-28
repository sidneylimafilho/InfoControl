using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using InfoControl.Runtime;
using InfoControl.Web.Services;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.SystemFramework;

namespace InfoControl.Web.UI
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Alerts : DataServiceBase
    {
        // Add [WebGet] attribute to use HTTP GET
        [OperationContract]
        public string GetAlerts()
        {
            using (var manager = new AlertManager(null))
                return manager.GetAlerts(User.Identity.UserId, DateTime.Now.Date)
                    .Select(al => new {al.AlertId, al.Description}).SerializeToJson();
        }

        [OperationContract]
        public void Delete(int alertId)
        {
            using (var manager = new AlertManager(null))
                manager.DeleteAlert(alertId);
        }
    }
}