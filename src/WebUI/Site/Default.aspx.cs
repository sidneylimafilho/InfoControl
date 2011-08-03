using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl.Web.Mail;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Comments;
using Vivina.Erp.BusinessRules.WebSites;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;

namespace Vivina.Erp.WebUI.Site
{
    public partial class SitePageBase : PageBase
    {
        private Customer _customer;
        private WebPage _page;

        #region Properties

        private Category _category;

        public WebPage WebPage
        {
            get { return _page; }
        }

        public Customer Customer
        {
            get
            {
                return _customer ??
                       (_customer =
                        new CustomerManager(this).GetCustomerByUserName(Company.CompanyId, User.Identity.UserName));
            }
        }

        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public string CategoryPath
        {
            get
            {
                if (!String.IsNullOrEmpty(Request["catid"]))
                    return Request["catid"];
                return "";
            }
        }

        public Category Category
        {
            get
            {
                if (_category == null)
                    if (!String.IsNullOrEmpty(CategoryPath))
                        using (var manager = new CategoryManager(null))
                            _category = manager.GetCategory(Company.CompanyId, CategoryPath);
                return _category;
            }
        }

        #endregion

        protected override void OnPreInit(EventArgs e)
        {
            Trace.Warn("SitePageBase", "Begin OnPreInit");
            base.OnPreInit(e);

            var manager = new SiteManager(this);

            //
            // Retrieves the correct _page
            //
            if (Request.RawUrl.EndsWith("default.aspx") || Request.RawUrl.EndsWith("home.aspx") ||
                Request.RawUrl.EndsWith("index.aspx") || Request.RawUrl.EndsWith("site/"))
            {
                _page = manager.GetMainPage(Company.CompanyId);
                MasterPageFile = Company.GetMasterPagePath(_page);
            }
            else if (!String.IsNullOrEmpty(Request["p"]))
            {
                string[] pageId = Request["p"].Split(',');

                int possibleInt = 0;
                if (!Int32.TryParse(pageId[0], out possibleInt))
                    possibleInt = Convert.ToInt32(pageId[0].DecryptFromHex());

                _page = manager.GetWebPage(Company.CompanyId, possibleInt);

                MasterPageFile = Company.GetMasterPagePath(_page);
            }
            else
            {
                _page = manager.WebPageNotFound();

                if (!Company.CompanyUsers.Any(user => user.UserId == User.Identity.UserId))
                    MasterPageFile = Company.GetMasterPagePath();
            }


            string filePath = Server.MapPath(Request.FilePath);
            DateTime lastWriteTime = File.GetLastWriteTimeUtc(filePath);
            DateTime lastModified = lastWriteTime < _page.ModifiedDate ? _page.ModifiedDate : lastWriteTime;

            //
            // Set Cache
            //                    
            Response.Cache.SetLastModified(lastModified);
            if (Request.Headers["If-Modified-Since"] == lastModified.ToRFC1123())
            {
                Response.StatusCode = 304;
                Response.End();
            }

            if ((Request["format"] ?? "").ToUpper() == "RAW")
            {
                Response.Clear();
                Response.Write(_page.Description);
                Response.End();
            }

            Trace.Warn("SitePageBase", "End OnPreInit");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //
            // Allow upload files
            //
            Form.Enctype = "multipart/form-data";

            ProcessPage();

            if (IsPostBack && Request["action"] == "FORM")
                ProcessCustomForm();

            if (IsPostBack && Request["action"] == "BudgetRequest")
                ProcessBudget();
        }

        public string GetQueryStringNavigateUrl(string field, string value)
        {
            var builder = new StringBuilder();

            if (Request.RawUrl.IndexOf("?") > 0)
                builder.Append(Request.RawUrl.Remove(Request.RawUrl.IndexOf("?")));
            else
                builder.Append(Request.RawUrl);

            builder.Append("?");
            foreach (string key in Request.QueryString.AllKeys)
            {
                if (!field.Equals(key, StringComparison.OrdinalIgnoreCase) && key != "catid")
                {
                    builder.Append(HttpUtility.UrlEncode(key));
                    builder.Append("=");
                    builder.Append(HttpUtility.UrlEncode(Request.QueryString[key]));
                    builder.Append("&");
                }
            }

            return (builder + HttpUtility.UrlEncode(field) + "=" + HttpUtility.UrlEncode(value));
        }

        private void CreateTitle()
        {
            Title = "";

            if (!_page.Name.ToLower().StartsWith("home") && !_page.Name.ToLower().StartsWith("default"))
            {
                WebPage parentPage = _page;
                while (parentPage != null && parentPage.WebPage1 != null)
                {
                    Title += parentPage.Name + " - ";
                    parentPage = parentPage.WebPage1;
                }
            }

            Title += (Company.LegalEntityProfile.FantasyName ?? Company.LegalEntityProfile.CompanyName);
        }

        public string GetCategoryBreadcrumbs(Category category)
        {
            if (category == null) return "";

            return GetCategoryBreadcrumbs(category.Category1) +
                   string.Format(@"<a href=""{0}"">{1}</a> > ",
                                 HttpUtility.UrlDecode(ResolveUrl(category.Url)),
                                 category.Name);
        }

