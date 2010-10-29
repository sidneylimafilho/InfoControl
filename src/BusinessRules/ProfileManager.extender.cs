using System;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class ProfileManager
    {
        public const Int32 CNPJLENGTH = 18;
        public const Int32 CPFLENGTH = 14;

        /// <summary>
        /// Returns a single row of the Profile table
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public Profile GetProfile(int profileId)
        {
            //
            return DbContext.Profiles.Where(x => x.ProfileId == profileId).FirstOrDefault();
        }

        /// <summary>
        /// This method return the profile by cpf
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public Profile GetProfile(string cpf)
        {
            //
            return DbContext.Profiles.Where(profile => profile.CPF == cpf).FirstOrDefault();
        }

        public Profile GetProfile(int companyId, string cpf)
        {
            IQueryable<Profile> query = from profile in DbContext.Profiles
                                        join customer in DbContext.Customers on profile.ProfileId equals
                                            customer.ProfileId into gCustomers
                                        from customer in gCustomers.DefaultIfEmpty()
                                        join supplier in DbContext.Suppliers on profile.ProfileId equals
                                            supplier.ProfileId into gSuppliers
                                        from supplier in gSuppliers.DefaultIfEmpty()
                                        join employee in DbContext.Employees on profile.ProfileId equals
                                            employee.ProfileId into gEmployees
                                        from employee in gEmployees.DefaultIfEmpty()
                                        join representant in DbContext.Representants on profile.ProfileId equals
                                            representant.ProfileId into gRepresentants
                                        from representant in gRepresentants.DefaultIfEmpty()

                                        join user in DbContext.Users on profile.ProfileId equals user.ProfileId into gUsers
                                        from user in gUsers.DefaultIfEmpty()
                                        where
                                            ((profile.CPF == cpf) &&
                                             (user.ProfileId.HasValue || customer.CompanyId == companyId || supplier.CompanyId == companyId ||
                                              employee.CompanyId == companyId || representant.CompanyId == companyId))
                                        select profile;

            return query.FirstOrDefault();
        }


        /// <summary>
        /// This method returns rows of the side table "EducationLevels" of the same companyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<EducationLevel> GetAllEducationLevel()
        {
            //
            return DbContext.EducationLevels.Sort("NAME");
        }

        /// <summary>
        /// This method returns rows of the side table "MaritalState" of the same companyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<MaritalStatus> GetMaritalStatus()
        {
            //
            return DbContext.MaritalStatus.Sort("NAME");
        }

        /// <summary>
        /// This method return the company profile by cnpj 
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public LegalEntityProfile GetLegalEntityProfile(string cnpj)
        {
            return DbContext.LegalEntityProfiles.Where(cp => cp.CNPJ == cnpj).FirstOrDefault();
        }

        /// <summary>
        /// This method return the Lawyer profile by process
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public Profile GetLawyerProfile(string professionalRegister)
        {
            return DbContext.Profiles.Where(x => x.ProfessionalRegister == professionalRegister).FirstOrDefault();
        }

        /// <summary>
        /// This method return the company profile by id
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public LegalEntityProfile GetLegalEntityProfile(int LegalEntityProfileId)
        {
            return
                DbContext.LegalEntityProfiles.Where(p => p.LegalEntityProfileId == LegalEntityProfileId).FirstOrDefault();
        }

        /// <summary>
        /// This method return true if the profile exists
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public bool ProfileExists(string cpf)
        {
            Profile profile = new ProfileManager(this).GetProfile(cpf);
            return profile != null;
        }


        /// <summary>
        /// This method return true if the LegalEntity exists
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public bool LegalEntityExists(string cnpj)
        {
            LegalEntityProfile profile = new ProfileManager(this).GetLegalEntityProfile(cnpj);
            return profile != null && profile.CNPJ != "99.999.999/0001-91";
        }




        /// <summary>
        /// This method returns the cpf numbers that exist by company in db
        /// </summary>
        /// <param name="companyId">can't be null</param>
        /// <returns></returns>
        public IQueryable<string> GetCPFnumbers(Int32 companyId)
        {
            IQueryable<string> query = from profile in DbContext.Profiles
                                       join customer in DbContext.Customers on profile.ProfileId equals
                                           customer.ProfileId into gCustomers
                                       from customer in gCustomers.DefaultIfEmpty()
                                       join supplier in DbContext.Suppliers on profile.ProfileId equals
                                           supplier.ProfileId into gSuppliers
                                       from supplier in gSuppliers.DefaultIfEmpty()
                                       join employee in DbContext.Employees on profile.ProfileId equals
                                           employee.ProfileId into gEmployees
                                       from employee in gEmployees.DefaultIfEmpty()
                                       join representant in DbContext.Representants on profile.ProfileId equals
                                           representant.ProfileId into gRepresentants
                                       from representant in gRepresentants.DefaultIfEmpty()
                                       where customer.CompanyId == companyId || supplier.CompanyId == companyId ||
                                            employee.CompanyId == companyId || representant.CompanyId == companyId
                                       select profile.CPF;

            return query;

        }

        /// <summary>
        /// This method returns the cnpj numbers that exist by company in db
        /// </summary>
        /// <param name="companyId">can't be null</param>
        /// <returns></returns>
        public IQueryable<string> GetCNPJnumbers(Int32 companyId)
        {

            IQueryable<string> query = from legalEntityProfile in DbContext.LegalEntityProfiles
                                       join customer in DbContext.Customers on legalEntityProfile.LegalEntityProfileId equals
                                           customer.LegalEntityProfileId into gCustomers
                                       from customer in gCustomers.DefaultIfEmpty()

                                       join supplier in DbContext.Suppliers on legalEntityProfile.LegalEntityProfileId equals
                                           supplier.LegalEntityProfileId into gSuppliers
                                       from supplier in gSuppliers.DefaultIfEmpty()

                                       join company in DbContext.Companies on legalEntityProfile.LegalEntityProfileId equals
                                       company.LegalEntityProfileId into gCompanies
                                       from company in gCompanies.DefaultIfEmpty()

                                       join representant in DbContext.Representants on legalEntityProfile.LegalEntityProfileId equals
                                           representant.LegalEntityProfileId into gRepresentants
                                       from representant in gRepresentants.DefaultIfEmpty()

                                       where customer.CompanyId == companyId || supplier.CompanyId == companyId ||
                                            company.CompanyId == companyId || representant.CompanyId == companyId

                                       select legalEntityProfile.CNPJ;

            return query;
        }


    }
}