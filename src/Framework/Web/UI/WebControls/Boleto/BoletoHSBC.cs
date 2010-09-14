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
    public class BoletoHsbc : BoletoGenerator
    {
        public BoletoHsbc()
        {

        }

        override public int BancoCodigo
        {
            get { return 399; }
        }

        override public char BancoCodigoDV
        {

            get { return '9'; }
        }

        override public string BancoLogoTipo
        {

            get { return @"HSBCLogo.gif"; }
        }

        /// <summary>
        /// Utilizando os valores informados nas propriedes monta os valores de nosso
        /// número, linha digitável e código de barras. 
        /// posições.
        /// </summary>
        /// <param name="lNossoNumero">Retorna o nosso número.</param>
        /// <param name="lLinhaDigitavel">Retorna a linha digitável.</param>
        /// <param name="lCodigoBarras">Retorna o código de barras.</param>
        override public void MontaCodigos(HtmlBoleto boleto, out string lNossoNumero, out string lLinhaDigitavel, out string lCodigoBarras)
        {
            string sCampo1, sCampo2, sCampo3, sCampo4, sCampo5;
            string sCampoLivre, sValor, sbarra;

            sValor = Convert.ToInt32(boleto.ValorCobrado * 100).ToString();

            //**************************************************************************************
            //Código de Barras
            //**************************************************************************************
            lNossoNumero = boleto.Contrato.ToString("000000") + boleto.NossoNumero.ToString().PadLeft(5, '0');

            sValor = sValor.PadLeft(10, '0');
            sValor = FatorVencimento(boleto.DataVencimento) + sValor;

            sCampoLivre = lNossoNumero + boleto.CedenteAgencia.PadLeft(4, '0') + boleto.CedenteConta.PadLeft(7, '0') + boleto.Carteira + "1";

            sbarra = BancoCodigoFormatado() + "9" + sValor + sCampoLivre;
            lCodigoBarras = BancoCodigoFormatado() + "9" + Mod_dig11(sbarra) + sValor + sCampoLivre;

            //**************************************************************************************

            //**************************************************************************************
            //Linha Digitável
            //**************************************************************************************
            sCampo1 = "399" + "9" + lNossoNumero.Substring(0, 5);
            sCampo1 += Mod_dig10(sCampo1);

            sCampo2 = lNossoNumero.Substring(5, 6) + boleto.CedenteAgencia.PadLeft(4, '0');
            sCampo2 += Mod_dig10(sCampo2);

            sCampo3 = boleto.CedenteConta.PadLeft(7, '0') + boleto.Carteira + "1";
            sCampo3 += Mod_dig10(sCampo3);

            sCampo4 = Mod_dig11(sbarra);

            sCampo5 = sValor;

            lLinhaDigitavel = sCampo1.Substring(0, 5) + "." + sCampo1.Substring(5, 5) + " ";
            lLinhaDigitavel += sCampo2.Substring(0, 5) + "." + sCampo2.Substring(5, 5) + " ";
            lLinhaDigitavel += sCampo3.Substring(0, 6) + "." + sCampo2.Substring(6, 5) + " ";
            lLinhaDigitavel += sCampo4;
            lLinhaDigitavel += sCampo5;

            //**************************************************************************************
            lNossoNumero += "-" + Mod_dig11(lNossoNumero);
        }
    }
}
