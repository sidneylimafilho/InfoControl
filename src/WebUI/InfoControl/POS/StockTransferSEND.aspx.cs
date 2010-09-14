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
using InfoControl.Web.Security;

[PermissionRequired("StockTransfer")]
public partial class Company_Stock_TransferSend : Vivina.Erp.SystemFramework.PageBase
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
    protected void odsCompany_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["matrixId"] = (int)Company.MatrixId;
    }
    protected void odsDeposit_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Convert.ToInt16(cboCompany.SelectedValue);
    }
    protected void cboCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        cboDeposit.DataBind();
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
        var product = pManager.GetProductByName((int)Company.MatrixId, productName);
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
            ShowError(Resources.Exception.nonexistentProduct);
            BindGrid();
        }

    }
    protected void grdInventory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        DataTable productList = (DataTable)Page.ViewState["ProductList"];
        if (productList.Rows.Count > 1)
        {
            //
            // Changed the instruction below that was deleting the selected index - 1
            // causing the wrong element to be erased in case of 2 or more items.
            // Original code: productList.Rows.RemoveAt(e.RowIndex - 1);
            //
            productList.Rows.RemoveAt(e.RowIndex);
            Page.ViewState["ProductList"] = productList;
            BindGrid();
        }
        else
        {
            productList.Rows.Clear();
            Server.Transfer("StockTransferSend.aspx");
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //Manager
        InventoryManager iManager = new InventoryManager(this);
        CompanyManager cManager = new CompanyManager(this);

        //these Objects are Initialized in loop because its necessary provide the Insert with new objects
        Inventory inv;
        InventoryMoviment mov;
        InventoryHistory his;

        DataTable productList = (DataTable)Page.ViewState["ProductList"];
        Inventory inventoryData = new Inventory();
        Product prod = new Product();
        Deposit dep = new Deposit();

        if (productList.Rows.Count <= 1)
        {
            ShowError(Resources.Exception.SelectProduct);
            return;
        }

        productList.Rows.RemoveAt(0);

        dep = cManager.GetCurrentDeposit(User.Identity.UserId, Company.CompanyId);
        try
        {
            foreach (DataRow row in productList.Rows)
            {
                inv = new Inventory();

                if (Deposit != null)
                    inv.DepositId = dep.DepositId;

                inv.CompanyId = Company.CompanyId;
                inv.ProductId = Convert.ToInt16(row["ProductId"]);
                inv.Quantity = Convert.ToInt16(row["Quantity"]);

                mov = new InventoryMoviment();
                mov.CompanyId = inv.CompanyId;
                mov.DepositId = inv.DepositId;
                mov.DepositDestinationId = Convert.ToInt16(cboDeposit.SelectedValue);
                mov.ModifiedDate = DateTime.Now;
                mov.ProductId = inv.ProductId;
                mov.Quantity = inv.Quantity;
                mov.CompanyDestinationId = Convert.ToInt16(cboCompany.SelectedValue);
                mov.Refused = false;

                if (Deposit != null)
                {
                    iManager.InsertStockMovement(inv, mov, Convert.ToInt32(row["Quantity"]), User.Identity.UserId);
                }
                else
                {
                    ShowError("O usuário não está vinculado a nenhum inventário!");
                    return;
                }

                Session["CompanySender"] = Company.CompanyId;
            }
        }
        catch (InvalidOperationException ex)
        {
            ShowError(Resources.Exception.InvalidStockQuantity);
            return;
        }
        InSuccess();
    }

    #endregion
}
