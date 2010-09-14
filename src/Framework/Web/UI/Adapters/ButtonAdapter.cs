using System;
using System.Web.UI;
using System.Web.UI.WebControls.Adapters;

namespace InfoControl.Web.UI.Adapters
{
    public class ButtonAdapter : WebControlAdapter
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Control.EnsureSecurity();

            Page.ClientScript.RegisterClientScriptResource(GetType(),
                                                           "InfoControl.Web.UI.Adapters.ButtonEffects.js");
        }


        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (Control.Visible)
            {
                writer.WriteBeginTag("span");
                if (Control.Enabled)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), Control.ClientID,
                                                            "InfoControl.ButtonAdapter.AttachEvents('" +
                                                            Control.ClientID + "');", true);
                }
                else
                {
                    Control.CssClass += "_disabled";
                }
                writer.WriteAttribute("class", Control.CssClass);
                Control.CssClass = "";

                writer.Write(HtmlTextWriter.TagRightChar);
            }
            base.RenderBeginTag(writer);
        }

        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);
            writer.WriteFullBeginTag("span");
            writer.WriteEndTag("span");
            writer.WriteEndTag("span");
        }
    }
}