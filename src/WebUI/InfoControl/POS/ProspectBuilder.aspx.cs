using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Vivina.Erp.SystemFramework;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules.Services;
using System.Collections.Generic;
using System.IO;

[PermissionRequired("ProspectBuilder")]
public partial class Company_POS_ProspectBuilder : Vivina.Erp.SystemFramework.PageBase
{
    Customer customer;

    SaleManager saleManager;
    CustomerManager customerManager;

    private DataTable _additionalValues = new DataTable();
    public Decimal PerLineToTal = Decimal.Zero;
    public decimal total = Decimal.Zero;
    private Int32 _budgetId = 0;

    public Int32 BudgetId
    {
        get
        {
            if (Session["BudgetId"] != null)
                _budgetId = Convert.ToInt32(Session["BudgetId"]);
            return _budgetId;
        }

        set
        {
            Session["BudgetId"] = value;
            _budgetId = Convert.ToInt32(Session["BudgetId"]);
        }
    }


    public List<BudgetItem> BudgetItemList
    {
        get
        {
            if (Session["BudgetItemList"] == null)
                Session["BudgetItemList"] = new List<BudgetItem>();

            return Session["BudgetItemList"] as List<BudgetItem>;
        }
        set
        {
            Session["BudgetItemList"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Initialize variables
        prospectComments.HomePath = Company.GetFilesDirectory();

        saleManager = new SaleManager(this);
        customerManager = new CustomerManager(this);

        if (!IsPostBack)
        {
            BudgetItemList = null;

            if (!String.IsNullOrEmpty(Request["BudgetId"]))
                BudgetId = Convert.ToInt32(Request["BudgetId"]);

            cboVendor.DataSource = new HumanResourcesManager(this).GetSalesPerson(Company.CompanyId);
            cboVendor.DataBind();

            if (BudgetId != 0)
                ShowBudget(BudgetId);
        }
    }

    private void ClearFields()
    {
        ucCurrFieldUnitPrice.CurrencyValue = null;
        ucCurrFieldQuantityData.CurrencyValue = null;
    }

    /// <summary>
    /// This method loads budgetItems objects to grid  
    /// </summary>
    /// </summary>
    private void LoadBudgetItemsToGrid()
    {
        BudgetItemList = null;
        foreach (var item in saleManager.GetBudgetItemByBudget(BudgetId, Company.CompanyId))
        {
            var budgetItem = new BudgetItem();

            budgetItem.BudgetId = item.BudgetId;
            budgetItem.BudgetItemId = item.BudgetItemId;
            budgetItem.ModifiedDate = item.ModifiedDate;
            budgetItem.Observation = item.Observation;

            if (item.ProductId.HasValue)
                budgetItem.SpecialProductName = item.Product.Name;
            else if (item.ServiceId.HasValue)
                budgetItem.SpecialProductName = item.Service.Name;
            else
                budgetItem.SpecialProductName = item.SpecialProductName;

            budgetItem.ProductCode = item.ProductId.HasValue ? item.Product.ProductCode : item.ProductCode;

            budgetItem.ProductDescription = item.ProductDescription;
            budgetItem.ProductId = item.ProductId;
            budgetItem.ServiceId = item.ServiceId;
            budgetItem.UnitCost = item.UnitCost;
            budgetItem.UnitPrice = item.UnitPrice;
            budgetItem.Quantity = item.Quantity;
            budgetItem.Reference = item.Reference;

            BudgetItemList.Add(budgetItem);
        }

        grdProducts.DataSource = BudgetItemList;
        grdProducts.DataBind();

    }

    private void ShowBudget(Int32 budgetId)
    {
        //
        //Load customer's informations
        //

        var originalBudget = saleManager.GetBudget(budgetId, Company.CompanyId);

        LoadBudgetItemsToGrid();

        txtBudgetCode.Text = originalBudget.BudgetCode;

        cboVendor.SelectedValue = originalBudget.VendorId.ToString();

        if (originalBudget.Customer != null)
            sel_customer.ShowCustomer(originalBudget.Customer);

        txtCustomerName.Text = originalBudget.CustomerName;
        txtCustomerMail.Text = originalBudget.CustomerMail;
        txtPhone.Text = originalBudget.CustomerPhone;

        //
        //Load product's informations
        //


        txtDiscount.Attributes["onkeyup"] = "CalculateDiscount();";

        var budget = new SaleManager(this).GetBudget(budgetId, Company.CompanyId);
        Product product = null;
        Service service = null;

        prospectComments.SubjectId = budgetId;

        foreach (var item in budget.BudgetItems)
        {
            product = new Product { ProductId = 0, Name = item.SpecialProductName, ProductCode = item.ProductCode };

            if (item.ProductId.HasValue)
                product.CopyPropertiesFrom(item.Product);
            else if (item.ServiceId.HasValue)
                service = new ServicesManager(this).GetService(item.ServiceId.Value);

            ucCurrFieldUnitPrice.CurrencyValue = item.UnitPrice;
            ucCurrFieldQuantityData.CurrencyValue = item.Quantity;

            ClearFields();
        }

        txtObservation.Text = budget.Observation;
        txtContactName.Text = budget.ContactName;
        txtDeliveryDate.Text = budget.DeliveryDate;
        txtWarrant.Text = budget.Warranty;
        ucCurrFieldExpirationDate.CurrencyValue = budget.ExpirationDate;
        txtPaymentMethod.Text = budget.PaymentMethod;
        txtDeliveryDescription.Text = budget.DeliveryDescription;
        txtObservation.Text = budget.Observation;
        txtTreatment.Text = budget.Treatment;
        txtDiscount.Text = Convert.ToString(budget.Discount);

        ucCurrFieldAdditionalCost.CurrencyValue = budget.AdditionalCost;

    }

    protected void grdProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (grdProducts.Rows.Count == 0)
            PerLineToTal = total = Decimal.Zero;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            PerLineToTal = Convert.ToInt32(e.Row.Cells[2].Text) * Convert.ToDecimal(e.Row.Cells[4].Text);
            e.Row.Cells[5].Text = String.Format("{0:c}", PerLineToTal);
            total += PerLineToTal;
        }

        lblTotal.Text = total.ToString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (IsValidBudget())
        {
            SaveBudget();
            Response.Redirect("Prospects.aspx");
        }
    }

