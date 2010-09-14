using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using System.Text;
using System.Net.Security;

using InfoControl.Net;
using InfoControl.Net.Mail;
using InfoControl.Net.Mail.Mime;

namespace InfoControl.Net.Mail.POP3
{
    /// <summary>
    /// Pop33 Client.
    /// </summary>
    /// <example>
    /// <code>
    /// using(PopClient c = new Pop3Client()){
    ///		c.Connect("ivx",110);
    ///		c.Authenticate("test","test",true);
    ///		
    ///		Pop3MessagesInfo mInf = c.GetMessagesInfo();
    ///		
    ///		// Get first message if there is any
    ///		if(mInf.Count > 0){
    ///			byte[] messageData = c.GetMessage(mInf.MessageNumber);
    ///		
    ///			// Do your suff
    ///			
    ///			// Parse message
    ///			MimeParser m = MimeParser(messageData);
    ///			string from = m.From;
    ///			string subject = m.Subject;			
    ///			// ... 
    ///		}		
    ///	}
    /// </code>
    /// </example>
    public class Pop3Client : IDisposable
    {
        Stream socketStream;
        StreamHelper streamHelper;
        private BufferedSocket m_pSocket = null;
        private SocketLogger m_pLogger = null;
        private bool m_Connected = false;
        private bool m_Authenticated = false;
        private string m_ApopHashKey = "";
        private bool m_LogCmds = false;

        /// <summary>
        /// Occurs when Pop33 session has finished and session log is available.
        /// </summary>
        public event LogEventHandler SessionLog = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Pop3Client()
        {
        }

        #region function Dispose

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        public void Dispose()
        {
            try
            {
                Disconnect();
            }
            catch
            {
            }
        }

        #endregion


        #region function Connect

        /// <summary>
        /// Connects to specified host.
        /// </summary>
        /// <param name="host">Host name.</param>
        /// <param name="port">Port number.</param>
        public void Connect(string host, int port)
        {
            Connect(host, port, false);
        }

        /// <summary>
        /// Connects to specified host.
        /// </summary>
        /// <param name="host">Host name.</param>
        /// <param name="port">Port number.</param>
        public void Connect(string host, int port, bool sslEnabled)
        {
            if (!m_Connected)
            {

                TcpClient client = new TcpClient(host, port);
                if (sslEnabled)
                {
                    socketStream = new SslStream(client.GetStream(), false, null, null);
                    (socketStream as SslStream).AuthenticateAsClient(host);
                }
                else
                {
                    socketStream = client.GetStream();
                }
                streamHelper = new StreamHelper(socketStream, Encoding.ASCII);

                m_Connected = true;

                string reply = streamHelper.ReadToEnd();

                if (reply.StartsWith("+OK"))
                {
                    // Try to read APOP hash key, if supports APOP
                    if (reply.IndexOf("<") > -1 && reply.IndexOf(">") > -1)
                    {
                        m_ApopHashKey = reply.Substring(reply.LastIndexOf("<"), reply.LastIndexOf(">") - reply.LastIndexOf("<") + 1);
                    }
                }
            }
        }

        #endregion

        #region function Disconnect

        /// <summary>
        /// Closes connection to Pop33 server.
        /// </summary>
        public void Disconnect()
        {

            if (socketStream != null)
            {
                // Send QUIT
                socketStream.Write(Encoding.ASCII.GetBytes("QUIT"), 0, 4);
                socketStream.Close();
            }


            if (m_pLogger != null)
            {
                m_pLogger.Flush();
            }
            m_pLogger = null;

            m_pSocket = null;
            m_Connected = false;
            m_Authenticated = false;
        }

        #endregion

        #region function Authenticate

