using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Services;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

using InfoControl;

using InfoControl.Web.Security;
[PermissionRequired("Contracts")]
public partial class InfoControl_Administration_Contracts : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cboContractStatus.SelectedIndex = Convert.ToInt32(Page.Customization["ContractStatus"]);
            cboContractType.SelectedIndex = Convert.ToInt32(Page.Customization["ContractType"]);
            cboPageSize.SelectedIndex = Convert.ToInt32(Page.Customization["cboPageSize"]);
            //  sel_customer.ShowCustomer(new CustomerManager(this).GetCustomer(Convert.ToInt32(Page.Customization["CustomerId"]), Company.CompanyId));

            ucDurationIntervalDate.DateInterval = (DateTimeInterval)Page.Customization["durationIntervalDate"];
        }
    }

    #region Contracts

    protected void SelCustomer_SelectedCustomer(object sender, SelectedCustomerEventArgs e)
    {
        if (e.Customer != null)
        {
            Page.ViewState["CustomerId"] = e.Customer.CustomerId;
        }

    }
    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {

        grdContracts.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdContracts.DataBind();

    }


    protected void odsContracts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

        if (!String.IsNullOrEmpty(cboContractType.SelectedValue))
            e.InputParameters["contractTypeId"] = Convert.ToInt32(cboContractType.SelectedValue);

        if (!String.IsNullOrEmpty(cboContractStatus.SelectedValue))
            e.InputParameters["contractStatusId"] = Convert.ToInt32(cboContractStatus.SelectedValue);

        if (ucDurationIntervalDate != null)
            e.InputParameters["durationIntervalDate"] = ucDurationIntervalDate.DateInterval;

        if (Page.ViewState["CustomerId"] != null)
            e.InputParameters["customerId"] = Convert.ToInt32(Page.ViewState["CustomerId"]);

        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void grdContracts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");


            e.Row.Attributes["onclick"] = "location='Contract.aspx?ContractId=" + e.Row.DataItem.GetPropertyValue("ContractId") + "';";
        }
    }

    protected void odsContracts_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        //
        // This method is to not allow deleting items that are associated with others Tables
        //
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


    #endregion


    protected void btnSearchContract_Click(object sender, EventArgs e)
    {
        grdContracts.DataBind();

        Page.Customization["durationIntervalDate"] = ucDurationIntervalDate.DateInterval;
        Page.Customization["ContractStatus"] = cboContractStatus.SelectedIndex;
        Page.Customization["ContractType"] = cboContractType.SelectedIndex;
        Page.Customization["cboPageSize"] = cboPageSize.SelectedIndex;
        // Page.Customization["CustomerId"] = Page.ViewState["CustomerId"];
    }

    #region DataSources
    protected void odsContractStatus_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
    }
    protected void odsPaymentMethod_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
    #endregion
 

    protected void odsContractType_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;

    }

    [WebMethod]
    public static bool DeleteContract(Int32 companyId, Int32 contractId)
    {
        bool result = true;
        using (ContractManager contractManager = new ContractManager(null))
        {
            try
            {
                contractManager.DeleteContract(contractManager.GetContract(companyId, contractId));
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                result = false;
            }
        }
        return result;
    }
}
