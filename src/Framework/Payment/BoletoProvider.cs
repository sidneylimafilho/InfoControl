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

namespace InfoControl.Payment
{


    public class BoletoProvider : PaymentProvider
    {


        public override PaymentResult Process(string total, PaymentMode mode, int numParcels, string filiacao, string distribuidor, int numPedido, CreditCard cartao)
        {
            filiacao = filiacao ?? "1001734898"; // 1001734898 filiacao de exemplo
            numParcels = numParcels == 0 ? 1 : numParcels;


            //
            // Pega endereço dos components da Visa
            //            
            if (Config.Address == null) throw new ProviderException("The Boleto endpoint provider is not configured!");

            string paymentNethod = ((mode == PaymentMode.InCash) ? "10" + numParcels.ToString("00") :
                                    (mode == PaymentMode.CreditWithInterestStore) ? "20" + numParcels.ToString("00") :
                                    (mode == PaymentMode.CreditWithInterestIssuer) ? "30" + numParcels.ToString("00") :
                                    "A001");


            var html = new StringBuilder();

            html.Append("<script>location=\"" + Config.Address + "?saleId=" + numPedido + "\";</script>");

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
    }
}
