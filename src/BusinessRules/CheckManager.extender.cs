namespace Vivina.Erp.BusinessRules
{
    public class CheckManager
    {
        /// <summary>
        /// This method return all checks of a company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        //public IQueryable<Check> GetChecks(int companyId)
        //{
        //    return DbContext.Checks.Where( c=> c.co);
        //}

        /// <summary>
        /// /// This method return all checks of a company, enabling paging and sorting in client side
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        //public List<Check> GetChecksAsList(int companyId, string sortExpression, int startRowIndex, int maximumRows)
        //{
        //    IQueryable<Check> x = GetChecks(companyId);
        //    return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "CheckId").ToList();
        //}
    }
}