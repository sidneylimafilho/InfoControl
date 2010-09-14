using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules.Properties;
using Vivina.Erp.SystemFramework;
using InfoControl;




namespace Vivina.Erp.WebUI.Administration
{
    public partial class BirthDayCustomers : Vivina.Erp.SystemFramework.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        //protected void grdBirthDayCustomers_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        //{

        //    Context.Items["CustomerId"] = grdBirthDayCustomers.DataKeys[e.NewSelectedIndex]["CustomerId"];
        //    Server.Transfer("Customer.aspx");

        //}

        protected void odsBirthDayCustomers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
        }

        protected void grdBirthDayCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes["onclick"] = "location='Customer.aspx?CustomerId=" + e.Row.DataItem.GetPropertyValue("CustomerId").EncryptToHex() + "' ;";

        }

        
    }
   
}
