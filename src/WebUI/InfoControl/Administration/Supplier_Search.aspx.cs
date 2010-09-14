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
using InfoControl.Web.Security;

[PermissionRequired("Suppliers")]
public partial class InfoControl_Administration_Supplier_Search : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //the Hashtable contais the properties of supplier
        Hashtable htSupplier = new Hashtable();
        htSupplier.Add("CompanyId", Company.CompanyId);
        htSupplier.Add("Name", txtName.Text);
        htSupplier.Add("CPF",txtCPF.Text);
        htSupplier.Add("CNPJ", txtCNPJ.Text);
        htSupplier.Add("Contact", txtContact.Text);
        htSupplier.Add("Email", txtMail.Text);
        htSupplier.Add("Phone", txtPhone.Text);
        htSupplier.Add("votingTitle", txtVoringTitle.Text);
        htSupplier.Add("RG", txtRG.Text);
        htSupplier.Add("Ranking", rtnRanking.CurrentRating);
        Context.Items["htSupplier"] = htSupplier;
        Server.Transfer("SupplierSearch_Results.aspx");
    }
}
