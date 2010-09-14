using InfoControl;

namespace Vivina.Erp.BusinessRules.Accounting
{
    public interface IAccountSearch
    {
        int? AccountPlanId { get; set; }
        int? CostCenterId { get; set; }
        int? ParcelStatus { get; set; }
        DateTimeInterval dateTimeInterval { get; set; }
        string Name { get; set; }
        decimal? ParcelValue { get; set; }
        string Identification { get; set; }
        int? AccountId { get; set; }
    }
}