using System;
using System.Xml;
using System.Configuration;
using System.ServiceModel;

namespace InfoControl.Payment
{


    public class MasterCardProvider : PaymentProvider
    {

        public override PaymentResult Process(string total, PaymentMode mode, int numParcels, string filiacao, string distribuidor, int numPedido, CreditCard cartao)
        {
            XmlNode response;
            Int32 resultNumber = 0;

            var provider = new MasterCard.komerci_captureSoapClient(new BasicHttpBinding(BasicHttpSecurityMode.Transport),
                                                                    new EndpointAddress(Config.Address));

            string txtmode = (mode == PaymentMode.InCash) ? "04" :
                             (mode == PaymentMode.CreditWithInterestIssuer) ? "06" :
                             (mode == PaymentMode.CreditWithInterestStore) ? "08" : "";


            //
            // 1° requisição : GetAuthorized
            // Informações passadas pelo usuário: número de parcelas,número do cartão e Portador(nome do cliente)
            // Informações padrão: total da compra,tipo da transação, filiação,  mês e ano correntes,
            // distribuidor e número do documento1

            //Tipo de transação : 04 a vista, 06 parcelada pelo emissor e 08 parcelado pelo estabelecimento
            //ConfTxn tem o valor S por confirmação da transação da aplicação de ecommerce  

            response = provider.GetAuthorizedTst(total, txtmode,
                numParcels.ToString("00"),
                filiacao,
                numPedido.ToString("00"),
                cartao.Number,
                cartao.Cvc2,
                cartao.ExpirationMonth,
                cartao.ExpirationYear,
                cartao.CardHolder,
                null,
                "12345",
                null,
                null,
                null,
                null, null, null, null, null, null, null, null, "S");



            if (!String.IsNullOrEmpty(response.FirstChild.InnerText))
                resultNumber = Convert.ToInt32(response.FirstChild.InnerText);

            if (resultNumber != 0)
                return new PaymentResult(null, response.FirstChild.InnerText);


            // informações retornadas pela 1° requisição e que precisam ser passadas na segunda
            // numero do pedido, de autorização, comprovante de venda e uma sequencia de numeros da masterCard 

            var salesNumber = response.ChildNodes[2].InnerText;
            var authorizationNumber = response.ChildNodes[5].InnerText;
            var voucherNumber = response.ChildNodes[6].InnerText;
            var sequenceNumber = response.ChildNodes[7].InnerText;

            //
            // 2° requisição : ConfirmTXN
            // 
#if DEBUG
            response = provider.ConfirmTxnTst("", sequenceNumber, voucherNumber, authorizationNumber, numParcels.ToString("00"), txtmode, total, filiacao, distribuidor, numPedido.ToString(), salesNumber, null, null, null, null, null, null, null);
#endif
            if (!String.IsNullOrEmpty(response.FirstChild.InnerText))
                resultNumber = Convert.ToInt32(response.FirstChild.InnerText);

            if (resultNumber != 0)
                return new PaymentResult(response.FirstChild.InnerText, null);


            //
            //3° requisição: Cupom
            //

            //Response.Redirect("WebForm2.aspx?voucherNumber=" + voucherNumber.EncryptToHex() + "&cardNumber=" + txtNumCartao.Text.EncryptToHex() + "&authorizationNumber=" + authorizationNumber.EncryptToHex() + "&total=" + txtTotal.Text);
#warning ultimo parâmetro não estava sendo passado
            return new PaymentResult(null, null);
        }

        public override void SaveConfiguration(string filiacao, string operation) { }
    }
}
