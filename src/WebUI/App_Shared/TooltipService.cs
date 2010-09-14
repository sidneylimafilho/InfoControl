using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using InfoControl.Web.Security;
using Vivina.Erp.SystemFramework;

// NOTE: If you change the class name "TooltipService" here, you must also update the reference to "TooltipService" in Web.config.
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
[ServiceContract(Namespace = "Http://InfoControl")]
public class TooltipService : DataControllerBase
{
    [OperationContract]
    public void SetToolTipClosed(string page, string toolTipId)
    {
        if (User.Personalization[page] == null)
            User.Personalization[page] = new Hashtable();

        (User.Personalization[page] as Hashtable)[toolTipId] = true;
    }
}
