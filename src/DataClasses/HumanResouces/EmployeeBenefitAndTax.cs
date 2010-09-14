using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Collections;
using System.Collections.Generic;


namespace Vivina.Erp.DataClasses
{
    public partial class Employee
    {
        private Dictionary<string, decimal> _CurrentEvents;
        public Dictionary<string, decimal> CurrentEvents
        {
            get
            {
                return this._CurrentEvents ?? (this._CurrentEvents = new Dictionary<string, decimal>());
            }
        }
    }


}
