using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;
using System.Data.Linq;
using InfoControl.Security.Cryptography;

namespace Vivina.Erp.BusinessRules
{
    public class TransporterManager : BusinessManager<InfoControlDataContext>
    {
        public TransporterManager(IDataAccessor container)
            : base(container)
        {
        }

        /// <summary>
        /// Returns a single Transporter
        /// </summary>
        /// <param name="transporterId"></param>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public Transporter GetTransporter(Int32 transporterId)
        {
            return DbContext.Transporters.Where(x => x.TransporterId == transporterId).FirstOrDefault();
        }

        /// <summary>
        /// Returns all transporters of a company
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public IQueryable<Transporter> GetTransportersByCompany(Int32 CompanyId)
        {
            return DbContext.Transporters.Where(x => x.CompanyId == CompanyId);
        }

        /// <summary>
        /// this method retuen the transporter by LegalEntityProfile
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="legalEntityProfileId"></param>
        /// <returns></returns>
        public Transporter GetTransporterByLegalEntityProfile(Int32 companyId, Int32 legalEntityProfileId)
        {
            return
                DbContext.Transporters.Where(
                    t => t.CompanyId == companyId && t.LegalEntityProfileId == legalEntityProfileId).FirstOrDefault();
        }

        /// <summary>
        /// Return all transporters of a company, as table
        /// this method are ready for client sort and page
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetTransportersByCompany(Int32 companyId, string sortExpression, int startRowIndex,
                                                   int maximumRows)
        {
            var query = from transporter in GetTransportersByCompany(companyId)
                        join legalEntityProfile in DbContext.LegalEntityProfiles on transporter.LegalEntityProfileId
                            equals legalEntityProfile.LegalEntityProfileId into gLegalEntityProfile
                        from legalEntityProfile in gLegalEntityProfile.DefaultIfEmpty()
                        select new
                                   {
                                       Name = legalEntityProfile.CompanyName,
                                       legalEntityProfile.Phone,
                                       legalEntityProfile.Email,
                                       legalEntityProfile.CNPJ,
                                       transporter.ModifiedDate,
                                       transporter.TransporterId,
                                       transporter.CompanyId,
                                       transporter.LegalEntityProfileId,
                                       transporter.Company,
                                       transporter.LegalEntityProfile,
                                       transporter.State,
                                       transporter.StateId,
                                       transporter.Vendor
                                   };

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "TransporterId").AsQueryable();
        }

        /// <summary>
        /// This method returns quantity of transporters by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int GetTransporterByCompanyCount(int companyId)
        {
            return GetTransportersByCompany(companyId).Count();
        }

