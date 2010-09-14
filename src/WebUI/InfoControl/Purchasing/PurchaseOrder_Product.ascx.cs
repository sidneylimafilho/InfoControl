using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl.Web;
using InfoControl;
using InfoControl.Web.Mail;
using InfoControl.Web.UI;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


using Exception = Resources.Exception;

public partial class Company_POS_PurchaseOrder_Product : Vivina.Erp.SystemFramework.UserControlBase<Company_POS_PurchaseOrder>
{
    public List<PurchaseOrderQuotedItem> ProductList
    {
        get
        {
            if (Page.ViewState["_productList"] == null)
                Page.ViewState["_productList"] = new List<PurchaseOrderQuotedItem>();
            return Page.ViewState["_productList"] as List<PurchaseOrderQuotedItem>;
        }
        set { Page.ViewState["_productList"] = value; }
    }

    private void BindGrid()
    {
        PurchaseOrderDecision purchaseOrderDecision;
        switch (Convert.ToInt32(cboPurchaseOrderDecision.SelectedValue))
        {
            case (Int32)PurchaseOrderDecision.BestDeadline:
                purchaseOrderDecision = PurchaseOrderDecision.BestDeadline;
                break;
            case (Int32)PurchaseOrderDecision.LowTotalPrice:
                purchaseOrderDecision = PurchaseOrderDecision.LowTotalPrice;
                break;
            default:
                purchaseOrderDecision = PurchaseOrderDecision.LowUnitPrice;
                break;
        }
        ProductList = Page.Manager.GetPurchaseOrderQuotedItems(Page.Company.CompanyId,
                                                               Page.PurchaseOrder.PurchaseOrderId,
                                                               purchaseOrderDecision);
        grdProducts.DataSource = ProductList;
        grdProducts.DataBind();
    }

    /*
        private PurchaseOrderQuotedItem ConvertPurchaseRequestItemToPurchaseOrderProduct(
            PurchaseRequestItem purchaseRequestItem)
        {
            String productName = purchaseRequestItem.Product != null ? purchaseRequestItem.Product.Name : String.Empty;
            return new PurchaseOrderQuotedItem(productName, Convert.ToInt32(purchaseRequestItem.Amount), "", Decimal.Zero, 0);
        }
    */

    protected void odsSupplierContacts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

