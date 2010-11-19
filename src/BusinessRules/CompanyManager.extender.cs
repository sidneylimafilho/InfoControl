using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Web.Security;
using InfoControl;
using InfoControl.Data;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules.WebSites;
using Vivina.Erp.DataClasses;
using Application = InfoControl.Web.Configuration.Application;
using User = InfoControl.Web.Security.DataEntities.User;

namespace Vivina.Erp.BusinessRules
{
    public partial class CompanyManager
    {
        #region CompanySettings

        #region DocumentsTemplates

        public IQueryable<DocumentTemplateType> GetAllDocumentTemplateTypes()
        {
            return DbContext.DocumentTemplateTypes;
        }

        /// <summary>
        /// This method retrieve the documentsTemplates of an specific company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable GetDocumentsTemplatesByCompany(Int32 companyID)
        {
            var query = from documentsTemplates in DbContext.DocumentTemplates
                        join documentTemplateTypes in DbContext.DocumentTemplateTypes on
                            documentsTemplates.DocumentTemplateTypeId equals
                            documentTemplateTypes.DocumentTemplateTypeId
                        where documentsTemplates.CompanyId == companyID
                        select new
                                   {
                                       documentsTemplates.FileName,
                                       documentsTemplates.DocumentTemplateId,
                                       documentsTemplates.FileUrl,
                                       DocumentTemplateTypeName = documentTemplateTypes.Name,
                                       documentTemplateTypes.DocumentTemplateTypeId
                                   };
            return query;
        }

        /// <summary>
        /// This method retrieves documentTemplates by company and documentTemplateType
        /// </summary>
        /// <param name="companyId">can't be null</param>
        /// <param name="documentTemplateType">can't be null</param>
        /// <returns></returns>
        public IQueryable<DocumentTemplate> GetDocumentTemplates(Int32 companyId, Int32 documentTemplateTypeId)
        {
            return DbContext.DocumentTemplates.Where(documentTemplate => documentTemplate.CompanyId == companyId
                && documentTemplate.DocumentTemplateTypeId == documentTemplateTypeId);
        }


