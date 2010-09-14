using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl.Web.Security;
using DiscountType = Vivina.Erp.BusinessRules.SaleManager.DiscountType;


[PermissionRequired("ProspectBuilder")]
public partial class Company_POS_Prospect : Vivina.Erp.SystemFramework.PageBase
{
    int index;
    decimal? subTotal;
    decimal? discount;
    decimal? ipiTax;
    decimal? ipi;
    decimal? additional;
    Budget budget;

    /// <summary>
    /// load data of budget for report
    /// </summary>
    public void loadDataOfBudget()
    {


        if (budget != null)
        {
            var humanResourcesManager = new HumanResourcesManager(this);

            lblProspectNumber.Text = "<b>Número da proposta :&nbsp;&nbsp;&nbsp;</b>" + budget.BudgetCode + "<br /><br />";
            if (budget.VendorId.HasValue)
                lblVendor.Text = "<b>Vendedor:</b>&nbsp;&nbsp;&nbsp;" + humanResourcesManager.GetEmployee(Company.CompanyId, Convert.ToInt32(budget.VendorId)).Profile.Name + "<br /><br />";
            //contact

            if (!String.IsNullOrEmpty(budget.ContactName))
                lblContactName.Text = "Aos cuidados.: " + budget.ContactName + "<br/><br/>";



            //show or hide the fieldset of supply
            bool showSupplierFieldset = true;
            if (!string.IsNullOrEmpty(budget.DeliveryDate))
            {
                lblEntrega.Text = budget.DeliveryDate;
                lblEntrega.Visible = lblDeliveryText.Visible = true;
            }
            else
                showSupplierFieldset = lblEntrega.Visible = lblDeliveryText.Visible = false;


            if (!string.IsNullOrEmpty(budget.Warranty))
            {
                lblGarantia.Text = budget.Warranty;
                lblEntrega.Visible = lblWarrantText.Visible = true;
            }
            else
                showSupplierFieldset = lblEntrega.Visible = lblWarrantText.Visible = false;

            if (!String.IsNullOrEmpty(budget.ExpirationDate.ToString()))
            {
                lblValidade.Text = budget.ExpirationDate.ToString() + " dia(s)";
                lblProspectValidate.Visible = lblValidade.Visible = true;
            }
            else
                showSupplierFieldset = lblProspectValidate.Visible = lblValidade.Visible = false;

            if (!String.IsNullOrEmpty(budget.PaymentMethod))
            {
                lblPagamento.Text = budget.PaymentMethod;
                lblPagamento.Visible = lblpayment.Visible = lblPaymentAditional.Visible = true;
            }
            else
                showSupplierFieldset = lblPagamento.Visible = lblpayment.Visible = lblPaymentAditional.Visible = false;

            if (!String.IsNullOrEmpty(budget.DeliveryDescription))
            {
                lblDeliveryDescription.Text = budget.DeliveryDescription;
                lblDeliveryDescription.Visible = lblDeliveryDescriptionText.Visible = true;
            }
            else
                showSupplierFieldset = lblDeliveryDescription.Visible = lblDeliveryDescriptionText.Visible = false;
            //set the visible of supply Fieldset
            pnlDelivery.Visible = showSupplierFieldset;

            lblOBS.Text = budget.Observation.Replace("\n", "<br>");

            //lblContato.Text = budget.ContactName;
            tblObs.Visible = budget.Observation.Length > 0;

            lblCover.Text = budget.Cover;
            lblSummary.Text = budget.Summary;

            lblAditional.Text = String.Format("{0:f}", budget.AdditionalCost.ToString());
        }
        else
            pnlDelivery.Visible = false;
    }
    /// <summary>
    /// load data of customer for report
    /// </summary>
    public void loadDataOfCustomer()
    {

        if (budget != null)
        {
            CustomerManager customerManager = new CustomerManager(this);
            SelCustomer.ShowCustomer(customerManager.GetCustomer(budget.CustomerId.Value, (int)Company.MatrixId));

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        StoreHtmlContent = true;

        CompanyManager cManager;
        CompanyConfiguration cSettings;
        SaleManager saleManager;

        if (!IsPostBack)
        {
            if (Request.QueryString["BudgetId"] != null)
                Page.ViewState["BudgetId"] = Request.QueryString["BudgetId"];

            if (Page.ViewState["BudgetId"] != null)
            {
                saleManager = new SaleManager(this);
                budget = saleManager.GetBudget(Convert.ToInt32(Page.ViewState["BudgetId"]), Company.CompanyId);
                double ipi = 0;
                double sub = 0;


                subTotal = budget.BudgetItems.Sum(i => i.UnitPrice * i.Quantity);

                rptProduct.DataSource = budget.BudgetItems;
                rptProduct.DataBind();

                loadDataOfBudget();
                loadDataOfCustomer();
            }
        }



    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (budget == null)
            budget = new Budget();

        discount = Convert.ToDecimal(budget.Discount);
        ipiTax = Convert.ToDecimal(budget.IPI);
        additional = Convert.ToDecimal(budget.AdditionalCost);
        Label lblDescription;

        //
        // Get the SubTotal, aplly the discount, calculate the IPI, ans so, add the other costs
        //            
        if (budget.DiscountType == (int)DiscountType.Percentage)
            discount = subTotal * (discount / 100);

        ipi = (subTotal - discount) * ipiTax / 100;

        lblSubTotal.Text = String.Format("{0:###,##0.00}", subTotal);
        lblDiscount.Text = String.Format("{0:f}", discount);
        lblIPI.Text = String.Format("{0:f}", ipi);
        lblTotal.Text = String.Format("{0:###,##0.00}", (Convert.ToDecimal(subTotal) - Convert.ToDecimal(discount) + Convert.ToDecimal(additional) + Convert.ToDecimal(ipi)));

        lblDiscount.Visible = lblDiscountMessage.Visible = lblDiscount.Text != "0,00";
        lblAditional.Visible = lblAditionalMessage.Visible = !String.IsNullOrEmpty(lblAditional.Text);
        lblIPI.Visible = lblIPIMessage.Visible = lblIPI.Text != "0,00";
        lblSubTotal.Visible = lblSubTotalMessage.Visible = lblSubTotal.Text != lblTotal.Text;
    }
    protected void odsBudgetItems_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CompanyId"] = Company.CompanyId;
    }
    protected void rptProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //para acessar os controles contidos no repeater usei o indice.
        //obtive os indices verificando os valores do mesmo em tempo de execução(modo debug)
        if (e.Item != null)
        {
            DataRowView row = e.Item.DataItem as DataRowView;
            //subTotal += Convert.ToDecimal(row["SubTotal"]);
        }

