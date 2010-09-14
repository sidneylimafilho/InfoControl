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
using InfoControl;

public partial class InfoControl_Host_ScheduleTasks : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void grdScheduleTasks_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            Server.Transfer("ScheduleTask.aspx");
        }
    }

    protected void grdScheduleTasks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //
            // Cancel a nested event fires
            //
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
            e.Row.Attributes["onclick"] = "location='ScheduleTask.aspx?ScheduledTaskId=" + e.Row.DataItem.GetPropertyValue("ScheduledTaskId").EncryptToHex() + "' ;";
        }
    }

    protected void grdScheduleTasks_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            if (e.Exception.InnerException is System.Data.SqlClient.SqlException)
            {
                System.Data.SqlClient.SqlException err = e.Exception.InnerException as System.Data.SqlClient.SqlException;
                if (err.ErrorCode.Equals(Convert.ToInt32("0x80131904", 16)))
                {
                    ShowError("O registro não pode ser apagado pois há outros registros associados!");
                    e.ExceptionHandled = true;
                }
            }
        }
    }

    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Response.Redirect("ScheduleTask.aspx");
    }

    //protected void grdScheduleTasks_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Context.Items["ScheduleTaskId"] = grdScheduleTasks.DataKeys[grdScheduleTasks.SelectedIndex]["ScheduleTaskId"].ToString();
    //    Server.Transfer("ScheduleTask.aspx");
    //}

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdScheduleTasks.PageSize = Convert.ToInt32(cboPageSize.Text);
    }
}
