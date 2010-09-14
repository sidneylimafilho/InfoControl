using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Permissions;

namespace InfoControl.Web.UI.WebControls
{

    [ToolboxData("<{0}:ContextMenu runat=server></{0}:ContextMenu>")]    
    public class ContextMenu : Panel
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Page.ClientScript.RegisterClientScriptInclude("Mask",  StringResources.ClientScriptFilesPath + "Mask.js");

        }

    }
}
