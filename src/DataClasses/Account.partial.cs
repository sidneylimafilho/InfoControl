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
    public partial class Account : INotifyPropertyChanging, INotifyPropertyChanged
    {
        /// <summary>
        /// AccountNumberDigit
        /// </summary>
        public string AccountNumberDigit
        {

            get
            {
                if (!String.IsNullOrEmpty(AccountNumber))
                    return AccountNumber.Substring(AccountNumber.Length - 1, 1)[0].ToString();
                else
                    throw new FormatException();
            }
        }

        /// <summary>
        /// AgencyDigit
        /// </summary>
        public string AgencyDigit
        {

            get
            {
                if (!String.IsNullOrEmpty(Agency))
                    return AccountNumber.Substring(Agency.Length - 1, 1)[0].ToString();
                else
                    throw new FormatException();
            }
        }

    }
}
