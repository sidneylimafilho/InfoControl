using System.Linq;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class BarCodeTypeManager : BusinessManager<InfoControlDataContext>
    {
        public BarCodeTypeManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// Return all BarCodeTypes
        /// </summary>
        /// <returns></returns>
        public IQueryable<BarCodeType> GetAllBarCodeTypes()
        {
            return DbContext.BarCodeTypes;
        }
    }
}