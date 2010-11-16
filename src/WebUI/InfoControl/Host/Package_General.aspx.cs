using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl;

namespace Vivina.Erp.WebUI.Host
{
    public partial class Package_General : Vivina.Erp.SystemFramework.PageBase
    {

        PackagesManager packagesManager;
        Package package;
        Package originalPackage;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request["PackageId"]))
                {
                    ShowPackage();
                    litTitle.Text = String.Empty;
                }

            }

        }

        private void ShowPackage()
        {
            packagesManager = new PackagesManager(this);

            package = packagesManager.GetPackages(Convert.ToInt32(Request["PackageId"]));

            txtName.Text = package.Name;
            txtNumberItems.Text = Convert.ToString(package.NumberItems);
            txtNumberUsers.Text = Convert.ToString(package.NumberUsers);
            ucCurrFieldPrice.CurrencyValue = package.Price;
            ucCurrFieldProductPrice.CurrencyValue = package.ProductPrice;
            ucCurrFieldSetupFee.CurrencyValue = package.SetupFee;
            ucCurrFieldValueByHour.CurrencyValue = package.UserPerHourPrice;
            chkIsActive.Checked = Convert.ToBoolean(package.IsActive);
        }

        private void SavePackage()
        {
            packagesManager = new PackagesManager(this);
            package = new Package();
            originalPackage = new Package();

            if (!String.IsNullOrEmpty(Request["PackageId"]))
            {
                originalPackage = packagesManager.GetPackages(Convert.ToInt32(Request["PackageId"]));
                package.CopyPropertiesFrom(originalPackage);
            }

            package.Name = txtName.Text;
            package.NumberItems = Convert.ToInt32(txtNumberItems.Text);
            package.NumberUsers = Convert.ToInt32(txtNumberUsers.Text);
            package.Price = Convert.ToDecimal(ucCurrFieldPrice.CurrencyValue);
            package.ProductPrice = ucCurrFieldProductPrice.CurrencyValue;
            package.SetupFee = ucCurrFieldSetupFee.CurrencyValue;
            package.UserPerHourPrice = ucCurrFieldValueByHour.CurrencyValue;
            package.IsActive = chkIsActive.Checked;

            if (String.IsNullOrEmpty(Request["PackageId"]))
            {
                packagesManager.Insert(package);
                Response.Redirect("Package.aspx?PackageId=" + package.PackageId);
            }
            else
                packagesManager.Update(originalPackage, package);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SavePackage();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request["PackageId"]))
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "parent.location='Packages.aspx';", true);
            else
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "location='Packages.aspx';", true);
        }
    }
}
