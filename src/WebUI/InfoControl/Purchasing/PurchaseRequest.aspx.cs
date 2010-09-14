using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using InfoControl;
using InfoControl.Web;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Accounting;
using Vivina.Erp.DataClasses;

using InfoControl.Web.UI;
using Exception = Resources.Exception;

namespace Vivina.Erp.WebUI.Purchasing
{
    public partial class PurchaseRequest : Vivina.Erp.SystemFramework.PageBase
    {
        private DataClasses.PurchaseRequest _pRequest;
        private int _purchaseRequestId;

        public List<PurchaseRequestItem> PurchaseRequestItems
        {
            get
            {
                return (List<PurchaseRequestItem>)(Page.ViewState["_purchaseRequestItems"] ??
                        (Page.ViewState["_purchaseRequestItems"] = new List<PurchaseRequestItem>()));
            }
            set { Page.ViewState["_purchaseRequestItems"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _purchaseRequestId = Convert.ToInt32(Request["rid"]);
            if (!IsPostBack)
            {
                cboTreeBoxCostCenter.DataBind();
                if (!String.IsNullOrEmpty(Request["rid"]))
                    LoadPurchaseRequest(_purchaseRequestId);
            }
        }

        private void LoadPurchaseRequest(int requestId)
        {
            PurchaseRequestItems = new PurchaseManager(this)
                .GetPurchaseRequestItems(requestId)
                .ToList();
            BindPurchaseRequestItemList();
        }

        protected void btnAddPurchaseRequestItem_Click(object sender, EventArgs e)
        {
            Product product = selProduct.Product;

            if (selProduct.Product.CompositeProducts.Count == 0)
                if (selProduct.Product == null || selProduct.Package == null)
                {
                    ShowError("O produto deve possuir embalagem!");
                    return;
                }

            if (selProduct.Product.CompositeProducts.Count > 0)
            {
                PurchaseRequestItems.Clear();
                foreach (CompositeProduct composite in selProduct.Product.CompositeProducts)
                {
                    PurchaseRequestItems.Add(new PurchaseRequestItem
                                             {
                                                 ProductId = composite.ProductId,
                                                 Product = composite.Product1,
                                                 ProductManufacturer = composite.ProductManufacturer,
                                                 ProductManufacturerId = composite.ProductManufacturerId,
                                                 ProductPackage = composite.ProductPackage,
                                                 ProductPackageId = composite.ProductPackageId,
                                                 Amount = ucCurrFieldAmount.IntValue * composite.Amount
                                             });
                    tbForm.Visible = false;
                    ViewState["compositeProductId"] = selProduct.Product.ProductId;
                }
            }
            else
            {
                PurchaseRequestItems.Add(new PurchaseRequestItem
                                         {
                                             ProductId = product.ProductId,
                                             Product = selProduct.Product,
                                             ProductManufacturer = selProduct.Manufacturer,
                                             ProductManufacturerId = selProduct.Manufacturer != null
                                                                         ? selProduct.Manufacturer.ProductManufacturerId
                                                                         : (int?)null,
                                             ProductPackage = selProduct.Package,
                                             ProductPackageId = selProduct.Package.ProductPackageId,
                                             Amount = ucCurrFieldAmount.IntValue
                                         });
                ucCurrFieldAmount.CurrencyValue = null;
            }
            BindPurchaseRequestItemList();
            
            selProduct.Name = "";
        }

        private void BindPurchaseRequestItemList()
        {
            dtlPurchaseRequestItem.DataSource = PurchaseRequestItems;
            dtlPurchaseRequestItem.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (PurchaseRequestItems.Count == 0)
            {
                ShowError(Exception.AddProductInList);
                return;
            }

            if (String.IsNullOrEmpty(Address1.PostalCode))
            {
                ShowError("O local de entrega é obrigatório!");
                return;
            }

            _pRequest = new DataClasses.PurchaseRequest
                        {
                            EmployeeId = Employee.EmployeeId,
                            CostCenterId = Convert.ToInt32(cboTreeBoxCostCenter.SelectedValue),
                            CompanyId = Company.CompanyId,
                            PostalCode = Address1.PostalCode,
                            AddressComp = Address1.AddressComp,
                            AddressNumber = Address1.AddressNumber,
                            PurchaseRequestId = _purchaseRequestId
                        };

            if (ViewState["compositeProductId"] != null)
            {
                _pRequest.ProductId = Convert.ToInt32(ViewState["compositeProductId"]);
                _pRequest.Amount = ucCurrFieldAmount.IntValue;
            }

            foreach (GridViewRow row in dtlPurchaseRequestItem.Rows)
            {
                CurrencyField textBox1 = row.FindControl<CurrencyField>("TextBox1") ?? new CurrencyField();
                PurchaseRequestItems[row.RowIndex].Amount -= textBox1.IntValue;
            }

            new PurchaseManager(this).SavePurchaseRequest(_pRequest, PurchaseRequestItems);

            Response.Redirect("PurchaseRequests.aspx");
        }

        protected void odsCostCenter_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
        }

        protected void dtlPurchaseRequestItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            PurchaseRequestItems.RemoveAt(e.RowIndex);
            BindPurchaseRequestItemList();
        }

        protected void tree_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.Expanded = !e.Node.Text.StartsWith("-");
            e.Node.Text = e.Node.Text.TrimStart('-').Trim();
        }

        protected void cboDeposit_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var depositManager = new DepositManager(null))
            {
                Deposit deposit = depositManager.GetDeposit(Convert.ToInt32(cboDeposit.SelectedValue));
                Address1.PostalCode = deposit.PostalCode;
                Address1.AddressNumber = deposit.AddressNumber;
                Address1.AddressComp = deposit.AddressComp;
            }
        }

        protected void dtlPurchaseRequestItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[2].Visible = !tbForm.Visible;
        }
    }
}