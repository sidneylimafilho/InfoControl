using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using InfoControl;
using InfoControl.IO;
using InfoControl.Data;
using InfoControl.Web;
using InfoControl.Web.Reporting;



public partial class ReportGenerator : ReportGeneratorPage
{
    Hashtable steps = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        BlockContextMenu = false;
        base.Wizard = wzdMain;
        if (!IsPostBack)
        {
            wzdMain.ActiveStepIndex = 0;
            Session["ReportSettings"] = null;
        }
    }

       
}

