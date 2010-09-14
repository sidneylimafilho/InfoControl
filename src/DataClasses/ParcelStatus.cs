using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vivina.Erp.DataClasses
{
    public partial class ParcelStatus
    {
        public const int EXPIRED = 1;
        public const int EXPIRING = 2;
        public const int OPEN = 3;
        public const int CLOSED = 4;
        public const int ALL = 0;
    }
}
