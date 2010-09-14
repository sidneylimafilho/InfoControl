using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfoControl.Data;
using Vivina.Erp.DataClasses;
using System.Data.OleDb;
using System.IO;

namespace Vivina.Erp.BusinessRules
{
    public class CustomerImporter : BusinessManager<InfoControlDataContext>
    {
        public CustomerImporter(IDataAccessor container)
            : base(container)
        {
        }

        private CustomerManager customerManager;
        private ContactManager contactManager;

        private CustomerManager CustomerManager
        {
            get
            {
                if (customerManager == null)
                    customerManager = new CustomerManager(this);

                return customerManager;
            }
        }

        private ContactManager ContactManager
        {
            get
            {
                if (contactManager == null)
                    contactManager = new ContactManager(this);

                return contactManager;
            }
        }

        /// <summary>
        /// This dictionary stores the relevant columms for db and their status(true or false),
        /// dependent if exist into excel file or no
        /// </summary>
        Dictionary<String, bool> listExistentColumms = new Dictionary<String, bool>();

        List<String> listCPF = new List<String>();
        List<String> listCNPJ = new List<String>();

        int errors = 0;
        int totalRegisters = 0;

        /// <summary>
        /// This method retrieves customers data from a excel file and stores in db
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        public void ImportDataFromExcelFile(Int32 companyId, Int32 userId, string fileName, out string message)
        {

            ProfileManager profileManager;

            profileManager = new ProfileManager(this);

            using (var excelConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + fileName + @"; Extended Properties=Excel 5.0"))
            {

                var excelCommand = new OleDbCommand("Select * from [CLIENTES$]", excelConnection);

                excelConnection.Open();
                OleDbDataReader excelReader;
                try
                {
                    excelReader = excelCommand.ExecuteReader();
                }
                catch (OleDbException)
                {
                    message = "Nome da planilha incorreto! Certifique-se que o nome da planilha no seu arquivo é CLIENTES";
                    return;
                }

                RetrieveExistentColunms(excelReader);

                //
                // Verifies if exist the necessary columms from excel file 
                //

                if (!ExistRequiredInformationsToProfile(excelReader) && !ExistRequiredColumnsToLegalEntityProfile(excelReader) || !GetColumm("CEP", excelReader))
                {
                    message = "Coluna obrigatória faltando no arquivo!";
                    return;
                }

                var addressManager = new AddressManager(this);

                while (excelReader.Read())
                {
                    totalRegisters++;

                    if (!String.IsNullOrEmpty(excelReader["CEP"].ToString()))
                        if (addressManager.GetAddress(excelReader["CEP"].ToString().Replace("-", "")) == null)
                        {
                            if (IsValidAddress(excelReader))
                                InsertNewAddress(excelReader);
                            else
                            {
                                errors++;
                                continue;
                            }
                        }

                    //
                    // Retrieve the cpnj's e cpf's from db for to compare with
                    // the cpnj's e cpf's retrieved from excel file
                    //

                    listCPF.AddRange(profileManager.GetCPFnumbers(companyId));
                    listCNPJ.AddRange(profileManager.GetCNPJnumbers(companyId));

                    var customer = new Customer();
                    var profile = new Profile();
                    var legalEntityProfile = new LegalEntityProfile();
                    var contact = new Contact();
                                     
                    if (ExistRequiredInformationsToProfile(excelReader) && !String.IsNullOrEmpty(excelReader["CPF"].ToString()) && !String.IsNullOrEmpty(excelReader["NOME"].ToString()))
                    {
                        //
                        // profile informations
                        //

                        if (ExistCpf(companyId, excelReader))
                        {
                            // verifies if the cpf is related with some customer
                            if (CustomerManager.GetCustomer(companyId, excelReader["CPF"].ToString()) == null)
                            {                             
                                AttachCustomerToProfile(companyId, userId, excelReader);
                                continue;
                            }
                            else errors++;

                            continue;
                        }

                        profile = FillProfileData(excelReader);

                        if (!ValidateProfileData(profile) || !ValidateAddressData(profile.PostalCode ?? String.Empty, profile.AddressNumber ?? String.Empty, profile.AddressComp ?? String.Empty))
                        {
                            errors++;
                            continue;
                        }

                        //save the customer with profile
                        customer.CompanyId = companyId;
                        customer.ModifiedDate = DateTime.Now;
                        customer.Profile = profile;

                        CustomerManager.Insert(customer);

                        //
                        // insert a contact and related it with customer
                        //
                        if (ExistsColumm("CONTATO"))
                            if (!String.IsNullOrEmpty(excelReader["CONTATO"].ToString()))
                                SaveCustomerContact(customer, userId, excelReader["CONTATO"].ToString());

                        listCPF.Add(excelReader["CPF"].ToString());
                        continue;
                    }
                    //
                    // LegalEntityProfile informations
                    //

                    if (ExistRequiredColumnsToLegalEntityProfile(excelReader) && !String.IsNullOrEmpty(excelReader["CNPJ"].ToString()) && !String.IsNullOrEmpty(excelReader["RAZAO_SOCIAL"].ToString()))
                    {
                        if (ExistCnpj(companyId, excelReader))
                        {
                            // verifies if the cnpj is related with some customer
                            if (CustomerManager.GetCustomer(companyId, excelReader["CNPJ"].ToString()) == null)
                            {
                                AttachCustomerToLegalEntityProfile(companyId, userId, excelReader);
                                continue;
                            }
                            else
                                errors++;

                            continue;
                        }

                        legalEntityProfile = FillLegalEntityProfileData(excelReader);

                        if (!ValidateLegalEntityProfileData(legalEntityProfile)
                            || !ValidateAddressData(profile.PostalCode ?? String.Empty, profile.AddressNumber ?? String.Empty, profile.AddressComp ?? String.Empty))
                        {
                            errors++;
                            continue;
                        }

                        //
                        // insert a customer
                        //
                        customer.CompanyId = companyId;
                        customer.ModifiedDate = DateTime.Now;
                        customer.LegalEntityProfile = legalEntityProfile;
                        CustomerManager.Insert(customer);

                        //
                        // insert a contact and related it with customer
                        //
                        if (ExistsColumm("CONTATO"))
                            if (!String.IsNullOrEmpty(excelReader["CONTATO"].ToString()))
                                SaveCustomerContact(customer, userId, excelReader["CONTATO"].ToString());

                        listCNPJ.Add(excelReader["CNPJ"].ToString());
                    }
                }

                message = "Total de Registros: " + totalRegisters + "; Registros importados com sucesso: " + Convert.ToInt32(totalRegisters - errors) +
                    "; Registros não importados por incompatibilidade de dados: " + errors;

                excelConnection.Close();
            }

        }

