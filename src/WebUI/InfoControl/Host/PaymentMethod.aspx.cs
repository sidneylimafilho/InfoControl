using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Vivina.Erp.DataClasses;

public partial class Host_PaymentMethod : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Context.Items["PaymentMethodId"] != null)
            {
                Page.ViewState["PaymentMethodId"] = Context.Items["PaymentMethodId"];
                frmPaymentMethod.ChangeMode(FormViewMode.Edit);
            }
        }
    }
    protected void odsPaymentMethod_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["PaymentMethodId"] = Convert.ToInt16(Page.ViewState["PaymentMethodId"]);
    }
    protected void odsPaymentMethod_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        PaymentMethod pm = (PaymentMethod)e.InputParameters["entity"];
        pm.ModifiedDate = DateTime.Now;
    }
    protected void odsPaymentMethod_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        PaymentMethod pm = (PaymentMethod)e.InputParameters["entity"];
        pm.ModifiedDate = DateTime.Now;
    }
    protected void odsPaymentMethod_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        Server.Transfer("PaymentMethods.aspx");
    }
    protected void odsPaymentMethod_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        Server.Transfer("PaymentMethods.aspx");
    }
    protected void frmPaymentMethod_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
            Server.Transfer("PaymentMethods.aspx");
    }
}
