<%@ Control Language="C#" AutoEventWireup="true" %>

<script runat="server">
    public string RemoveComments(string text)
    {
        System.Text.RegularExpressions.Regex regex;

        // Remove comments in // style
        text = new System.Text.RegularExpressions.Regex("(\\/)(\\/)(.*)").Replace(text, "");

        // Remove line break
        //text = new System.Text.RegularExpressions.Regex("\\r|\\n|\\t").Replace(text, "");

        // Remove exceeding whitespaces
        text = new System.Text.RegularExpressions.Regex(" +").Replace(text, " ");

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
        Response.ContentType = Request["type"] ?? "text/css";
        Response.Clear();

        Include("common.css");
        Include("text.css");
        Include("global.css");


        Include("Leafbox/Cyan/Leafbox.css");
        Include("Leafbox/Orange/Leafbox.css");
        Include("Leafbox/Purple/Leafbox.css");
        Include("Leafbox/Yellow/Leafbox.css");
        Include("controls/ToolTip/ToolTip.css");
        Include("controls/Alert/Alert.css");
        Include("controls/Notification/jquery.jgrowl.css");
        Include("controls/button/button.css");
        Include("controls/calendar/calendar.css");
        Include("controls/calendar/jquery.ui.core.css");
        Include("controls/calendar/jquery.ui.theme.css");
        Include("controls/gridview/gridview.css");
        Include("controls/menu/menu.css");
        Include("controls/numericUpDown/numericUpDown.css");
        Include("controls/RadScheduler/RadScheduler.css");
        Include("controls/RadPanelbar/Outlook.css");
        Include("controls/Rating/Rating.css");
        Include("controls/TabStrip/Tabs.css");
        Include("controls/TextBox/textcontrol.css");        
        Include("controls/validator/validator.css");

        Response.End();

    }
</script>

