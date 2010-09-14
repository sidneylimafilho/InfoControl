using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl.Data;
using InfoControl;
using System.IO;

namespace Vivina.Erp.WebUI
{
    public partial class CompanySymbol : Vivina.Erp.SystemFramework.PageBase
    {
        CompanyConfiguration companyConfigurationUpdated;
        CompanyManager companyManager;


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            companyManager = new CompanyManager(this);
            companyConfigurationUpdated = new CompanyConfiguration();

            companyConfigurationUpdated.CopyPropertiesFrom(Company.CompanyConfiguration);

            string file = txtImageUpload.PostedFile.FileName;
            string fileExtension = Path.GetExtension(file);

            companyConfigurationUpdated.WelcomeText = txtWelcomeText.Text;
            
            if (fileExtension.ToUpper() != ".GIF" && fileExtension.ToUpper() != ".JPG" && fileExtension.ToUpper() != ".PNG")
                companyConfigurationUpdated.Logo = Company.CompanyConfiguration.Logo;
            else
                companyConfigurationUpdated.Logo = resizeImage(txtImageUpload, 183, 51);
                        
            companyManager.UpdateCompanyConfiguration(Company.CompanyConfiguration, companyConfigurationUpdated);
        }

        private Byte[] resizeImage(FileUpload fileUpload, int width, int height)
        {
            //
            //Get file extension
            //
            string fileExtension = Path.GetExtension(fileUpload.PostedFile.FileName);

            //
            //Generate thumbnail image
            //
            MemoryStream mstrOut = new MemoryStream();
            System.Drawing.Image img = System.Drawing.Image.FromStream(new MemoryStream(fileUpload.FileBytes));
            img = img.GetThumbnailImage(183, 51, null, IntPtr.Zero);

            //
            //Verify file extension
            //
            if (fileExtension.ToUpper() == ".GIF")
                img.Save(mstrOut, System.Drawing.Imaging.ImageFormat.Gif);

            if (fileExtension.ToUpper() == ".JPG")
                img.Save(mstrOut, System.Drawing.Imaging.ImageFormat.Jpeg);

            if (fileExtension.ToUpper() == ".PNG")
                img.Save(mstrOut, System.Drawing.Imaging.ImageFormat.Png);

            return mstrOut.ToArray();
        }


    }
}
