using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Accounting;


namespace Vivina.Erp.WebUI.Accounting
{
    public partial class PurchaseRequests : Vivina.Erp.SystemFramework.PageBase
    {
        private PurchaseManager purchaseManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                chkGrouping.Items[1].Selected = true; // Cidade
                chkGrouping.Items[2].Selected = true; // Centro de Cust
                chkGrouping.Items[3].Selected = true; // Categoria
                // RegroupGrid();
            }
            btnGeneratePurchaseOrder.Visible = (Employee.PurchaseCeilingValue >= 0);
            //grdPurchaseReq.Visible = Employee.PurchaseCeilingValue.HasValue;
        }

        protected void btnGeneratePurchaseOrder_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Request["requestItems"]))
            {
                ShowError("É necessário escolher produtos para avançar!");
                return;
            }

            
            int previousCity = -1;
            var productManager = new ProductManager(this);
            foreach (string requestItem in Request["requestItems"].Split(','))
            {
                string[] item = requestItem.Trim().Split('|');
                int productId = Convert.ToInt32(item[0]);
                //int productPackageId = Convert.ToInt32(item[1]);
                //int productManufacturerId = Convert.ToInt32(item[2]);
                //int requestAmount = Convert.ToInt32(item[3]);
                //string centerCost = item[4];
                int cityId = Convert.ToInt32("0" + item[5]);


                Product product = productManager.GetProduct(productId, Company.CompanyId);
                if (product != null)
                    if (product.RequiresAuthorization.Value && (Employee.CentralBuyer != true))
                    {
                        ShowError(product.Name + " é comprado apenas pela sede, requisição encaminhada ao setor de compras!");
                        return;
                    }

                if (previousCity != -1 && cityId != previousCity)
                {
                    ShowError("Não pode gerar um processo de compra para cidades diferentes!");
                    return;
                }

                previousCity = cityId;
            }

            Context.Items["PurchaseRequest"] = true;
            Server.Transfer("PurchaseOrder.aspx");
        }

        protected void odsPurchaseRequests_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
            e.InputParameters["employeeId"] = Employee.EmployeeId;
        }

        protected void chkGrouping_TextChanged(object sender, EventArgs e)
        {
            // RegroupGrid();

        }

        private void RegroupGrid()
        {
            grdPurchaseReq.MasterTableView.GroupByExpressions.Clear();
            int i = 0;
            if (chkGrouping.Items[0].Selected)
                grdPurchaseReq.MasterTableView.GroupByExpressions.Insert(i++,
                        AddGroupByExpression("Req.", "PurchaseRequestId", "<b><a href='PurchaseRequest.aspx?rid={0}'>{0}</a></b>"));

            if (chkGrouping.Items[1].Selected)
                grdPurchaseReq.MasterTableView.GroupByExpressions.Insert(i++,
                        AddGroupByExpression("Cidade.", "City", "<b>{0}</b>"));

            if (chkGrouping.Items[2].Selected)
                grdPurchaseReq.MasterTableView.GroupByExpressions.Insert(i++,
                        AddGroupByExpression("Centro de Custo.", "CostCenter", "<b>{0}</b>"));

            if (chkGrouping.Items[3].Selected)
                grdPurchaseReq.MasterTableView.GroupByExpressions.Insert(i++,
                        AddGroupByExpression("Categoria.", "CategoryName", "<b>{0}</b>"));
        }

        private static GridGroupByExpression AddGroupByExpression(string headerText, string column, string formatString)
        {
            var expression = new GridGroupByExpression();

            //
            // Select Field
            //
            var gridSelectByField = new GridGroupByField
            {
                FieldName = column,
                HeaderText = headerText,
                FormatString = formatString
            };
            expression.SelectFields.Add(gridSelectByField);

            //
            // Group Field
            //
            var gridGroupByField = new GridGroupByField();
            gridGroupByField.FieldName = column;
            expression.GroupByFields.Add(gridGroupByField);
            return expression;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            purchaseManager = new PurchaseManager(this);
            var purchaseOrderManager = new PurchaseOrderManager(this);

            if (!String.IsNullOrEmpty(Request["request"]))
                foreach (string requestId in Request["request"].Split(','))
                {
                    var purchaseReq = purchaseOrderManager.GetPurchaseRequest(Convert.ToInt32(requestId));
                    foreach (var item in purchaseReq.PurchaseRequestItems)
                        if (item.PurchaseOrderItemId.HasValue)
                        {
                            ShowError("Não pode excluir uma requisição que há processo de compra associado!");
                            return;
                        }

                    purchaseManager.DeletePurchaseRequest(Convert.ToInt32(requestId));
                }

            if (!String.IsNullOrEmpty(Request["requestItems"]))
                foreach (string requestItem in Request["requestItems"].Split(','))
                {
                    string[] item = requestItem.Trim().Split('|');

                    purchaseManager.DeletePurchaseRequestItem(Convert.ToInt32("0" + item[6]));
                }
            grdPurchaseReq.DataBind();
        }
    }
}