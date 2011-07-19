using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using Vivina.Erp.DataClasses;
using System.ServiceModel;

namespace Vivina.Erp.WebUI.InfoControl
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)] // Para apresentar o erro sem precisar ligar o trace no Web.config
    public class InfoControlDataService : DataService<InfoControlDataContext>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Company", EntitySetRights.None );
            config.SetEntitySetAccessRule("Customer", EntitySetRights.None);
            
            config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
        }
    }
}