        #region Useful private methods

        /// <summary>
        /// Verifies if exists CPF and NOME columns
        /// </summary>
        /// <param name="excelReader"></param>
        /// <returns></returns>
        private bool ExistRequiredInformationsToProfile(OleDbDataReader excelReader)
        {
            return ExistsColumm("CPF") && ExistsColumm("NOME");
        }

        /// <summary>
        /// Verifies if exists CNPJ and RAZAO_SOCIAL columns
        /// </summary>
        /// <param name="excelReader"></param>
        /// <returns></returns>
        private bool ExistRequiredColumnsToLegalEntityProfile(OleDbDataReader excelReader)
        {
            return ExistsColumm("CNPJ") && ExistsColumm("RAZAO_SOCIAL");
        }

        /// <summary>
        /// This method fills a profile object with data from excel file
        /// </summary>
        /// <param name="excelReader"></param>
        /// <returns>The profile object filled from excel file </returns>
        private Profile FillProfileData(OleDbDataReader excelReader)
        {
            var profile = new Profile();

            profile.Name = excelReader["NOME"].ToString();
            profile.CPF = excelReader["CPF"].ToString();
            profile.PostalCode = excelReader["CEP"].ToString().Replace("-", "");

            //
            //  Columns not required
            //

            if (ExistsColumm("TELEFONE"))
                profile.Phone = excelReader["TELEFONE"].ToString();

            if (ExistsColumm("CELULAR"))
                profile.Phone = excelReader["CELULAR"].ToString();

            if (ExistsColumm("NUMERO"))
                profile.AddressNumber = excelReader["NUMERO"].ToString();

            if (ExistsColumm("COMPLEMENTO"))
                profile.AddressComp = excelReader["COMPLEMENTO"].ToString();

            if (ExistsColumm("FAX"))
                profile.Fax = excelReader["FAX"].ToString();

            if (ExistsColumm("EMAIL"))
                profile.Email = excelReader["EMAIL"].ToString();

            return profile;
        }

