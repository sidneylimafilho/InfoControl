using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Adapters;

namespace InfoControl.Web.UI.Adapters
{
    public class DropDownListAdapter : WebControlAdapter
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!(Control is BaseValidator))
                Control.EnsureSecurity();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (Control.Enabled)
                base.Render(writer);
            else
            {
                var combo = (Control as DropDownList);
                if (combo.SelectedItem != null)
                    writer.Write(combo.SelectedItem.Text);
            }
        }
    }
}