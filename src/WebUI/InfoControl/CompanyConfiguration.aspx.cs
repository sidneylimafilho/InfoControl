using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

//[PermissionRequired("CompanyConfiguration")]
public partial class Company_RH_HeaderFooter : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {


        //if (!IsPostBack)
        //{

        //    if (Company.CompanyConfiguration != null)
        //    {
        //        txtHeader.Content = Company.CompanyConfiguration.ReportHeader;
        //        txtFooter.Content = Company.CompanyConfiguration.ReportFooter;
        //        txtReportBottom.Text = Company.CompanyConfiguration.ReportMargimBottom;
        //        txtReportLeft.Text = Company.CompanyConfiguration.ReportMarginLeft;
        //        txtReportRight.Text = Company.CompanyConfiguration.ReportMarginRight;
        //        txtReportUp.Text = Company.CompanyConfiguration.ReportMarginTop;
        //        txtWelcomeText.Content = Company.CompanyConfiguration.WelcomeText;
        //        txtWelcomeText.Focus();

        //        txtPrinterFooter.Text = Company.CompanyConfiguration.PrinterFooter;
        //        txtUnitPrice1Name.Text = Company.CompanyConfiguration.UnitPrice1Name;
        //        if (String.IsNullOrEmpty(txtUnitPrice1Name.Text))
        //            txtUnitPrice1Name.Text = "Preço de venda";
        //        txtUnitPrice2Name.Text = Company.CompanyConfiguration.UnitPrice2Name;
        //        txtUnitPrice3Name.Text = Company.CompanyConfiguration.UnitPrice3Name;
        //        txtUnitPrice4Name.Text = Company.CompanyConfiguration.UnitPrice4Name;
        //        txtUnitPrice5Name.Text = Company.CompanyConfiguration.UnitPrice5Name;


        //        //contract Additional values
        //        txtContractAdicionalValue1Name.Text = Company.CompanyConfiguration.ContractAdditionalValue1Name;
        //        txtContractAdicionalValue2Name.Text = Company.CompanyConfiguration.ContractAdditionalValue2Name;
        //        txtContractAdicionalValue3Name.Text = Company.CompanyConfiguration.ContractAdditionalValue3Name;
        //        txtContractAdicionalValue4Name.Text = Company.CompanyConfiguration.ContractAdditionalValue4Name;
        //        txtContractAdicionalValue5Name.Text = Company.CompanyConfiguration.ContractAdditionalValue5Name;


        //    }

        //}
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        
        //    CompanyConfiguration companyConfig = new CompanyConfiguration();

        //    string file = txtImageUpload.PostedFile.FileName;
        //    string fileExtension = Path.GetExtension(file);
        //    if (fileExtension.ToUpper() != ".GIF" && fileExtension.ToUpper() != ".JPG" && fileExtension.ToUpper() != ".PNG")
        //        companyConfig.Logo = Company.CompanyConfiguration.Logo;
        //    else
        //        companyConfig.Logo = resizeImage(txtImageUpload, 183, 51);

        //    companyConfig.ReportHeader = txtHeader.Content;
        //    companyConfig.ReportFooter = txtFooter.Content;
        //    companyConfig.WelcomeText = txtWelcomeText.Content.Replace("$0", "<br>");
        //    companyConfig.ReportMarginTop = txtReportUp.Text;
        //    companyConfig.ReportMarginRight = txtReportRight.Text;
        //    companyConfig.ReportMarginLeft = txtReportLeft.Text;
        //    companyConfig.ReportMargimBottom = txtReportBottom.Text;
        //    companyConfig.PrinterFooter = txtPrinterFooter.Text;


        //    //Sale PriceValues
        //    companyConfig.UnitPrice1Name = txtUnitPrice1Name.Text;
        //    companyConfig.UnitPrice2Name = txtUnitPrice2Name.Text;
        //    companyConfig.UnitPrice3Name = txtUnitPrice3Name.Text;
        //    companyConfig.UnitPrice4Name = txtUnitPrice4Name.Text;
        //    companyConfig.UnitPrice5Name = txtUnitPrice5Name.Text;

        //    //Contract AdditionalValues

        //    companyConfig.ContractAdditionalValue1Name = txtContractAdicionalValue1Name.Text;
        //    companyConfig.ContractAdditionalValue2Name = txtContractAdicionalValue2Name.Text;
        //    companyConfig.ContractAdditionalValue3Name = txtContractAdicionalValue3Name.Text;
        //    companyConfig.ContractAdditionalValue4Name = txtContractAdicionalValue4Name.Text;
        //    companyConfig.ContractAdditionalValue5Name = txtContractAdicionalValue5Name.Text;


