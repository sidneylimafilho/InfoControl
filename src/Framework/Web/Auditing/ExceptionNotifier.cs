using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.SessionState;
using InfoControl.Data;
using InfoControl.Web.Configuration;
using InfoControl.Web.Security;

namespace InfoControl.Web.Auditing
{
    public enum EventType
    {
        Warning,
        Information,
        Sugestion,
        Error
    }

    [DataObject]
    public class ExceptionManager
    {
        private readonly ExceptionNotifierDataContext dbContext;
        private readonly DataManager manager;
        private Event eventEntity;

        public ExceptionManager()
        {
            manager = new DataManager(false);
            dbContext = manager.CreateDataContext<ExceptionNotifierDataContext>();
        }

        #region Members

        private readonly Hashtable databaseConfig =
            ConfigurationManager.GetSection("InfoControl/ExceptionNotifier/Database") as Hashtable;

        private readonly Hashtable emailConfig =
            ConfigurationManager.GetSection("InfoControl/ExceptionNotifier/Email") as Hashtable;

        private readonly Hashtable eventLogConfig =
            ConfigurationManager.GetSection("InfoControl/ExceptionNotifier/EventLog") as Hashtable;

        private readonly Hashtable fileConfig =
            ConfigurationManager.GetSection("InfoControl/ExceptionNotifier/File") as Hashtable;

        #endregion

        /// <summary>
        /// Get all events in database, Events are exceptions, alerts or info throw for the system
        /// </summary>
        /// <returns></returns>        
        public IQueryable<Event> GetAllEvents()
        {
            return dbContext.Events;
        }

        /// <summary>
        /// Get all events in database, Events are exceptions, alerts or info throw for the system
        /// </summary>
        /// <returns></returns>        
        public IQueryable<Event> GetAllEvents(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetAllEvents().SortAndPage(sortExpression, startRowIndex, maximumRows, "Name");
        }

        /// <summary>
        /// Get the Event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>        
        public Event GetEvent(int eventId)
        {
            return GetAllEvents().Where(ev => ev.EventId == eventId).FirstOrDefault();
        }

        /// <summary>
        /// Notifies a exception when occurs in the Application
        /// </summary>
        /// <param name="ex"></param>
        public void Notify(Exception ex)
        {
            //
            // Realiza as notificações
            //
            if (eventLogConfig != null && Convert.ToBoolean(eventLogConfig["active"]))
            {
                WriteToEventLog(ex);
            }

            if (databaseConfig != null && Convert.ToBoolean(databaseConfig["active"]))
            {
                if (!MaintenanceModule.Enabled)
                {
                    throw new ConfigurationErrorsException("It is necessary to configure the Maintenance Module!");
                }
                LogErrorInDatabase(ex);
            }

            if (emailConfig != null && Convert.ToBoolean(emailConfig["active"]))
            {
                SendMail(ex);
            }

            if (fileConfig != null && Convert.ToBoolean(fileConfig["active"]))
            {
                WriteToFile(ex);
            }
        }

        #region Notifiers

        private void WriteToEventLog(Exception ex)
        {
            var log = new EventLog();
            log.Source = "Application";
            log.Log = "Application";

            string message = "An exception occurred communicating with the data source.\n\n";
            message += "Action: \n\n";
            message += "Exception: " + ex.Message;

            log.WriteEntry(message, EventLogEntryType.Error);
        }

