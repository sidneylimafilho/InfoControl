using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Vivina.Erp.SystemFramework;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl.Data;
using InfoControl;

using InfoControl.Web.Security;
[PermissionRequired("Contracts")]
public partial class InfoControl_Administration_Contract : Vivina.Erp.SystemFramework.PageBase
{
    public System.Collections.Generic.List<ContractAssociated> ContractsAssociated
    {
        get
        {
            if (Session["ContractsAssociated"] == null)
                Session["ContractsAssociated"] = new System.Collections.Generic.List<ContractAssociated>();
            return (System.Collections.Generic.List<ContractAssociated>)Session["ContractsAssociated"];
        }
        set
        {
            Session["ContractsAssociated"] = value;

        }
    }
    ContractManager contractManager;
    Contract contract;
    public Int32 contractId;
    #region functions

    private void ShowAdditionalValues()
    {

        lblContractAdditionalValue1.Text = Company.CompanyConfiguration.ContractAdditionalValue1Name;
        lblContractAdditionalValue2.Text = Company.CompanyConfiguration.ContractAdditionalValue2Name;
        lblContractAdditionalValue3.Text = Company.CompanyConfiguration.ContractAdditionalValue3Name;
        lblContractAdditionalValue4.Text = Company.CompanyConfiguration.ContractAdditionalValue4Name;
        lblContractAdditionalValue5.Text = Company.CompanyConfiguration.ContractAdditionalValue5Name;

        fdsAdditional.Visible = pnlContractAdditionalValue1.Visible = !String.IsNullOrEmpty(Company.CompanyConfiguration.ContractAdditionalValue1Name);
        fdsAdditional.Visible |= pnlContractAdditionalValue2.Visible = !String.IsNullOrEmpty(Company.CompanyConfiguration.ContractAdditionalValue2Name);
        fdsAdditional.Visible |= pnlContractAdditionalValue3.Visible = !String.IsNullOrEmpty(Company.CompanyConfiguration.ContractAdditionalValue3Name);
        fdsAdditional.Visible |= pnlContractAdditionalValue4.Visible = !String.IsNullOrEmpty(Company.CompanyConfiguration.ContractAdditionalValue4Name);
        fdsAdditional.Visible |= pnlContractAdditionalValue5.Visible = !String.IsNullOrEmpty(Company.CompanyConfiguration.ContractAdditionalValue5Name);
    }

