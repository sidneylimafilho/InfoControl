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

using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

public partial class Company_POS_InventoryPrice : Vivina.Erp.SystemFramework.PageBase
{
    public Label AverageCost;
    public TextBox ProfitMargin;
    public TextBox UnitPrice;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InventoryManager iManager = new InventoryManager(this);
            ViewState["DepositId"] = Context.Items["DepositId"].ToString();
            DataTable inv = iManager.GetInventoriesByDeposit(Context.Items["DepositId"].ToString(), Company.CompanyId);
            ViewState["ProductId"] = inv.Rows[0]["ProductId"].ToString();
            lblAverageCost.Text = inv.Rows[0]["AverageCosts"].ToString().Remove(inv.Rows[0]["AverageCosts"].ToString().Length - 4, 4);
            lblProductName.Text = inv.Rows[0]["Name"].ToString();
            lblProductCode.Text = inv.Rows[0]["ProductCode"].ToString();
            lblQuantity.Text = inv.Rows[0]["Quantity"].ToString();
            txtUnitPrice.Text = inv.Rows[0]["UnitPrice"].ToString().Remove(inv.Rows[0]["UnitPrice"].ToString().Length - 4, 4);

        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Server.Transfer("Inventories.aspx");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            InventoryManager iManager = new InventoryManager(this);
            if (ViewState["DepositId"].ToString() == "Matrix")
                iManager.AdjustInventoryPrice((int)Company.MatrixId,
                    Convert.ToDecimal(txtUnitPrice.Text),
                    Convert.ToDecimal(txtProfitMargin.Text),
                    Convert.ToInt16(ViewState["ProductId"]));
            else
                iManager.AdjustInventoryPrice(Company.CompanyId,
                    Convert.ToDecimal(txtUnitPrice.Text),
                    Convert.ToDecimal(txtProfitMargin.Text),
                    Convert.ToInt16(ViewState["ProductId"]));
        }
        catch (Exception ex)
        {
            throw ex;
        }
        Server.Transfer("Inventories.aspx");
    }
}
