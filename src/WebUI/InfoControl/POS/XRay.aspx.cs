using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

//using Dundas.Gauges.WebControl;
using Dundas.Charting.WebControl;

public partial class InfoControl_POS_LightningX : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Vivina.Erp.BusinessRules.SaleManager saleManager = new Vivina.Erp.BusinessRules.SaleManager(this);
        //ChartConsumptionBySale.CircularGauges["circularGauge"].Pointers["pointer"].Value = saleManager.GetConsumptionBySale(Company.CompanyId);


    }
    protected void odsLightningx_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
}
