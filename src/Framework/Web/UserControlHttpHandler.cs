using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.SessionState;
using System.Web.UI;
using InfoControl.Web.UI;
using System.Reflection;
using System.Net;

namespace InfoControl.Web
{
    public class UserControlHttpHandler : HttpDataHandler
    {

        public override bool IsReusable
        {
            get { return false; }
        }

        public override void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string filePath = context.Server.MapPath(request.FilePath);
            if (System.IO.File.Exists(filePath))
            {
                var page = new DataPage();

                var lastModified = System.IO.File.GetLastWriteTimeUtc(filePath);
                if (request.Headers["If-Modified-Since"] != lastModified.ToRFC1123())
                {

                    page.Controls.Add(page.LoadControl(request.FilePath));

                    //
                    // Set internal variables that verify whether the page contains a form
                    // 
                    page.GetType().GetMethod("OnFormRender", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(page, null);

                    //
                    // Set Cache
                    //                    
                    response.Cache.SetLastModified(lastModified);

                    context.Server.Execute(page, response.Output, true);
                }
                else if (!page.IsPostBack)
                    response.StatusCode = (int)HttpStatusCode.NotModified;
            }
        }
    }
}