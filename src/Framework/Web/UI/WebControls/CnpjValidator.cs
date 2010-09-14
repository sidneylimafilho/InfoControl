using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InfoControl.Web.UI.WebControls
{
    public class CnpjValidator : CustomValidator
    {
        public CnpjValidator()
        {            
            ClientValidationFunction = "IsCnpjValid";
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Page.ClientScript.RegisterClientScriptResource(typeof(CnpjValidator), "InfoControl.Web.UI.WebControls.Resources.script.js");
        }
    }
}