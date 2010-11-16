using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.BusinessRules.Services;
using Vivina.Erp.DataClasses;
using InfoControl;
using System.Web.Services;
using InfoControl.Web.Security;

[PermissionRequired("Service")]
public partial class InfoControl_Services_Services : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cboPageSize.Items.Add(new ListItem() { Value = Int16.MaxValue.ToString(), Text = "Todos" });
        }

    }

    protected void grvServices_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Attributes["onclick"] = "location='Service.aspx?ServiceId=" + e.Row.DataItem.GetPropertyValue("ServiceId") + "';";
            PostBackOptions postOptions = new PostBackOptions(this.grvServices, "Select$" + e.Row.RowIndex.ToString());
            String insertScript = ClientScript.GetPostBackEventReference(postOptions);



            //
            // This for not uses equal, to not overbound the index
            //
            for (int a = 0; a < e.Row.Cells.Count; a++)
            {
                e.Row.Cells[a].Attributes.Add("onclick", insertScript);
            }
            //
            // Cancel a nested event fires
            //
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }
    }

    protected void grvServices_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            Server.Transfer("service.aspx");
        }
    }

    
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Server.Transfer("service.aspx");
    }

    protected void odsServices_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void odsServices_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
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

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grvServices.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grvServices.DataBind();
    }


    [WebMethod]
    public static bool DeleteService(int serviceId)
    {
        bool result = true;
        using (ServicesManager servicesManager = new ServicesManager(null))
        {
            try
            {
                servicesManager.DeleteService(servicesManager.GetService(serviceId));
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                result = false;
            }
        }
        return result;
    }
}
