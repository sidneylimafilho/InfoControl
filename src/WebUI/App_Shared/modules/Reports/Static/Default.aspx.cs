using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class Company_Reports_Static_Default : InfoControl.Web.UI.DataPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Repeater1.DataSourceID = "";
        
        txtReporter.Attributes["onkeyup"] = "__doPostBack('ctl00$ContentPlaceHolder$txtReporter','')";
        if (txtReporter.Text.Length > 3)
            Repeater1.DataSourceID = "BusinessManagerDataSource1";
    }

    protected void BusinessManagerDataSource1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["name"] = txtReporter.Text;
    }
}
