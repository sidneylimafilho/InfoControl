using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System;

namespace Vivina.Erp.DataClasses
{
    public partial class User : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public string Name
        {
            get
            {
                return Profile != null ? Profile.Name : UserName;
            }
        }

        public bool IsEmployee
        {
            get
            {
                return Profile.Employees.Any();
            }
        }

        public Employee AsEmployee
        {
            get
            {
                return Profile.Employees.FirstOrDefault();
            }
        }

    }
}
