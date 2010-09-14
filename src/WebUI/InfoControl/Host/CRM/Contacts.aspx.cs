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
using System.Xml.Linq;
using Vivina.InfoControl.SystemFramework.Web.WebControls;

using Vivina.Framework;
using Vivina.Framework.Security.Cryptography;
using Vivina.Framework.Web.Security;

[PermissionRequired("Contacts")]
public partial class InfoControl_CRM_Contacts : Vivina.InfoControl.SystemFramework.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void odsContacts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (!String.IsNullOrEmpty(txtContactName.Text))
            e.InputParameters["contactName"] = txtContactName.Text;

        if (!String.IsNullOrEmpty(txtOwner.Text))
            e.InputParameters["contactOwner"] = txtOwner.Text;

        //if (!String.IsNullOrEmpty(cboOwner.SelectedValue))
        //    e.InputParameters["contactOwner"] = cboOwner.SelectedValue;

        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["initialLetter"] = ucAlphabeticalPaging.Letter;
    }

    protected void ucAlphabeticalPaging_SelectedLetter(object sender, SelectedLetterEventArgs e)
    {
        grdContacts.DataBind();
    }

    protected void grdContacts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToBoolean(e.Row.DataItem.GetPropertyValue("isCustomerContact")))

                e.Row.Attributes["onclick"] = "location='../Administration/Customer.aspx?CustomerId=" + e.Row.DataItem.GetPropertyValue("ownerId").EncryptToHex() + "';";
            else
                e.Row.Attributes["onclick"] = "location='../Administration/Supplier.aspx?SupplierId=" + e.Row.DataItem.GetPropertyValue("ownerId").EncryptToHex() + "';";
        }
    }

    protected void odsOwner_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Page.Customization["contactName"] = txtContactName.Text;
        Page.Customization["contactOwner"] = txtOwner.Text;
        //Page.Customization["contactOwner"] = cboOwner.SelectedValue;

        grdContacts.DataBind();
    }
}
