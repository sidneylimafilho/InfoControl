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

using InfoControl;
using InfoControl.Web.Security;

[PermissionRequired("Accounting")]
public partial class InfoControl_Accounting_AlertFinancier : Vivina.Erp.SystemFramework.PageBase
{
    #region functions


    /// <summary>
    /// this method return the first day of the current month
    /// </summary>
    /// <param name="Data">Data de hoje</param>
    /// <returns></returns>
    public DateTime firstDayOfMonth(DateTime date)
    {
        return DateTime.Parse("01" + date.ToString("/MM/yyyy"));
    }

    /// <summary>
    /// this method return the last day of the current month
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    public DateTime lastDayOfMonth(DateTime date)
    {
        return firstDayOfMonth(date).AddMonths(1).AddDays(-1);
    }


    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void dataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["beginDate"] = firstDayOfMonth(DateTime.Now.Date);
        e.InputParameters["endDate"] = lastDayOfMonth(DateTime.Now.Date);
    }
    protected void grdStock_SelectedIndexChanged(object sender, EventArgs e)
    {
        Context.Items["ProductId"] = grdStock.DataKeys[grdStock.SelectedIndex]["ProductId"].ToString();
        Server.Transfer("../Administration/Product.aspx");
    }
    protected void grdBill_SelectedIndexChanged(object sender, EventArgs e)
    {
        Context.Items["BillId"] = grdBill.DataKeys[grdBill.SelectedIndex]["BillId"].ToString();
        Server.Transfer("Bill.aspx");
    }
    protected void grdInvoices_SelectedIndexChanged(object sender, EventArgs e)
    {
        Context.Items["InvoiceId"] = grdInvoices.DataKeys[grdInvoices.SelectedIndex]["InvoiceId"].ToString();
        Server.Transfer("Invoice.aspx");
    }
    protected void odsStock_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;

    }

    protected void odsInvoices_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["dateTimeInterval"] = new DateTimeInterval(firstDayOfMonth(DateTime.Now.Date), firstDayOfMonth(DateTime.Now.Date));

    }
}
