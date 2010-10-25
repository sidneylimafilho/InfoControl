using System;
using System.Collections;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using InfoControl;
using InfoControl.Web;
using InfoControl.Web.Services;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Reports;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;

namespace Vivina.Erp.WebUI
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class SearchService : DataServiceBase
    {
        [JavaScriptSerializer]
        [ServiceKnownType(typeof(Customer))]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public object HelloWorld(Customer formData, params object[] parameters)
        {
            var request = new Customer();
            request.AccountNumber = "49468-9";
            request.CustomerId = 10;
            request.Bank = new Bank { BankId = 1, Name = "Itau" };
            return request;
        }

        [JavaScriptSerializer]
        [OperationContract]
        public ClientResponse GetOneSampleData(Hashtable parameters, Hashtable formData)
        {
            return new ClientResponse
                       {
                           Data = (new[]
                                       {
                                           new {Nome = "asfdasd", ID = 1},
                                           new {Nome = "fdgfjfgf", ID = 2},
                                           new {Nome = " gk lyui yui ", ID = 3},
                                           new {Nome = "645retyy", ID = 4},
                                           new {Nome = "sdgfhdfrd", ID = 5},
                                           new {Nome = "fde wertewrtw", ID = 6},
                                           new {Nome = "Haroldo de Andrade", ID = 7}
                                       }).FirstOrDefault(x => x.ID == Convert.ToInt32(parameters["itemId"]))
                       };
        }

        [JavaScriptSerializer]
        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json)]
        public ClientResponse GetSampleData(Hashtable parameters, Hashtable formData)
        {
            return new ClientResponse
                       {
                           Data = new[]
                                      {
                                          new {Nome = Convert.ToString(parameters["itemId"]) + "1) asfdasd", ID = 1},
                                          new {Nome = Convert.ToString(parameters["itemId"]) + "2) fdgfjfgf", ID = 2},
                                          new {Nome = Convert.ToString(parameters["itemId"]) + "3) gk lyui yui ", ID = 3},
                                          new {Nome = Convert.ToString(parameters["itemId"]) + "4) 645retyy", ID = 4},
                                          new {Nome = Convert.ToString(parameters["itemId"]) + "5) sdgfhdfrd", ID = 5},
                                          new {Nome = Convert.ToString(parameters["itemId"]) + "6) fde wertewrtw", ID = 6},
                                          new {Nome = Convert.ToString(parameters["itemId"]) + "7) Haroldo de Andrade", ID = 7}
                                      }
                       };
        }

        #region auto-complete services

        [JavaScriptSerializer]
        [OperationContract]
        public object FindCustomers(Hashtable parameters, Hashtable formData)
        {
            return new ClientResponse
            {
                Data = new CustomerManager(this).SearchCustomers(Company.CompanyId,
                    Convert.ToString(formData["txtSearch"]), Convert.ToInt32(parameters["limit"])).ToArray()
            };
        }

        [JavaScriptSerializer]
        public JsonResult SearchSuppliers(string q, int limit)
        {
            return ClientResponse(() => new SupplierManager(this).SearchSupplier(Company.CompanyId, q, limit).ToArray());
        }

        [JavaScriptSerializer]
        public JsonResult SearchProduct(string q, int limit)
        {
            using (var manager = new ProductManager(null))
                return ClientResponse(() => manager.SearchProduct((int)Company.MatrixId, q, limit).ToArray());
        }

        [JavaScriptSerializer]
        public JsonResult SearchContact(string q, int limit)
        {
            using (var manager = new ContactManager(null))
                return ClientResponse(() => manager.SearchContacts(Company.CompanyId, q, limit).ToArray());
        }

        [JavaScriptSerializer]
        public JsonResult SearchProductAndService(String q, Int32 limit)
        {
            using (var receiptManager = new ReceiptManager(null))
                return ClientResponse(() => receiptManager.SearchProductAndService((Int32)Company.MatrixId, q, limit).ToArray());
        }

        [JavaScriptSerializer]
        public JsonResult SearchManufacturer(string q, int limit)
        {
            using (var manager = new ManufacturerManager(null))
                return ClientResponse(() => manager.SearchManufacturer((int)Company.MatrixId, q, limit).ToArray());
        }

        [JavaScriptSerializer]
        public JsonResult SearchTransporter(string q, int limit)
        {
            using (var transporterManager = new TransporterManager(null))
                return ClientResponse(() => transporterManager.SearchTransporter((int)Company.MatrixId, q, limit).ToArray());
        }

        [JavaScriptSerializer]
        public JsonResult SearchProductInInventory(string q, int limit)
        {
            using (var manager = new ProductManager(null))
                return ClientResponse(() => manager.SearchProductInInventory(User.Identity.UserId, q, limit).ToArray());
        }

        [JavaScriptSerializer]
        public JsonResult SearchUser(string q, int limit)
        {
            using (var companyManager = new CompanyManager(null))
                return ClientResponse(() => companyManager.SearchUser(Company.CompanyId, q, limit).ToArray());
        }

        [JavaScriptSerializer]
        [ScriptMethod]
        public JsonResult SearchReport(string prefixText, int count)
        {
            using (var reportsManager = new ReportsManager(null))
                return ClientResponse(() => reportsManager.SearchReportAsArray(prefixText, count));
        }

        [JavaScriptSerializer]
        public JsonResult SearchEmployees(string q, int limit)
        {
            if (String.IsNullOrEmpty(q)) return null;

            using (var humanResourcesManager = new HumanResourcesManager(null))
                return ClientResponse(() => humanResourcesManager.SearchEmployees(Company.CompanyId, q, limit).ToArray());
        }

        #endregion

        #region Start Page Search

        //[JsonFilter]
        //public Recognizable[] SearchSuppliers(string text)
        //{
        //    if (String.IsNullOrEmpty(text))
        //        return null;

        //    using (var manager = new SearchManager(null))
        //    {
        //        var suppliersList = from supplier in manager.GetSuppliersByName(Company.CompanyId, text, "", 0, 12)
        //                            select new Recognizable(supplier.SupplierId.EncryptToHex(),
        //                                                    supplier.Profile != null ?
        //                                                        supplier.Profile.Name :
        //                                                        supplier.LegalEntityProfile.CompanyName);

        //        return suppliersList.ToArray();
        //    }
        //}


        [JavaScriptSerializer]
        public JsonResult SearchContacts(string text)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            using (var manager = new SearchManager(null))
            {
                IQueryable<Recognizable> contactsList = from contact in manager.GetContacts(Company.CompanyId, text, "", 0, 12)
                                                        select new Recognizable(contact.ContactId.EncryptToHex(), contact.Name);

                return ClientResponse(() => contactsList.ToArray());
            }
        }


        [JavaScriptSerializer]
        public JsonResult SearchProducts(string text)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            using (var manager = new ProductManager(null))
            {
                IQueryable<Recognizable> productList = from product in manager.GetProducts(Company.CompanyId, null, true, null, text, text,
                                                                                           false, null, null, null, "", 0, 12)
                                                       select new Recognizable(product.ProductId.EncryptToHex(), product.Name);

                return ClientResponse(() => productList.ToArray());
            }
        }


        [JavaScriptSerializer]
        public JsonResult SearchBills(string text)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            using (var manager = new SearchManager(null))
            {
                IQueryable<Recognizable> billList = from bill in manager.GetBills(Company.CompanyId, text, "", 0, 12)
                                                    select new Recognizable(bill.BillId.EncryptToHex(), bill.Description);

                return ClientResponse(() => billList.ToArray());
            }
        }


        [JavaScriptSerializer]
        public JsonResult SearchInvoices(string text)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            using (var manager = new SearchManager(null))
            {
                IQueryable<Recognizable> invoiceList = from invoice in manager.GetInvoices(Company.CompanyId, text, "", 0, 12)
                                                       select new Recognizable(invoice.InvoiceId.EncryptToHex(), invoice.Description);

                return ClientResponse(() => invoiceList.ToArray());
            }
        }


        [JavaScriptSerializer]
        public JsonResult SearchHelpPages(string text)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            using (var manager = new SearchManager(null))
            {
                IQueryable<Recognizable> helpPageList = from helpPage in manager.GetHelpPages(Company.CompanyId, text, "", 0, 12)
                                                        select new Recognizable(helpPage.WebPageId.EncryptToHex(), helpPage.Name);

                return ClientResponse(() => helpPageList.ToArray());
            }
        }

        #endregion

        //[JsonFilter]
        //public JsonResult GetServiceOrders(Hashtable Params, Hashtable FormData)
        //{
        //    using (var servicesManager = new ServicesManager(null))
        //        return ClientResponse(() => servicesManager.GetOpenServiceOrdersToArray(Company.CompanyId));
        //}

        //[JsonFilter]
        //public JsonResult GetOpenCustomerCalls(Hashtable Params, Hashtable FormData)
        //{
        //    using (var customerManager = new CustomerManager(null))
        //        return ClientResponse(() => customerManager.GetOpenCustomerCalls(Company.CompanyId).ToArray());
        //}


        // Add more operations here and mark them with [JsonFilter]
    }
}