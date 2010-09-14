using System;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class DropPayoutManager : BusinessManager<InfoControlDataContext>
    {
        public DropPayoutManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// Método utilizado para inserir uma entrada na Tabela
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(DropPayout entity)
        {
            DbContext.DropPayouts.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Pega o conjunto de Sangrias da Tabela DropPayout referente a uma data de início
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="depositId"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public IQueryable<DropPayout> GetSangriaByDate(int companyId, int depositId, DateTimeInterval dateInterval)
        {
            //if (startDate.Date != null && endDate.Date != null)
            //{
            return DbContext.DropPayouts.Where(x => x.CompanyId == companyId && x.DepositId == depositId
                                                    && x.ModifiedDate.Value.Date >= dateInterval.BeginDate &&
                                                    x.ModifiedDate.Value.Date <= dateInterval.EndDate &&
                                                    x.Amount < Decimal.Zero);
            //}
        }

        /// <summary>
        /// Pega o conjunto de 
        /// Suplementos da Tabela DropPayout referente a uma data de início
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="depositId"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public IQueryable<DropPayout> GetSuplementoByDate(int companyId, int depositId, DateTimeInterval dateInterval)
        {
            return DbContext.DropPayouts.Where(x => x.CompanyId == companyId && x.DepositId == depositId
                                                    && x.ModifiedDate >= dateInterval.BeginDate &&
                                                    x.ModifiedDate <= dateInterval.EndDate && x.Amount > 0);
        }
    }
}