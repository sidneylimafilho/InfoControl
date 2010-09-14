using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class SupplierManager : BusinessManager<InfoControlDataContext>
    {
        public SupplierManager(IDataAccessor container)
            : base(container)
        {
        }


        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Supplier entity)
        {
            DbContext.Suppliers.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(Supplier entity)
        {
            entity.CreatedDate = entity.ModifiedDate = System.DateTime.Now;
            DbContext.Suppliers.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }
    }
}