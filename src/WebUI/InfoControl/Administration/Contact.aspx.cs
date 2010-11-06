using System;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl;
using InfoControl;

namespace Vivina.Erp.WebUI.Administration
{
    public partial class Contact : Vivina.Erp.SystemFramework.PageBase
    {
        ContactManager contactManager;
        DataClasses.Contact contact;

        protected void Page_Load(object sender, EventArgs e)
        {
            litTitle.Visible = (Session["CustomerId"] == null && Session["SupplierId"] == null) || !String.IsNullOrEmpty(Request["w"]);

            if (!IsPostBack)
                if (!String.IsNullOrEmpty(Request["ContactId"]))
                    ShowContact();
        }

        private void ShowContact()
        {
            contactManager = new ContactManager(this);
            contact = contactManager.GetContact(Convert.ToInt32(Request["ContactId"].DecryptFromHex()));

            txtName.Text = contact.Name;
            txtCellPhone.Text = contact.CellPhone;
            txtMail.Text = contact.Email;
            txtMsn.Text = contact.Msn;
            txtObservation.Text = contact.Observation;
            txtPhone.Text = contact.Phone;
            txtPhone2.Text = contact.Phone2;
            txtSector.Text = contact.Sector;
            txtSkype.Text = contact.Skype;

            ucAddress.AddressComp = contact.AddressComp;
            ucAddress.AddressNumber = contact.AddressNumber;
            ucAddress.PostalCode = contact.PostalCode;
        }

        private void SaveContact()
        {
            contactManager = new ContactManager(this);
            contact = new DataClasses.Contact();
            var originalContact = new DataClasses.Contact();

            if (!String.IsNullOrEmpty(Request["ContactId"]))
            {
                originalContact = contactManager.GetContact(Convert.ToInt32(Request["ContactId"].DecryptFromHex()));
                contact.CopyPropertiesFrom(originalContact);
            }
            else contact.UserId = User.Identity.UserId;

            contact.CompanyId = Company.CompanyId;
            contact.AddressComp = ucAddress.AddressComp;
            contact.AddressNumber = ucAddress.AddressNumber;
            contact.PostalCode = ucAddress.PostalCode;

            contact.CellPhone = txtCellPhone.Text;
            contact.Email = txtMail.Text;
            contact.Msn = txtMsn.Text;
            contact.Name = txtName.Text;
            contact.Observation = txtObservation.Text;
            contact.Phone = txtPhone.Text;
            contact.Phone2 = txtPhone2.Text;
            contact.Sector = txtSector.Text;
            contact.Skype = txtSkype.Text;


            if (!String.IsNullOrEmpty(Request["ContactId"]))
                contactManager.Update(originalContact, contact);
            else
            {
                contactManager.Insert(contact);

                if (Session["CustomerId"] != null)
                {
                    var customerContact = new CustomerContact();
                    customerContact.CompanyId = Company.CompanyId;
                    customerContact.CustomerId = Convert.ToInt32(Session["CustomerId"].ToString().DecryptFromHex());
                    customerContact.ContactId = contact.ContactId;
                    contactManager.InsertCustomerContact(customerContact);
                }
                else
                {
                    var supplierContact = new SupplierContact();
                    supplierContact.CompanyId = Company.CompanyId;
                    supplierContact.SupplierId = Convert.ToInt32(Session["SupplierId"].ToString().DecryptFromHex());
                    supplierContact.ContactId = contact.ContactId;
                    contactManager.InsertSupplierContact(supplierContact);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveContact();

            if (!String.IsNullOrEmpty(Request["w"]))
            {
                CloseModalPopup();
                return;
            }

            if (Session["CustomerId"] == null && Session["SupplierId"] == null)
            {
                Response.Redirect("../CRM/Contacts.aspx");
                return;
            }

            if (Session["CustomerId"] != null)
            {
                var customerId = Convert.ToString(Session["CustomerId"]).DecryptFromHex();
                Session["CustomerId"] = null;
                Response.Redirect("Contacts.aspx?CustomerId=" + customerId.EncryptToHex());
            }
            else
            {
                var supplierId = Convert.ToString(Session["SupplierId"]).DecryptFromHex();
                Session["SupplierId"] = null;
                Response.Redirect("Contacts.aspx?SupplierId=" + supplierId.EncryptToHex());
            }
        }

        /// <summary>
        /// This method closes the modalPopup
        /// </summary>
        private void CloseModalPopup()
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", "top.$.LightBoxObject.close()();", true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(Request["w"]))
            {
                CloseModalPopup();
                return;
            }

            if (Session["CustomerId"] == null && Session["SupplierId"] == null)
            {
                Response.Redirect("../CRM/Contacts.aspx");
                return;
            }

            if (Session["CustomerId"] != null)
            {
                var customerId = Session["CustomerId"];
                Session["CustomerId"] = null;
                Response.Redirect("Contacts.aspx?CustomerId=" + customerId);
            }
            else
            {
                var supplierId = Session["SupplierId"];
                Session["SupplierId"] = null;
                Response.Redirect("Contacts.aspx?SupplierId=" + supplierId);
            }
        }
    }
}
