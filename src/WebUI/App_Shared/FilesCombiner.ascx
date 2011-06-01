<%@ Control Language="C#" AutoEventWireup="true" %><%@ Import Namespace="System.IO" %>

<script runat="server">

    public string RemoveComments(string text)
    {
        Regex regex;

        // Remove comments in // style
        //text = new System.Text.RegularExpressions.Regex("([^\"]\\/\\/.*)").Replace(text, "");

        // Remove line break
        //text = new System.Text.RegularExpressions.Regex("\\r|\\n|\\t").Replace(text, "");

        // Remove exceeding whitespaces
        //text = new Regex(" +").Replace(text, " ");

        // Remove comments in /**/ style
        //text = new System.Text.RegularExpressions.Regex(@"/\*(.|\n)*\*/[^*]").Replace(text, "");


        return text;
    }


    public string GetFileContentInCache(string path, string callerPath)
    {
        callerPath = callerPath ?? Request.PhysicalPath;

        if (Cache[path] == null)
        {
            string text = File.ReadAllText(Server.MapPath(path));

            Cache.Add(path, text,
                      new CacheDependency(Server.MapPath(path)),
                      Cache.NoAbsoluteExpiration,
                      Cache.NoSlidingExpiration,
                      CacheItemPriority.Normal,
                      (key, value, reason) => GetFileContentInCache(key, callerPath));

            //
            // Marca o arquivo como modificado assim o servidor não retornará um http 304 
            //
            File.SetLastWriteTime(callerPath, DateTime.UtcNow);
        }
        return Cache[path].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = Request["type"] ?? "text/javascript";
        Response.Clear();

        string extension = Request["ext"] ?? "";
        string basePath = Request["base"] ?? "";
        string[] files = (Request["f"] ?? "").Split(',');
        foreach (string filename in files)
            if (!String.IsNullOrEmpty(filename))
                Response.Write(RemoveComments(GetFileContentInCache(basePath + filename.Trim() + extension, Request.PhysicalPath)));

        Response.End();
    }

</script>

