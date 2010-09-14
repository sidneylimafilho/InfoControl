using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.SystemFramework;
using Vivina.Erp.WebUI.Site;

public partial class Site_CategoriesControl : Vivina.Erp.SystemFramework.UserControlBase<SitePageBase>
{
    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public bool Recursivo { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        var manager = new Vivina.Erp.BusinessRules.CategoryManager(this);
        var parentId = default(int?);
        if (Page.Category != null)
            parentId = Page.Category.CategoryId;

        list.DataSource = manager.GetChildCategories(Page.Company.CompanyId, parentId, Recursivo);
        list.DataBind();
    }    
}