using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl.Web.Security;

[PermissionRequired("Devolution")]
public partial class Company_POS_Exchange : Vivina.Erp.SystemFramework.PageBase
{
    private DataTable CreateDevolution()
    {
        DataTable devolution = new DataTable();
        devolution.Columns.Add("ProductId", typeof(int));
        devolution.Columns.Add("ProductName", typeof(string));
        devolution.Columns.Add("Quantity", typeof(int));
        devolution.Columns.Add("UnitPrice", typeof(decimal));
        devolution.Columns.Add("UnitCost", typeof(decimal));
        Page.ViewState["Devolution"] = devolution;
        return devolution;
    }
    private void AddDevolution(int productId, string productName, int quantity, decimal unitPrice, decimal unitCost)
    {
        DataTable devolution = (DataTable)Page.ViewState["Devolution"];
        devolution.Rows.Add(productId, productName, quantity, unitPrice, unitCost);
    }
    private void BindGrid()
    {
        //
        // Bind Grid
        //
        grdDevolution.DataSource = (DataTable)Page.ViewState["Devolution"];
        grdDevolution.DataBind();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Context.Items["OK"] != null)
        {
            ShowError("Devolução Efetuada com Sucesso !");
        }
        if (!IsPostBack)
        {
            CreateDevolution();
        }
    }
    protected override void OnPreRenderComplete(EventArgs e)
    {
        base.OnPreRenderComplete(e);
        btnOK.Visible = (grdDevolution.Rows.Count > 0);
    }
    protected void btnAddSale_Click(object sender, ImageClickEventArgs e)
    {

        SaleManager sManager = new SaleManager(this);
        //PaymentMethodManager pManager = new PaymentMethodManager(this);
        DepositManager dManager = new DepositManager(this);

        Sale sale = sManager.GetSale((int)Company.MatrixId, Convert.ToInt32("0" + txtSaleNumber.Text.Replace("_","")));

        //
        // Verifica o número da nota, ele precisa existir, e ter o mesmo matrix ID
        //
        if (sale == null)
        {
            ShowError(Resources.Exception.nonExistentSale);
            pnlExchange.Visible = false;
        }
        //else
        //{
        //    //
        //    // Preenche as Grids de Pagamento e Produtos 
        //    //
        //    //pnlExchange.Visible = true;
        //    ViewState["SaleId"] = sale.SaleId;
        //    //DataTable payment = pManager.GetPaymentBySale(sale.CompanyId, sale.SaleId);
        //    DataTable productData = sManager.GetSaleProducts(sale.CompanyId, sale.SaleId);
        //    grdSaleItems.DataSource = productData;
        //    grdSaleItems.DataBind();
        //    //grdPaymentMethod.DataSource = payment;
        //    grdPaymentMethod.DataBind();

        //    //
        //    // Preenche a Combo com os Produtos disponíveis na nota
        //    // 
        //    cboProducts.DataSource = productData;
        //    cboProducts.DataTextField = "Name";
        //    cboProducts.DataValueField = "ProductId";
        //    cboProducts.DataBind();

        //    //
        //    // Preenche as Informações na tela
        //    //
        //    lblSaleDate.Text = sale.SaleDate.ToString();
        //    lblDiscount.Text = sale.Discount.ToString();
        //    //lblUserName.Text = payment.Rows[0]["Name2"].ToString();

        //    //
        //    // Verifica o depósito de origem
        //    // Foi utilizado o try, pois essa coluna não havia no banco, e esse try, 
        //    // evita uma tela amarela
        //    //
        //    try
        //    {
        //        lblSourceStore.Text = dManager.GetDeposit((int)sale.DepositId).Name;
        //    }
        //    catch
        //    {
        //        lblSourceStore.Text = "Dado não disponível";
        //    }

        //    //
        //    // Calcula o total da venda
        //    //
        //    Decimal total = (decimal)0;
        //    //for (int i = 0; i < payment.Rows.Count; i++)
        //    //{
        //    //    total += Convert.ToDecimal(payment.Rows[i]["Amount"]);
        //    //    lblSaleTotal.Text = total.ToString();
        //    //}

        //    //
        //    // Reseta a Grid de Devolução
        //    //
        //    lblDevoltionValue.Visible = false;
        //    txtDevolutionValue.Text = "0,00";
        //    txtDevolutionValue.Visible = false;
        //    ViewState["Devolution"] = CreateDevolution();
        //    BindGrid();
        //}
    }
    protected void btnDevolution_Click(object sender, ImageClickEventArgs e)
    {
        decimal devolutionValue = Convert.ToDecimal("0" + txtDevolutionValue.Text);

        if (txtQuantityIN.Text == "")
            txtQuantityIN.Text = "1";

        //
        // Varre a grid de produtos, na parte central da tela
        //
        for (int i = 0; i < grdSaleItems.Rows.Count; i++)
        {
            //
            // Verifica em qual linha da Grid se encontra o produto 
            // que pretendemos adicionar
            //
            if (Convert.ToInt16(grdSaleItems.DataKeys[i]["ProductId"]) == Convert.ToInt16(cboProducts.SelectedValue))
            {
                //
                // Verifica se a quantidade é menor ou igual ao que há na nota, para inserir os dados
                // na Grid de devolução
                // Se não for, exibe uma mensagem de erro na tela
                //                
                if (Convert.ToInt16(txtQuantityIN.Text) <= Convert.ToInt16(grdSaleItems.Rows[i].Cells[2].Text))
                {
                    //                    
                    // Varre a grid de produtos que serão devolvidos
                    //
                    for (int j = 0; j < grdDevolution.Rows.Count; j++)
                    {
                        //
                        // Verifica se esse produto já se encontra na grid
                        // Se não estiver adiciona o produto na grid
                        //
                        if (Convert.ToInt16(grdDevolution.DataKeys[j]["ProductId"]) == Convert.ToInt16(cboProducts.SelectedValue))
                        {
                            int quantity = Convert.ToInt16(grdDevolution.Rows[j].Cells[4].Text) +
                                Convert.ToInt16(txtQuantityIN.Text);
                            //
                            // Se ainda for possível adicionar mais do mesmo produto, ele será adicionado
                            // na mesma linha.
                            // Senão o usuário terá uma mensagem de erro na tela.
                            //
                            if (quantity <= Convert.ToInt16(grdSaleItems.Rows[i].Cells[2].Text))
                            {
                                grdDevolution.Rows[j].Cells[4].Text = quantity.ToString();
                                devolutionValue += Convert.ToDecimal(grdSaleItems.DataKeys[i]["UnitPrice"]) *
                                Convert.ToInt16(txtQuantityIN.Text);
                                txtDevolutionValue.Text = devolutionValue.ToString();
                                return;
                            }
                            else
                            {
                                ShowError("<br />Você pode devolver no máximo a quantidade que foi vendida !<br />");
                                return;
                            }
                        }
                    }
                    //
                    // Insere os items na Grid de Devolução
                    //
                    AddDevolution(Convert.ToInt16(cboProducts.SelectedValue),
                        cboProducts.SelectedItem.Text,
                        Convert.ToInt16(txtQuantityIN.Text),
                        Convert.ToDecimal(grdSaleItems.DataKeys[i]["UnitPrice"]),
                        Convert.ToDecimal(grdSaleItems.DataKeys[i]["UnitCost"])
                        );
                    BindGrid();
                    //
                    // Mostra o valor sugerido para o usuário, para a devolução
                    //
                    lblDevoltionValue.Visible = true;
                    txtDevolutionValue.Visible = true;
                    devolutionValue += Convert.ToDecimal(grdSaleItems.DataKeys[i]["UnitPrice"]) *
                        Convert.ToInt16(txtQuantityIN.Text);
                    txtDevolutionValue.Text = devolutionValue.ToString();
                }
                else
                {
                    ShowError("<br />Você pode devolver no máximo a quantidade que foi vendida !<br />");
                }
            }
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Inventory inv = new Inventory();        
        InventoryManager iManager = new InventoryManager(this);

        foreach (GridViewRow row in grdDevolution.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                inv = iManager.GetInventory(Company.CompanyId,
                    Convert.ToInt16(grdDevolution.DataKeys[row.RowIndex]["ProductId"]),
                    Deposit.DepositId);
                inv.RealCost = Convert.ToDecimal(grdDevolution.DataKeys[row.RowIndex]["UnitCost"]);
                inv.UnitPrice = Convert.ToDecimal(grdDevolution.DataKeys[row.RowIndex]["UnitPrice"]);
                inv.Quantity = Convert.ToInt16(grdDevolution.DataKeys[row.RowIndex]["Quantity"]);

                iManager.StockDeposit(inv, null, User.Identity.UserId);

                //his.CompanyId = Company.CompanyId;
                //his.DepositId = Deposit.DepositId;
                //his.DestinationDepositId = Deposit.DepositId;
                //his.InventoryEntryTypeId = (int)EntryType.Devolution;
                //his.LogDate = DateTime.Now;
                //his.CurrencyRateId = inv.CurrencyRateId;
                //his.FiscalNumber = inv.FiscalNumber;
                //his.Localization = inv.Localization;
                //his.MinimumRequired = inv.MinimumRequired;
                //his.ProductId = inv.ProductId;
                //his.Profit = inv.Profit;
                //his.Quantity = Convert.ToInt16(grdDevolution.DataKeys[row.RowIndex]["Quantity"]);
                //his.RealCost = Convert.ToDecimal(grdDevolution.DataKeys[row.RowIndex]["UnitCost"]);
                //his.UnitPrice = Convert.ToDecimal(grdDevolution.DataKeys[row.RowIndex]["UnitPrice"]);
                //his.SupplierId = inv.SupplierId;

                //iManager.InsertHistory(his);
            }
        }

        DropPayout dp = new DropPayout();
        DropPayoutManager dManager = new DropPayoutManager(this);
        dp.Amount = Convert.ToDecimal(txtDevolutionValue.Text);
        dp.Comment = "Devolução de items da venda nº " + txtSaleNumber.Text;
        dp.CompanyId = Company.CompanyId;
        dp.DepositId = Deposit.DepositId;
        dp.ModifiedDate = DateTime.Now;
        dp.UserId = User.Identity.UserId;

        dManager.Insert(dp);

        Context.Items["OK"] = "OK";
        Server.Transfer("Exchange.aspx");
    }
}