        int showValue = Convert.ToInt32(Page.ViewState["ShowValues"]);

        switch (showValue)
        {
            case 1:
                hidePriceLabels(false);
                e.Item.Controls[15].Visible = false; //lblTotalValue
                e.Item.Controls[14].Visible = false; //lblTotalPrice
                break;
            case 2:
                hidePriceLabels(false);
                e.Item.Controls[11].Visible = false; //Value
                e.Item.Controls[13].Visible = false; //lblProductPrice  
                break;
            case 3:
                hidePriceLabels(true);
                e.Item.Controls[11].Visible = false; //Value
                e.Item.Controls[13].Visible = false; //lblProductPrice 
                e.Item.Controls[15].Visible = false; //lblTotalValue
                e.Item.Controls[14].Visible = false; //lblTotalPrice
                break;
        }


    }
    /// <summary>
    /// this method hide price labels of price
    /// </summary>
    private void hidePriceLabels(bool visible)
    {
        lblSubTotal.Visible = lblSubTotalMessage.Visible = visible;
        lblAditional.Visible = lblAditionalMessage.Visible = visible;
        lblDiscount.Visible = lblDiscountMessage.Visible = visible;
        lblIPI.Visible = lblIPIMessage.Visible = visible;
        lblTotal.Visible = lblTotalMessage.Visible = visible;

    }
    //
    //This method format the field product description for
    //does not exceed the size of the control repeater. 
    //
    public DataTable FormatProductDescription(DataTable table)
    {
        DataTable tableFormated = new DataTable();
        tableFormated = table;
        char[] descriptionOriginal;
        StringBuilder descriptionFormated = new StringBuilder();

        for (int i = 0; i < table.Rows.Count; i++)
        {
            descriptionOriginal = (tableFormated.Rows[i]["Description"].ToString()).ToCharArray();

            if (descriptionOriginal.Length > 120)
            {
                for (int j = 0; j < descriptionOriginal.Length; j++)
                {
                    descriptionFormated.Append(descriptionOriginal[j]);

                    if (j > 0 && j % 120 == 0 && descriptionOriginal[121] != '\n')
                    {
                        descriptionFormated.Append("\n");
                    }
                }
                tableFormated.Rows[i]["Description"] = descriptionFormated.ToString();
            }
        }

        return tableFormated;
    }

    protected void btnSendTOCustomer_Click(object sender, EventArgs e)
    {
        SaleManager manager = new SaleManager(this);
        manager.SendBudgetToCustomer(Convert.ToInt32(Page.ViewState["BudgetId"]), Company, 0, String.Empty);
    }
}

