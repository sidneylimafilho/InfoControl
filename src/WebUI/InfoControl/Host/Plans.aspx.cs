using System;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl;
using Vivina.Erp.SystemFramework;


public partial class Host_Plans : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucDateTimeInterval.DateInterval = new DateTimeInterval(DateTime.Now, DateTime.Now.AddMonths(2));

            if (Page.Customization["beginDate"] != null && Page.Customization["endDate"] != null)
            {
                ucDateTimeInterval.DateInterval = new DateTimeInterval(Convert.ToDateTime(Page.Customization["beginDate"]), Convert.ToDateTime(Page.Customization["endDate"]));
            }
        }
    }

    protected void grdPlans_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = "location='Plan.aspx?PlanId=" + grdPlans.DataKeys[e.Row.RowIndex]["PlanId"] + "';";

            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdPlans.PageSize = Convert.ToInt32(cboPageSize.Text);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Page.Customization["beginDate"] = ucDateTimeInterval.DateInterval.BeginDate;
        Page.Customization["endDate"] = ucDateTimeInterval.DateInterval.EndDate;

        grdPlans.DataBind();
    }

    protected void odsPlans_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["dateTimeInterval"] = ucDateTimeInterval.DateInterval;
        e.InputParameters["name"] = txtFindPlan.Text;
    }
}