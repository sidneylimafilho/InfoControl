using System;
using System.Web.UI.WebControls.Adapters;

namespace InfoControl.Web.UI.Adapters
{
    public class LinkButtonAdapter : WebControlAdapter
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Control.EnsureSecurity();
        }
    }
}