        /// <summary>
        /// Basic delete method
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Transporter entity)
        {
            //DbContext.Transporters.Attach(entity);
            DbContext.Transporters.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// basic insert method
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Transporter entity)
        {
            DbContext.Transporters.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// basic update method
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        public void Update(Transporter original_entity, Transporter entity)
        {
            //DbContext.Transporters.Attach(original_entity);
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method return the Transporter by legalEntityProfileId
        /// </summary>
        /// <param name="legalEntityProfileId"></param>
        /// <returns></returns>
        public Transporter GetTransporterByProfile(int legalEntityProfileId)
        {
            return DbContext.Transporters.Where(x => x.LegalEntityProfileId == legalEntityProfileId).FirstOrDefault();
        }

        /// <summary>
        /// this method is used to fill the autocomplete of transporter
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="name"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<Recognizable> SearchTransporter(Int32 companyId, string name, Int32 maximumRows)
        {
            string methodName = MethodBase.GetCurrentMethod().ToString();

            //if (DataManager.CacheCommands[methodName] == null)
            //{

            DataManager.CacheCommands[methodName] =
              CompiledQuery.Compile<InfoControlDataContext, int, string, int, IQueryable<Recognizable>>(
                  (ctx, _companyId, _name, _maximumRows) => (from transporter in DbContext.Transporters
                                                             join legalEntityProfile in DbContext.LegalEntityProfiles on
                                                                 transporter.LegalEntityProfileId equals
                                                                 legalEntityProfile.LegalEntityProfileId
                                                             where
                                                                 transporter.CompanyId == companyId &&
                                                                 (transporter.LegalEntityProfile.CompanyName.Contains(name) ||
                                                                  transporter.LegalEntityProfile.FantasyName.Contains(name))
                                                             select new Recognizable(transporter.TransporterId.ToString(),
                                                                 (transporter.LegalEntityProfile.CNPJ) + " | " +
                                                                 (transporter.LegalEntityProfile.CompanyName))).Take(maximumRows));

            //   query = query.Take(maximumRows);

            // DataManager.CacheCommands[methodName] = DbContext.GetCommand(query);
            //}

            var method =
              (Func<InfoControlDataContext, int, string, int, IQueryable<Recognizable>>)
              DataManager.CacheCommands[methodName];

            return method(DbContext, companyId, name, maximumRows);

            //DataReader reader = DataManager.ExecuteCachedQuery(methodName, " | ", companyId, "%" + name + "%",
            //                                                   "%" + name + "%");
            //var list = new List<string>();
            //while (reader.Read())
            //{
            //    list.Add(reader.GetString(0));
            //}
            //return list.ToArray();
        }

        /// <summary>
        /// this method search supplier
        /// </summary>
        /// <param name="htTransporter"></param>
        /// <returns></returns>
        public IQueryable SearchTransporters(Hashtable htTransporter, string sortExpression, int startRowIndex,
                                             int maximumRows)
        {
            var queryTransporter = from transporter in DbContext.Transporters
                                   where transporter.CompanyId == Convert.ToInt32(htTransporter["CompanyId"])
                                   join legalEntityProfile in DbContext.LegalEntityProfiles on
                                       transporter.LegalEntityProfileId equals legalEntityProfile.LegalEntityProfileId
                                   select new
                                              {
                                                  transporter.TransporterId,
                                                  legalEntityProfile.CompanyName,
                                                  legalEntityProfile.FantasyName,
                                                  legalEntityProfile.CNPJ,
                                                  legalEntityProfile.Phone,
                                                  legalEntityProfile.Email,
                                                  legalEntityProfile.Website
                                              };

            if (!String.IsNullOrEmpty(htTransporter["FantasyName"].ToString()))
                queryTransporter =
                    queryTransporter.Where(t => t.FantasyName.Contains(htTransporter["FantasyName"].ToString()));

            if (!String.IsNullOrEmpty(htTransporter["CompanyName"].ToString()))
                queryTransporter =
                    queryTransporter.Where(t => t.CompanyName.Contains(htTransporter["CompanyName"].ToString()));

            if (htTransporter["CNPJ"].ToString() != "__.___.___/____-__")
                queryTransporter = queryTransporter.Where(t => t.CNPJ.Contains(htTransporter["CNPJ"].ToString()));

            if (htTransporter["Phone"].ToString() != "(__)____-____")
                queryTransporter = queryTransporter.Where(t => t.Phone.Contains(htTransporter["Phone"].ToString()));

            if (!String.IsNullOrEmpty(htTransporter["Email"].ToString()))
                queryTransporter = queryTransporter.Where(t => t.Email.Contains(htTransporter["Email"].ToString()));

            if (!String.IsNullOrEmpty(htTransporter["WebSite"].ToString()))
                queryTransporter = queryTransporter.Where(t => t.Website.Contains(htTransporter["WebSite"].ToString()));

            return queryTransporter.SortAndPage(sortExpression, startRowIndex, maximumRows, "TransporterId");
        }

        /// <summary>
        /// This method count returned by SearchTransporters
        /// </summary>
        /// <param name="htTransporter"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 SearchTransportersCount(Hashtable htTransporter, string sortExpression, int startRowIndex,
                                             int maximumRows)
        {
            return
                SearchTransporters(htTransporter, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        ///// <summary>
        ///// this method search transporter using a hashtable object
        ///// </summary>
        ///// <param name="htTransporter"></param>
        ///// <returns></returns>
        //public DataTable SearchTransporters(Hashtable htTransporter)
        //{
        //    //create a new StringBuilder
        //    System.Text.StringBuilder sbSql = new System.Text.StringBuilder();
        //    sbSql.Append("Select Transporter.TransporterId, LegalEntityProfile.CompanyName, LegalEntityProfile.CNPJ, LegalEntityProfile.Email, LegalEntityProfile.Phone");
        //    sbSql.Append(" From Transporter Inner Join LegalEntityProfile On Transporter.LegalEntityProfileId = LegalEntityProfile.LegalEntityProfileId");

        //    System.Text.StringBuilder sbWhere = new System.Text.StringBuilder();
        //    if (htTransporter != null)
        //    {
        //        DataManager.Parameters.Add("@CompanyId", Convert.ToInt32(htTransporter["CompanyId"]));
        //        sbWhere.Append(" Transporter.CompanyId = @CompanyId and");

        //        if (!String.IsNullOrEmpty(htTransporter["FantasyName"].ToString()))
        //        {
        //            DataManager.Parameters.Add("@FantasyName", htTransporter["FantasyName"].ToString());
        //            sbWhere.Append(" LegalEntityProfile.FantasyName = @FantasyName and");
        //        }

        //        if (!String.IsNullOrEmpty(htTransporter["CompanyName"].ToString()))
        //        {
        //            DataManager.Parameters.Add("@CompanyName", htTransporter["CompanyName"].ToString());
        //            sbWhere.Append(" LegalEntityProfile.CompanyName = @CompanyName and");
        //        }

        //        if (htTransporter["CNPJ"].ToString() != "__.___.___/____-__")
        //        {
        //            DataManager.Parameters.Add("@CNPJ", htTransporter["CNPJ"].ToString());
        //            sbWhere.Append(" LegalEntityProfile.CNPJ = @CNPJ and");
        //        }

        //        if (htTransporter["Phone"].ToString() != "(__)____-____")
        //        {
        //            DataManager.Parameters.Add("@Phone", htTransporter["Phone"].ToString());
        //            sbWhere.Append(" LegalEntityProfile.Phone = @Phone and");
        //        }

        //        if (!String.IsNullOrEmpty(htTransporter["Email"].ToString()))
        //        {
        //            DataManager.Parameters.Add("@Email", htTransporter["Email"].ToString());
        //            sbWhere.Append(" LegalEntityProfile.Email = @Email and");
        //        }

        //        if (!String.IsNullOrEmpty(htTransporter["WebSite"].ToString()))
        //        {
        //            DataManager.Parameters.Add("@WebSite", htTransporter["WebSite"].ToString());
        //            sbWhere.Append(" LegalEntityProfile.WebSite = @WebSite and");
        //        }
        //    }
        //    if (sbWhere.Length != 0)
        //    {
        //        sbSql.Append(" where");
        //        sbSql.Append(sbWhere.Remove(sbWhere.Length - 3, 3));
        //    }
        //    return DataManager.ExecuteDataTable(sbSql.ToString());
        //}

        /// <summary>
        /// this method return the Transporter by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Transporter GetTransporterByName(string name, Int32 companyId)
        {
            return
                DbContext.Transporters.Where(
                    t =>
                    (t.LegalEntityProfile.CompanyName.Contains(name) || t.LegalEntityProfile.FantasyName.Contains(name)) &&
                    t.CompanyId == companyId).FirstOrDefault();
        }
    }
}