using System;
using System.Data;

using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;

namespace Vivina.Erp.SystemFramework
{
    public partial class UserControlBase : InfoControl.Web.UI.DataUserControl
    {
        new public PageBase Page
        {
            get
            {
                return (PageBase)base.Page;
            }
        }
    }


    public partial class UserControlBase<T> : Vivina.Erp.SystemFramework.UserControlBase
        where T : Vivina.Erp.SystemFramework.PageBase        
    {
        new public T Page
        {
            get
            {
                return (T)base.Page;
            }
        }
    }
}





