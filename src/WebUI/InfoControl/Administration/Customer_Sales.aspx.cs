using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;

public partial class Company_Administration_WebUserControl : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["CustomerId"]))
            Page.ViewState["CustomerId"] = Request["CustomerId"].DecryptFromHex();
    }
    protected void odsCustomerSales_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CustomerId"] = Convert.ToInt32(Page.ViewState["CustomerId"]);
        e.InputParameters["CompanyId"] = Company.CompanyId;
    }

    protected void grdSaleByCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
            e.Row.Attributes["onclick"] = "top.tb_show('Visualizador de Vendas','POS/SaleViewer.aspx?SaleId=" + grdSaleByCustomer.DataKeys[e.Row.RowIndex]["SaleId"].EncryptToHex() + "' );return;";

    }
}
