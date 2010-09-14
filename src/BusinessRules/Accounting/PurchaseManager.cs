using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules.Accounting
{
    public class PurchaseManager : BusinessManager<InfoControlDataContext>
    {
        public PurchaseManager(IDataAccessor container)
            : base(container)
        {
        }

        /// <summary>
        /// This method inserts a Purchase Request and its Items
        /// </summary>
        /// <param name="purchaseRequest">Can't be null</param>
        /// <param name="purchaseRequestItems">Can't be null</param>
        public void SavePurchaseRequest(PurchaseRequest purchaseRequest, List<PurchaseRequestItem> purchaseRequestItems)
        {
            int purchaseRequestId = purchaseRequest.PurchaseRequestId;
            if (purchaseRequestId > 0)
            {
                purchaseRequest =
                    DbContext.PurchaseRequests.Where(p => p.PurchaseRequestId == purchaseRequestId).FirstOrDefault();
                DbContext.PurchaseRequestItems.DeleteAllOnSubmit(purchaseRequest.PurchaseRequestItems);
            }

            purchaseRequest.ModifiedDate = DateTime.Now;

            DbContext.PurchaseRequests.InsertOnSubmit(purchaseRequest);
            DbContext.SubmitChanges();

            foreach (PurchaseRequestItem item in purchaseRequestItems)
            {
                if (Convert.ToBoolean(item.Product.RequiresAuthorization))
                {
                    var humanResourcesManager = new HumanResourcesManager(this);
                    User user = humanResourcesManager.GetCentralBuyer(purchaseRequest.CompanyId);
                    new AlertManager(this).InsertAlert(
                        user.UserId,
                        "Há requisição de produtos centralizados, <a href='Purchasing/PurchaseRequests.aspx' target='content'>clique aqui</a> para analisá-la!");

                    return;
                }

                item.PurchaseRequestId = purchaseRequest.PurchaseRequestId;
                item.CompanyId = purchaseRequest.CompanyId;
                //item.Detach();
                DbContext.PurchaseRequestItems.InsertOnSubmit(item.Detach());
            }
            DbContext.SubmitChanges();
        }

        public void DeletePurchaseRequest(int purchaseRequestId)
        {
            DbContext.PurchaseRequestItems.DeleteAllOnSubmit(
                DbContext.PurchaseRequestItems.Where(ri => ri.PurchaseRequestId == purchaseRequestId));
            DbContext.PurchaseRequests.DeleteAllOnSubmit(
                DbContext.PurchaseRequests.Where(ri => ri.PurchaseRequestId == purchaseRequestId));
            DbContext.SubmitChanges();
        }

        public void DeletePurchaseRequestItem(int purchaseRequestItemId)
        {
            DbContext.PurchaseRequestItems.DeleteAllOnSubmit(
                DbContext.PurchaseRequestItems.Where(ri => ri.PurchaseRequestItemId == purchaseRequestItemId));
            DbContext.SubmitChanges();
        }

        public IQueryable<PurchaseRequestItem> GetPurchaseRequestItems(int purchaseRequestId)
        {
            return DbContext.PurchaseRequestItems.Where(pr => pr.PurchaseRequestId == purchaseRequestId);
        }

        public IList GetPurchaseRequestItemsByCompany(Int32 companyId, Int32 employeeId, String sortExpression,
                                                      Int32 startRowIndex, Int32 maximumRows)
        {
            Employee employee = new HumanResourcesManager(this).GetEmployee(companyId, employeeId);

            if (employee == null)
                return null;

            IQueryable<PurchaseRequestItem> purchaseRequestItems = DbContext.PurchaseRequestItems;

            if (!employee.PurchaseCeilingValue.HasValue) //if he can't buy just get his itens
                purchaseRequestItems =
                    DbContext.PurchaseRequestItems.Where(pi => pi.PurchaseRequest.EmployeeId == employeeId);

            //group of products
            var products = from purchaseRequestItem in purchaseRequestItems
                           join purchaseRequest in DbContext.PurchaseRequests on purchaseRequestItem.PurchaseRequestId
                               equals purchaseRequest.PurchaseRequestId
                           join purchaseOrder in DbContext.PurchaseOrders on purchaseRequestItem.PurchaseOrderId equals
                               purchaseOrder.PurchaseOrderId into gPurchaseOrder
                           from purchaseOrder in gPurchaseOrder.DefaultIfEmpty()
                           join purchaseOrderStatus in DbContext.PurchaseOrderStatus on
                               purchaseOrder.PurchaseOrderStatusId equals purchaseOrderStatus.PurchaseOrderStatusId into
                               gPurchaseOrderStatus
                           from purchaseOrderStatus in gPurchaseOrderStatus.DefaultIfEmpty()
                           join address in DbContext.Addresses on purchaseRequest.PostalCode equals address.PostalCode
                               into gAddress
                           from address in gAddress.DefaultIfEmpty()
                           join product in DbContext.Products on purchaseRequestItem.ProductId equals product.ProductId
                           where purchaseRequest.CompanyId == companyId && !purchaseRequest.ProductId.HasValue
                           orderby purchaseRequest.PurchaseRequestId descending
                           select new
                                      {
                                          product.ProductId,
                                          Name =
                               product.Name + " - " + (purchaseRequestItem.ProductPackage.Name ?? "") + " - " +
                               (purchaseRequestItem.ProductManufacturer.Name ?? ""),
                                          purchaseRequest.PurchaseRequestId,
                                          purchaseRequestItem.PurchaseRequestItemId,
                                          purchaseRequestItem.ProductManufacturerId,
                                          purchaseRequestItem.ProductPackageId,
                                          purchaseOrder.PurchaseOrderCode,
                                          PurchaseOrderId = (int?) purchaseOrder.PurchaseOrderId,
                                          PurchaseOrderStatus = purchaseOrderStatus.Name,
                                          City = address.NeighborhoodEntity.City.Name,
                                          CityId = (int?) address.NeighborhoodEntity.City.CityId,
                                          CostCenter = purchaseRequest.CostCenter.Name,
                                          CategoryName = product.Category.Name,
                                          purchaseRequestItem.Amount
                                      };

            var products2 = from purchaseRequest in DbContext.PurchaseRequests
                            join purchaseOrder in DbContext.PurchaseOrders on
                                purchaseRequest.PurchaseRequestItems.FirstOrDefault().PurchaseOrderId equals
                                purchaseOrder.PurchaseOrderId into gPurchaseOrder
                            from purchaseOrder in gPurchaseOrder.DefaultIfEmpty()
                            join purchaseOrderStatus in DbContext.PurchaseOrderStatus on
                                purchaseOrder.PurchaseOrderStatusId equals purchaseOrderStatus.PurchaseOrderStatusId
                                into gPurchaseOrderStatus
                            from purchaseOrderStatus in gPurchaseOrderStatus.DefaultIfEmpty()
                            join address in DbContext.Addresses on purchaseRequest.PostalCode equals address.PostalCode
                                into gAddress
                            from address in gAddress.DefaultIfEmpty()
                            join product in DbContext.Products on purchaseRequest.ProductId equals product.ProductId
                            where purchaseRequest.CompanyId == companyId && purchaseRequest.ProductId.HasValue
                            orderby purchaseRequest.PurchaseRequestId descending
                            select new
                                       {
                                           product.ProductId,
                                           product.Name,
                                           purchaseRequest.PurchaseRequestId,
                                           PurchaseRequestItemId = 0,
                                           ProductManufacturerId = (int?) null,
                                           ProductPackageId = (int?) null,
                                           purchaseOrder.PurchaseOrderCode,
                                           PurchaseOrderId = (int?) purchaseOrder.PurchaseOrderId,
                                           PurchaseOrderStatus = purchaseOrderStatus.Name,
                                           City = address.NeighborhoodEntity.City.Name,
                                           CityId = (int?) address.NeighborhoodEntity.City.CityId,
                                           CostCenter = purchaseRequest.CostCenter.Name,
                                           CategoryName = product.Category.Name,
                                           Amount = (decimal?) purchaseRequest.Amount
                                       };
            var list = products.ToList();
            list.AddRange(products2.ToList());
            return list;
        }

        public Int32 GetPurchaseRequestItemsByCompanyCount(Int32 companyId, Int32 employeeId, String sortExpression,
                                                           Int32 startRowIndex, Int32 maximumRows)
        {
            return
                GetPurchaseRequestItemsByCompany(companyId, employeeId, sortExpression, startRowIndex, maximumRows).Cast
                    <IQueryable>().Count();
        }

        /// <summary>
        /// This method returns a PurchaseOrderQuotedItem by Product
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        //public PurchaseOrderQuotedItem PurchaseOrderQuotedItemByProduct(Int32 companyId, Int32 productId)
        //{
        //    IQueryable<PurchaseOrderQuotedItem> query = from purchaseRequestItem in DbContext.PurchaseRequestItems
        //                                                where purchaseRequestItem.ProductId == productId
        //                                                group purchaseRequestItem by purchaseRequestItem.ProductId
        //                                                into gPurchaseRequestItem
        //                                                    select new PurchaseOrderQuotedItem(
        //                                                        gPurchaseRequestItem.FirstOrDefault().Product.Name, 
        //                                                        Convert.ToInt32(gPurchaseRequestItem.Sum(x => x.Amount)), 
        //                                                        "", 0, productId);
        //    return query.FirstOrDefault();
        //}
        public IQueryable SearchSupplierCanditate(int purchaseOrderId)
        {
            return from quotationItem in DbContext.QuotationItems
                   join quotation in DbContext.Quotations on quotationItem.QuotationId equals quotation.QuotationId
                   where
                       (
                           from purchaseOrderItem in DbContext.PurchaseOrderItems
                           where purchaseOrderItem.PurchaseOrderId == purchaseOrderId
                           select purchaseOrderItem
                       ).Any(p => p.ProductId == quotationItem.PurchaseOrderItem.ProductId)
                   group quotation by quotation.Supplier
                   into grouping
                       select new
                                  {
                                      grouping.Key.SupplierId,
                                      Name = grouping.Key.LegalEntityProfile != null
                                                 ? grouping.Key.LegalEntityProfile.CompanyName
                                                 : grouping.Key.Profile.Name
                                  };
        }
    }
}