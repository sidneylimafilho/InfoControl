using System.Net;
using System.Net.Mail;
using InfoControl.Web.Configuration;
using System;
using System.IO;
using System.Security.Policy;
using InfoControl.Net;

namespace InfoControl.Web.Mail
{
    public static class Postman
    {

        private static SmtpClient Client
        {
            get
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.EnableSsl = (WebConfig.Web.Smtp.Network.Port == 587);
                smtpClient.Credentials = new NetworkCredential(WebConfig.Web.Smtp.Network.UserName,
                                                               WebConfig.Web.Smtp.Network.Password);
                return smtpClient;
            }
        }

        /// <summary>
        /// Sends the specified message to an SMTP server for delivery.
        /// </summary>
        /// <param name="message">A System.Net.Mail.MailMessage that contains the message to send.</param>
        public static void Send(MailMessage message)
        {
            Send(message, null, null);
        }

        /// <summary>
        /// Sends the specified message to an SMTP server for delivery.
        /// </summary>
        /// <param name="message">A System.Net.Mail.MailMessage that contains the message to send.</param>
        public static void Send(MailMessage message, string[] addressFileToAttach)
        {
            Send(message, null, addressFileToAttach);
        }

        /// <summary>
        /// Sends the specified message to an SMTP server for delivery.
        /// </summary>
        /// <param name="message">A System.Net.Mail.MailMessage that contains the message to send.</param>
        public static void Send(MailMessage message, Uri templateBodyUrl, string[] addressFileToAttach)
        {
            if (addressFileToAttach != null)
            {
                foreach (var file in addressFileToAttach)
                    if (!String.IsNullOrEmpty(file))
                        message.Attachments.Add(new Attachment(file));
            }

            if (templateBodyUrl != null)
                message.Body = (new HttpWebClient()).GetUrl(templateBodyUrl.ToString());

            Client.Send(message);
        }

        /// <summary>
        /// Sends the specified e-mail message to an SMTP server for delivery. The message
        ///     sender, recipients, subject, and message body are specified using System.String
        ///     objects.
        /// </summary>
        /// <param name="from">email account that wiil send the message </param>
        /// <param name="to">email account that will receive the message </param>
        /// <param name="subject"> subject of email</param>
        /// <param name="body">message in body of email</param>
        /// <param name="addressFileToAttach"> address of file for to attach with email message</param>
        public static void Send(string from, string to, string subject, Uri templateBodyUrl, string[] addressFileToAttach)
        {
            var msg = new MailMessage(from, to);
            msg.From = new MailAddress(from, from);
#if NET40
            msg.ReplyToList.Add(new MailAddress(from, from));
#else
            msg.ReplyTo = new MailAddress(from, from);
#endif
            msg.IsBodyHtml = true;

            Send(msg, templateBodyUrl, addressFileToAttach);
        }

        /// <summary>
        /// Sends the specified e-mail message to an SMTP server for delivery. The message
        ///     sender, recipients, subject, and message body are specified using System.String
        ///     objects.
        /// </summary>
        /// <param name="from">email account that wiil send the message </param>
        /// <param name="to">email account that will receive the message </param>
        /// <param name="subject"> subject of email</param>
        /// <param name="body">message in body of email</param>
        /// <param name="addressFileToAttach"> address of file for to attach with email message</param>
        public static void Send(string from, string to, string subject, string body, string[] addressFileToAttach)
        {
            var msg = new MailMessage(from, to, subject, body);
            msg.From = new MailAddress(from, from);

#if NET40
            msg.ReplyToList.Add(new MailAddress(from, from));
#else
            msg.ReplyTo = new MailAddress(from, from);
#endif

            msg.IsBodyHtml = true;
            
            Send(msg, null, addressFileToAttach);
        }

        /// <summary>
        /// Sends the specified e-mail message to an SMTP server for delivery. This method does not block the calling thread and allows the caller to pass an object to the method that is invoked when the operation completes.
        /// </summary>
        /// <param name="message">A System.Net.Mail.MailMessage that contains the message to send.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        public static void SendAsync(MailMessage message, object userToken)
        {
            Client.SendAsync(message, userToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="recipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="userToken"></param>
        public static void SendAsync(string from, string recipients, string subject, string body, object userToken)
        {
            Client.SendAsync(from, recipients, subject, body, userToken);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SendAsyncCancel()
        {
            Client.SendAsyncCancel();
        }
    }
}