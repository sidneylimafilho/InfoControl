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
using InfoControl.Data;
using InfoControl;
using InfoControl.Web.Security;
using InfoControl;

[PermissionRequired("Companies")]
public partial class Users_Register : Vivina.Erp.SystemFramework.PageBase
{
    CompanyManager companyManager;
    Company originalCompany;
    private void loadMatrixName()
    {
        cboMatrixId.DataSource = companyManager.GetMatrixCompaniesNames(Page.User.Identity.UserId);
        cboMatrixId.DataBind();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        companyManager = new CompanyManager(this);

        if (Context.Items["CompanyId"] != null)
            Page.ViewState["CompanyId"] = Context.Items["CompanyId"];

        if (Request.QueryString["CompanyId"] != null)
            Page.ViewState["CompanyId"] = Request.QueryString["CompanyId"];

        if (Page.ViewState["CompanyId"] != null)
        {
            Page.ViewState["loaded"] = true;
            originalCompany = companyManager.GetCompany(Convert.ToInt32(ViewState["CompanyId"]));
            if (!IsPostBack && originalCompany != null)
            {
                Profile_LegalEntity1.CompanyProfileEntity = originalCompany.LegalEntityProfile;
                loadMatrixName();

                ListItem listItem = cboMatrixId.Items.FindByValue(originalCompany.MatrixId.ToString());
                if (listItem != null)
                    cboMatrixId.SelectedValue = listItem.Value;
            }
        }
        else if (Page.ViewState["LegalEntityProfileId"] != null)
        {
            originalCompany = companyManager.GetCompanyByProfile(Convert.ToInt32(Page.ViewState["LegalEntityProfileId"]));
            if (originalCompany != null)
            {
                Page.ViewState["ProfileExists"] = "0";

                /*if isn't a postback set the values of company in profile_LegalEntity1
                 * else the values are reload in all postback
                */
                if (!IsPostBack)
                {
                    Profile_LegalEntity1.CompanyProfileEntity = originalCompany.LegalEntityProfile;
                    loadMatrixName();
                    cboMatrixId.Items.FindByValue(originalCompany.MatrixId.ToString()).Selected = true;
                }

            }

        }
    }
    protected void odsMatrixCompanies_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["referenceCompanyId"] = Company.ReferenceCompanyId;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Company company = new Company();
            // Clone the original company for the linq track changes 
            if (originalCompany != null)
                company.CopyPropertiesFrom(originalCompany);

            //
            // Set the company properties 
            //
            company.ReferenceCompanyId = Company.ReferenceCompanyId;


            company.MatrixId = String.IsNullOrEmpty(cboMatrixId.SelectedValue) ? company.CompanyId : Convert.ToInt32(cboMatrixId.SelectedValue);
            company.StartDate = DateTime.Now;
            company.NextStatementDueDate = DateTime.Now;
            company.ModifiedDate = DateTime.Now;

            company.LegalEntityProfileId = Profile_LegalEntity1.CompanyProfileEntity.LegalEntityProfileId;

            //
            // Add entity for insert
            //
            if (company.LegalEntityProfileId == 0)
                company.LegalEntityProfile = Profile_LegalEntity1.CompanyProfileEntity;

            if (Page.ViewState["CompanyId"] == null && Page.ViewState["ProfileExists"] != "0")
            {
                InsertCompanyStatus status = companyManager.InsertCompany(company, User.Identity.UserId, Company.CompanyId);

                if (status != InsertCompanyStatus.Success)
                {
                    ShowError("Ocorreu um erro ao inserir uma empresa!");
                    return;
                }
            }
            else
                companyManager.Update(originalCompany, company);

            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "top.ResetHeader();", true);

            if (!String.IsNullOrEmpty(Request["host"]))
                Response.Redirect("~/Infocontrol/Host/Companies.aspx");
            else
                Response.Redirect("Companies.aspx");
        }
    }

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["host"]))
            Response.Redirect("~/Infocontrol/Host/Companies.aspx");
        else
            Response.Redirect("Companies.aspx");
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        pnlOtherData.Visible = Profile_LegalEntity1.CompanyProfileEntity != null;

    }
    protected void odsMatrixCompany_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["userId"] = User.Identity.UserId;
    }

}
