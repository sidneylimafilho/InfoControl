<%@ WebService Language="C#" Class="OutdoorConfig" %>
using System;
using System.Web;
using System.Web.Caching;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Specialized;
using System.Web.Services;
using System.Web.Services.Protocols;


/// <summary>
/// Summary description for OutdoorConfig
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class OutdoorConfig : System.Web.Services.WebService
{
    [WebMethod]
    public void GetNextOutdoor()
    {
        if (Context.Cache["ListOutdoor"] == null)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath("ListOutdoor.xml"));
            Context.Cache.Insert("ListOutdoor", ds.Tables[0], new CacheDependency(Server.MapPath("ListOutdoor.xml")));
        }


        DataTable dt = Context.Cache["ListOutdoor"] as DataTable;

        Random r = new Random();
        int ID = r.Next(dt.Rows.Count - 1);

        Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        Context.Response.Write(dt.Rows[ID]["outdoor_Text"].ToString());

    }
}