        e.InputParameters["companyID"] = Page.Company.CompanyId;
        e.InputParameters["supplierID"] = selSupplier.SupplierId;

    }

    protected void cboPurchaseOrderDecision_TextChanged(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(cboPurchaseOrderDecision.SelectedValue))
            return;

        BindGrid();
    }

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ProductList = null;
            grdProducts.DataBind();

            txtPurchaseOrderCode.Text = Page.PurchaseOrder.PurchaseOrderCode;

            ShowPurchaseControlStatus(Page.PurchaseOrder);

            //
            // Check if PurchaseOrder is loaded
            //
            if (Page.PurchaseOrder.PurchaseOrderId > 0)
            {
                //txtPurchaseOrderCode.Text = Page.PurchaseOrder.PurchaseOrderCode;

                ListItem listItem =
                    cboPurchaseOrderDecision.Items.FindByValue(Convert.ToString(Page.PurchaseOrder.PurchaseOrderDecision));

                if (listItem != null)
                    cboPurchaseOrderDecision.SelectedValue = listItem.Value;

                ProductList = Page.Manager.GetPurchaseOrderQuotedItems(Page.Company.CompanyId,
                                                                       Page.PurchaseOrder.PurchaseOrderId,
                                                                       PurchaseOrderDecision.LowUnitPrice);
                BindGrid();
            }
        }

        if (Convert.ToBoolean(Context.Items["PurchaseRequest"]))
            ConvertPurchaseRequestItemsInPurchaseOrder();

        if (Page.PurchaseOrder.Quotations.Count == 0)
            cboPurchaseOrderDecision.Enabled = false;

        fdsSentToSupplier.Visible = false;
        if (Page.PurchaseOrder.PurchaseOrderStatusId == PurchaseOrderStatus.SentToSupplier || Page.PurchaseOrder.PurchaseOrderStatusId == PurchaseOrderStatus.InProcess)
            fdsSentToSupplier.Visible = true;

        fdsDecision.Visible = false;
        if (Page.PurchaseOrder.PurchaseOrderStatusId == PurchaseOrderStatus.Approved || Page.PurchaseOrder.PurchaseOrderStatusId == PurchaseOrderStatus.Bought || Page.PurchaseOrder.PurchaseOrderStatusId == PurchaseOrderStatus.Concluded)
        {
            fdsDecision.Visible = true;
            if (Page.PurchaseOrder.SupplierId.HasValue)
            {
                cboSupplierWinner.DataBind();
                cboSupplierWinner.SelectedValue = Page.PurchaseOrder.SupplierId.ToString();
                cboSupplierWinner.Enabled = false;
                cboSupplierWinner_SelectedIndexChanged(this, e);
            }
        }
    }

    private void ConvertPurchaseRequestItemsInPurchaseOrder()
    {
        txtPurchaseOrderCode.Text = Page.PurchaseOrder.PurchaseOrderCode;
        var productManager = new ProductManager(this);
        var purchaseOrderManager = new PurchaseOrderManager(this);

        foreach (string requestItem in Request["requestItems"].Split(','))
        {
            string[] item = requestItem.Trim().Split('|');
            int productId = Convert.ToInt32(item[0]);
            int productPackageId = Convert.ToInt32("0" + item[1]);
            int? productManufacturerId = String.IsNullOrEmpty(item[2])
                                             ? (int?)null
                                             : Convert.ToInt32("0" + item[2]);
            int requestAmount = Convert.ToInt32("0" + item[3]);
            //string centerCost = item[4];
            int cityId = Convert.ToInt32("0" + item[5]);
            int purchaseRequestItemId = Convert.ToInt32("0" + item[6]);
            int purchaseRequestId = Convert.ToInt32("0" + item[7]);

            if (purchaseRequestItemId > 0)
                ProductList.Add(
                    new PurchaseOrderQuotedItem(
                        item[item.Length - 1].DecryptFromHex(),
                        requestAmount,
                        requestAmount,
                        "",
                        0m,
                        productId,
                        productPackageId,
                        productManufacturerId,
                        purchaseRequestItemId,
                        null,
                        0));
            else
                foreach (var prItem in purchaseOrderManager.GetPurchaseRequestItems(purchaseRequestId))
                    ProductList.Add(
                        new PurchaseOrderQuotedItem(
                            prItem.Product.Name,
                            Convert.ToInt32(prItem.Amount.Value),
                            Convert.ToInt32(prItem.Amount.Value),
                            "",
                            0m,
                            prItem.ProductId,
                            prItem.ProductPackageId.Value,
                            prItem.ProductManufacturerId,
                            prItem.PurchaseRequestItemId,
                            null,
                            0));
        }
        Page.SavePurchaseOrder();

        BindGrid();
    }

    private void AddProduct(String productName, Product product, Int32 quantity)
    {
        ProductList.Add(new PurchaseOrderQuotedItem(productName, quantity, quantity, String.Empty, Decimal.Zero, product.ProductId, 0, 0, 0, null, 0));
        BindGrid();
    }

    protected void grdProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Page.Manager.DeletePurchaseOrderItem(ProductList[e.RowIndex].PurchaseOrderItemId);
        Page.SavePurchaseOrder();
        BindGrid();
    }

    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {
        if (selProduct.Product == null)
        {
            Page.ShowError("Produto inexistente!");
            return;
        }

        if (selProduct.Product.RequiresAuthorization.Value && !Page.Employee.CentralBuyer.Value)
        {
            Page.ShowError(
                "Há produtos que necessitam de autorização, este processo de compra não poderá ser finalizado!");
            Page.Wizard.DisplaySideBar = false;
            Page.Wizard.Enabled = false;
            return;
        }

        AddProduct(selProduct.Name, selProduct.Product, ucCurrFieldQuantityData.IntValue);

        selProduct.Name = "";
        ucCurrFieldQuantityData.CurrencyValue = null;
    }

    protected void SelSupplier_SelectedSupplier(object sender, SelectedSupplierEventArgs e)
    {
        if (e.Supplier == null)
            return;

        var contactManager = new ContactManager(this);
        cboSupplierContacts.DataSource = contactManager.GetContactsBySupplier(Page.Company.CompanyId, e.Supplier.SupplierId);
        cboSupplierContacts.DataBind();
        pnlSupplierContacts.Visible = cboSupplierContacts.Items.Count > 0;
    }

    protected void BtnSendQuotationRequest_Click(object sender, EventArgs e)
    {

        if (Page.SavePurchaseOrder(PurchaseOrderStatus.SentToSupplier) == null)
            return;

        var purchaseOrderManager = new PurchaseOrderManager(this);

        //
        //Este pedaço de código, verifica se tem modelo de ordem de compra cadastrado no sistema 
        //
        DocumentTemplate documentTemplate = purchaseOrderManager.GetSupplyAuthorizationDocumentTemplate(Page.Company.CompanyId);

        foreach (var item in Page.PurchaseOrder.PurchaseOrderItems)
            if (item.ProductPackage.RequiresQuotationInPurchasing)
                documentTemplate = purchaseOrderManager.GetBudgetRequestDocumentTemplate(Page.Company.CompanyId);

        if (documentTemplate == null)
        {
            Page.ShowError(Exception.PurchaseDocumentTemplateIsNull);
            return;
        }

        if (selSupplier.Supplier == null)
        {
            Page.ShowError("Selecione um fornecedor!");
            return;
        }

        if (String.IsNullOrEmpty(cboSupplierContacts.SelectedValue))
        {
            Page.ShowError("O contato selecionado não possui e-mail!");
            return;
        }

        Contact contact = new ContactManager(this).GetContact(Convert.ToInt32(cboSupplierContacts.SelectedValue));
        if (contact == null || String.IsNullOrEmpty(contact.Email))
        {
            Page.ShowError("O contato selecionado não possui e-mail!");
            return;
        }

        var message = new MailMessage(Page.Company.LegalEntityProfile.Email,
                                      contact.Email,
                                      "Solicitação de Orçamento",
                                      ApplyDocumentTemplate(Page.PurchaseOrder, documentTemplate));
        message.IsBodyHtml = true;
        message.ReplyTo = new MailAddress(Page.User.Identity.UserName);
        message.BodyEncoding = System.Text.Encoding.UTF8;
        Postman.Send(message);

        selSupplier.ShowSupplier(null);

        Page.ShowAlert("E-mail enviado!");
    }

    protected void btnDownloadRequestBudget_Click(object sender, EventArgs e)
    {
        if (Page.SavePurchaseOrder() == null)
            return;

        var purchaseOrderManager = new PurchaseOrderManager(this);

        //
        //Este pedaço de código, verifica se tem modelo de ordem de compra cadastrado no sistema 
        //
        DocumentTemplate documentTemplate = purchaseOrderManager.GetSupplyAuthorizationDocumentTemplate(Page.Company.CompanyId);

        foreach (var item in Page.PurchaseOrder.PurchaseOrderItems)
            if (item.ProductPackage.RequiresQuotationInPurchasing)
            {
                documentTemplate = purchaseOrderManager.GetBudgetRequestDocumentTemplate(Page.Company.CompanyId);
                if (documentTemplate == null)
                {
                    Page.ShowError(Exception.BudgetDocumentTemplateIsNull);
                    return;
                }
            }

        if (documentTemplate == null)
        {
            Page.ShowError(Exception.SupplyAuthorizationTemplateIsNull);
            return;
        }

        ExportDocumentTemplate("SolicitacaoOrcamento" + Page.PurchaseOrder.PurchaseOrderCode, documentTemplate);
    }

    private void ExportDocumentTemplate(string fileName, DocumentTemplate documentTemplate)
    {
        //
        // Limpa os buffers de resposta, para não enviar os cabeçalhos da página ASPX
        //
        Response.Clear();
        Response.ContentType = "text/doc";
        //
        // Seta o cabeçalho que irá dizer ao navegador que é para fazer download, ao invés de apresentar na tela.
        //
        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".doc");
        //
        // Indica que o formato do arquivo será o mesmo do modelo
        //
        Response.ContentEncoding = Encoding.UTF8;


        //
        // Aplica as trocas das máscaras no modelo, ou seja, substitui os []'s pelo conteúdo e retorna
        //
        var contentFile = ApplyDocumentTemplate(Page.PurchaseOrder, documentTemplate);
        //
        // Envia o arquivo para o cliente
        //
        Response.Write(contentFile);

        //
        // Interrompe o processamento da página para que o conteúdo do modelo não se misture
        // ao HTML da página PurchaseOrder.aspx
        //
        Response.End();
    }

    private string ApplyDocumentTemplate(PurchaseOrder order, DocumentTemplate documentTemplate)
    {
        //
        // Busca a localização do modelo no servidor e retorna seu conteúdo
        //
        string contentFile = File.ReadAllText(Server.MapPath(documentTemplate.FileUrl));

        //
        // Aplica as trocas das máscaras no modelo, ou seja, substitui os []'s pelo conteúdo e retorna
        //
        return new PurchaseOrderManager(this).ApplyPurchaseOrderInDocumentTemplate(Page.PurchaseOrder, contentFile);

    }

    #endregion

    #region Decision

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        if (Page.SavePurchaseOrder(PurchaseOrderStatus.Approved) == null)
            return;

        Server.Transfer("PurchaseOrders.aspx");
    }

    protected void btnReprove_Click(object sender, EventArgs e)
    {
        if (Page.SavePurchaseOrder(PurchaseOrderStatus.Reproved) == null)
            return;

        Server.Transfer("PurchaseOrders.aspx");
    }

    protected void btnDownloadPurchaseOrder_Click(object sender, EventArgs e)
    {
        //
        //verifica se tem modelo de ordem de compra cadastrado no sistema 
        //
        DocumentTemplate documentTemplate = new PurchaseOrderManager(this).GetPurchaseOrderDocumentTemplate(Page.Company.CompanyId);
        if (documentTemplate == null)
        {
            Page.ShowError(Exception.PurchaseDocumentTemplateIsNull);
            return;
        }

        Page.ViewState["SupplierId"] = cboSupplierWinner.SelectedValue;

        if (Page.SavePurchaseOrder(PurchaseOrderStatus.Bought) == null)
            return;

        ExportDocumentTemplate("OrdemDeCompra-" + Page.PurchaseOrder.PurchaseOrderCode, documentTemplate);
    }

    protected void btnSendPurchaseOrderByMail_Click(object sender, EventArgs e)
    {
        Page.ViewState["SupplierId"] = cboSupplierWinner.SelectedValue;

        if (Page.SavePurchaseOrder(PurchaseOrderStatus.Bought) == null)
            return;

        //
        //Este pedaço de código, verifica se tem modelo de ordem de compra cadastrado no sistema 
        //
        DocumentTemplate documentTemplate = new PurchaseOrderManager(this).GetPurchaseOrderDocumentTemplate(Page.Company.CompanyId);
        if (documentTemplate == null)
        {
            Page.ShowError(Exception.PurchaseDocumentTemplateIsNull);
            return;
        }

        Contact contact = new ContactManager(this).GetContact(Convert.ToInt32(cboContactWinner.SelectedValue));
        if (contact == null || String.IsNullOrEmpty(contact.Email))
        {
            Page.ShowError("O contato selecionado não possui e-mail!");
            return;
        }

        var message = new MailMessage(Page.Company.LegalEntityProfile.Email,
                                      contact.Email,
                                      "Ordem de Compra",
                                      ApplyDocumentTemplate(Page.PurchaseOrder, documentTemplate));
        message.IsBodyHtml = true;
        message.ReplyTo = new MailAddress(Page.User.Identity.UserName);
        message.BodyEncoding = System.Text.Encoding.Default;
        Postman.Send(message);

        Page.ShowAlert("E-mail enviado!");
    }

    private void ShowPurchaseControlStatus(PurchaseOrder purchaseOrder)
    {
        actionsViews.ActiveViewIndex = 0;
        if (purchaseOrder != null)
            switch (purchaseOrder.PurchaseOrderStatusId)
            {
                case PurchaseOrderStatus.InProcess:
                case PurchaseOrderStatus.Reproved:
                case PurchaseOrderStatus.SentToSupplier:
                    actionsViews.ActiveViewIndex = 0;
                    break;
                case PurchaseOrderStatus.WaitingforApproval:
                    actionsViews.ActiveViewIndex = 0;
                    if (Page.PurchaseOrder.ApproverUserId == Page.User.Identity.UserId)
                        actionsViews.ActiveViewIndex = 1;
                    break;
                case PurchaseOrderStatus.Approved:
                    actionsViews.ActiveViewIndex = 2;
                    break;
                case PurchaseOrderStatus.Concluded:
                case PurchaseOrderStatus.Bought:
                    actionsViews.ActiveViewIndex = 3;

                    grdProducts.Columns[4].Visible = true;

                    selEmployee.EmployeeId = Page.PurchaseOrder.ReceiptEmployeeId;
                    txtReceiptNumber.Text = Page.PurchaseOrder.ReceiptNumber;
                    ucReceiptTotalValue.CurrencyValue = Page.PurchaseOrder.ReceiptTotalValue;
                    datReceiptDate.DateTime = Page.PurchaseOrder.ReceiptDate;

                    btnSentToDeposit.Visible = Page.PurchaseOrder.PurchaseOrderStatusId == PurchaseOrderStatus.Bought;
                    break;
            }
    }

    protected void odsSuppliers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Page.Company.CompanyId;
        e.InputParameters["purchaseOrderId"] = Page.PurchaseOrder.PurchaseOrderId;
    }

    #endregion

    protected void cboSupplierWinner_SelectedIndexChanged(object sender, EventArgs e)
    {
        var contactManager = new ContactManager(this);
        cboContactWinner.DataSource = contactManager.GetContactsBySupplier(Page.Company.CompanyId,
                                                                              Page.PurchaseOrder.SupplierId ?? Convert.ToInt32(cboSupplierWinner.SelectedValue));
        cboContactWinner.DataBind();
    }

    protected void btnSentToDeposit_Click(object sender, EventArgs e)
    {
        if (datReceiptDate.DateTime < DateTime.Now.Date)
        {
            Page.ShowError("A data de recebimento da Nota fiscal deve ser maior que hoje!");
            return;
        }

        if (selEmployee.EmployeeId == null)
        {
            Page.ShowError("É necessário selecionar o empregado que recebeu a nota!");
            return;
        }

        if (String.IsNullOrEmpty(txtReceiptNumber.Text))
        {
            Page.ShowError("Faltou inserir o numero da nota fiscal!");
            return;
        }

        Page.PurchaseOrder.ReceiptDate = datReceiptDate.DateTime;
        Page.PurchaseOrder.ReceiptTotalValue = ucReceiptTotalValue.CurrencyValue;
        Page.PurchaseOrder.ReceiptNumber = txtReceiptNumber.Text;
        Page.PurchaseOrder.ReceiptEmployeeId = selEmployee.EmployeeId;

        foreach (GridViewRow row in grdProducts.Rows)
        {
            CurrencyField ucQtdReceived = row.FindControl<CurrencyField>("ucQtdReceived") ?? new CurrencyField();
            Page.PurchaseOrder.PurchaseOrderItems[row.RowIndex].QuantityReceived -= ucQtdReceived.IntValue;
        }

        if (Page.SavePurchaseOrder(PurchaseOrderStatus.Concluded) == null)
            return;
    }


}