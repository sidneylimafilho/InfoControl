using System;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Adapters;

namespace InfoControl.Web.UI.Adapters
{
    public class LabelAdapter : WebControlAdapter
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!(Control is BaseValidator))
                Control.EnsureSecurity();
        }
    }
}