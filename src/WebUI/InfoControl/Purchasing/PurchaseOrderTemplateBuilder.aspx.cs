using System;
using System.Text;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


namespace Vivina.Erp.WebUI.Purchasing
{
    public partial class PurchaseOrderTemplateBuilder : Vivina.Erp.SystemFramework.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PurchaseOrderCode"] == null)
                return;

            String purchaseOrderCode = Convert.ToString(Session["PurchaseOrderCode"]);
            var purchaseOrderManager = new PurchaseOrderManager(this);
            PurchaseOrder purchaseOrder = purchaseOrderManager.GetPurchaseOrderByPurchaseOrderCode(purchaseOrderCode);

            Response.Clear();
            Response.ContentType = "text/rtf";
            Response.AddHeader("content-disposition", "attachment;filename=OrdemDeCompra.rtf");
            Response.ContentEncoding = Encoding.Default;
            Response.Write(purchaseOrderManager.ApplyPurchaseOrderInDocumentTemplate(purchaseOrder,
                                                                           Company.CompanyConfiguration.
                                                                               PurchaseOrderTemplate));
            Response.End();
        }
    }
}