        public virtual Event LogErrorInDatabase(Exception ex)
        {
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                //
                // Create an event
                //
                eventEntity = new InfoControl.Web.Auditing.Event
                                {
                                    ApplicationId = Application.Current.ApplicationId,
                                    CurrentDate = DateTime.Now,
                                    ExceptionCode = ex.GetHashCode().ToString(),

                                    Message = "<h2>Message: " + ex.Message.Replace("\n", "<br />") + "</h2><br />"
                                      + "<b>Source: </b>" + ex.Source + "<br />"
                                      + (ex.InnerException != null ? "<b>InnerException: </b>" + ex.InnerException.GetType() + "<br />" : String.Empty)
                                      + "<b>TargetSite: </b>" + ex.TargetSite + "<br />"
                                      + "<b>Path: </b>" + context.Request.Path + "<br />"
                                      + "<b>URL Referer: </b>" + (context.Request.UrlReferrer != null ? context.Request.UrlReferrer.AbsolutePath : String.Empty) + "<br />"
                                      + "<b>Stack Trace: </b>" + ex.StackTrace.Replace("\n", "<br />")
                                      + (!String.IsNullOrEmpty(ex.HelpLink) ? "<b>HelpLink: </b><br />" + ex.HelpLink + "<br />" : String.Empty)
                                      + GetTextErrorLog(),

                                    Name = ex.GetType().ToString(),
                                    Path = context.Request.FilePath,
                                    RefererUrl = context.Request.UrlReferrer != null
                                                     ? context.Request.UrlReferrer.AbsolutePath
                                                     : String.Empty,
                                    Source = ex.Source,
                                    StackTrace = ex.StackTrace,
                                    TargetSite = ex.TargetSite.Name,
                                    EventType = Convert.ToInt32(EventType.Error)
                                };

                if (context.User != null && context.User is AccessControlPrincipal &&
                    (context.User as AccessControlPrincipal).Identity.UserId > 0)
                    eventEntity.UserId = (context.User as AccessControlPrincipal).Identity.UserId;

                

                //  eventEntity.StackTrace += ex.HelpLink;

                dbContext.Events.InsertOnSubmit(eventEntity);
                dbContext.SubmitChanges();
            }

            return eventEntity;
        }

        private void WriteToFile(Exception ex)
        {
            if (fileConfig["filePath"] == null)
            {
                throw new NullReferenceException("The 'InfoControl/File' section is required!");
            }

            if (String.IsNullOrEmpty(fileConfig["filePath"].ToString()))
            {
                throw new NullReferenceException("The 'InfoControl/File' section is empty!");
            }

            string text = GetHeaderErrorLog(ex);
            File.AppendAllText(GetFileNameLog(DateTime.Now.Date), text);
        }

        private void SendMail(Exception ex)
        {
            if (String.IsNullOrEmpty(emailConfig["from"].ToString()))
                throw new Exception("Para enviar o log de erros, por email, precisa configurar um remetente!");

            if (String.IsNullOrEmpty(emailConfig["to"].ToString()))
                throw new Exception("Para enviar o log de erros, por email, precisa configurar um destinat�rio!");

            var smtp = new SmtpClient();
            var mail = new MailMessage(emailConfig["to"].ToString(), emailConfig["from"].ToString());
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            //
            // Se for para envio consolidado verifica se j� passou o dia
            //
            if (Convert.ToBoolean(emailConfig["digest"]))
            {
                DateTime sentDate = GetSentDate();

                //
                // S� envia quando totalizar o dia
                //
                if (sentDate < DateTime.Now.Date)
                {
                    var countError = ((int)HttpContext.Current.Cache["CountError"]);
                    if (countError > 1)
                    {
                        mail.Subject = "Foram encontrados " + countError + " erros no sistema " +
                                       Application.Current.Name;
                    }
                    else
                    {
                        mail.Subject = "Foi encontrado " + countError + " erro no sistema " + Application.Current.Name;
                    }
                    mail.Body = HttpContext.Current.Cache["EmailDigestBody"].ToString();

                    smtp.Send(mail);

                    // Zera o contador de erros
                    HttpContext.Current.Cache["CountError"] = 0;

                    // Limpa o cache                     
                    HttpContext.Current.Cache["EmailDigestBody"] = "";

                    // Seta a data para hoje, para s� enviar o pr�ximo digest 
                    // dia seguinte                    
                    HttpContext.Current.Cache["SentDate"] = DateTime.Now.Date;
                }
                else
                {
                    //
                    // Guarda a quantidade de erros ocorridos no dia
                    //
                    if (HttpContext.Current.Cache["CountError"] == null)
                        HttpContext.Current.Cache["CountError"] = 0;

                    var countError = ((int)HttpContext.Current.Cache["CountError"]);
                    countError += 1;
                    HttpContext.Current.Cache["CountError"] = countError;

                    //
                    // Acumula os errors para serem enviados 
                    //
                    if (HttpContext.Current.Cache["EmailDigestBody"] == null)
                        HttpContext.Current.Cache["EmailDigestBody"] = "";

                    var mailDigestBody = (string)HttpContext.Current.Cache["EmailDigestBody"];
                    mailDigestBody += GetHeaderErrorLog(ex);
                    HttpContext.Current.Cache["EmailDigestBody"] = mailDigestBody;
                }
            }
            else
            {
                //
                // Dispara a cada erro que ocorra
                //
                mail.Subject = "Foi encontrado erro no sistema " + Application.Current.Name;
                mail.Body = GetHeaderErrorLog(ex);
                smtp.Send(mail);
            }
        }

