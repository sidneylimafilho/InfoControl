using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl;
using Vivina.Erp.DataClasses;

using Vivina.Erp.BusinessRules;
using InfoControl;
using InfoControl.Web.Security;


[PermissionRequired("Sale")]
public partial class Company_POS_SaleList : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucDateTimeInterval.DateInterval = new DateTimeInterval(DateTime.Now.Date, DateTime.Now.Date);
            txtFiscalNumber.Text = Convert.ToString(Page.Customization["txtFiscalNumber"]);
            chkShowCanceledSale.Checked = Convert.ToBoolean(Page.Customization["chkShowCanceledSale"]);

            if (Page.Customization["BeginDate"] != null && Page.Customization["EndDate"] != null)
                ucDateTimeInterval.DateInterval = new DateTimeInterval((DateTime)Page.Customization["BeginDate"], (DateTime)Page.Customization["EndDate"]);

            cboPageSize.SelectedIndex = Convert.ToInt32(Page.Customization["cboPageSize"]);

        }
    }

    protected void odsSaleList_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["customerId"] = null;
        e.InputParameters["saleStatusId"] = null;
        e.InputParameters["receiptNumber"] = null;

        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["dateTimeInterval"] = ucDateTimeInterval.DateInterval;
        e.InputParameters["showCanceled"] = chkShowCanceledSale.Checked;

        if (!String.IsNullOrEmpty(cboSaleStatus.SelectedValue))
            e.InputParameters["saleStatusId"] = Convert.ToInt32(cboSaleStatus.SelectedValue);

        if (Page.ViewState["CustomerId"] != null)
            e.InputParameters["customerId"] = Convert.ToInt32(Page.ViewState["CustomerId"]);

        if (!String.IsNullOrEmpty(txtFiscalNumber.Text) && txtFiscalNumber.Text.Length <= 9)
            e.InputParameters["receiptNumber"] = Convert.ToInt32(txtFiscalNumber.Text);

        var representantUser = new RepresentantManager(this).GetRepresentantUser(User.Identity.UserId);

        if (representantUser != null)
            e.InputParameters["representantId"] = representantUser.RepresentantId;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Page.Customization["chkShowCanceledSale"] = chkShowCanceledSale.Checked;
        Page.Customization["cboPageSize"] = cboPageSize.SelectedIndex;
        Page.Customization["CustomerId"] = Page.ViewState["CustomerId"];
        Page.Customization["txtFiscalNumber"] = txtFiscalNumber.Text;

        if (ucDateTimeInterval.DateInterval != null)
        {
            Page.Customization["BeginDate"] = ucDateTimeInterval.DateInterval.BeginDate;
            Page.Customization["EndDate"] = ucDateTimeInterval.DateInterval.EndDate;
        }

        Int32 pageSize = 0;
        if (cboPageSize.SelectedValue.Equals("All"))
            pageSize = int.MaxValue;
        else
            pageSize = Convert.ToInt32(cboPageSize.SelectedValue);

        grdSalesList.PageSize = pageSize;

        ///if not exists a selected number, 'rebind' because the date values
        if (String.IsNullOrEmpty(txtFiscalNumber.Text) || txtFiscalNumber.Text.Length > 9)
        {
            grdSalesList.DataBind();
            return;
        }
        SaleManager sManager = new SaleManager(this);
        Sale sale = sManager.GetSaleByFiscalNumber((int)Company.MatrixId, Convert.ToInt32(txtFiscalNumber.Text));

        ///if not exists sale related with receipt number return and 'rebind' the grid
        if (sale == null)
        {
            ShowError(Resources.Exception.nonexistentReceiptNumber);
            grdSalesList.DataBind();
            return;
        }

        ///retrieve the SaleId and show the sale related with this saleId
        Response.Redirect("SaleViewer.aspx?SaleId=" + sale.SaleId.EncryptToHex());

    }

    protected void grdSalesList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
            e.Row.Attributes["onclick"] = "location='SaleViewer.aspx?SaleId=" + grdSalesList.DataKeys[e.Row.RowIndex]["SaleId"].EncryptToHex() + "';";
    }

    protected void SelCustomer_SelectedCustomer(object sender, SelectedCustomerEventArgs e)
    {
        Page.ViewState["CustomerId"] = e.Customer.CustomerId;
    }

}
