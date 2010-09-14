using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class DepositManager : BusinessManager<InfoControlDataContext>
    {
        public DepositManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Deposits.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Deposit> GetAllDeposits()
        {
            return DbContext.Deposits;
        }

        /// <summary>
        /// Returns all Deposits of a company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<Deposit> GetDepositByCompany(int companyId)
        {
            return DbContext.Deposits.Where(x => x.CompanyId == companyId);
        }

        /// <summary>
        /// Returns deposits by company
        /// </summary>
        /// <param name="companyId">Can't be null</param>
        /// <returns></returns>
        public DataTable GetDepositByCompanyAsDataTable(int companyId)
        {
            return GetDepositByCompany(companyId).ToDataTable();
        }


        /// <summary>
        /// Returns all deposit of a company, as list
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public List<Deposit> GetDepositsByCompanyAsList(Int32 companyId, String sortExpression, Int32 startRowIndex,
                                                        Int32 maximumRows)
        {
            IQueryable<Deposit> x = GetDepositByCompany(companyId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "DepositId").ToList();
        }

        public Int32 GetDepositsByCompanyAsListCount(Int32 companyId, String sortExpression, Int32 startRowIndex,
                                                     Int32 maximumRows)
        {
            return GetDepositsByCompanyAsList(companyId, sortExpression, startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// returns a single deposit
        /// </summary>
        /// <param name="depositId"></param>
        /// <returns></returns>
        public Deposit GetDeposit(int depositId)
        {
            return DbContext.Deposits.Where(x => x.DepositId == depositId).FirstOrDefault();
        }

        /// <summary>
        /// This is a basic insert method
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Deposit entity)
        {
            DbContext.Deposits.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this is a basic delete method
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Deposit entity)
        {
            //DbContext.Deposits.Attach(entity); 
            DbContext.Deposits.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this is a basic update method
        /// </summary>
        /// <param name="originalEntity"></param>
        /// <param name="entity"></param>
        public void Update(Deposit originalEntity, Deposit entity)
        {
            originalEntity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Returns all deposits that the user are not administrating
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataReader GetRemainingDeposit(int companyId, int userId)
        {
            DataManager.Parameters.Add("@companyId", companyId);
            DataManager.Parameters.Add("@userId", userId);
            return
                DataManager.ExecuteReader(
                    @"SELECT DepositId, Name
                                               FROM Deposits
                                               WHERE (CompanyId = @companyId) AND (NOT EXISTS
                                                     (SELECT Dep.DepositId
                                                      FROM Deposits AS Dep INNER JOIN
                                                      CompanyUsers AS CP ON Dep.DepositId = CP.DepositId
                                                      WHERE (Dep.DepositId = Deposits.DepositId) 
                                                      AND (CP.CompanyId = @companyId) 
                                                      AND (CP.UserId = @userId)))");
        }
    }
}