        /// <summary>
        /// Authenticates user.
        /// </summary>
        /// <param name="userName">User login name.</param>
        /// <param name="password">Password.</param>
        /// <param name="tryApop"> If true and Pop33 server supports APOP, then APOP is used, otherwise normal login used.</param>
        public void Authenticate(string userName, string password, bool tryApop)
        {
            if (!m_Connected)
            {
                throw new Exception("You must connect first !");
            }

            if (m_Authenticated)
            {
                throw new Exception("You are already authenticated !");
            }

            // Supports APOP, use it
            if (tryApop && m_ApopHashKey.Length > 0)
            {
                //--- Compute md5 hash -----------------------------------------------//
                byte[] data = System.Text.Encoding.ASCII.GetBytes(m_ApopHashKey + password);

                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] hash = md5.ComputeHash(data);

                string hexHash = BitConverter.ToString(hash).ToLower().Replace("-", "");
                //---------------------------------------------------------------------//

                streamHelper.Write("APOP " + userName + " " + hexHash);

                string reply = streamHelper.ReadToEnd();
                if (reply.StartsWith("+OK"))
                {
                    m_Authenticated = true;
                }
                else
                {
                    throw new Exception("Server returned:" + reply);
                }
            }
            else
            { // Use normal LOGIN, don't support APOP 
                streamHelper.Write("USER " + userName + "\r\n");

                string reply = streamHelper.ReadToEnd();
                if (reply.StartsWith("+OK"))
                {
                    streamHelper.Write("PASS " + password + "\r\n");

                    reply = streamHelper.ReadToEnd();
                    if (reply.StartsWith("+OK"))
                    {
                        m_Authenticated = true;
                    }
                    else
                    {
                        throw new Exception("Server returned:" + reply);
                    }
                }
                else
                {
                    throw new Exception("Server returned:" + reply);
                }
            }
        }

        #endregion


        #region function GetMessagesInfo

        /// <summary>
        /// Gets messages info.
        /// </summary>
        public Pop3MessagesInfo GetMessagesInfo()
        {
            if (!m_Connected)
            {
                throw new Exception("You must connect first !");
            }

            if (!m_Authenticated)
            {
                throw new Exception("You must authenticate first !");
            }

            Pop3MessagesInfo messagesInfo = new Pop3MessagesInfo();

            // Before getting list get UIDL list, then we make full message info (UID,Nr,Size).
            Hashtable uidlList = GetUidlList();

            streamHelper.Write("LIST" + "\r\n");

            /* NOTE: If reply is +OK, this is multiline respone and is terminated with '.'.
            Examples:
                C: LIST
                S: +OK 2 messages (320 octets)
                S: 1 120				
                S: 2 200
                S: .
                ...
                C: LIST 3
                S: -ERR no such message, only 2 messages in maildrop
            */

            // Read first line of reply, check if it's ok
            string line = streamHelper.ReadToEnd();
            if (line.StartsWith("+OK"))
            {
                // Read lines while get only '.' on line itshelf.
                while (true)
                {
                    line = streamHelper.ReadToEnd();

                    // End of data
                    if (line.Trim() == ".")
                    {
                        break;
                    }
                    else
                    {
                        string[] param = line.Trim().Split(new char[] { ' ' });
                        int nr = Convert.ToInt32(param[0]);
                        long size = Convert.ToInt64(param[1]);

                        messagesInfo.Add(uidlList[nr].ToString(), nr, size);
                    }
                }
            }
            else
            {
                throw new Exception("Server returned:" + line);
            }

            return messagesInfo;
        }

        #endregion

        #region function GetUidlList

        /// <summary>
        /// Gets uid listing.
        /// </summary>
        /// <returns>Returns Hashtable containing uidl listing. Key column contains message NR and value contains message UID.</returns>
        public Hashtable GetUidlList()
        {
            if (!m_Connected)
            {
                throw new Exception("You must connect first !");
            }

            if (!m_Authenticated)
            {
                throw new Exception("You must authenticate first !");
            }

            Hashtable retVal = new Hashtable();

            streamHelper.Write("UIDL" + "\r\n");

            /* NOTE: If reply is +OK, this is multiline respone and is terminated with '.'.
            Examples:
                C: UIDL
                S: +OK
                S: 1 whqtswO00WBw418f9t5JxYwZ
                S: 2 QhdPYR:00WBw1Ph7x7
                S: .
                ...
                C: UIDL 3
                S: -ERR no such message
            */

            // Read first line of reply, check if it's ok
            string line = streamHelper.ReadToEnd();
            if (line.StartsWith("+OK"))
            {
                // Read lines while get only '.' on line itshelf.				
                while (true)
                {
                    line = streamHelper.ReadToEnd();

                    // End of data
                    if (line.Trim() == ".") break;

                    string[] param = line.Trim().Split(new char[] { ' ' });
                    int nr = Convert.ToInt32(param[0]);
                    string uid = param[1];

                    retVal.Add(nr, uid);
                }
            }
            else
            {
                throw new Exception("Server returned:" + line);
            }

            return retVal;
        }

