using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.ComponentModel;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;
using Vivina.Erp.WebUI;

using InfoControl;
[SupportsEventValidation]
[ValidationProperty("CustomerId")]
[ControlValueProperty("CustomerId")]
public partial class App_Shared_SelectCustomer : Vivina.Erp.SystemFramework.UserControlBase
{
    CustomerManager customerManager;
    Customer customer;
    private bool _enable = true;
    private bool _enableToolTip = true;

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public bool Enabled
    {
        get { return _enable; }
        set { _enable = value; }
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]

    public Int32? CustomerId { get { return (Int32?)ViewState["CustomerId"]; } set { ViewState["CustomerId"] = value; } }

    public Customer Customer
    {
        get
        {
            return customer ?? (customer = new CustomerManager(this).GetCustomer((Int32)ViewState["CustomerId"], Page.Company.CompanyId));
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        

        imgUnselect.Visible = txtCustomer.Enabled = Enabled;
        if (Convert.ToBoolean(Request["ReadOnly"]))
        {
            lnkCustomerName.Enabled = false;
            lnkCustomerName.OnClientClick = "";
            imgUnselect.Visible = false;
        }
    }

    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        ProfileManager profileManager;
        //select the customer
        if (txtCustomer.Text.Contains("|"))
        {
            OnSelectingCustomer(this, new SelectingCustomerEventArgs() { CustomerName = txtCustomer.Text });

            customerManager = new CustomerManager(this);
            profileManager = new ProfileManager(this);

            string[] identifications = txtCustomer.Text.Split('|');
            string identification = identifications[0].ToString().Trim();

            customer = customerManager.GetCustomer(Page.Company.CompanyId, identification);

            ShowCustomer(customer);
            txtCustomer.Text = "";
        }

    }

    protected void OnSelectingCustomer(Object sender, SelectingCustomerEventArgs e)
    {
        if (SelectingCustomer != null)
            SelectingCustomer(sender, e);
    }

    protected void OnSelectedCustomer(Object sender, SelectedCustomerEventArgs e)
    {
        if (SelectedCustomer != null)
            SelectedCustomer(sender, e);
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        //Page.ClientScript.RegisterHiddenField(this.ClientID, CustomerId.ToString());
    }

    public event EventHandler<SelectingCustomerEventArgs> SelectingCustomer;
    public event EventHandler<SelectedCustomerEventArgs> SelectedCustomer;
    /// <summary>
    /// show the Data of customer
    /// </summary>
    /// <param name="customer"></param>
    public void ShowCustomer(Customer customer)
    {
        if (customer != null)
        {

            txtCustomer.Enabled = _enable;
            ViewState["CustomerId"] = customer.CustomerId;
            if (customer.LegalEntityProfile != null)
            {
                if (customer.LegalEntityProfile.Address != null)
                {
                    lblCustomerAddress.Text = customer.LegalEntityProfile.Address.Name
                        + ", " + customer.LegalEntityProfile.AddressNumber
                        + " " + customer.LegalEntityProfile.AddressComp;

                    lblCustomerLocalization.Text = customer.LegalEntityProfile.Address.City
                        + " - " + customer.LegalEntityProfile.Address.Neighborhood
                        + ", " + customer.LegalEntityProfile.Address.StateId;

                    lblPostalCode.Text = "CEP: " + customer.LegalEntityProfile.Address.PostalCode;
                }
                lnkCustomerName.Text = customer.LegalEntityProfile.CompanyName;
                if (customer.LegalEntityProfile.Phone != "(__)____-____" && customer.LegalEntityProfile.Phone.Trim() != "-")
                    lblCustomerPhone.Text = "Tel: " + customer.LegalEntityProfile.Phone.Replace("(__)____-____", "");
                else
                    lblCustomerPhone.Visible = false;
                lblCNPJ.Text = customer.LegalEntityProfile.CNPJ;
            }
            else
            {
                if (customer.Profile.Address != null)
                {
                    lblCustomerAddress.Text = customer.Profile.Address.Name
                        + " " + customer.Profile.AddressNumber
                        + " " + customer.Profile.AddressComp;

                    lblCustomerLocalization.Text = customer.Profile.Address.City
                        + " - " + customer.Profile.Address.Neighborhood
                        + ", " + customer.Profile.Address.StateId;

                    lblPostalCode.Text = "CEP: " + customer.Profile.Address.PostalCode;
                }
                lnkCustomerName.Text = customer.Profile.Name;
                if (customer.Profile.Phone != "(__)____-____" && customer.Profile.Phone.Trim() != "-")
                    lblCustomerPhone.Text = "Tel: " + customer.Profile.Phone.Replace("(__)____-____", "");
                else
                    lblCustomerPhone.Visible = false;
                lblCNPJ.Text = customer.Profile.CPF;
            }
            OnSelectingCustomer(this, new SelectingCustomerEventArgs() { CustomerName = txtCustomer.Text });
            lnkCustomerName.OnClientClick = "top.tb_show('Cadastro de Clientes','Administration/Customer.aspx?CustomerId=" + customer.CustomerId.EncryptToHex() + "' );return;";
            pnlCustomer.Visible = true;
            pnlCustomerSearch.Style.Add(HtmlTextWriterStyle.Display, "none");
            OnSelectedCustomer(this, new SelectedCustomerEventArgs() { Customer = customer });


        }
        else
        {
            CustomerId = null;
        }

    }


}

public class SelectingCustomerEventArgs : EventArgs
{
    public String CustomerName { get; set; }
}
public class SelectedCustomerEventArgs : EventArgs
{
    public Customer Customer { get; set; }
}