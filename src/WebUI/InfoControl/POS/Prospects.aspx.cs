using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl.Web.Security;
using BudgetStatus = Vivina.Erp.BusinessRules.SaleManager.BudgetStatus;
using Vivina.Erp.BusinessRules.Services;
using System.Linq;


[PermissionRequired("ProspectBuilder")]
public partial class InfoControl_POS_Prospects : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

#warning a atribuição a essa variável de sessão é temporária e deverá ser revista; ela é utilizada na prospectBuilder.aspx.cs
            Session["BudgetId"] = null;

            Int32 tmp = 0;
            ListItem listItem;
            cboPageSize.Items.Add(new ListItem() { Value = Int16.MaxValue.ToString(), Text = "Todos" });
            rbtstatus.Items.Add(new ListItem("Todos", ""));
            rbtstatus.Items.Add(new ListItem("Abertos", Convert.ToInt32(BudgetStatus.Open).ToString()));
            rbtstatus.Items.Add(new ListItem("Enviados", Convert.ToInt32(BudgetStatus.SentToCustomer).ToString()));
            rbtstatus.Items.Add(new ListItem("Aceito", Convert.ToInt32(BudgetStatus.Accepted).ToString()));
            rbtstatus.Items.Add(new ListItem("Não aceito", Convert.ToInt32(BudgetStatus.Rejected).ToString()));

            if (!String.IsNullOrEmpty(Convert.ToString(Page.Customization["cboPageSize"])))
            {
                listItem = cboPageSize.Items.FindByValue(Convert.ToString(Page.Customization["cboPageSize"]));
                if (listItem != null)
                    cboPageSize.SelectedValue = listItem.Value;
            }

            if (!String.IsNullOrEmpty(Convert.ToString(Page.Customization["rbtStatus"])))
            {
                listItem = rbtstatus.Items.FindByValue(Convert.ToString(Page.Customization["rbtStatus"]));
                if (listItem != null)
                    rbtstatus.SelectedValue = listItem.Value;
            }

            //
            // Load SalesPerson to the DropDownList
            //
            var humanResourcesManager = new HumanResourcesManager(this);
            cboVendor.DataSource = humanResourcesManager.GetSalesPerson(Company.CompanyId);
            cboVendor.DataBind();
        }
    }

    protected void odsProspects_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["customerId"] = null;
        e.InputParameters["budgetStatus"] = null;
        e.InputParameters["beginDate"] = ucBeginDate.DateTime;
        e.InputParameters["endDate"] = ucEndDate.DateTime;

        if (!String.IsNullOrEmpty(selProduct.Name))
            e.InputParameters["productName"] = selProduct.Name.Split('|').GetValue(0).ToString().Trim();

        if (!txtPhone.Text.Equals("(__)____-____"))
            e.InputParameters["telephone"] = txtPhone.Text;

        if (!String.IsNullOrEmpty(cboVendor.SelectedValue))
            e.InputParameters["vendorId"] = Convert.ToInt32(cboVendor.SelectedValue);

        if (Page.ViewState["CustomerId"] != null)
            e.InputParameters["customerId"] = Convert.ToInt32(Page.ViewState["CustomerId"]);

        if (!String.IsNullOrEmpty(rbtstatus.SelectedValue))
        {
            e.InputParameters["budgetStatus"] = Convert.ToInt32(rbtstatus.SelectedValue);
        }
    }

    protected void grdProspects_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
            Response.Redirect("ProspectBuilder.aspx");
    }

    protected void grdProspects_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            if (e.Exception.InnerException is System.Data.SqlClient.SqlException)
            {
                System.Data.SqlClient.SqlException err = e.Exception.InnerException as System.Data.SqlClient.SqlException;
                if (err.ErrorCode.Equals(Convert.ToInt32("0x80131904", 16)))
                {
                    ShowError(Resources.Exception.DeletingRegisterWithForeignKey);
                    e.ExceptionHandled = true;
                }
            }
        }
    }

    protected void grdProspects_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");

            e.Row.Attributes["onclick"] = "location='ProspectBuilder.aspx?BudgetId=" + grdProspects.DataKeys[e.Row.RowIndex]["BudgetId"] + "';";

        }
    }

    protected void SelCustomer_SelectedCustomer(object sender, SelectedCustomerEventArgs e)
    {
        Page.ViewState["CustomerId"] = e.Customer.CustomerId;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Page.Customization["cboPageSize"] = cboPageSize.SelectedValue;
        Page.Customization["rbtStatus"] = rbtstatus.SelectedValue;

        grdProspects.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdProspects.DataBind();
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {

        grdProspects.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdProspects.DataBind();

    }

    [WebMethod]
    public static bool DeleteBudget(int budgetId, int companyId)
    {
        bool result = true;
        using (SaleManager saleManager = new SaleManager(null))
        {
            try
            {
                var serviceOrder = new ServicesManager(null).GetServiceOrder(companyId, budgetId);

                if (serviceOrder != null)
                    saleManager.DetachBudgetFromServiceOrder(serviceOrder.ServiceOrderId);

                saleManager.Delete(saleManager.GetBudget(budgetId, companyId));
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                result = false;
            }
        }
        return result;
    }

}
