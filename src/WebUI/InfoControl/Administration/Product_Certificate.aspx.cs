using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl;

namespace Vivina.Erp.WebUI.Administration
{
    public partial class Product_Certificate : Vivina.Erp.SystemFramework.PageBase
    {
        int productId;
        protected void Page_Load(object sender, EventArgs e)
        {
            productId = Convert.ToInt32(Request["pid"].DecryptFromHex());
        }

        protected void btnSaveCetification_Click(object sender, ImageClickEventArgs e)
        {
            ProductManager productManager = new ProductManager(this);
            productManager.InsertProductCertificate(productId, new ProductCertificate()
            {
                Name = txtCertification.Text
            });
            DataBind();
            txtCertification.Text = String.Empty;
        }

        protected void odsProductCertificates_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["productId"] = productId;
        }
    }
}
