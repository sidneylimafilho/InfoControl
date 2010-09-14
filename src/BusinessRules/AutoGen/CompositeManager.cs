using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using Vivina.Framework;
using Vivina.Framework.Configuration;
using Vivina.Framework.Data;
using Vivina.InfoControl.DataClasses;

namespace Vivina.InfoControl.BusinessRules
{


    public partial class CompositeManager : Vivina.Framework.Data.BusinessManager<InfoControlDataContext>
    {
        public CompositeManager(IDataAccessor container) : base(container) { }
        /// <summary>
        /// This method retrieves all CompositeProducts.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<CompositeProduct> GetAllComposites()
        {

            return DbContext.CompositeProducts;
        }

        /// <summary>
        /// This method gets record counts of all CompositeProducts.
        /// Do not change this method.
        /// </summary>
        public int GetAllCompositesCount()
        {
            return GetAllComposites().Count();
        }

        /// <summary>
        /// This method retrieves a single CompositeProduct.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=CompositeId>CompositeId</param>
        public CompositeProduct GetComposite(Int32 CompositeId)
        {

            return DbContext.CompositeProducts.Where(x => x.CompositeId == CompositeId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves CompositeProduct by Product.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=ProductId>ProductId</param>
        /// <param name=CompanyId>CompanyId</param>
        public CompositeProduct GetCompositeByProduct(Int32 ProductId, Int32 CompanyId)
        {

            return DbContext.CompositeProducts.Where(x => x.ProductId == ProductId).FirstOrDefault();
        }   

       

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(CompositeProduct entity)
        {

            DbContext.CompositeProducts.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(CompositeProduct original_entity, CompositeProduct entity)
        {

            DbContext.CompositeProducts.Attach(original_entity);
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();

        }
       
        

    }
}
