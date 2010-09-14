using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;

namespace Vivina.Erp.WebUI
{
    public partial class CompanyModels : Vivina.Erp.SystemFramework.PageBase
    {
        CompanyManager companyManager;
        String fisicalPath;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAddContractTemplate_Click(object sender, ImageClickEventArgs e)
        {
            if (String.IsNullOrEmpty(Company.LegalEntityProfile.Website))
            {
                ShowError("A empresa não tem um site configurado! Ex: www.vivina.com.br");
                return;
            }

            var modelFileName = fupDocumentTemplate.PostedFile.FileName;

            if (!modelFileName.EndsWith(".htm") && !modelFileName.EndsWith(".html") && !modelFileName.EndsWith(".rtf"))
            {
                ShowError("Extensão do documento inválida! Selecione documentos de extensão .htm, .html ou .rtf");
                return;
            }


            companyManager = new CompanyManager(this);
            var documentTemplate = new DocumentTemplate
                                   {
                                       CompanyId = Company.CompanyId,
                                       FileName = fupDocumentTemplate.FileName,
                                       FileUrl = Company.GetDocumentTemplateDirectory() + fupDocumentTemplate.PostedFile.FileName,
                                       DocumentTemplateTypeId = Convert.ToInt32(cboDocumentTemplateTypes.SelectedValue)
                                   };

            companyManager.InsertDocumentTemplate(documentTemplate);
            grdDocumentsTemplate.DataBind();

            //
            // Save in Hard Disk
            //
            fupDocumentTemplate.SaveAs(Server.MapPath(documentTemplate.FileUrl));


        }

        protected void odsDocumentTemplates_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyID"] = Company.CompanyId;
        }

        protected void grdDocumentsTemplate_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            fisicalPath = e.Keys["FileUrl"].ToString();
        }

        protected void odsDocumentTemplates_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.InputParameters["FileUrl"] = Server.MapPath(fisicalPath);
        }
    }
}
