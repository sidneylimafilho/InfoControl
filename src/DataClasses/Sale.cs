using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vivina.Erp.DataClasses
{
    public partial class Sale
    {
        public FinancierOperation FinancierOperation
        {
            get
            {
                return this.Invoice.Parcels.First().FinancierOperation;
            }
        }
    }
}
