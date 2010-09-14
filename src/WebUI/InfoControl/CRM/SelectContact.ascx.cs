using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using System.ComponentModel;
using InfoControl;


namespace Vivina.Erp.WebUI.InfoControl.CRM
{

    [SupportsEventValidation]
    [ValidationProperty("ContactId")]
    [ControlValueProperty("ContactId")]
    public partial class SelectContact : Vivina.Erp.SystemFramework.UserControlBase
    {
        Contact contact;
        private bool _enable = true;
        private bool _enableToolTip = true;
        //  private bool _required;

        private Contact _contact;

        /// <summary>
        /// This method empty values 
        /// </summary>
        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public void Reset()
        {
            txtContact.Text = String.Empty;

            pnlContactSearch.Style.Add(HtmlTextWriterStyle.Display, "block");
            pnlContact.Style.Add(HtmlTextWriterStyle.Display, "none");
            ContactId = null;
        }

        public string Name
        {
            get { return txtContact.Text; }
            set { txtContact.Text = value; }
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


        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public bool Enabled
        {
            get { return _enable; }
            set { _enable = value; }
        }

        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public Int32? ContactId { get { return (Int32?)ViewState["ContactId"]; } set { ViewState["ContactId"] = value; } }

        public Contact Contact
        {
            get
            {
                return _contact ?? (_contact = new ContactManager(this).GetContact(Convert.ToInt32(ViewState["ContactId"])));
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtContact.Attributes["servicepath"] = ResolveUrl("~/Controller/SearchService/SearchContact");
            //reqTxtContact.ValidationGroup = Required ? ValidationGroup : "_NonValidation";
        }

        public event EventHandler<SelectingContactEventArgs> SelectingContact;
        public event EventHandler<SelectedContactEventArgs> SelectedContact;

        protected void OnSelectingContact(Object sender, SelectingContactEventArgs e)
        {
            if (SelectingContact != null)
                SelectingContact(sender, e);
        }
        protected void OnSelectedContact(Object sender, SelectedContactEventArgs e)
        {
            if (SelectedContact != null)
                SelectedContact(sender, e);
        }

        protected void txtContact_TextChanged(object sender, EventArgs e)
        {
            OnSelectingContact(this, new SelectingContactEventArgs() { ContactName = txtContact.Text });

            var contactManager = new ContactManager(this);
            contact = contactManager.GetContact(Page.Company.CompanyId, txtContact.Text);

            ShowContact(contact);
            //txtContact.Text = string.Empty;
        }

        public void ShowContact(Contact contact)
        {
            if (contact != null)
            {
                txtContact.Enabled = _enable;
                ViewState["ContactId"] = contact.ContactId;

                lblEmail.Text = !String.IsNullOrEmpty(contact.Email) ? contact.Email : String.Empty;
                lblPhone.Text = contact.Phone != "(__)____-____" ? contact.Phone : String.Empty;

                OnSelectingContact(this, new SelectingContactEventArgs() { ContactName = txtContact.Text });

                pnlContact.Visible = true;

                pnlContactSearch.Style.Add(HtmlTextWriterStyle.Display, "none");

                lnkContactName.Text = contact.Name;
                lnkContactName.OnClientClick = "top.tb_show('Contato','Administration/Contact.aspx?ContactId=" + contact.ContactId.EncryptToHex() + "' );return;";
                OnSelectedContact(this, new SelectedContactEventArgs() { Contact = contact });
            }
            else
                ContactId = null;
        }

    }

    public class SelectingContactEventArgs : EventArgs
    {
        public String ContactName { get; set; }
    }
    public class SelectedContactEventArgs : EventArgs
    {
        public Contact Contact { get; set; }
    }


}