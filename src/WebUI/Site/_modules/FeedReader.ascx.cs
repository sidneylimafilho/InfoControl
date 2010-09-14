using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web.Caching;
using System.Web.UI;
using System.Xml.Linq;
using InfoControl.Data;

public partial class Site_FeedReader : Vivina.Erp.SystemFramework.UserControlBase
{
    private bool isTwitterFeed;
    public string queryString;

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string FeedUrl { get; set; }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public int Quantidade { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        isTwitterFeed = FeedUrl.ToLower().Contains("twitter.com");

        Quantidade = Quantidade > 0 ? Quantidade : 15;

        if (!String.IsNullOrEmpty(FeedUrl))
            if (Visible)
                pageList.DataSource = GetFeed();

        pageList.DataBind();
    }

    public DataTable GetFeed()
    {
        //
        // Return in cache, for growing performance
        //
        if (Cache[FeedUrl] != null)
            return Cache[FeedUrl] as DataTable;

        var dt = new DataTable();

        //
        // else retrieve in Twitter and caching
        //
        try
        {
            XDocument tweetResults = XDocument.Load(FeedUrl);
            var q = from tweet in tweetResults.Descendants("item").Take(Quantidade)
                    select new
                               {
                                   Title = isTwitterFeed ? "" : (string) tweet.Element("title"),
                                   Html = isTwitterFeed ? FixTwitterTitle((string) tweet.Element("title")) : "",
                                   //(string)tweet.Element("description"),
                                   DatePublished = (DateTime) tweet.Element("pubDate"),
                                   Id = (string) tweet.Element("guid"),
                                   Link = (string) tweet.Element("link"),
                                   Author = (string) tweet.Element("author")
                               };

            dt = q.ToDataTable(true);
            Cache.Insert(FeedUrl, dt, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
        }
        catch (Exception ex)
        {
            ex.GetBaseException();
        }

        return dt;
    }

    private string FixTwitterTitle(string text)
    {
        if (isTwitterFeed)
        {
            string[] list = (text ?? "").Split(' ');

            for (int i = 0; i < list.Length; i++)
            {
                if (i == 0 && list[i].EndsWith(":"))
                    list[i] = "";

                if (list[i].StartsWith("@"))
                    list[i] = "<a href='http://twitter.com/" + list[i].Replace("@", "") + "'>" + list[i] + "</a>";

                if (list[i].StartsWith("http"))
                    list[i] = "<a href='" + list[i] + "'>" + list[i] + "</a>";
            }

            return string.Join(" ", list);
        }
        else
            return text;
    }
}