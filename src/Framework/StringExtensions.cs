using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace InfoControl
{
    public static partial class StringExtensions
    {
        public static string FormatAll(this string identifier, params object[] args)
        {
            return identifier = string.Format(identifier, args);
        }
        public static byte[] ToUtf8Bytes(this string identifier)
        {
            return Encoding.UTF8.GetBytes(identifier);
        }

        public static bool IsValidMail(this string text)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(text, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }

        public static string ToHtml(this string s)
        {
            var sb = new StringBuilder();
            var output = new StringWriter(sb);

            // const char ch = '\0';
            if (s != null)
            {
                int length = s.Length;
                for (int i = 0; i < length; i++)
                {
                    char ch2 = s[i];
                    switch (ch2)
                    {
                        case '\n':
                            output.Write("<br>");
                            break;

                        case '\r':
                            break;

                        case ' ':
                            break;

                        case '"':
                            output.Write("&quot;");
                            break;

                        case '&':
                            output.Write("&amp;");
                            break;

                        case '<':
                            output.Write("&lt;");
                            break;

                        case '>':
                            output.Write("&gt;");
                            break;

                        default:
                            if ((ch2 >= '\x00a0') && (ch2 < 'Ā'))
                            {
                                output.Write("&#");
                                output.Write(((int)ch2).ToString(NumberFormatInfo.InvariantInfo));
                                output.Write(';');
                            }
                            else
                                output.Write(ch2);

                            break;
                    }
                }
            }

            return sb.ToString();
        }

        public static string ToRtf(this string html)
        {
            return new HtmlToRtfConverter().ConvertString(html);
        }

        public static string RemoveAccent(this string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                // A
                text = text.Replace("Ã", "A");
                text = text.Replace("À", "A");
                text = text.Replace("Á", "A");
                text = text.Replace("Â", "A");
                text = text.Replace("ã", "a");
                text = text.Replace("à", "a");
                text = text.Replace("á", "a");
                text = text.Replace("â", "a");

                // E
                text = text.Replace("É", "E");
                text = text.Replace("Ê", "E");
                text = text.Replace("é", "e");
                text = text.Replace("ê", "e");

                //I
                text = text.Replace("Í", "I");
                text = text.Replace("Î", "I");
                text = text.Replace("í", "i");
                text = text.Replace("î", "i");

                // O
                text = text.Replace("Õ", "O");
                text = text.Replace("Ó", "O");
                text = text.Replace("Ô", "O");
                text = text.Replace("õ", "o");
                text = text.Replace("ó", "o");
                text = text.Replace("ô", "o");

                //I
                text = text.Replace("Ú", "U");
                text = text.Replace("Û", "U");
                text = text.Replace("ú", "u");
                text = text.Replace("û", "u");

                //Others
                text = text.Replace("Ç", "C");
                text = text.Replace("ç", "c");
            }
            return text;
        }

        public static string RemoveSpecialChars(this string text)
        {
            text = RemoveAccent(text);


            text = text.Replace(".", "");
            text = text.Replace("@", "");
            text = text.Replace("!", "");
            text = text.Replace("?", "");
            text = text.Replace("~", "");
            text = text.Replace("`", "");
            text = text.Replace("'", "");
            text = text.Replace("\"", "");
            text = text.Replace("#", "");
            text = text.Replace("$", "");
            text = text.Replace("%", "");
            text = text.Replace("^", "");
            text = text.Replace("&", "");
            text = text.Replace("*", "");
            text = text.Replace("(", "");
            text = text.Replace(")", "");
            text = text.Replace("¡", "");
            text = text.Replace("²", "");
            text = text.Replace("³", "");
            text = text.Replace("¤", "");
            text = text.Replace("€", "");
            text = text.Replace("¼", "");
            text = text.Replace("½", "");
            text = text.Replace("¾", "");
            text = text.Replace("‘", "");
            text = text.Replace("’", "");
            text = text.Replace("¥", "");
            text = text.Replace("×", "");
            text = text.Replace("«", "");
            text = text.Replace("»", "");
            text = text.Replace("´", "");
            text = text.Replace("¶", "");
            text = text.Replace("¿", "");
            text = text.Replace("ç", "");
            text = text.Replace("µ", "");
            text = text.Replace("ñ", "");
            text = text.Replace("©", "");
            text = text.Replace("æ", "");
            text = text.Replace("®", "");
            text = text.Replace("þ", "");
            text = text.Replace("ß", "");
            text = text.Replace("ð", "");
            text = text.Replace("ø", "");
            text = text.Replace("¬", "");
            text = text.Replace(":", "");
            text = text.Replace(";", "");
            text = text.Replace(",", "");
            text = text.Replace("+", "");
            text = text.Replace("*", "");
            text = text.Replace("/", "");
            text = text.Replace("=", "");
            text = text.Replace("\"", "");
            text = text.Replace("|", "");
            text = text.Replace("\\", "");
            text = text.Replace(" ", "-");

            return text;
        }

        /// <summary>
        /// Convert the first letter to Uppercase each word
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToCapitalize(this string text)
        {
            string[] list = text.Split(' ');
            for (int i = 0; i < list.Length; i++)
                if (list[i].Length > 1)
                    list[i] = Char.ToUpper(list[i][0]) + list[i].Substring(1).ToLower();

            return String.Join(" ", list);
        }

        /// <summary>
        /// Short a string and append ...
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxChars"></param>
        /// <returns></returns>
        public static string Shortly(this string text, int maxChars)
        {
            if (!String.IsNullOrEmpty(text))
                return text.Length > maxChars ? text.Substring(0, maxChars) + "..." : text;
            return null;
        }

        /// <summary>
        /// String.Join function as Extension
        /// </summary>
        /// <param name="array"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join(this string[] array, string separator)
        {
            return String.Join(separator, array);
        }

        /// <summary>
        /// Remove Domain prefix name
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static string RemoveDpnInUrl(this string domain)
        {
            // Check null value
            domain = (domain ?? "").ToLower();

            domain = domain.Replace(".br", "");
            domain = domain.Replace(".com", "");
            domain = domain.Replace(".org", "");
            domain = domain.Replace(".net", "");
            domain = domain.Replace(".biz", "");
            domain = domain.Replace(".eti", "");
            domain = domain.Replace(".adv", "");
            domain = domain.Replace("http://", "");
            domain = domain.Replace("www.", "");
            domain = domain.Replace(".vivina", "");
            domain = domain.Replace("vivina", "");
            domain = domain.Replace("infocontrol", "");
            domain = domain.Replace("pooba", "");
            domain = domain.Replace(".localhost", "");

            return domain;
        }
    }
}