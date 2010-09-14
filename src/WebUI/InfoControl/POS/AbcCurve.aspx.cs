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
using Vivina.Erp.BusinessRules;
using InfoControl.Web.Security;

[PermissionRequired("AbcCurve")]
public partial class InfoControl_POS_AbcCurve : Vivina.Erp.SystemFramework.PageBase
{
    ProductManager productManager;
    Int32 rowsCount = 0;
    Int32 productsCount;
    IList listAbcCurve;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Page.ViewState["error"] != null)
        {
            ShowError(Page.ViewState["error"].ToString());
        }
        if (!IsPostBack)
        {
            ucCurrFieldValA.Text = "20";
            ucCurrFieldValB.Text = "30";
            ucCurrFieldValC.Text = "50";
            ucDateTimeInterval.DateInterval = new InfoControl.DateTimeInterval(DateTime.Now, DateTime.Now.AddMonths(1));
            SetABCCurveParameters();
        }

    }
    protected void odsDeposit_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
    protected void cboDeposit_TextChanged(object sender, EventArgs e)
    {
        grdAbcCurve.DataBind();
    }
    protected void btnSelectParameter_Click(object sender, EventArgs e)
    {

        Decimal parameterSum = Convert.ToDecimal(ucCurrFieldValA.CurrencyValue + ucCurrFieldValB.CurrencyValue + ucCurrFieldValC.CurrencyValue + ucCurrFieldValD.CurrencyValue + ucCurrFieldValE.CurrencyValue);
        if (parameterSum != 100)
            Page.ViewState["error"] = "A soma dos valores digitados deve ser igual a 100";
        else
            SetABCCurveParameters();
    }
    protected void grdAbcCurve_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            rowsCount = rowsCount + 1;
            if (rowsCount <= Convert.ToDecimal(Page.ViewState["A"]))
                e.Row.Cells[e.Row.Cells.Count - 1].Text = "A";
            else if (rowsCount <= Convert.ToDecimal(Page.ViewState["B"]))
                e.Row.Cells[e.Row.Cells.Count - 1].Text = "B";
            else if (rowsCount <= Convert.ToDecimal(Page.ViewState["C"]))
                e.Row.Cells[e.Row.Cells.Count - 1].Text = "C";
            else if (rowsCount <= Convert.ToDecimal(Page.ViewState["D"]))
                e.Row.Cells[e.Row.Cells.Count - 1].Text = "D";
            else if (rowsCount <= Convert.ToDecimal(Page.ViewState["E"]))
                e.Row.Cells[e.Row.Cells.Count - 1].Text = "E";

        }
    }
    protected void grdAbcCurve_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    protected void SetABCCurveParameters()
    {
        productManager = new ProductManager(this);
        if (cboDeposit.SelectedValue != "0")
            listAbcCurve = productManager.GetProductsRankByDeposit(Company.CompanyId, Convert.ToInt32(cboDeposit.SelectedValue), "", ucDateTimeInterval.DateInterval);
        else
            listAbcCurve = productManager.GetProductsRankByDeposit(Company.CompanyId, null, "", ucDateTimeInterval.DateInterval);

        productsCount = listAbcCurve.Count;
        Page.ViewState["A"] = (Convert.ToDecimal("0" + ucCurrFieldValA.Text) * productsCount) / 100;
        Page.ViewState["B"] = ((Convert.ToDecimal("0" + ucCurrFieldValB.Text) * productsCount) / 100) + Convert.ToDecimal(Page.ViewState["A"]);
        Page.ViewState["C"] = ((Convert.ToDecimal("0" + ucCurrFieldValC.Text) * productsCount) / 100) + Convert.ToDecimal(Page.ViewState["B"]);
        Page.ViewState["D"] = ((Convert.ToDecimal("0" + ucCurrFieldValD.Text) * productsCount) / 100) + Convert.ToDecimal(Page.ViewState["C"]);
        Page.ViewState["E"] = ((Convert.ToDecimal("0" + ucCurrFieldValE.Text) * productsCount) / 100) + Convert.ToDecimal(Page.ViewState["D"]);
        grdAbcCurve.DataSource = listAbcCurve;
        grdAbcCurve.DataBind();
    }
    protected void grdAbcCurve_Sorting(object sender, GridViewSortEventArgs e)
    {

    }
}