        //    CompanyManager cManager = new CompanyManager(this);
        //    CompanyConfiguration oldConfig = new CompanyConfiguration();
        //    if (Company.CompanyConfiguration.CompanyConfigurationId != 0)
        //    {
        //        companyConfig.CompanyConfigurationId = Company.CompanyConfiguration.CompanyConfigurationId;
        //        oldConfig.CopyPropertiesFrom(Company.CompanyConfiguration);
        //        cManager.UpdateCompanyConfiguration(oldConfig, companyConfig);
        //    }
        //    else
        //        cManager.InsertCompanyConfiguration(companyConfig, Company);

        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "top.ResetHeader();", true);
        //    RefreshCredentials();
        //}

        //protected void btnAddContractTemplate_Click(object sender, ImageClickEventArgs e)
        //{

        //    if (fupContractTemplate.PostedFile.FileName.Length < 4 && fupContractTemplate.PostedFile.FileName.Substring(fupContractTemplate.PostedFile.FileName.Length - 4, 4).ToUpper() != ".RTF")
        //    {
        //        ShowError(Resources.Exception.InvalidFormatFile);
        //        return;
        //    }
        //    ContractTemplate contractTemplate = new ContractTemplate();
        //    ContractManager contractManager = new ContractManager(this);
        //    contractTemplate.CompanyId = Company.CompanyId;
        //    contractTemplate.FileName = txtContractTemplateFileName.Text;
        //    contractTemplate.FileContent = System.Text.Encoding.Default.GetString(fupContractTemplate.FileBytes);
        //    contractManager.saveContractTemplate(contractTemplate);
        //    grdContractTemplate.DataBind();

    }

    protected void odsContractTemplate_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    /// <summary>
    /// this method resize image
    /// </summary>
    /// <param name="fileUpload">reference of fileupload</param>
    /// <param name="width">required width</param>
    /// <param name="height">required height</param>
    /// <returns>return image resized</returns>
    //private Byte[] resizeImage(FileUpload fileUpload, int width, int height)
    //{
    //    //
    //    //Get file extension
    //    //
    //    string fileExtension = Path.GetExtension(fileUpload.PostedFile.FileName);

    //    //
    //    //Generate thumbnail image
    //    //
    //    MemoryStream mstrOut = new MemoryStream();
    //    System.Drawing.Image img = System.Drawing.Image.FromStream(new MemoryStream(fileUpload.FileBytes));
    //    img = img.GetThumbnailImage(183, 51, null, IntPtr.Zero);

    //    //
    //    //Verify file extension
    //    //
    //    if (fileExtension.ToUpper() == ".GIF")
    //        img.Save(mstrOut, System.Drawing.Imaging.ImageFormat.Gif);

    //    if (fileExtension.ToUpper() == ".JPG")
    //        img.Save(mstrOut, System.Drawing.Imaging.ImageFormat.Jpeg);

    //    if (fileExtension.ToUpper() == ".PNG")
    //        img.Save(mstrOut, System.Drawing.Imaging.ImageFormat.Png);

    //    return mstrOut.ToArray();
    //}


    //protected void grdContractTemplate_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
    //    }
    //}
    protected void grdContractTemplate_Sorting(object sender, GridViewSortEventArgs e)
    {

    }
    protected void odsContractTemplate_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
    {

    }

    protected void btnPurchaseOrderFileName_Click(object sender, EventArgs e)
    {
        //    if (fupPurchaseOrderFile.PostedFile.FileName.Length < 4 && fupPurchaseOrderFile.PostedFile.FileName.Substring(fupPurchaseOrderFile.PostedFile.FileName.Length - 4, 4).ToUpper() != ".RTF")
        //    {
        //        ShowError(Resources.Exception.InvalidFormatFile);
        //        return;
        //    }
        //    CompanyManager companyManager = new CompanyManager(this);
        //    CompanyConfiguration newCompanyConfiguration = new CompanyConfiguration();
        //    if (Company.CompanyConfiguration != null)
        //        newCompanyConfiguration.CopyPropertiesFrom(Company.CompanyConfiguration);

        //    newCompanyConfiguration.PurchaseOrderTemplate = System.Text.Encoding.Default.GetString(fupPurchaseOrderFile.FileBytes);


        //    if (Company.CompanyConfiguration != null)
        //        companyManager.UpdateCompanyConfiguration(Company.CompanyConfiguration, newCompanyConfiguration);
        //    else
        //        companyManager.UpdateCompanyConfiguration(Company.CompanyConfiguration, newCompanyConfiguration);
        //}
    }
}
