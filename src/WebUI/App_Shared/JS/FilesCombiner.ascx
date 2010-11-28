<%@ Control Language="C#" AutoEventWireup="true" %>

<script runat="server">
    public string RemoveComments(string text)
    {
        System.Text.RegularExpressions.Regex regex;

        // Remove comments in // style
        //text = new System.Text.RegularExpressions.Regex("([^\"]\\/\\/.*)").Replace(text, "");

        // Remove line break
        //text = new System.Text.RegularExpressions.Regex("\\r|\\n|\\t").Replace(text, "");

        // Remove exceeding whitespaces
        //text = new System.Text.RegularExpressions.Regex(" +").Replace(text, " ");

        // Remove comments in /**/ style
        //text = new System.Text.RegularExpressions.Regex(@"/\*(.|\n)*\*/[^*]").Replace(text, "");


        return text;
    }

    public string GetFileContentInCache(string path)
    {
        if (Cache[path] == null)
        {
            var text = System.IO.File.ReadAllText(Server.MapPath(path));
            text = RemoveComments(text);
            Cache.Add(path, text, new CacheDependency(Server.MapPath(path)),
                System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration,
                CacheItemPriority.Normal, (string key, object value, CacheItemRemovedReason reason) => { GetFileContentInCache(key); });
        }
        return Cache[path].ToString();
    }

    public void Include(string path)
    {
        Response.Write(GetFileContentInCache(path));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = Request["type"] ?? "text/javascript";
        Response.Clear();

        Include("jquery.js");

        Include("jquery.cookies.js");
        Include("jquery.dimensions.js");
        Include("jquery.template.js");

        Include("jquery.jGrowl.js");
        Include("jquery.meioMask.js");
        Include("jquery.validate.js");
        Include("jquery.tooltip.js");
        Include("jquery.serializer.js");
        Include("jquery.template.js");

        Include("jquery.UI.core.js");
        Include("jquery.UI.widget.js");
        Include("jquery.UI.position.js");
        Include("jquery.UI.tabs.js");
        Include("jquery.UI.autocomplete.js");
        Include("jquery.UI.duallistbox.js");
        Include("jquery.ui.datepicker.js");
        Include("jquery.UI.htmlbox.js");

        Include("jquery.notification.js");

        Include("jquery.glob.js");
        Include("jquery.glob.pt-br.js");

        Include("smartclient/jquery.smartclient.js");


        Response.End();

    }
</script>

