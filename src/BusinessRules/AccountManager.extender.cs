using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class AccountManager
    {
        #region AccountPlanDeleteStatus enum

        public enum AccountPlanDeleteStatus
        {
            Valid = 1,
            ExistsAssociatedAccountPlan,
            DeletingRegisterWithForeignKey
        }

        #endregion

        #region Accounts

        /// <summary>
        /// This method returns Accounts by companyId
        /// With the 3 other parameters, you can use Sort and Paging on the DataSource
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        //public System.Collections.IList GetAccounts(int companyId, string sortExpression, int startRowIndex, int maximumRows)
        //{
        //    //
        //    var query = from bank in DbContext.Banks
        //                join acc in DbContext.Accounts on bank.BankId equals acc.BankId
        //                where (acc.CompanyId == companyId)
        //                select new
        //                {
        //                    acc.AccountId,
        //                    acc.BankId,
        //                    acc.CompanyId,
        //                    acc.Agency,
        //                    acc.AccountNumber,
        //                    acc.PostalCode,
        //                    acc.AddressNumber,
        //                    acc.AddressComp,
        //                    acc.AgencyPhone,
        //                    acc.AgencyManager,
        //                    acc.AgencyMail,
        //                    Name = bank.Name,
        //                    acc.PermitEncash,
        //                    acc.Penalty,
        //                    acc.Interest,
        //                    acc.OperationTax,
        //                    acc.DepartmentNumber,
        //                    acc.Convention,
        //                    acc.CedenteNumber
        //                };
        //    query = query.SortAndPage(sortExpression, startRowIndex, maximumRows, "Name");
        //    return query.ToList();
        //}
        /// <summary>
        /// this method returns account by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<Account> GetAccounts(int companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return DbContext.Accounts.Where(account => account.CompanyId == companyId)
                .SortAndPage(sortExpression, startRowIndex, maximumRows, "AccountId");
        }

        /// <summary>
        /// this is the count method of GetAccounts
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetAccountsCount(int companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetAccounts(companyId, sortExpression, startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// This method returns the Accounts formatted with Bank Name + AgencyName + Account
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IList GetAccountsWithShortName(int companyId)
        {
            //
            var query = from bank in DbContext.Banks
                        join acc in DbContext.Accounts on bank.BankId equals acc.BankId
                        where (acc.CompanyId == companyId)
                        select new
                                {
                                    ShortName = (bank.ShortName ?? String.Empty) + " - " + acc.AccountNumber,
                                    acc.AccountId
                                };
            return query.ToList();
        }

        /// <summary>
        /// This method retrieves all Accounts.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Account> GetAllAccounts()
        {
            //  
            return DbContext.Accounts;
        }

        /// <summary>
        /// This method gets record counts of all Accounts.
        /// Do not change this method.
        /// </summary>
        public int GetAllAccountsCount()
        {
            return GetAllAccounts().Count();
        }

        /// <summary>
        /// This method retrieves a single Account.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=AccountId>AccountId</param>
        /// <param name=CompanyId>CompanyId</param>
        public Account GetAccount(Int32 AccountId, Int32 CompanyId)
        {
            //  
            return DbContext.Accounts.Where(x => x.AccountId == AccountId && x.CompanyId == CompanyId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves Account by Company.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<Account> GetAccountByCompany(Int32 CompanyId)
        {
            //  
            return DbContext.Accounts.Where(x => x.CompanyId == CompanyId);
        }

        /// <summary>
        /// This method retrieves Account by Bank.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=BankId>BankId</param>
        public IQueryable<Account> GetAccountByBank(Int32 BankId)
        {
            //  
            return DbContext.Accounts.Where(x => x.BankId == BankId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all Accounts filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetAccounts(string tableName, Int32 Company_CompanyId, Int32 Bank_BankId, string sortExpression,
                                 int startRowIndex, int maximumRows)
        {
            IQueryable<Account> x = GetFilteredAccounts(tableName, Company_CompanyId, Bank_BankId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "AccountId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<Account> GetFilteredAccounts(string tableName, Int32 Company_CompanyId, Int32 Bank_BankId)
        {
            switch (tableName)
            {
                case "Company_Accounts":
                    return GetAccountByCompany(Company_CompanyId);
                case "Bank_Accounts":
                    return GetAccountByBank(Bank_BankId);
                default:
                    return GetAllAccounts();
            }
        }

        /// <summary>
        /// This method gets records counts of all Accounts filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetAccountsCount(string tableName, Int32 Company_CompanyId, Int32 Bank_BankId)
        {
            IQueryable<Account> x = GetFilteredAccounts(tableName, Company_CompanyId, Bank_BankId);
            return x.Count();
        }

        /// <summary>
        /// Returns the Cost Center
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public void InsertCostCenter(CostCenter costCenter)
        {
            DbContext.CostCenters.InsertOnSubmit(costCenter);
            DbContext.SubmitChanges();
        }

        #endregion

        #region AccountPlan

        /// <summary>
        /// Return the Accounting Plan
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable GetAccountingPlan(int companyId)
        {
            return DbContext.AccountingPlans.Where(x => x.CompanyId == companyId).OrderBy(t => t.Name).ToDataTable();
        }


        /// <summary>
        /// Returns a single register from Accounting Plan
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountingPlanId"></param>
        /// <returns></returns>
        public AccountingPlan GetAccountingPlan(int companyId, int accountingPlanId)
        {
            //
            return
                DbContext.AccountingPlans.Where(x => x.AccountingPlanId == accountingPlanId && x.CompanyId == companyId)
                    .FirstOrDefault();
        }

        /// <summary>
        /// This method is a basic insert method
        /// Change this method to change how the AccountingPlan is Inserted
        /// </summary>
        /// <param name="entity"></param>
        public int InsertAccountingPlan(AccountingPlan entity)
        {
            DbContext.AccountingPlans.InsertOnSubmit(entity);
            DbContext.SubmitChanges();

            //entity.Code = GetAccountingPlan(entity.CompanyId, entity.ParentId).Code;
            if (entity.AccountingPlan1 != null && String.IsNullOrEmpty(entity.Code) && entity.ParentId.HasValue)
            {
                AccountingPlan parent = GetAccountingPlan(entity.CompanyId, Convert.ToInt32(entity.ParentId));
                parent.AccountingPlans.Load();
                entity.Code = parent.Code + "." + parent.AccountingPlans.Count();
                DbContext.SubmitChanges();
            }

            return entity.AccountingPlanId;
        }


        public AccountPlanDeleteStatus DeleteAccountingPlan(AccountingPlan entity)
        {
            if (entity.AccountingPlans.Any())
                return AccountPlanDeleteStatus.ExistsAssociatedAccountPlan;

            if (entity.Invoices.Any() || entity.Bills.Any())
                return AccountPlanDeleteStatus.DeletingRegisterWithForeignKey;


            DbContext.AccountingPlans.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
            return AccountPlanDeleteStatus.Valid;
        }


        /// <summary>
        /// This method is a basic update method
        /// Change this method to change how the AccountingPlan is Updated
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        public void UpdateAccountingPlan(AccountingPlan original_entity, AccountingPlan entity)
        {
            // DbContext.AccountingPlans.Attach(original_entity);
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Register the Accountng Plan Template for begin uses
        /// </summary>
        /// <param name="companyId"></param>
        public void RegisterAccountingPlanTemplate(int companyId)
        {
            var accountingPlanId = new Stack<int>();

            //
            // 1. Entradas
            //
            accountingPlanId.Push(
                InsertAccountingPlan(new AccountingPlan { CompanyId = companyId, Code = "1", Name = "Entradas" }));

            //
            // 1.1 Operacionais
            //
            accountingPlanId.Push(
                InsertAccountingPlan(new AccountingPlan
                                         {
                                             CompanyId = companyId,
                                             Code = "1.1",
                                             Name = "Operacionais",
                                             ParentId = accountingPlanId.Peek()
                                         }));
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.1.01",
                                         Name = "Vendas",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.1.02",
                                         Name = "Recebimentos de Contas",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.1.03",
                                         Name = "Juros Recebidos",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.1.04",
                                         Name = "Recuperação de Despesas",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.1.05",
                                         Name = "Descontos Obtidos",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.1.06",
                                         Name = "Adiantamentos de Clientes",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.1.07",
                                         Name = "Outras Entradas",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.1.08",
                                         Name = "Transferência Banco -> Caixa",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.1.09",
                                         Name = "Vendas com Cheque pré",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.1.10",
                                         Name = "Devolução de Cliente",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.1.11",
                                         Name = "Devolução de Fornecedor",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.1.12",
                                         Name = "Vendas com crédito de cliente",
                                         ParentId = accountingPlanId.Peek()
                                     });
            accountingPlanId.Pop();
            //
            // 1.2 Não Operacionais
            //
            accountingPlanId.Push(
                InsertAccountingPlan(new AccountingPlan
                                         {
                                             CompanyId = companyId,
                                             Code = "1.2",
                                             Name = "Não Operacionais",
                                             ParentId = accountingPlanId.Peek()
                                         }));
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.2.01",
                                         Name = "Venda Imobilizado",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.2.02",
                                         Name = "Outras Entradas Não Operacionais",
                                         ParentId = accountingPlanId.Peek()
                                     });
            accountingPlanId.Pop();

            //
            // 1.3 Outras Entradas
            //
            accountingPlanId.Push(
                InsertAccountingPlan(new AccountingPlan
                                         {
                                             CompanyId = companyId,
                                             Code = "1.3",
                                             Name = "Outras Entradas",
                                             ParentId = accountingPlanId.Peek()
                                         }));
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.3.01",
                                         Name = "Saldo Anterior",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.3.02",
                                         Name = "Reforço de Numerário",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "1.3.03",
                                         Name = "Transferências do Caixas",
                                         ParentId = accountingPlanId.Peek()
                                     });
            accountingPlanId.Pop();


            //
            // 2. Saidas
            //
            accountingPlanId.Push(
                InsertAccountingPlan(new AccountingPlan { CompanyId = companyId, Code = "2", Name = "Saídas" }));


            //
            // 2.1 Operacionais
            //
            accountingPlanId.Push(
                InsertAccountingPlan(new AccountingPlan
                                         {
                                             CompanyId = companyId,
                                             Code = "2.1",
                                             Name = "Operacionais",
                                             ParentId = accountingPlanId.Peek()
                                         }));

            //
            // 2.1.01 Despesas Funcionais
            //
            accountingPlanId.Push(
                InsertAccountingPlan(new AccountingPlan
                                         {
                                             CompanyId = companyId,
                                             Code = "2.1.01",
                                             Name = "Despesas Funcionais",
                                             ParentId = accountingPlanId.Peek()
                                         }));
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.01.01",
                                         Name = "Luz e Agua",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.01.02",
                                         Name = "Telefone",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.01.03",
                                         Name = "Funcionários",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.01.04",
                                         Name = "Insumos",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.01.05",
                                         Name = "Conservação",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.01.06",
                                         Name = "Veículos",
                                         ParentId = accountingPlanId.Peek()
                                     });
            accountingPlanId.Pop();

            //
            // 2.1.02 Despesas Administrativas
            //
            accountingPlanId.Push(
                InsertAccountingPlan(new AccountingPlan
                                         {
                                             CompanyId = companyId,
                                             Code = "2.1.02",
                                             Name = "Despesas Administrativas",
                                             ParentId = accountingPlanId.Peek()
                                         }));
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.02.01",
                                         Name = "Pró-Labore",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.02.02",
                                         Name = "Cartório",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.02.03",
                                         Name = "Informática",
                                         ParentId = accountingPlanId.Peek()
                                     });
            accountingPlanId.Pop();


            //
            // 2.1.03 Despesas Tributárias
            //
            accountingPlanId.Push(
                InsertAccountingPlan(new AccountingPlan
                                         {
                                             CompanyId = companyId,
                                             Code = "2.1.03",
                                             Name = "Despesas Tributárias",
                                             ParentId = accountingPlanId.Peek()
                                         }));
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.03.01",
                                         Name = "ICMS",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.03.02",
                                         Name = "ISS",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.03.03",
                                         Name = "PIS",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.03.04",
                                         Name = "COFINS",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.03.05",
                                         Name = "IRPJ",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.03.06",
                                         Name = "CSSL",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.03.07",
                                         Name = "Bombeiros",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.03.08",
                                         Name = "IPTU",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.03.09",
                                         Name = "Imposto Simples",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.03.10",
                                         Name = "Contribuição Sindical",
                                         ParentId = accountingPlanId.Peek()
                                     });
            accountingPlanId.Pop();

            //
            // 2.1.04 Mercadorias
            //
            accountingPlanId.Push(
                InsertAccountingPlan(new AccountingPlan
                                         {
                                             CompanyId = companyId,
                                             Code = "2.1.04",
                                             Name = "Mercadorias",
                                             ParentId = accountingPlanId.Peek()
                                         }));
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.04.1",
                                         Name = "Reposição de Estoque",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.04.2",
                                         Name = "Baixa de Crédito do Fornecedor",
                                         ParentId = accountingPlanId.Peek()
                                     });
            accountingPlanId.Pop();

            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.05",
                                         Name = "Descontos Concedidos",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.1.06",
                                         Name = "Acréscimos",
                                         ParentId = accountingPlanId.Peek()
                                     });
            accountingPlanId.Pop();

            //
            // 2.2 Imobilizado
            //
            accountingPlanId.Push(
                InsertAccountingPlan(new AccountingPlan
                                         {
                                             CompanyId = companyId,
                                             Code = "2.2",
                                             Name = "Imobilizado",
                                             ParentId = accountingPlanId.Peek()
                                         }));
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.2.1",
                                         Name = "Construções",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.2.2",
                                         Name = "Mobiliário e Decoração",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.2.3",
                                         Name = "Maquinário e Equipamentos",
                                         ParentId = accountingPlanId.Peek()
                                     });
            accountingPlanId.Pop();

            //
            // 2.3 Investimentos
            //
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.3",
                                         Name = "Investimentos",
                                         ParentId = accountingPlanId.Peek()
                                     });


            //
            // 2.4 Não Operacionais
            //
            accountingPlanId.Push(
                InsertAccountingPlan(new AccountingPlan
                                         {
                                             CompanyId = companyId,
                                             Code = "2.4",
                                             Name = "Não Operacionais",
                                             ParentId = accountingPlanId.Peek()
                                         }));
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.4.01",
                                         Name = "Multas",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.4.02",
                                         Name = "Reclamações Trabalhistas",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.4.03",
                                         Name = "Prejuízos Eventuais",
                                         ParentId = accountingPlanId.Peek()
                                     });
            InsertAccountingPlan(new AccountingPlan
                                     {
                                         CompanyId = companyId,
                                         Code = "2.4.03",
                                         Name = "Outras Saídas Não Operacionais",
                                         ParentId = accountingPlanId.Peek()
                                     });
            accountingPlanId.Pop();
        }

        /// <summary>
        /// this methpd returns inbound plans
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable GetInboundPlan(int companyId)
        {
            return GetAccountsPlan(INBOUNDCODE, companyId).ToDataTable();
        }

        /// <summary>
        /// this methpd returns outbound plans
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable GetOutboundPlan(int companyId)
        {
            return GetAccountsPlan(OUTBOUNDCODE, companyId).ToDataTable();
        }

        public DataTable GetAccountsPlanAsDataTable(int companyId)
        {
            return GetAccountsPlan(null, companyId).OrderBy(t => t.Name).ToDataTable();
        }


        /// <summary>
        /// this method returns accounts plans by company and code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        private IQueryable<AccountingPlan> GetAccountsPlan(String code, int companyId)
        {
            IQueryable<AccountingPlan> AccountsPlan =
                DbContext.AccountingPlans.Where(accountingPlan => accountingPlan.CompanyId == companyId);

            if (!String.IsNullOrEmpty(code) && (code == OUTBOUNDCODE || code == INBOUNDCODE))
                AccountsPlan = AccountsPlan.Where(accountingPlan => accountingPlan.Code.StartsWith(code));

            return AccountsPlan;
        }

        #endregion

        #region CFOP

        /// <summary>
        /// Returns the CFOP Data
        /// </summary>
        /// <returns></returns>
        public IQueryable<CFOP> GetCFOP()
        {
            return DbContext.CFOPs;
        }

        /// <summary>
        /// This method returns all CFOPs formatted to show in a combo box
        /// </summary>
        /// <returns></returns>
        public IList GetCFOPFormatted()
        {
            return (from cf in GetCFOP()
                    select new
                               {
                                   name = cf.Code + " - " + cf.Name,
                                   cf.CfopId,
                                   cf.Code,
                               }).OrderBy(st => st.name).ToList();
        }

        #endregion

        #region CostCenter

        /// <summary>
        /// Register the Cost Center Template for begin uses
        /// </summary>
        /// <param name="companyId"></param>
        internal void RegisterCostCenterTemplate(int companyId)
        {
            InsertCostCenter(new CostCenter { CompanyId = companyId, Name = "Administrativo" });
            InsertCostCenter(new CostCenter { CompanyId = companyId, Name = "Compras" });
            InsertCostCenter(new CostCenter { CompanyId = companyId, Name = "Engenharia" });
            InsertCostCenter(new CostCenter { CompanyId = companyId, Name = "Financeiro" });
            InsertCostCenter(new CostCenter { CompanyId = companyId, Name = "Logistica" });
            InsertCostCenter(new CostCenter { CompanyId = companyId, Name = "Operacional" });
            InsertCostCenter(new CostCenter { CompanyId = companyId, Name = "Manutenção" });
            InsertCostCenter(new CostCenter { CompanyId = companyId, Name = "Presidência" });
            InsertCostCenter(new CostCenter { CompanyId = companyId, Name = "Vendas" });
        }

        /// <summary>
        /// Returns the Cost Center
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable GetCostsCenterAsDataTable(int companyId)
        {
            IQueryable<CostCenter> query =
                DbContext.CostCenters.Where(x => x.CompanyId == companyId).OrderBy(t => t.Name);
            return query.ToDataTable();
        }

        /// <summary>
        /// this method returns a CostCenter by CostCenterId
        /// </summary>
        /// <param name="costCenterId"></param>
        /// <returns></returns>
        public CostCenter GetCostCenter(Int32 costCenterId)
        {
            return DbContext.CostCenters.Where(costCenter => costCenter.CostCenterId == costCenterId).FirstOrDefault();
        }

        /// <summary>
        /// this method updates costcenter
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        public void UpdateCostCenter(CostCenter original_entity, CostCenter entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }


        /// <summary>
        /// this method deletes a cost center
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteCostCenter(CostCenter entity)
        {
            DbContext.CostCenters.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        #endregion

        #region AccountRegister

        /// <summary>
        /// this method calculates the amounts of bill and invoices
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private Decimal GetAccountAmountByPeriod(Int32 companyId, DateTime? beginDate, DateTime? endDate,
                                                 Int32 accountId)
        {
            var financialManager = new FinancialManager(this);

            return Decimal.Subtract(
                financialManager.GetRegisteredInvoiceAmountByPeriod(companyId, beginDate, endDate, accountId),
                financialManager.GetRegisteredBillAmountByPeriod(companyId, beginDate, endDate, accountId));
        }

        public Decimal GetAccountAmountBeforePeriod(Int32 companyId, DateTime? endDate, Int32 accountId)
        {
            return GetAccountAmountByPeriod(companyId, null, endDate, accountId);
        }

        /// <summary>
        /// This method returns the sum of parcels values from invoices of specified company
        /// </summary>
        /// <param name="companyId">can't be null</param>
        /// <returns></returns>
        public decimal GetParcelsValueFromInvoices(Int32 companyId)
        {
            var result = decimal.Zero;

            var parcels = DbContext.Parcels.Where(parcel => parcel.InvoiceId.HasValue && parcel.CompanyId == companyId);

            if (parcels.Any())
                result = parcels.Sum(x => x.Amount);

            return result;
        }

        /// <summary>
        /// This method returns the sum of parcels values from bills of specified company
        /// </summary>
        /// <param name="companyId">can't be null</param>
        /// <returns></returns>
        public decimal GetParcelsValueFromBills(Int32 companyId)
        {
            var result = decimal.Zero;

            var parcels = DbContext.Parcels.Where(parcel => parcel.BillId.HasValue && parcel.CompanyId == companyId);

            if (parcels.Any())
                result = parcels.Sum(x => x.Amount);

            return result;
        }


        /// <summary>
        /// This method returns the sum of conciliated parcel values from invoices of specified company
        /// </summary>
        /// <param name="companyId">can't be null</param>
        /// <returns></returns>
        public Decimal? GetSumConciliatedParcelsValueFromInvoices(Int32 companyId)
        {
            Decimal? result = decimal.Zero;

            var conciliatedParcels = DbContext.Parcels.Where(parcel => parcel.InvoiceId.HasValue && parcel.OperationDate.HasValue && parcel.CompanyId == companyId);

            if (conciliatedParcels.Any())
                result = conciliatedParcels.Sum(x => x.EffectedAmount);

            return result;
        }

        /// <summary>
        /// This method returns the sum of conciliated parcel values from bills of specified company
        /// </summary>
        /// <param name="companyId"> can't be null</param>
        /// <returns></returns>
        public Decimal? GetSumConciliatedParcelsValueFromBills(Int32 companyId)
        {

            Decimal? result = decimal.Zero;

            var conciliatedParcels = DbContext.Parcels.Where(parcel => parcel.BillId.HasValue && parcel.OperationDate.HasValue && parcel.CompanyId == companyId);

            if (conciliatedParcels.Any())
                result = conciliatedParcels.Sum(x => x.EffectedAmount);

            return result;

        }


        #endregion



        #region FinancierOperation

        /// <summary>
        /// this method inserts a FinancierOperation
        /// </summary>
        /// <param name="entity"></param>
        public void InsertFinancierOperation(FinancierOperation entity)
        {
            DbContext.FinancierOperations.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        ///// <summary>
        ///// This method return a financierOperation by specified paymentMethod
        ///// </summary>
        ///// <param name="paymentMethodId"></param>
        ///// <returns></returns>
        //public FinancierOperation GetFinancierOperation(Int32 companyId, Int32 paymentMethodId)
        //{
        //    return DbContext.FinancierOperations.Where(x => x.CompanyId == companyId && x.PaymentMethodId == paymentMethodId).FirstOrDefault();
        //}

        /// <summary>
        /// this method updates a FinancierOperation
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        public void UpdateFinancierOperation(FinancierOperation original_entity, FinancierOperation entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method deletes a FinancierOperation
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteFinancierOperation(FinancierOperation entity)
        {
            DbContext.FinancierOperations.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method returns a FinancierOperation
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="FinancierOperationId"></param>
        /// <returns></returns>
        public FinancierOperation GetFinancierOperation(Int32 companyId, Int32 financierOperationId)
        {
            return
                DbContext.FinancierOperations.Where(cOperation => cOperation.CompanyId == companyId && cOperation.FinancierOperationId == financierOperationId).
                    FirstOrDefault();
        }

        /// <summary>
        /// This method returns the financierOperation by specified paymentMethod
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        private FinancierOperation GetFinancierOperationByPaymentMethod(Int32 companyId, Int32 paymentMethodId)
        {
            return DbContext.FinancierOperations.Where(x => x.CompanyId == companyId && x.PaymentMethodId == paymentMethodId).FirstOrDefault();
        }


        /// <summary>
        /// this method returns a FinancierOperation
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="FinancierOperationId"></param>
        /// <returns></returns>
        public FinancierOperation GetFinancierOperationBoleto(Int32 companyId)
        {
            return (from op in DbContext.FinancierOperations
                    where op.CompanyId == companyId && op.PaymentMethodId == PaymentMethod.Boleto
                    select op).FirstOrDefault();
        }


        /// <summary>
        /// this method returns FinancierOperations by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetFinancierOperations(Int32 companyId, string sortExpression,
                                                                              int startRowIndex, int maximumRows)
        {

            var query = from financierOperation in GetFinancierOperations(companyId)
                        select new
                        {
                            PaymentMethodName = financierOperation.PaymentMethod.Name,
                            financierOperation.FinancierOperationId,
                            financierOperation.CompanyId
                        };

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "PaymentMethodName");
        }


        /// <summary>
        /// This method returns financierOperations by paymentMethod
        /// </summary>
        /// <param name="companyId"> can not be null</param>
        /// <param name="paymentMethodId">can not be null</param>
        /// <returns></returns>
        public IQueryable<FinancierOperation> GetFinancierOperations(Int32 companyId, Int32 paymentMethodId)
        {
            return DbContext.FinancierOperations.Where(financierOperation => financierOperation.CompanyId == companyId && financierOperation.PaymentMethodId == paymentMethodId);
        }

        public DataTable GetFinancierOperationsAsDataTable(Int32 companyId)
        {
            return GetFinancierOperations(companyId).ToDataTable();
        }

        /// <summary>
        /// This method returns all financierOperation by company
        /// </summary>
        /// <param name="companyId"></param>
        public IQueryable<FinancierOperation> GetFinancierOperations(Int32 companyId)
        {
            return DbContext.FinancierOperations.Where(financierOperation => financierOperation.CompanyId == companyId);
        }

        /// <summary>
        /// This method returns the PaymentMethods's name of FinancierOperations of an specific company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable GetPaymentMethods(Int32 companyId)
        {
            IQueryable query = from operation in GetFinancierOperations(companyId)
                               select new
                                          {
                                              operation.FinancierOperationId,
                                              PaymentMethodName = operation.PaymentMethod.Name
                                          };
            return query;
        }

        /// <summary>
        /// This method get the parcels related to a single payment method
        /// </summary>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        public IList GetParcelsByPaymentMethod(Int32 paymentMethodId)
        {
            var query = from fOperation in DbContext.FinancierOperations
                        join parcel in DbContext.Parcels on fOperation.FinancierOperationId equals
                            parcel.FinancierOperationId
                        where (parcel.PaymentMethodId == paymentMethodId)
                        select new
                                   {
                                       parcel.ParcelId,
                                       parcel.DueDate,
                                       parcel.EffectedAmount,
                                       parcel.Amount,
                                       parcel.Description,
                                       parcel.Invoice,
                                       parcel.Bill,
                                       parcel.AccountId,
                                       parcel.IsRecurrent,
                                       parcel.RecurrentPeriod,
                                       parcel.CompanyId,
                                       parcel.PaymentMethodId,
                                       parcel.IdentificationNumber,
                                       parcel.FinancierOperationId,
                                       parcel.OperationDate
                                   };
            return query.ToList();
        }


        /// <summary>
        /// this is the count method of GetFinancierOperationsByCompany method
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetFinancierOperationsCount(Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetFinancierOperations(companyId, sortExpression, startRowIndex, maximumRows).Cast<Object>().Count();
        }

        #endregion

        #region FinancierCondition

        /// <summary>
        /// this method inserts a FinancierCondition
        /// </summary>
        /// <param name="entity"></param>
        public void InsertFinancierCondition(FinancierCondition entity)
        {
            DbContext.FinancierConditions.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method updates a FinancierCondition
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        public void UpdateFinancierCondition(FinancierCondition original_entity, FinancierCondition entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method deletes a FinancierCondition
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteFinancierCondition(FinancierCondition entity)
        {
            DbContext.FinancierConditions.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method deletes severals financierConditions by financierOperationId
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="financierOperationId"></param>
        public void DeleteFinancierConditions(Int32 companyId, Int32 financierOperationId)
        {
            DbContext.FinancierConditions.DeleteAllOnSubmit(GetFinancierConditionsByFinancierOperation(companyId, financierOperationId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method returns a specific FinancierCondition
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="FinancierConditionId"></param>
        /// <returns></returns>
        public FinancierCondition GetFinancierCondition(Int32 companyId, Int32 FinancierConditionId)
        {
            return
                DbContext.FinancierConditions.Where(
                    cOperationCondition =>
                    cOperationCondition.CompanyId == companyId &&
                    cOperationCondition.FinancierConditionId == FinancierConditionId).FirstOrDefault();
        }

        /// <summary>
        /// this method retuns FinancierCondition by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<FinancierCondition> GetFinancierConditionsByFinancierOperation(Int32 companyId,
                                                                                         Int32 financierOperationId)
        {
            return
                DbContext.FinancierConditions.Where(
                    c => c.CompanyId == companyId && c.FinancierOperationId == financierOperationId);
        }

        /// <summary>
        /// this is the count method of GetFinancierConditionsByCompany method
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Int32 GetFinancierConditionsCountByFinancierOperation(Int32 companyId, Int32 financierOperationId)
        {
            return GetFinancierConditionsByFinancierOperation(companyId, financierOperationId).Count();
        }

        /// <summary>
        /// this method retuns FinancierCondition by company with sort and page
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<FinancierCondition> GetFinancierConditionsByFinancierOperation(Int32 companyId,
                                                                                         Int32 financierOperationId,
                                                                                         string sortExpression,
                                                                                         int startRowIndex,
                                                                                         int maximumRows)
        {
            return GetFinancierConditionsByFinancierOperation(companyId, financierOperationId)
                .SortAndPage(sortExpression, startRowIndex, maximumRows, "ParcelCount");
        }

        /// <summary>
        /// this is the count method of GetFinancierConditionsbyCompany
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetFinancierConditionsCountByFinancierOperation(Int32 companyId, Int32 financierOperationId,
                                                                     string sortExpression, int startRowIndex,
                                                                     int maximumRows)
        {
            return GetFinancierConditionsByFinancierOperation(companyId, financierOperationId, sortExpression,
                                                           startRowIndex, maximumRows).Count();
        }

        public FinancierCondition GetFinancierConditionByParcelCount(Int32 companyId, Int32 financialOperationId,
                                                                     Int32 parcelCount)
        {
            return GetFinancierConditionsByFinancierOperation(companyId, financialOperationId)
                .Where(fOperation => fOperation.ParcelCount == parcelCount)
                .FirstOrDefault();
        }

        public IQueryable GetFormatedFinancierConditionsByFinancierOperation(Int32 companyId, Int32 financierOperationId)
        {
            var query = from financierCondition in GetFinancierConditionsByFinancierOperation(companyId, financierOperationId)
                        select new
                        {
                            ParcelInterval = financierCondition.ParcelCount,
                            financierCondition.FinancierOperationId,
                            financierCondition.FinancierConditionId,
                            financierCondition.MonthlyTax
                        };

            return query;
        }

        #endregion

        #region PaymentMethod

        public IQueryable<PaymentMethod> GetAllPaymentMethod()
        {
            return DbContext.PaymentMethods;
        }

        //public IQueryable<PaymentMethodType> GetAllPaymentMethodTypes()
        //{
        //    return DbContext.PaymentMethodTypes;
        //}

        /// <summary>
        /// This method returns a Payment Method
        /// </summary>
        /// <param name="paymentMethodId">Can't be null</param>
        /// <returns>an PaymentMethod Otherwise a default entity</returns>
        public PaymentMethod GetPaymentMethod(Int32 paymentMethodId)
        {
            return DbContext.PaymentMethods.Where(p => p.PaymentMethodId.Equals(paymentMethodId)).FirstOrDefault();
        }

        #endregion

        #region Expenditure Authorization

        /// <summary>
        /// This method retrieves the expenditureAuthorizations from a specific Company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetExpenditureAuthorizations(Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            var query = from expenditureAutorization in DbContext.ExpenditureAuthorizations
                        where expenditureAutorization.CompanyId == companyId
                        select new
                        {
                            expenditureAutorization.ExpenditureAuthorizationId,
                            expenditureAutorization.CustomerCall.CreatedDate,
                            expenditureAutorization.CustomerCall.CallNumber,
                            expenditureAutorization.Amount,
                            CustomerName = expenditureAutorization.CustomerCall.Customer.ProfileId.HasValue ? expenditureAutorization.CustomerCall.Customer.Profile.Name : expenditureAutorization.CustomerCall.Customer.LegalEntityProfile.CompanyName,
                            expenditureAutorization.IsDenied,
                            expenditureAutorization.CustomerCall,
                            EmployeeName = expenditureAutorization.CustomerCall.TechnicalEmployeeId.HasValue ? expenditureAutorization.CustomerCall.Employee.Profile.Name : string.Empty
                        };

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "Amount");
        }

        /// <summary>
        /// This method just returns the number of registers that GetExpenditureAuthorizations method returns 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetExpenditureAuthorizationsCount(Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetExpenditureAuthorizations(companyId, sortExpression, startRowIndex, maximumRows).Cast<Object>().Count();
        }

        /// <summary>
        /// This method returns a specified expenditureAuthorization
        /// </summary>
        /// <param name="expenditureAuthorizationId"></param>
        /// <returns></returns>
        public ExpenditureAuthorization GetExpenditureAuthorization(Int32 expenditureAuthorizationId)
        {
            return DbContext.ExpenditureAuthorizations.Where(exp => exp.ExpenditureAuthorizationId == expenditureAuthorizationId).FirstOrDefault();
        }

        /// <summary>
        /// This method updates the "IsDenied" information in specified expenditureAuthorization from db
        /// </summary>
        /// <param name="expenditureAuthorization">Connected expenditureAuthorization object to db </param>
        /// <param name="status">true for denied or false for authorized</param>
        public void SetAuthorizationStatusInExpenditure(ExpenditureAuthorization expenditureAuthorization, bool status)
        {
            expenditureAuthorization.IsDenied = status;
            DbContext.SubmitChanges();

        }

        /// <summary>
        /// This method authorizes specified expenditures generating a bill related        
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="expenditureAuthorizationIds"></param>
        public void AuthorizeExpenditures(Int32 companyId, List<Int32> expenditureAuthorizationIds)
        {
            var financialManager = new FinancialManager(this);

            //
            // Creating Parcel
            //

            var parcel = new Parcel();

            decimal parcelAmount = 0;
            parcel.CompanyId = companyId;
            parcel.Description = "1/1";

            var predictableDate = DateTime.Now.AddMonths(1);
            parcel.DueDate = new DateTime(predictableDate.Year, predictableDate.Month, 5);

            //
            // Creating Bill
            //

            var bill = new Bill();

            bill.CompanyId = companyId;
            bill.Description = "Despesas de funcionários";

            var listParcels = new List<Parcel>();
            listParcels.Add(parcel);
            financialManager.Insert(bill, listParcels);

            foreach (var expenditureAuthorizationId in expenditureAuthorizationIds)
            {
                var expenditureAuthorization = GetExpenditureAuthorization(expenditureAuthorizationId);
                expenditureAuthorization.BillId = bill.BillId;

                SetAuthorizationStatusInExpenditure(expenditureAuthorization, false);
                parcelAmount += expenditureAuthorization.Amount;
            }

            parcel.Amount = parcelAmount;

            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method non authorizes the especified expenditures
        /// </summary>
        /// <param name="expenditureAuthorizationIds"></param>
        public void NonAuthorizeExpenditures(List<Int32> expenditureAuthorizationIds)
        {
            var financialManager = new FinancialManager(this);
            var billId = 0;

            foreach (var expenditureAuthorizationId in expenditureAuthorizationIds)
            {
                var expenditureAuthorization = GetExpenditureAuthorization(expenditureAuthorizationId);

                if (expenditureAuthorization.BillId.HasValue)
                {
                    billId = expenditureAuthorization.BillId.Value;
                    expenditureAuthorization.BillId = null;

                    financialManager.DeleteBill(billId, expenditureAuthorization.CompanyId);
                }
                SetAuthorizationStatusInExpenditure(expenditureAuthorization, true);
            }
        }

        /// <summary>
        /// This method inserts a new expenditureAuthorization in db
        /// </summary>
        /// <param name="ExpenditureAuthorization"></param>
        private void InsertExpenditureAuthorization(ExpenditureAuthorization expenditureAuthorization)
        {
            DbContext.ExpenditureAuthorizations.InsertOnSubmit(expenditureAuthorization);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method deletes an expenditureAuthorization from db
        /// </summary>
        /// <param name="expenditureAuthorizationId"></param>
        public void DeleteExpenditureAuthorization(Int32 expenditureAuthorizationId)
        {
            DbContext.ExpenditureAuthorizations.DeleteOnSubmit(GetExpenditureAuthorization(expenditureAuthorizationId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method saves the expenditureAuthorization, for insert or update 
        /// </summary>
        /// <param name="expenditureAuthorization"></param>
        public void SaveExpenditureAuthorization(ExpenditureAuthorization expenditureAuthorization)
        {
            //
            //Insert
            //

            if (expenditureAuthorization.ExpenditureAuthorizationId == 0)
            {
                InsertExpenditureAuthorization(expenditureAuthorization);
                return;
            }

            //
            // Update
            //

            var originalExpenditureAuthorization = GetExpenditureAuthorization(expenditureAuthorization.ExpenditureAuthorizationId);
            UpdateExpenditureAuthorization(originalExpenditureAuthorization, expenditureAuthorization);
        }

        /// <summary>
        /// This method updates an expenditureAuthorization from db
        /// </summary>
        /// <param name="originalExpenditureAuthorization"></param>
        /// <param name="expenditureAuthorization"></param>
        private void UpdateExpenditureAuthorization(ExpenditureAuthorization originalExpenditureAuthorization, ExpenditureAuthorization expenditureAuthorization)
        {
            originalExpenditureAuthorization.CopyPropertiesFrom(expenditureAuthorization);
            DbContext.SubmitChanges();
        }

        #endregion


        private const string INBOUNDCODE = "1";
        private const string OUTBOUNDCODE = "2";



        public String GenerateAutomaticDebitFile(Company company, Account account, List<Parcel> parcels,
                                                 Int32 sequentialNumber)
        {
            var strBuilderAutomaticDebitFile = new StringBuilder();
            Int32 totalRows = 0;
            totalRows = GenerateAutomaticDebitFileHeader(strBuilderAutomaticDebitFile, company, account,
                                                         sequentialNumber);
            totalRows += GenerateAutomaticDebitFileContent(strBuilderAutomaticDebitFile, company, account, parcels);
            GenerateAutomaticDebitFileTrailler(strBuilderAutomaticDebitFile, parcels, totalRows);

            return strBuilderAutomaticDebitFile.ToString(0, strBuilderAutomaticDebitFile.Length);
        }

        private Int32 GenerateAutomaticDebitFileHeader(StringBuilder strBuilderHeader, Company company, Account account,
                                                       Int32 sequentialNumber)
        {
            /* Função responsavel pela configuração do documento de débito automatico.Modelo definido pela FEBRABAN
            * 
            * O modelo/Código é dividido por blocos definidos por letras
            * 
            * Os campos alfanuméricos são representados por letras A e os numéricos por 0
            * Sendo que os campos default estão preenchidos com seus valores corretos
            * 
            * 
            * O modelo de comentário é: Tamanho do campo - descrição do campo
            *
            */

            /// A - Header

            Int32 totalRows = 0;

            String A_registerCode = "A"; //1 - Default
            String A_deliveryCode = "1"; //1 - Default
            String A_conveneCode = "00000000000000000000"; //20 -Definido pelo banco
            String A_companyName = company.LegalEntityProfile.CompanyName.PadRight(20, ' ').Substring(0, 20); //20-
            String A_bankCode = account.Bank.BankNumber.PadLeft(3, '0').Substring(0, 3);
            //3-Código do banco na câmara de compensação
            String A_BankName = account.Bank.ShortName.PadRight(20, ' ');
            String A_createdDate = DateTime.Now.ToShortDateString(); //8
            String A_SequentialNumber = sequentialNumber.ToString().PadLeft(6, '0'); //6-
            String A_layoutVersion = "04"; //2
            String A_serviceIdentification = "DÉBITO AUTOMÁTICO"; //17
            String A_reservedSpace = String.Empty.PadLeft(52, ' '); //52

            strBuilderHeader.Append(A_registerCode).Append(A_deliveryCode).Append(A_conveneCode).Append(A_companyName);
            strBuilderHeader.Append(A_BankName).Append(A_createdDate).Append(A_SequentialNumber).Append(A_layoutVersion)
                .Append(A_serviceIdentification).Append(A_reservedSpace);
            strBuilderHeader.AppendLine();
            totalRows++;

            return totalRows;
        }

        private Int32 GenerateAutomaticDebitFileContent(StringBuilder strBuilderContent, Company company,
                                                        Account account, List<Parcel> parcels)
        {
            /* Função responsavel pela configuração do documento de débito automatico.Modelo definido pela FEBRABAN
             * 
             * O modelo/Código é dividido por blocos definidos por letras
             * 
             * Os campos alfanuméricos são representados por letras A e os numéricos por 0
             * Sendo que os campos default estão preenchidos com seus valores corretos
             * 
             * 
             * O modelo de comentário é: Tamanho do campo - descrição do campo
             *
             */


            Int32 totalRows = 0;
            foreach (Parcel parcel in parcels)
            {
                if (!parcel.InvoiceId.HasValue || !parcel.Invoice.CustomerId.HasValue)
                    continue;
                /// B
                {
                    String B_registerCode = "B"; //1

                    String B_customerIdentificationAtCompany = parcel.Invoice.Customer.LegalEntityProfile != null
                                                                   ? parcel.Invoice.Customer.LegalEntityProfile.
                                                                         CompanyName
                                                                   : parcel.Invoice.Customer.Profile.Name;
                    //25 - Identificação única do cliente
                    B_customerIdentificationAtCompany.PadRight(20, ' ').Substring(0, 20);

                    if (parcel.Invoice.Customer.Agency == null) parcel.Invoice.Customer.Agency = String.Empty;
                    String B_debitAgency = parcel.Invoice.Customer.Agency.PadRight(4, ' ').Substring(0, 4); //4

                    if (parcel.Invoice.Customer.AccountNumber == null)
                        parcel.Invoice.Customer.AccountNumber = String.Empty;
                    String B_customerIdentificationAtBank =
                        parcel.Invoice.Customer.AccountNumber.PadRight(14, ' ').Substring(0, 14);
                    //14 -Identificação única utilizada pelo banco

                    String B_optionDate = DateTime.Now.ToShortDateString(); //8
                    String B_reservedSpace = String.Empty.PadLeft(97, ' '); //97
                    String B_operationCode = "1"; //1

                    strBuilderContent.Append(B_registerCode).Append(B_customerIdentificationAtCompany).Append(
                        B_debitAgency);
                    strBuilderContent.Append(B_customerIdentificationAtCompany).Append(B_optionDate).Append(
                        B_reservedSpace).Append(B_operationCode);

                    strBuilderContent.AppendLine();
                    totalRows++;
                }

                /// C
                {
                    String C_registerCode = "C"; //1
                    String C_customerIdentificationAtCompany =
                        parcel.Invoice.Customer.CustomerId.ToString().PadRight(25, ' ').Substring(0, 25); //25
                    String C_debitAgency = parcel.Invoice.Customer.Agency.PadRight(4, ' ').Substring(0, 4); //4
                    String C_customerIdentificationAtBank =
                        parcel.Invoice.Customer.AccountNumber.PadRight(14, ' ').Substring(0, 14);
                    //14 -Identificação única utilizada pelo banco
                    String C_firstOcurrence = String.Empty.PadLeft(40, ' '); //40
                    String C_secondOcurrence = String.Empty.PadLeft(40, ' '); //40
                    String C_reservedSpace = String.Empty.PadLeft(40, ' '); //40
                    String C_operationCode = "2"; //1

                    strBuilderContent.Append(C_registerCode).Append(C_customerIdentificationAtBank).Append(C_debitAgency)
                        .Append(C_customerIdentificationAtBank);
                    strBuilderContent.Append(C_firstOcurrence).Append(C_secondOcurrence).Append(C_reservedSpace).Append(
                        C_operationCode);
                    strBuilderContent.AppendLine();
                    totalRows++;
                }

                ///D
                {
                    String D_registerCode = "C"; //1
                    String D_customerIdentificationAtOriginalCompany = String.Empty.PadLeft(25, ' ');
                    //25 - Identificação do cliente na empresa antiga
                    String D_debitAgency = String.Empty.PadLeft(4, ' '); //4
                    String D_customerIdentificationAtBank = String.Empty.PadLeft(14, ' ');
                    //14 -Identificação única utilizada pelo banco
                    String D_customerIdentificationAtNewCompany = String.Empty.PadLeft(25, ' ');
                    //25 - Identificação do cliente na empresa nova
                    String D_Ocurrence = String.Empty.PadLeft(60, ' '); //60
                    String D_reservedSpace = String.Empty.PadLeft(20, ' '); //20
                    String D_operationCode = "0"; //1

                    strBuilderContent.Append(D_registerCode).Append(D_customerIdentificationAtOriginalCompany).Append(
                        D_debitAgency);
                    strBuilderContent.Append(D_customerIdentificationAtBank).Append(D_customerIdentificationAtNewCompany)
                        .Append(D_Ocurrence);
                    strBuilderContent.Append(D_reservedSpace).Append(D_operationCode);

                    strBuilderContent.AppendLine();
                    totalRows++;
                }

                ///E
                {
                    String E_registerCode = "E"; //1
                    String E_customerIdentificationAtCompany =
                        parcel.Invoice.Customer.CustomerId.ToString().PadRight(25, ' ').Substring(0, 25); //25
                    String E_debitAgency = String.Empty.PadLeft(4, ' '); //4
                    String E_customerIdentificationAtBank =
                        parcel.Invoice.Customer.AccountNumber.PadRight(14, ' ').Substring(0, 14);
                    //14 -Identificação única utilizada pelo banco
                    String E_dueDate = parcel.DueDate.ToShortDateString(); //8
                    String E_debitValue = "000000000000000".PadLeft(15, '0'); //15
                    String E_currencyCode = "03"; //2
                    String E_spaceForCompanyUse = String.Empty.PadLeft(60, ' '); //60
                    String E_reservedSpace = String.Empty.PadLeft(20, ' '); //20
                    String E_operationCode = "0"; //1

                    strBuilderContent.Append(E_registerCode).Append(E_customerIdentificationAtCompany).Append(
                        E_debitAgency);
                    strBuilderContent.Append(E_customerIdentificationAtCompany).Append(E_dueDate).Append(E_debitValue).
                        Append(E_currencyCode);
                    strBuilderContent.Append(E_spaceForCompanyUse).Append(E_reservedSpace).Append(E_operationCode);


                    strBuilderContent.AppendLine();
                    totalRows++;
                }
            }


            return totalRows;
        }

        private void GenerateAutomaticDebitFileTrailler(StringBuilder strBuilderTrailler, List<Parcel> parcels,
                                                        Int32 totalRows)
        {
            /* Função responsavel pela configuração do documento de débito automatico.Modelo definido pela FEBRABAN
             * 
             * O modelo/Código é dividido por blocos definidos por letras
             * 
             * Os campos alfanuméricos são representados por letras A e os numéricos por 0
             * Sendo que os campos default estão preenchidos com seus valores corretos
             * 
             * 
             * O modelo de comentário é: Tamanho do campo - descrição do campo
             *
             */

            Decimal parcelsSum = Decimal.Zero;

            foreach (Parcel parcel in parcels)
            {
                if (parcel.Invoice == null || parcel.Invoice.Customer == null)
                    continue;

                parcelsSum += parcel.Amount;
            }
            totalRows++;

            ///Z

            String Z_registerCode = "Z"; //1
            String Z_quantityOfRegisters = totalRows.ToString().PadLeft(6, '0').Substring(0, 6);
            String Z_amountOfRegisters = parcelsSum.ToString().PadLeft(17, '0').Substring(0, 17); //17
            String Z_reservedSpace = String.Empty.PadLeft(126, ' '); //126

            strBuilderTrailler.Append(Z_registerCode).Append(Z_quantityOfRegisters).Append(Z_amountOfRegisters).Append(
                Z_reservedSpace);
        }

        private void ParseAutomaticDebitFileRegister(String debitFile)
        {
            const Int32 length = 150;

            String registerA = debitFile.Substring(0, length); //
            String registerB = debitFile.Substring(150, length); //
            String registerC = debitFile.Substring(300, length);
            String registerD = debitFile.Substring(450, length);
            String registerE = debitFile.Substring(600, length);
            String registerF = debitFile.Substring(750, length); //
            String registerH = debitFile.Substring(900, length);
            String registerI = debitFile.Substring(1050, length);
            String registerJ = debitFile.Substring(1200, length); //
            String registerL = debitFile.Substring(1350, length);
            String registerT = debitFile.Substring(1500, length);
            String registerX = debitFile.Substring(1650, length);
            String registerZ = debitFile.Substring(1800, length);

            ParseAutomaticDebitFileRegisterA(registerA);
            ParseAutomaticDebitFileRegisterB(registerB);
            ParseAutomaticDebitFileRegisterF(registerF);
            ParseAutomaticDebitFileRegisterJ(registerJ);
            ParseAutomaticDebitFileRegisterZ(registerJ);
        }

        private void ParseAutomaticDebitFileRegisterA(string register)
        {
            String sequentialNumber = register.Substring(74, 6);
        }

        private void ParseAutomaticDebitFileRegisterB(string register)
        {
            String customerIdentificationAtCompany = register.Substring(2, 25);
        }

        private void ParseAutomaticDebitFileRegisterF(string register)
        {
            String customerIdentificationAtCompany = register.Substring(2, 25);
            String customerIdentificationAtBank = register.Substring(31, 14);
            String returnCode = register.Substring(68, 2);
        }

        private void ParseAutomaticDebitFileRegisterJ(string register)
        {
            String executionSequentialNumber = register.Substring(2, 7);
            String executionDate = register.Substring(34, 8);
        }

        private void ParseAutomaticDebitFileRegisterZ(string register)
        {
            String quantityOfRegisters = register.Substring(2, 9);
            String totalAmount = register.Substring(8, 17);
        }


#warning refatorar esta query
        /// <summary>
        /// This method retrieves the sum of amount values of invoices and bills by month of a specific company 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable GetMonthlyCashMoney(Int32 companyId)
        {

            var query = from parcel in DbContext.Parcels
                        where parcel.CompanyId == companyId
                        group parcel by parcel.Company into gParcels
                        select new
                        {
                            #region Sum invoices values

                            firstMonthSumInvoice = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.Month && parcel.DueDate.Year == DateTime.Now.Year && parcel.InvoiceId != null).Sum(parcel => parcel.Amount) ?? 0,

                            secondMonthSumInvoice = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(1).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(1).Year && parcel.InvoiceId != null).Sum(parcel => parcel.Amount) ?? 0,

                            thirdMonthSumInvoice = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(2).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(2).Year && parcel.InvoiceId != null).Sum(parcel => parcel.Amount) ?? 0,

                            fourthMonthSumInvoice = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(3).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(3).Year && parcel.InvoiceId != null).Sum(parcel => parcel.Amount) ?? 0,

                            fifthMonthSumInvoice = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(4).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(4).Year && parcel.InvoiceId != null).Sum(parcel => parcel.Amount) ?? 0,

                            sixthMonthSumInvoice = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(5).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(5).Year && parcel.InvoiceId != null).Sum(parcel => parcel.Amount) ?? 0,

                            seventhMonthSumInvoice = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(6).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(6).Year && parcel.InvoiceId != null).Sum(parcel => parcel.Amount) ?? 0,

                            eighthMonthSumInvoice = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(7).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(7).Year && parcel.InvoiceId != null).Sum(parcel => parcel.Amount) ?? 0,

                            ninthMonthSumInvoice = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(8).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(8).Year && parcel.InvoiceId != null).Sum(parcel => parcel.Amount) ?? 0,

                            tenthMonthSumInvoice = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(9).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(9).Year && parcel.InvoiceId != null).Sum(parcel => parcel.Amount) ?? 0,

                            eleventhMonthSumInvoice = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(10).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(10).Year && parcel.InvoiceId != null).Sum(parcel => parcel.Amount) ?? 0,

                            twelfthMonthSumInvoice = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(11).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(11).Year && parcel.InvoiceId != null).Sum(parcel => parcel.Amount) ?? 0,
                            #endregion

                            #region Sum bills values

                            firstMonthSumBill = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.Month && parcel.DueDate.Year == DateTime.Now.Year && parcel.BillId != null).Sum(parcel => parcel.Amount) ?? 0,

                            secondMonthSumBill = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(1).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(1).Year && parcel.BillId != null).Sum(parcel => parcel.Amount) ?? 0,

                            thirdMonthSumBill = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(2).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(2).Year && parcel.BillId != null).Sum(parcel => parcel.Amount) ?? 0,

                            fourthMonthSumBill = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(3).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(3).Year && parcel.BillId != null).Sum(parcel => parcel.Amount) ?? 0,

                            fifthMonthSumBill = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(4).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(4).Year && parcel.BillId != null).Sum(parcel => parcel.Amount) ?? 0,

                            sixthMonthSumBill = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(5).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(5).Year && parcel.BillId != null).Sum(parcel => parcel.Amount) ?? 0,

                            seventhMonthSumBill = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(6).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(6).Year && parcel.BillId != null).Sum(parcel => parcel.Amount) ?? 0,

                            eighthMonthSumBill = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(7).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(7).Year && parcel.BillId != null).Sum(parcel => parcel.Amount) ?? 0,

                            ninthMonthSumBill = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(8).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(8).Year && parcel.BillId != null).Sum(parcel => parcel.Amount) ?? 0,

                            tenthMonthSumBill = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(9).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(9).Year && parcel.BillId != null).Sum(parcel => parcel.Amount) ?? 0,

                            eleventhMonthSumBill = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(10).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(10).Year && parcel.BillId != null).Sum(parcel => parcel.Amount) ?? 0,

                            twelfthMonthSumBill = (decimal?)gParcels.Key.Parcels.Where(parcel => parcel.DueDate.Month == DateTime.Now.AddMonths(11).Month && parcel.DueDate.Year == DateTime.Now.AddMonths(11).Year && parcel.BillId != null).Sum(parcel => parcel.Amount) ?? 0




                            #endregion
                        };
            return query;
        }


        public object GetCashFlowByYear(int companyId) { 
            return GetCashFlowByYear(companyId, null, null); 
        }

        public object GetCashFlowByYear(int companyId, int? accPlanParentId) { 
            return GetCashFlowByYear(companyId, accPlanParentId);
        }

        public object GetCashFlowByYear(int companyId, int? accPlanParentId, int? year)
        {
            year = year ?? DateTime.Now.Year;

            DataManager.Parameters.Add("@companyId", companyId);
            DataManager.Parameters.Add("@year", year);

            if (accPlanParentId.HasValue)
                DataManager.Parameters.Add("@accPlanParentId", accPlanParentId.Value);
            else
                DataManager.Parameters.Add("@accPlanParentId", DBNull.Value);
            

            return DataManager.ExecuteHashtable(@"

    WITH AccountingPlanTree (AccountingPlanId, ParentId, DueDate, Amount) AS 
	(
		SELECT ap.AccountingPlanId, ap.ParentId,  MONTH(ISNULL(p.EffectedDate, p.DueDate)) as DueDate, CAST(SUM(p.Amount* -1) as decimal(18,2) )
		FROM (select * from Parcel where Year(DueDate)=@year)  p INNER JOIN 
    		 Bill b ON p.BillId = b.BillId INNER JOIN		
    		 AccountingPlan ap ON ap.AccountingPlanId = b.AccountingPlanId 
		WHERE  p.CompanyId=@companyId
		GROUP BY ap.AccountingPlanId, ap.ParentId,  MONTH(ISNULL(p.EffectedDate, p.DueDate))
	    
	UNION ALL
	    
		SELECT ap.AccountingPlanId, ap.ParentId,  MONTH(ISNULL(p.EffectedDate, p.DueDate)) as DueDate, CAST(SUM(p.Amount) as decimal(18,2))
		FROM (select * from Parcel where Year(DueDate)=@year) p INNER JOIN 
    		 Invoice i ON p.InvoiceId = i.InvoiceId INNER JOIN		
    		 AccountingPlan ap ON ap.AccountingPlanId = i.AccountingPlanId 
		WHERE p.CompanyId=@companyId
		GROUP BY ap.AccountingPlanId, ap.ParentId,  MONTH(ISNULL(p.EffectedDate, p.DueDate))

	UNION ALL

		SELECT 
    		 ap.AccountingPlanId, 
    		 ap.ParentId,  
    		 tree.DueDate,     		
    		 CAST( tree.Amount as decimal(18,2))    	  
		FROM AccountingPlan ap INNER JOIN	
    		 AccountingPlanTree tree ON ap.AccountingPlanId = tree.ParentId 


	)


	SELECT 
        ap.AccountingPlanId,
		ap.Code + ' - ' + ap.Name as Name, 	
		ISNULL([1], 0) [January], 
        ISNULL([2], 0) [February], 
        ISNULL([3], 0) [March], 
        ISNULL([4], 0) [April], 
        ISNULL([5], 0) [May], 
        ISNULL([6], 0) [June], 
		ISNULL([7], 0) [July], 
        ISNULL([8], 0) [August], 
        ISNULL([9], 0) [September], 
        ISNULL([10], 0) [October], 
        ISNULL([11], 0) [November], 
        ISNULL([12], 0) [December]  
	FROM (
		SELECT AccountingPlanId, ParentId, DueDate, SUM(AMOUNT) amount
		FROM AccountingPlanTree
		GROUP by AccountingPlanId, ParentId, DueDate
	) AS tbl PIVOT (
		SUM(AMOUNT) 
		FOR [DueDate] IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
	) AS pivotTable INNER JOIN 
		 AccountingPlan ap ON pivotTable.AccountingPlanId = ap.AccountingPlanId 
	WHERE (CASE WHEN @accPlanParentId is NULL and ap.ParentId is null THEN 1
			    WHEN @accPlanParentId is not NULL and ap.ParentId = @accPlanParentId THEN 1
			    ELSE 0 END) = 1 

");
        }

    }
}