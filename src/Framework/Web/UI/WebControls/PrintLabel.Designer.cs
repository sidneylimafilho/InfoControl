using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;


namespace InfoControl.Web.UI.WebControls
{
    class PrintLabelsDesigner : ContainerControlDesigner
    {
        public override string GetDesignTimeHtml(DesignerRegionCollection regions)
        {
            PrintLabel printLabel = Component as PrintLabel;

            HtmlTextWriter htmlWriter = new HtmlTextWriter(new StringWriter());
            printLabel.RenderControl(htmlWriter);
            return htmlWriter.InnerWriter.ToString();
        }


    }
}
