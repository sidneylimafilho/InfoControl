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

using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl.Web.Security;

[PermissionRequired("Upgrade")]
public partial class Company_Administration_MyInfoControl_Upgrade : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnInsert_Click(object sender, ImageClickEventArgs e)
    {
        PackagesManager pManager = new PackagesManager(this);
        PackageAdditional pAdditional = new PackageAdditional();

        pAdditional.CompanyId = Company.CompanyId;
        pAdditional.PackageId = Convert.ToInt16(cboPackage.SelectedValue);
        pAdditional.EndDate = pAdditional.StartDate = DateTime.Now;
        pManager.UpGrade(pAdditional);

        grdPackageAdditional.DataBind();
        cboPackage.DataBind();
    }
    protected void odsAdditionalPackages_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
    protected void grdPackageAdditional_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
            (e.Row.Cells[e.Row.Cells.Count - 1].FindControl("btnDelete") as LinkButton).Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
    }
    protected void grdPackageAdditional_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        PackagesManager pManager = new PackagesManager(this);
        pManager.DownGrade(Convert.ToInt32(grdPackageAdditional.DataKeys[e.RowIndex]["AddonId"]));
        grdPackageAdditional.DataBind();
        e.Cancel = true;
    }
}
