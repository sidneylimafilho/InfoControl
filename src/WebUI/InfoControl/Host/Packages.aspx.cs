using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using InfoControl;

using Vivina.Erp.SystemFramework;

public partial class Host_Packages : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void grdPackages_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //
            // Cancel a nested event fires
            //
            e.Row.Attributes["onclick"] = "location='Package.aspx?PackageId=" + grdPackages.DataKeys[e.Row.RowIndex]["PackageId"] + "';";
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble == true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");


        }
    }

    protected void odsPackages_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        //
        // This method is to not allow deleting Products that are associated with others Tables
        //
        if (e.Exception != null)
        {
            if (e.Exception.InnerException is System.Data.SqlClient.SqlException)
            {
                System.Data.SqlClient.SqlException err = e.Exception.InnerException as System.Data.SqlClient.SqlException;
                if (err.ErrorCode.Equals(Convert.ToInt32("0x80131904", 16)))
                {
                    ShowError(Resources.Exception.DeletingRegisterWithForeignKey);
                    e.ExceptionHandled = true;
                }
            }
        }

    }
}

