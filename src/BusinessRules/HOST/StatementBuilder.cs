using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vivina.Erp.DataClasses;
using InfoControl.Web.Configuration;

namespace Vivina.Erp.BusinessRules.Statements
{
    public class StatementBuilder : InfoControl.Web.ScheduledTasks.IScheduledTaskWorker
    {
        private Company _company;
        public Company Company
        {
            get
            {
                var companymanager = new CompanyManager(this);

                return _company ?? (_company = companymanager.GetCompany(1));
            }
        }

        public void DoWork()
        {
            using (var companyManager = new CompanyManager(null))
            using (var financialManager = new FinancialManager(null))
            {
                // Para cada empresa verifica se tem parcela em atraso
                foreach (var company in companyManager.GetAllCompanies())
                {

                    // Se tiver parcela em atraso bloqueia a empresa

                    // Se náo tiver verifica se é o dia da próxima fatura
                    if (company.NextStatementDueDate.Date.Equals(DateTime.Now.Date))
                        companyManager.GenerateStatement(company);                  
                }
            }
        }

        private StatementItem CheckActveUsers(Package package)
        {
            var item = new StatementItem { 
                Name = "Usuários",
                Quantity = 1,
                Value = package.Price
            };
            return item;
        }

        #region InfoControl.Web.ScheduledTasks.IScheduledTaskWorker
        private InfoControl.Data.DataManager _dataManager;
        public InfoControl.Data.DataManager DataManager
        {
            get { return _dataManager ?? (_dataManager = new InfoControl.Data.DataManager(false)); }
        }

        public void Dispose()
        {
            DataManager.Dispose();
        }

        public InfoControl.Data.DataManager SourceDataManager
        {
            get { throw new NotImplementedException(); }
        }
        #endregion
    }
}
