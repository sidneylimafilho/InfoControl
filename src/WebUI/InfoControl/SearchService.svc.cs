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
using System.Web;

namespace Vivina.Erp.WebUI
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class SearchService : DataServiceBase
    {
        #region Sample Methods
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
        #endregion


        #region auto-complete services

        [OperationContract, JavaScriptSerializer]
        public ClientResponse FindCustomers(string q, int limit)
        {
            return new ClientResponse(() =>
            {
                using (var manager = new CustomerManager(null))
                    return manager.SearchCustomers(Company.CompanyId, q, limit).ToArray();
            });
        }

        [OperationContract, JavaScriptSerializer]
        public ClientResponse FindHelpPages(string q, int limit)
        {
            return new ClientResponse(() =>
            {
                using (var manager = new SearchManager(null))
                    return manager.GetHelpPages(Company.CompanyId, q, "", 0, limit)
                                  .Select(p => new Recognizable(p.Url.Replace("~", Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath),
                                                                p.Name))
                                  .ToArray();
            });
        }

        [OperationContract, JavaScriptSerializer]
        public ClientResponse FindSuppliers(string q, int limit)
        {
            return new ClientResponse(() =>
            {
                using (var manager = new SupplierManager(null))
                    return manager.SearchSupplier(Company.CompanyId, q, limit).ToArray();
            });
        }

        [OperationContract, JavaScriptSerializer]
        public ClientResponse FindProducts(string q, int limit)
        {
            return new ClientResponse(() =>
            {
                using (var manager = new ProductManager(null))
                    return manager.SearchProduct((int)Company.MatrixId, q, limit).ToArray();
            });
        }

        [OperationContract, JavaScriptSerializer]
        public ClientResponse FindContacts(string q, int limit)
        {
            return new ClientResponse(() =>
            {
                using (var manager = new ContactManager(null))
                    return manager.SearchContacts(Company.CompanyId, q, limit).ToArray();
            });
        }

        [OperationContract, JavaScriptSerializer]
        public ClientResponse FindProductAndService(String q, Int32 limit)
        {
            return new ClientResponse(() =>
            {
                using (var receiptManager = new ReceiptManager(null))
                    return receiptManager.SearchProductAndService((Int32)Company.MatrixId, q, limit).ToArray();
            });
        }

        [OperationContract, JavaScriptSerializer]
        public ClientResponse FindManufacturer(string q, int limit)
        {
            return new ClientResponse(() =>
            {
                using (var manager = new ManufacturerManager(null))
                    return manager.SearchManufacturer((int)Company.MatrixId, q, limit).ToArray();
            });
        }

        [OperationContract, JavaScriptSerializer]
        public ClientResponse FindTransporter(string q, int limit)
        {
            return new ClientResponse(() =>
            {
                using (var transporterManager = new TransporterManager(null))
                    return transporterManager.SearchTransporter((int)Company.MatrixId, q, limit).ToArray();
            });
        }

        [OperationContract, JavaScriptSerializer]
        public ClientResponse FindProductInInventory(string q, int limit)
        {
            return new ClientResponse(() =>
            {
                using (var manager = new ProductManager(null))
                    return manager.SearchProductInInventory(User.Identity.UserId, q, limit).ToArray();
            });
        }

        [OperationContract, JavaScriptSerializer]
        public ClientResponse FindUser(string q, int limit)
        {
            return new ClientResponse(() =>
            {
                using (var companyManager = new CompanyManager(null))
                    return companyManager.SearchUser(Company.CompanyId, q, limit).ToArray();
            });
        }

        [OperationContract, JavaScriptSerializer]
        public ClientResponse FindReport(string prefixText, int count)
        {
            return new ClientResponse(() =>
            {
                using (var reportsManager = new ReportsManager(null))
                    return reportsManager.SearchReportAsArray(prefixText, count);
            });
        }

        [OperationContract, JavaScriptSerializer]
        public ClientResponse FindEmployees(string q, int limit)
        {
            return new ClientResponse(() =>
            {
                using (var humanResourcesManager = new HumanResourcesManager(null))
                    return humanResourcesManager.SearchEmployees(Company.CompanyId, q, limit).ToArray();
            });
        }

        #endregion

      

        //[JavaScriptSerializer]
        //public JsonResult SearchBills(string text)
        //{
        //    if (String.IsNullOrEmpty(text))
        //        return null;

        //    using (var manager = new SearchManager(null))
        //    {
        //        IQueryable<Recognizable> billList = from bill in manager.GetBills(Company.CompanyId, text, "", 0, 12)
        //                                            select new Recognizable(bill.BillId.EncryptToHex(), bill.Description);

        //        return ClientResponse(() => billList.ToArray());
        //    }
        //}


        //[JavaScriptSerializer]
        //public JsonResult SearchInvoices(string text)
        //{
        //    if (String.IsNullOrEmpty(text))
        //        return null;

        //    using (var manager = new SearchManager(null))
        //    {
        //        IQueryable<Recognizable> invoiceList = from invoice in manager.GetInvoices(Company.CompanyId, text, "", 0, 12)
        //                                               select new Recognizable(invoice.InvoiceId.EncryptToHex(), invoice.Description);

        //        return ClientResponse(() => invoiceList.ToArray());
        //    }
        //}

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