using System;
using System.Web.UI;
using System.ComponentModel;

using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

using InfoControl;

[SupportsEventValidation]
[ValidationProperty("SupplierId")]
[ControlValueProperty("SupplierId")]
public partial class App_Shared_SelectSupplier : Vivina.Erp.SystemFramework.UserControlBase
{
    public Int32? SupplierId
    {
        get { return (Int32?)ViewState["SupplierId"]; }
        set { ViewState["SupplierId"] = value; }
    }

    Supplier supplier;
    public Supplier Supplier
    {
        get
        {
            return supplier ??
                   (supplier = new SupplierManager(this)
                              .GetSupplier(Convert.ToInt32(ViewState["SupplierId"]),
                                           Page.Company.CompanyId));
        }
    }

    SupplierManager supplierManager;

    ProfileManager profileManager;
    //  private bool _required = false;

    protected void Page_Load(object sender, EventArgs e)
    {


        //valSupplier.ValidationGroup = Required ? ValidationGroup : "_NonValidation";
    }

    //create the functions(these functions are used in events)
    protected void OnSelectingSupplier(object sender, SelectingSupplierEventArgs e)
    {
        if (SelectingSupplier != null)
            SelectingSupplier(sender, e);
    }

    protected void OnSelectedSupplier(object sender, SelectedSupplierEventArgs e)
    {
        supplier = e.Supplier;

        if (SelectedSupplier != null)
            SelectedSupplier(sender, e);
    }

    //create the events
    public event EventHandler<SelectingSupplierEventArgs> SelectingSupplier;
    public event EventHandler<SelectedSupplierEventArgs> SelectedSupplier;

    protected void txtSupplier_TextChanged(object sender, EventArgs e)
    {
        //attach the event Selecting(begin)
        OnSelectingSupplier(this, new SelectingSupplierEventArgs() { SupplierName = txtSupplier.Text });
        //select the supplier
        if (!String.IsNullOrEmpty(txtSupplier.Text))
        {
            profileManager = new ProfileManager(this);
            supplierManager = new SupplierManager(this);

            string[] identifications = txtSupplier.Text.Split('|');
            string identification = identifications[0].ToString().Trim();

            supplier = supplierManager.GetSupplier(Page.Company.CompanyId, identification);

            ShowSupplier(supplier);
        }

    }

    public void ShowSupplier(Supplier supplier)
    {        
        if (supplier != null)
        {
            ViewState["SupplierId"] = supplier.SupplierId;
            if (supplier.LegalEntityProfile != null)
            {
                if (supplier.LegalEntityProfile.Address != null)
                {
                    lblSupplierAddress.Text = supplier.LegalEntityProfile.Address.Name
                        + " " + supplier.LegalEntityProfile.AddressNumber
                        + " " + supplier.LegalEntityProfile.AddressComp;
                    lblSupplierLocalization.Text = supplier.LegalEntityProfile.Address.City
                    + " - " + supplier.LegalEntityProfile.Address.Neighborhood
                    + ", " + supplier.LegalEntityProfile.Address.StateId;

                    lblPostalCode.Text = "CEP: " + supplier.LegalEntityProfile.Address.PostalCode;
                }



                lblSupplierPhone.Text = "Tel: " + supplier.LegalEntityProfile.Phone.Replace("(__)____-____", "");

                lblCNPJ.Text = supplier.LegalEntityProfile.CNPJ;
            }
            else
            {
                if (supplier.Profile.Address != null)
                {
                    lblSupplierAddress.Text = supplier.Profile.Address.Name
                        + " " + supplier.Profile.AddressNumber
                        + " " + supplier.Profile.AddressComp;

                    lblSupplierLocalization.Text = supplier.Profile.Address.City
                    + " - " + supplier.Profile.Address.Neighborhood
                    + ", " + supplier.Profile.Address.StateId;


                    lblPostalCode.Text = "CEP: " + supplier.Profile.Address.PostalCode;
                }



                lblSupplierPhone.Text = "Tel: " + supplier.Profile.Phone.Replace("(__)____-____", "");

                lblCNPJ.Text = supplier.Profile.CPF;
            }
            String encripted = supplier.SupplierId.EncryptToHex();

            pnlSupplier.Visible = true;
            pnlSupplierSearch.Style.Add(HtmlTextWriterStyle.Display, "none");
            //attach the event Selectd(end)
            OnSelectedSupplier(this, new SelectedSupplierEventArgs() { Supplier = supplier });
        }
        else
        {
            SupplierId = null;
            pnlSupplier.Visible = false;
            txtSupplier.Text = "";
        }
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string ValidationGroup
    {
        get;
        set;
    }

    //[Bindable(true, BindingDirection.TwoWay)]
    //[PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    //public bool Required
    //{
    //    get { return _required; }
    //    set { _required = value; }
    //}

    /// <summary>
    /// This method empty values 
    /// </summary>
    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public void Reset()
    {
        txtSupplier.Text = "";
        pnlSupplier.Visible = false;
        SupplierId = null;
        pnlSupplierSearch.Style.Add(HtmlTextWriterStyle.Display, "block");

    }

}


public class SelectingSupplierEventArgs : EventArgs
{
    public string SupplierName { get; set; }
}

public class SelectedSupplierEventArgs : EventArgs
{
    public Supplier Supplier { get; set; }
}
