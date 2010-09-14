using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


namespace Vivina.Erp.WebUI.InfoControl.Accounting
{
    public partial class CashMonthly : Vivina.Erp.SystemFramework.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            var saldo = Decimal.Zero;

             foreach(var item in new AccountManager(this).GetMonthlyCashMoney(Company.CompanyId))
             {
              
             
             
             }

        }

        protected void odsMonthlyCash_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
        }

      
    }
}
