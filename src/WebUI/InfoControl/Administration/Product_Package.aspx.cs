using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


namespace Vivina.Erp.WebUI.Administration
{
    public partial class Product_Package : Vivina.Erp.SystemFramework.PageBase
    {
        private int productId;

        protected void Page_Load(object sender, EventArgs e)
        {
            productId = Convert.ToInt32(Request["pid"]);
        }

        protected void ods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["productId"] = productId;
        }

        protected void btnSavePackage_Click(object sender, ImageClickEventArgs e)
        {
            var productManager = new ProductManager(this);
            productManager.InsertProductPackage(productId, new ProductPackage
                                                           {
                                                               Name = txtPackage.Text,
                                                               RequiresQuotationInPurchasing = chkRequiredQuotation.Checked
                                                               
                                                           });
            DataBind();
            txtPackage.Text = String.Empty;
        }

        protected void grdProductPackage_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ShowError("Há processos de compra que contém produtos com esta embalagem!");
                e.ExceptionHandled = true;
            }
        }
    }
}