namespace Vivina.Erp.BusinessRules
{
    //public partial class CheckManager : InfoControl.Data.BusinessManager<InfoControlDataContext>
    //{
    //    public CheckManager(IDataAccessor container) : base(container) { }
    //    /// <summary>
    //    /// This method retrieves all Checks.
    //    /// Change this method to alter how records are retrieved.
    //    /// </summary>
    //    public IQueryable<Check> GetAllChecks()
    //    {

    //        return DbContext.Checks;
    //    }

    //    /// <summary>
    //    /// This method gets record counts of all Checks.
    //    /// Do not change this method.
    //    /// </summary>
    //    public int GetAllChecksCount()
    //    {
    //        return GetAllChecks().Count();
    //    }

    //    /// <summary>
    //    /// This method retrieves a single Check.
    //    /// Change this method to alter how that record is received.
    //    /// </summary>
    //    /// <param name=CheckId>CheckId</param>
    //    public Check GetCheck(Int32 CheckId)
    //    {

    //        return DbContext.Checks.Where(x => x.CheckId == CheckId).FirstOrDefault();
    //    }

    //    /// <summary>
    //    /// This method retrieves Check by Bank.
    //    /// Change this method to alter how records are retrieved.
    //    /// </summary>
    //    /// <param name=BankId>BankId</param>
    //    public IQueryable<Check> GetCheckByBank(Int32 BankId)
    //    {

    //        return DbContext.Checks.Where(x => x.BankId == BankId);
    //    }        

    //    /// <summary>
    //    /// This method deletes a record in the table.
    //    /// Change this method to alter how records are deleted.
    //    /// </summary>
    //    /// <param name=entity>entity</param>
    //    public void Delete(Check entity)
    //    {

    //        DbContext.Checks.Attach(entity);
    //        DbContext.Checks.DeleteOnSubmit(entity);
    //        DbContext.SubmitChanges();
    //    }

    //    /// <summary>
    //    /// This method inserts a new record in the table.
    //    /// Change this method to alter how records are inserted.
    //    /// </summary>
    //    /// <param name=entity>entity</param>
    //    public void Insert(Check entity)
    //    {

    //        DbContext.Checks.InsertOnSubmit(entity);
    //        DbContext.SubmitChanges();
    //    }

    //    /// <summary>
    //    /// This method updates a record in the table.
    //    /// Change this method to alter how records are updated.
    //    /// </summary>
    //    /// <param name=original_entity>original_entity</param>
    //    /// <param name=entity>entity</param>
    //    public void Update(Check original_entity, Check entity)
    //    {

    //        DbContext.Checks.Attach(original_entity);
    //        original_entity.CopyPropertiesFrom(entity);
    //        DbContext.SubmitChanges();

    //    }

    //}
}