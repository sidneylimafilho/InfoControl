using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

using InfoControl.Web.UI;
using Exception = Resources.Exception;

public partial class App_Shared_Parcels : Vivina.Erp.SystemFramework.UserControlBase
{
    private IList<Parcel> _lstParcels;

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public IList<Parcel> DataSource
    {
        get
        {
            if (_lstParcels == null)
            {
                _lstParcels = (List<Parcel>)Session["lstParcels"];
                if (_lstParcels == null)
                {
                    _lstParcels = new List<Parcel>();
                    Session["lstParcels"] = _lstParcels;
                }
            }

            if (_lstParcels.Count() == 0 && isFilled())
                btnAddParcel_Click(this, new ImageClickEventArgs(0, 0));

            return _lstParcels;
        }
        set
        { Session["lstParcels"] = value; }
    }

    // public bool companyHasFinancierOperationWithBoleto;

    private AccountManager _accountManager;
    public AccountManager AccountManager
    {
        get
        {
            if (_accountManager == null)
                _accountManager = new AccountManager(this);

            return _accountManager;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (AccountManager.GetFinancierOperationBoleto(Page.Company.CompanyId) != null)
            grdParcel.Columns[0].Visible = true;
        else
            grdParcel.Columns[0].Visible = false;

        if (!IsPostBack)
        {
            BindGrid(-1);
        }
    }

    protected void btnAddParcel_Click(object sender, ImageClickEventArgs e)
    {
        btnAddParcel.Focus();

        DateTime dueDate = DateTime.Now.Sql2005MinValue();

        PaymentMethod paymentMethod = new AccountManager(this).GetPaymentMethod(Convert.ToInt32(cboPaymentMethod.SelectedValue));

        decimal amount = Decimal.Zero;
        if (ucAmount.CurrencyValue.HasValue)
            amount = ucAmount.CurrencyValue.Value;

        int qtdParcels = ucCurrFieldQtdParcel.IntValue;

        if (ucDtDueDate.DateTime.HasValue)
            dueDate = ucDtDueDate.DateTime.Value;

        //
        // Clear fields for infinite loop in get_DataSource
        //
        ucDtDueDate.DateTime = null;
        ucAmount.CurrencyValue = null;
        ucCurrFieldQtdParcel.CurrencyValue = null;

        //pManager = new ParcelsManager(this);
        Int32? accountId = null;
        if (!String.IsNullOrEmpty(cboAccount.SelectedValue))
            accountId = Convert.ToInt32(cboAccount.SelectedValue);

        Parcel parcel;
        FinancierOperation financierOperation = null;

        if (paymentMethod.PaymentMethodId == PaymentMethod.Boleto)
            financierOperation = AccountManager.GetFinancierOperationBoleto(Page.Company.CompanyId);

        //verify the value of each parcel
        for (int idx = 1; idx <= qtdParcels; idx++)
        {
            parcel = new Parcel();
            parcel.Amount = Math.Round(amount / qtdParcels, 2);
            parcel.DueDate = dueDate;
            parcel.Description = idx + "/" + qtdParcels;
            parcel.PaymentMethodId = paymentMethod.PaymentMethodId;
            //parcel.IdentificationNumber = txtIdentificationNumber.Text;
            parcel.AccountId = accountId;

            if (financierOperation != null)
                parcel.FinancierOperationId = financierOperation.FinancierOperationId;

            //
            // Se tem conta então já cadastra como quitada
            //
            if (accountId.HasValue)
            {
                parcel.EffectedAmount = parcel.Amount;
                parcel.EffectedDate = parcel.DueDate;
            }

            parcel.CompanyId = Page.Company.CompanyId;

            if (Page.ViewState["BillId"] != null)
                parcel.BillId = Convert.ToInt32(Page.ViewState["BillId"]);
            else if (Page.ViewState["InvoiceId"] != null)
                parcel.InvoiceId = Convert.ToInt32(Page.ViewState["InvoiceId"]);

            //esta condicional verifica se a parcela corrente é a primeira, se a condição for satisfeita
            //ele calcula a parcela. fez se necessário esta condicional para o caso do valor da parcela
            // der uma dizima periodica. o codigo calcula todas as parcelas e verifica a diferença entre o valor total
            // e o somatório da parcela, essa diferença é somada ao valor da primeira parcela
            if (idx == 1)
                parcel.Amount += (amount - (parcel.Amount * qtdParcels));

            DataSource.Add(parcel);

            switch (Convert.ToInt32(cboPeriod.SelectedValue))
            {
                case 30:
                    dueDate = dueDate.AddMonths(1); break;
                case 60:
                    dueDate = dueDate.AddMonths(2); break;
                case 7:
                    dueDate = dueDate.AddDays(7); break;
                case 15:
                    dueDate = dueDate.AddDays(15); break;
                case 10:
                    dueDate = dueDate.AddDays(10); break;
                //default:
                //    dueDate = dueDate.AddDays(Convert.ToInt32(cboPeriod.SelectedValue) / 30); break;
            }
        }
        BindGrid(-1);
        grdParcel.Visible = true;
        cboAccount.SelectedValue = String.Empty;
        //   txtIdentificationNumber.Text = String.Empty;
    }

    protected void grdParcel_RowDeleted(object sender, GridViewDeletedEventArgs e) { }

    protected void grdParcel_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //find objects
        //Parcel parcel = DataSource[e.RowIndex];
        var accountManager = new AccountManager(this);

        var txtEffectedDate = (grdParcel.Rows[e.RowIndex].FindControl("txtEffectedDate") as Date);
        var txtDueDate = (grdParcel.Rows[e.RowIndex].FindControl("txtGrdDueDate") as Date);
        var ucCurrFieldEffectedAmount = (grdParcel.Rows[e.RowIndex].FindControl("txtEffectedAmount") as CurrencyField);
        var txtIdentificationNumber = (grdParcel.Rows[e.RowIndex].FindControl("txtIdentificationNumber") as TextBox);
        var ddlCboAccountName = (grdParcel.Rows[e.RowIndex].FindControl("cboAccountName") as DropDownList);
        var ddlcboPaymentMethod = (grdParcel.Rows[e.RowIndex].FindControl("cboPaymentMethod") as DropDownList);
        var lblAmount = (grdParcel.Rows[e.RowIndex].FindControl("lblAmount") as Label);

        if (DataSource[e.RowIndex].ParcelId != 0)
        {
            // Update Mode, there is an object attached

            DataSource[e.RowIndex].Account = null;
            DataSource[e.RowIndex].PaymentMethod = null;
            DataSource[e.RowIndex].FinancierOperation = null;

            if (!String.IsNullOrEmpty(ddlCboAccountName.SelectedValue))
                DataSource[e.RowIndex].Account = accountManager.GetAccount(Convert.ToInt32(ddlCboAccountName.SelectedValue), Page.Company.CompanyId);

            if (!String.IsNullOrEmpty(ddlcboPaymentMethod.SelectedValue))
            {
                DataSource[e.RowIndex].PaymentMethod = accountManager.GetPaymentMethod(Convert.ToInt32(ddlcboPaymentMethod.SelectedValue));

                if (Convert.ToInt32(ddlcboPaymentMethod.SelectedValue) == PaymentMethod.Boleto)
                {
                    var financierOperation = accountManager.GetFinancierOperationBoleto(Page.Company.CompanyId);

                    if (financierOperation != null)
                        DataSource[e.RowIndex].FinancierOperation = financierOperation;
                }
            }
        }
        else
        {
            // Insert mode, so there is no object Attached

            DataSource[e.RowIndex].AccountId = null;
            DataSource[e.RowIndex].PaymentMethodId = null;
            DataSource[e.RowIndex].IdentificationNumber = String.Empty;

            if (!String.IsNullOrEmpty(ddlCboAccountName.SelectedValue))
                DataSource[e.RowIndex].AccountId = accountManager.GetAccount(Convert.ToInt32(ddlCboAccountName.SelectedValue), Page.Company.CompanyId).AccountId;

            if (!String.IsNullOrEmpty(ddlcboPaymentMethod.SelectedValue))
            {
                DataSource[e.RowIndex].PaymentMethodId = Convert.ToInt32(ddlcboPaymentMethod.SelectedValue);

                if (Convert.ToInt32(ddlcboPaymentMethod.SelectedValue) == PaymentMethod.Boleto)
                {
                    var financierOperation = accountManager.GetFinancierOperationBoleto(Page.Company.CompanyId);

                    if (financierOperation != null)
                        DataSource[e.RowIndex].FinancierOperationId = financierOperation.FinancierOperationId;
                }
            }
        }

        if (!String.IsNullOrEmpty(txtIdentificationNumber.Text))
            DataSource[e.RowIndex].IdentificationNumber = txtIdentificationNumber.Text;

        DataSource[e.RowIndex].EffectedAmount = Convert.ToDecimal(ucCurrFieldEffectedAmount.CurrencyValue);
        DataSource[e.RowIndex].Amount = Convert.ToDecimal(lblAmount.Text.Remove(0, 3));
        DataSource[e.RowIndex].DueDate = txtDueDate.DateTime.Value;

        DataSource[e.RowIndex].EffectedDate = txtEffectedDate.DateTime;
        if (txtEffectedDate.DateTime.HasValue)
        {
            if (txtEffectedDate.DateTime.Value < DateTime.MinValue.Sql2005MinValue() || txtEffectedDate.DateTime.Value > DateTime.Now)
            {
                Page.ShowError(Exception.DateBiggerThanCurrentDate);
                e.Cancel = true;
                return;
            }
        }

        if (txtEffectedDate.DateTime.HasValue && ucCurrFieldEffectedAmount.CurrencyValue == 0)
        {
            Page.ShowError("Parcela não pode ser quitada com valor 0(zero)!");
            e.Cancel = true;
            return;
        }

        BindGrid(-1);
    }

    protected void grdParcel_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataSource.Remove(DataSource.ElementAtOrDefault(e.RowIndex));
        BindGrid(-1);
    }

    protected void dataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Page.Company.CompanyId;
    }

    protected void grdParcel_RowDataBound(object sender, GridViewRowEventArgs e)
    { }

    private void BindGrid(int index)
    {
        grdParcel.DataSource = DataSource;
        grdParcel.EditIndex = index;
        grdParcel.DataBind();
    }

    public void Clear()
    {
        DataSource = null;
        grdParcel.DataBind();
    }

    private bool isFilled()
    {
        return (ucDtDueDate.DateTime.HasValue && ucAmount.CurrencyValue.HasValue && ucCurrFieldQtdParcel.CurrencyValue.HasValue);
    }

    protected void grdParcel_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        BindGrid(e.NewSelectedIndex);
    }

    protected void grdParcel_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        BindGrid(-1);
    }
}
