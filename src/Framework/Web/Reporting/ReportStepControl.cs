using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Resources;
using System.Reflection;

using InfoControl.Data;
using InfoControl.Web;
using InfoControl.Web.UI;

namespace InfoControl.Web.Reporting
{
    public class ReportStepControl : DataUserControl
    {
        public ReportStepControl()
        {
        }

        #region Properties
        new public ReportGeneratorPage Page
        {
            get
            {
                return (base.Page as ReportGeneratorPage);
            }
            set
            {
                base.Page = value;
            }
        }

        #endregion

    }
}
