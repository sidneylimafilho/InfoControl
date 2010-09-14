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
    public partial class LegalEntityProfile
    {
        public static readonly string CnpjLiberalProfessional = "99.999.999/0001-99";

        /// <summary>
        /// Indicates whether the company registered has CNPJ as 99.999.999/0001-99
        /// </summary>
        public bool IsLiberalProfessional
        {
            get { return CNPJ == CnpjLiberalProfessional; }
        }
    }
}
