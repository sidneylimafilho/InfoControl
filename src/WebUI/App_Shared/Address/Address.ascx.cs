using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl.Web.UI;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

[SupportsEventValidation]
[ValidationProperty("PostalCode")]
public partial class Address : DataUserControl
{
    public string Name
    {
        get { return txtAddress.Text; }
    }


    public string City
    {
        get { return txtCity.Text; }
    }

    public string State
    {
        get { return cboStates.SelectedValue; }
    }

    public string Neighborhood
    {
        get { return txtNeighborhood.Text; }
    }

    public int NeighborhoodId
    {
        get { return (Int32)ViewState["NeighborhoodId"]; }
    }

    public int CityId
    {
        get { return (Int32)ViewState["CityId"]; }
    }

    public string StateId
    {
        get { return ViewState["StateId"].ToString(); }
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public bool Required { get; set; }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string ValidationGroup { get; set; }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string PostalCode
    {
        get
        {
            string postalCode = txtPostalCode.Text.Replace("_", "").Replace("-", "");



            return String.IsNullOrEmpty(postalCode)
                       ? null
                       : postalCode;
        }
        set
        {
            txtPostalCode.Text = value;
            LoadPostalCode(txtPostalCode.Text);
        }
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string AddressNumber
    {
        get { return txtNumber.Text; }
        set { txtNumber.Text = value; }
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string AddressComp
    {
        get { return txtSubName.Text; }
        set { txtSubName.Text = value; }
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string FieldsetTitle { get; set; }

    public event EventHandler Changed;
    public void OnChanged(object sender, EventArgs e)
    {
        if (Changed != null)
            Changed(sender, e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //
        // Save the Address
        //

        if (IsPostBack && !ScriptManager.GetCurrent(Page).IsInAsyncPostBack &&
            !String.IsNullOrEmpty(txtAddress.Text) && !String.IsNullOrEmpty(txtPostalCode.Text))
        {
            var manager = new AddressManager(this);
            City city = manager.SaveCity(txtCity.Text.ToUpper(), cboStates.SelectedValue);
            Neighborhood neighborhood = manager.SaveNeighborhood(txtNeighborhood.Text.ToUpper(), city.CityId);
            manager.SaveAddress(PostalCode, txtAddress.Text.ToUpper(), neighborhood.NeighborhoodId);
        }


        if (ScriptManager.GetCurrent(Page) != null)
            ScriptManager.GetCurrent(Page).Scripts.Add(new ScriptReference("~/App_shared/address/address.ascx.js"));

        //btnCorreios.Visible = btnGoogle.Visible = Page.User.IsAuthenticated;

        lblTitle.Text = FieldsetTitle;
       // btnGoogle.Attributes["onclick"] = "top.InfoControl.Address.ShowMap('" + txtPostalCode.ClientID + "');";
        if (!Required)
            addressPanel.Style.Add("display", "none");

        EnableValidators(Required);
    }

    private void EnableValidators(bool required)
    {
        valPostalCode.Enabled = required;
        valNumber.Enabled = required;
        valNeighborhood.Enabled = required;
        valAddress.Enabled = required;
        valCity.Enabled = required;
        valStates.Enabled = required;

        valPostalCode.ValidationGroup = ValidationGroup;
        valNumber.ValidationGroup = ValidationGroup;
        valNeighborhood.ValidationGroup = ValidationGroup;
        valAddress.ValidationGroup = ValidationGroup;
        valCity.ValidationGroup = ValidationGroup;
        valStates.ValidationGroup = ValidationGroup;
    }

    protected void customValidator_OnServerValidate(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = (LoadPostalCode(txtPostalCode.Text) != null);
    }

    private Vivina.Erp.DataClasses.Address LoadPostalCode(string postalCode)
    {
        Vivina.Erp.DataClasses.Address address = null;
        if (!String.IsNullOrEmpty(postalCode))
        {
            var manager = new AddressManager(this);
            address = manager.GetAddress(postalCode.Replace("-", ""));

            if (address != null)
            {
                txtNeighborhood.Text = address.Neighborhood;
                txtCity.Text = address.City;
                txtAddress.Text = address.Name;
                cboStates.SelectedValue = address.StateId;
                ViewState["NeighborhoodId"] = address.NeighborhoodId;
                ViewState["CityId"] = address.CityId;
                ViewState["StateId"] = address.StateId;
                ViewState["addressExists"] = true;
            }
        }
        else
        {
            ViewState["addressExists"] = false;
            txtNeighborhood.Text = String.Empty;
            txtCity.Text = String.Empty;
            txtAddress.Text = String.Empty;
            txtNumber.Text = String.Empty;
            cboStates.ClearSelection();

        }
        EnableValidators(true);
        return address;
    }

    protected void txtPostalCode_TextChanged(object sender, EventArgs e)
    {
        LoadPostalCode(txtPostalCode.Text);
        OnChanged(sender, e);
    }
}