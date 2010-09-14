using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InfoControl.Web.UI.WebControls
{
    public class CpfValidator : CustomValidator
    {
        public CpfValidator()
        {            
            ClientValidationFunction = "IsCpfValid";
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Page.ClientScript.RegisterClientScriptResource(typeof(CpfValidator), "InfoControl.Web.UI.WebControls.Resources.script.js");
        }       
    }
}