using System;
using System.Linq;

namespace Vivina.Erp.DataClasses
{

    public partial class PurchaseOrder
    {
        #region Properties
        private Quotation _quotation;
        public Quotation WinnerQuotation
        {
            get
            {
                return _quotation ??
                    (_quotation = Quotations.Where(x => x.SupplierId == Convert.ToInt32(this.SupplierId)).FirstOrDefault());

            }
        }

        #endregion


    }


    public partial class PurchaseOrderItem
    {
        #region Properties
        private QuotationItem _quotation;
        public QuotationItem WinnerQuotationItem
        {
            get
            {
                return _quotation ??
                    (_quotation = QuotationItems.Where(x => x.SupplierId == PurchaseOrder.SupplierId).FirstOrDefault());

            }
        }

        #endregion


    }
}