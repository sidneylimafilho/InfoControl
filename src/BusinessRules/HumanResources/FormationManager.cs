using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using Vivina.Framework.Configuration;
using Vivina.Framework.Data;
using Vivina.InfoControl.DataClasses;

namespace Vivina.InfoControl.BusinessRules.HumanResources
{
    public partial class FormationManager : Vivina.Framework.Data.BusinessManager
    {
        public FormationManager(IDataAccessor container) : base(container) { }

        #region History
        /// <summary>
        /// Returns a DataSet with all information about the history, the formation name.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public DataTable GetFomationHistories(int companyId, int employeeId)
        {
            InfoControlDataContext db = DataManager.CreateContext<InfoControlDataContext>();
            var query = from his in db.FormationHistories
                        join form in db.Formations on his.FormationId equals form.FormationId                        
                        where his.EmployeeId == employeeId && his.CompanyId == companyId
                        select new { his, formName = form.Name };
            return query.ToDataTable();
        }
        /// <summary>
        /// Return a single row from the table FormationHistory
        /// </summary>
        /// <param name="serviceHistoryId"></param>
        /// <returns></returns>
        public FormationHistory GetFormationHistory(int formationHistoryId)
        {
            InfoControlDataContext db = DataManager.CreateContext<InfoControlDataContext>();
            return db.FormationHistories.Where(x => x.FormationHistoryId == formationHistoryId).FirstOrDefault();
        }
        /// <summary>
        /// Basic Insert Method
        /// </summary>
        /// <param name="entity"></param>
        public void InsertFormationHistory(FormationHistory entity)
        {
            InfoControlDataContext db = DataManager.CreateContext<InfoControlDataContext>();
            db.FormationHistories.Add(entity);
            db.SubmitChanges();
        }
        /// <summary>
        /// Basic Update Method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="original_entity"></param>
        public void UpdateFormationHistory(FormationHistory entity, FormationHistory original_entity)
        {
            InfoControlDataContext db = DataManager.CreateContext<InfoControlDataContext>();
            db.FormationHistories.Attach(original_entity);
            original_entity.CopyPropertiesFrom(entity);
            db.SubmitChanges();
        }
        /// <summary>
        /// Basic Delete Method
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteFormationHistory(FormationHistory entity)
        {
            InfoControlDataContext db = DataManager.CreateContext<InfoControlDataContext>();
            db.FormationHistories.Attach(entity);
            db.FormationHistories.Remove(entity);
            db.SubmitChanges();
        }
        #endregion
    }
}
