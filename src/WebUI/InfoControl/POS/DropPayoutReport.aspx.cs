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

using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.SystemFramework;
using InfoControl.Web.Security;

[PermissionRequired("DropPayout")]
public partial class Company_POS_DropPayoutReport : Vivina.Erp.SystemFramework.PageBase
{
    private void CalculateDropPayout(DateTime startDate, DateTime endDate)
    {
        DataTable sales = new DataTable();
        DropPayoutManager dManager = new DropPayoutManager(this);
        SaleManager sManager = new SaleManager(this);
        Decimal total = (decimal)0;
        Decimal totalSangria = (decimal)0;
        Decimal totalSuplemento = (decimal)0;
        Decimal aVista = (decimal)0;

        //
        // This If is making sure that the code below will not execute in the rare case of the user do not
        // have a deposit registered.
        //
        if (Deposit != null)
        {
            IQueryable<DropPayout> sangria =
                dManager.GetSangriaByDate(Company.CompanyId, Deposit.DepositId, ucDateTimeInterval.DateInterval);
            rptSangria.DataSource = sangria;
            rptSangria.DataBind();
            rptSangriaValores.DataSource = sangria;
            rptSangriaValores.DataBind();

            IQueryable<DropPayout> suplemento =
                dManager.GetSuplementoByDate(Company.CompanyId, Deposit.DepositId, ucDateTimeInterval.DateInterval);
            rptSuplemento.DataSource = suplemento;
            rptSuplemento.DataBind();
            rptSuplementoValores.DataSource = suplemento;
            rptSuplementoValores.DataBind();

            sales = sManager.MapOfSale_Payment(Company.CompanyId, ucDateTimeInterval.DateInterval);

            rptFormasPagamento.DataSource = sales;
            rptValores.DataSource = sales;
            rptFormasPagamento.DataBind();
            rptValores.DataBind();
            for (int i = 0; i < sales.Rows.Count; i++)
            {
                total += Convert.ToDecimal(sales.Rows[i]["Value"]);
                if (sales.Rows[i]["Name"].ToString() == "Dinheiro")
                    aVista = Convert.ToDecimal(sales.Rows[i]["Value"]);
            }

            foreach (DropPayout sang in sangria)
            {
                totalSangria += sang.Amount;
            }
            foreach (DropPayout sup in suplemento)
            {
                totalSuplemento += sup.Amount;
            }
        }
        lblCaixa.Text = (aVista + totalSangria + totalSuplemento).ToString("C");
        lblTotalPagamento.Text = total.ToString("C");

        lstExtract.DataBind();

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //    txtStartDate.Text = DateTime.Today.ToShortDateString();
            //    txtEndDate.Text = DateTime.Today.ToShortDateString();
            ucDateTimeInterval.DateInterval = new InfoControl.DateTimeInterval(DateTime.Now, DateTime.Now);

        }

        //CalculateDropPayout(Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text + " 23:59:59"));
        CalculateDropPayout(ucDateTimeInterval.DateInterval.BeginDate, ucDateTimeInterval.DateInterval.EndDate);
    }
    protected void odsExtract_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["customerId"] = null;
        e.InputParameters["dateTimeInterval"] = ucDateTimeInterval.DateInterval;
        e.InputParameters["showCanceled"] = false;
    }
    protected void lstExtract_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
            Page.ViewState["Total"] = 0;
        else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            double total = Convert.ToDouble((e.Item.FindControl("lblTtl") as Label).Text);
            Page.ViewState["Total"] = Convert.ToDouble(Page.ViewState["Total"]) + total;
        }
        lblFinalTotal.Text = "Total: " + Convert.ToDecimal(Page.ViewState["Total"]).ToString("C");
    }
}
