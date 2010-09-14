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
using System.Web.Services;


using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl;
using InfoControl.Web.Security;


[PermissionRequired("Representants")]
public partial class InfoControl_Administration_Representants : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cboPageSize.Items.Add(new ListItem() { Value = Int16.MaxValue.ToString(), Text = "Todos" });
        }
    }
    protected void odsRepresentant_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["initialLetter"] = ucAlphabeticalPaging.Letter;
    }
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Server.Transfer("Representant.aspx");
    }
    protected void grdRepresentants_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //
        //Verify if the row is a data row, to not get header and footer
        //
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
            e.Row.Attributes["onclick"] = "location='Representant.aspx?RepresentantId=" + e.Row.DataItem.GetPropertyValue("RepresentantId").EncryptToHex() + "';";
        }
    }
    protected void grdRepresentants_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            Server.Transfer("Representant.aspx");
        }
    }
    protected void grdRepresentants_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

    }



    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {

        grdRepresentants.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdRepresentants.DataBind();

    }
    protected void odsRepresentant_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
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

    [WebMethod]
    public static bool DeleteRepresentant(int representantid)
    {
        bool result = true;
        using (RepresentantManager representantManager = new RepresentantManager(null))
        {
            try
            {
                representantManager.Delete(representantManager.GetRepresentant(representantid));
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                result = false;
            }
        }
        return result;
    }

    protected void ucAlphabeticalPaging_SelectedLetter(object sender, SelectedLetterEventArgs e)
    {
        grdRepresentants.DataBind();
    }
}
