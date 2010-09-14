using System;

namespace InfoControl.Text.RegularExpressions
{
    /// <summary>
    /// Summary description for RegexPattern.
    /// </summary>
    public class RegexLib
    {
        private RegexLib()
        {
        }

        /// <summary>
        /// Regular Expression para validação de Data no formato DD/MM/YYYY
        /// </summary>
        public const string Date =
            @"^(((0?[1-9]|[12]\d|3[01])\/(0?[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0?[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0?[1-9]|1\d|2[0-8])\/0?2\/((19|[2-9]\d)\d{2}))|(29\/0?2\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$";

        public const string Time =
            @"^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])(:([0-5]?[0-9]))?$";

        public const string Float =
            @"^[-+]?((\d{1,3}(\.\d{3})*|(\d+))(\,\d{2})?)$";

        public const string Integer =
            @"^[-+]?(\d{1,3})((\.\d{3})*|(\d*))?$";

        public const string CNPJ =
            @"^([0-9]{2,3}\.[0-9]{3}\.[0-9]{3}\/[0-9]{4}-[0-9]{2})$";

        public const string CPF =
            @"^(\d{3}\x2E\d{3}\x2E\d{3}\x2D\d{2})$";

        public const string Mail =
            @"^(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)$";

        public const string CEP =
            @"^(\d{5}(-\d{3})?)$";

    }
}