    /// <summary>
    /// This method validates the budget verifying informations about customer and produts 
    /// </summary>
    /// <param name="msg"></param>
    private bool IsValidBudget()
    {
        string msg = "";
        if (!sel_customer.CustomerId.HasValue)
        {
            if (String.IsNullOrEmpty(txtCustomerName.Text))
                msg = Resources.Exception.UnselectedCustomer;

            if (String.IsNullOrEmpty(txtPhone.Text.RemoveMask()))
                msg = "É necessário preencher o telefone!";
        }

        if (sel_customer.CustomerId.HasValue)
            if (String.IsNullOrEmpty(sel_customer.Customer.Phone) && String.IsNullOrEmpty(txtPhone.Text.RemoveMask()))
                msg = "É necessário preencher o telefone!";

        if (!BudgetItemList.Any())
            msg = "Selecione ao menos um produto ou serviço!";

        if (String.IsNullOrEmpty(cboVendor.SelectedValue))
            msg = "Não há vendedor associado!";

        if (!String.IsNullOrEmpty(msg))
        {
            ShowError(msg);
            return false;
        }

        return true;
    }

    private Budget SaveBudget()
    {
        var budget = saleManager.GetBudget(BudgetId, Company.CompanyId) ?? new Budget();

        //
        // Fill budget based on a new customer or a selected one 
        //
        if (sel_customer.CustomerId.HasValue)
        {
            budget.CustomerId = sel_customer.CustomerId;
            budget.CustomerName = null;

            budget.CustomerMail = !String.IsNullOrEmpty(sel_customer.Customer.Email) ? sel_customer.Customer.Email : txtCustomerMail.Text;
            budget.CustomerPhone = !String.IsNullOrEmpty(sel_customer.Customer.Phone) ? sel_customer.Customer.Phone : txtPhone.Text;
        }
        else if (!String.IsNullOrEmpty(txtCustomerName.Text))
        {
            budget.CustomerId = null;
            budget.CustomerName = txtCustomerName.Text;

            budget.CustomerMail = txtCustomerMail.Text;
            budget.CustomerPhone = txtPhone.Text;
        }

        //
        // Changed vendor to be required in budgets, some old records may not have one so the vendor is set
        // even in update mode
        //

        budget.AdditionalCost = ucCurrFieldAdditionalCost.CurrencyValue;

        var discountValue = decimal.Zero;

        if (!String.IsNullOrEmpty(txtDiscount.Text))
        {
            if (txtDiscount.Text.Contains("%"))
            {
                discountValue = Convert.ToDecimal(txtDiscount.Text.Replace("%", ""));
                budget.Discount = discountValue * Convert.ToDecimal(lblTotal.Text) / 100;
            }
            else
                budget.Discount = Convert.ToDecimal(txtDiscount.Text);
        }

        if (budget.BudgetId == 0)
            budget.CreatedByUser = User.Identity.UserName;
        else
            budget.ModifiedByUser = User.Identity.UserName;

        budget.VendorId = Convert.ToInt32(cboVendor.SelectedValue);
        budget.DeliveryDate = txtDeliveryDate.Text;
        budget.Warranty = txtWarrant.Text;
        budget.ExpirationDate = ucCurrFieldExpirationDate.IntValue;
        budget.PaymentMethod = txtPaymentMethod.Text;
        budget.ContactName = txtContactName.Text;
        budget.DeliveryDescription = txtDeliveryDescription.Text;
        budget.Observation = txtObservation.Text;
        budget.Treatment = txtTreatment.Text;
        budget.CompanyId = Company.CompanyId;
        budget.BudgetCode = txtBudgetCode.Text;

        return saleManager.SaveBudget(budget, BudgetItemList);
    }

    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {
        if (ucCurrFieldQuantityData.CurrencyValue == decimal.Zero)
        {
            ShowError("Quantidade não pode ser 0(zero)!");
            return;
        }

        BudgetItem budgetItem = new BudgetItem();
        Product product = SelProductAndService.Product.Duplicate();

        if (product.ProductId == 0)
            product.Name = SelProductAndService.Name;

        Service service = SelProductAndService.Service.Duplicate() ?? new Service();
        if (SelProductAndService.IsProduct)
        {
            if (pnlEdit.Visible && Convert.ToInt32(Page.ViewState["InsertItemID"]) == Convert.ToInt32(Page.ViewState["EditItemID"]))
            {
                product.Description = DescriptionTextBox.Value;
                pnlEdit.Visible = false;
            }

            Page.ViewState["InsertItemID"] = product.ProductId;
            budgetItem.ProductId = product.ProductId;
            budgetItem.SpecialProductName = product.Name;


            budgetItem.ProductDescription = pnlEdit.Visible ? DescriptionTextBox.Value : product.Description;
            pnlEdit.Visible = false;
        }

        if (SelProductAndService.IsService)
        {
            Page.ViewState["InsertItemID"] = service.ServiceId;
            budgetItem.ServiceId = service.ServiceId;
            budgetItem.SpecialProductName = service.Name;
            budgetItem.ProductDescription = pnlEdit.Visible ? DescriptionTextBox.Value : String.Empty;
            pnlEdit.Visible = false;
        }

        if (!SelProductAndService.IsProduct && !SelProductAndService.IsService)
            budgetItem.SpecialProductName = SelProductAndService.Name;

        var inventory = new Inventory();

        if (product != null && product.ProductId != 0)
        {
            if (Deposit != null)
                inventory = new InventoryManager(this).GetProductInventory(Company.CompanyId,
                                                                         product.ProductId,
                                                                         Deposit.DepositId);
        }

        budgetItem.UnitPrice = ucCurrFieldUnitPrice.CurrencyValue.Value;
        budgetItem.Quantity = ucCurrFieldQuantityData.IntValue;
        budgetItem.Reference = txtReference.Text;
        budgetItem.ProductCode = product.ProductCode;
        budgetItem.ModifiedDate = DateTime.Now;

        if (inventory != null)
            budgetItem.UnitCost = inventory.RealCost;

        BudgetItemList.Add(budgetItem);

        grdProducts.DataSource = BudgetItemList;
        grdProducts.DataBind();

        ClearFields();
        DescriptionTextBox.Value = String.Empty;
        SelProductAndService.ClearField();
    }

