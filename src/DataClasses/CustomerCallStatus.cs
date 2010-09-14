using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vivina.Erp.DataClasses
{
    public partial class CustomerCallStatus
    {
        public const int New = 1;
        public const int Closed = 2;
        public const int Waiting = 3;
        // the const open, represents the New or Waiting
        public const int Open = 4;
    }
}
