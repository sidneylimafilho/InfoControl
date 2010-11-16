using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.WebUI.RH
{
    public partial class dependants : Vivina.Erp.SystemFramework.PageBase
    {
        private int employeeId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty((Request["eid"])))
                employeeId = Convert.ToInt32(Request["eid"]); 
        }

        protected void odsDependents_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
            e.InputParameters["employeeId"] = employeeId;
        }

        protected void grdDependents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit)
            {
                if (e.Row.RowState == DataControlRowState.Alternate || e.Row.RowState == DataControlRowState.Normal)
                {
                    PostBackOptions postOptions = new PostBackOptions(grdDependents, "Edit$" + e.Row.RowIndex.ToString());
                    string insertScript = Page.ClientScript.GetPostBackEventReference(postOptions);

                    for (int a = 0; a < (e.Row.Cells.Count - 2); a++)
                    {
                        e.Row.Cells[a].Attributes.Add("onclick", insertScript);
                    }

                    e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble == true;javascript:if(confirm('Deseja realmente apagar a linha solicitada') == false)return false;");
                }
            }
        }

        protected void grdDependents_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var humanResourcesManager = new HumanResourcesManager(this);
            var originalDependent = humanResourcesManager.GetEmployeeDependent(Convert.ToInt32(grdDependents.DataKeys[e.RowIndex]["EmployeeDependentId"]));
            var dependent = new EmployeeDependent();

            if (originalDependent != null)
                dependent.CopyPropertiesFrom(originalDependent);

            dependent.FamilyTree = (grdDependents.Rows[e.RowIndex].FindControl("txtUpdateFamilyTree") as TextBox).Text;
            dependent.Name = (grdDependents.Rows[e.RowIndex].FindControl("txtUpdateName") as TextBox).Text;

            if (!String.IsNullOrEmpty((grdDependents.Rows[e.RowIndex].FindControl("txtUpdateBirthDate") as TextBox).Text))
                dependent.BirthDate = Convert.ToDateTime((grdDependents.Rows[e.RowIndex].FindControl("txtUpdateBirthDate") as TextBox).Text);

            humanResourcesManager.UpdateEmployeeDependent(originalDependent, dependent);

            grdDependents.EditIndex = -1;
            grdDependents.DataBind();

            e.Cancel = true;
        }

        protected void btnAddDependent_Click(object sender, ImageClickEventArgs e)
        {
            var humanResourcesManager = new HumanResourcesManager(this);
            var dependent = new EmployeeDependent();

            dependent.Name = txtName.Text;
            dependent.FamilyTree = txtFamilyTree.Text;
            dependent.BirthDate = ucDtBirthDate.DateTime.Value;

            dependent.EmployeeId = employeeId;
            dependent.CompanyId = Company.CompanyId;

            humanResourcesManager.InsertEmployeeDependent(dependent);

            //
            // Clear fields
            //

            txtName.Text = string.Empty;
            txtFamilyTree.Text = string.Empty;
            ucDtBirthDate.DateTime = null;

            grdDependents.DataBind();
        }
    }
}
