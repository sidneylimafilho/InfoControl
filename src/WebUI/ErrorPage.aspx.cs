using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using InfoControl.Web;
using InfoControl.Web.Configuration;
using InfoControl.Web.UI;



public partial class ErrorPage : InfoControl.Web.UI.DataPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BlockContextMenu = !WebConfig.Web.Compilation.Debug;

        //
        // Trata o erro para mostrar na página
        //
        Exception objErr = (Exception)Session["Error"];

        if (objErr != null)
        {
            while (objErr.InnerException != null)
                objErr = objErr.InnerException;

            string err = "<br /><h1>Server Error in Infocontrol Application.</h1><hr>" +
                    "<br /><h2>Error Message: [=MESSAGE=]</h2>" +
                    "<br /><p><b>Stack Trace:</b><br /> [=TRACE=]</p>" +
                    "<br />[=URL=]";

            err = err.Replace("[=URL=]", Request.Url.ToString());
            err = err.Replace("[=MESSAGE=]", objErr.Message.Replace("\n", "<br />"));
            err = err.Replace("[=TRACE=]", objErr.StackTrace.Replace("\n", "<br />"));
            lblError.Text = err;
        }


        LabelError.Style.Add(HtmlTextWriterStyle.Display, WebConfig.Web.Compilation.Debug ? "" : "none");
    }
}
