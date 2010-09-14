using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: WebResource("InfoControl.Web.aspnet_client.Vivina_Framework.Scripts.Mask.js", "text/javascript")]
namespace InfoControl.Web.UI.WebControls
{

    [ToolboxData("<{0}:MaskedTextBox runat=server></{0}:MaskedTextBox>")]
    [ToolboxBitmap(typeof(MaskedTextBox))]
    public class MaskedTextBox : TextBox
    {
        /// <summary>
        /// Mascara que o campo utilizará para validar a entrada de dados
        /// </summary>
        [Bindable(true)]
        [Category("Mask Settings")]
        [Localizable(true)]
        [Description("Mascara que o campo utilizará para validar a entrada de dados")]
        public string Mask
        {
            get { return Attributes["mask"]; }
            set { Attributes["mask"] = value; }
        }

        [Bindable(true)]
        [Category("Mask Settings")]
        [Localizable(true)]
        public string MaskDisplay
        {
            get { return Attributes["maskDisplay"]; }
            set { Attributes["maskDisplay"] = value; }
        }


        [Bindable(true)]
        [Category("Mask Settings")]
        [Localizable(true)]
        public string MaskNumeric
        {
            get { return Attributes["maskNumeric"]; }
            set { Attributes["maskNumeric"] = value; }
        }

        [Bindable(true)]
        [Category("Mask Settings")]
        [Localizable(true)]
        public string MaskAlpha
        {
            get { return Attributes["maskAlpha"]; }
            set { Attributes["maskAlpha"] = value; }
        }

        [Bindable(true)]
        [Category("Mask Settings")]
        [Localizable(true)]
        public string MaskAlphaNumeric
        {
            get { return Attributes["maskAlphaNumeric"]; }
            set { Attributes["maskAlphaNumeric"] = value; }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //
            // Propriedades de configuração
            //
            if (Attributes["maskAlphaNumeric"] == null)
                Attributes["maskAlphaNumeric"] = "?";

            if (Attributes["maskAlpha"] == null)
                Attributes["maskAlpha"] = "X";

            if (Attributes["maskNumeric"] == null)
                Attributes["maskNumeric"] = "9";

            if (Attributes["maskDisplay"] == null)
                Attributes["maskDisplay"] = "_";

            if (Attributes["RegexPattern"] == null)
                Attributes["RegexPattern"] = "";

            if (Attributes["OnRegexNoMatch"] == null)
                Attributes["OnRegexNoMatch"] = "";

            if (Attributes["OnRegexMatch"] == null)
                Attributes["OnRegexMatch"] = "";

            if (Attributes["OnWrongKeyPressed"] == null)
                Attributes["OnWrongKeyPressed"] = "";

            //
            // Eventos
            //
            Attributes["onfocus"] = "MaskedTextBox_GotFocus(this);";
            Attributes["oncut"] = "MaskedTextBox_OnCut(this);";
            Attributes["onclick"] = "MaskedTextBox_OnClick(event, this);";
            Attributes["onpaste"] = "MaskedTextBox_OnPaste(this);";
            Attributes["oninput"] = "MaskedTextBox_OnInput(event, this);";
            Attributes["onkeydown"] = "MaskedTextBox_KeyDown(event, this);";
            Attributes["onkeypress"] = "MaskedTextBox_KeyPress(event, this);";
            Attributes["onblur"] = "MaskedTextBox_LostFocus(this);";


            Page.ClientScript.RegisterClientScriptInclude("_Global", StringResources.ClientScriptFilesPath + "_Global.js");
            Page.ClientScript.RegisterClientScriptInclude("_Config", StringResources.ClientScriptFilesPath + "_Config.js");
            Page.ClientScript.RegisterClientScriptInclude("Mask", StringResources.ClientScriptFilesPath + "Mask.js");

        }


    }
}
