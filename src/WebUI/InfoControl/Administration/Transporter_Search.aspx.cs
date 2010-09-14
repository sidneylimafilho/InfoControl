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


[PermissionRequired("Transporters")]
public partial class InfoControl_Administration_Transporter_Search : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Hashtable htTransporter = new Hashtable();

        htTransporter.Add("CompanyId", Company.CompanyId);
        htTransporter.Add("FantasyName", txtFantasyName.Text);
        htTransporter.Add("CompanyName", txtCompanyName.Text);
        htTransporter.Add("CNPJ", txtCNPJ.Text);
        htTransporter.Add("Phone", txtPhone.Text);
        htTransporter.Add("Email", txtEmail.Text);
        htTransporter.Add("WebSite", txtWebSite.Text);

        Context.Items["htTransporter"] = htTransporter;
        Server.Transfer("TransporterSearch_Results.aspx");
    }
}
