using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.WebUI.Site;

public partial class Site_ProductsControl : Vivina.Erp.SystemFramework.UserControlBase<SitePageBase>
{

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public int Qtd { get; set; }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string Ordem { get; set; }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public int? CriptoId
    {
        get
        {
            if (!String.IsNullOrEmpty(Request["pid"]))
                return Convert.ToInt32(Request["pid"]);
            return default(int?);
        }
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string ContenhaTexto { get; set; }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string Fabricante { get; set; }




    protected void odsProducts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (!e.ExecutingSelectCount)
            e.Arguments.SortExpression = Ordem;


        e.InputParameters["companyId"] = Page.Company.CompanyId;

        if (Page.Deposit != null)
            e.InputParameters["depositId"] = Page.Deposit.DepositId;

        e.InputParameters["description"] = null;

        e.InputParameters["isTemp"] = false;
        e.InputParameters["initialLetter"] = null;

        e.InputParameters["categoriesRecursive"] = true;



        e.InputParameters["productId"] = null;
        e.InputParameters["categoryId"] = null;
        if (Page.Category != null)
            e.InputParameters["categoryId"] = Page.Category.CategoryId;
        else if (CriptoId.HasValue)
            e.InputParameters["productId"] = CriptoId.Value;



        e.InputParameters["name"] = null;
        if (!String.IsNullOrEmpty(ContenhaTexto))
            e.InputParameters["name"] = ContenhaTexto;



        e.InputParameters["manufacturerId"] = null;
        if (!String.IsNullOrEmpty(Fabricante))
            e.InputParameters["manufacturerId"] = Convert.ToInt32(Fabricante.DecryptFromHex());
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Qtd > 0)
            dtpProducts.PageSize = Qtd;

        //dtpProducts.PageSize = 20;
        if (!String.IsNullOrEmpty(Request["qtd"]))
            dtpProducts.PageSize = Qtd = Convert.ToInt32(Request["qtd"]);

        Fabricante = Fabricante ?? Request["f"];
        Ordem = Ordem ?? Request["orderBy"];
        ContenhaTexto = ContenhaTexto ?? Request["txt"];
    }
}