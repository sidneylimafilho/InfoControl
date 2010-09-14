using System;
using System.Collections.Generic;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class ContractManager : BusinessManager<InfoControlDataContext>
    {
        #region Enums

        public enum ContractStatus
        {
            Accepted = 1,
            Unaccepted,
            Cancelled,
            Signed,
            Suspended,
            InAnalise,
            Pendent,
            Removed
        }

        public enum ContractValidationStatus
        {
            Valid = 1,
            GeneratedAsPendent,
            Invalid
        }

        #endregion

        public ContractManager(IDataAccessor container)
            : base(container)
        {
        }

        #region GetMethods

        /// <summary>
        /// this method returns a Iqueryable by some parameters
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="contractTypeId"></param>
        /// <param name="contractStatusId"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        /// 
        public IQueryable GetContracts(DateTimeInterval durationIntervalDate,
                                       Int32? contractTypeId, Int32? contractStatusId, Int32? customerId,
                                       Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            var query = from contract in DbContext.Contracts
                        where contract.CompanyId == companyId
                        join customer in DbContext.Customers on contract.CustomerId equals customer.CustomerId
                        join profile in DbContext.Profiles on customer.ProfileId equals profile.ProfileId into gProfiles
                        from profile in gProfiles.DefaultIfEmpty()
                        join legalEntityProfile in DbContext.LegalEntityProfiles on customer.LegalEntityProfileId equals
                            legalEntityProfile.LegalEntityProfileId into gLegalEntityProfile
                        from legalEntityProfile in gLegalEntityProfile.DefaultIfEmpty()
                        select new
                                   {
                                       contract.ContractId,
                                       contract.CompanyId,
                                       contract.CustomerId,
                                       contract.BeginDate,
                                       contract.HH,
                                       contract.ExpiresDate,
                                       contract.FinancierOperationId,
                                       contract.InterestDeferredPayment,
                                       contract.Penalty,
                                       contract.ContractValue,
                                       contract.AdditionalValue1,
                                       contract.AdditionalValue2,
                                       contract.AdditionalValue3,
                                       contract.AdditionalValue4,
                                       contract.AdditionalValue5,
                                       contract.InvoiceId,
                                       contract.Observation,
                                       contract.ContractTypeId,
                                       contract.ContractStatusId,
                                       contract.FirstParcelDueDate,
                                       contract.Periodicity,
                                       contract.Parcels,
                                       contract.ContractNumber,
                                       contract.FinancierConditionId,
                                       contract.RepresentantId,
                                       contract.EmployeeId,
                                       CustomerName = legalEntityProfile.CompanyName ?? profile.Name,
                                       ContractType =
                            contract.ContractType == null ? String.Empty : contract.ContractType.Name
                                   };

            if (durationIntervalDate != null)
                query =
                    query.Where(
                        contract =>
                        contract.BeginDate >= durationIntervalDate.BeginDate &&
                        contract.ExpiresDate <= durationIntervalDate.EndDate);

            if (contractTypeId.HasValue)
                query = query.Where(contract => contract.ContractStatusId == contractStatusId);
            if (contractStatusId.HasValue)
                query = query.Where(contract => contract.ContractStatusId == contractStatusId);
            if (customerId.HasValue)
                query = query.Where(contract => contract.CustomerId == customerId);

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "ContractId");
        }

        /// <summary>
        /// This method retrieves all documentsTemplates of contract in specified company
        /// </summary>
        /// <param name="companyId"> can't be bull</param>
        /// <returns></returns>
        public IQueryable<DocumentTemplate> GetContractTemplatesByCompany(Int32 companyId)
        {
            return DbContext.DocumentTemplates.Where(documentTemplate => documentTemplate.CompanyId == companyId
                && documentTemplate.DocumentTemplateTypeId == Convert.ToInt32(DocumentTemplateTypes.Contract));
        }


        /// <summary>
        /// this is the count method GetContracts(DateTime? BeginIntervalFirstDate, DateTime? BeginIntervalLastDate, DateTime? ExpiresIntervalFirstDate, DateTime? ExpiresIntervalLastDate, Int32? contractTypeId, Int32? contractStatusId, Int32? customerId, Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        /// </summary>
        /// <param name="BeginIntervalFirstDate"></param>
        /// <param name="BeginIntervalLastDate"></param>
        /// <param name="ExpiresIntervalFirstDate"></param>
        /// <param name="ExpiresIntervalLastDate"></param>
        /// <param name="contractTypeId"></param>
        /// <param name="contractStatusId"></param>
        /// <param name="customerId"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetContractsCount(DateTimeInterval durationIntervalDate, Int32? contractTypeId, Int32? contractStatusId, Int32? customerId,
                                       Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetContracts(durationIntervalDate, contractTypeId, contractStatusId, customerId,
                             companyId, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method returns contracr by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetContractByCompany(Int32 companyId, string sortExpression, int startRowIndex,
                                               int maximumRows)
        {
            return GetContracts(null, null, null, null, companyId, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this methods returns contracts by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<Contract> GetContractsByCompany(Int32 companyId, string sortExpression, int startRowIndex,
                                                          int maximumRows)
        {
            return DbContext.Contracts.Where(contract => contract.CompanyId == companyId).SortAndPage(sortExpression,
                                                                                                      startRowIndex,
                                                                                                      maximumRows,
                                                                                                      "ContractId");
        }

        /// <summary>
        /// this is the count method of GetContractsByCompany
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetContractsByCompanyCount(Int32 companyId, string sortExpression, int startRowIndex,
                                                int maximumRows)
        {
            return GetContractsByCompany(companyId, sortExpression, startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// This method return all contracts by company
        /// </summary>
        /// <param name="companyId">companyId</param>
        /// <returns></returns>
        public IQueryable<Contract> RetrieveContractsByCustomer(int companyId, int customerId)
        {
            return DbContext.Contracts.Where(x => x.CompanyId == companyId && x.CustomerId == customerId);
        }

        public IQueryable<Contract> RetrieveUnassociatedContractsByCustomer(int companyId, int customerId)
        {
            return DbContext.Contracts.Where(x => x.CompanyId == companyId && x.CustomerId == customerId);
        }

        /// <summary>
        /// this method return contracts by Customer
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<Contract> RetrieveContractsByCustomer(int companyId, int customerId, string sortExpression,
                                                                int startRowIndex, int maximumRows)
        {
            return
                DbContext.Contracts.Where(c => c.CompanyId == companyId && c.CustomerId == customerId).SortAndPage(
                    sortExpression, startRowIndex, maximumRows, "ContractId");
        }

        /// <summary>
        /// This method returns quantity of contracts by customer
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int RetrieveContractsByCustomerCount(int companyId, int customerId, string sortExpression,
                                                    int startRowIndex, int maximumRows)
        {
            return
                RetrieveContractsByCustomer(companyId, customerId, sortExpression ?? "", startRowIndex, maximumRows).
                    Count();
        }

        #endregion

        /// <summary>
        /// This method insert a new contract on the database 
        /// </summary>
        /// <param name="entity"></param>
        public ContractValidationStatus InsertContract(Contract entity, Int32 qtdParcels,
                                                       List<ContractAssociated> contractAssociated)
        {
            ContractValidationStatus statusValidationContract = ValidateContract(entity);
            if (statusValidationContract == ContractValidationStatus.Invalid)
                return statusValidationContract;

            DbContext.Contracts.InsertOnSubmit(entity);
            DbContext.SubmitChanges();

            ProcessContract(entity, qtdParcels);

            SaveContractAssociateds(contractAssociated, entity.ContractId);

            return statusValidationContract;
        }

#warning retirar o Get deste método e usar attach
        /// <summary>
        /// This method delete a contract of the database
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteContract(Contract entity)
        {
            DbContext.Contracts.DeleteOnSubmit(GetContract(entity.CompanyId, entity.ContractId));
            DbContext.SubmitChanges();
        }

#warning retirar o Get deste método e usar attach
        /// <summary>
        /// This method updates a contract on the database
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        public ContractValidationStatus UpdateContract(Contract original_entity, Contract entity, int qtdParcels,
                                                       List<ContractAssociated> contractAssociated)
        {
            ContractValidationStatus statusValidationContract = ValidateContract(entity);
            if (statusValidationContract == ContractValidationStatus.Invalid)
                return statusValidationContract;

            original_entity = GetContract(original_entity.CompanyId, original_entity.ContractId);
            original_entity.CopyPropertiesFrom(entity);

            DbContext.SubmitChanges();

            ProcessContract(original_entity, qtdParcels);


            DbContext.ContractAssociateds.DeleteAllOnSubmit(original_entity.ContractAssociateds);
            DbContext.SubmitChanges();

            SaveContractAssociateds(contractAssociated, original_entity.ContractId);


            return statusValidationContract;
        }


        /// <summary>
        /// This method saves a list of contractAssociated
        /// </summary>
        /// <param name="contractAssociated"> Can't be null</param>
        /// <param name="contractID"> Can't be null</param>
        private void SaveContractAssociateds(List<ContractAssociated> contractAssociated, Int32 contractID)
        {
            foreach (ContractAssociated item in contractAssociated)
            {
                item.ContractId = contractID;
                DbContext.ContractAssociateds.InsertOnSubmit(item);
            }
            DbContext.SubmitChanges();
        }

        private void ProcessContract(Contract contract, int qtdParcels)
        {
            if (IsReadyToGenerateLoanFromContract(contract))
            {
                //GenerateLoanFromContract(contract, qtdParcels);
                DbContext.SubmitChanges();
            }
        }

        /// <summary>
        /// this method check is the contract is ready to generate its invoice
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Boolean IsReadyToGenerateLoanFromContract(Contract entity)
        {
            return (entity.ContractStatusId == (Int32)ContractStatus.Accepted && entity.InvoiceId == null);
        }


        /// <summary>
        /// Generate All Invoices to a contract
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        //private void GenerateLoanFromContract(Contract contract, int qtdParcels)
        //{
        //    AccountManager accountManager = new AccountManager(this);


        //    //contract.FinancierOperation = accountManager.GetFinancierOperation(contract.CompanyId, Convert.ToInt32(contract.FinancierOperationId));

        //    contract.FinancierCondition = accountManager.GetFinancierConditionByParcelCount(contract.CompanyId, Convert.ToInt32(contract.FinancierOperationId), qtdParcels) ?? new FinancierCondition();


        //    if (contract.FinancierOperation.Brokerage.HasValue && contract.FinancierOperation.Brokerage.Value)
        //    {
        //        GenerateBrokerCommission(contract);
        //        GenerateRepresententBrokerComission(contract);
        //    }
        //    else
        //    {
        //        GenerateMonthlyInvoicesFromLoan(contract);
        //        GenerateBillToCustomerFromLoan(contract);
        //    }
        //}

        #region Calculate Commission

        //private void GenerateBrokerCommission(Contract contract)
        //{
        //    InvoicesManager manager = new InvoicesManager(this);


        //    Invoice invoice = new Invoice();
        //    invoice.CompanyId = contract.CompanyId;
        //    invoice.CustomerId = contract.CustomerId;
        //    invoice.EntryDate = contract.FirstParcelDueDate.Value.AddDays(contract.FinancierOperation.ReceiveDay).NextUtilDay();
        //    //invoice.Creditor = !String.IsNullOrEmpty(contract.ContractNumber) ? contract.ContractNumber : contract.ContractId.ToString();
        //    invoice.Description = "Comissão Contrato N. " + contract.ContractNumber;
        //    //invoice.InvoiceValue = Decimal.Zero;

        //    if (contract.InvoiceId.HasValue)
        //    {
        //        Invoice original_invoice = manager.GetInvoice(contract.CompanyId, contract.InvoiceId.Value);
        //        new ParcelsManager(this).DeleteAllNextInvoiceParcels(invoice, DateTime.Now);
        //        manager.Update(original_invoice, invoice);
        //    }
        //    else
        //    {
        //        //manager.Insert(invoice, null);
        //        manager.InsertRetrievingId(invoice);
        //        contract.InvoiceId = invoice.InvoiceId;
        //    }
        //    GenerateBrokerCommissionParcel(contract, invoice);


        //    DbContext.SubmitChanges();
        //}

        //private Parcel GenerateBrokerCommissionParcel(Contract contract, Invoice invoice)
        //{
        //    if (contract.FinancierOperation == null)
        //        return null;

        //    Parcel parcel = new Parcel();
        //    parcel.FinancierOperationId = contract.FinancierOperationId;
        //    parcel.CompanyId = contract.CompanyId;
        //    parcel.Description = "1/1";
        //    parcel.DueDate = contract.FirstParcelDueDate.Value.AddDays(contract.FinancierOperation.ReceiveDay).NextUtilDay(); ;

        //    parcel.Amount = CalculateNetWorth(contract) * contract.FinancierCondition.PercentCompany / 100m;

        //    if (contract.Insurance.HasValue) //seguro
        //        parcel.Amount += contract.Insurance.Value * Convert.ToDecimal(contract.FinancierOperation.InsurancePercent) / 100m;

        //    if (contract.MoneyReserves.HasValue) //pecúlio
        //        parcel.Amount += contract.MoneyReserves.Value * Convert.ToDecimal(contract.FinancierOperation.PeculiumPercent) / 100m;

        //    if (contract.MonthlyFee.HasValue)// mensalidade
        //        parcel.Amount += (contract.MonthlyFee.Value * contract.FinancierCondition.ParcelCount) * Convert.ToDecimal(contract.FinancierOperation.MonthlyFeePercent) / 100m;

        //    //invoice.InvoiceValue += parcel.Amount;
        //    parcel.InvoiceId = invoice.InvoiceId;
        //    new InvoicesManager(this).Insert(parcel, contract.FinancierCondition);

        //    return parcel;
        //}

        //private void GenerateRepresententBrokerComission(Contract contract)
        //{
        //    if (contract.RepresentantId.HasValue)
        //    {
        //        BillManager billManager = new BillManager(this);

        //        Bill bill = new Bill();
        //        bill.CompanyId = contract.CompanyId;
        //        bill.DocumentType = (Int32)DocumentType.others;
        //        bill.EntryDate = contract.FirstParcelDueDate.Value.AddDays(contract.FinancierOperation.ReceiveDay).NextUtilDay();

        //        //if (contract.RepresentantId.HasValue)
        //        //{
        //        //   // bill.Creditor = (contract.Representant.Profile != null ? contract.Representant.Profile.AbreviatedName : contract.Representant.LegalEntityProfile.CompanyName);
        //        //    bill.Description = "Comissão »" + bill.Creditor;
        //        //}


        //        billManager.Insert(bill, null);

        //        GenerateRepresententBrokerComissionParcel(contract, bill);

        //        DbContext.SubmitChanges();
        //    }
        //}

        //private Parcel GenerateRepresententBrokerComissionParcel(Contract contract, Bill bill)
        //{
        //    if (contract.FinancierOperation == null)
        //        return null;

        //    Parcel parcel = new Parcel();
        //    parcel.FinancierOperationId = contract.FinancierOperationId;
        //    parcel.CompanyId = contract.CompanyId;
        //    parcel.Description = "1/1";
        //    parcel.DueDate = contract.FirstParcelDueDate.Value.AddDays(contract.FinancierOperation.ReceiveDay).NextUtilDay(); ;

        //    parcel.Amount = CalculateNetWorth(contract) * contract.FinancierCondition.PercentRepresentant / 100m;

        //    if (contract.Insurance.HasValue) //seguro
        //        parcel.Amount += contract.Insurance.Value * Convert.ToDecimal(contract.FinancierOperation.InsurancePercentPerRepresentant) / 100m;

        //    if (contract.MoneyReserves.HasValue) //pecúlio
        //        parcel.Amount += contract.MoneyReserves.Value * Convert.ToDecimal(contract.FinancierOperation.PeculiumPercentPerRepresentant) / 100m;

        //    if (contract.MonthlyFee.HasValue)// mensalidade
        //        parcel.Amount += (contract.MonthlyFee.Value * contract.FinancierCondition.ParcelCount) * Convert.ToDecimal(contract.FinancierOperation.MonthlyFeePercentPerRepresentant) / 100m;

        //    //bill.BillValue += parcel.Amount;
        //    parcel.BillId = bill.BillId;
        //    new BillManager(this).Insert(parcel, contract.FinancierCondition);
        //    return parcel;
        //}

        #endregion

        private void GenerateBillToCustomerFromLoan(Contract contract)
        {
            var billManager = new FinancialManager(this);
            var parcelsManager = new ParcelsManager(this);
            var customerManager = new CustomerManager(this);

            var bill = new Bill();
            bill.CompanyId = contract.CompanyId;
            bill.DocumentType = (Int32)DocumentType.others;

            Customer customer = customerManager.GetCustomer(contract.CustomerId, contract.CompanyId);
            //bill.Creditor = customer.Profile != null ? customer.Profile.Name : customer.LegalEntityProfile.CompanyName;
            //bill.Description = "Empréstimo p/ " + bill.Creditor;

            //bill.BillValue = contract.ContractValue.Value - CalculateOtherDebits(contract);
            billManager.Insert(bill, null);

            //
            var parcel = new Parcel();
            parcel.FinancierOperationId = contract.FinancierOperationId;
            parcel.CompanyId = contract.CompanyId;
            parcel.BillId = bill.BillId;

            parcel.Description = "1/1";
            parcel.DueDate = contract.BeginDate.NextUtilDay();
            //parcel.Amount = bill.BillValue.Value;
            parcelsManager.Insert(parcel, null);
        }

        //private void GenerateMonthlyInvoicesFromLoan(Contract entity)
        //{
        //    InvoicesManager manager = new InvoicesManager(this);
        //    Invoice invoice = new Invoice();
        //    Invoice original_invoice = new Invoice();

        //    if (entity.InvoiceId.HasValue)
        //        original_invoice = manager.GetInvoice(entity.CompanyId, Convert.ToInt32(entity.InvoiceId));

        //    invoice.CopyPropertiesFrom(original_invoice);

        //    invoice.CompanyId = entity.CompanyId;
        //    invoice.CustomerId = entity.CustomerId;
        //    invoice.Description = !String.IsNullOrEmpty(entity.ContractNumber) ? entity.ContractNumber : entity.ContractId.ToString();
        //    invoice.InvoiceValue = decimal.Zero;

        //    if (or/ger.InsertRetrievingId(invoice);
        //    else
        //        manager.Update(original_invoice, invoice);

        //    entity.InvoiceId = invoice.InvoiceId;
        //    invoice.InvoiceValue += GenerateMonthlyInvoiceParcelFromLoan(entity);

        //    DbContext.SubmitChanges();

        //}

        /// <summary>
        /// this method generates MonthlyInvoice's parcels
        /// </summary>
        /// <param name="entity">Current Contract</param>
        /// <param name="qtdParcels">Number of parcels</param>
        /// <returns>returns the sum of invoice's parcels</returns>
        private Decimal GenerateMonthlyInvoiceParcelFromLoan(Contract entity)
        {
            DateTime dueDate = Convert.ToDateTime(entity.FirstParcelDueDate);
            Decimal parcelAmount = decimal.Zero;
            Parcel parcel;
            var invoicesManager = new FinancialManager(this);
            for (int i = 1; i <= entity.FinancierCondition.ParcelCount; i++)
            {
                if ((entity.Periodicity.Value == 30
                         ? entity.BeginDate.AddMonths(1)
                         : entity.BeginDate.AddDays(entity.Periodicity.Value)) > DateTime.Now.Date)
                {
                    parcel = CreateMonthlyInvoiceParcelFromLoan(entity,
                                                                (i / entity.FinancierCondition.ParcelCount).ToString(),
                                                                dueDate);
                    parcelAmount += parcel.Amount;
                    invoicesManager.Insert(parcel, entity.FinancierCondition);
                }

                //If days contains 30, Add a Month
                if (entity.Periodicity.Value == 30)
                    dueDate = dueDate.AddMonths(1);
                else
                    dueDate = dueDate.AddDays(entity.Periodicity.Value);
            }
            DbContext.SubmitChanges();

            return parcelAmount;
        }

        private Parcel CreateMonthlyInvoiceParcelFromLoan(Contract entity, String description, DateTime dueDate)
        {
            var parcel = new Parcel();
            parcel.FinancierOperationId = entity.FinancierOperationId;
            parcel.CompanyId = entity.CompanyId;
            parcel.InvoiceId = entity.InvoiceId;
            parcel.Description = description;
            parcel.DueDate = dueDate.NextUtilDay();
            parcel.Amount = Convert.ToDecimal(entity.ContractValue.Value * entity.FinancierCondition.MonthlyTax);

            if (entity.Insurance.HasValue)
                parcel.Amount += entity.Insurance.Value / entity.FinancierCondition.ParcelCount;

            if (entity.MoneyReserves.HasValue)
                parcel.Amount += entity.MoneyReserves.Value / entity.FinancierCondition.ParcelCount;

            if (entity.MonthlyFee.HasValue)
                parcel.Amount += entity.MonthlyFee.Value;

            return parcel;
            //invoice.InvoiceValue += parcel.Amount;
        }


        private Decimal CalculateNetWorth(Contract contract)
        {
            return (contract.ContractValue.Value * contract.FinancierCondition.MonthlyTax.Value *
                    contract.FinancierCondition.ParcelCount) - CalculateOtherDebits(contract);
        }

        private Decimal CalculateOtherDebits(Contract contract)
        {
            //
            // Calculate the Liquid Value
            //
            Decimal otherDebt = Decimal.Zero;
            foreach (ContractAssociated associated in contract.ContractAssociateds)
            {
                if (associated.ContractId.HasValue && associated.Contract.ContractValue.HasValue)
                    otherDebt += associated.Contract.ContractValue.Value;
                else
                    otherDebt += associated.Amount;
            }
            return otherDebt;
        }

        /// <summary>
        /// this method calculates a Parcel value from contract
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="paymentMethodId"></param>
        /// <param name="qtdParcel"></param>
        /// <param name="contractValue"></param>
        /// <returns></returns>
        public Decimal CalculateParcelsValue(Int32 companyId, Int32 financierOperationId, Int32 qtdParcels,
                                             Decimal contractValue)
        {
            var accountManager = new AccountManager(this);
            FinancierCondition financierCondition =
                accountManager.GetFinancierConditionByParcelCount(companyId, financierOperationId, qtdParcels) ??
                new FinancierCondition();
            return Convert.ToDecimal(contractValue * financierCondition.MonthlyTax);
        }

        /// <summary>
        /// this method return a Contract
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public Contract GetContract(Int32 companyId, Int32 contractId)
        {
            return
                DbContext.Contracts.Where(c => c.CompanyId == companyId && c.ContractId == contractId).FirstOrDefault();
        }

        /// <summary>
        /// This method return all contract type
        /// </summary>
        /// <returns></returns>
        public IQueryable<ContractType> GetAllContractType()
        {
            return DbContext.ContractTypes;
        }

        /// <summary>
        /// This method return all contract type
        /// </summary>
        /// <returns></returns>
        public IQueryable<ContractType> GetContractTypes(Int32 companyId)
        {
            return GetAllContractType().Where(ct => ct.CompanyId == companyId).OrderBy(ct => ct.Name);
        }

        /// <summary>
        /// This method return all contract status
        /// </summary>
        /// <returns></returns>
        public IQueryable<DataClasses.ContractStatus> GetAllContractStatus()
        {
            return DbContext.ContractStatus;
        }

        /// <summary>
        /// this method returns a validation status of contract
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private ContractValidationStatus ValidateContract(Contract entity)
        {
            ContractValidationStatus status = ContractValidationStatus.Valid;
            Customer customer = new CustomerManager(this).GetCustomer(entity.CustomerId, entity.CompanyId);

            if (Convert.ToDecimal(entity.ContractValue) > Convert.ToDecimal(customer.CreditLimit) &&
                entity.ContractStatusId == (Int32)ContractStatus.Accepted) /// if is other state do the Operation
            {
                entity.ContractStatusId = (Int32)ContractStatus.Pendent;

                if (entity.ContractId == 0) // if is a new Contract, create it how Pendent
                    status = ContractValidationStatus.GeneratedAsPendent;
                else
                    status = ContractValidationStatus.Invalid;
            }
            return status;
        }



        #region X-Ray

        /// <summary>
        /// this method returns amount of pendent contracts
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<Contract> GetPendentContracts(Int32 companyId)
        {
            IQueryable<Contract> query = from contract in DbContext.Contracts
                                         where
                                             contract.CompanyId == companyId &&
                                             contract.ContractStatusId == (Int32)ContractStatus.Pendent
                                         select contract;
            return query;
        }



        /// <summary>
        /// this methods retunrs contracts by Representant
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable GetContractsByRepresentant(Int32 companyId)
        {
            var query = from contract in DbContext.Contracts
                        where contract.CompanyId == companyId
                        group contract by contract.RepresentantId
                            into gContracts
                            select new
                                       {
                                           RepresentantName =
                            (gContracts.FirstOrDefault().Representant.LegalEntityProfile.CompanyName ?? "") +
                            (gContracts.FirstOrDefault().Representant.Profile.Name ?? ""),
                                           ContractQuantity = gContracts.Count(),
                                           ContractAmount =
                            gContracts.Sum(contract => (contract.ContractValue ?? 0) * (contract.Parcels ?? 0))
                                       };
            return query;
        }

        /// <summary>
        /// this method returns contracts by CustomerType
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable GetContractsByCustomerType(Int32 companyId)
        {
            var query = from contract in DbContext.Contracts
                        where contract.CompanyId == companyId
                        join customer in DbContext.Customers on contract.CustomerId equals customer.CustomerId
                        join customerType in DbContext.CustomerTypes on customer.CustomerTypeId equals
                            customerType.CustomerTypeId into gCustomerType
                        from custormerType in gCustomerType.DefaultIfEmpty()
                        group contract by contract.Customer.CustomerTypeId
                            into gContracts
                            select new
                                       {
                                           CustomerTypeName =
                            gContracts.FirstOrDefault().Customer.CustomerType.Name ?? "",
                                           ContractQuantity = gContracts.Count(),
                                           ContractAmount =
                            gContracts.Sum(contract => (contract.ContractValue ?? 0) * (contract.Parcels ?? 0))
                                       };


            return query;
        }

        #endregion

        #region ContractAssociated

        /// <summary>
        /// this method returns the associated contracts
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public IQueryable<ContractAssociated> GetAssociatedContracts(Int32 companyId, Int32 contractId)
        {
            return GetAssociatedContractsByCompany(companyId).Where(cAssociated => cAssociated.ContractId == contractId);
        }

        /// <summary>
        /// this method returns the associated contracts with Sort and Page
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="contractId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<ContractAssociated> GetAssociatedContracts(Int32 companyId, Int32 contractId,
                                                                     string sortExpression, int startRowIndex,
                                                                     int maximumRows)
        {
            return
                DbContext.ContractAssociateds.Where(
                    cAssociated => cAssociated.CompanyId == companyId && cAssociated.ContractId == contractId).
                    SortAndPage(sortExpression, startRowIndex, maximumRows, "ContractAssociatedId");
        }

        /// <summary>
        /// this is the count method of GetAssociatedContracts
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="contractId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetAssociatedContractsCount(Int32 companyId, Int32 contractId, string sortExpression,
                                                 int startRowIndex, int maximumRows)
        {
            return GetAssociatedContracts(companyId, contractId, sortExpression, startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// this method returns the associated contracts by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        private IQueryable<ContractAssociated> GetAssociatedContractsByCompany(Int32 companyId)
        {
            return DbContext.ContractAssociateds.Where(cAssociated => cAssociated.CompanyId == companyId);
        }


        /// <summary>
        /// this method deletes ContractAssociated
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteAssociatedContract(ContractAssociated entity)
        {
            DbContext.ContractAssociateds.Attach(entity);
            DbContext.ContractAssociateds.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method inserts an AssociatedContract
        /// </summary>
        /// <param name="entity"></param>
        public void InsertAssociatedContract(ContractAssociated entity)
        {
            DbContext.ContractAssociateds.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        #endregion
    }
}