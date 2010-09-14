using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System;

namespace Vivina.Erp.DataClasses
{
    public partial class PaymentMethod
    {
        public const int Cash = 1;
        public const int Maestro = 2;
        public const int MasterCard = 3;
        public const int Visa = 4;
        public const int VisaElectron = 5;
        public const int Boleto = 6;
        public const int CreditNote = 7;
        public const int Check = 8;
        public const int AccountWithDraw = 9;
        public const int OnLineTransfer = 10;

        public bool IsCash { get { return PaymentMethodId == Cash; } }
        public bool IsMaestro { get { return PaymentMethodId == Maestro; } }
        public bool IsMasterCard { get { return PaymentMethodId == MasterCard; } }
        public bool IsVisa { get { return PaymentMethodId == Visa; } }
        public bool IsVisaElectron { get { return PaymentMethodId == VisaElectron; } }
        public bool IsBoleto { get { return PaymentMethodId == Boleto; } }
        public bool IsCreditNote { get { return PaymentMethodId == CreditNote; } }
        public bool IsCheck { get { return PaymentMethodId == Check; } }
        public bool IsAccountWithDraw { get { return PaymentMethodId == AccountWithDraw; } }
        public bool IsOnLineTransfer { get { return PaymentMethodId == OnLineTransfer; } }

    }

    //public partial class PaymentMethodType
    //{
    //    public const int CashOrOnLineTransfer = 1;
    //    public const int CreditCard = 2;
    //    public const int Boleto = 3;
    //    public const int LongTermBill = 4;
    //}
}

