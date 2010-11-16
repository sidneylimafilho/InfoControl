using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules;
using InfoControl;

[PermissionRequired("Receipt")]
public partial class InfoControl_Administration_Receipts : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cboPageSize.Items.Add(new ListItem() { Value = Int16.MaxValue.ToString(), Text = "Todos" });



            if (Page.Customization["dateTimeInterval"] == null)
                ucDateTimeinterval.DateInterval = new DateTimeInterval(DateTime.Now.AddMonths(-1), DateTime.Now);
            else
                ucDateTimeinterval.DateInterval = (InfoControl.DateTimeInterval)Page.Customization["dateTimeInterval"];


            cboPageSize.SelectedIndex = Convert.ToInt32(Page.Customization["cboPageSize"]);





            if (Page.Customization["ucCurrFieldFinalNumber"] != null)
                ucCurrFieldFinalNumber.CurrencyValue = Convert.ToDecimal(Page.Customization["ucCurrFieldFinalNumber"]);

            if (Page.Customization["ucCurrFieldInitialNumber"] != null)
                ucCurrFieldInitialNumber.CurrencyValue = Convert.ToDecimal(Page.Customization["ucCurrFieldInitialNumber"]);


            cboReceiptType.SelectedIndex = Convert.ToInt32(Page.Customization["cboReceiptType"]);


            txtSelectCustomer.Text = Convert.ToString(Page.Customization["selectCustomer"]);




        }


    }
    protected void odsReceipts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

        e.InputParameters["companyId"] = Company.CompanyId;


        e.InputParameters["dateTimeInterval"] = ucDateTimeinterval.DateInterval;

        if (Page.ViewState["CustomerId"] != null)
            e.InputParameters["customerId"] = Convert.ToInt32(Page.ViewState["CustomerId"]);

        if (Page.ViewState["SupplierId"] != null)
            e.InputParameters["supplierId"] = Convert.ToInt32(Page.ViewState["SupplierId"]);

        e.InputParameters["selectCustomer"] = txtSelectCustomer.Text;


        if (!String.IsNullOrEmpty(cboReceiptType.SelectedValue))
            e.InputParameters["receiptType"] = Convert.ToInt32(cboReceiptType.SelectedValue);

        //if (cboReceiptType.SelectedValue == "all")
        //    e.InputParameters["receiptType"] = (Int32)ReceiptManager.ReceiptType.All;

        //if (cboReceiptType.SelectedValue == "entry")
        //    e.InputParameters["receiptType"] = (Int32)ReceiptManager.ReceiptType.Entry;

        //if (cboReceiptType.SelectedValue == "delivery")
        //    e.InputParameters["receiptType"] = (Int32)ReceiptManager.ReceiptType.Delivery;

        e.InputParameters["initialNumber"] = ucCurrFieldInitialNumber.CurrencyValue;

        e.InputParameters["finalNumber"] = ucCurrFieldFinalNumber.CurrencyValue;



    }
    protected void grdReceipts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = "location='Receipt.aspx?ReceiptId=" + e.Row.DataItem.GetPropertyValue("ReceiptId") + "';";
            //
            // Cancel the SELECT click and throw a Confirm Window to delete a line
            //
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }
    }

    protected void grdReceipts_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            Response.Redirect("Receipt.aspx");
        }
    }
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Server.Transfer("receipt.aspx");
    }
   
    protected void SelCustomer_SelectedCustomer(object sender, SelectedCustomerEventArgs e)
    {
        if (e.Customer != null)
        {
            Page.ViewState["CustomerId"] = e.Customer.CustomerId;
        }

    }

    protected void selSupplier_SelectedSupplier(object sender, SelectedSupplierEventArgs e)
    {
        if (e.Supplier != null)
            Page.ViewState["SupplierId"] = e.Supplier.SupplierId;
    }

    protected void btnSearchReceipt_Click(object sender, EventArgs e)
    {

        Page.Customization["cboPageSize"] = cboPageSize.SelectedIndex;
        Page.Customization["ucCurrFieldFinalNumber"] = ucCurrFieldFinalNumber.CurrencyValue;
        Page.Customization["ucCurrFieldInitialNumber"] = ucCurrFieldInitialNumber.CurrencyValue;
        Page.Customization["cboReceiptType"] = cboReceiptType.SelectedIndex;
        Page.Customization["selectCustomer"] = txtSelectCustomer.Text;
        Page.Customization["dateTimeInterval"] = ucDateTimeinterval.DateInterval;

        grdReceipts.DataBind();




    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {

        grdReceipts.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdReceipts.DataBind();

    }


    [WebMethod]
    public static bool DeleteReceipt(int receiptId, int companyId)
    {

        bool result = true;

        var sale = new SaleManager(null).GetSaleByReceipt(companyId, receiptId);

        if (sale != null)
            new SaleManager(null).SetNullReceiptIDInSale(sale.SaleId);

        using (ReceiptManager receiptManager = new ReceiptManager(null))
        {
            try
            {
                receiptManager.DeleteReceipt(receiptManager.GetReceipt(receiptId, companyId));
            }
            catch (System.Data.SqlClient.SqlException)
            {
                result = false;
            }
        }
        return result;
    }
}
