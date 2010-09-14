using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vivina.Erp.DataClasses
{
    public partial class SaleStatus
    {
        public const int Paid = 1; // aguardando triagem
        public const int PendingExpedition = 2;
        public const int Delivered = 3;
        public const int WaitingPayment = 4;
        public const int Canceled = 5;
    }
}
