using System.Web;
using System.Web.SessionState;

namespace InfoControl.Web
{
    public abstract class HttpDataHandler : DataAccessor, IHttpHandler, IRequiresSessionState
    {
        #region IHttpHandler Members

        public abstract bool IsReusable { get; }
        public abstract void ProcessRequest(HttpContext context);

        #endregion
    }
}