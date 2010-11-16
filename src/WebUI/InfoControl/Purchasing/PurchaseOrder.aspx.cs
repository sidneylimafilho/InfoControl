using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;

using Exception = Resources.Exception;

[PermissionRequired("PurchaseOrder")]
public partial class Company_POS_PurchaseOrder : Vivina.Erp.SystemFramework.PageBase<PurchaseOrderManager>
{
    #region PurchaseStep enum

    public enum PurchaseStep
    {
        Supplier = 0,
        Cover = 1,
        Product = 2,
        Summary = 3,
        Success = 4
    }

    #endregion

    private PurchaseOrder _purchaseOrder;
    private PurchaseOrderItem pItem = new PurchaseOrderItem();
    private int purchaseOrderId;
    public Wizard Wizard;

    public PurchaseOrder PurchaseOrder
    {
        get
        {
            if (_purchaseOrder != null)
                return _purchaseOrder;

            if (Page.ViewState["_purchaseOrderId"] != null)
                purchaseOrderId = Convert.ToInt32(Page.ViewState["_purchaseOrderId"]);

            _purchaseOrder = Manager.GetPurchaseOrder(purchaseOrderId, Company.CompanyId);
            if (_purchaseOrder != null)
                return _purchaseOrder;

            return (_purchaseOrder = new PurchaseOrder
                                      {
                                          PurchaseOrderCode = Util.GenerateUniqueID()
                                      });
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Wizard = wzdPurchaseOrder;
        if (Request["pid"] != null)
            purchaseOrderId = Convert.ToInt32(Request["pid"]);

        if (!IsPostBack)
        {
            Page.ViewState["PurchaseCode"] = null;
            Page.ViewState["_productList"] = null;
            Page.ViewState["Cover"] = null;
            Page.ViewState["SupplierId"] = null;
            Page.ViewState["Summary"] = null;
        }
        Page.ViewState["ActiveStepIndex"] = wzdPurchaseOrder.ActiveStepIndex;
    }

    protected void odsBudgetItems_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CompanyId"] = Company.CompanyId;
    }

    protected void wzdPurchaseOrder_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        if (e.NextStepIndex == (int)PurchaseStep.Summary)
            if (Session["_productList"] == null && Page.ViewState["_productList"] == null)
            {
                e.Cancel = true;
                ShowError(Exception.AddProductInList);
            }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        Manager.DbContext.SubmitChanges();
    }

    public PurchaseOrder SavePurchaseOrder(int? status)
    {
        var purchaseOrder = PurchaseOrder.Duplicate();

        var productList = (List<PurchaseOrderQuotedItem>)Page.ViewState["_productList"];
        if (productList == null || productList.Count == 0)
        {
            ShowError(Exception.AddProductInList);
            return null;
        }

        if (status.HasValue)
            purchaseOrder.PurchaseOrderStatusId = status.Value;

        purchaseOrder.PurchaseOrderDecision = Convert.ToInt32(Page.ViewState["PurchaseOrderDecision"] ?? PurchaseOrderDecision.LowUnitPrice);
        purchaseOrder.CompanyId = Company.CompanyId;
        purchaseOrder.ModifiedDate = DateTime.Now;


        if (Page.ViewState["SupplierId"] != null)
            purchaseOrder.SupplierId = Convert.ToInt32(Page.ViewState["SupplierId"]);

        if (PurchaseOrder.PurchaseOrderId > 0)
            Manager.Update(purchaseOrder, productList, Page.User.Identity.UserId);
        else
        {
            purchaseOrder.EmployeeId = Employee.EmployeeId;
            Manager.Insert(purchaseOrder, productList, Page.User.Identity.UserId);

            Page.ViewState["_purchaseOrderId"] = purchaseOrder.PurchaseOrderId;

        }

        _purchaseOrder = null;

        Page.ViewState["PurchaseOrderCode"] = null;
        Page.ViewState["PurchaseOrderDecision"] = null;
        Page.ViewState["_productList"] = Manager.GetPurchaseOrderQuotedItems(
                                               Company.CompanyId,
                                               purchaseOrder.PurchaseOrderId,
                                               PurchaseOrderDecision.LowUnitPrice);


        return PurchaseOrder;
    }

    public PurchaseOrder SavePurchaseOrder()
    {
        return SavePurchaseOrder(null);
    }
}