        #endregion

        #region function GetMessage
       
        /// <summary>
        /// Gets specified message.
        /// </summary>
        /// <param name="nr">Message number.</param>
        public System.Net.Mail.MailMessage GetMessage(int nr)
        {
            if (!m_Connected)
            {
                throw new Exception("You must connect first !");
            }

            if (!m_Authenticated)
            {
                throw new Exception("You must authenticate first !");
            }

            streamHelper.Write("RETR " + nr.ToString() + "\r\n");

            // Read first line of reply, check if it's ok
            string line = streamHelper.ReadToEnd();
            if (line.StartsWith("+OK"))
            {
                MimeParser parser = new MimeParser(Core.DoPeriodHandling(socketStream, false, false).ToArray());

                return parser.ToMailMessage();
            }
            else
            {
                throw new Exception("Server returned:" + line);
            }
        }              

        #endregion

        #region function DeleteMessage

        /// <summary>
        /// Deletes specified message
        /// </summary>
        /// <param name="messageNr">Message number.</param>
        public void DeleteMessage(int messageNr)
        {
            if (!m_Connected)
            {
                throw new Exception("You must connect first !");
            }

            if (!m_Authenticated)
            {
                throw new Exception("You must authenticate first !");
            }

            streamHelper.Write("DELE " + messageNr.ToString() + "\r\n");

            // Read first line of reply, check if it's ok
            string line = streamHelper.ReadToEnd();
            if (!line.StartsWith("+OK"))
            {
                throw new Exception("Server returned:" + line);
            }
        }

        #endregion

        #region function GetTopOfMessage

        /// <summary>
        /// Gets top lines of message.
        /// </summary>
        /// <param name="nr">Message number which top lines to get.</param>
        /// <param name="nLines">Number of lines to get.</param>
        public System.Net.Mail.MailMessage GetTopOfMessage(int nr, int nLines)
        {
            if (!m_Connected)
            {
                throw new Exception("You must connect first !");
            }

            if (!m_Authenticated)
            {
                throw new Exception("You must authenticate first !");
            }


            streamHelper.Write("TOP " + nr.ToString() + " " + nLines.ToString() + "\r\n");

            // Read first line of reply, check if it's ok
            string line = streamHelper.ReadToEnd();
            if (line.StartsWith("+OK"))
            {
                MimeParser parser = new MimeParser(Core.DoPeriodHandling(socketStream, false, false).ToArray());

                return parser.ToMailMessage();
            }
            else
            {
                throw new Exception("Server returned:" + line);
            }
        }

        #endregion

        #region function Reset

        /// <summary>
        /// Resets session.
        /// </summary>
        public void Reset()
        {
            if (!m_Connected)
            {
                throw new Exception("You must connect first !");
            }

            if (!m_Authenticated)
            {
                throw new Exception("You must authenticate first !");
            }

            streamHelper.Write("RSET" + "\r\n");

            // Read first line of reply, check if it's ok
            string line = streamHelper.ReadToEnd();
            if (!line.StartsWith("+OK"))
            {
                throw new Exception("Server returned:" + line);
            }
        }

        #endregion


        #region properties Implementation

        /// <summary>
        /// Gets or sets if to log commands.
        /// </summary>
        public bool LogCommands
        {
            get { return m_LogCmds; }

            set { m_LogCmds = value; }
        }

        #endregion

    }
}