        /// <summary>
        /// This method checks if the specified columm exist in listExistentColumms
        /// </summary>
        /// <param name="colummName"></param>
        /// <returns></returns>
        private bool ExistsColumm(string colummName)
        {
            if (listExistentColumms.Any(x => x.Key == colummName && x.Value))
                return true;

            return false;
        }

        /// <summary>
        /// This method fills a LegalEntityProfile object with data from excel file
        /// </summary>
        /// <param name="excelReader"></param>
        /// <returns>The LegalEntityProfile object filled from excel file</returns>
        private LegalEntityProfile FillLegalEntityProfileData(OleDbDataReader excelReader)
        {
            var legalEntityProfile = new LegalEntityProfile();

            legalEntityProfile.CompanyName = excelReader["RAZAO_SOCIAL"].ToString();
            legalEntityProfile.CNPJ = excelReader["CNPJ"].ToString();
            legalEntityProfile.PostalCode = excelReader["CEP"].ToString().Replace("-", "");

            //
            // Fields not required
            //

            if (ExistsColumm("NOME_FANTASIA"))
                legalEntityProfile.FantasyName = excelReader["NOME_FANTASIA"].ToString();

            if (ExistsColumm("INSC_ESTADUAL"))
                legalEntityProfile.FantasyName = excelReader["INSC_ESTADUAL"].ToString();

            if (ExistsColumm("INSC_MUNICIPAL"))
                legalEntityProfile.FantasyName = excelReader["INSC_MUNICIPAL"].ToString();

            if (ExistsColumm("TELEFONE"))
                legalEntityProfile.Phone = excelReader["TELEFONE"].ToString();

            if (ExistsColumm("NUMERO"))
                legalEntityProfile.AddressNumber = excelReader["NUMERO"].ToString();

            if (ExistsColumm("COMPLEMENTO"))
                legalEntityProfile.AddressComp = excelReader["COMPLEMENTO"].ToString();

            if (ExistsColumm("FAX"))
                legalEntityProfile.Fax = excelReader["FAX"].ToString();

            if (ExistsColumm("EMAIL"))
                legalEntityProfile.Email = excelReader["EMAIL"].ToString();

            return legalEntityProfile;
        }

        /// <summary>
        /// This method fills a Contact object with necessary data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="contactName"></param>
        /// <returns>A Contact object filled</returns>
        private Contact FillContactData(Int32 companyId, Int32 userId, string contactName)
        {
            var contact = new Contact();

            contact.Name = contactName;
            contact.CompanyId = companyId;
            contact.UserId = userId;

            return contact;
        }

        /// <summary>
        /// This method save the contact, relates it with customer and inserts in db
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="userId"></param>
        /// <param name="contactName"></param>
        private void SaveCustomerContact(Customer customer, Int32 userId, string contactName)
        {
            var contact = FillContactData(customer.CompanyId, userId, contactName);

            ContactManager.Insert(contact);

            var customerContact = new CustomerContact { CompanyId = customer.CompanyId, ContactId = contact.ContactId, CustomerId = customer.CustomerId };
            ContactManager.InsertCustomerContact(customerContact);
        }

        /// <summary>
        ///  This method verifies if the address data of legalEntityProfile or profile, be
        ///  into range of characters supported by database
        /// </summary>
        /// <param name="postalCode"></param>
        /// <param name="addressNumber"></param>
        /// <param name="addressComp"></param>
        /// <returns></returns>
        private bool ValidateAddressData(string postalCode, string addressNumber, string addressComp)
        {
            if (postalCode.Trim().Length > 8 || addressNumber.Trim().Length > 10 || addressComp.Trim().Length > 50)
                return false;

            return true;
        }

        /// <summary>
        ///  This method verifies if the Profile data be
        ///  into range of characters supported by database
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        private bool ValidateProfileData(Profile profile)
        {
            if (profile.CPF.Trim().Length > 14 || profile.Name.Trim().Length > 100 || profile.Email.Trim().Length > 50
                || profile.Phone.Trim().Length > 13 || profile.Fax.Trim().Length > 13)
                return false;

            return true;
        }

