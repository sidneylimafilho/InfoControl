using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using InfoControl.Web.Security.DataEntities;
using Vivina.Erp.SystemFramework;

using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.SystemFramework;
using System.Web.Services;


public partial class Host_FunctionList : Vivina.Erp.SystemFramework.PageBase
{
 

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void grdFunctions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //
            // Cancel a nested event fires
            //
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");

        }
    }
  
  
   
    
    protected void odsFunctions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
    }
    protected void grdFunctions_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert") //Testa o clique no botão de inclusão de registro
        {
            Server.Transfer("Function.aspx");
            //isInserting = true;
            //e.Cancel = true;

            //this.frmFunctions.Visible = true;
            //this.grdFunctions.Visible = false;
            //this.grdFunctions.DataSourceID = null;
            //this.frmFunctions.ChangeMode(FormViewMode.Insert);
            //this.frmFunctions.PageIndex = -1;
            //this.frmFunctions.DataBind();
        }
    }
    protected void grdFunctions_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

        //this.frmFunctions.Visible = true;
        //this.grdFunctions.Visible = false;

        //this.frmFunctions.PageIndex = ((grdFunctions.PageIndex * grdFunctions.PageSize) + e.NewSelectedIndex);
        //this.frmFunctions.ChangeMode(FormViewMode.Edit);


        //Context.Items["FunctionId"] = grdServices.DataKeys[e.NewSelectedIndex]["ServiceOrderId"];
        // Server.Transfer("../Services/ServiceOrder.aspx");

        Context.Items["FunctionId"] = grdFunctions.DataKeys[e.NewSelectedIndex]["FunctionId"];
        Server.Transfer("Function.aspx");



    }

   
    [WebMethod]

    public static bool DeleteFunction(int functionId)
    {
        bool result = true;
        using (FunctionManager functionManager = new FunctionManager(null))
        {
            try
            {
                functionManager.DeleteFunction(functionId);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                result = false;
            }
        }
        return result;

    }

}