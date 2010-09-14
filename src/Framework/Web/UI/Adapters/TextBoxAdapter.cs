using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InfoControl.Web.UI.Adapters
{
    public class TextBoxAdapter : DataWebControlAdapter
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Control.EnsureSecurity();
            Page.ClientScript.RegisterClientScriptResource(typeof(TextBoxAdapter),
                                                           "InfoControl.Web.UI.Adapters.TextBoxEffects.js");
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (Control.Visible)
            {
                if (Control.Enabled)
                {
                    //
                    // Force the OnFocus as Attribute, because the SetFocus method render first
                    //
                    Control.Attributes["onfocus"] += "GlassTextBox_Focus(this);";
                    Control.Attributes["onblur"] += "GlassTextBox_Blur(this);";
                    Page.ClientScript.RegisterStartupScript(GetType(), Control.ClientID,
                                                            "InfoControl.TextBoxAdapter.AttachEvents('" +
                                                            Control.ClientID + "');\n", true);

                    writer.WriteBeginTag("span");
                    writer.WriteAttribute("class", Control.CssClass);
                    Control.CssClass = "";
                    writer.Write(HtmlTextWriter.TagRightChar);
                    base.RenderBeginTag(writer);
                }

                //
                // Write the content because textarea dont work
                //
                if ((Control as TextBox).TextMode == TextBoxMode.MultiLine || !Control.Enabled)
                    writer.Write((Control as TextBox).Text);
            }
        }

        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            if (Control.Enabled)
            {
                base.RenderEndTag(writer);

                if ((Control as TextBox).TextMode != TextBoxMode.MultiLine)
                {
                    writer.WriteFullBeginTag("span");
                    writer.WriteEndTag("span");
                }
                writer.WriteEndTag("span");
            }
        }
    }
}