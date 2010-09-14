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
    public class BoletoBradesco : BoletoGenerator
    {
        public BoletoBradesco()
        {

        }

        override public int BancoCodigo
        {
            get { return 237; }
        }

        override public char BancoCodigoDV
        {

            get { return '2'; }
        }

        override public string BancoLogoTipo
        {

            get { return @"BradescoLogo.gif"; }
        }

        /// <summary>
        /// Utilizando os valores informados nas propriedes monta os valores de nosso
        /// número, linha digitável e código de barras.
        /// </summary>
        /// <param name="lNossoNumero">Retorna o nosso número.</param>
        /// <param name="lLinhaDigitavel">Retorna a linha digitável.</param>
        /// <param name="lCodigoBarras">Retorna o código de barras.</param>
        override public void MontaCodigos(HtmlBoleto boleto, out string lNossoNumero, out string lLinhaDigitavel, out string lCodigoBarras)
        {
            string sCampo1, sCampo2, sCampo3;
            string sCampoLivre, sValor, sbarra;

            sValor = Convert.ToInt32(boleto.ValorCobrado * 100).ToString();

            //**************************************************************************************
            //Código de Barras
            //**************************************************************************************
            lNossoNumero = boleto.Contrato.ToString("000000") + boleto.NossoNumero.ToString().PadLeft(5, '0');
            lNossoNumero += Mod_dig11(lNossoNumero);

            sValor = sValor.PadLeft(10, '0');
            sValor = FatorVencimento(boleto.DataVencimento) + sValor;

            sCampoLivre = Left(boleto.CedenteAgencia, 4).PadLeft(4, '0') + boleto.Carteira.ToString("00").PadLeft(2, '0') +
                Left(lNossoNumero, 11).PadLeft(11, '0') + Left(boleto.CedenteConta, 7).PadLeft(7, '0') +
                "0";
            sbarra = BancoCodigoFormatado() + "9" + sValor + sCampoLivre;
            lCodigoBarras = BancoCodigoFormatado() + "9" + Mod_dig11(sbarra) + sValor + sCampoLivre;
            //**************************************************************************************

            //**************************************************************************************
            //Linha digitável
            //**************************************************************************************
            sCampo1 = BancoCodigoFormatado() + "9" + Left(sCampoLivre, 5);
            sCampo1 = sCampo1 + Mod_dig10(sCampo1);

            sCampo2 = sCampoLivre.Substring(5, 10);
            sCampo2 = sCampo2 + Mod_dig10(sCampo2);

            sCampo3 = sCampoLivre.Substring(15, 10);
            sCampo3 = sCampo3 + Mod_dig10(sCampo3);

            lLinhaDigitavel = Left(sCampo1, 5) + "." + sCampo1.Substring(5, 5) + "  " +
                Left(sCampo2, 5) + "." + sCampo2.Substring(5, 6) + "  " +
                Left(sCampo3, 5) + "." + sCampo3.Substring(5, 6) + "  " +
                Mod_dig11(sbarra) + "  " + sValor;
            //**************************************************************************************

            lNossoNumero = lNossoNumero.Substring(0, lNossoNumero.Length - 1) + "-" + lNossoNumero.Substring(lNossoNumero.Length - 1, 1);
        }
    }
}
