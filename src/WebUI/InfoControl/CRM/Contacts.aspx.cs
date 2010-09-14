using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl.Web.Security;
using System.Web.Services;
using Vivina.Erp.BusinessRules;

[PermissionRequired("Contacts")]
public partial class InfoControl_CRM_Contacts : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["CustomerId"] = null;
            Session["SupplierId"] = null;
        }
    }

    protected void odsContacts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (!String.IsNullOrEmpty(txtContactName.Text))
            e.InputParameters["contactName"] = txtContactName.Text;

        if (!String.IsNullOrEmpty(txtOwner.Text))
            e.InputParameters["contactOwner"] = txtOwner.Text;

        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["initialLetter"] = ucAlphabeticalPaging.Letter;

        if (!String.IsNullOrEmpty(cboUser.SelectedValue))
            e.InputParameters["userId"] = Convert.ToInt32(cboUser.SelectedValue);
    }

    protected void odsUsers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void ucAlphabeticalPaging_SelectedLetter(object sender, SelectedLetterEventArgs e)
    {
        grdContacts.DataBind();
    }

    protected void grdContacts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
            e.Row.Attributes["onclick"] = "location='../Administration/Contact.aspx?ContactId=" + e.Row.DataItem.GetPropertyValue("ContactId").EncryptToHex() + "';";
    }

    protected void odsOwner_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Page.Customization["contactName"] = txtContactName.Text;
        Page.Customization["contactOwner"] = txtOwner.Text;


        grdContacts.DataBind();
    }


    [WebMethod]
    public static bool DeleteContact(Int32 contactId)
    {
        using (var contactManager = new ContactManager(null))
        {   
            try
            {
                contactManager.DeleteContact(contactId);
                return true;
            }
            catch 
            {
                return false;
            }
          
        }

    
    }

  

}
