using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.SystemFramework;
using InfoControl;


namespace Vivina.Erp.WebUI.InfoControl.Accounting
{
    public partial class Statements : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void grdStatements_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Attributes["onclick"] = "location='Statement.aspx?StatementId=" + grdStatements.DataKeys[e.Row.RowIndex]["StatementId"] + "';";

            }
        }

        protected void odsStatement_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
        }

    }
}