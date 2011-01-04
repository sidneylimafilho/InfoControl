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
        text = new System.Text.RegularExpressions.Regex(" +").Replace(text, " ");

        // Remove comments in /**/ style
        //text = new System.Text.RegularExpressions.Regex(@"/\*(.|\n)*\*/[^*]").Replace(text, "");


        return text;
    }



    public string GetFileContentInCache(string path)
    {
        if (Cache[path] == null)
        {
            var text = RemoveComments(System.IO.File.ReadAllText(Server.MapPath(path)));

            Cache.Add(path, text,
                      new CacheDependency(Server.MapPath(path)),
                      System.Web.Caching.Cache.NoAbsoluteExpiration,
                      System.Web.Caching.Cache.NoSlidingExpiration,
                      CacheItemPriority.Normal,
                      (string key, object value, CacheItemRemovedReason reason) =>
                          GetFileContentInCache(key));

            //
            // Marca o arquivo como modificado assim o servidor não retornará um http 304 
            //
            System.IO.File.SetLastWriteTime(Request.PhysicalPath, DateTime.UtcNow);
        }
        return Cache[path].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = Request["type"] ?? "text/javascript";
        Response.Clear();

        var extension = Request["ext"] ?? "";
        var basePath = Request["base"] ?? "";
        var files = (Request["f"] ?? "").Split(',');
        foreach (var filename in files)
            if (!String.IsNullOrEmpty(filename))
                Response.Write( GetFileContentInCache(basePath + filename.Trim() + extension) );

        Response.End();

    }
</script>

