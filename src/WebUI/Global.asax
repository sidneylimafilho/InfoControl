<%@ Application Language="C#" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.Services" %>
<%@ Import Namespace="System.Web.Services" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="InfoControl.Web.ScheduledTasks" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        Scheduler.Instance.Start();

        // Start MVC Routing
        RouteTable.Routes.MapRoute(
            "Default",
            "controller/{controller}/{action}/{id}",
            new { controller = "SearchService", action = "HelloWorld", id = "" }
        );
        
       

    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
HttpContext ctx = HttpContext.Current;
        Exception exception = ctx.Server.GetLastError();

        string errorInfo =
                "<br>Offending URL: " + ctx.Request.Url.ToString() +
                "<br>Source: " + exception.Source +
                "<br>Message: " + exception.Message +
                "<br>Stack trace: " + exception.StackTrace;
        System.IO.File.AppendAllText("c:\\error.log", errorInfo);

        ctx.Server.ClearError();

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        if (Application["Session_End"] != null)
            (Application["Session_End"] as EventHandler)(this, e);

        using (MembershipManager membershipManager = new MembershipManager(null))
            membershipManager.Logoff(System.Threading.Thread.CurrentPrincipal as AccessControlPrincipal);


    }
       
</script>

