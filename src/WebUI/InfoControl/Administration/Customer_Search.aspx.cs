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
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;

public partial class InfoControl_Administration_Customer_Search : Vivina.Erp.SystemFramework.PageBase
{
    private Customer customer;

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Hashtable htCustomer = new Hashtable();
        htCustomer.Add("CompanyId", Company.CompanyId);
        htCustomer.Add("Name", txtName.Text);
        htCustomer.Add("CPF", txtCPF.Text);
        htCustomer.Add("CNPJ", txtCNPJ.Text);
        htCustomer.Add("Email", txtMail.Text);
        htCustomer.Add("Phone", txtPhone.Text);
        htCustomer.Add("votingTitle", txtVoringTitle.Text);
        htCustomer.Add("RG", txtRG.Text);
        htCustomer.Add("Contact", txtContact.Text);
        htCustomer.Add("Ranking", rtnRanking.CurrentRating);
        Context.Items["htCustomer"] = htCustomer;
        Server.Transfer("CustomerSearch_Results.aspx");
    }
}
