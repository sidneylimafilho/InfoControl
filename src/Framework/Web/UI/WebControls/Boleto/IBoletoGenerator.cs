using System;
using System.Collections.Generic;
using System.Text;

namespace InfoControl.Web.UI.WebControls
{
    public abstract class BoletoGenerator
    {

        public abstract int BancoCodigo
        {
            get;
        }

        public abstract char BancoCodigoDV
        {

            get;
        }

        public abstract string BancoLogoTipo
        {

            get;
        }

        /// <summary>
        /// Utilizando os valores informados nas propriedes monta os valores de nosso
        /// número, linha digitável e código de barras.
        /// </summary>
        /// <param name="lNossoNumero">Retorna o nosso número.</param>
        /// <param name="lLinhaDigitavel">Retorna a linha digitável.</param>
        /// <param name="lCodigoBarras">Retorna o código de barras.</param>
        public abstract void MontaCodigos(HtmlBoleto boleto, out string lNossoNumero, out string lLinhaDigitavel, out string lCodigoBarras);

        public virtual string FatorVencimento(DateTime lDtVencimento)
        {
            string sFatorVencto;
            TimeSpan lTimeSpan = lDtVencimento.Subtract(new DateTime(1997, 10, 7));
            sFatorVencto = lTimeSpan.Days.ToString();
            sFatorVencto = sFatorVencto.PadLeft(4, '0');
            if (sFatorVencto.StartsWith("0")) return "0000";
            else return sFatorVencto;
        }

        public virtual string Mod_dig07(string cVariavel)
        {
            int nSoma = 0, nMult = 2, nIndice;

            for (nIndice = cVariavel.Length - 1; nIndice >= 0; nIndice--)
            {
                nSoma += (Convert.ToByte(cVariavel[nIndice]) - 48) * nMult;
                if (nMult == 7) nMult = 2;
                else nMult++;
            }

            nSoma = nSoma % 11;
            if (nSoma > 1) nSoma = 11 - nSoma;

            return nSoma.ToString();
        }

        public virtual string Mod_dig10(string cVariavel)
        {
            int nSoma = 0, nMult = 2, nIndice, nProd;

            for (nIndice = cVariavel.Length - 1; nIndice >= 0; nIndice--)
            {
                nProd = (Convert.ToByte(cVariavel[nIndice]) - 48) * nMult;

                if (nProd > 9) nSoma += nProd - 9;
                else nSoma += nProd;

                if (nMult == 2) nMult = 1;
                else nMult = 2;
            }

            nSoma = nSoma % 10;
            if (nSoma > 0) nSoma = 10 - nSoma;
            return nSoma.ToString();
        }

        public virtual string Mod_dig11(string cVariavel)
        {
            string lRetorno = "0";
            int nSoma = 0, nMult = 2, nIndice;

            for (nIndice = cVariavel.Length - 1; nIndice >= 0; nIndice--)
            {
                nSoma += (Convert.ToByte(cVariavel[nIndice]) - 48) * nMult;
                if (nMult == 9) nMult = 2;
                else nMult++;
            }

            nSoma = nSoma * 10;
            nSoma = nSoma % 11;
            if ((nSoma > 9) || (nSoma < 2)) lRetorno = "1";
            else lRetorno = nSoma.ToString();

            return lRetorno;
        }

        /// <summary>
        /// Monta o código do banco contendo dígito.
        /// </summary>
        public string BancoCodigoCompleto()
        {
            return BancoCodigoFormatado() + "-" + BancoCodigoDV;
        }

        /// <summary>
        /// Formata o código do banco com três dígitos.
        /// </summary>
        public string BancoCodigoFormatado()
        {
            return BancoCodigo.ToString("000");
        }

        public string Left(string lValue, int lLength)
        {
            
            if (lValue.Length > lLength) return lValue.Substring(0, lLength);
            else return lValue;
        }
    }
}
