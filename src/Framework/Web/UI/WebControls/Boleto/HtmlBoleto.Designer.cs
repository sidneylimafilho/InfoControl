using System;
using System.Drawing.Design;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace InfoControl.Web.UI.WebControls
{
    /// <summary>
    /// Summary description for RoundedCornersDesign.
    /// </summary>
    internal class HtmlBoletoDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            return (Component as HtmlBoleto).GetLayoutHtml();
        }
    }
}