        /// <summary>
        ///   This method verifies if the legalEntityProfile data be
        ///  into range of characters supported by database
        /// </summary>
        /// <param name="legalEntityProfile"></param>
        /// <returns></returns>
        private bool ValidateLegalEntityProfileData(LegalEntityProfile legalEntityProfile)
        {
            if (legalEntityProfile.CNPJ.Trim().Length > 18 || legalEntityProfile.CompanyName.Trim().Length > 100 || legalEntityProfile.FantasyName.Trim().Length > 100
                || legalEntityProfile.Email.Trim().Length > 50 || legalEntityProfile.Phone.Trim().Length > 13 || legalEntityProfile.Fax.Trim().Length > 13)
                return false;

            return true;
        }

        /// <summary>
        /// This method verifies if exist a specified columm in excel file
        /// </summary>
        /// <param name="colummName"></param>
        /// <param name="excelReader"></param>
        /// <returns></returns>
        private bool GetColumm(string colummName, OleDbDataReader excelReader)
        {
            var existColumm = new Boolean();

            try
            {
                excelReader.GetOrdinal(colummName);
                existColumm = true;
            }
            catch (IndexOutOfRangeException)
            {
                existColumm = false;
            }

            return existColumm;
        }

        /// <summary>
        /// This method adds the existent columns to listExistentColumms  
        /// </summary>
        /// <param name="excelReader"></param>
        private void RetrieveExistentColunms(OleDbDataReader excelReader)
        {
            //
            // LegalEntityProfile informations
            //

            listExistentColumms.Add("CNPJ", GetColumm("CNPJ", excelReader));
            listExistentColumms.Add("RAZAO_SOCIAL", GetColumm("RAZAO_SOCIAL", excelReader));
            listExistentColumms.Add("NOME_FANTASIA", GetColumm("NOME_FANTASIA", excelReader));
            listExistentColumms.Add("INSC_ESTADUAL", GetColumm("INSC_ESTADUAL", excelReader));
            listExistentColumms.Add("INSC_MUNICIPAL", GetColumm("INSC_MUNICIPAL", excelReader));

            //
            // Profile informations
            //

            listExistentColumms.Add("CPF", GetColumm("CPF", excelReader));
            listExistentColumms.Add("NOME", GetColumm("NOME", excelReader));
            listExistentColumms.Add("CELULAR", GetColumm("CELULAR", excelReader));

            //
            // General informations
            //

            listExistentColumms.Add("TELEFONE", GetColumm("TELEFONE", excelReader));
            listExistentColumms.Add("EMAIL", GetColumm("EMAIL", excelReader));
            listExistentColumms.Add("FAX", GetColumm("FAX", excelReader));
            listExistentColumms.Add("CONTATO", GetColumm("CONTATO", excelReader));

            //
            // Address informations
            //

            listExistentColumms.Add("CEP", GetColumm("CEP", excelReader));
            listExistentColumms.Add("ENDERECO", GetColumm("ENDERECO", excelReader));
            listExistentColumms.Add("ESTADO", GetColumm("ESTADO", excelReader));
            listExistentColumms.Add("CIDADE", GetColumm("CIDADE", excelReader));
            listExistentColumms.Add("BAIRRO", GetColumm("BAIRRO", excelReader));
            listExistentColumms.Add("NUMERO", GetColumm("NUMERO", excelReader));
            listExistentColumms.Add("COMPLEMENTO", GetColumm("COMPLEMENTO", excelReader));
        }

        /// <summary>
        /// This method verifies if the cnpj already exist in db
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="excelReader"></param>
        /// <returns></returns>
        private bool ExistCnpj(Int32 companyId, OleDbDataReader excelReader)
        {
            if (listCNPJ.Find(x => x.Equals(excelReader["CNPJ"].ToString())) != null)
                return true;

            return false;
        }

