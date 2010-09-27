using System;
using System.Web;
using InfoControl;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.SystemFramework
{
    public static class Util
    {
        /// <summary>
        /// Erase the format text that MaskedEdit put
        /// </summary>
        /// <param name="textbox"></param>
        /// <returns></returns>
        public static string RemoveMask(this string textbox)
        {
            return (textbox ?? "").Replace("(__)", "") // Phone
                .Replace("_/_", "") // DateTime
                .Replace("_", "")
                .Replace(".", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GenerateUniqueID()
        {
            return DateTime.Now.ToString("yyMMddHHmmss");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string GenerateWebPageUrl(object page)
        {
            if (page != null)
            {
                var webPage = page as WebPage;

                string seoText = "";
                while ((webPage = webPage.WebPage1) != null)
                    seoText = webPage.Name.Trim().RemoveSpecialChars() + "/" + seoText;

                return GenerateUrl((page as WebPage).WebPageId,
                                   (page as WebPage).Name,
                                   (page as WebPage).RedirectUrl,
                                   seoText);
            }

            return "";
        }

      


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string GenerateUrl(object id, object text, object redirectUrl, string page)
        {
            text = text ?? "";

            if (String.IsNullOrEmpty(Convert.ToString(redirectUrl)))
                return "~/site/" + page + HttpUtility.UrlEncode(text.ToString().RemoveSpecialChars()) + "," + id + ".aspx";

            return Convert.ToString(redirectUrl);
        }



        /// <summary>
        /// Função criada para testes que necessitem de um cnpj difenrente para cada registro
        /// </summary>
        /// <returns></returns>
        public static string CnpjGenerator()
        {
            var cnpj = new int[14];
            string result = String.Empty;

            var r = new Random();

            //
            // Preenche os 8 primeiros dígitos com números aleatórios 
            //
            for (int i = 0; i < 8; i++)
            {
                cnpj[i] = r.Next(10);
            }

            //
            // Preenche os últimos 4 dígitos antes dos dígitos verificadores com o padrão /'0001'.
            //
            cnpj[8] = cnpj[9] = cnpj[10] = 0;
            cnpj[11] = 1;

            //
            // Cálculo para extrair o 1º dígito verificador
            //
            cnpj[12] = 5 * cnpj[0] + 4 * cnpj[1] + 3 * cnpj[2] + 2 * cnpj[3];
            cnpj[12] += 9 * cnpj[4] + 8 * cnpj[5] + 7 * cnpj[6] + 6 * cnpj[7];
            cnpj[12] += 5 * cnpj[8] + 4 * cnpj[9] + 3 * cnpj[10] + 2 * cnpj[11];
            cnpj[12] = 11 - cnpj[12] % 11;
            if (cnpj[12] >= 10)
                cnpj[12] = 0;


            //
            // Cálculo para extrair o 2º dígito verificador
            //
            cnpj[13] = 6 * cnpj[0] + 5 * cnpj[1] + 4 * cnpj[2] + 3 * cnpj[3];
            cnpj[13] += 2 * cnpj[4] + 9 * cnpj[5] + 8 * cnpj[6] + 7 * cnpj[7];
            cnpj[13] += 6 * cnpj[8] + 5 * cnpj[9] + 4 * cnpj[10] + 3 * cnpj[11];
            cnpj[13] += 2 * cnpj[12];
            cnpj[13] = 11 - cnpj[13] % 11;
            if (cnpj[13] >= 10)
                cnpj[13] = 0;

            foreach (int item in cnpj)
            {
                result += Convert.ToString(item);
            }

            return result;
        }

        /// <summary>
        /// Função criada para testes que necessitem de um Email difenrente para cada registro
        /// </summary>
        /// <returns></returns>
        public static string EmailGenerator()
        {
            string email = String.Empty;
            Random r = new Random();

            //
            // Preenche o email com números e os converte para os respectivos caracteres da tabela ascii
            //
            for (int i = 0; i < 19; i++)
            {
                if (i == 10)
                {
                    email += "@";
                    i++;
                }

                email += Convert.ToChar(r.Next(97, 122)).ToString();
            }

            email += ".com";

            return email;
        }
    }
}