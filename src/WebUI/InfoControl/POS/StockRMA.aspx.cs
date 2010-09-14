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

[PermissionRequired("RMA")]
public partial class Company_POS_StockRMA : Vivina.Erp.SystemFramework.PageBase
{
    #region Functions
    public bool isInserting = false;
    private bool CheckIfCanSale(int productId, int quantity)
    {
        return true;
        //Deposit dep = new Deposit();
        //CompanyManager cManager = new CompanyManager(this);
        //InventoryManager iManager = new InventoryManager(this);

        ////
        //// Get the data of the respectively inventory
        ////
        //var prod = iManager.GetProductInventory(productId, Deposit.DepositId);

        ////
        //// Compare the Inventory Quantity wich the Sale quantity, if the Sale Quantity are 
        //// less than the Inventory, the Can Sale flag will be true, else the sale will be canceled
        ////
        //if (quantity <= prod.Quantity) return true;
        //else return false;
    }
    private void InSuccess()
    {
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
        int Quantity = 0;
        for (int i = 0; i < productList.Rows.Count; i++)
        {
            if (productList.Rows[i]["ProductName"].ToString() == product)
            {
                Quantity = (int)productList.Rows[i]["Quantity"];
                productList.Rows[i].Delete();
            }
        }
        inventory.Quantity += Quantity;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        Form.DefaultButton = "";

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

    protected void grdInventory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable productList = (DataTable)Page.ViewState["ProductList"];
        productList.Rows.RemoveAt(e.RowIndex);
        Page.ViewState["ProductList"] = productList;
        BindGrid();
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable productList = (DataTable)Page.ViewState["ProductList"];
        Inventory inv = new Inventory();
        Inventory inventoryData = new Inventory();
        Product prod = new Product();
        InventoryRMA rma = new InventoryRMA();
        InventoryHistory his = new InventoryHistory();
        InventoryManager iManager = new InventoryManager(this);
        CompanyManager cManager = new CompanyManager(this);

        if (productList.Rows.Count <= 1)
        {
            ShowError(Resources.Exception.InsertProduct);
            return;
        }

        foreach (DataRow row in productList.Rows)
        {
            if (row.Table.Rows.IndexOf(row) != 0)
            {
                if (!CheckIfCanSale(Convert.ToInt16(row["ProductId"]), Convert.ToInt16(row["Quantity"])))
                {
                    ShowError(Resources.Exception.InvalidStockQuantity + " Linha: " + Convert.ToInt16((row.Table.Rows.IndexOf(row)) + 1).ToString());
                    return;
                }
            }
        }

        productList.Rows.RemoveAt(0);

        try
        {
            foreach (DataRow row in productList.Rows)
            {
                inv.CompanyId = Company.CompanyId;
                inv.DepositId = Deposit.DepositId;
                inv.ProductId = Convert.ToInt16(row["ProductId"]);
                inv.Quantity = Convert.ToInt16(row["Quantity"]);
                inventoryData = iManager.GetProductInventory(Company.CompanyId, inv.ProductId, inv.DepositId);
                if (inventoryData != null)
                {
                    rma.CompanyId = inv.CompanyId;
                    rma.DepositId = inv.DepositId;
                    rma.ModifiedDate = DateTime.Now;
                    rma.ProductId = inv.ProductId;
                    rma.Quantity = inv.Quantity;
                    rma.Substituted = false;
                    rma.SupplierId = Convert.ToInt16(inventoryData.SupplierId);
                    iManager.InventoryDrop(inv, inv.Quantity, (int)DropType.RMA, null);
                    iManager.InsertStockRMA(rma);
                }
            }
            InSuccess();
        }
        catch (Exception ex)
        {
            ShowError(Resources.Exception.nonSentProductToRMA);
        }
    }
    protected void grdInventory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdInventory.EditIndex = 0;
        BindGrid();
    }
}
