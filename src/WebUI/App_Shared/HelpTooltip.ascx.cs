using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using InfoControl.Web;

namespace Vivina.Erp.WebUI.App_Shared
{
    public partial class HelpTooltip : System.Web.UI.UserControl
    {
        /// <summary>
        /// Represents a text that show up to help
        /// </summary>
        public string Text
        {
            get { return lblHelpText.InnerHtml; }
            set { lblHelpText.InnerHtml = value; }
        }

        [TemplateContainer(typeof(HelpTootipContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate ItemTemplate { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            var toolTipContainer = new HelpTootipContainer();
            ItemTemplate.InstantiateIn(toolTipContainer);
            PlaceHolder1.Controls.Add(toolTipContainer);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }

    public class HelpTootipContainer : Control, INamingContainer
    {
    }
}