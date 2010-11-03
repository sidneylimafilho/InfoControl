using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Vivina.Erp.DataClasses;
using InfoControl;


public partial class InfoControl_Judicial_Contracts : Vivina.Erp.SystemFramework.PageBase
{
    #region Contracts

    protected void grdContracts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
            e.Row.Attributes["onclick"] = "top.$.lightbox('Accounting/Contract.aspx?ContractId=" + grdContracts.DataKeys[e.Row.RowIndex]["ContractId"].EncryptToHex() + "&lightbox[iframe]=true' );return;";
    }


    #endregion
    protected void odsContracts_Selecting1(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["customerId"] = Page.ViewState["CustomerId"] ?? Request["CustomerId"].DecryptFromHex();
    }



}
