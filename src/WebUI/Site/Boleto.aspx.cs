using System;
using System.Linq;
using System.Web;
using BoletoNet;
using InfoControl.Web.UI;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;
using UserControl = System.Web.UI.UserControl;

//using InfoControl.Net.Mail.Mime;
//using InfoControl.Net.Mail.POP3;

public partial class Site_Boleto : PageBase
{


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["parcelId"]))
        {
            var parcel = (new ParcelsManager(this)).GetParcel(Convert.ToInt32(Request["parcelId"]), Company.CompanyId);
            var boleto = parcel.GenerateBoleto();
            boleto.Valida();
            BoletoBancario.CodigoBanco = (short)boleto.Banco.Codigo;
            BoletoBancario.Boleto = boleto;           
        }

        if (!String.IsNullOrEmpty(Request["saleId"]))
        {
            var sale = (new SaleManager(this)).GetSale(Company.CompanyId, Convert.ToInt32(Request["saleId"]));            
            var boleto = sale.GenerateBoleto();
            boleto.Valida();
            BoletoBancario.CodigoBanco = (short)boleto.Banco.Codigo;
            BoletoBancario.Boleto = boleto;
                     
        }
    }
}