        #region Actions

        private void ProcessBudget()
        {
            //
            // Verify if the request is not null ["nome"] and  ["email"]
            //
            if (String.IsNullOrEmpty(Request["nome"]))
                throw new Exception();
            if (String.IsNullOrEmpty(Request["email"]))
                throw new Exception();

            //
            // Populate Budget
            //
            var budget = new Budget
                             {
                                 CustomerName = Request["nome"],
                                 CustomerMail = Request["email"],
                                 CompanyId = Company.CompanyId,
                                 BudgetCode = "OR" + Util.GenerateUniqueID()
                             };
            //saleManager.Insert(budget);

            //
            // Count all elements and assign the values to object
            //
            var observation = new StringBuilder();
            foreach (string key in Request.Form)
                if (!key.StartsWith("_") &&
                    !key.ToLower().Contains("submit") &&
                    !key.ToLower().Contains("action") &&
                    !key.ToLower().Contains("ctl"))
                    observation.Append(Server.UrlDecode(key) + ":" + Request[key] + "\n");

            budget.Observation = Convert.ToString(observation);

            //
            // Save file
            //
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFile file = Request.Files[i];
                if (!String.IsNullOrEmpty(file.FileName))
                {
                    file.SaveAs(Server.MapPath(Company.GetBudgetsDirectory() + file.FileName));
                    // ReSharper disable UseObjectOrCollectionInitializer
                    var comm = new Comment();
                    // ReSharper restore UseObjectOrCollectionInitializer
                    comm.SubjectId = budget.BudgetId;
                    comm.PageName = "ProspectBuilder_Product.aspx";
                    comm.FileName = file.FileName;
                    comm.FileUrl = ResolveUrl(Company.GetBudgetsDirectory() + file.FileName);
                    comm.Description = "<a href='" + comm.FileUrl + "'>" + file.FileName + "</a>";
                    comm.UserName = Request["nome"];
                    comm.Email = Request["email"];
                    new CommentsManager(this).Save(comm);
                }
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "",
                                               "alert('Enviado com sucesso!'); location='" + Request.UrlReferrer + "';",
                                               true);
        }

        private void ProcessCustomForm()
        {
            if (String.IsNullOrEmpty(Company.LegalEntityProfile.Email))
            {
                ClientScript.RegisterStartupScript(GetType(), "",
                                                   "alert('Esta empresa não possui e-mail se recebimento configurado! Para configurar acesse o InfoControl, no menu Administração > Empresas.');",
                                                   true);
                return;
            }

            var htmlBody = new StringBuilder();

            foreach (string key in Request.Form)
                if (!key.StartsWith("_") &&
                    !key.ToLower().Contains("submit") &&
                    !key.ToLower().Contains("action") &&
                    !key.ToLower().Contains("ctl"))
                {
                    htmlBody.Append(Server.UrlDecode(key) + ": " + Request[key] + "<br /><br />");
                }

            Postman.Send(
                "mailer@infocontrol.com.br",
                Company.LegalEntityProfile.Email,
                "Fale Conosco",
                htmlBody.ToString(),
                null);

            ClientScript.RegisterStartupScript(Page.GetType(), "",
                                               "alert('Enviado com sucesso!'); location='" + Request.UrlReferrer + "';",
                                               true);
        }

        private void ProcessPage()
        {
            if (ucWebPage != null)
            {
                if (File.Exists(Server.MapPath(ResolveUrl(Company.CompanyId + "/" + Request["t"]))))
                {
                    ExternalContent.InnerHtml =
                        File.ReadAllText(Server.MapPath(ResolveUrl(Company.CompanyId + "/" + Request["t"])),
                                         Encoding.UTF8);
                    ExternalContent.Visible = true;
                    ucWebPage.Visible = false;
                    return;
                }

                if (!String.IsNullOrEmpty(_page.RedirectUrl))
                    Response.Redirect(_page.RedirectUrl);

                CreateTitle();

                if (!_page.IsPublished)
                {
                    var panel = new Panel();
                    panel.Style.Add("background-image",
                                    "url(" + ResolveUrl("~/App_Shared/themes/glasscyan/bgWebPageDraft.png") + ")");
                    panel.Style.Add("position", "absolute");
                    panel.Style.Add("width", "100%");
                    panel.Style.Add("height", "100%");
                    panel.Style.Add("top", "0px");
                    panel.Style.Add("left", "0px");
                    panel.Style.Add("z-index", "9999999");
                    Page.Form.Controls.Add(panel);
                }

                if (!String.IsNullOrEmpty(Request["type"]))
                    ucWebPage.Type = Request["type"];
                if (!String.IsNullOrEmpty(Request["max"]))
                    ucWebPage.MaxCount = Convert.ToInt32(Request["max"]);
                if (!String.IsNullOrEmpty(Request["tag"]))
                    ucWebPage.Tag = Request["tag"];
                if (!String.IsNullOrEmpty(Request["pagecat"]))
                    ucWebPage.Category = Request["pagecat"];
            }
        }

        #endregion
    }
}