        public void InsertDocumentTemplate(DocumentTemplate documentTemplate)
        {
            DbContext.DocumentTemplates.InsertOnSubmit(documentTemplate);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method returns an specific documentTemplate by your ID
        /// </summary>
        /// <param name="DocumentTemplateID"></param>
        /// <returns></returns>
        public DocumentTemplate GetDocumentTemplate(Int32 DocumentTemplateID)
        {
            return
                DbContext.DocumentTemplates.Where(
                    documentTemplate => documentTemplate.DocumentTemplateId == DocumentTemplateID).FirstOrDefault();
        }

        /// <summary>
        /// This method delete an documentTemplate by your ID
        /// </summary>
        /// <param name="documentTemplateId"></param>
        public void DeleteDocumentTemplate(Int32 documentTemplateId)
        {
            DbContext.DocumentTemplates.DeleteOnSubmit(GetDocumentTemplate(documentTemplateId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Method created to implement the fisical delete of document template files
        /// </summary>
        /// <param name="documentTemplateId"></param>
        public void DeleteDocumentTemplate(Int32 documentTemplateId, String FileUrl)
        {
            File.Delete(FileUrl);
            DbContext.DocumentTemplates.DeleteOnSubmit(GetDocumentTemplate(documentTemplateId));
            DbContext.SubmitChanges();
        }

        #endregion

        /// <summary>
        /// Insert a CompanyConfiguration
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="companyId"></param>
        public void InsertCompanyConfiguration(CompanyConfiguration configuration, Company company)
        {
            DbContext.CompanyConfigurations.InsertOnSubmit(configuration);
            DbContext.SubmitChanges();

            company.CompanyConfigurationId = configuration.CompanyConfigurationId;
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Update a CompanyConfiguration
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="original_entity"></param>
        public void UpdateCompanyConfiguration(CompanyConfiguration original_entity, CompanyConfiguration entity)
        {
            original_entity = GetCompanyConfiguration(original_entity.CompanyConfigurationId);
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        #endregion

        #region company

        public IList GetMatrixCompaniesNames(Int32 userId)
        {
            //InfoControlDataContext Context = DataManager.CreateContext<InfoControlDataContext>();
            var query = from company in DbContext.Companies
                        where (company.User.UserId == userId && company.CompanyId == company.MatrixId)
                        select new
                                   {
                                       company.CompanyId,
                                       company.LegalEntityProfile.CompanyName
                                   };
            return query.ToList();
        }

        /// <summary>
        /// M�todo usado para criar a primeira companhia, utilizado na tela de registro
        /// </summary>
        /// <param name="newCompany"></param>
        /// <param name="newUser"></param>
        /// <param name="newProfile"></param>
        /// <returns></returns>
        public InsertCompanyStatus InsertMatrixCompany(Company newCompany, User newUser, Profile profile)
        {
            //
            // Insert the profile
            //
            var pManager = new ProfileManager(this);

            // Profile profile = pManager.GetProfile(newProfile.CPF) ?? newProfile;
            //if (profile.ProfileId == 0)
            pManager.Insert(profile);
            //else
            //{
            //    profile.Name = newProfile.Name;
            //    profile.Email = newProfile.Email;
            //    profile.PostalCode = newProfile.PostalCode;
            //    //profile.Address = newProfile.Address;
            //    profile.AddressComp = newProfile.AddressComp;
            //    profile.AddressNumber = newProfile.AddressNumber;
            //    profile.Phone = newProfile.Phone;
            //    DbContext.SubmitChanges();
            //}

            //
            //Insert Admin user
            //
            Int32 UserId;
            //newUser.ProfileId
            DataClasses.User original_User = GetUserByUserName(newUser.Email);
            if (original_User != null)
            {
                UserId = original_User.UserId;
            }
            else
            {
                MembershipCreateStatus status;
                var membershipManager = new MembershipManager(this);
                newUser.ProfileId = profile.ProfileId;
                membershipManager.Insert(
                    newUser,
                    out status,
                    (Membership.Provider as VivinaMembershipProvider).RequiresValidEmail);

                //
                //verify if the status of the inclusion are ok
                //
                if (status != MembershipCreateStatus.Success)
                {
                    DataManager.Rollback();
                    switch (status)
                    {
                        case MembershipCreateStatus.DuplicateUserName:
                            return InsertCompanyStatus.DuplicatedUserName;
                        case MembershipCreateStatus.InvalidPassword:
                            return InsertCompanyStatus.InvalidPassword;
                        case MembershipCreateStatus.DuplicateEmail:
                            return InsertCompanyStatus.DuplicatedAdminEmail;
                        default:
                            return InsertCompanyStatus.InvalidUser;
                    }
                }
                UserId = newUser.UserId;
            }

            if (newCompany.LegalEntityProfile.IsLiberalProfessional)
                newCompany.LegalEntityProfile.CompanyName = newCompany.LegalEntityProfile.FantasyName = profile.Name;

            var insertCompanyStatus = InsertCompany(newCompany, UserId, 0);

            newCompany.ReferenceCompanyId = newCompany.CompanyId;
            newCompany.MatrixId = newCompany.CompanyId;
            DbContext.SubmitChanges();

            return insertCompanyStatus;
        }

        /// <summary>
        /// Método usado para criar mais companias, referenciando as novas companias, � 1� compania
        /// Ou compania de referência
        /// </summary>
        /// <param name="company"></param>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public InsertCompanyStatus InsertCompany(Company company, int userId, int matrixCompanyId)
        {
            if (GetCompany(company.LegalEntityProfile.CNPJ) != null)
                return InsertCompanyStatus.DuplicateCNPJ;

            var profileManager = new ProfileManager(this);
            LegalEntityProfile original_legalEntityProfile;

            //
            // update the legalEntityProfile
            //
            if (company.LegalEntityProfile != null)
            {
                original_legalEntityProfile = profileManager.GetLegalEntityProfile(company.LegalEntityProfile.CNPJ);
                if (original_legalEntityProfile != null)
                {
                    //update the legalEntityProfile
                    if (!String.IsNullOrEmpty(company.LegalEntityProfile.CompanyName))
                        original_legalEntityProfile.CompanyName = company.LegalEntityProfile.CompanyName;

                    if (!String.IsNullOrEmpty(company.LegalEntityProfile.FantasyName))
                        original_legalEntityProfile.FantasyName = company.LegalEntityProfile.FantasyName;

                    original_legalEntityProfile.Email = company.LegalEntityProfile.Email;
                    original_legalEntityProfile.IE = company.LegalEntityProfile.IE;
                    original_legalEntityProfile.Phone = company.LegalEntityProfile.Phone;
                    original_legalEntityProfile.Address = company.LegalEntityProfile.Address;
                    original_legalEntityProfile.AddressComp = company.LegalEntityProfile.AddressComp;
                    original_legalEntityProfile.AddressNumber = company.LegalEntityProfile.AddressNumber;
                    profileManager.DbContext.SubmitChanges();

                    company.LegalEntityProfile = original_legalEntityProfile;
                }
            }



            //
            //Method to insert a new company
            //
            company.CreatorUserId = userId;
            Insert(company);
            company.ReferenceCompanyId = company.CompanyId;
            company.MatrixId = company.CompanyId;
            if (matrixCompanyId != 0)
            {
                company.ReferenceCompanyId = GetCompany(matrixCompanyId).ReferenceCompanyId;
                company.MatrixId = matrixCompanyId;
            }
            DbContext.SubmitChanges();

            //
            //Method to create a Deposit
            //
            Deposit dep = CreateMatrixDeposit(company.CompanyId);

            //
            //method to create a new role to this company
            //
            Role role = CreateAdminRole(company.CompanyId);

            //
            //method to set all permissions writable to the admin
            //
            var pManager = new PlanManager(this);
            Plan plan = pManager.GetCurrentPlan(company.CompanyId);
            if (plan != null)
            {
                CreatePermissions(plan.PlanId, role.RoleId, company.CompanyId);
            }

            //
            //method to associate the user with the company
            //
            AssociateUser(company.CompanyId, userId, dep.DepositId, /*IsMain*/ true);

            //
            //method to associate the user with the role
            //
            AddUserInRoles(userId, company.CompanyId, role.RoleId);

            //
            // method to create a customer under the Host "Vivina", with the company data of the user
            //
            AddContactInHostCustomer(AddCompanyAsCustomer(company), userId);

            //
            // Configurate the company inserting template of the Best Pratices, Report, etc.
            //
            SetCompanyConfiguration(company);

            //
            // Configure home page
            //
            CreateHomePage(company, userId);

            //SetCompanyPaymentMethods(company);

            return InsertCompanyStatus.Success;
        }

        private void CreateHomePage(Company company, int userId)
        {
            var manager = new SiteManager(this);
            manager.Save(new WebPage
                             {
                                 CompanyId = company.CompanyId,
                                 Name = "Página Principal",
                                 IsPublished = true,
                                 PublishedDate = DateTime.Now,
                                 ModifiedDate = DateTime.Now,
                                 IsMainPage = true,
                                 UserId = userId
                             }, null);
        }

        private void ConfigureDNSRecords(Company company)
        {
            try
            {
                string zoneTemplate = "zone \"[website]\" IN {type master;file \"zones\\db.[website].txt\";allow-transfer {none;};};";

                File.AppendAllText("D:\\dns\\etc\\named.conf",
                                   zoneTemplate.Replace("[website]", company.LegalEntityProfile.Website),
                                   Encoding.UTF8);

                zoneTemplate = File.ReadAllText("D:\\dns\\etc\\zones\\template.txt", Encoding.UTF8);
                zoneTemplate = zoneTemplate.Replace("[website]", company.LegalEntityProfile.Website);

                File.WriteAllText("D:\\dns\\etc\\zones\\db." + company.LegalEntityProfile.Website + ".txt",
                                  zoneTemplate,
                                  Encoding.UTF8);

            }
            catch { }
        }

        /// <summary>
        /// this method add the administrator of new company as contact of host'customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        private void AddContactInHostCustomer(Int32 customerId, Int32 userId)
        {
            var contactManager = new ContactManager(this);
            var customerContactManager = new CustomerContactManager(this);
            var profileManager = new ProfileManager(this);
            Profile profile = profileManager.GetProfileByUser(userId);
            var contact = new Contact
                              {
                                  Name = profile.Name,
                                  Phone = profile.Phone,
                                  CellPhone = profile.CellPhone,
                                  Address = profile.Address,
                                  AddressComp = profile.AddressComp,
                                  AddressNumber = profile.AddressNumber,
                                  Email = profile.Email,
                                  PostalCode = profile.PostalCode
                              };

            contactManager.Insert(contact);

            var customerContact = new CustomerContact();
            customerContact.CompanyId = GetHostCompany().CompanyId;
            customerContact.ContactId = contact.ContactId;
            customerContact.CustomerId = customerId;
            customerContactManager.Insert(customerContact);
        }

        /// <summary>
        /// Add all payment methods to the company after configure
        /// </summary>
        /// <param name="company"></param>
        //private void SetCompanyPaymentMethods(Company company)
        //{
        //    PaymentMethodManager manager = new PaymentMethodManager(this);
        //    foreach (PaymentMethod paymentMethod in DbContext.PaymentMethods)
        //    {
        //        CompanyPaymentMethod companyPaymentMethod = new CompanyPaymentMethod();
        //        companyPaymentMethod.PaymentMethodId = paymentMethod.PaymentMethodId;
        //        companyPaymentMethod.CompanyId = company.CompanyId;
        //        manager.InsertCompanyPaymentMethod(companyPaymentMethod);
        //    }
        //}
        /// <summary>
        /// Configurate the company inserting template of the Best Pratices, Report, etc.
        /// </summary>
        /// <param name="company"></param>
        private void SetCompanyConfiguration(Company company)
        {
            //
            //Method to create a Company Configuration
            //
            InsertCompanyConfiguration(new CompanyConfiguration(), company);

            //
            // Accounting Plan
            //
            var accountManager = new AccountManager(this);
            accountManager.RegisterAccountingPlanTemplate(company.CompanyId);

            //
            // Cost Center
            //
            accountManager.RegisterCostCenterTemplate(company.CompanyId);
        }

        /// <summary>
        /// This method return a single company
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
#warning Esse m�todo est� fora dos padr�es de nomenclatura
        public LegalEntityProfile GetCompanyByUser(Int32 userId)
        {
            //InfoControlDataContext Context = DataManager.CreateContext<InfoControlDataContext>();
            IQueryable<LegalEntityProfile> query = from legalEntiyProfile in DbContext.LegalEntityProfiles
                                                   join company in DbContext.Companies on
                                                       legalEntiyProfile.LegalEntityProfileId equals
                                                       company.LegalEntityProfileId
                                                   join companyUser in DbContext.CompanyUsers on company.CompanyId
                                                       equals companyUser.CompanyId
                                                   where companyUser.UserId == userId
                                                   select legalEntiyProfile;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Return all companies by user, as IQueryable
        /// this method are ready for client sort and page
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetCompaniesByUser(Int32 userId, string sortExpression, int startRowIndex, int maximumRows)
        {
            var query = from company in GetAllCompanies()
                        join userCompanies in DbContext.CompanyUsers on company.CompanyId equals userCompanies.CompanyId
                        join legalEntityProfile in DbContext.LegalEntityProfiles on company.LegalEntityProfileId equals
                            legalEntityProfile.LegalEntityProfileId into gLegalEntityProfile
                        from legalEntityProfile in gLegalEntityProfile.DefaultIfEmpty()
                        where userCompanies.UserId == userId
                        select new
                                   {
                                       Name = legalEntityProfile.CompanyName ?? "",
                                       CNPJ = legalEntityProfile.CNPJ ?? "",
                                       Mail = legalEntityProfile.Email ?? "",
                                       Telephone = legalEntityProfile.Phone ?? "",
                                       company.CompanyId,
                                       company.Activities,
                                       company.Image,
                                       company.NextStatementDueDate,
                                       company.StartDate,
                                       company.ModifiedDate,
                                       company.PlanId,
                                       company.MatrixId,
                                       company.AddressNumber,
                                       company.ReferenceCompanyId,
                                       company.AddressComp,
                                       company.CompanyConfigurationId,
                                       company.LegalEntityProfile,
                                       company.LegalEntityProfileId,
                                       company.CreatorUserId
                                   };

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "CompanyId").AsQueryable();
        }

        /// <summary>
        /// This method returns quantity of company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int GetCompanyByUserCount(int userId)
        {
            return GetCompaniesByUser(userId).Count();
        }

        /// <summary>
        /// This method returns the user that registered the specified company
        /// </summary>
        /// <param name="companyId">can't be null</param>
        /// <returns></returns>
        public Profile GetResponsableForCompany(Int32 companyId)
        {
            var query = from companyUser in DbContext.CompanyUsers
                        join user in DbContext.Users on companyUser.UserId equals user.UserId
                        join profile in DbContext.Profiles on user.ProfileId equals profile.ProfileId
                        where companyUser.CompanyId == companyId && companyUser.IsMain == true
                        select profile;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// M�todo usado para pegar a Companhia que o usu�rio est� administrando no momento, diretamente 
        /// relacionado com a Combo de companhia, que fica no Header
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal Company GetCurrentAdminCompany(string userName)
        {
            IQueryable<Company> query = from company in DbContext.Companies
                                        join userCompanies in DbContext.CompanyUsers on company.CompanyId equals
                                            userCompanies.CompanyId
                                        join users in DbContext.Users on userCompanies.UserId equals users.UserId
                                        where (users.UserName == userName) && userCompanies.IsMain                                        
                                        select company;

            return query.FirstOrDefault();
        }

        private static Company _hostCompany;

        /// <summary>
        /// This method returns the Admin Company
        /// </summary>
        /// <returns></returns>
        public Company GetHostCompany()
        {
            if (_hostCompany != null)
                return _hostCompany;

            _hostCompany = (from company in DbContext.Companies
                                   where company.CompanyId == 1
                                   select company).FirstOrDefault();

            _hostCompany.LegalEntityProfile.GetHashCode();
            _hostCompany.LegalEntityProfile.Address.GetHashCode();
            _hostCompany.LegalEntityProfile.Address.City.GetHashCode();
            _hostCompany.LegalEntityProfile.Address.Neighborhood.GetHashCode();
            _hostCompany.Plan.GetHashCode();
            _hostCompany.User.GetHashCode();

            return _hostCompany;
        }

        /// <summary>
        /// M�todo usado para pegar o dep�sito atual, da companhia que o usu�rio est� administrando
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Deposit GetCurrentDeposit(Int32 userId, Int32 companyId)
        {
            Func<InfoControlDataContext, int, int, IQueryable<Deposit>> GetCurrentDepositCompiled =
                CompiledQuery.Compile<InfoControlDataContext, int, int, IQueryable<Deposit>>(
                    (_DbContext, _userId, _companyId) =>
                    from dep in _DbContext.Deposits
                    join userCompanies in _DbContext.CompanyUsers on dep.DepositId equals userCompanies.DepositId
                    where userCompanies.UserId == _userId && userCompanies.CompanyId == _companyId
                    select dep
                    );

            return GetCurrentDepositCompiled(DbContext, userId, companyId).FirstOrDefault();
        }

        /// <summary>
        /// This method returns all company that a user are in
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
#warning Esse método está fora dos padrões de nomenclatura -> GetCompaniesAsList
        public List<Company> GetCompaniesByUser(Int32 userId)
        {
            //InfoControlDataContext Context = DataManager.CreateContext<InfoControlDataContext>();
            IQueryable<Company> query = from company in DbContext.Companies
                                        join userCompanies in DbContext.CompanyUsers on company.CompanyId equals
                                            userCompanies.CompanyId
                                        where userCompanies.UserId == userId
                                        select company;
            return query.ToList();
        }

        /// <summary>
        /// retorna todas as empresas que um determinado usu�rio pode administrar
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList GetCompaniesNames(Int32 userId)
        {
            //InfoControlDataContext Context = DataManager.CreateContext<InfoControlDataContext>();

            var query = DbContext.Companies.AsQueryable();
            if (userId > 1)
                query = query.Where(c => c.CompanyUsers.Any(ci => ci.UserId == userId));

            return query.Select(c => new { c.CompanyId, c.LegalEntityProfile.CompanyName }).ToList();
        }

        /// <summary>
        /// M�todo usado para selecionar todas as companhias que tem um relacionamento hier�rquico entre elas
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="matrixId"></param>
        /// <returns></returns>
        public IList GetRelatedCompanies(int companyId, int matrixId)
        {
            //InfoControlDataContext Context = DataManager.CreateContext<InfoControlDataContext>();
            var sql =
                from comp in DbContext.Companies
                join legalEntity in DbContext.LegalEntityProfiles on comp.LegalEntityProfileId equals
                    legalEntity.LegalEntityProfileId
                where (comp.MatrixId == matrixId)
                orderby comp.CompanyId
                select new
                           {
                               comp.CompanyId,
                               Name = legalEntity.CompanyName
                           };
            return sql.ToList();
        }

        /// <summary>
        /// This method fully removes a company from system, for HOST use only !!!
        /// </summary>
        /// <param name="CompanyId"></param>
        public void DeleteCompany(Int32 companyId)
        {
            //change the roleadmin to Null
            Company company_Role = GetCompany(companyId);
            company_Role.RoleAdminId = null;
            DbContext.SubmitChanges();



            var queryRepresentantUsers = from representant in DbContext.Representants
                                         join representantUser in DbContext.RepresentantUsers
                                         on representant.RepresentantId equals representantUser.RepresentantId
                                         where representant.CompanyId == companyId
                                         select representantUser;

            DbContext.RepresentantUsers.DeleteAllOnSubmit(queryRepresentantUsers);



            //
            //this method returns all user of a selected company that are only users of this company
            //
            var query = from user in DbContext.Users
                        join companyUser in DbContext.CompanyUsers on user.UserId equals companyUser.UserId
                        where companyUser.CompanyId == companyId && user.CompanyUsers.Count() == 1
                        select new
                                   {
                                       user.UserId,
                                       user.ProfileId
                                   };

            DataTable dtUsers = query.ToDataTable();

            DbContext.SubmitChanges();

            // set the CreatorUserId to Null
            List<Company> companies;
            foreach (DataRow row in dtUsers.Rows)
            {
                int userId = Convert.ToInt32(row["UserId"]);
                companies = GetCompaniesByUser(Convert.ToInt32(row["UserId"]));
                foreach (Company company in companies)
                    company.CreatorUserId = null;
                DbContext.SubmitChanges();
            }

            DbContext.UsersInRoles.DeleteAllOnSubmit(DbContext.UsersInRoles.Where(x => x.CompanyId == companyId));
            DbContext.SubmitChanges();

            DbContext.ServiceOrders.DeleteAllOnSubmit(DbContext.ServiceOrders.Where(x => x.CompanyId == companyId));
            DbContext.SubmitChanges();

            DbContext.CustomerCalls.DeleteAllOnSubmit(DbContext.CustomerCalls.Where(x => x.CompanyId == companyId));
            DbContext.SubmitChanges();


            DbContext.Companies.DeleteAllOnSubmit(DbContext.Companies.Where(x => x.CompanyId == companyId || x.MatrixId == companyId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method changes the company that the user are administrating
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mainCompanyId"></param>
        public void ChangeCompany(int userId, int mainCompanyId)
        {
            //
            // Reset Admin in Companies
            //
            string query = String.Empty;
            query =
                @"
                UPDATE CompanyUser
                SET IsMain = 0
                WHERE UserId = @userId AND CompanyId <> @companyId";
            DataManager.Parameters.Add("@userId", userId);
            DataManager.Parameters.Add("@companyId", mainCompanyId);
            DataManager.ExecuteNonQuery(query);

            //
            // Set the current company as admin
            //
            query =
                @"
                UPDATE CompanyUser
                SET IsMain = 1
                WHERE UserId = @userId AND CompanyId = @companyId";
            DataManager.Parameters.Add("@userId", userId);
            DataManager.Parameters.Add("@companyId", mainCompanyId);
            int result = DataManager.ExecuteNonQuery(query);

            if (result == 0 && userId == 1)
            {
                query = @" SELECT TOP 1 depositId FROM Deposit WHERE CompanyId=@companyId";
                DataManager.Parameters.Add("@companyId", mainCompanyId);
                int depositId = Convert.ToInt32(DataManager.ExecuteDataRow(query)[0]);

                query =
                    @"
                INSERT CompanyUser (CompanyId, UserId, DepositId, IsMain) 
                VALUES (@companyId, 1, @depositId, 1)";
                DataManager.Parameters.Add("@companyId", mainCompanyId);
                DataManager.Parameters.Add("@depositId", depositId);
                DataManager.ExecuteNonQuery(query);
            }
        }

        /// <summary>
        /// Method to make a search in the company table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataTable SearchCompanies(Company entity)
        {
            string sql =
                "SELECT *,CompanyId,CAST (0 AS bit) as status FROM Company INNER JOIN LegalEntityProfile ON Company.LegalEntityProfileId=LegalEntityProfile.LegalEntityProfileId ";
            string where = "";
            if (entity != null)
            {
                if (!String.IsNullOrEmpty(entity.Activities))
                {
                    DataManager.Parameters.Add("@Activities", "%" + entity.Activities + "%");
                    where = where + " Activities like @Activities AND";
                }

                if (!String.IsNullOrEmpty(entity.LegalEntityProfile.CNPJ))
                {
                    DataManager.Parameters.Add("@CNPJ", "%" + entity.LegalEntityProfile.CNPJ + "%");
                    where = where + " CNPJ like @CNPJ AND";
                }

                if (!String.IsNullOrEmpty(entity.LegalEntityProfile.FantasyName))
                {
                    DataManager.Parameters.Add("@FantasyName", "%" + entity.LegalEntityProfile.FantasyName + "%");
                    where = where + " FantasyName like @FantasyName AND";
                }
                if (!String.IsNullOrEmpty(entity.LegalEntityProfile.IE))
                {
                    DataManager.Parameters.Add("@IE", "%" + entity.LegalEntityProfile.IE + "%");
                    where = where + " IE like @IE AND";
                }
                if (!String.IsNullOrEmpty(entity.LegalEntityProfile.Email))
                {
                    DataManager.Parameters.Add("@Mail", "%" + entity.LegalEntityProfile.Email + "%");
                    where = where + " Email like @Mail AND";
                }
                if (!String.IsNullOrEmpty(entity.LegalEntityProfile.CompanyName))
                {
                    DataManager.Parameters.Add("@Name", "%" + entity.LegalEntityProfile.CompanyName + "%");
                    where = where + " CompanyName like @Name AND";
                }
                if (!String.IsNullOrEmpty(entity.LegalEntityProfile.PostalCode))
                {
                    DataManager.Parameters.Add("@PostalCode", "%" + entity.LegalEntityProfile.PostalCode + "%");
                    where = where + " PostalCode like @PostalCode AND";
                }
                if (entity.NextStatementDueDate != DateTime.MinValue)
                {
                    DataManager.Parameters.Add("@NextDueDate", "%" + entity.NextStatementDueDate + "%");
                    where = where + " NextDueDate like @NextDueDate AND";
                }

                if (entity.StartDate != DateTime.MinValue)
                {
                    DataManager.Parameters.Add("@StartDate", "%" + entity.StartDate + "%");
                    where = where + " StartDate like @StartDate AND";
                }
                if (!String.IsNullOrEmpty(entity.LegalEntityProfile.Phone))
                {
                    DataManager.Parameters.Add("@Telephone", "%" + entity.LegalEntityProfile.Phone + "%");
                    where = where + " Telephone like @Telephone AND";
                }
                if (!String.IsNullOrEmpty(entity.LegalEntityProfile.Website))
                {
                    DataManager.Parameters.Add("@WebSite", "%" + entity.LegalEntityProfile.Website + "%");
                    where = where + " WebSite like @WebSite AND";
                }
                if (where != "")
                {
                    sql += " where" + where.Remove(where.Length - 3, 3);
                }
            }
            return DataManager.ExecuteDataTable(sql);
        }


        /// <summary>
        /// This method returns the quantity of users by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Int32 GetCompanyUsersCount(Int32 companyId)
        {
            //
            // Na cláusula "where" a condição userId != 1, significa que a contagem não deve incluir o primeiro 
            // usuário que foi cadastrado no sistema, que no caso é "Sidney Lima Filho".             
            //
            return DbContext.CompanyUsers.Where(companyUser => companyUser.CompanyId == companyId && companyUser.UserId != 1).Count();
        }


        /// <summary>
        /// This method returns a user by the cpf of profile related with it
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public DataClasses.User GetUser(string cpf)
        {
            var query = (from user in DbContext.Users
                         join profile in DbContext.Profiles on user.ProfileId equals profile.ProfileId
                         where profile.CPF.Equals(cpf)
                         select user).FirstOrDefault();
            return query;
        }


        /// <summary>
        /// This method returns the 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable GetUsers(Int32 companyId)
        {

            var query = from user in DbContext.Users
                        join companyUser in DbContext.CompanyUsers on user.UserId equals companyUser.UserId
                        where companyUser.CompanyId == companyId
                        select new
                        {
                            Username = user.Profile.Name,
                            user.UserId,
                            user.Email,
                            user.ProfileId
                        };

            return query;
        }

        [Obsolete("Utilizar o método na versão linq abaixo")]
        /// <summary>
        /// this method search the companies using a hashTable object
        /// </summary>
        /// <param name="htCompany"></param>
        /// <returns></returns>
        public DataTable SearchCompanies(Hashtable htCompany)
        {
            //create a new StringBuilder
            var sbSql = new StringBuilder();

            //add data in the string
            sbSql.Append(@"
SELECT 
    Company.CompanyId, 
    LegalEntityProfile.FantasyName,
     LegalEntityProfile.CNPJ,
    LegalEntityProfile.CompanyName,
    LegalEntityProfile.IE,
    LegalEntityProfile.Phone, 
    LegalEntityProfile.Email, 
    LegalEntityProfile.WebSite, 
    Company.StartDate,    
    (	select MAX(Users.LastActivityDate) 
		FROM Users INNER JOIN 
			 CompanyUser ON Users.UserId = CompanyUser.UserId
		WHERE Users.UserId<>1 AND 
			  CompanyUser.CompanyId = Company.CompanyId) AS LastActivityDate,
	(	select COUNT(*) 
		FROM CompanyUser
		WHERE CompanyUser.UserId<>1 AND 
			  CompanyUser.CompanyId = Company.CompanyId) AS NumberUsers			  
FROM 
    Company INNER JOIN 
    LegalEntityProfile ON Company.LegalEntityProfileId = LegalEntityProfile.LegalEntityProfileId INNER JOIN 
    Plans ON Company.PlanId = Plans.PlanId
WHERE 1=1 ");

            var sbWhere = new StringBuilder();
            if (!String.IsNullOrEmpty(htCompany["CNPJ"].ToString()))
            {
                DataManager.Parameters.Add("@CNPJ", "%" + htCompany["CNPJ"] + "%");
                sbWhere.Append("AND (LegalEntityProfile.CNPJ like @CNPJ)");
            }
            if (!String.IsNullOrEmpty(htCompany["FantasyName"].ToString()))
            {
                DataManager.Parameters.Add("@FantasyName", "%" + htCompany["FantasyName"] + "%");
                sbWhere.Append("AND (LegalEntityProfile.FantasyName like @FantasyName)");
            }
            if (!String.IsNullOrEmpty(htCompany["IE"].ToString()))
            {
                DataManager.Parameters.Add("@IE", "%" + htCompany["IE"] + "%");
                sbWhere.Append("AND (LegalEntityProfile.IE like @IE) ");
            }
            if (!String.IsNullOrEmpty(htCompany["Email"].ToString()))
            {
                DataManager.Parameters.Add("@Email", "%" + htCompany["Email"] + "%");
                sbWhere.Append("AND (LegalEntityProfile.Email like @Email) ");
            }
            if (!String.IsNullOrEmpty(htCompany["CompanyName"].ToString()))
            {
                DataManager.Parameters.Add("@CompanyName", "%" + htCompany["CompanyName"] + "%");
                sbWhere.Append("AND (LegalEntityProfile.CompanyName like @CompanyName) ");
            }
            if (htCompany["StartDate"] != null)
            {
                DataManager.Parameters.Add("@StartDate", Convert.ToDateTime(htCompany["StartDate"]));
                sbWhere.Append("AND (Company.StartDate >= @StartDate) ");
            }
            if (!String.IsNullOrEmpty(htCompany["Phone"].ToString()))
            {
                DataManager.Parameters.Add("@Phone", "%" + htCompany["Phone"] + "%");
                sbWhere.Append("AND (LegalEntityProfile.Phone like @Phone) ");
            }
            if (!String.IsNullOrEmpty(htCompany["Website"].ToString()))
            {
                DataManager.Parameters.Add("@Website", "%" + htCompany["Website"] + "%");
                sbWhere.Append("AND (LegalEntityProfile.Website like @Website) ");
            }

            sbSql.Append(sbWhere);
            sbSql.Append(
                @" GROUP BY Company.CompanyId, LegalEntityProfile.FantasyName, LegalEntityProfile.CNPJ, LegalEntityProfile.CompanyName, LegalEntityProfile.IE, LegalEntityProfile.Phone, LegalEntityProfile.Email, LegalEntityProfile.WebSite, Company.StartDate");

            if (!String.IsNullOrEmpty(htCompany["sortExpression"].ToString()))
                sbSql.Append(" ORDER BY " + htCompany["sortExpression"].ToString() + " " + htCompany["sortDirection"]);

            return DataManager.ExecuteDataTable(sbSql.ToString());
        }

        /// <summary>
        /// This method searchs companies by their cnpj, name and email
        /// </summary>
        /// <param name="cnpj"></param>
        /// <param name="companyName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public IQueryable SearchCompanies(string cnpj, string companyName, string email)
        {
            var query = from company in DbContext.Companies
                        join legalEntityProfile in DbContext.LegalEntityProfiles on company.LegalEntityProfileId equals legalEntityProfile.LegalEntityProfileId
                        join plan in DbContext.Plans on company.PlanId equals plan.PlanId
                        select new
                        {
                            company.CompanyId,
                            legalEntityProfile.FantasyName,
                            legalEntityProfile.CNPJ,
                            legalEntityProfile.CompanyName,
                            legalEntityProfile.IE,
                            legalEntityProfile.Phone,
                            legalEntityProfile.Email,
                            legalEntityProfile.Website,
                            company.StartDate,
                            LastActivityDate = company.CompanyUsers.Max(x => x.User.LastActivityDate),
                            NumberUsers = company.CompanyUsers.Where(x => x.UserId != 1).Count()
                        };

            if (!String.IsNullOrEmpty(cnpj))
                query = query.Where(x => x.CNPJ == cnpj);

            if (!String.IsNullOrEmpty(companyName))
                query = query.Where(x => x.CompanyName == companyName);

            if (!String.IsNullOrEmpty(email))
                query = query.Where(x => x.Email == email);

            return query;
        }

        /// <summary>
        /// This method returns a company in accordance with your profile.
        /// </summary>
        /// <param name="legalEntityProfileId"></param>
        /// <returns></returns>
        public Company GetCompanyByProfile(int legalEntityProfileId)
        {
            return DbContext.Companies.Where(x => x.LegalEntityProfileId == legalEntityProfileId).FirstOrDefault();
        }

        /// <summary>
        /// this method return true if the CompanyExists
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public bool checkCompanyExists(string cnpj)
        {
            var profileManager = new ProfileManager(this);
            LegalEntityProfile legalEntityProfile;
            legalEntityProfile = profileManager.GetLegalEntityProfile(cnpj);
            if (legalEntityProfile != null && GetCompanyByProfile(legalEntityProfile.LegalEntityProfileId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// This method inserts a new user
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="user"></param>
        /// <param name="profile"></param>
        /// <param name="depositId"></param>
        /// <returns></returns>
        [Obsolete("This method is out-of-date")]
        public InsertCompanyStatus InsertUser(int companyId, User user, Profile profile, int? depositId)
        {
            //
            // If ProfileId equal 0 then the profile no exists in bd
            //
            var pManager = new ProfileManager(this);
            if (profile.ProfileId == 0)
                pManager.Insert(profile);

            //
            //method to insert a new user
            //
            var provider = Membership.Provider as VivinaMembershipProvider;
            MembershipCreateStatus status;
            var membershipManager = new MembershipManager(this);
            user.ProfileId = profile.ProfileId;
            membershipManager.Insert(user, out status, provider.RequiresValidEmail);
            //
            //verify if the status of the inclusion is ok
            //
            if (status != MembershipCreateStatus.Success)
            {
                DataManager.Rollback();
                switch (status)
                {
                    case MembershipCreateStatus.InvalidPassword:
                        return InsertCompanyStatus.InvalidPassword;
                    case MembershipCreateStatus.DuplicateEmail:
                        return InsertCompanyStatus.DuplicatedAdminEmail;
                }
            }

            //
            // Associate the User with company
            //
            AssociateUser(companyId, user.UserId, depositId, true);

            //
            // Insert a Employee
            //
            var humanResourcesManager = new HumanResourcesManager(this);
            var employee = new Employee();
            employee.IsActive = true;
            employee.ProfileId = profile.ProfileId;
            employee.CompanyId = companyId;
            humanResourcesManager.InsertEmployee(employee, new List<EmployeeAdditionalInformation>());

            return InsertCompanyStatus.Success;
        }

        /// <summary>
        /// This method verifies if exist the userName specified in db 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ExistsUserName(String userName)
        {
            return DbContext.Users.Where(user => user.UserName.Equals(userName.ToLower())).FirstOrDefault() != null;
        }

        /// <summary>
        /// This method verifies if exist user related a specified company 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ExistsUserInCompany(Int32 companyId, string userName)
        {
            return DbContext.CompanyUsers.Where(companyUser => companyUser.CompanyId == companyId && companyUser.User.UserName == userName).FirstOrDefault() != null;
        }


        /// <summary>
        /// This method inserts a user
        /// </summary>
        /// <param name="companyId">Can't be null</param>
        /// <param name="user">Can't be null. This entity can't be attached in db</param>
        /// <param name="depositId">can be null</param>
        /// <returns>a status based on InsertCompanyStatus</returns>
        public InsertCompanyStatus InsertUser(int companyId, int? depositId, int? representantId, User user, Profile profile)
        {
            var membershipManager = new MembershipManager(this);

            //
            //method to insert a new user
            //

            var provider = Membership.Provider as VivinaMembershipProvider;
            MembershipCreateStatus status;

            membershipManager.Insert(user, out status, provider.RequiresValidEmail);

            //
            //verifies if the status of the inclusion is ok
            //

            if (status != MembershipCreateStatus.Success)
            {
                DataManager.Rollback();
                switch (status)
                {
                    case MembershipCreateStatus.InvalidPassword:
                        return InsertCompanyStatus.InvalidPassword;
                    case MembershipCreateStatus.DuplicateEmail:
                        return InsertCompanyStatus.DuplicatedAdminEmail;
                }
            }

            if (profile.ProfileId == 0)
                new ProfileManager(this).Insert(profile);

            AttachProfileEntityToUser(companyId, user.UserId, profile.ProfileId);

            if (representantId.HasValue)
                AddRepresentantToUser(companyId, user.UserId, Convert.ToInt32(representantId));

            //
            // Associate the User with company
            //
            if (!IsCompanyUser(companyId, user.UserId))
                AssociateUser(companyId, user.UserId, depositId, true);

            return InsertCompanyStatus.Success;
        }

        #endregion

        #region Statements

        /// <summary>
        /// Calculate and generate the statements from a period
        /// </summary>
        /// <param name="company"></param>
        /// <param name="beginPeriod"></param>
        /// <param name="endPeriod"></param>
        internal void GenerateStatement(Company company)
        {
            var statement = new Statement()
            {
                CompanyId = company.CompanyId,
                PeriodBegin = company.NextStatementDueDate.Date.AddMonths(-1),
                PeriodEnd = company.NextStatementDueDate.Date.AddSeconds(-1),
                Name = "Relatório de Utilização do InfoControl (" + company.NextStatementDueDate.AddMonths(-1).ToString("MM/yyyy") + ")"
            };

            ProcessStatementItemActiveUsers(company, statement);

            ProcessStatementItemSite(company, statement);

            Save(company, statement);
        }

        internal void ProcessStatementItemActiveUsers(Company company, Statement statement)
        {
            var item = new StatementItem();
            item.Name = "Usuários";
            item.UnitCost = company.Plan.Package.Price;
            item.Quantity = UserActivesInMonth(statement.CompanyId, statement.PeriodBegin, statement.PeriodEnd);
            item.Value = company.Plan.Package.Price * item.Quantity;

            statement.StatementItems.Add(item);
        }

        internal void ProcessStatementItemActiveProducts(Company company, Statement statement)
        {
            var item = new StatementItem();
            item.Name = "Produtos";
            item.Quantity = company.Products.Count();
            item.Value = 0.08m * item.Quantity;
            statement.StatementItems.Add(item);

            item = new StatementItem();
            item.Name = "Imagens de Produtos";
            item.Quantity = company.Products.Sum(p => p.ProductImages.Count());
            item.Value = 0.04m * item.Quantity;
            statement.StatementItems.Add(item);
        }

        internal void ProcessStatementItemSite(Company company, Statement statement)
        {
            if (!String.IsNullOrEmpty(company.LegalEntityProfile.Website))
            {
                statement.StatementItems.Add(new StatementItem()
                {
                    Name = "Manutenção do Site",
                    Quantity = 1,
                    Value = 50.00m
                });

                ProcessStatementItemActiveProducts(company, statement);
            }
        }

        private Invoice ConvertStatementToInvoice(Company company, Statement statement)
        {
            var customerManager = new CustomerManager(this);
            return new Invoice()
            {
                CompanyId = GetHostCompany().CompanyId,
                CustomerId = customerManager.GetCustomerByLegalEntityProfile(GetHostCompany().CompanyId, company.LegalEntityProfileId).CustomerId,
                EntryDate = DateTime.Now,
                Description = "Manutenção InfoControl"
            };
        }

        /// <summary>
        /// Save the entity
        /// </summary>
        /// <param name="entity"></param>
        public void Save(Company company, Statement statement)
        {
            //
            // Create invoice for the related customer
            //
            statement.StatementTotal = statement.StatementItems.Sum(x => x.Value);
            var list = new List<Parcel>();
            list.Add(new Parcel()
            {
                DueDate = statement.PeriodEnd.AddDays(15),
                Amount = statement.StatementTotal
            });

            var financialManager = new FinancialManager(this);
            var invoice = ConvertStatementToInvoice(company, statement);
            financialManager.Insert(invoice, list);

            //
            // Update data
            //
            statement.Invoice = invoice;
            company.NextStatementDueDate = company.NextStatementDueDate.AddMonths(1);

            //
            // Save statement
            //
            if (statement.StatementId > 0)
            {
                var original = GetStatement(statement.StatementId);
                original.CopyPropertiesFrom(statement);
            }
            else
                DbContext.Statements.InsertOnSubmit(statement);


            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Get all statements by a company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        internal IQueryable<Statement> GetStatements(int companyId)
        {
            return from si in DbContext.Statements
                   where si.CompanyId == companyId
                   orderby si.PeriodBegin descending
                   select si;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Statement GetStatement(int statementId)
        {
            return DbContext.Statements.Where(x => x.StatementId == statementId).FirstOrDefault();
        }


        /// <summary>
        /// This method retrieves statements by a specific company
        /// This method is typical for listControls
        /// </summary>
        /// <param name="companyId">can't be null</param>
        /// <param name="sortExpression">supplied by listControl</param>
        /// <param name="startRowIndex">supplied by listControl</param>
        /// <param name="maximumRows">supplied by listControl</param>
        /// <returns></returns>
        public IQueryable GetStatements(Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        {

            var query = from statement in GetStatements(companyId)
                        join company in DbContext.Companies on statement.HostCompanyId equals company.CompanyId
                        where statement.PeriodBegin >= DateTime.Now.AddMonths(-12)
                        select new
                        {
                            CustomerName = company.LegalEntityProfile.CompanyName,
                            statement.StatementId,
                            statement.BoletusNumber,
                            statement.StatementTotal,
                            statement.PeriodBegin,
                            statement.PeriodEnd,
                            statement.Name
                        };

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "StatementId");
        }

        /// <summary>
        /// This method just returns the number of registers retrivied of GetStatements method
        /// </summary>
        /// <param name="companyId">can't be null</param>
        /// <param name="sortExpression">supplied by listControl</param>
        /// <param name="startRowIndex">supplied by listControl</param>
        /// <param name="maximumRows">supplied by listControl</param>
        /// <returns></returns>
        public Int32 GetStatementsCount(Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetStatements(companyId, sortExpression, startRowIndex, maximumRows).Cast<object>().Count();
        }

        #endregion

        #region StatementItems
        /// <summary>
        /// Get statementitems by Statement
        /// </summary>
        /// <param name="statementId"></param>
        /// <returns></returns>
        public IQueryable<StatementItem> GetStatementItems(int statementId)
        {
            return from si in DbContext.StatementItems
                   where si.StatementId == statementId
                   select si;
        }
        #endregion





        #region Create Company Functions

        /// <summary>
        /// Method to change a user deposit
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="depositId"></param>
        public void ChangeDeposit(int companyId, int userId, int? depositId)
        {
            string query = String.Empty;
            query =
                @"
                UPDATE CompanyUser
                SET DepositId = @depositId
                WHERE UserId = @userId AND CompanyId = @companyId ";

            DataManager.Parameters.Add("@userId", userId);
            DataManager.Parameters.Add("@depositId", depositId.HasValue
                                                         ? depositId.Value
                                                         : (object)DBNull.Value);
            DataManager.Parameters.Add("@companyId", companyId);
            DataManager.ExecuteNonQuery(query);
        }

        /// <summary>
        /// Method to add a new deposit
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        private Deposit CreateMatrixDeposit(int companyId)
        {
            var dep = new Deposit();
            dep.CompanyId = companyId;
            dep.Name = "Matriz";
            new DepositManager(this).Insert(dep);
            return dep;
        }

        /// <summary>
        /// Method to create a new role of "ADMIN" type
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        private Role CreateAdminRole(int companyId)
        {
            var rolesManager = new RolesManager(this);
            var role = new Role();

            role.ApplicationId = Application.Current.ApplicationId;
            role.Name = "Admin";
            role.Description = "Administrador";
            role.LastUpdatedDate = DateTime.Now;
            role.CompanyId = companyId;
            rolesManager.Insert(role);

            //
            // Update the Role Admin in Company
            //
            Company company = GetCompany(companyId);
            company.RoleAdminId = role.RoleId;
            DbContext.SubmitChanges();

            return role;
        }

        /// <summary>
        /// Method to add a USER in a determined ROLE
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <param name="roleId"></param>
        private void AddUserInRoles(int userId, int companyId, int roleId)
        {
            var newUserInRoles = new UsersInRole();
            var userInRolesManager = new UsersInRolesManager(this);

            newUserInRoles.UserId = userId;
            newUserInRoles.RoleId = roleId;
            newUserInRoles.CompanyId = companyId;
            userInRolesManager.Insert(newUserInRoles);
        }

        /// <summary>
        /// Method to add a new COMPANY in a CUSTOMER of the HOST COMPANY
        /// </summary>
        /// <param name="newCompany"></param>
        private Int32 AddCompanyAsCustomer(Company newCompany)
        {
            var customerManager = new CustomerManager(this);

            var customer = new Customer();
            customer.CompanyId = GetHostCompany().CompanyId;
            customer.LegalEntityProfileId = newCompany.LegalEntityProfileId;
            if (customerManager.ExistCustomer(customer))
                customer = customerManager.GetCustomerByLegalEntityProfile(customer.CompanyId,
                                                                           Convert.ToInt32(customer.LegalEntityProfileId));
            else
                customerManager.Insert(customer);
            return customer.CustomerId;
        }

        /// <summary>
        /// Method to create a permission
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="roleId"></param>
        /// <param name="companyId"></param>
        private void CreatePermissions(int planId, int roleId, int companyId)
        {
            var permissionManager = new PermissionManager(this);
            var functionManager = new FunctionManager(this);
            IList<Function> functions = functionManager.GetFunctionsByPlan(planId);

            foreach (Function function in functions)
            {
                //Below the line was changed to give the new object Permission
                //to method permissionManager.Insert an object each time it is called
                var newPermission = new Permission();
                newPermission.FunctionId = function.FunctionId;
                newPermission.PermissionTypeId = (int)AccessControlActions.Change;
                newPermission.RoleId = roleId;
                newPermission.CompanyId = companyId;
                permissionManager.Insert(newPermission);
            }
        }

        #endregion

        #region Users


        public void UpdateUser(Int32 companyId, User user, Profile originalProfile, Profile profile)
        {

            // - atualizar o user
            // - atualizar o perfil
            // - associar o perfil ao usuário 
            // - associá-lo a uma compania




        }


        /// <summary>
        /// This method removes a representant from user
        /// </summary>
        /// <param name="userId"></param>
        public void RemoveRepresentantFromUser(Int32 userId)
        {
            var representantManager = new RepresentantManager(this);

            if (representantManager.GetRepresentantUser(userId) != null)
                representantManager.DeleteRepresentantUser(userId);
        }

        /// <summary>
        /// This method adds a representant to user
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="representantId"></param>
        public void AddRepresentantToUser(Int32 companyId, Int32 userId, Int32 representantId)
        {
            var representantManager = new RepresentantManager(this);
            var representantUser = new RepresentantUser
            {
                CompanyId = companyId,
                UserId = userId,
                RepresentantId = representantId
            };

            representantManager.InsertRepresentantUser(representantUser);
        }

        /// <summary>
        /// This method attachs a profile in specified user
        /// </summary>
        /// <param name="user">a connected user entity</param>
        /// <param name="profileId">a profileId to attach </param>
        public void AttachProfileEntityToUser(Int32 companyId, Int32 userId, Int32 profileId)
        {
            var user = GetUser(companyId, userId);
            user.Profile = new ProfileManager(this).GetProfile(profileId);
            DbContext.SubmitChanges();
        }

        private int UserActivesInMonth(int companyId, DateTime beginDate, DateTime endDate)
        {
            var query = from users in GetActiveUsers(companyId)
                        where (from activity in users.UserActivityLogs
                               where activity.LoginDate > beginDate && activity.LoginDate < endDate
                               select activity).Any()
                        select users;

            return query.Count();
        }

        /// <summary>
        /// Return the number of users in a plan
        /// </summary>
        /// <param name="referenceCompanyId"></param>
        /// <returns></returns>
        public int GetUsersCountByPlan(int referenceCompanyId)
        {
            //InfoControlDataContext Context = DataManager.CreateContext<InfoControlDataContext>();
            IQueryable<int> sql =
                from cUsers in DbContext.CompanyUsers
                join comp in DbContext.Companies on cUsers.CompanyId equals comp.CompanyId
                where comp.ReferenceCompanyId == referenceCompanyId
                select cUsers.UserId;
            return sql.Count();
        }

        /// <summary>
        /// This method all users of a company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public List<DataClasses.User> GetUserByCompanyAsList(Int32 companyId, string sortExpression, int startRowIndex,
                                                             int maximumRows)
        {
            //InfoControlDataDbContext Context = DataManager.CreateContext<InfoControlDataContext>();
            IQueryable<DataClasses.User> query =
                from companyUsers in DbContext.CompanyUsers
                join user in DbContext.Users on companyUsers.UserId equals user.UserId
                join company in DbContext.Companies on companyUsers.CompanyId equals company.CompanyId
                where company.CompanyId == companyId && companyUsers.UserId != 1
                select user;
            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "UserId").ToList();
        }

        /// <summary>
        /// Get all users that are online
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<DataClasses.User> GetUsersOnline(Int32 companyId)
        {
            IQueryable<DataClasses.User> query =
                from user in GetActiveUsers(companyId)
                where
                    (user.IsOnline ||
                     user.LastActivityDate > DateTime.Now.AddMinutes(Membership.UserIsOnlineTimeWindow * -1))
                select user;
            return query;
        }

        /// <summary>
        /// This method associates the user with the company in the Company Users
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="depositId"></param>
        /// <param name="isMain"></param>
        public void AssociateUser(int companyId, int userId, int? depositId, bool isMain)
        {
            //InfoControlDataContext Context = DataManager.CreateContext<InfoControlDataContext>();
            DbContext.CompanyUsers.InsertOnSubmit(
                new CompanyUser
                    {
                        CompanyId = companyId,
                        UserId = userId,
                        DepositId = depositId,
                        IsMain = isMain
                    });
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method delete a user
        /// </summary>
        /// <param name="newUser"></param>
        public void DeleteUser(User newUser)
        {
            var membershipManager = new MembershipManager(this);
            newUser = membershipManager.GetUser(newUser.UserId);
            membershipManager.Delete(newUser);
        }

        /// <summary>
        /// This method delete an companyUser
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        public void DeleteCompanyUser(Int32 companyId, Int32 userId)
        {
            DbContext.CompanyUsers.DeleteOnSubmit(GetCompanyUser(companyId, userId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method remove the relationship of user and company
        /// </summary>
        /// <param name="newUser"></param>
        public void DisassociateUser(Int32 userId, Int32 companyId)
        {
            CompanyUser companyUser = GetCompanyUser(companyId, userId);
            DbContext.UsersInRoles.DeleteAllOnSubmit(companyUser.User.UsersInRoles);
            DbContext.SubmitChanges();

            DbContext.CompanyUsers.DeleteOnSubmit(companyUser);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method returns the companyUser
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsCompanyUser(Int32 companyId, Int32 userId)
        {
            return GetCompanyUser(companyId, userId) != null;
        }

        /// <summary>
        /// This method returns the company user of deal with userName of user
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public CompanyUser GetCompanyUser(Int32 companyId, String userName)
        {
            return
                DbContext.CompanyUsers.Where(u => u.CompanyId == companyId && u.User.UserName.Equals(userName)).
                    FirstOrDefault();
        }

        /// <summary>
        /// This method returns the company user of deal with userName of user
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public CompanyUser GetCompanyUser(Int32 companyId, Int32 userId)
        {
            return DbContext.CompanyUsers.Where(u => u.CompanyId == companyId && u.UserId == userId).FirstOrDefault();
        }

        /// <summary>
        /// This method updates a user, profile and deposit
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <param name="depositId"></param>
        /// <param name="profile"></param>
        /// <param name="user"></param>
        public void UpdateUser(Int32 companyId, Int32 userId, Int32? depositId, Int32? representantId, Profile profile, User user)
        {
            //
            // Save Profile
            //
            var profileManager = new ProfileManager(this);

            profileManager.SaveProfile(profile);

            if (representantId.HasValue)
            {
                RemoveRepresentantFromUser(userId);
                AddRepresentantToUser(companyId, userId, (int)representantId);
            }
            else
                RemoveRepresentantFromUser(userId);

            AttachProfileEntityToUser(companyId, userId, profile.ProfileId);
            UpdateUser(userId, companyId, depositId, user);
        }

        /// <summary>
        /// This method updates an user
        /// </summary>
        /// <param name="userId">Can't be null</param>
        /// <param name="companyId">Can't be null</param>
        /// <param name="depositId"></param>
        /// <param name="user">Can't be null</param>
        public void UpdateUser(int userId, int companyId, int? depositId, User user)
        {
            //
            // Save User
            //
            var mManager = new MembershipManager(this);

            User original_user = mManager.GetUser(userId);

            mManager.Update(original_user, user);
            //
            // Deposit
            //
            ChangeDeposit(companyId, userId, depositId);

            //
            // Associate the User with company
            //
            if (GetCompanyUser(companyId, userId) == null)
                AssociateUser(companyId, userId, depositId, true);
        }

        /// <summary>
        /// this method returns a user by profile
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public DataClasses.User GetUserByProfile(Int32 profileId)
        {
            return DbContext.Users.Where(x => x.ProfileId == profileId).FirstOrDefault();
        }

        public DataClasses.User GetUserByUserName(String userName)
        {
            return DbContext.Users.Where(user => user.UserName == userName).FirstOrDefault();
        }

        /// <summary>
        /// this method return a User by employeeId
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public DataClasses.User GetUserByEmployee(Int32 employeeId)
        {
            IQueryable<DataClasses.User> query = from user in DbContext.Users
                                                 join profile in DbContext.Profiles on user.ProfileId equals
                                                     profile.ProfileId
                                                 join employee in DbContext.Employees on profile.ProfileId equals
                                                     employee.ProfileId
                                                 where employee.EmployeeId == employeeId
                                                 select user;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// this method returns a user by UserId
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataClasses.User GetUser(Int32 companyId, Int32 userId)
        {
            return DbContext.Users.Where(user => user.UserId == userId).FirstOrDefault();
        }

        /// <summary>
        /// this method returns users by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<DataClasses.User> GetUsersByCompany(Int32 companyId)
        {
            IQueryable<DataClasses.User> users = from user in DbContext.Users
                                                 join companyUser in DbContext.CompanyUsers on user.UserId equals
                                                     companyUser.UserId
                                                 where companyUser.CompanyId == companyId
                                                 select user;
            return users;
        }

        /// <summary>
        /// this method returns Active users by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<DataClasses.User> GetActiveUsers(Int32 companyId)
        {
            IQueryable<DataClasses.User> query = from companyUsers in DbContext.CompanyUsers
                                                 join user in DbContext.Users on companyUsers.UserId equals user.UserId
                                                 where
                                                     companyUsers.CompanyId == companyId &&
                                                     (companyUsers.CompanyId == 1 || user.UserId != 1) &&
                                                     user.IsActive
                                                 select user;
            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable GetTechnicalUserAsDataTable(Int32 companyId)
        {
            var employeeManager = new HumanResourcesManager(this);
            var query = from employee in employeeManager.GetTechnicalEmployee(companyId)
                        join profile in DbContext.Profiles on employee.ProfileId equals profile.ProfileId
                        join user in DbContext.Users on profile.ProfileId equals user.ProfileId
                        select new
                                   {
                                       profile.Name,
                                       user.UserId
                                   };
            return query.ToDataTable();
        }

        /// <summary>
        /// this is the SearchUser,this method is used at Select's control
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="name"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<Recognizable> SearchUser(Int32 companyId, string name, Int32 maximumRows)
        {
            string methodName = MethodBase.GetCurrentMethod().ToString();

            var query = (from user in GetUsersByCompany(companyId)
                         where user.IsActive && !user.IsLockedOut && user.Profile.Name.Contains(name)
                         select new Recognizable(user.UserId.ToString(), user.Profile.CPF + " | " + user.Profile.Name)).Take(maximumRows);

            return query;

        }

        #endregion

        #region Boletu

        /// <summary>
        /// This method returns all packages
        /// </summary>
        /// <param name="referenceCompanyId"></param>
        /// <returns></returns>
        public IQueryable<Package> GetPackages(int referenceCompanyId)
        {
            IQueryable<Package> queryPackage = from comp in DbContext.Companies
                                               join plan in DbContext.Plans on comp.PlanId equals plan.PlanId
                                               where comp.CompanyId == referenceCompanyId
                                               select plan.Package;

            IQueryable<Package> queryAdditional = from addPack in DbContext.PackageAdditionals
                                                  join pack in DbContext.Packages on addPack.PackageId equals
                                                      pack.PackageId
                                                  where addPack.CompanyId == referenceCompanyId
                                                  select pack;

            return queryPackage.Union(queryAdditional);
        }

        #endregion

        #region others

        /// <summary>
        /// this function return all cnae
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllCnae()
        {
            return DbContext.Cnaes.ToDataTable();
        }

        /// <summary>
        /// this method return all JudicialNature
        /// </summary>
        public DataTable GetAllJudicialNature()
        {
            return DbContext.JudicialNatures.ToDataTable();
        }

        /// <summary>
        /// this method return all ProfitAssessment
        /// </summary>
        public DataTable GetAllProfitAssessment()
        {
            return DbContext.ProfitAssessments.ToDataTable();
        }

        ///// <summary>
        ///// this method retuen all unitPriceNames by company
        ///// </summary>
        ///// <param name="companyConfigurationId"></param>
        ///// <returns></returns>
        //public DataTable GetUnitPriceNames(Int32 companyConfigurationId)
        //{ 

        //}

        #endregion

        #region WebSite

        /// <summary>
        /// this method return a company by its website
        /// </summary>
        /// <param name="webSite"></param>
        /// <returns></returns>
        public Company GetCompanyByWebSite(String webSite)
        {
            webSite = webSite.RemoveDpnInUrl();
            return DbContext.Companies.Where(c => c.LegalEntityProfile.Website.Contains(webSite)).FirstOrDefault();
        }

        #endregion
    }

    //public static class CompanyExtensions
    //{

    //}

    /// <summary>
    /// The typed Results of an Insert or Update Action
    /// </summary>    
    public enum InsertCompanyStatus
    {
        DuplicateCNPJ,
        DuplicatedUserName,
        InvalidUser,
        DuplicatedAdminEmail,
        Success,
        InvalidPassword
    }
}
