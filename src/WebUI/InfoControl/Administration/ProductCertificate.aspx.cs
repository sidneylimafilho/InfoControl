using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Vivina.Framework;
using Vivina.InfoControl.DataClasses;
using Vivina.InfoControl.BusinessRules;
using Vivina.Framework.Security.Cryptography;

namespace Vivina.InfoControl.WebUI.InfoControl.Administration
{
    public partial class Product_Certificate : Vivina.InfoControl.SystemFramework.Web.PageBase
    {
        int productId;
        protected void Page_Load(object sender, EventArgs e)
        {
            productId = Convert.ToInt32(Request["ProductId"]);
        }

        protected void btnSaveCetification_Click(object sender, ImageClickEventArgs e)
        {
            ProductManager productManager = new ProductManager(this);
            productManager.InsertProductCertificate(productId, new ProductCertificate()
            {
                Name = txtCertification.Text
            });
            grdProductCertificates.DataBind();
        }

        protected void grdProductCertificates_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ProductManager productManager = new ProductManager(this);
            productManager.DeleteProductCertificate(Convert.ToInt32(grdProductCertificates.DataKeys[e.RowIndex]["ProductCertificateId"]));
            grdProductCertificates.DataBind();
        }

        protected void odsProductCertificates_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["productId"] = productId; 
        }

    }
}