        #endregion

        #region BodyBuilder Functions

        private DateTime GetSentDate()
        {
            if (HttpContext.Current.Cache["SentDate"] == null)
            {
                HttpContext.Current.Cache["SentDate"] = DateTime.Now.Date;
            }
            return ((DateTime)HttpContext.Current.Cache["SentDate"]);
        }

        private string GetFileNameLog(DateTime date)
        {
            return (GetFilePathLog() + "\\ERROR_LOG_" + date.ToString(fileConfig["dateFormatInFileName"].ToString()) +
                    ".HTM");
        }

        private string GetRequestFileNameLog(DateTime date)
        {
            return (GetFilePathLog() + "\\ERROR_LOG_REQUEST_" + date.Ticks + ".HTM");
        }

        private string GetFilePathLog()
        {
            if (!Directory.Exists(fileConfig["filePath"].ToString()))
            {
                throw new ConfigurationErrorsException(
                    "The file path configured in 'InfoControl/File' section is invalid!");
            }

            return fileConfig["filePath"].ToString();
        }


        public static string GetHeaderErrorLog(Exception ex)
        {
            string body =
                @"<html>
                                <head>
                                    <title>[=Message=]</title>
                                    <style>
                                     body {font-family:'Verdana';font-weight:normal;font-size: .7em;color:black;} 
                                     p {font-family:'Verdana';font-weight:normal;color:black;margin-top: -5px}
                                     b {font-family:'Verdana';font-weight:bold;color:black;margin-top: -5px}
                                     H1 { font-family:'Verdana';font-weight:normal;font-size:18pt;color:red }
                                     H2 { font-family:'Verdana';font-weight:normal;font-size:14pt;color:maroon }
                                     pre {font-family:'Lucida Console';font-size: .9em}
                                     .marker {font-weight: bold; color: black;text-decoration: none;}
                                     .version {color: gray;}
                                     .error {margin-bottom: 10px;}
                                     .expandable { text-decoration:underline; font-weight:bold; color:navy; cursor:hand; }
                                    </style>
                                </head>
                                <body bgcolor='white'>

                                        <span><H1>Server Error in '[=ApplicationName=]' Application.<hr width=100% size=1 color=silver></H1>

                                        <h2> <i>[=Message=]</i> </h2></span>

                                        <font face='Arial, Helvetica, Geneva, SunSans-Regular, sans-serif '>

                                        <b> Description: </b>An unhandled exception occurred during the execution of the current web request. Please review the stack trace for more information about the error and where it originated in the code.

                                        <br><br>

                                        <b> Exception Details: </b>[=ExceptionType=]: [=Message=]<br><br>
                                        
                                        <br>

                                        <b> Source File: </b>[=FilePath=] [=TargetSite=]<b> &nbsp;&nbsp;
                                        <br><br>

                                        <b>Stack Trace:</b> <br><br>

                                        <table width=100% bgcolor='#ffffcc'>
                                           <tr>
                                              <td>
                                                  <code><pre>[=StackTrace=]</pre></code>
                                              </td>
                                           </tr>
                                        </table>

                                        <br>

                                        <hr width=100% size=1 color=silver>

                                        <b>Version Information:</b>[=.NetVersion=]&nbsp;Microsoft .NET Framework Version:2.0.50727.42; ASP.NET Version:2.0.50727.42

                                        </font>

                                </body>
                            </html>";

            body = body.Replace("[=Message=]", ex.Message);
            body = body.Replace("[=ExceptionType=]", ex.GetType().ToString());
            body = body.Replace("[=StackTrace=]", ex.StackTrace);
            body = body.Replace("[=TargetSite=]", ex.TargetSite.Name);
            body = body.Replace("[=.NetVersion=]", Environment.Version.ToString());
            body = body.Replace("[=FilePath=]", HttpContext.Current.Request.FilePath);
            body = body.Replace("[=ApplicationName=]", HttpContext.Current.Request.PhysicalApplicationPath);


            return (body);
        }

