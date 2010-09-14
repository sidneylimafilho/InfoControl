using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;


public partial class Company_Stock_Movement : Vivina.Erp.SystemFramework.PageBase
{
    public bool isInserting = false;

    #region Functions

    private void InSuccess()
    {
        cboDeposit.SelectedIndex = 0;

        lblSuccess.Text = "Dados salvos com sucesso";
        lblSuccess.Visible = true;
        DataTable productList = (DataTable)Page.ViewState["ProductList"];
        productList.Clear();
        productList.Rows.Add(0, "", 0);
        Page.ViewState["ProductList"] = productList;
        BindGrid();
    }
    private DataTable CreateProductList()
    {
        DataTable productList = new DataTable();
        productList.Columns.Add("ProductId", typeof(int));
        productList.Columns.Add("ProductName", typeof(string));
        productList.Columns.Add("Quantity", typeof(int));
        Page.ViewState["ProductList"] = productList;
        return productList;
    }
    private void AddProduct(Inventory inventory, string product)
    {
        DataTable productList = (DataTable)Page.ViewState["ProductList"];
        productList.Rows.Add(inventory.ProductId, product, inventory.Quantity);
    }
    private void BindGrid()
    {
        //
        // Bind Grid
        //
        grdInventory.DataSource = (DataTable)Page.ViewState["ProductList"];
        grdInventory.DataBind();
    }
    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        Form.DefaultButton = "";
        lblSuccess.Visible = false;

        if (Page.ViewState["ProductList"] == null)
        {
            CreateProductList();
            DataTable productList = (DataTable)Page.ViewState["ProductList"];
            productList.Rows.Add(0, "", 0);
            grdInventory.EditIndex = 0;
            Page.ViewState["ProductList"] = productList;
            BindGrid();
        }
    }
    protected void odsSupplier_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
    protected void odsDeposit_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["userId"] = User.Identity.UserId;
    }
    protected void grdInventory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (isInserting && e.Row.RowIndex == 0)
        {
            //
            // If the grid is in insert mode, this "if" turn the delete button of the grid
            // to visible = false, only showing the controls that have SAVE and CANCEL functions
            //
            int i = e.Row.Cells.Count;
            e.Row.Cells[i - 2].Visible = false;
            return;
        }

        GridView grid = sender as GridView;

        e.Row.Attributes["onclick"] = "";
        e.Row.Attributes["onclick"] = "";

        //
        // This action, gets all lines that are not currently edited and sets
        // the actions to only show the DELETE button, SAVE and CANCEL are now visible = false
        // and the DELETE button now, receives a confirm dialog, asking the user about delection.
        //

        if (e.Row.RowType == DataControlRowType.DataRow &&
            ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Normal))
        {
            if (e.Row.RowState != DataControlRowState.Edit)
            {
                int i = e.Row.Cells.Count;
                e.Row.Cells[i - 1].Visible = false;
                e.Row.Cells[i - 2].Attributes["Width"] = "1%";
                e.Row.Cells[i - 2].Attributes["Align"] = "center";
            }
        }

        //
        // Sets the visible = false to DELETE button on the edited row
        //

        else if (e.Row.RowType == DataControlRowType.DataRow &&
            ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit))
        {
            int i = e.Row.Cells.Count;
            e.Row.Cells[i - 2].Visible = false;
        }

        //
        // At last, do not show any text or buttons in the HEADER COLUMN of EDIT controls
        //
        else if (e.Row.RowType == DataControlRowType.Header)
        {
            int i = e.Row.Cells.Count;
            e.Row.Cells[i - 1].Visible = false;
        }
    }
    protected void grdInventory_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataTable productList = (DataTable)Page.ViewState["ProductList"];
        Inventory inv = new Inventory();
        ProductManager pManager = new ProductManager(this);

        var productName = (grdInventory.Rows[e.RowIndex].FindControl("txtProduct") as TextBox).Text;
        var product = pManager.GetProductByName(Company.CompanyId, productName);
        if (product != null)
        {
            inv.ProductId = product.ProductId;
            inv.Quantity = Convert.ToInt16((grdInventory.Rows[e.RowIndex].FindControl("txtQuantity") as TextBox).Text);
            AddProduct(inv, productName);
            BindGrid();
            e.Cancel = true;
        }
        else
        {
            ShowError("Esse produto ainda não foi cadastrado,<br />faça o cadastro na tela de produtos, e tente novamente<br /><br />");
            BindGrid();
        }

    }
    protected void grdInventory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable productList = (DataTable)Page.ViewState["ProductList"];
        productList.Rows.RemoveAt(e.RowIndex);
        Page.ViewState["ProductList"] = productList;
        BindGrid();
    }
    private bool CheckIfCanSale(int productId, int quantity)
    {
        InventoryManager iManager = new InventoryManager(this);
        //
        // Get the data of the respectively inventory
        //
        var prod = iManager.GetProductInventory(Company.CompanyId, productId, Deposit.DepositId);

        //
        // Compare the Inventory Quantity wich the Sale quantity, if the Sale Quantity are 
        // less than the Inventory, the Can Sale flag will be true, else the sale will be canceled
        //
        return quantity <= prod.Quantity;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable productList = (DataTable)Page.ViewState["ProductList"];
        Inventory inv = new Inventory();
        Product prod = new Product();
        Deposit dep = new Deposit();
        InventoryMoviment mov = new InventoryMoviment();
        InventoryManager iManager = new InventoryManager(this);
        CompanyManager cManager = new CompanyManager(this);

        if (productList.Rows.Count < 1)
        {
            ShowError("Deve haver pelo menos uma linha com dados do produto.");
            return;
        }
        foreach (DataRow row in productList.Rows)
        {
            if (row.Table.Rows.IndexOf(row) != 0)
            {
                if (!CheckIfCanSale(Convert.ToInt16(row["ProductId"]), Convert.ToInt16(row["Quantity"])))
                {
                    ShowError("O estoque da linha <b>" + Convert.ToInt16((row.Table.Rows.IndexOf(row)) + 1).ToString() + "</b> não pode ficar negativo<br />");
                    return;
                }
            }
        }




        productList.Rows.RemoveAt(0);

        dep = cManager.GetCurrentDeposit(User.Identity.UserId, Company.CompanyId);

        foreach (DataRow row in productList.Rows)
        {
            inv.CompanyId = Company.CompanyId;
            inv.DepositId = dep.DepositId;
            inv.ProductId = Convert.ToInt16(row["ProductId"]);
            inv.Quantity = Convert.ToInt16(row["Quantity"]);

            mov.CompanyId = inv.CompanyId;
            mov.DepositId = inv.DepositId;
            mov.DepositDestinationId = Convert.ToInt16(cboDeposit.SelectedValue);
            mov.ModifiedDate = DateTime.Now;
            mov.ProductId = inv.ProductId;
            mov.Quantity = inv.Quantity;

            iManager.InventoryDrop(inv, 0, (int)DropType.Transfer, null);
            iManager.InsertStockMovement(inv, mov, inv.Quantity, User.Identity.UserId);
        }
        InSuccess();
    }

    #endregion
}