    /// <summary>
    /// this functions show a contract
    /// </summary>
    private void ShowContract()
    {
        contractManager = new ContractManager(this);
        contract = contractManager.GetContract(Company.CompanyId, Convert.ToInt32(Page.ViewState["ContractId"]));
        Parcel parcel;
        ParcelsManager parcelsManager = new ParcelsManager(this);

        cboContractTemplate.Attributes["onchange"] = "location='ContractTemplateBuilder.aspx?ContractId=" + contract.ContractId + "&ContractTemplateId='+this.value";


        //
        // if exists invoice then show gridview else show parcel fields
        //
        if (contract.InvoiceId.HasValue && contract.Invoice.Parcels.Any())
        {
            parcel = contract.Invoice.Parcels.FirstOrDefault();
            if (parcel != null)
            {
                lnkParcelValue.Text = "Valor da parcelas: " + parcel.Amount.ToString();
                lnkParcelValue.NavigateUrl = "Invoice.aspx?InvoiceId=" + parcel.InvoiceId.EncryptToHex();
            }
        }

        contractId = contract.ContractId;

        SelCustomer.ShowCustomer(contract.Customer);
        ucdtIntervalContract.DateInterval = new DateTimeInterval(contract.BeginDate, contract.ExpiresDate);
        txtObservation.Text = contract.Observation;

        ucHH.CurrencyValue = contract.HH;
        if (contract.InterestDeferredPayment.HasValue)
            txtInterestDeferredPayment.Text = contract.InterestDeferredPayment.ToString();

        ucContractValue.CurrencyValue = contract.ContractValue;

        ucContractAdditionalValue1.CurrencyValue = contract.AdditionalValue1;
        ucContractAdditionalValue2.CurrencyValue = contract.AdditionalValue2;
        ucContractAdditionalValue3.CurrencyValue = contract.AdditionalValue3;
        ucContractAdditionalValue4.CurrencyValue = contract.AdditionalValue4;
        ucContractAdditionalValue5.CurrencyValue = contract.AdditionalValue5;

        ucMonthlyFee.CurrencyValue = contract.MonthlyFee;
        ucInsurance.CurrencyValue = contract.Insurance;
        ucMoneyReserves.CurrencyValue = contract.MoneyReserves;

        ShowAdditionalValues();

        ucPenalty.CurrencyValue = contract.Penalty;

        if (contract.EmployeeId.HasValue)
            cboVendors.SelectedValue = contract.EmployeeId.ToString();
        if (contract.RepresentantId.HasValue)
            cboRepresentants.SelectedValue = contract.RepresentantId.ToString();

        txtContractNumber.Text = contract.ContractNumber;
        cboContractType.DataBind();
        cboContractType.SelectedValue = contract.ContractTypeId.ToString();
        
        cboContractStatus.DataBind();
        cboContractStatus.SelectedValue = contract.ContractStatusId.ToString();

        if (contract.FinancierOperationId.HasValue)
        {
            cboPaymentMethods.DataBind();
            cboPaymentMethods.SelectedValue = contract.FinancierOperationId.ToString();
        }

        if (contract.Parcels.HasValue)
        {
            cboParcels.DataBind();
            ListItem item = cboParcels.Items.FindByText(contract.Parcels.ToString());
            if (item != null)
                cboParcels.Items.FindByValue(item.Value).Selected = true;
        }

        ucParcelDueDate.DateTime = contract.FirstParcelDueDate;

        ContractAssociated newContractAssociated;
        foreach (ContractAssociated item in contract.ContractAssociateds)
        {
            newContractAssociated = new ContractAssociated();
            newContractAssociated.CopyPropertiesFrom(item);
            ContractsAssociated.Add(newContractAssociated);
        }

        BindContractsAssociated();
        ShowParcelValue();
    }

    private void BindContractsAssociated()
    {
        grdAssociatedContracts.DataSource = ContractsAssociated;
        grdAssociatedContracts.DataBind();
    }

    #endregion

