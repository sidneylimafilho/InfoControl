using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vivina.Erp.DataClasses
{
    public partial class Sale
    {
        public Sale(int saleStatusId)
        {
            SaleStatusId = saleStatusId;
        }

        public decimal Total
        {
            get { return SaleItems.Sum(si => si.Quantity * si.UnitPrice) + FreightValue.Value; }
        }

        public FinancierOperation FinancierOperation
        {
            get { return this.Invoice.Parcels.First().FinancierOperation; }
        }
    }
}
