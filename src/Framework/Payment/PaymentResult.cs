using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoControl.Payment
{


    public class PaymentResult
    {
        //public readonly string SalesNumber;
        //public readonly string AuthorizationNumber;
        //public readonly string VoucherNumber;
        //public readonly string SequenceNumber;
        public readonly string ErrorMessage;
        public readonly string HtmlReturn;
        public readonly bool HasError = false;

        public PaymentResult(string htmlReturn, string error)
        {
            HasError = !String.IsNullOrEmpty(error);
            ErrorMessage = error;
            HtmlReturn = htmlReturn;
        }

        public PaymentResult(string htmlReturn)
        {
            HtmlReturn = htmlReturn;
        }
    }
}
