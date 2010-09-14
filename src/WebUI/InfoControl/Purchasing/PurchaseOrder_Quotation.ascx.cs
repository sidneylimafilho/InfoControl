using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl.Web;
using InfoControl.Web.UI;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;


using InfoControl.Web.UI;

namespace Vivina.Erp.WebUI.Purchasing
{
    public partial class PurchaseOrder_Quotation : Vivina.Erp.SystemFramework.UserControlBase<Company_POS_PurchaseOrder>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cboCandidateSupplier.Items.Clear();
                cboCandidateSupplier.Items.Add(new ListItem
                                               {
                                                   Text = "",
                                                   Value = ""
                                               });
                cboCandidateSupplier.DataBind();

                grdProductsQuotation.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (selSuppllier.SupplierId == null && String.IsNullOrEmpty(cboCandidateSupplier.SelectedValue))
            {
                Page.ShowError("Selecione um fornecedor!");
                return;
            }

            if (datDeliveryDate.DateTime < DateTime.Now.Date)
            {
                Page.ShowError("A data de entrega deve ser maior que hoje!");
                return;
            }

            if (grdProductsQuotation.Rows.Count == 0)
                return;

            int supplierId = selSuppllier.SupplierId ?? Convert.ToInt32(cboCandidateSupplier.SelectedValue);

            var ucTotalAmount = grdProductsQuotation.FooterRow.FindControl<CurrencyField>("ucTotalAmount");

            //
            // Load Quotation
            //
            Quotation quotation = Page.Manager.GetQuotation(Page.PurchaseOrder.PurchaseOrderId, supplierId) ??
                                  new Quotation
                                  {
                                      CompanyId = Page.Company.CompanyId,
                                      PurchaseOrderId = Page.PurchaseOrder.PurchaseOrderId,
                                      SupplierId = supplierId,
                                      TotalPrice = ucTotalAmount.CurrencyValue.Value,
                                      DeliveryDate = datDeliveryDate.DateTime.Value
                                  };

            var quotationItems = new List<QuotationItem>();

            foreach (GridViewRow row in grdProductsQuotation.Rows)
            {
                var ucCurrFieldPrice = (CurrencyField)row.FindControl("ucCurrFieldPrice");
                int purchaseOrderItemId = Convert.ToInt32(grdProductsQuotation.DataKeys[row.RowIndex]["PurchaseOrderItemId"]);

                QuotationItem originalQuotationItem = Page.Manager.GetQuotationItem(Page.Company.CompanyId,
                                                                      supplierId,
                                                                      purchaseOrderItemId,
                                                                      Page.PurchaseOrder.PurchaseOrderId);

                var quotationItem = originalQuotationItem.Duplicate();
                quotationItem.CompanyId = Page.Company.CompanyId;
                quotationItem.Price = ucCurrFieldPrice.CurrencyValue.Value;
                quotationItem.SupplierId = supplierId;
                quotationItem.PurchaseOrderItemId = purchaseOrderItemId;
                quotationItem.PurchaseOrderId = Page.PurchaseOrder.PurchaseOrderId;

                quotationItems.Add(quotationItem);
            }

            //
            //caso a ordem de compra não estiver cadastrada
            //
            if (Page.PurchaseOrder.PurchaseOrderId == 0)
                quotation.PurchaseOrderId = Page.SavePurchaseOrder().PurchaseOrderId;

            Page.Manager.SaveQuotation(quotation, quotationItems, Page.Employee);
            Page.ViewState["_productList"] = Page.Manager.GetPurchaseOrderQuotedItems(
                                               Page.Company.CompanyId,
                                               Page.PurchaseOrder.PurchaseOrderId,
                                               PurchaseOrderDecision.LowUnitPrice);

            if (Page.PurchaseOrder.PurchaseOrderStatusId == PurchaseOrderStatus.Reproved ||
                Page.PurchaseOrder.PurchaseOrderStatusId == PurchaseOrderStatus.WaitingforApproval)
                Page.ShowAlert("O processo não pode ser concluído e foi direcionado ao setor de compras para analise!");

            foreach (QuotationItem item in quotationItems)
            {
                PurchaseOrderItem pItem = Page.Manager.GetPurchaseOrderItem(item.PurchaseOrderItemId);
                if (Page.PurchaseOrder.Quotations.Count() < 3 && pItem.ProductPackage.RequiresQuotationInPurchasing)
                    Page.ShowAlert("Para este processo ser aprovado, necessita de 3 cotações ou mais!");

            }
        }

        protected void odsPurchaseOrder_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["matrixId"] = Page.Company.MatrixId;
        }

        protected void odsPurchaseOrderItems_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["matrixId"] = Page.Company.MatrixId;
            e.InputParameters["purchaseOrderId"] = Page.PurchaseOrder.PurchaseOrderId;
            e.InputParameters["supplierId"] = selSuppllier.SupplierId ?? Convert.ToInt32("0" + cboCandidateSupplier.SelectedValue);
        }

        private void ClearFields()
        {
            selSuppllier.Reset();

            grdProductsQuotation.DataBind();
        }

        protected void selSupplier_OnSelectedSupplier(object sender, SelectedSupplierEventArgs e)
        {
            grdProductsQuotation.DataBind();

            var quotation = Page.Manager.GetQuotation(Page.PurchaseOrder.PurchaseOrderId, selSuppllier.Supplier.SupplierId);
            if (quotation != null)
            {
                grdProductsQuotation.FooterRow.FindControl<CurrencyField>("ucTotalAmount").CurrencyValue = quotation.TotalPrice;
                datDeliveryDate.DateTime = quotation.DeliveryDate;
                var ucTotalAmount = grdProductsQuotation.FooterRow.FindControl<CurrencyField>("ucTotalAmount");
                ucTotalAmount.CurrencyValue = quotation.TotalPrice ?? quotation.QuotationItems.Sum(x => x.Price * x.PurchaseOrderItem.QuantityOrdered);
            }
        }

        protected void odsSearchSuppliers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["purchaseOrderId"] = Page.PurchaseOrder.PurchaseOrderId;
        }

        protected void cboCandidateSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            odsPurchaseOrderItems.SelectMethod = "GetLastQuotationValues";
            grdProductsQuotation.DataBind();
        }
    }
}