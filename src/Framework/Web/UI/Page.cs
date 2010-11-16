using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using InfoControl.Web.Configuration;

namespace InfoControl.Web.UI
{
    /// <summary>
    /// Represents a System.Web.UI.Page with helper functions and extra functions
    /// </summary>
    public class Page : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            //
            // Recupera o cookie NoBot que foi criado no script injetado pelo evento OnRender 
            //
            if (Request.HttpMethod == "POST" && String.IsNullOrEmpty(Request["NoBot"]))
            {
                //
                // 412 Precondition Failed
                // The server does not meet one of the preconditions that the requester put on the request
                //
                Response.StatusCode = 412;
                Response.StatusDescription = "Precondition Failed";
            }


            //
            // Crack Telerik
            //
            Context.Items["RadControlRandomNumber"] = 0;

            //
            // Allow SQL Compact Edition run in Web Hosting Environment
            //
            AppDomain.CurrentDomain.SetData("SQLServerCompactEditionUnderWebHosting", true);

            base.OnPreInit(e);
        }

        #region Properties

        private bool _blockContextMenu = false;
        private bool _crossFrameCookies = true;

        private bool _mapFunctionKeys = true;
        private PageStatePersister _pageStatePersister;

        public bool BlockContextMenu
        {
            get { return _blockContextMenu; }
            set { _blockContextMenu = value; }
        }

        public bool MapFunctionKeys
        {
            get { return _mapFunctionKeys; }
            set { _mapFunctionKeys = value; }
        }

        /// <summary>
        /// Indica se ao processar a página será armazenada o conteúdo Html
        /// </summary>
        [Bindable(true)]
        [Category("Others")]
        [Localizable(true)]
        [Description("Indica se ao processar a página será armazenada o conteúdo Html")]
        public bool StoreHtmlContent { get; set; }

        public string StoredHtml
        {
            get { return Convert.ToString(Session[StringResources.StoredHtmlContentKey]); }
        }

        /// <summary>
        /// Indica se a página suporta cache no cliente<para>O Default é false</para>
        /// </summary>
        //[Bindable(true)]
        //[Category("Others")]
        //[Localizable(true)]
        //[Description("Indica se a página suporta cache no cliente")]
        //public bool ClientCache
        //{
        //    get { return (bool)(ViewState["_clientCache"] ?? false); }
        //    set { ViewState["_clientCache"] = value; }
        //}

        /// <summary>
        /// Indica se a página suporta os cookies entre frames
        /// </summary>        
        public bool CrossFrameCookies
        {
            get { return _crossFrameCookies; }
            set { _crossFrameCookies = value; }
        }

        /// <summary>
        /// Gets a dictionary of state information that allows you to save and restore
        /// the view state of a server control across multiple requests for the same
        /// page.
        /// </summary>
        public new StateBag ViewState
        {
            get { return base.ViewState; }
        }

        /// <summary>
        /// Gets the System.Web.UI.PageStatePersister object associated with the page.
        /// </summary>
        protected override PageStatePersister PageStatePersister
        {
            get
            {
                if (_pageStatePersister == null)
                    _pageStatePersister = new SessionPageStatePersister(this);
                return _pageStatePersister;
            }
        }

        #endregion

        #region Methods

        protected override void OnPreRender(EventArgs e)
        {
            Context.Trace.Warn("Page", "Begin OnPreRender");

            base.OnPreRender(e);

            if (Response.ContentType == "text/html" && String.IsNullOrEmpty(Request["HTTP_X_MICROSOFTAJAX"]) &&
                !Convert.ToBoolean(Request["PinJs"]))
                Response.Filter = new ScriptDeferFilter(Response);

            //
            // Client Scripts
            //
            if (_mapFunctionKeys)
                ClientScript.RegisterClientScriptResource(typeof(Page), "InfoControl.Web.UI.Resources.MapFunctionKeys.js");

            if (_blockContextMenu)
                ClientScript.RegisterClientScriptResource(typeof(Page), "InfoControl.Web.UI.Resources.BlockContextMenu.js");

            //if (!ClientCache)
            //{
            //    // Evita o cahce seguramente no protocolo HTTP/1.1 ,
            //    // como browsers antigos não aceitam essa validação se faz necessário os próximos dois.
            //    Response.AddHeader("cache-control", "private, no-store, no-cache, must-revalidate");

            //    // Evita o cache em conexões seguras (SSL)
            //    Response.AddHeader("pragma", "no-cache");
            //}

            string clientScriptFile = Server.MapPath(Request.CurrentExecutionFilePath) + ".js";
            if (File.Exists(clientScriptFile))
            {
                ScriptManager manager = ScriptManager.GetCurrent(this);
                if (manager != null)
                {
                    manager.Scripts.Add(new ScriptReference(Request.Url.AbsolutePath + ".js"));
                }
                else
                {
                    ClientScript.RegisterClientScriptInclude(Request.Url.AbsolutePath, Request.Url.AbsolutePath + ".js");
                }
            }

            Context.Trace.Warn("Page", "End OnPreRender");
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (StoreHtmlContent)
            {
                // Writer que armazenará o conteudo renderizado
                var content = new HtmlTextWriter(new StringWriter());

                // Envia o conteudo para o novo writer
                base.Render(content);

                // Armazena o conteudo na sessão
                Session[StringResources.StoredHtmlContentKey] = content.InnerWriter.ToString();

                // Devolve o conteudo ao writer original para apresentar no corpo da pagina
                writer.Write(Session[StringResources.StoredHtmlContentKey].ToString());
            }
            else
            {
                base.Render(writer);
            }


            //
            // Adiciona este javascript pois muitos webbot não processam javascript, logo as requisições não conterão este cookie
            // 
            //
            writer.WriteLine("<script>document.cookie=\"NoBot=" + Request["ASP.NET_SessionId"] + "; path=/;\";</script>");
        }

        /// <summary>
        /// Importa um arquivo que contem o CSS
        /// </summary>
        /// <param name="path">caminho do arquivo a ser importado</param>
        public void ImportStylesheet(string path)
        {
            var link = new HtmlLink();
            link.Href = path;
            link.Attributes["type"] = "text/css";
            link.Attributes["rel"] = "stylesheet";

            Page.Header.Controls.AddAt(0, link);
        }


        #endregion
    }
}