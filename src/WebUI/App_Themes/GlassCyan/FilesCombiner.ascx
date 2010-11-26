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

        Include("global.cssx");
        Include("text.cssx");
        
        Include("../_global/global.cssx");
        Include("../_global/controls/ToolTip/ToolTip.cssx");
        Include("../_global/controls/Alert/Alert.cssx");
        Include("../_global/Leafbox/Cyan/Leafbox.cssx");
        Include("../_global/Leafbox/Orange/Leafbox.cssx");
        Include("../_global/Leafbox/Purple/Leafbox.cssx");
        Include("../_global/Leafbox/Yellow/Leafbox.cssx");
        
        Include("../../App_Shared/js/jquery.jgrowl.css");                
        
        Include("button/button.cssx");

        Include("calendar/calendar.cssx");
        Include("calendar/jquery.ui.core.cssx");
        Include("calendar/jquery.ui.theme.cssx");        
        
        Include("gridview/gridview.cssx");
        
        Include("menu/menu.cssx");
        
        Include("numericUpDown.cssx");

        Include("RadScheduler/RadScheduler.cssx");
                
        Include("Rating/Rating.cssx");
        
        Include("TabStrip/Tabs.cssx");
        
        Include("TextBox/textcontrol.cssx");
        
        Include("Tooltip/tooltip.cssx");
        
        Include("validator/validator.cssx");        

        Response.End();

    }
</script>

