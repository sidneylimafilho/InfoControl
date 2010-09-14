using System;
using System.Data.Services.Common;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;

namespace Vivina.Erp.DataClasses
{
    public partial class InfoControlDataContext
    {
        [Function(Name = "dbo.AccountingPlanTree", IsComposable = true)]
        public IQueryable<AccountingPlan> AccountingPlanTree([Parameter(Name = "AccPlanId", DbType = "Int")] Nullable<int> accPlanId, [Parameter(DbType = "Int")] Nullable<int> companyId)
        {
            return CreateMethodCallQuery<AccountingPlan>(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), accPlanId, companyId);
        }

        [Function(Name = "dbo.GetChildPages", IsComposable = true)]
        public IQueryable<WebPage> GetChildPages([Parameter(DbType = "Int")] Nullable<int> companyId, [Parameter(DbType = "Int")] Nullable<int> parentId)
        {
            return CreateMethodCallQuery<WebPage>(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), companyId, parentId);
        }

        [Function(Name = "dbo.GetChildCategories", IsComposable = true)]
        public IQueryable<Category> GetChildCategories([Parameter(DbType = "Int")] Nullable<int> companyId, [Parameter(DbType = "Int")] Nullable<int> parentId)
        {
            return CreateMethodCallQuery<Category>(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), companyId, parentId);
        }



    }

    #region Extension, Attributes and others


    [Serializable]
    [DataServiceKey("AccountId")]
    public partial class Account { }

    [Serializable]
    [DataServiceKey("WorkJourneyId")]
    public partial class WorkJourney { }

    [Serializable]
    [DataServiceKey("AccountingPlanId")]
    public partial class AccountingPlan { }

    [Serializable]
    [DataServiceKey("AddonInfoId")]
    public partial class AdditionalInformation { }

    [Serializable]
    [DataServiceKey("AddonInfoDataId")]
    public partial class AdditionalInformationData { }

    [Serializable]
    [DataServiceKey("PostalCode")]
    public partial class Address { }

    [Serializable]
    [DataServiceKey("AlertId")]
    public partial class Alert { }

    [Serializable]
    [DataServiceKey("AlienationId")]
    public partial class Alienation { }

    [Serializable]
    [DataServiceKey("ApplicationId")]
    public partial class Application { }

    [Serializable]
    [DataServiceKey("BankId")]
    public partial class Bank { }

    [Serializable]
    [DataServiceKey("BarCodeTypeId")]
    public partial class BarCodeType { }

    [Serializable]
    [DataServiceKey("BillId")]
    public partial class Bill { }

    [Serializable]
    [DataServiceKey("BondId")]
    public partial class Bond { }

    [Serializable]
    [DataServiceKey("BranchId")]
    public partial class Branch { }

    [Serializable]
    [DataServiceKey("BranchId", "FunctionId")]
    public partial class BranchFunction { }

    [Serializable]
    [DataServiceKey("BudgetId")]
    public partial class Budget { }

    [Serializable]
    [DataServiceKey("BudgetItemId")]
    public partial class BudgetItem { }

    [Serializable]
    [DataServiceKey("BudgetStatusId")]
    public partial class BudgetStatus { }

    [Serializable]
    [DataServiceKey("CategoryId")]
    public partial class Category { }

    [Serializable]
    [DataServiceKey("CfopId")]
    public partial class CFOP { }

    [Serializable]
    [DataServiceKey("CheckId")]
    public partial class Check { }

    [Serializable]
    [DataServiceKey("CityId")]
    public partial class City { }

    [Serializable]
    [DataServiceKey("CnaeId")]
    public partial class Cnae { }

    [Serializable]
    [DataServiceKey("CommentId")]
    public partial class Comment { }

    [Serializable]
    [DataServiceKey("CompanyId")]
    public partial class Company { }

    [Serializable]
    [DataServiceKey("CompanyConfigurationId")]
    public partial class CompanyConfiguration { }

    [Serializable]
    [DataServiceKey("CompanyId", "UserId")]
    public partial class CompanyUser { }

    [Serializable]
    [DataServiceKey("CompositeProductId")]
    public partial class CompositeProduct { }

    [Serializable]
    [DataServiceKey("ContactId")]
    public partial class Contact { }

    [Serializable]
    [DataServiceKey("ContractId")]
    public partial class Contract { }

    [Serializable]
    [DataServiceKey("ContractAssociatedId")]
    public partial class ContractAssociated { }

    [Serializable]
    [DataServiceKey("ContractPendencyId")]
    public partial class ContractPendency { }

    [Serializable]
    [DataServiceKey("ContractStatusId")]
    public partial class ContractStatus { }

    [Serializable]
    [DataServiceKey("ContractTypeId")]
    public partial class ContractType { }

    [Serializable]
    [DataServiceKey("CostCenterId")]
    public partial class CostCenter { }

    [Serializable]
    [DataServiceKey("CurrencyRateId")]
    public partial class CurrencyRate { }

    [Serializable]
    [DataServiceKey("CustomerId")]
    public partial class Customer { }

    [Serializable]
    [DataServiceKey("CustomerCallId")]
    public partial class CustomerCall { }

    [Serializable]
    [DataServiceKey("CustomerCallStatusId")]
    public partial class CustomerCallStatus { }

    [Serializable]
    [DataServiceKey("CustomerCallTypeId")]
    public partial class CustomerCallType { }

    [Serializable]
    [DataServiceKey("CustomerId", "ContactId")]
    public partial class CustomerContact { }

    [Serializable]
    [DataServiceKey("CustomerEquipmentId")]
    public partial class CustomerEquipment { }

    [Serializable]
    [DataServiceKey("CustomerFollowupId")]
    public partial class CustomerFollowup { }

    [Serializable]
    [DataServiceKey("CustomerFollowupActionId")]
    public partial class CustomerFollowupAction { }

    [Serializable]
    [DataServiceKey("CustomerTypeId")]
    public partial class CustomerType { }

    [Serializable]
    [DataServiceKey("FunctionId")]
    public partial class CustomFunction { }

    [Serializable]
    [DataServiceKey("DepositId")]
    public partial class Deposit { }

    [Serializable]
    [DataServiceKey("DocumentTemplateId")]
    public partial class DocumentTemplate { }

    [Serializable]
    [DataServiceKey("DocumentTemplateTypeId")]
    public partial class DocumentTemplateType { }

    [Serializable]
    [DataServiceKey("DropPayoutId")]
    public partial class DropPayout { }

    [Serializable]
    [DataServiceKey("EducationLevelId")]
    public partial class EducationLevel { }

    [Serializable]
    [DataServiceKey("EmployeeId")]
    public partial class Employee { }

    [Serializable]
    [DataServiceKey("EmployeeId", "AddonInfoId")]
    public partial class EmployeeAdditionalInformation { }

    [Serializable]
    [DataServiceKey("EmployeeCompetencyId")]
    public partial class EmployeeCompetency { }

    [Serializable]
    [DataServiceKey("EmployeeDependentId")]
    public partial class EmployeeDependent { }

    [Serializable]
    [DataServiceKey("EmployeeFunctionId")]
    public partial class EmployeeFunction { }

    [Serializable]
    [DataServiceKey("EmployeeFunctionHistoryId")]
    public partial class EmployeeFunctionHistory { }

    [Serializable]
    [DataServiceKey("EmployeeId")]
    public partial class EmployeeOtherSchool { }

    [Serializable]
    [DataServiceKey("EventId")]
    public partial class Event { }

    [Serializable]
    [DataServiceKey("EventStatusId")]
    public partial class EventStatus { }

    [Serializable]
    [DataServiceKey("ExpenditureAuthorizationId")]
    public partial class ExpenditureAuthorization { }

    [Serializable]
    [DataServiceKey("FamilyRendIntervalId")]
    public partial class FamilyRendInterval { }

    [Serializable]
    [DataServiceKey("FinancierConditionId")]
    public partial class FinancierCondition { }

    [Serializable]
    [DataServiceKey("FinancierOperationId")]
    public partial class FinancierOperation { }

    [Serializable]
    [DataServiceKey("FunctionId")]
    public partial class Function { }

    [Serializable]
    [DataServiceKey("InssIntervalId")]
    public partial class InssInterval { }

    [Serializable]
    [DataServiceKey("InventoryId")]
    public partial class Inventory { }

    [Serializable]
    [DataServiceKey("InventoryDropTypeId")]
    public partial class InventoryDropType { }

    [Serializable]
    [DataServiceKey("InventoryEntryTypeId")]
    public partial class InventoryEntryType { }

    [Serializable]
    [DataServiceKey("ProductId")]
    public partial class InventoryHistory { }

    [Serializable]
    [DataServiceKey("InventoryMovimentId")]
    public partial class InventoryMoviment { }

    [Serializable]
    [DataServiceKey("InventoryRMAId")]
    public partial class InventoryRMA { }

    [Serializable]
    [DataServiceKey("InventorySerialId")]
    public partial class InventorySerial { }

    [Serializable]
    [DataServiceKey("InvoiceId")]
    public partial class Invoice { }

    [Serializable]
    [DataServiceKey("IrrfIntervalId")]
    public partial class IrrfInterval { }

    [Serializable]
    [DataServiceKey("JudicialNatureId")]
    public partial class JudicialNature { }

    [Serializable]
    [DataServiceKey("LegalEntityProfileId")]
    public partial class LegalEntityProfile { }

    [Serializable]
    [DataServiceKey("ManufacturerId")]
    public partial class Manufacturer { }

    [Serializable]
    [DataServiceKey("MaritalStatusId")]
    public partial class MaritalStatus { }

    [Serializable]
    [DataServiceKey("NeighborhoodId")]
    public partial class Neighborhood { }

    [Serializable]
    [DataServiceKey("OrganizationLevelId")]
    public partial class OrganizationLevel { }

    [Serializable]
    [DataServiceKey("OtherSchoolId")]
    public partial class OtherSchool { }

    [Serializable]
    [DataServiceKey("PackageId")]
    public partial class Package { }

    [Serializable]
    [DataServiceKey("PackageAdditionalId")]
    public partial class PackageAdditional { }

    [Serializable]
    [DataServiceKey("PackageFunctionId")]
    public partial class PackageFunction { }

    [Serializable]
    [DataServiceKey("PageCategoryId")]
    public partial class PageCategory { }

    [Serializable]
    [DataServiceKey("PageTagId")]
    public partial class PageTag { }

    [Serializable]
    [DataServiceKey("ParcelId")]
    public partial class Parcel { }

    [Serializable]
    [DataServiceKey("PaymentMethodId")]
    public partial class PaymentMethod { }

    [Serializable]
    [DataServiceKey("PermissionId")]
    public partial class Permission { }

    [Serializable]
    [DataServiceKey("PermissionTypeId")]
    public partial class PermissionType { }

    [Serializable]
    [DataServiceKey("PlanId")]
    public partial class Plan { }

    [Serializable]
    [DataServiceKey("PostId")]
    public partial class Post { }

    [Serializable]
    [DataServiceKey("ProductId")]
    public partial class Product { }

    [Serializable]
    [DataServiceKey("ProductCertificateId")]
    public partial class ProductCertificate { }

    [Serializable]
    [DataServiceKey("ProductImageId")]
    public partial class ProductImage { }

    [Serializable]
    [DataServiceKey("ProductManufacturerId")]
    public partial class ProductManufacturer { }

    [Serializable]
    [DataServiceKey("ProductPackageId")]
    public partial class ProductPackage { }


    [Serializable]
    [DataServiceKey("ProductPartId")]
    public partial class ProductPart { }

    [Serializable]
    [DataServiceKey("ProfileId")]
    public partial class Profile { }

    [Serializable]
    [DataServiceKey("ProfileAddressId")]
    public partial class ProfileAddress { }

    [Serializable]
    [DataServiceKey("ProfitAssessmentId")]
    public partial class ProfitAssessment { }

    [Serializable]
    [DataServiceKey("PurchaseOrderId")]
    public partial class PurchaseOrder { }

    [Serializable]
    [DataServiceKey("PurchaseOrderItemId")]
    public partial class PurchaseOrderItem { }

    [Serializable]
    [DataServiceKey("PurchaseOrderStatusId")]
    public partial class PurchaseOrderStatus { }

    [Serializable]
    [DataServiceKey("PurchaseRequestId")]
    public partial class PurchaseRequest { }

    [Serializable]
    [DataServiceKey("PurchaseRequestItemId")]
    public partial class PurchaseRequestItem { }

    [Serializable]
    [DataServiceKey("QuotationId")]
    public partial class Quotation { }

    [Serializable]
    [DataServiceKey("QuotationItemId")]
    public partial class QuotationItem { }

    [Serializable]
    [DataServiceKey("ReceiptId")]
    public partial class Receipt { }

    [Serializable]
    [DataServiceKey("ReceiptFieldConfigurationId")]
    public partial class ReceiptFieldConfiguration { }

    [Serializable]
    [DataServiceKey("ReceiptItemId")]
    public partial class ReceiptItem { }

    [Serializable]
    [DataServiceKey("ReportColumnId")]
    public partial class ReportColumn { }

    [Serializable]
    [DataServiceKey("ReportColumnsSchemaId")]
    public partial class ReportColumnsSchema { }

    [Serializable]
    [DataServiceKey("ReportDataFunctionId")]
    public partial class ReportDataFunction { }

    [Serializable]
    [DataServiceKey("ReportDataTypeId")]
    public partial class ReportDataType { }

    [Serializable]
    [DataServiceKey("ReportFilterId")]
    public partial class ReportFilter { }

    [Serializable]
    [DataServiceKey("ReportFilterTypeId")]
    public partial class ReportFilterType { }

    [Serializable]
    [DataServiceKey("ReportId")]
    public partial class Report { }

    [Serializable]
    [DataServiceKey("ReportSortId")]
    public partial class ReportSort { }

    [Serializable]
    [DataServiceKey("ReportTablesSchemaId")]
    public partial class ReportTablesSchema { }

    [Serializable]
    [DataServiceKey("RepresentantId")]
    public partial class Representant { }

    [Serializable]
    [DataServiceKey("RepresentantUserId")]
    public partial class RepresentantUser { }

    [Serializable]
    [DataServiceKey("RoleId")]
    public partial class Role { }

    [Serializable]
    [DataServiceKey("SaleId")]
    public partial class Sale { }

    [Serializable]
    [DataServiceKey("SaleItemId")]
    public partial class SaleItem { }

    [Serializable]
    [DataServiceKey("SaleStatusId")]
    public partial class SaleStatus { }

    [Serializable]
    [DataServiceKey("ScheduledTaskId")]
    public partial class ScheduledTask { }

    [Serializable]
    [DataServiceKey("ServiceId")]
    public partial class Service { }

    [Serializable]
    [DataServiceKey("ServiceOrderId")]
    public partial class ServiceOrder { }

    [Serializable]
    [DataServiceKey("ServiceOrderBookId")]
    public partial class ServiceOrderBook { }

    public partial class ServiceOrderEquipmentDamage { }

    [Serializable]
    [DataServiceKey("ServiceOrderHaltTypeId")]
    public partial class ServiceOrderHaltType { }

    [Serializable]
    [DataServiceKey("ServiceOrderInstallTypeId")]
    public partial class ServiceOrderInstallType { }

    [Serializable]
    [DataServiceKey("ServiceOrderItemId")]
    public partial class ServiceOrderItem { }

    [Serializable]
    [DataServiceKey("ServiceOrderProductDamageId")]
    public partial class ServiceOrderProductDamage { }

    [Serializable]
    [DataServiceKey("ServiceOrderProductTypeId")]
    public partial class ServiceOrderProductType { }

    [Serializable]
    [DataServiceKey("ServiceOrderStatusId")]
    public partial class ServiceOrderStatus { }

    [Serializable]
    [DataServiceKey("ServiceOrderTestId")]
    public partial class ServiceOrderTest { }

    [Serializable]
    [DataServiceKey("ServiceOrderTypeId")]
    public partial class ServiceOrderType { }

    [Serializable]
    [DataServiceKey("ServiceTypeId")]
    public partial class ServiceType { }

    [Serializable]
    [DataServiceKey("SexId")]
    public partial class Sex { }

    [Serializable]
    [DataServiceKey("ShiftId")]
    public partial class Shift { }

    [Serializable]
    [DataServiceKey("StateId")]
    public partial class State { }

    [Serializable]
    [DataServiceKey("StatementId")]
    public partial class Statement { }

    [Serializable]
    [DataServiceKey("StatementItemId")]
    public partial class StatementItem { }

    [Serializable]
    [DataServiceKey("StatusHistoryId")]
    public partial class StatusHistory { }

    [Serializable]
    [DataServiceKey("SupplierId")]
    public partial class Supplier { }

    [Serializable]
    [DataServiceKey("SupplierCategoryId")]
    public partial class SupplierCategory { }

    [Serializable]
    [DataServiceKey("SupplierContactId")]
    public partial class SupplierContact { }

    [Serializable]
    [DataServiceKey("SystemParameterId")]
    public partial class SystemParameter { }

    [Serializable]
    [DataServiceKey("TaskId")]
    public partial class Task { }

    [Serializable]
    [DataServiceKey("TaskStatusId")]
    public partial class TaskStatus { }

    [Serializable]
    [DataServiceKey("TaskUserId")]
    public partial class TaskUser { }

    [Serializable]
    [DataServiceKey("TransporterId")]
    public partial class Transporter { }

    [Serializable]
    [DataServiceKey("UserActivityLogId")]
    public partial class UserActivityLog { }

    [Serializable]
    [DataServiceKey("UserId")]
    public partial class User { }

    [Serializable]
    [DataServiceKey("UsersInRoleId")]
    public partial class UsersInRole { }

    [Serializable]
    [DataServiceKey("VacationIntervalId")]
    public partial class VacationInterval { }

    [Serializable]
    [DataServiceKey("WebPageId")]
    public partial class WebPage { }


    #endregion
}