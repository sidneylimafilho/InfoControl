using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoControl.Web.Reporting
{
    public abstract class IReportItemBuilder
    {
        public RdlSchema.StyleType RowsStyle { get; set; }
        public RdlSchema.StyleType HeaderStyle { get; set; }
    }
}
