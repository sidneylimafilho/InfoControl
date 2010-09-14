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

using Vivina.Erp.SystemFramework;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl.Web.Security;

[PermissionRequired("Boletu")]
public partial class Company_Administration_Boletu : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ////
        //// Cliente
        ////
        //Boleto.SacadoNome = Company.LegalEntityProfile.CompanyName;

        //if (Company.LegalEntityProfile.PostalCode != null)
        //{
        //    Boleto.SacadoCEP = Company.LegalEntityProfile.PostalCode;
        //    Boleto.SacadoEndereco = Company.LegalEntityProfile.Address.Name
        //    + " " + Company.LegalEntityProfile.AddressNumber + " " + Company.LegalEntityProfile.AddressComp;

        //    Boleto.SacadoCidade = Company.LegalEntityProfile.Address.City;
        //    Boleto.SacadoBairro = Company.LegalEntityProfile.Address.Neighborhood;
        //    Boleto.SacadoUF = Company.LegalEntityProfile.Address.StateId;
        //}
        //Boleto.SacadoCPF_CNPJ = Company.LegalEntityProfile.CNPJ;

        ////
        //// Dados Boleto
        ////                
        //Boleto.DataEmissao = DateTime.Now;
        //Boleto.DataVencimento = Company.NextDueDate;
        //Boleto.DataProcessamento = DateTime.Now;
        //Boleto.DataDocumento = DateTime.Now;
        //Boleto.Aceite = true;
        //Boleto.Carteira = 0;

        //Boleto.DataDocumento = DateTime.Now;
        //Boleto.DataEmissao = DateTime.Now;
        //Boleto.NossoNumero = Company.CompanyId.ToString("000000") + DateTime.Today.ToString("yy") + DateTime.Today.DayOfYear.ToString();


        ////
        //// Valor
        ////
        //CompanyManager cManager = new CompanyManager(this);
        //IQueryable<Package> packages = cManager.GetPackages(Company.ReferenceCompanyId);
        //decimal ttl = 0;
        //foreach (Package pkg in packages)
        //{
        //    Boleto.Instrucao += pkg.Name + "&nbsp;&nbsp;&nbsp;&nbsp;" + pkg.Price.ToString("C") + "<Br />";
        //    ttl += pkg.Price;
        //}

        //Boleto.Instrucao += "<br/>&nbsp; Custos Bancários:" + "&nbsp;&nbsp;&nbsp;&nbsp;" + 4.ToString("C");
        //ttl += 4;        
        //Boleto.Valor = Convert.ToSingle(ttl);

        ////
        ////  Vivina
        ////        
        //Boleto.CedenteNome = "Vivina Consultoria e Informática";
        //Boleto.CedenteAgencia = "0183-X";
        //Boleto.CedenteConta = "24887";
        //Boleto.CedenteContaDV = "0";


    }
}
