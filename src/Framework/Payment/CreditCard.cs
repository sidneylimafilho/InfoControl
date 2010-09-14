using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoControl.Payment
{
    public class CreditCard
    {
        public readonly string Number;
        public readonly string Cvc2;
        public readonly string ExpirationMonth;
        public readonly string ExpirationYear;
        public readonly string CardHolder;
        public readonly int Bank;

        public CreditCard(string cardHolder, string number, string cvc2, string expirationMonth, string expirationYear, int bank)
        {
            Number = number;
            Cvc2 = cvc2;
            ExpirationMonth = expirationMonth;
            ExpirationYear = expirationYear;
            CardHolder = cardHolder;
            Bank = bank;
        }
    }
}
