using System;
using System.Linq;
using System.Text;
using System.Web;
using InfoControl;
using Vivina.Erp.BusinessRules.Comments;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;

namespace Vivina.Erp.WebUI.Site
{
    public class SiteRss : SitePageBase
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            Response.ContentType = "text/xml";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(600));
            Response.Cache.SetCacheability(HttpCacheability.Public);

            Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            Response.Write(FormatHeader(WebPage));
            Response.Write("<channel>");
            Response.Write(FormatBlogSettings(WebPage));

            var query = from pages in WebPage.WebPages
                        where pages.IsPublished
                        orderby pages.PublishedDate descending
                        select pages;

            var limit = !String.IsNullOrEmpty(Request["limit"]) ? Convert.ToInt32(Request["limit"]) : 10;

            foreach (WebPage childWebPage in query.Take(limit))
                Response.Write(FormatCData(childWebPage));

            Response.Write("</channel></rss>");

            Response.End();
        }

        private string FormatCData(WebPage page)
        {
            string link = Request.Url.Scheme + "://" + Request.Url.Host + ResolveUrl(page.Url);
            string creator = page.User.UserName + " (" + page.User.Profile.AbreviatedName;
            var builder = new StringBuilder();
            builder.Append("<item>");
            builder.Append("<author>" + creator + ")</author>");
            // builder.Append("<dc:creator>" + creator + ")</dc:creator>");
            // builder.Append("<managingEditor>" + creator + ")</managingEditor>");
            builder.Append("<title><![CDATA[ " + page.Name + " ]]> </title>");
            builder.Append("<link>" + link + "</link>");
            builder.Append("<comments>" + link + "?#comments</comments>");
            builder.Append("<guid isPermaLink=\"true\">" + link + "</guid>");
            builder.Append("<description><![CDATA[ " + page.Description + " ]]> </description>");
            builder.Append("<pubDate>" + page.PublishedDate.Value.ToUniversalTime().ToString("R") + "</pubDate>");

            //foreach (PageTag tag in page.PageTags)
            //    builder.Append("<tag><![CDATA[ " + tag.Name + " ]]> </tag>");

            var commentsCount = new CommentsManager(this).GetComments(page.WebPageId, "comments.aspx").Count();
            builder.Append("<slash:comments>{0}</slash:comments>".FormatAll(commentsCount));

            builder.Append("</item>");

            return builder.ToString();
        }

        private string FormatHeader(WebPage page)
        {
            return "<rss version=\"2.0\" " +
                   "xmlns:content=\"http://purl.org/rss/1.0/modules/content/\" " +
                   "xmlns:wfw=\"http://wellformedweb.org/CommentAPI/\" " +
                   "xmlns:dc=\"http://purl.org/dc/elements/1.1/\" " +
                   "xmlns:atom=\"http://www.w3.org/2005/Atom\" " +
                   "xmlns:sy=\"http://purl.org/rss/1.0/modules/syndication/\" " +
                   "xmlns:slash=\"http://purl.org/rss/1.0/modules/slash/\" " +
                   "xmlns:media=\"http://search.yahoo.com/mrss/\" >";
        }

        private string FormatBlogSettings(WebPage page)
        {
            var builder = new StringBuilder();
            // builder.Append("<item>");
            builder.Append("<title>" + WebPage.Name + "</title>");
            builder.Append("<description><![CDATA[" + WebPage.Description + "]]></description>");
            builder.Append("<link>" + Request.Url.Scheme + "://" + Request.Url.Host + ResolveUrl(WebPage.Url) + "</link>");
            //builder.Append("<generator>Vivina InfoControl</generator>");
            //builder.Append("<language>pt-br</language>");
            //builder.Append("<sy:updatePeriod>dialy</sy:updatePeriod>");
            //builder.Append("<sy:updateFrequency>1</sy:updateFrequency>");
            //builder.Append("</item>");
            return builder.ToString();
        }
    }
}