using System;
using System.Data.Linq;
using System.IO;
using System.Web;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;

public partial class Company_ImageHandler : Vivina.Erp.SystemFramework.PageBase
{
    protected override void OnPreInit(EventArgs e)
    {
        if (Company != null)
        {
            if (Company.CompanyConfiguration == null)
                Company.CompanyConfiguration = new CompanyConfiguration();

            if (Company.CompanyConfiguration.Logo == null)
                Company.CompanyConfiguration.Logo =
                    new Binary(File.ReadAllBytes(Server.MapPath("~/App_Shared/themes/glasscyan/Menu/blank.gif")));

            Response.Clear();
            Response.Cache.SetCacheability(HttpCacheability.Server);
            Response.Cache.SetMaxAge(new TimeSpan(24, 0, 0));
            Response.Cache.SetExpires(DateTime.Now.AddDays(1));
            Response.Cache.VaryByParams.IgnoreParams = true;
            Response.AddHeader("cache-control", "private");
            Response.ContentType = "image/gif";
            Response.BinaryWrite(Company.CompanyConfiguration.Logo.ToArray());
            Response.End();
        }
    }
}