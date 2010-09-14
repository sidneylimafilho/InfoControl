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
    public class BoletoCaixa : BoletoGenerator
    {
        string _LocalPagamento = String.Empty;
        public BoletoCaixa()
        {
            _LocalPagamento = "Pagável em qualquer banco até o vencimento preferencialmente na CAIXA e casas lotéricas.";
        }

        override public int BancoCodigo
        {
            get { return 104; }
        }

        override public char BancoCodigoDV
        {

            get { return '0'; }
        }

        override public string BancoLogoTipo
        {

            get { return @"CEFLogo.gif"; }
        }

        private string DVCampoLivre(string cVariavel)
        {
            int nSoma = 0, nMult = 2, nIndice;

            for (nIndice = cVariavel.Length - 1; nIndice >= 0; nIndice--)
            {
                nSoma += (Convert.ToByte(cVariavel[nIndice]) - 48) * nMult;
                if (nMult == 9) nMult = 2;
                else nMult++;
            }

            nSoma = 11 - nSoma % 11;
            if (nSoma > 9) nSoma = 0;
            return nSoma.ToString();
        }

        public string AgenciaCedente(HtmlBoleto boleto)
        {
            string lAgencia = boleto.CedenteAgencia.PadLeft(4, '0');
            string lContrato = boleto.Contrato.ToString().PadLeft(6, '0');
            return lAgencia + "/" + lContrato + "-" + DVCampoLivre(lAgencia + lContrato);
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
            lNossoNumero = "99" + boleto.NossoNumero.ToString().PadLeft(16, '0');
            lNossoNumero += "-" + DVCampoLivre(lNossoNumero);

            sValor = sValor.PadLeft(10, '0');
            sValor = FatorVencimento(boleto.DataVencimento) + sValor;

            sCampoLivre = "1" + Left(boleto.Contrato.ToString(), 6).PadLeft(6, '0') + "99" + boleto.NossoNumero.ToString().PadLeft(16, '0');

            sbarra = BancoCodigoFormatado() + "9" + sValor + sCampoLivre;
            lCodigoBarras = BancoCodigoFormatado() + "9" + Mod_dig11(sbarra) + sValor + sCampoLivre;
            //**************************************************************************************

            //**************************************************************************************
            //Linha digitável
            //**************************************************************************************
            sCampo1 = BancoCodigoFormatado() + "9" + Left(sCampoLivre, 5);
            sCampo1 += Mod_dig10(sCampo1);

            sCampo2 = sCampoLivre.Substring(5, 10);
            sCampo2 += Mod_dig10(sCampo2);

            sCampo3 = sCampoLivre.Substring(15, 10);
            sCampo3 += Mod_dig10(sCampo3);

            lLinhaDigitavel = Left(sCampo1, 5) + "." + sCampo1.Substring(5, 5) + "  " +
                Left(sCampo2, 5) + "." + sCampo2.Substring(5, 6) + "  " +
                Left(sCampo3, 5) + "." + sCampo3.Substring(5, 6) + "  " +
                Mod_dig11(sbarra) + "  " + sValor;
            //**************************************************************************************

        }
    }
}
