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


public partial class App_Reports_DinamicReports : InfoControl.Web.UI.DataPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
  
    protected void grdDinamicReports_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            Server.Transfer("DynamicReport.aspx");
        }
    }
    protected void grdDinamicReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('" + Resources.Resource.RemovingRegisterMessage + "') == false) return false;");
            e.Row.Attributes["onclick"] = "location='DynamicReport.aspx?ReportTablesSchemaId=" + Convert.ToInt32(grdDinamicReports.DataKeys[e.Row.RowIndex]["ReportTablesSchemaId"]).EncryptToHex() + "';";
             
        }
    }
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Server.Transfer("DynamicReport.aspx");
    }
    protected void odsDinamicReports_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        //
        // This method is to not allow deleting items that are associated with others Tables
        //
        if (e.Exception != null)
        {
            if (e.Exception.InnerException is System.Data.SqlClient.SqlException)
            {
                System.Data.SqlClient.SqlException err = e.Exception.InnerException as System.Data.SqlClient.SqlException;
                if (err.ErrorCode.Equals(Convert.ToInt32("0x80131904", 16)))
                {
                    //ShowError(Resources.Exception.DeletingRegisterWithForeignKey);
                    e.ExceptionHandled = true;
                }
            }
        }
    }
}
