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
using System.Web.Services;

using InfoControl;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.SystemFramework;
using InfoControl.Web.Security;


[PermissionRequired("Transporters")]
public partial class Company_Trasporters : Vivina.Erp.SystemFramework.PageBase
{
  
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            cboPageSize.Items.Add(new ListItem() { Value = Int16.MaxValue.ToString(), Text = "Todos" });
           
        }
    }
    
    protected void odsTransporters_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = (int)Company.MatrixId;
    }
    
    protected void grdTransporters_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //
        //Verify if the row is a data row, to not get header and footer
        //
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
            e.Row.Attributes["onclick"] = "location='Transporter.aspx?TransporterId=" + e.Row.DataItem.GetPropertyValue("TransporterId").EncryptToHex() + "';";
        }
    }
    
    protected void grdTransporters_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            Server.Transfer("Transporter.aspx");
        }

    }
    
  
    
    protected void odsTransporters_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
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
    
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Server.Transfer("Transporter.aspx");
    }
   
    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdTransporters.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdTransporters.DataBind();
    }

    [WebMethod]
    public static bool DeleteTransporter(int transporterid)
    {
        bool result = true;
        using (TransporterManager transporterManager = new TransporterManager(null))
        {
            try
            {
                transporterManager.Delete(transporterManager.GetTransporter(transporterid));
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                result = false;
            }
        }
        return result;
    }
}
