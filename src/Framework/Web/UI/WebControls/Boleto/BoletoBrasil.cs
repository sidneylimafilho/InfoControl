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
    public class BoletoBancoBrasil : BoletoGenerator
    {
        public BoletoBancoBrasil()
        {

        }

        override public int BancoCodigo
        {
            get { return 1; }
        }

        override public char BancoCodigoDV
        {

            get { return '9'; }
        }

        override public string BancoLogoTipo
        {

            get { return @"BBLogo.gif"; }
        }

        /// <summary>
        /// Utilizando os valores informados nas propriedes monta os valores de nosso
        /// número, linha digitável e código de barras. ATENÇÃO: Foi implementado o convênio com seis
        /// </summary>
        /// <param name="lNossoNumero">Retorna o nosso número.</param>
        /// <param name="lLinhaDigitavel">Retorna a linha digitável.</param>
        /// <param name="lCodigoBarras">Retorna o código de barras.</param>
        override public void MontaCodigos(HtmlBoleto boleto, out string lNossoNumero, out string lLinhaDigitavel, out string lCodigoBarras)
        {
            string sCampo1, sCampo2, sCampo3;
            string sCampoLivre, sValor, sbarra;



            //**************************************************************************************
            //Código de Barras
            //**************************************************************************************            
            sValor = Convert.ToInt32(boleto.ValorCobrado * 100).ToString();
            sValor = sValor.PadLeft(10, '0');
            sValor = FatorVencimento(boleto.DataVencimento) + sValor;

            lNossoNumero = boleto.Contrato.ToString("0000000")+ boleto.NossoNumero.ToString().PadLeft(10, '0');

            sCampoLivre = lNossoNumero.PadLeft(23, '0') + boleto.Carteira.ToString("00");

            sbarra = BancoCodigoFormatado() + "9" + sValor + sCampoLivre;
            lCodigoBarras = BancoCodigoFormatado() + "9" + Mod_dig11(sbarra) + sValor + sCampoLivre;

            //**************************************************************************************

            //**************************************************************************************
            //Linha Digitável
            //**************************************************************************************
            sCampo1 = BancoCodigoFormatado() + "9" + Left(sCampoLivre, 5);
            sCampo1 += Mod_dig10(sCampo1);

            sCampo2 = sCampoLivre.Substring(5, 10);
            sCampo2 += Mod_dig10(sCampo2);

            sCampo3 = sCampoLivre.Substring(15, 10);
            sCampo3 += Mod_dig10(sCampo3);

            while (sValor[0] == '0')
                sValor = sValor.Remove(0, 1);

            lLinhaDigitavel = Left(sCampo1, 5) + "." + sCampo1.Substring(5, 5) + " " +
                Left(sCampo2, 5) + "." + sCampo2.Substring(5, 6) + " " +
                Left(sCampo3, 5) + "." + sCampo3.Substring(5, 6) + " " +
                Mod_dig11(sbarra) + " " + sValor.PadRight(13, ' ');

            //**************************************************************************************

            lNossoNumero += "-" + Mod_dig11(lNossoNumero); //Mostra o nosso numero com DV
        }
    }
}
