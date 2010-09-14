using System;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class RepresentantManager : BusinessManager<InfoControlDataContext>
    {
        public RepresentantManager(IDataAccessor container)
            : base(container)
        {
        }

        /// <summary>
        /// This method return representants by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
#warning este metodo esta com o codigo duplicado
        public IQueryable GetRepresentantsByCompany(Int32 companyId, string sortExpression, String initialLetter,
                                                    int startRowIndex, int maximumRows)
        {
            var query =
                from representant in DbContext.Representants.Where(representant => representant.CompanyId == companyId)
                join legalEntityProfile in DbContext.LegalEntityProfiles on representant.LegalEntityProfileId equals
                    legalEntityProfile.LegalEntityProfileId into gLegalEntityProfile
                from legalEntityProfile in gLegalEntityProfile.DefaultIfEmpty()
                join profile in DbContext.Profiles on representant.ProfileId equals profile.ProfileId into gProfile
                from profile in gProfile.DefaultIfEmpty()
                select new
                           {
                               Name = legalEntityProfile.CompanyName ?? profile.Name,
                               Phone = legalEntityProfile.Phone ?? profile.Phone,
                               Identification = legalEntityProfile.CNPJ ?? profile.CPF,
                               Email = legalEntityProfile.Email ?? profile.Email,
                               representant.RepresentantId,
                               representant.ModifiedDate,
                               representant.CompanyId,
                               representant.Profile,
                               representant.ProfileId,
                               representant.LegalEntityProfile,
                               representant.LegalEntityProfileId,
                               representant.BankId,
                               representant.AccountNumber,
                               representant.Agency,
                               representant.Rating
                           };

            if (!String.IsNullOrEmpty(initialLetter))
                query = query.Where(rep => rep.Name.StartsWith(initialLetter));

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "RepresentantId");
        }

        /// <summary>
        /// This method return representants by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable GetRepresentantsByCompany(Int32 companyId)
        {
            var query =
                from representant in DbContext.Representants.Where(representant => representant.CompanyId == companyId)
                join legalEntityProfile in DbContext.LegalEntityProfiles on representant.LegalEntityProfileId equals
                    legalEntityProfile.LegalEntityProfileId into gLegalEntityProfile
                from legalEntityProfile in gLegalEntityProfile.DefaultIfEmpty()
                join profile in DbContext.Profiles on representant.ProfileId equals profile.ProfileId into gProfile
                from profile in gProfile.DefaultIfEmpty()
                select new
                           {
                               Name = legalEntityProfile.CompanyName ?? profile.Name,
                               Phone = legalEntityProfile.Phone ?? profile.Phone,
                               Identification = legalEntityProfile.CNPJ ?? profile.CPF,
                               Email = legalEntityProfile.Email ?? profile.Email,
                               representant.RepresentantId,
                               representant.ModifiedDate,
                               representant.CompanyId,
                               representant.Profile,
                               representant.ProfileId,
                               representant.LegalEntityProfile,
                               representant.LegalEntityProfileId,
                               representant.BankId,
                               representant.AccountNumber,
                               representant.Agency,
                               representant.Rating
                           };
            return query;
        }

        public Int32 GetRepresentantsByCompanyCount(Int32 companyId, string sortExpression, String initialLetter,
                                                    int startRowIndex, int maximumRows)
        {
            return
                GetRepresentantsByCompany(companyId, sortExpression, initialLetter, startRowIndex, maximumRows).Cast
                    <IQueryable>().Count();
        }


        /// <summary>
        /// This method inserts a new relationship between representant and user
        /// </summary>
        /// <param name="representantUser"> can't be null</param>
        public void InsertRepresentantUser(RepresentantUser representantUser)
        {
            DbContext.RepresentantUsers.InsertOnSubmit(representantUser);
            DbContext.SubmitChanges();
        }


        /// <summary>
        /// This method deletes a relationship  between representant and user
        /// </summary>
        /// <param name="representantId">can't be null</param>
        /// <param name="userId">can't be null</param>
        public void DeleteRepresentantUser(Int32 userId)
        {
            DbContext.RepresentantUsers.DeleteOnSubmit(GetRepresentantUser(userId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method retrieve a specific RepresentantUser entity
        /// </summary>
        /// <param name="userId">can't be null</param>
        /// <returns></returns>
        public RepresentantUser GetRepresentantUser(Int32 userId)
        {
            return DbContext.RepresentantUsers.Where(representantUser => representantUser.UserId == userId).FirstOrDefault();
        }

        /// <summary>
        /// this method delete Representant
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Representant entity)
        {
            //DbContext.Representants.Attach(entity);
            DbContext.Representants.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method inserts a representant
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Representant entity)
        {
            DbContext.Representants.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method updates a Representant
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        public void update(Representant original_entity, Representant entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method return a Representant
        /// </summary>
        /// <param name="representantId"></param>
        public Representant GetRepresentant(Int32 representantId)
        {
            return
                DbContext.Representants.Where(representant => representant.RepresentantId == representantId).
                    FirstOrDefault();
        }

        /// <summary>
        /// this method return a representant in accordance with his LegalEntityProfile
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="LegalEntityProfileId"></param>
        /// <returns></returns>
        public Representant GetRepresentantByLegalEntityProfile(int companyId, int LegalEntityProfileId)
        {
            return
                DbContext.Representants.Where(
                    x => x.CompanyId == companyId && x.LegalEntityProfileId == LegalEntityProfileId).FirstOrDefault();
        }

        /// <summary>
        /// This method returns a representant in accordance with his profile
        /// </summary>
        ///<param name="companyId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public Representant GetRepresentantByProfile(int companyId, int profileId)
        {
            return
                DbContext.Representants.Where(x => x.CompanyId == companyId && x.ProfileId == profileId).FirstOrDefault();
        }
    }
}