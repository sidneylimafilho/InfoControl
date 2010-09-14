using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace InfoControl.Web.Reporting.DataClasses
{
    partial class ReportFilterType
    {
        public enum Text
        {
            StartWith = 1,
            Contains = 2,
            EndWith = 3,
            Equals = 4
        }
        public enum Integer
        {
            Equals = 5,
            NotEquals = 6,
            LessThan = 7,
            GreaterThan = 8,
            LessOrEqualsThan = 9,
            GreaterOrEqualsThan = 10
        }

        public enum Date
        {
            Equals = 11,
            NotEquals = 12,
            LessThan = 13,
            GreaterThan = 14,
            LessOrEqualsThan = 15,
            GreaterOrEqualsThan = 16
        }

        public enum ForeignKey
        {
            List = 17
        }



        //public static int 11	igual a	3	[=Column=] = [=Parameter=]
        //public static int 12	diferente de	3	[=Column=] <> [=Parameter=]
        //public static int 13	menor que	3	[=Column=] < [=Parameter=]
        //public static int 14	maior que	3	[=Column=] > [=Parameter=]
        //public static int 15	menor ou igual a	3	[=Column=] <= [=Parameter=]
        //public static int 16	maior ou igual a	3	[=Column=] >= [=Parameter=]


    }
}
