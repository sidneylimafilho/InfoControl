using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl.Runtime;

namespace InfoControl.Web
{
    public class CacheHandler : IHttpHandler
    {
        public CacheHandler()
        {
            this.css = " <style> .table { font-family:Verdena; font-size:20px; background-color:#FFFF99; }\r\n          .tableDetail { font-family:Verdena; font-size:20px; background-color:#99CCFF;  }  </style>  ";
            this.table = new TableHelper();
        }

        private void DisplayCachedItems(HttpContext context, string key)
        {
            TableHelper helper1 = new TableHelper();
            helper1.CssClass = "tableDetail";
            TableRow row1 = helper1.CreateRow();
            TableHeaderRow row2 = this.table.CreateHeaderRow();
            helper1.AddCell(row1, context.Cache[key].SerializeToXml());
            helper1.Rows.Add(row1);
            helper1.Rows.AddAt(0, row2);
            this.RenderTable(context, helper1);
        }

        public void ProcessRequest(HttpContext context)
        {
            string text1 = string.Empty;
            if (((context.Request.QueryString["cacheKey"] != null) && (context.Request.QueryString["remove"] != null)) && Convert.ToBoolean(context.Request.QueryString["remove"]))
            {
                text1 = context.Request.QueryString["cacheKey"];
                context.Cache.Remove(text1);
                this.RenderTable(context);
            }
            IDictionaryEnumerator enumerator1 = context.Cache.GetEnumerator();
            while (enumerator1.MoveNext())
            {
                TableRow row1 = this.table.CreateRow();
                this.table.AddCell(row1, enumerator1.Key.ToString());
                this.table.AddCell(row1, enumerator1.Value.ToString());
                this.table.AddCell(row1, enumerator1.Value.GetType().Name);
                if (enumerator1.Value.SerializeToXml() != null)
                {
                    this.table.AddCell(row1, "View Details", "Cache.axd?cacheKey=" + enumerator1.Key.ToString());
                }
                else
                {
                    this.table.AddCell(row1, "Object cannot be serialized");
                }
                this.table.AddCell(row1, "Remove", "Cache.axd?cacheKey=" + enumerator1.Key.ToString() + "&remove=true");
                this.table.Rows.Add(row1);
            }
            TableHeaderRow row2 = this.table.CreateHeaderRow();
            this.table.AddHeaderCell(row2, "KEY");
            this.table.AddHeaderCell(row2, "VALUE");
            this.table.AddHeaderCell(row2, "TYPE");
            this.table.Rows.AddAt(0, row2);
            this.table.CssClass = "table";
            this.RenderTable(context);
            if ((context.Request.QueryString["cacheKey"] != null) && (context.Request.QueryString["remove"] == null))
            {
                text1 = context.Request.QueryString["cacheKey"];
                context.Cache[text1].SerializeToXml();
                this.DisplayCachedItems(context, text1);
            }
        }

        private void RenderTable(HttpContext context)
        {
            StringWriter writer1 = new StringWriter();
            HtmlTextWriter writer2 = new HtmlTextWriter(writer1);
            this.table.RenderControl(writer2);
            context.Response.Write("<html><head>");
            context.Response.Write(this.css);
            context.Response.Write("</head><body>");
            context.Response.Write(writer1.ToString());
            context.Response.Write("</body></html>");
        }

        private void RenderTable(HttpContext context, Table table)
        {
            StringWriter writer1 = new StringWriter();
            HtmlTextWriter writer2 = new HtmlTextWriter(writer1);
            table.RenderControl(writer2);
            context.Response.Write("<div class='tableDetail'>");
            context.Response.Write(writer1.ToString());
            context.Response.Write("</div>");
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        private string css;
        private TableHelper table;
    }
}

