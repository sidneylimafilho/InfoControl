using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Vivina.Erp.BusinessRules.Services;
using InfoControl;
using InfoControl.Web.Security;

namespace Vivina.Erp.WebUI.InfoControl.Services
{
    public partial class Summary : Vivina.Erp.SystemFramework.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void grdCustomerCalls_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes["onclick"] = "top.$.LightBoxObject.show('CRM/CustomerCall.aspx?lightbox[iframe]=true&CustomerCallId=" + grdCustomerCalls.DataKeys[e.Row.RowIndex]["CustomerCallId"].EncryptToHex() + "');";
        }

        protected void grdServiceOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes["onclick"] = "top.$.LightBoxObject.show('Services/ServiceOrder.aspx?lightbox[iframe]=true&ServiceOrderId=" + grdServiceOrders.DataKeys[e.Row.RowIndex]["ServiceOrderId"].EncryptToHex() + "');";
        }


        protected void dataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
        }
    }

}