    #region DataSources
    protected void odsContractStatus_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
    }

    protected void odsFinancierOperation_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;

    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ContractsAssociated = null;
            txtContractNumber.Text = Vivina.Erp.SystemFramework.Util.GenerateUniqueID();

            ucdtIntervalContract.DefaultBeginDate = DateTime.Now.Date;
            ShowAdditionalValues();
            upContractTemplate.Visible = false; 

            if (!String.IsNullOrEmpty(Request["ContractId"]))
            {
                Int32 contractID = 0;
                if (!Int32.TryParse(Request["ContractId"].DecryptFromHex(), out contractID))
                    return;
                Page.ViewState["ContractId"] = contractID;
            }

            if (Page.ViewState["ContractId"] != null)
            {
                ShowContract();
                grdAssociatedContracts.DataBind();
                upContractTemplate.Visible = true;
            }
        }
        ucParcelDueDate.ShowTime = false;
        ucDtDueDate.ShowTime = false;
        ucDtPaidDate.ShowTime = false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Contract original_contract = new Contract();
        contractManager = new ContractManager(this);
        contract = new Contract();

        if (Page.ViewState["ContractId"] != null)
            original_contract = contractManager.GetContract(Company.CompanyId, Convert.ToInt32(Page.ViewState["ContractId"]));

        contract.CopyPropertiesFrom(original_contract);

        contract.CompanyId = Company.CompanyId;
        contract.CustomerId = Convert.ToInt32(Page.ViewState["CustomerId"]);
        contract.BeginDate = ucdtIntervalContract.DateInterval.BeginDate;
        contract.ExpiresDate = ucdtIntervalContract.DateInterval.EndDate;
        contract.Observation = txtObservation.Text;
        contract.ContractNumber = txtContractNumber.Text;
        contract.Periodicity = 30;

        contract.Penalty = ucPenalty.CurrencyValue;
        if (!String.IsNullOrEmpty(cboPaymentMethods.SelectedValue))
        {
            contract.FinancierOperationId = Convert.ToInt32(cboPaymentMethods.SelectedValue);
            contract.FinancierConditionId = Convert.ToInt32(cboParcels.SelectedValue);
        }

        if (!String.IsNullOrEmpty(cboContractType.SelectedValue))
            contract.ContractTypeId = Convert.ToInt32(cboContractType.SelectedValue);


        contract.RepresentantId = null;        
        if (!String.IsNullOrEmpty(cboRepresentants.SelectedValue))
            contract.RepresentantId = Convert.ToInt32(cboRepresentants.SelectedValue);

        contract.EmployeeId = null;
        if (!String.IsNullOrEmpty(cboVendors.SelectedValue))
            contract.EmployeeId = Convert.ToInt32(cboVendors.SelectedValue);
            

        if (!String.IsNullOrEmpty(cboContractStatus.SelectedValue))
            contract.ContractStatusId = Convert.ToInt32(cboContractStatus.SelectedValue);

        contract.HH = ucHH.CurrencyValue;
        if (!String.IsNullOrEmpty(txtInterestDeferredPayment.Text))
            contract.InterestDeferredPayment = Convert.ToDecimal(txtInterestDeferredPayment.Text.RemoveMask());

        if (ucContractValue.CurrencyValue.HasValue)
            contract.ContractValue = ucContractValue.CurrencyValue.Value;

        contract.AdditionalValue1 = ucContractAdditionalValue1.CurrencyValue;
        contract.AdditionalValue2 = ucContractAdditionalValue2.CurrencyValue;
        contract.AdditionalValue3 = ucContractAdditionalValue3.CurrencyValue;
        contract.AdditionalValue4 = ucContractAdditionalValue4.CurrencyValue;
        contract.AdditionalValue5 = ucContractAdditionalValue5.CurrencyValue;
        contract.MoneyReserves = ucMoneyReserves.CurrencyValue;
        contract.MonthlyFee = ucMonthlyFee.CurrencyValue;
        contract.Insurance = ucInsurance.CurrencyValue;

        if (!String.IsNullOrEmpty(cboParcels.SelectedValue))
            contract.Parcels = Convert.ToInt32(cboParcels.SelectedItem.Text);

        DateTime dueDate = DateTime.MinValue.Sql2005MinValue();

        if (ucParcelDueDate.DateTime.HasValue)
            contract.FirstParcelDueDate = ucParcelDueDate.DateTime.Value.Date;

        ContractManager.ContractValidationStatus contractValidationStatus = ContractManager.ContractValidationStatus.Valid;

        if (original_contract.ContractId != 0)
            contractValidationStatus = contractManager.UpdateContract(original_contract, contract, Convert.ToInt32(cboParcels.SelectedItem.Text), ContractsAssociated);
        else
            contractValidationStatus = contractManager.InsertContract(contract, Convert.ToInt32(cboParcels.SelectedItem.Text), ContractsAssociated);

        if (contractValidationStatus == ContractManager.ContractValidationStatus.Valid)
            Response.Redirect("~/InfoControl/Accounting/Contracts.aspx");

        if (contractValidationStatus == ContractManager.ContractValidationStatus.GeneratedAsPendent)
            ShowAlert(Resources.Exception.ContractGeneratedAsdepend);

        if (contractValidationStatus == ContractManager.ContractValidationStatus.Invalid)
            ShowError(Resources.Exception.CustomerCreditLimitInvalid);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Contracts.aspx");
    }

    protected void SelCustomer_SelectedCustomer(object sender, SelectedCustomerEventArgs e)
    {
        if (e.Customer != null)
        {
            Page.ViewState["CustomerId"] = e.Customer.CustomerId;
            if (e.Customer.SalesPersonId.HasValue && cboVendors.Items.FindByValue(e.Customer.SalesPersonId.ToString()) != null)
                cboVendors.SelectedValue = e.Customer.SalesPersonId.ToString();

            cboCustomerContracts.Items.Clear();
            cboCustomerContracts.DataBind();
        }
    }

    protected void odsParcels_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (!String.IsNullOrEmpty(cboPaymentMethods.SelectedValue))
        {
            e.InputParameters["companyId"] = Company.CompanyId;
            e.InputParameters["financierOperationId"] = Convert.ToInt32(cboPaymentMethods.SelectedValue);
        }
    }

    protected void cboFinancierOperationId_TextChanged(object sender, EventArgs e)
    {
        cboParcels.Items.Clear();
        cboParcels.Items.Add(new ListItem() { Value = "", Text = "" });
        cboParcels.DataBind();
    }

    protected void odsContractTemplates_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void cboParcels_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowParcelValue();
    }

    private void ShowParcelValue()
    {
        ///calculates parcelValue
        if (!ucContractValue.CurrencyValue.HasValue ||
            String.IsNullOrEmpty(cboPaymentMethods.SelectedValue) ||
            String.IsNullOrEmpty(cboParcels.SelectedValue))
            return;

        ContractManager contractManager = new ContractManager(this);

        Int32 financierOperationId = Convert.ToInt32(cboPaymentMethods.SelectedValue);
        Int32 qtdParcels = Convert.ToInt32(cboParcels.SelectedItem.Text);
        Decimal value = Convert.ToDecimal(ucContractValue.CurrencyValue);

        Decimal parcelValue = contractManager.CalculateParcelsValue(Company.CompanyId, financierOperationId, qtdParcels, value);

        lnkParcelValue.Visible = true;

        lnkParcelValue.Text = string.Empty;
        lnkParcelValue.Text = cboParcels.SelectedItem.Text + " de " + Math.Round(parcelValue, 2).ToString("C");
        lnkParcelValue.Font.Bold = true;
        lnkParcelValue.Font.Size = 12;

        ///Calculating Contract Interval
        if (ucParcelDueDate.DateTime.HasValue && contract != null)
            ucParcelDueDate.DateTime = contract.FirstParcelDueDate;
    }

    protected void odsGeneric_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void odsContractsByCustomer_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (Page.ViewState["CustomerId"] != null)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
            e.InputParameters["customerId"] = Convert.ToInt32(Page.ViewState["CustomerId"]);
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ContractAssociated contractAssociated = new ContractAssociated();

        contractAssociated.CompanyId = Company.CompanyId;
        if (ucDtPaidDate.DateTime.HasValue)
            contractAssociated.PaidDate = ucDtPaidDate.DateTime.Value;
        if (ucDtDueDate.DateTime.HasValue)
            contractAssociated.DueDate = ucDtDueDate.DateTime.Value.Date;
        if (ucCUrrFieldContractValue.CurrencyValue.HasValue)
            contractAssociated.Amount = ucCUrrFieldContractValue.CurrencyValue.Value;

        if (!String.IsNullOrEmpty(cboCustomerContracts.SelectedValue))
        {
            contractAssociated.ContractId = Convert.ToInt32(cboCustomerContracts.SelectedValue);
            cboCustomerContracts.Items.Remove(cboCustomerContracts.SelectedItem);
        }

        ContractsAssociated.Add(contractAssociated);
        BindContractsAssociated();
    }

    protected void grdAssociatedContracts_Sorting(object sender, GridViewSortEventArgs e)
    {

    }

    protected void grdAssociatedContracts_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void grdAssociatedContracts_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (ContractsAssociated.ElementAtOrDefault(e.RowIndex) != default(ContractAssociated))
            ContractsAssociated.Remove(ContractsAssociated.ElementAtOrDefault(e.RowIndex));

        BindContractsAssociated();
    }

    protected void grdAssociatedContracts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("CommandName", "Delete");
    }
}
