using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl;

public partial class Company_POS_Sale_Parcels : Vivina.Erp.SystemFramework.PageBase
{
    private DataTable _paymentTable;
    public DataTable PaymentTable
    {
        get
        {
            _paymentTable = Page.ViewState["PaymentTable"] as DataTable;
            if (_paymentTable == null)
            {
                _paymentTable = new DataTable();
                _paymentTable.Columns.Add("FinancierOperationId", typeof(int));
                _paymentTable.Columns.Add("FinancierOperationName", typeof(string));
                _paymentTable.Columns.Add("DueDate", typeof(DateTime));
                _paymentTable.Columns.Add("Amount", typeof(decimal));
                _paymentTable.Columns.Add("AccountId", typeof(int));
                Page.ViewState["PaymentTable"] = _paymentTable;
                Page.ViewState["totalSale"] = totalSale;
            }
            return Page.ViewState["PaymentTable"] as DataTable;
        }
    }

    Decimal totalSale = 0;
    Decimal totalParcels = 0;
    Decimal restant = 0;
    Int32 qtdParcels = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();

            ucDueDate.DateTime = DateTime.Now.Date;

            ucCurrFieldQuantityParcels.Text = "1";
            if (Server.UrlDecode(Request["total"]) != null)
                ucCurrFieldAmount.Text = Server.UrlDecode(Request["total"]);

            btnFinishSale.Attributes["accesskey"] = "F10";
            ttpEmployee.Visible = cboEmployee.Items.Count > 0;

