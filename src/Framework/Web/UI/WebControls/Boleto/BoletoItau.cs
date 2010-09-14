using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;


namespace InfoControl.Web.UI.WebControls
{
    public class BoletoItau : BoletoGenerator
    {

        public BoletoItau()
        {
            
        }

        public override int BancoCodigo
        {
            get { return 341; }
        }

        public override char BancoCodigoDV
        {

            get { return '7'; }
        }

        public override string BancoLogoTipo
        {

            get { return @"ItauLogo.gif"; }
        }

        

        /// <summary>
        /// Utilizando os valores informados nas propriedes monta os valores de nosso
        /// número, linha digitável e código de barras.
        /// </summary>
        /// <param name="lNossoNumero">Retorna o nosso número.</param>
        /// <param name="lLinhaDigitavel">Retorna a linha digitável.</param>
        /// <param name="lCodigoBarras">Retorna o código de barras.</param>
        public override void MontaCodigos(HtmlBoleto boleto, out string lNossoNumero, out string lLinhaDigitavel, out string lCodigoBarras)
        {
            string sCampo1, sCampo2, sCampo3, sCampo4, sCampo5;
            string sCampoLivre, sValor, sbarra;

            sValor = Convert.ToInt32(boleto.ValorCobrado * 100).ToString();

            //**************************************************************************************
            //Código de Barras
            //**************************************************************************************
            sValor = sValor.PadLeft(10, '0') + FatorVencimento(boleto.DataVencimento).PadLeft(4, '0');

            lNossoNumero = boleto.SequNossNume.PadLeft(7, '0');
            lNossoNumero += Mod_dig11(lNossoNumero);

            sCampoLivre = boleto.Carteira.ToString("000") + lNossoNumero;
            sCampoLivre += Mod_dig10(boleto.CedenteAgencia + boleto.CedenteConta + boleto.Carteira + lNossoNumero);
            sCampoLivre += boleto.CedenteAgencia.PadLeft(4, '0') + boleto.CedenteConta.PadLeft(5, '0');
            sCampoLivre += Mod_dig10(boleto.CedenteAgencia + boleto.CedenteConta) + "000";

            sbarra = BancoCodigoFormatado() + "9" + sValor + sCampoLivre;

            lCodigoBarras = BancoCodigoFormatado() + "9" + Mod_dig11(sbarra) + sValor + sCampoLivre;
            //**************************************************************************************

            //**************************************************************************************
            //Linha Digitável
            //**************************************************************************************
            sCampo1 = BancoCodigoFormatado() + "9" + boleto.Carteira.ToString("000") + lNossoNumero.Substring(0, 2);
            sCampo1 += Mod_dig10(sCampo1);

            sCampo2 = lNossoNumero.Substring(2, 6) + Mod_dig10(boleto.CedenteAgencia + boleto.CedenteConta + boleto.Carteira + lNossoNumero);
            sCampo2 += boleto.CedenteAgencia.Substring(0, 3);
            sCampo2 += Mod_dig10(sCampo2);

            sCampo3 = boleto.CedenteAgencia.Substring(3, 1) + boleto.CedenteConta.Substring(0, 5);
            sCampo3 += boleto.CedenteContaDV + "000";
            sCampo3 += Mod_dig10(sCampo3);

            sCampo4 = Mod_dig11(sbarra);

            sCampo5 = sValor;

            lLinhaDigitavel = sCampo1.Substring(0, 5) + "." + sCampo1.Substring(5, 5) + " ";
            lLinhaDigitavel += sCampo2.Substring(0, 5) + "." + sCampo2.Substring(5, 6) + " ";
            lLinhaDigitavel += sCampo3.Substring(0, 5) + "." + sCampo2.Substring(5, 6) + " ";
            lLinhaDigitavel += sCampo4 + " ";
            lLinhaDigitavel += sCampo5;

            lNossoNumero = lNossoNumero.Substring(0, lNossoNumero.Length - 1) + "-" + lNossoNumero.Substring(lNossoNumero.Length - 1, 1);
        }
    }
}