    protected void btnShowProductDescription_Click(object sender, ImageClickEventArgs e)
    {
        pnlEdit.Visible = false;
        Page.ViewState["EditItemID"] = 0;

        if (String.IsNullOrEmpty(SelProductAndService.Name))
        {
            ShowError(Resources.Exception.UnselectedProductOrService);
            return;
        }

        Product product = SelProductAndService.Product;
        Service service = SelProductAndService.Service;
        if (product == null && service == null)
        {
            ShowError(Resources.Exception.NonExistentProductOrService);
            return;
        }

        pnlEdit.Visible = true;
        if (product != null)
        {
            Page.ViewState["EditItemID"] = product.ProductId;
            DescriptionTextBox.Value = product.Description;
        }
        else if (service != null)
        {
            Page.ViewState["EditItemID"] = service.ServiceId;
            DescriptionTextBox.Value = service.Name;
        }
    }

    protected void btnModel_Click(object sender, EventArgs e)
    {
        //
        // Verify if there's a budget request model for that company
        //            
        if (String.IsNullOrEmpty(cboBudgetModels.SelectedValue))
        {
            ShowError(Resources.Exception.BudgetDocumentTemplateIsNull);
            return;
        }

        if (IsValidBudget())
        {
            var budget = SaveBudget();
            if (budget != null)
            {
                BudgetId = budget.BudgetId;
                var documentTemplate = new CompanyManager(this).GetDocumentTemplate(Convert.ToInt32(cboBudgetModels.SelectedValue));
                ExportDocumentTemplate(budget, "SolicitaçãoDeOrçamento" + budget.BudgetCode, documentTemplate);
            }
        }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        string msg = "";

        if (sel_customer.CustomerId.HasValue)
        {
            if (String.IsNullOrEmpty(sel_customer.Customer.Email) && String.IsNullOrEmpty(txtCustomerMail.Text))
                msg = "Cliente sem email!";

        }
        else if (String.IsNullOrEmpty(txtCustomerMail.Text))
            msg = "Cliente sem email!";

        if (!String.IsNullOrEmpty(msg))
        {
            ShowError(msg);
            return;
        }

        if (String.IsNullOrEmpty(cboBudgetModels.SelectedValue))
        {
            ShowError(Resources.Exception.BudgetDocumentTemplateIsNull);
            return;
        }

        if (IsValidBudget())
        {
            var budget = SaveBudget();
            BudgetId = budget.BudgetId;

            var budgetDocumentTemplate = new CompanyManager(this).GetDocumentTemplate(Convert.ToInt32(cboBudgetModels.SelectedValue));
            var directoryTempFile = String.Empty;

            //
            // Verifies if the budget will be send with a file attached
            //
            if (!budgetDocumentTemplate.FileName.Contains(".html") || !budgetDocumentTemplate.FileName.Contains(".htm"))
            {
                directoryTempFile = "C:\\Temp\\SolicitaçãoDeOrçamento" + budget.BudgetCode.Replace("/", "-").Replace("\\", "-") + ".rtf";

                try
                {
                    if (File.Exists(directoryTempFile))
                        File.Delete(directoryTempFile);

                    // creates the file and copies the content of budget
                    File.AppendAllText(directoryTempFile, saleManager.ApplyBudgetTemplate(budget, Convert.ToInt32(cboBudgetModels.SelectedValue)));

                    saleManager.SendBudgetToCustomer(budget.BudgetId, Company, Convert.ToInt32(cboBudgetModels.SelectedValue), directoryTempFile);
                    ShowAlert("A proposta " + budget.BudgetCode + " foi enviada ao cliente com sucesso!");

                    File.SetAttributes(directoryTempFile, FileAttributes.Temporary);
                    return;

                }
                catch (System.IO.IOException)
                { 
                    ShowError("Erro! Arquivo de modelo já está aberto por outro programa!");
                    return;
                }
            }

            //
            // budget in email body
            //
            saleManager.SendBudgetToCustomer(budget.BudgetId, Company, Convert.ToInt32(cboBudgetModels.SelectedValue), String.Empty);
            ShowAlert("A proposta " + budget.BudgetCode + " foi enviada ao cliente com sucesso!");
        }
    }

    protected void odsBudgetItens_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["budgetId"] = BudgetId;
    }

    protected void odsBudgetModels_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["documentTemplateTypeId"] = Convert.ToInt32(DocumentTemplateTypes.ProspectOrBudget);
    }

    private void ExportDocumentTemplate(Budget budget, string fileName, DocumentTemplate documentTemplate)
    {

        //
        // Clean buffers of response to not send headers of APSX pages
        // 
        Response.Clear();
        Response.ContentType = "text/doc";
        //
        // Sets the header that tells to browser to download not show in the screen
        // 
        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".doc");
        //
        // Indicate that file format will be the same model
        // 
        Response.ContentEncoding = System.Text.Encoding.Default;

        //
        // Apply the changes from model, changing []'s for the content
        // 
        Response.Write(saleManager.ApplyBudgetTemplate(budget, documentTemplate.DocumentTemplateId));

        //
        // Cut the page process to not merge the model content to the page HTML
        // 
        Response.End();
    }

    protected void grdProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BudgetItemList.Remove(BudgetItemList.ElementAt(e.RowIndex));
        grdProducts.DataSource = BudgetItemList;
        grdProducts.DataBind();
    }
}
