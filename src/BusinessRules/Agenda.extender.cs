using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vivina.InfoControl.DataClasses;
using Vivina.InfoControl.BusinessRules;
using Vivina.Framework.Configuration;
using Vivina.Framework.Data;

namespace Vivina.InfoControl.BusinessRules
{
    public class Agenda : Vivina.Framework.Data.BusinessManager
    {
        public Agenda(IDataAccessor container) : base(container) { }
        /*
        public IQueryable<Agenda> GetAllAgendasByInterval(int companyId, int employeeId, int interval)
        {
            InfoControlDataContext db = DataManager.CreateContext<InfoControlDataContext>();
            return db.Agendas.Where(x => x.CompanyId == companyId && x.EmployeeId == employeeId); 
        }
         */

    }
}
