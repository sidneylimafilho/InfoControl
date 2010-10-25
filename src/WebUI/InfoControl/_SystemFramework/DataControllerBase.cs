using InfoControl;
using InfoControl.Web.Services;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.SystemFramework
{
    public class DataServiceBase : DataService
    {
        private Company _company;

        public Company Company
        {
            get
            {
                if (_company == null)
                    if ((_company = (Company)Session["_company"]) == null)
                    {
                        var manager = new CompanyManager(null);
                        Session["_company"] = _company = manager.GetCompanyByContext(System.Web.HttpContext.Current);
                        _company.LegalEntityProfile.Address.City.ToLower();
                    }


                return _company;
            }
        }
    }
}