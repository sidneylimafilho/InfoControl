using System;
using System.Collections;
using System.Linq;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class PermissionTypesManager : BusinessManager<InfoControlDataContext>
    {
        public PermissionTypesManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all PermissionTypes.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<PermissionType> GetAllPermissionTypes()
        {
            return DbContext.PermissionTypes;
        }

        /// <summary>
        /// This method gets record counts of all PermissionTypes.
        /// Do not change this method.
        /// </summary>
        public int GetAllPermissionTypesCount()
        {
            return GetAllPermissionTypes().Count();
        }

        /// <summary>
        /// This method retrieves a single PermissionTypes.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=PermissionTypeId>PermissionTypeId</param>
        public PermissionType GetPermissionTypes(Int32 PermissionTypeId)
        {
            return DbContext.PermissionTypes.Where(x => x.PermissionTypeId == PermissionTypeId).FirstOrDefault();
        }

        /// <summary>
        /// This method pages and sorts over all PermissionTypes.
        /// Do not change this method.
        /// </summary>
        /// <param name=sortExpression>sortExpression</param>
        /// <param name=startRowIndex>startRowIndex</param>
        /// <param name=maximumRows>maximumRows</param>
        public IList GetAllPermissionTypes(string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetAllPermissionTypes().SortAndPage(sortExpression, startRowIndex, maximumRows, "PermissionTypeId").
                    ToList();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(PermissionType entity)
        {
            DbContext.PermissionTypes.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(PermissionType entity)
        {
            DbContext.PermissionTypes.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(PermissionType original_entity, PermissionType entity)
        {
            DbContext.PermissionTypes.Attach(original_entity);
            original_entity.Name = entity.Name;
            original_entity.Description = entity.Description;
            DbContext.SubmitChanges();
        }
    }
}