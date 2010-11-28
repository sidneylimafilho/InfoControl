using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl;

namespace Vivina.Erp.WebUI.App_Reports
{
    public partial class DynamicReportVariables : Vivina.Erp.SystemFramework.PageBase
    {

        Int32 reportTablesSchemaId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
                grdReportColumnsSchema.Columns[2].HeaderText = "<a href='DynamicReportField.aspx?ReportTablesSchemaId=" + Request["ReportTablesSchemaId"] + "'> <div class='insert' title='inserir'>    </div> </a> ";

            if (!String.IsNullOrEmpty(Request["ReportTablesSchemaId"]))
                reportTablesSchemaId = Convert.ToInt32(Request["ReportTablesSchemaId"]);

        }

        protected void odsReportColumnsSchemas_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["tableId"] = reportTablesSchemaId;
        }

        protected void grdReportColumnsSchema_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");

                e.Row.Attributes["onclick"] = "location='DynamicReportField.aspx?ReportColumnsSchemaId=" + Convert.ToString(grdReportColumnsSchema.DataKeys[e.Row.RowIndex]["ReportColumnsSchemaId"]) + "&ReportTablesSchemaId=" + Convert.ToString(grdReportColumnsSchema.DataKeys[e.Row.RowIndex]["ReportTablesSchemaId"]) + "';";

            }
        }
        protected void btnShowEditColumns_Click(object sender, EventArgs e)
        {
            Response.Redirect("DynamicReportField.aspx?ReportTablesSchemaId=" + Request["ReportTablesSchemaId"]);

        }

        protected void odsReportColumnsSchemas_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
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
}
