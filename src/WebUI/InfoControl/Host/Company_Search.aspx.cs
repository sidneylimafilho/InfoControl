using System;
using System.Collections;
using Vivina.Erp.DataClasses;

public partial class Company_Search : Vivina.Erp.SystemFramework.PageBase
{
    private Company _company;

    public Company Company
    {
        get
        {
            return _company;
        }
        set
        {
            _company = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void BtnLimpar_Click(object sender, EventArgs e)
    {
    }
    protected void BtnPesquisar_Click(object sender, EventArgs e)
    {
        Hashtable htCompany = new Hashtable();
        htCompany.Add("CNPJ", TxtCNPJ.Text);
        htCompany.Add("FantasyName", TxtFantasia.Text);
        htCompany.Add("IE", TxtIE.Text);
        htCompany.Add("Email", TxtEmail.Text);
        htCompany.Add("sortExpression", "LastActivityDate");

        htCompany.Add("sortDirection", "desc");
        

        htCompany.Add("CompanyName", txtEmpresa.Text);

        if (this.TxtDtInicio.Text != "")
            htCompany.Add("StartDate", Convert.ToDateTime(this.TxtDtInicio.Text));

        htCompany.Add("Phone", txtTelefone.Text);
        htCompany.Add("Website", TxtSite.Text);
        Context.Items["htCompany"] = htCompany;

        Server.Transfer("Company_Search_Results.aspx");
    }


}
