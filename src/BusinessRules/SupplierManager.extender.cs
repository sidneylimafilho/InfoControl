using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;
using InfoControl.Security.Cryptography;

namespace Vivina.Erp.BusinessRules
{
    public partial class SupplierManager
    {
        #region AdvancedCRUD

        /// <summary>
        /// This method insert a supplier and return the supplier id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertRetrievingID(Supplier entity)
        {
            //   
            DbContext.Suppliers.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
            return entity.SupplierId;
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(Supplier original_entity, Supplier entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            original_entity.ModifiedDate = DateTime.Now;
            DbContext.SubmitChanges();
        }

        #endregion

        /// <summary>
        /// This method returns a single supplier by this name
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public Supplier GetSupplierByName(int companyId, string supplier)
        {
            //   
            return
                DbContext.Suppliers.Where(
                    x =>
                    x.CompanyId == companyId &&
                    (x.Profile.Name == supplier || x.LegalEntityProfile.CompanyName == supplier)).FirstOrDefault();
        }

        /// <summary>
        /// This method returns a Contact associated with a Supplier
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns>Contact</returns>
        public Contact GetSupplierContact(int contactId)
        {
            return new ContactManager(this).GetContact(contactId);
        }

        /// <summary>
        /// this method return a supplier in accordance with your LegalEntityProfile
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="legalEntityProfileId"></param>
        /// <returns></returns>
        public Supplier GetSuppliersByLegalEntityProfile(int companyId, int legalEntityProfileId)
        {
            return
                DbContext.Suppliers.Where(
                    x => x.CompanyId == companyId && x.LegalEntityProfileId == legalEntityProfileId).FirstOrDefault();
        }

        /// <summary>
        /// This method returns a supplier in accordance with your profile
        /// </summary>
        ///<param name="companyId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public Supplier GetSupplierByProfile(int companyId, int profileId)
        {
            return DbContext.Suppliers.Where(x => x.CompanyId == companyId && x.ProfileId == profileId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves all Suppliers.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Supplier> GetAllSuppliers()
        {
            ////   

            return DbContext.Suppliers;
        }

        /// <summary>
        /// This method gets record counts of all Suppliers.
        /// Do not change this method.
        /// </summary>
        public int GetAllSuppliersCount()
        {
            return GetAllSuppliers().Count();
        }

        /// <summary>
        /// This method retrieves a single Supplier.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=supplierId>supplierId</param>
        /// <param name=companyId>companyId</param>
        public Supplier GetSupplier(Int32 supplierId, Int32 companyId)
        {
            return
                DbContext.Suppliers.Where(x => x.SupplierId == supplierId && x.CompanyId == companyId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves a single Supplier.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=SupplierId>SupplierId</param>
        /// <param name=CompanyId>CompanyId</param>
        public Supplier GetSupplier(Int32 companyId, string cpfOrCnpj)
        {
            return GetSupplierByCompany(companyId)
                        .Where(x => x.Profile.CPF == cpfOrCnpj || x.LegalEntityProfile.CNPJ == cpfOrCnpj)
                        .FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves Supplier by Company.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<Supplier> GetSupplierByCompany(Int32 companyId)
        {
            return DbContext.Suppliers.Where(x => x.CompanyId == companyId);
        }

        /// <summary>
        /// Return all suppliers of a company, as queryable
        /// this method are ready for client sort and page
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetSupplierByCompany(Int32 companyId, string sortExpression, String initialLetter,
                                               int startRowIndex, int maximumRows)
        {
            var query = from supplier in GetSupplierByCompany(companyId)
                        join legalEntityProfile in DbContext.LegalEntityProfiles on supplier.LegalEntityProfileId equals
                            legalEntityProfile.LegalEntityProfileId into gLegalEntityProfile
                        from legalEntityProfile in gLegalEntityProfile.DefaultIfEmpty()
                        join profile in DbContext.Profiles on supplier.ProfileId equals profile.ProfileId into gProfile
                        from profile in gProfile.DefaultIfEmpty()
                        select new
                                   {
                                       Name = legalEntityProfile.CompanyName ?? profile.Name,
                                       Phone = legalEntityProfile.Phone ?? profile.Phone,
                                       Identification = legalEntityProfile.CNPJ ?? profile.CPF,
                                       Email = legalEntityProfile.Email ?? profile.Email,
                                       supplier.SupplierId,
                                       supplier.ModifiedDate,
                                       supplier.CompanyId,
                                       supplier.PostalCode,
                                       supplier.Profile,
                                       supplier.ProfileId,
                                       supplier.LegalEntityProfile,
                                       supplier.LegalEntityProfileId,
                                       supplier.BankId,
                                       supplier.AccountNumber,
                                       supplier.Agency,
                                       supplier.Ranking
                                   };

            if (!String.IsNullOrEmpty(initialLetter))
                query = query.Where(sup => sup.Name.StartsWith(initialLetter));

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "SupplierId").AsQueryable();
        }

        public Int32 GetSupplierByCompanyCount(Int32 companyId, string sortExpression, String initialLetter,
                                               int startRowIndex, int maximumRows)
        {
            return
                GetSupplierByCompany(companyId, sortExpression, initialLetter, startRowIndex, maximumRows).Cast
                    <IQueryable>().Count();
        }

        /// <summary>
        /// This method returns quantity of suppliers by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int GetSuppliersByCompanyCount(int companyId)
        {
            return GetSupplierByCompany(companyId).Count();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<Supplier> GetFilteredSuppliers(string tableName, Int32 Company_CompanyId)
        {
            switch (tableName)
            {
                case "Company_Suppliers":
                    return GetSupplierByCompany(Company_CompanyId);
                default:
                    return GetAllSuppliers();
            }
        }

        /// <summary>
        /// This method return name of supplier
        /// </summary>
        /// <param name=entity>entity</param>
        public IList getNames(int matrixId)
        {
            var query = from supplier in DbContext.Suppliers
                        join profile in DbContext.Profiles
                            on supplier.ProfileId equals profile.ProfileId
                            into gprofile
                        from profile in gprofile.DefaultIfEmpty()
                        join legalEntityProfile in DbContext.LegalEntityProfiles
                            on supplier.LegalEntityProfileId equals legalEntityProfile.LegalEntityProfileId
                            into gLegalEntityProfile
                        from legalEntityProfile in gLegalEntityProfile.DefaultIfEmpty()
                        where supplier.CompanyId == matrixId
                        select
                            new
                                {
                                    Name = profile.Name ?? "" + legalEntityProfile.CompanyName ?? "",
                                    supplier.SupplierId
                                };
            return query.ToList();
        }

        #region SearchMethods

        public IQueryable<Recognizable> SearchSupplier(Int32 companyId, string name, Int32 maximumRows)
        {
            string methodName = MethodBase.GetCurrentMethod().ToString();

            if (DataManager.CacheCommands[methodName] == null)
            {
                DataManager.CacheCommands[methodName] =
                    CompiledQuery.Compile<InfoControlDataContext, int, string, int, IQueryable<Recognizable>>(
                        (ctx, _companyId, _name, _maximumRows) =>
                        (from supplier in ctx.Suppliers
                         join profile in ctx.Profiles on supplier.ProfileId equals profile.ProfileId into gprofile
                         from profile in gprofile.DefaultIfEmpty()
                         join legalEntityProfile in ctx.LegalEntityProfiles on supplier.LegalEntityProfileId equals
                             legalEntityProfile.LegalEntityProfileId into gLegalEntityProfile
                         from legalEntityProfile in gLegalEntityProfile.DefaultIfEmpty()
                         where
                             supplier.CompanyId == _companyId &&
                             (profile.Name.Contains(_name) || legalEntityProfile.CompanyName.Contains(_name) ||
                              legalEntityProfile.FantasyName.Contains(_name))
                         select new Recognizable(supplier.SupplierId.ToString(),
                             (profile.CPF ?? legalEntityProfile.CNPJ) + " | " +
                             (profile.Name ?? legalEntityProfile.FantasyName ?? legalEntityProfile.CompanyName))).Take(_maximumRows));
            }

            var method =
                (Func<InfoControlDataContext, int, string, int, IQueryable<Recognizable>>)
                DataManager.CacheCommands[methodName];

            return method(DbContext, companyId, name, maximumRows);
        }

        /// <summary>
        /// this method search supplier using a hashtable object 
        /// </summary>
        /// <param name="htSupplier"></param>
        /// <returns></returns>
        public IQueryable SearchSuppliers(Hashtable htSupplier, string initialLetter, string sortExpression,
                                          int startRowIndex, int maximumRows)
        {
            ///retrieving suppliers by companyId
            IQueryable<Supplier> querySuppliers = from supplier in DbContext.Suppliers
                                                  where supplier.CompanyId == Convert.ToInt32(htSupplier["CompanyId"])
                                                  select supplier;

            ///retrieving supplierContacts related with supplier
            IQueryable<SupplierContact> querySupplierContacts = from supplier in querySuppliers
                                                                join supplierContacts in DbContext.SupplierContacts on
                                                                    supplier.SupplierId equals
                                                                    supplierContacts.SupplierId into gSupplierContacts
                                                                from supplierContacts in
                                                                    gSupplierContacts.DefaultIfEmpty()
                                                                select supplierContacts;

            if (!String.IsNullOrEmpty(htSupplier["Contact"] as String))
            {
                ///join supplierContact whit supplier
                querySuppliers = from supplier in querySuppliers
                                 join supplierContact in querySupplierContacts on supplier.SupplierId equals
                                     supplierContact.SupplierId
                                 where supplierContact.Contact.Name.Contains(htSupplier["Contact"].ToString())
                                 select supplier;

                /// after join, retrieving supplier using distinc Method
                querySuppliers = querySuppliers.Distinct();

                ///Distinct method is used to discriminate the suppliers, because the relationship of suppliers and
                ///contact return suppliers with only one distinc property(contact)
            }

            var query = from suppliers in querySuppliers
                        join profiles in DbContext.Profiles on suppliers.ProfileId equals profiles.ProfileId into
                            gProfiles
                        from profiles in gProfiles.DefaultIfEmpty()
                        join legalEntityProfiles in DbContext.LegalEntityProfiles on suppliers.LegalEntityProfileId
                            equals legalEntityProfiles.LegalEntityProfileId into gLegalEntityProfiles
                        from legalEntityProfiles in gLegalEntityProfiles.DefaultIfEmpty()
                        select new
                                   {
                                       suppliers.SupplierId,
                                       suppliers.ModifiedDate,
                                       suppliers.PostalCode,
                                       suppliers.ProfileId,
                                       suppliers.LegalEntityProfileId,
                                       suppliers.CompanyId,
                                       suppliers.BankId,
                                       suppliers.AccountNumber,
                                       suppliers.Agency,
                                       suppliers.Ranking,
                                       ProfileName = suppliers.Profile.Name,
                                       suppliers.LegalEntityProfile.CompanyName,
                                       suppliers.LegalEntityProfile.FantasyName,
                                       Name = profiles.Name ?? legalEntityProfiles.CompanyName,
                                       Identification = profiles.CPF ?? legalEntityProfiles.CNPJ,
                                       Email = profiles.Email ?? legalEntityProfiles.Email,
                                       Phone = profiles.Phone ?? legalEntityProfiles.Phone,
                                       votingTitle = profiles != null
                                                         ? profiles.VotingTitle
                                                         : String.Empty,
                                       RG = profiles != null
                                                ? profiles.RG
                                                : String.Empty
                                   };

            //if (!String.IsNullOrEmpty(htSupplier["SupplierId"] as String))
            //    query = query.Where(s => s.SupplierId == Convert.ToInt32(htSupplier["SupplierId"].ToString()));

            if (!String.IsNullOrEmpty(htSupplier["SelectedSupplier"] as String))
                query = query.Where(s => s.Name.Contains(htSupplier["SelectedSupplier"].ToString()));

            if (htSupplier["CPF"].ToString() != "___.___.___-__")
                query = query.Where(s => s.Identification.Contains(htSupplier["CPF"].ToString()));
            else if (htSupplier["CNPJ"].ToString() != "__.___.___/____-__")
                query = query.Where(s => s.Identification.Contains(htSupplier["CNPJ"].ToString()));

            if (!String.IsNullOrEmpty(htSupplier["Email"] as String))
                query = query.Where(s => s.Email.Contains(htSupplier["Email"].ToString()));

            if (!String.IsNullOrEmpty(htSupplier["votingTitle"] as String))
                query = query.Where(s => s.votingTitle.Contains(htSupplier["votingTitle"].ToString()));

            if (!String.IsNullOrEmpty(htSupplier["RG"] as String))
                query = query.Where(s => s.RG.Contains(htSupplier["RG"].ToString()));

            if (htSupplier["Phone"].ToString() != "(__)____-____")
                query = query.Where(s => s.Phone.Contains(htSupplier["Phone"].ToString()));

            if (Convert.ToInt32(htSupplier["Ranking"]) > 0)
                query = query.Where(c => c.Ranking.Value >= Convert.ToInt32(htSupplier["Ranking"]));

            if (!String.IsNullOrEmpty(initialLetter))
                query = query.Where(sup => sup.Name.StartsWith(initialLetter));

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "SupplierId");
        }

        public Int32 SearchSuppliersCount(Hashtable htSupplier, string initialLetter, string sortExpression,
                                          int startRowIndex, int maximumRows)
        {
            return
                SearchSuppliers(htSupplier, initialLetter, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>()
                    .Count();
        }

        #endregion

        #region SupplierContacts

        /// <summary>
        /// Get all SupplierContacts of a company, as List
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="supplierId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public List<Contact> GetContactsByCompanyAsList(Int32 companyId, Int32 supplierId, string sortExpression,
                                                        int startRowIndex, int maximumRows)
        {
            //   
            IQueryable<Contact> query =
                from sc in DbContext.SupplierContacts
                join contacts in DbContext.Contacts on sc.ContactId equals contacts.ContactId
                where (sc.SupplierId == supplierId)
                select contacts;
            return query.ToList();
        }

        /// <summary>
        /// Basic Insert Method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="companyId"></param>
        /// <param name="supplierId"></param>
        public void AddContact(Contact entity, Int32 companyId, Int32 supplierId)
        {
            var contactManager = new ContactManager(this);
            entity.CompanyId = companyId;
            contactManager.Insert(entity);

            var scManager = new SuppliersContactManager(this);
            var sc = new SupplierContact();
            sc.SupplierId = supplierId;
            sc.ContactId = entity.ContactId;
            sc.CompanyId = companyId;
            scManager.Insert(sc);
        }

        /// <summary>
        /// Basic Delete Method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="supplierId"></param>
        /// <param name="companyId"></param>
        public void RemoveContact(Contact entity, Int32 supplierId, Int32 companyId)
        {
            var scManager = new SuppliersContactManager(this);
            var sc = new SupplierContact
                         {
                             SupplierId = supplierId,
                             ContactId = entity.ContactId
                         };
            scManager.Delete(sc);

            var contactManager = new ContactManager(this);
            contactManager.Delete(entity);
        }

        /// <summary>
        /// Basic Update Method
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="entity"></param>
        public void UpdateContact(int contactId, Contact entity)
        {
            var contactManager = new ContactManager(this);
            Contact originalContact = contactManager.GetContact(contactId);
            contactManager.Update(originalContact, entity);
        }

        #endregion

        #region SupplierCategory

        /// <summary>
        /// Returns all supplier categories
        /// </summary>
        /// <returns></returns>
        public IQueryable<SupplierCategory> GetSupplierCategory()
        {
            return DbContext.SupplierCategories;
        }

        #endregion
    }
}