        private string GetTextErrorLog()
        {
            var sb = new StringBuilder();

            string text = String.Empty;

            text = GetFormVariables();
            if (!String.IsNullOrEmpty(text))
                sb.Append(text + "<br /><br />");

            text = GetQueryStringVariables();
            if (!String.IsNullOrEmpty(text))
                sb.Append(text + "<br /><br />");

            text = GetSessionVariables();
            if (!String.IsNullOrEmpty(text))
                sb.Append(text + "<br /><br />");

            text = GetCookiesVariables();
            if (!String.IsNullOrEmpty(text))
                sb.Append(text + "<br /><br />");

            text = GetServerVariables();
            if (!String.IsNullOrEmpty(text))
                sb.Append(text + "<br /><br />");

            return (sb.ToString());
        }

        private string GetQueryStringVariables()
        {
            HttpRequest request = HttpContext.Current.Request;

            var sb = new StringBuilder();
            if (request.QueryString.Count > 0)
            {
                sb.Append("<h3>Query String Variables</h3>");
                sb.Append("<table border='1'>");
                for (int i = 0; i < request.QueryString.Count; i++)
                {
                    if (!String.IsNullOrEmpty(request.QueryString[i]))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + request.QueryString.Keys[i] + "&nbsp;</td>");
                        sb.Append("<td>" + request.QueryString[i] + "&nbsp;</td>");
                        sb.Append("</tr>");
                    }
                }
                sb.Append("</table>");
            }
            return (sb.ToString().Replace("\n", "<BR />"));
        }

        private string GetFormVariables()
        {
            HttpRequest request = HttpContext.Current.Request;

            var sb = new StringBuilder();
            if (request.Form.Count > 0)
            {
                sb.Append("<h3>Form Variables</h3>");
                sb.Append("<table border='1'>");

                for (int i = 0; i < request.Form.Count; i++)
                {
                    if (!String.IsNullOrEmpty(request.Form[i]))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td><b>" + request.Form.Keys[i] + "</b>&nbsp;</td>");
                        sb.Append("<td>" + request.Form[i] + "&nbsp;</td>");
                        sb.Append("</tr>");
                    }
                }
                sb.Append("</table>");
            }
            return (sb.ToString().Replace("\n", "<BR />"));
        }

        private string GetServerVariables()
        {
            HttpRequest request = HttpContext.Current.Request;

            var sb = new StringBuilder();
            if (request.ServerVariables.Count > 0)
            {
                sb.Append("<h3>Server Variables</h3>");
                sb.Append("<table border='1'>");

                foreach (string key in request.ServerVariables.Keys)
                {
                    if (!key.Contains("ALL"))
                    {
                        try
                        {
                            sb.Append("<tr>");
                            sb.Append("<td><b>" + key + "</b>&nbsp;</td>");
                            sb.Append("<td>" + request.ServerVariables.Get(key).Replace(";", "; <br/>") + "&nbsp;</td>");
                            sb.Append("</tr>");
                        }
                        catch
                        {
                        }
                    }
                }
                sb.Append("</table>");
            }
            return (sb.ToString().Replace("\n", "<BR />"));
        }

        private string GetSessionVariables()
        {
            HttpSessionState session = HttpContext.Current.Session;

            var sb = new StringBuilder();

            if (session != null && session.Count > 0)
            {
                sb.Append("<h3>Session</h3>");
                sb.Append("<table border='1'>");
                for (int i = 0; i < session.Count; i++)
                    if (!String.IsNullOrEmpty(Convert.ToString(session[i])))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td><b>" + session.Keys[i] + "</b>&nbsp;</td>");
                        sb.Append("<td>" + Convert.ToString(session[i]) + "&nbsp;</td>");
                        sb.Append("</tr>");
                    }

                sb.Append("</table>");
            }
            return (sb.ToString().Replace("\n", "<BR />"));
        }

        private string GetCookiesVariables()
        {
            HttpResponse response = HttpContext.Current.Response;

            var sb = new StringBuilder();

            if (response.Cookies.Count > 0)
            {
                sb.Append("<h3>Cookies</h3>");
                sb.Append("<table border='1'>");
                for (int i = 0; i < response.Cookies.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.Append("<td><b>" + response.Cookies.Keys[i] + "</b>&nbsp;</td>");
                    sb.Append("<td>" + response.Cookies[i].Value + "&nbsp;</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
            }
            return (sb.ToString().Replace("\n", "<BR />"));
        }

        #endregion
    }
}