            Page.ViewState["totalSale"] = Server.UrlDecode(Request["total"]);
        }
    }

    protected Decimal calculateParcelValue(Decimal value, Int32 qtdParcel)
    {
        return value / qtdParcel;
    }
    private String FormatDecimal(String value)
    {
        return value.Replace('_', ' ').Trim();
    }

    private void AddParcel()
    {
        Decimal amount = Convert.ToDecimal(ucCurrFieldAmount.Text);
        Decimal parcelValue;

        DateTime dueDate = ucDueDate.DateTime.Value;

        qtdParcels = ucCurrFieldQuantityParcels.IntValue;

        for (int i = 1; i <= qtdParcels; i++)
        {
            parcelValue = Math.Round(calculateParcelValue(amount, qtdParcels), 2);
            //esta condicional verifica se a parcela corrente é a primeira, se a condição for satisfeita
            //ele calcula a parcela. fez se necessário esta condicional para o caso do valor da parcela
            // der uma dizima periodica. o codigo calcula todas as parcelas e verifica a diferença entre o valor total
            // e o somatório da parcela, essa diferença é somada ao valor da primeira parcela
            if (i == 1)
                parcelValue = parcelValue + (amount - (parcelValue * qtdParcels));

            PaymentTable.Rows.Add(
                Convert.ToInt16(cboFinancierOperations.SelectedValue),
                cboFinancierOperations.SelectedItem.Text,
                dueDate,
                parcelValue,
                DBNull.Value);
            //add one month for next parcel
            dueDate = dueDate.AddMonths(1);
        }

        BindGrid();
    }

    private void BindGrid()
    {
        grdParcels.DataSource = PaymentTable;
        grdParcels.DataBind();
    }

    protected void btnFinishSale_Click(object sender, ImageClickEventArgs e)
    {
        SaveSale();
    }

    protected void odsPaymentMethod_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void btnAddParcel_Click(object sender, ImageClickEventArgs e)
    {
        if (ucCurrFieldAmount.CurrencyValue.Value == decimal.Zero)
        {
            ShowError("Valor total não pode ser 0(zero)!");
            return;
        }

        if (ucCurrFieldQuantityParcels.IntValue == 0)
        {
            ShowError("Quantidade de parcelas não pode ser 0(zero)!");
            return;
        }

        AddParcel();

        //ucDueDate.Text = ucCurrFieldAmount.Text = String.Empty;

        ucCurrFieldQuantityParcels.Text = "1";
        btnFinishSale.Enabled = true;
    }

    protected void odsSalesPerson_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    public Sale SaveSale()
    {
        ucDueDate.DateTime = ucDueDate.DateTime ?? DateTime.Now;
        ucCurrFieldQuantityParcels.Text = String.IsNullOrEmpty(ucCurrFieldQuantityParcels.Text) || ucCurrFieldQuantityParcels.IntValue == 0 ? "1" : ucCurrFieldQuantityParcels.Text;

        if (ucCurrFieldAmount.CurrencyValue.Value == decimal.Zero)
            ucCurrFieldAmount.Text = Convert.ToString(Page.ViewState["totalSale"]);

        if (grdParcels.Rows.Count == 0)
            AddParcel();

        SaleManager manager = new SaleManager(this);
        Sale sale = manager.SaveSale(CreateSale(), CreateSaleItemList(), User.Identity.UserId, CreateParcelList());

        if (chkPrint.Checked)
            HandleLegalTicket();

        //
        // Disposing Session from memory
        //
        Session["BudgetId"] = null;
        Session["CustomerId"] = null;
        //Session["basket"] = null;
        Session["discount"] = null;
        Session["FiscalNumber"] = null;
        Session["PaymentDataTable"] = null;

        if (chkReceipt.Checked)
        {

            Page.ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "CloseModal",
                    "top.$.LightBoxObject.close();" +
                    "top.content.location.href='../Accounting/Receipt.aspx?SaleId=" + sale.SaleId.EncryptToHex() + "';",
                    true);

        }
        else
        {
            Page.ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "CloseModal",
                    "top.$.LightBoxObject.close();" +
                    "top.content.location.href+='?';",
                    true);
        }
        return sale;
    }

    /// <summary>
    /// this method create and return a list of parcel
    /// </summary>
    /// <returns></returns>
    private List<Parcel> CreateParcelList()
    {
        Parcel parcel;
        List<Parcel> parcelList = new List<Parcel>();
        for (int i = 0; i < PaymentTable.Rows.Count; i++)
        {
            parcel = new Parcel();
            parcel.FinancierOperationId = Convert.ToInt16(PaymentTable.Rows[i]["FinancierOperationId"]);
            parcel.Amount = Convert.ToDecimal(PaymentTable.Rows[i]["Amount"]);
            parcel.Description = (i + 1).ToString() + "/" + PaymentTable.Rows.Count.ToString();
            parcel.DueDate = Convert.ToDateTime(PaymentTable.Rows[i]["DueDate"]);
            parcel.EffectedAmount = null;
            parcel.EffectedDate = null;
            parcel.CompanyId = Company.CompanyId;

            if (parcel.FinancierOperationId == 1)
            {
                parcel.EffectedDate = DateTime.Now;
                parcel.EffectedAmount = parcel.Amount;
            }
            parcelList.Add(parcel);
        }
        return parcelList;
    }

    /// <summary>
    /// this method create and return a list of SaleItem 
    /// </summary>
    /// <returns></returns>
    private List<SaleItem> CreateSaleItemList()
    {
        List<SaleItem> list = new List<SaleItem>();
        DataTable basket = Session["basket"] as DataTable;
        if (basket == null)
            return list;
        //
        foreach (DataRow row in basket.Rows)
        {
            SaleItem item = new SaleItem();
            item.CompanyId = Company.CompanyId;
            item.ModifiedDate = DateTime.Now;
            item.Quantity = Convert.ToInt16(row["Quantity"]);
            item.SerialNumber = Convert.ToString(row["SerialNumber"]);
            item.UnitPrice = Convert.ToDecimal(row["UnitPrice"]);
            item.UnitCost = Convert.ToDecimal(row["UnitCost"]);

            if (row["ProductId"] == DBNull.Value || Convert.ToInt32(row["ProductId"]) == 0)
            {
                item.ProductId = null;
                if (row["ProductName"].ToString().Contains("&nbsp;"))
                    row["ProductName"] = row["ProductName"].ToString().Remove(row["ProductName"].ToString().IndexOf("&nbsp;"));

                item.SpecialProductName = Convert.ToString(row["ProductName"]);
            }
            else
                item.ProductId = Convert.ToInt32(row["ProductId"]);

            list.Add(item);
        }
        return list;
    }

    /// <summary>
    /// this method create and return a Sale 
    /// </summary>
    /// <returns></returns>
    private Sale CreateSale()
    {
        Sale sale = new Sale();
        sale.SaleDate = DateTime.Now.Date;
        sale.OrderDate = DateTime.Now.Date;
        sale.ShipDate = DateTime.Now.Date;

        sale.CreatedByUser = User.Identity.UserName;

        sale.Discount = Convert.ToDecimal(Session["discount"]);
        sale.CompanyId = Company.CompanyId;

        if (cboEmployee.SelectedValue != null)
            sale.VendorId = Convert.ToInt32(cboEmployee.SelectedValue);

        if (Deposit != null)
            sale.DepositId = Deposit.DepositId;

        if (Session["CustomerId"] != null)
            sale.CustomerId = Convert.ToInt32(Session["CustomerId"]);

        if (Session["BudgetId"] != null)
            sale.BudgetId = Convert.ToInt16(Session["BudgetId"]);
        return sale;
    }

    private void HandleLegalTicket()
    {
        Ecf.Provider.AbrePortaSerial();
        Ecf.Provider.AbreCupom(
            Company.LegalEntityProfile.CompanyName,
            Company.AddressComp + " - " + Company.AddressNumber,
            "", "", "",
            Company.LegalEntityProfile.PostalCode,
            Company.LegalEntityProfile.IE,
            Company.LegalEntityProfile.CNPJ,
            "");

        //
        // The Sale Item Data
        //
        List<SaleItem> list = new List<SaleItem>();
        DataTable basket = Session["basket"] as DataTable;
        foreach (DataRow row in basket.Rows)
        {
            Ecf.Provider.VendeItem(
                Convert.ToString(row["ProductCode"]),
                Convert.ToString(row["ProductName"]).Split('&')[0],
                "FF",
                "I",
                Convert.ToInt16(row["Quantity"]),
                2,
                Convert.ToDouble(row["UnitPrice"]),
                "",
                "");
        }

        Ecf.Provider.IniciaFechamentoCupom(false, "", 0);

        //
        // Parcelas
        //

        for (int i = 0; i < PaymentTable.Rows.Count; i++)
        {
            Ecf.Provider.EfetuaFormaPagamento((string)PaymentTable.Rows[i]["FinancierOperationName"], Convert.ToDouble(PaymentTable.Rows[i]["Amount"]));
        }

        Ecf.Provider.TerminaFechamentoCupom("Garantia...");


        Ecf.Provider.FechaPortaSerial();
    }

    protected void grdParcels_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //
        //calculate total parcels
        //
        if (e.Row.RowType == DataControlRowType.DataRow)
            totalParcels += Convert.ToDecimal(grdParcels.DataKeys[e.Row.RowIndex]["Amount"]);

        //
        //calculate diff between total sale and total parcels
        //
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            totalSale = Convert.ToDecimal("0" + Page.ViewState["totalSale"]);

            lblTotalSale.Text = "<b>Total da venda: " + totalSale.ToString("C") + "</b>";
            lblTotalParcels.Text = "<b>Total das parcelas: " + totalParcels.ToString("C") + "</b>";
            lblDif.Text = "<b>Diferença: " + (totalParcels - totalSale).ToString("C") + "</b>";
        }
    }
    protected void odsFinancierOperation_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
}
