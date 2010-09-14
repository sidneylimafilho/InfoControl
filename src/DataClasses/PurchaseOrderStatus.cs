namespace Vivina.Erp.DataClasses
{
    public partial class PurchaseOrderStatus
    {
        public const int InProcess = 1;
        public const int SentToSupplier = 2;
        public const int Bought = 3;
        public const int Concluded = 4;
        public const int WaitingforApproval = 5;
        public const int Approved = 6;
        public const int Reproved = 7;
        public const int SentToBuyerCentral = 8;
    }
}