        /// <summary>
        /// This method inserts a customer and relate it with a existing legalEntityProfile
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="excelReader"></param>
        private void AttachCustomerToLegalEntityProfile(Int32 companyId, Int32 userId, OleDbDataReader excelReader)
        {
            var customer = new Customer();
            var profileManager = new ProfileManager(this);

            customer.CompanyId = companyId;
            customer.ModifiedDate = DateTime.Now;
            customer.LegalEntityProfileId = profileManager.GetLegalEntityProfile(excelReader["CNPJ"].ToString()).LegalEntityProfileId;

            CustomerManager.Insert(customer);

            if (ExistsColumm("CONTATO"))
                if (!String.IsNullOrEmpty(excelReader["CONTATO"].ToString()))
                    SaveCustomerContact(customer, userId, excelReader["CONTATO"].ToString());
        }

        /// <summary>
        /// This method inserts a customer and relate it with a existing legalEntityProfile
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="excelReader"></param>
        private void AttachCustomerToProfile(Int32 companyId, Int32 userId, OleDbDataReader excelReader)
        {
            var customer = new Customer();
            var profileManager = new ProfileManager(this);

            customer.CompanyId = companyId;
            customer.ModifiedDate = DateTime.Now;
            customer.ProfileId = profileManager.GetProfile(excelReader["CPF"].ToString()).ProfileId;

            CustomerManager.Insert(customer);

            if (ExistsColumm("CONTATO"))
                if (!String.IsNullOrEmpty(excelReader["CONTATO"].ToString()))
                    SaveCustomerContact(customer, userId, excelReader["CONTATO"].ToString());
        }

        /// <summary>
        /// This method verifies if the cpf already exist in db
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="excelReader"></param>
        /// <returns></returns>
        private bool ExistCpf(Int32 companyId, OleDbDataReader excelReader)
        {
            if (listCPF.Find(x => x.Equals(excelReader["CPF"].ToString())) != null)
                return true;

            return false;
        }

        /// <summary>
        /// This method relates city, state, neighborhood and inserts a new address
        /// </summary>
        /// <param name="excelReader"></param>
        private void InsertNewAddress(OleDbDataReader excelReader)
        {
            var addressManager = new AddressManager(this);
            String stateId = String.Empty;
            City city = new City();
            Neighborhood neighborhood = new Neighborhood();

            //
            //Get the city            
            //
            city = addressManager.GetCity(excelReader["CIDADE"].ToString());

            if (city == null)
            {
                // insert the new city
                city = new City { Name = excelReader["CIDADE"].ToString(), PostalCode = excelReader["CEP"].ToString(), StateId = stateId };
                addressManager.InsertCity(city);
            }

            //
            //Get the neighborhood
            //
            neighborhood = addressManager.GetNeighborhood(excelReader["BAIRRO"].ToString());

            if (neighborhood != null)
            {
                addressManager.SaveAddress(excelReader["CEP"].ToString(), excelReader["ENDEREÇO"].ToString(), neighborhood.NeighborhoodId);
            }
            else
            {
                neighborhood = new Neighborhood
                {
                    Name = excelReader["BAIRRO"].ToString(),
                    CityId = city.CityId
                };

                addressManager.InsertNeighborhood(neighborhood);
                addressManager.SaveAddress(excelReader["CEP"].ToString(), excelReader["ENDEREÇO"].ToString(), neighborhood.NeighborhoodId);
            }
        }

        /// <summary>
        /// This method verifies if the address from excel file is valid
        /// </summary>
        /// <param name="excelReader"></param>
        /// <returns></returns>
        private bool IsValidAddress(OleDbDataReader excelReader)
        {
            //
            //Validate
            //
            if (ExistsColumm("ESTADO") && ExistsColumm("CIDADE") && ExistsColumm("BAIRRO") && ExistsColumm("ENDEREÇO"))
            {
                if (String.IsNullOrEmpty(excelReader["CIDADE"].ToString()) || String.IsNullOrEmpty(excelReader["ESTADO"].ToString()) ||
                    String.IsNullOrEmpty(excelReader["BAIRRO"].ToString()) || String.IsNullOrEmpty(excelReader["ENDEREÇO"].ToString()))
                    return false;
            }
            else
                return false;

            //
            //Get the state
            //
            var stateId = new AddressManager(this).GetState(excelReader["ESTADO"].ToString());

            if (String.IsNullOrEmpty(stateId))
                return false;

            return true;
        }

        #endregion

    }
}
