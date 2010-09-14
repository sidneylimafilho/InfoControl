using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using InfoControl;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;

public partial class Company_Administration_Contacts : Vivina.Erp.SystemFramework.PageBase
{
    private ContactManager _contactManager;

    public ContactManager ContactManager
    {
        get
        {
            if (_contactManager == null)
                _contactManager = new ContactManager(this);

            return _contactManager;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["SupplierId"] = Request["SupplierId"];
            Session["CustomerId"] = Request["CustomerId"];
        }

        if (!String.IsNullOrEmpty(Request["SupplierId"]))
        {
            odsContacts.TypeName = "Vivina.Erp.BusinessRules.ContactManager";
            odsContacts.SelectMethod = "GetSupplierContacts";
            odsContacts.SelectCountMethod = "GetSupplierContactsCount";
            odsContacts.DeleteMethod = "DeleteSupplierContact";
            odsContacts.SelectParameters[0].Name = "supplierId";
        }

    }

    protected void grdContacts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = "location='Contact.aspx?ContactId=" + grdContacts.DataKeys[e.Row.RowIndex]["ContactId"].EncryptToHex() + "';";

            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }
    }

    protected void odsContacts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["SupplierId"]))
        {
            e.InputParameters["supplierId"] = Convert.ToInt32(Request["SupplierId"].DecryptFromHex());
            return;
        }

        e.InputParameters["customerId"] = Convert.ToInt32(Request["CustomerId"].DecryptFromHex());

    }

    protected void btnAddContact_Click(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(selContact.Name))
        {
            ShowError("Selecione um contato!");
            return;
        }          

        if (!selContact.ContactId.HasValue) 
        {
            ShowError("Contato não cadastrado!");
            return;
        }

        if (!String.IsNullOrEmpty(Request["CustomerId"]))
        {
            if (ExitsCustomerContact())
            {
                ShowError("Contato já adicionado a esse cliente!");
                return;
            }

            InsertCustomerContact();
            selContact.Reset();
            return;
        }

        if (ExitsSupplierContact())
        {
            ShowError("Contato já adicionado a esse fornecedor!");
            return;
        }

        InsertSupplierContact();
        selContact.Reset();
    }

    /// <summary>
    /// This method verifies if exist the customerContact
    /// </summary>
    /// <returns></returns>
    private bool ExitsCustomerContact()
    {

        return ContactManager.GetCustomerContact(selContact.ContactId.Value, Convert.ToInt32(Request["CustomerId"].DecryptFromHex())) != null;

    }

    /// <summary>
    /// This method verifies if exist the supplierContact
    /// </summary>
    /// <returns></returns>
    private bool ExitsSupplierContact()
    {
        return ContactManager.GetSupplierContact(selContact.ContactId.Value, Convert.ToInt32(Request["SupplierId"].DecryptFromHex())) != null;

    }

    /// <summary>
    /// This method inserts a new CustomerContact
    /// </summary>
    private void InsertCustomerContact()
    {
        var customerContact = new CustomerContact();

        customerContact.CompanyId = Company.CompanyId;
        customerContact.CustomerId = Convert.ToInt32(Request["CustomerId"].DecryptFromHex());
        customerContact.ContactId = Convert.ToInt32(selContact.ContactId);
        ContactManager.InsertCustomerContact(customerContact);
        grdContacts.DataBind();
    }

    /// <summary>
    /// This method inserts a new SupplierContact
    /// </summary>
    private void InsertSupplierContact()
    {
        var supplierContact = new SupplierContact();

        supplierContact.CompanyId = Company.CompanyId;
        supplierContact.SupplierId = Convert.ToInt32(Request["SupplierId"].DecryptFromHex());
        supplierContact.ContactId = Convert.ToInt32(selContact.ContactId);
        ContactManager.InsertSupplierContact(supplierContact);
        grdContacts.DataBind();
    }
}
