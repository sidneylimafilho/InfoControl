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

using System.Collections.Generic;
using InfoControl.Data;
using InfoControl;
using InfoControl;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;

using Vivina.Erp.SystemFramework;
using Telerik.Web.UI.Grid;
using Telerik.Web.UI;
using InfoControl.Web.Security;

[PermissionRequired("Invoices")]
public partial class Accounting_Invoices : Vivina.Erp.SystemFramework.PageBase
{


    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void odsInvoices_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Server.Transfer("Invoice.aspx");
    }

    protected void grdInvoices_SortCommand(object source, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            e.Canceled = true;
            Server.Transfer("Invoice.aspx");
        }
    }

    protected void grdInvoices_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "RowClick" && e.Item is GridDataItem)
        {
            Context.Items["InvoiceId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["InvoiceId"];
            Server.Transfer("Invoice.aspx");
        }
        else if (e.CommandName == "Delete" && e.Item.ItemType != GridItemType.GroupFooter)
        {
            var financialManager = new FinancialManager(this);
            try
            {
                if (e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["InvoiceId"] != null)
                    financialManager.DeleteInvoice(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["InvoiceId"]), Company.CompanyId);
                grdInvoices.DataBind();
            }
            catch (System.Data.SqlClient.SqlException err)
            {
                DataManager.Rollback();
                if (err.ErrorCode.Equals(Convert.ToInt32("0x80131904", 16)))
                    ShowError(Resources.Exception.DeletingRegisterWithForeignKey);
                e.Canceled = true;

            }

            if (grdInvoices.Items.Count == 0)
                Response.Redirect("Invoices.aspx");

        }
    }

    protected void odsSearchInvoices_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["accountSearch"] = ucAccountSearch;
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void grdInvoices_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            e.Item.Attributes["onclick"] = "location='Invoice.aspx?InvoiceId=" + e.Item.DataItem.GetPropertyValue("InvoiceId").EncryptToHex() + "';";

        if (e.Item.ItemType == GridItemType.GroupFooter)
            e.Item.Cells[e.Item.Cells.Count - 1].Visible = false;

    }

    protected void odsAccount_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (rbtExportAutomaticDebit.Checked)
        {
            AccountManager accountManager = new AccountManager(this);
            List<Parcel> lstParcels = new ParcelsManager(this).GetInvoiceParcelsByPeriodInAcount(Company.CompanyId, DateTime.Now.Sql2005MinValue(), DateTime.Now, null).ToList();
            Response.Clear();
            Response.ContentType = "text/rtf";
            string headerValue = "attachment;filename=Débito.rtf";
            Response.AddHeader("content-disposition", headerValue);
            Response.ContentEncoding = System.Text.Encoding.Default;
            // Response.Write(accountManager.GenerateAutomaticDebitFile(Company, accountManager.GetAccount(Convert.ToInt32(cboAccount.SelectedValue), Company.CompanyId), lstParcels, 1));
            Response.End();
        }
        else
        {
            try
            {
                var financialManager = new FinancialManager(this);
                Response.Clear();
                financialManager.GerarArquivoRemessa(Company.CompanyId, Convert.ToInt32(cboAccount.SelectedValue), DateTime.MinValue.Sql2005MinValue(), DateTime.MaxValue, Response.OutputStream);
                Response.ContentType = "text/txt";
                string headerValue = "attachment;filename=" + "Arquivo.txt";
                Response.AddHeader("content-disposition", headerValue);
                // Response.Write(Response.OutputStream);
                Response.End();
            }
            catch (Exception ex)
            {

                ShowError(Resources.Exception.InvalidBank);
            }

        }

    }

    protected void SearchAccount_SelectedParameters(object sender, EventArgs e)
    {
        grdInvoices.ShowFooter = grdInvoices.MasterTableView.ShowGroupFooter = true;

        grdInvoices.DataSourceID = odsSearchInvoices.ID;

        Session["AccountSearch"] = sender;

        grdInvoices.DataBind();

        // if there is no line won't show footer
        if (grdInvoices.Items.Count == 0)
            grdInvoices.ShowFooter = grdInvoices.MasterTableView.ShowGroupFooter = false;


    }

    protected void grdInvoices_DataBound(object sender, EventArgs e)
    {



    }
}
