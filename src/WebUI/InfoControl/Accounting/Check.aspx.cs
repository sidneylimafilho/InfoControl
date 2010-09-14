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

using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

public partial class Accounting_Check : Vivina.Erp.SystemFramework.PageBase
{
    private string FormatDouble(TextBox Double)
    {
        //
        // This method fixes the bug that happens on .NET with 2 diferents cultures
        // The data was inconsistante, 'cause the en-US culture uses the "." (dot) to separate decimal
        // values, when the pt-BR culture uses the "," (coma).
        // The replace for the "_" (underline) character is to fix the mask extender problem. If the focus
        // are in the control that have a mask, that control send a "_" to the database.
        //
        Double.Text = Double.Text.Replace("_", "0");
        Double.Text = Double.Text.Replace(".", "");
        Double.Text = Double.Text.Replace(",", ".");
        return Double.Text;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Context.Items["CheckId"] != null)
            {
                Page.ViewState["CheckId"] = Context.Items["CheckId"];
                frmCheck.ChangeMode(FormViewMode.Edit);
            }
        }
    }
    protected void odsCheck_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CheckId"] = Page.ViewState["CheckId"];
    }
    protected void odsCheck_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Check check = (Check)e.InputParameters["entity"];
        //check.CompanyId = Company.CompanyId;
        check.EntryDate = DateTime.Now;
    }
    protected void odsCheck_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Check check = (Check)e.InputParameters["entity"];
        //check.CompanyId = Company.CompanyId;
        check.EntryDate = DateTime.Now;
    }
    protected void odsCheck_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        Response.Redirect("Checks.aspx");
    }
    protected void odsCheck_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        Response.Redirect("Checks.aspx");
    }
    protected void frmCheck_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Update" || e.CommandName == "Insert")
        {
            TextBox CheckValueTextBox = frmCheck.FindControl("CheckValueTextBox") as TextBox;
            CheckValueTextBox.Text = FormatDouble(CheckValueTextBox);
        }
        else if (e.CommandName == "Cancel")
            Response.Redirect("Checks.aspx");
    }
}
