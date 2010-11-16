using System;
using System.Data;
using System.ComponentModel;
using System.Web.UI;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl.Web.Security;

[PermissionRequired("BarCode_Print")]
public partial class Company_Products_Print : Vivina.Erp.SystemFramework.PageBase
{
    public DataRow _row;

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public DataRow Row { get { return _row; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        InventoryManager manager = new InventoryManager(this);
        DataTable table = manager.GetProductsInInventory(Convert.ToInt32(Request.QueryString["ProductId"]), Request.QueryString["DepositId"], Company.CompanyId);
        _row = table.Rows[0];
        pnlError.Visible = false;

        if (Convert.ToInt32(_row["Quantity"]) > 0)
        {
            object[] products = new object[Convert.ToInt32(_row["Quantity"])];
            for (int i = 0; i < products.Length; i++)
                products[i] = _row;

            labels.DataSource = products;
            labels.LabelFormat = cboLabels.SelectedValue;
            labels.DataBind();
        }

    }
    protected void cboLabels_SelectedIndexChanged(object sender, EventArgs e)
    {
        labels.LabelFormat = cboLabels.SelectedValue;
    }
}
