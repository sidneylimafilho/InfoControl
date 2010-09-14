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

using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl.Web.Security;


[PermissionRequired("MapOfSales")]
public partial class Company_POS_MapOfSales : Vivina.Erp.SystemFramework.PageBase
{
    private double PaymentTotals = 0;
    private double CategoryTotals = 0;
    private double Totals = 0;
    private double SalesManTotals = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(txtPaymentStartDate.Text))
            txtPaymentStartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        if (String.IsNullOrEmpty(txtSalStartDate.Text))
            txtSalStartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        if (String.IsNullOrEmpty(txtTtlStartDate.Text))
            txtTtlStartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        if (String.IsNullOrEmpty(txtCatStartDate.Text))
            txtCatStartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        if (grdPayment.Rows.Count < 1)
            btnPaymentChart.Visible = false;
        if (grdCategory.Rows.Count < 1)
            btnCategoryChart.Visible = false;
        if (grdSalesMan.Rows.Count < 1)
            btnChartSalesperson.Visible = false;

    }
    protected void odsSelect_Department_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //e.InputParameters["startDate"] = Convert.ToDateTime(txtPaymentStartDate.Text);
        //e.InputParameters["endDate"] = DateTime.Now;
        e.InputParameters["dateInterval"] = new DateTimeInterval(Convert.ToDateTime(txtPaymentStartDate.Text), DateTime.Now);
        e.InputParameters["companyId"] = Company.CompanyId;
    }
    protected void odsSelect_SalesMan_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["startDate"] = Convert.ToDateTime(txtSalStartDate.Text);
        e.InputParameters["companyId"] = Company.CompanyId;
    }
    protected void odsSelect_Category_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["startDate"] = Convert.ToDateTime(txtCatStartDate.Text);
        e.InputParameters["companyId"] = Company.CompanyId;
    }
    protected void odsSelect_Totals_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["startDate"] = Convert.ToDateTime(txtTtlStartDate.Text);
        e.InputParameters["companyId"] = Company.CompanyId;
    }
    protected void txtPaymentStartDate_Click(object sender, EventArgs e)
    {
        chartPayment.Visible = false;
        grdPayment.Visible = true;
        grdPayment.DataBind();
    }
    protected void txtSalStartDate_Click(object sender, EventArgs e)
    {
        chartSalesperson.Visible = false;
        grdSalesMan.Visible = true;
        grdSalesMan.DataBind();
    }
    protected void txtCatStartDate_Click(object sender, EventArgs e)
    {
        chartCategory.Visible = false;
        grdCategory.Visible = true;
        grdCategory.DataBind();
    }
    protected void txtTtlStartDate_Click(object sender, EventArgs e)
    {
        grdTotals.DataBind();
    }
    protected void grdCategory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CategoryTotals += Convert.ToDouble(e.Row.Cells[1].Text);
        }
        lblCategoryTotal.Text = "Total R$ " + CategoryTotals.ToString("##,###,##0.00");
    }
    protected void grdPayment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            PaymentTotals += Convert.ToDouble(e.Row.Cells[1].Text);
        }
        lblPaymentTotal.Text = "Total R$ " + PaymentTotals.ToString("##,###,##0.00");
    }
    protected void grdTotals_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Totals += Convert.ToDouble(e.Row.Cells[1].Text);
        }
        lblTotals.Text = "Total R$ " + Totals.ToString("##,###,##0.00");
    }
    protected void grdSalesMan_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            SalesManTotals += Convert.ToDouble(e.Row.Cells[1].Text);
        }
        lblSalesPersonTotal.Text = "Total R$ " + SalesManTotals.ToString("##,###,##0.00");
    }
    protected void btnPaymentChart_Click(object sender, ImageClickEventArgs e)
    {
        grdPayment.Visible = false;
        chartPayment.Visible = true;
    }
    protected void btnCategoryChart_Click(object sender, ImageClickEventArgs e)
    {
        chartCategory.Visible = true;
        grdCategory.Visible = false;
    }
    protected void btnChartSalesperson_Click(object sender, ImageClickEventArgs e)
    {
        chartSalesperson.Visible = true;
        grdSalesMan.Visible = false;
    }
    protected void txtPaymentStartDate_Click(object sender, ImageClickEventArgs e)
    {

    }
}
