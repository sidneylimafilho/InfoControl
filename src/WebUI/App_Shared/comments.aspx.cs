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

using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Comments;

public partial class App_Shared_comments : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Comments1.SubjectId = Convert.ToInt32(Request["id"]);
        //Comments1.OpenInFrame = Convert.ToBoolean(Request["OpenInFrame"]);
        //Comments1.ShowButtons = Convert.ToBoolean(Request["showButtons"]);
    }
    protected void odsComments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["eventId"] = Convert.ToInt32(Page.ViewState["eventId"]);
    }
}
