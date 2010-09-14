using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Data.Linq;
using System.Text;
using InfoControl.Payment;
using System.Xml;
using InfoControl.Web.Configuration;
using System.Configuration.Provider;
using InfoControl.Net;
using System.ServiceModel.Configuration;
using System.IO;

namespace InfoControl.Payment
{


    public class VisaProvider : PaymentProvider
    {
        ChannelEndpointElement endpoint;

        public VisaProvider()
        {
            //
            // Pega endereço dos components da Visa
            //
            endpoint = WebConfig.ServiceModel.Clients.Endpoints.OfType<ChannelEndpointElement>()
                            .FirstOrDefault(ep => ep.Name.ToLower() == "visa");


            if (endpoint == null) throw new ProviderException("The Visa endpoint provider is not configured!");
        }

        public PaymentResult Process(string total, PaymentMode mode, int numParcels, string filiacao, string distribuidor, int numPedido, CreditCard cartao)
        {
            filiacao = filiacao ?? "1001734898"; // 1001734898 filiacao de exemplo
            numParcels = numParcels == 0 ? 1 : numParcels;



            string paymentNethod = ((mode == PaymentMode.InCash) ? "10" + numParcels.ToString("00") :
                                    (mode == PaymentMode.CreditWithInterestStore) ? "20" + numParcels.ToString("00") :
                                    (mode == PaymentMode.CreditWithInterestIssuer) ? "30" + numParcels.ToString("00") :
                                    "A001");


            var html = new StringBuilder();

            html.Append("<form action=\"" + endpoint.Address.AbsoluteUri + "\" method=\"post\" id=\"pay_VBV\">");
            //
            // Número único gerado a cada transação pela Visanet
            //
            html.Append("<input type=\"hidden\" name=\"tid\" value=\"" + GenerateTransationId(filiacao, paymentNethod) + " \">");
            //
            // Identificação do pedido na loja
            //
            html.Append("<input type=\"hidden\" name=\"orderid\" value=\"" + numPedido.ToString() + "\">");
            //
            // NUmero do banco quando o processo for Visa Electron
            //
            html.Append("<input type=\"hidden\" name=\"bank\" value=\"" + cartao.Bank + "\">");
            //
            // Numero do Cartão
            //
            html.Append("<input type=\"hidden\" name=\"bin\" value=\"" + cartao.Number + "\">");
            //
            // endereco onde está o arquivo de configuração .ini
            //
            html.Append("<input type=\"hidden\" name=\"merchid\" value=\"componentes_vbv/keys/" + filiacao + "\">");
            //
            // Valor da Transação -> R$100,00
            //
            html.Append("<input type=\"hidden\" name=\"damount\" value=\"R$" + total.ToString("f") + ">");
            //
            //Tipo de autenticação 
            // 0: para pedir o CVC2
            // 1: para não pedir o CVC2
            //
            html.Append("<input type=\"hidden\" name=\"authenttype\" value=\"0\">");
            //
            // Valor da Transação > R$100,00 -> 10000
            html.Append("<input type=\"hidden\" name=\"price\" value=\"" + total.ToString("f").Replace(",", "") + ">");
            //
            //Itens de livre preenchimento
            //
            html.Append("<input type=\"hidden\" name=\"free\" value=\"campo livre\">");
            html.Append("</form><script>document.getElementById('pay_VBV').submit();</script>");

            return new PaymentResult(html.ToString());
        }

        /// <summary>
        /// Generate a transaction id required by Visa 
        /// </summary>
        /// <param name="shopid"></param>
        /// <param name="pagamento"></param>
        /// <returns></returns>
        private string GenerateTransationId(string shopid, string pagamento)
        {
            //pagamento = pagamento == "02" ? "3002" : "1001";
            shopid = shopid.Substring(5, 5);

            //Ano, Hora, Minuto, Segundo e Décimo de Segundo
            String ano = DateTime.Now.ToString("yyyy").Substring(3, 1);
            String hora = DateTime.Now.Hour.ToString("00");
            String minuto = DateTime.Now.Minute.ToString("00");
            String segundo = DateTime.Now.ToString("ssf");

            if (segundo.Length == 2)
                segundo = "0" + segundo;

            //Obter Data Juliana
            String datajuliana = (new System.Globalization.JulianCalendar()).GetDayOfYear(DateTime.Now).ToString("000");

            return (shopid + ano + datajuliana + hora + minuto + segundo + pagamento);
        }

        private string GetResumeHtml()
        {
            var sb = new StringBuilder();
            sb.Append("<b>Dados do consumidor:</b><BR>");
            sb.Append("<b>Nome:</b> cosumidor - tel - email<BR>");
            sb.Append(" <BR>");
            sb.Append("<b>Dados de entrega:</b>" + "<BR>");
            sb.Append("<b>Nome:</b> VarNome  - VarEndereco  -  VarBairro -  VarCidade  - VarUF  -  VarCep <BR><BR>");
            sb.Append("<table border='0' cellspacing='0' width='100%'>");
            sb.Append("<tr bgcolor=#CCCCCC>");
            sb.Append("<td width='68' height='15'><b>Quant.</b></td>");
            sb.Append("<td width='68' height='15'><b>Item</b></td>");
            sb.Append("<td width='210' height='15'><b>Descri+ccedil;+atilde;o</b></td>");
            sb.Append("<td width='88' height='15'><b>Pre+ccedil;o</b></td>");
            sb.Append("<td width='88' height='15'><b>Sub-Total:</b></td>");
            sb.Append("</tr>");
            sb.Append("<tr bgcolor=#FFFFFF>");
            sb.Append("<td width='68' height='15'> + qty + </td>");
            sb.Append("<td width='68' height='15'> + item + </td>");
            sb.Append("<td width='210' height='15'> + produto + </td>");
            sb.Append("<td width='88' height='15'> + moeda(puni) + </td>");
            sb.Append("<td width='88' height='15'> + moeda(subtotal) + </td>");
            sb.Append("</tr></table>");
            return sb.ToString();
        }

        /// <summary>
        /// Save the Visa Configuration
        /// </summary>
        public void SaveConfiguration(string filiacao, string operation)
        {

            if (!String.IsNullOrEmpty(filiacao) && filiacao != "0")
            {
                string keysPath = endpoint.Contract.Split(new[] { '|' })[0];
                string keyFile = Path.Combine(keysPath, filiacao + ".keydata");
                File.WriteAllText(keyFile, operation);

                string configPath = endpoint.Contract.Split(new[] { '|' })[1];
                string template = File.ReadAllText(Path.Combine(configPath, "template.ini"));

                template = template.Replace("{KEY_FILE}", keyFile);
                template =  template.Replace("{MEMBERSHIP}", filiacao);

                File.WriteAllText(Path.Combine(configPath, filiacao + ".ini"), template);
            }
        }
    }
}
