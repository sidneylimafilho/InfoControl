using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;


namespace InfoControl.Net
{
    /// <summary>
    /// POST mode as send data
    /// </summary>
    public enum PostMode
    {
        UrlEncoded = 1,
        MultiPart = 2,
        Xml = 4
    }

    public enum MethodType
    {
        Get,
        Post
    }

    /// <summary>
    /// Provides a web client like a browser, mantain cookies, session, POST, GET, etc...
    /// </summary>
    public class HttpWebClient
    {

        #region Members
        BinaryWriter _postedData;
        string _userAgent = "Vivina Web Client";
        string _multiPartBoundary = "-----------------------------7cf2a327f01ae";
        #endregion

        #region Properties

        internal BinaryWriter PostedData
        {
            get
            {
                if (_postedData == null)
                    _postedData = new BinaryWriter(new MemoryStream());
                return (_postedData);
            }
        }

        public MethodType Method
        {
            get { return _method; }
            set { _method = value; }
        }
        private MethodType _method;

        /// <summary>
        /// Determines how data is POSTed when cPostBuffer is set.
        /// </summary>
        public PostMode PostMode
        {
            get { return _postMode; }
            set { _postMode = value; }
        }
        private PostMode _postMode = PostMode.UrlEncoded;

        /// <summary>
        ///  User name used for Authentication. 
        ///  To use the currently logged in user when accessing an NTLM resource you can use "AUTOLOGIN".
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        private string _userName = "";

        /// <summary>
        /// _password for Authentication.
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        private string _password = "";

        /// <summary>
        /// Address of the Proxy Server to be used.
        /// Use optional DEFAULTPROXY value to specify that you want to IE's Proxy Settings
        /// </summary>
        public string ProxyAddress
        {
            get { return _proxyAddress; }
            set { _proxyAddress = value; }
        }
        private string _proxyAddress = "";

        /// <summary>
        /// Semicolon separated Address list of the servers the proxy is not used for.
        /// </summary>
        public string ProxyByPass
        {
            get { return _proxyBypass; }
            set { _proxyBypass = value; }
        }
        private string _proxyBypass = "";

        /// <summary>
        /// _userName for a _password validating Proxy. Only used if the proxy info is set.
        /// </summary>
        public string ProxyUserName
        {
            get { return _proxyUserName; }
            set { _proxyUserName = value; }
        }
        private string _proxyUserName = "";

        /// <summary>
        /// _password for a _password validating Proxy. Only used if the proxy info is set.
        /// </summary>
        public string ProxyPassword
        {
            get { return _proxyPassword; }
            set { _proxyPassword = value; }
        }
        private string _proxyPassword = "";

        /// <summary>
        /// Timeout for the Web request in seconds. Times out on connection, read and send operations.
        /// Default is 30 seconds.
        /// </summary>
        public int ConnectTimeout
        {
            get { return _connectTimeout; }
            set { _connectTimeout = value; }
        }
        private int _connectTimeout = 300;

        /// <summary>
        /// Error Message if the Error Flag is set or an error value is returned from a method.
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }
        private string _errorMsg = "";

        /// <summary>
        /// Error flag if an error occurred.
        /// </summary>
        public bool Error
        {
            get { return _error; }
            set { _error = value; }
        }
        private bool _error = false;

        /// <summary>
        /// Determines whether errors cause exceptions to be thrown. By default errors 
        /// are handled in the class and the Error property is set for error conditions.
        /// (not implemented at this time).
        /// </summary>
        public bool ThrowExceptions
        {
            get { return _throwExceptions; }
            set { _throwExceptions = value; }
        }
        private bool _throwExceptions = false;

        /// <summary>
        /// If set to a non-zero value will automatically track cookies. The number assigned is the cookie count.
        /// </summary>
        public bool HandleCookies
        {
            get { return _handleCookies; }
            set { _handleCookies = value; }
        }
        private bool _handleCookies = false;

#if !CompactFramework
        public CookieContainer Cookies
        {
            get { return _cookies; }
            set { _cookies = value; }
        }
        private CookieContainer _cookies;
#endif
        public HttpWebResponse Response
        {
            get { return _webResponse; }
            set { _webResponse = value; }
        }
        private HttpWebResponse _webResponse;

        public HttpWebRequest Request
        {
            get { return _webRequest; }
            set { _webRequest = value; }
        }
        private HttpWebRequest _webRequest;
        #endregion

        #region Events
        /// <summary>
        /// Fires progress events when using GetUrlEvents() to retrieve a URL.
        /// </summary>
        public event OnReceiveDataHandler OnReceiveData;
        #endregion

        #region Method

        public void AddPostKey(string key, string value)
        {
            Encoding enc = Encoding.GetEncoding(1252);
            switch (_postMode)
            {
                case PostMode.UrlEncoded:
#if !CompactFramework
                    _postedData.Write(key + "=" + System.Web.HttpUtility.UrlEncode(value) + "&");
#else
                    _postedData.Write(key + "=" + value + "&");
#endif
                    break;
                case PostMode.MultiPart:
                    _postedData.Write(enc.GetBytes("--" + _multiPartBoundary + "\r\n"));
                    _postedData.Write(enc.GetBytes("Content-Disposition: form-data; "));
                    _postedData.Write(enc.GetBytes("name=\"" + key + "\"\r\n\r\n" + value));
                    _postedData.Write(enc.GetBytes("\r\n"));
                    break;
                default:
                    _postedData.Write(value);
                    break;
            }
        }

        /// <summary>
        /// Adds a fully self contained POST buffer to the request.
        /// Works for XML or previously encoded content.
        /// </summary>
        /// <param name="PostBuffer"></param>
        public void AddPostKey(string fullPostBuffer)
        {
            _postedData.Write(Encoding.GetEncoding(1252).GetBytes(fullPostBuffer));
        }

        public bool AddPostFile(string key, string fileName)
        {
            byte[] lcFile;

            if (_postMode != PostMode.MultiPart)
            {
                _errorMsg = "File upload allowed only with Multi-part forms";
                _error = true;
                return false;
            }

            try
            {
                FileStream loFile = new FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                lcFile = new byte[loFile.Length];
                loFile.Read(lcFile, 0, (int)loFile.Length);
                loFile.Close();
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
                _error = true;
                return false;
            }

            Encoding enc = Encoding.GetEncoding(1252);
            _postedData.Write(enc.GetBytes("--" + _multiPartBoundary + "\r\n"));
            _postedData.Write(enc.GetBytes("Content-Disposition: form-data; "));
            _postedData.Write(enc.GetBytes("name=\"" + key + "\" filename=\"" + new FileInfo(fileName).Name + "\"\r\n"));
            _postedData.Write(enc.GetBytes("\r\n"));
            _postedData.Write(lcFile);
            _postedData.Write(enc.GetBytes("\r\n"));

            return true;
        }


        /// <summary>
        /// Return a the result from an HTTP Url into a StreamReader.
        /// Client code should call Close() on the returned object when done reading.
        /// </summary>
        /// <param name="Url">Url to retrieve.</param>        
        /// <returns></returns>
        internal Stream GetUrlStream(string url)
        {

            _error = false;
            _errorMsg = "";

            Request = (HttpWebRequest)System.Net.WebRequest.Create(url);
            Request.UserAgent = _userAgent;
            Request.Timeout = _connectTimeout * 1000;


            // *** Handle Security for the request
            if (_userName.Length > 0)
            {
                if (_userName == "AUTOLOGIN")
                    Request.Credentials = CredentialCache.DefaultCredentials;
                else
                    Request.Credentials = new NetworkCredential(_userName, _password);
            }


            // *** Handle Proxy Server configuration
            if (_proxyAddress.Length > 0)
            {
                if (_proxyAddress == "DEFAULTPROXY")
                {
                    Request.Proxy = new WebProxy();
                }
                else
                {
                    WebProxy loProxy = new WebProxy(_proxyAddress, true);
#if !CompactFramework
                    if (_proxyBypass.Length > 0)
                    {
                        loProxy.BypassList = _proxyBypass.Split(';');
                    }
#endif

                    if (_proxyUserName.Length > 0)
                        loProxy.Credentials = new NetworkCredential(_proxyUserName, _proxyPassword);

                    Request.Proxy = loProxy;
                }
            }

#if !CompactFramework
            // *** Handle cookies - automatically re-assign 
            if (_handleCookies)
            {
                Request.CookieContainer = new CookieContainer();
                if (_cookies != null)
                {
                    Request.CookieContainer = _cookies;
                }
                else
                {
                    _cookies = new CookieContainer();
                }
            }
#endif

            // *** Deal with the POST buffer if any
            if (Method == MethodType.Post)
            {
                Request.Method = "POST";
                switch (_postMode)
                {
                    case PostMode.UrlEncoded:
                        Request.ContentType = "application/x-www-form-urlencoded";
                        // strip off any trailing & which can cause problems with some 
                        // http servers
                        //							if (cPostBuffer.EndsWith("&"))
                        //								cPostBuffer = cPostBuffer.Substring(0,cPostBuffer.Length-1);
                        break;
                    case PostMode.MultiPart:
                        Request.ContentType = "multipart/form-data; boundary=" + _multiPartBoundary;
                        _postedData.Write(Encoding.GetEncoding(1252).GetBytes("--" + _multiPartBoundary + "\r\n"));
                        break;
                    case PostMode.Xml:
                        Request.ContentType = "text/xml";
                        break;
                    default:
                        goto case PostMode.UrlEncoded;
                }

                Stream reqStream = Request.GetRequestStream();

                (_postedData.BaseStream as MemoryStream).WriteTo(reqStream);

                //*** Close the memory stream
                _postedData.BaseStream.Close();


                //Close the Binary Writer
                _postedData.Close();
                _postedData = null;


                //Close Request Stream
                reqStream.Close();

            }
            else
            {
                Request.ContentType = "application/x-www-form-urlencoded";
            }


            // *** Retrieve the response headers 
            Response = (HttpWebResponse)Request.GetResponse();

#if !CompactFramework
            // ** Save cookies the server sends
            if (_handleCookies)
            {
                if (Request.CookieContainer.Count > 0)
                {
                    _cookies = Request.CookieContainer;
                }

                if (Response.Cookies.Count > 0)
                {
                    if (_cookies == null)
                    {
                        _cookies = new CookieContainer();
                    }
                    _cookies.Add(Response.Cookies);
                }
            }
#endif



            //Recreate the stream
            _postedData = null;
            _postedData = PostedData;

            // *** drag to a stream
            //StreamReader strResponse = new StreamReader(Response.GetResponseStream(), enc);
            return Response.GetResponseStream();

        }

        /// <summary>
        /// Gets a the result from an HTTP Url into a string.
        /// </summary>
        /// <param name="Url">Url to retrieve.</param>
        /// <returns></returns>
        public string GetUrl(string url)
        {
            Method = MethodType.Get;
            return (ProcessUrl(url));
        }

        /// <summary>
        /// Post the data and retrieve an HTTP Url into a string
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public string PostUrl(string url)
        {
            Method = MethodType.Post;
            return (ProcessUrl(url));
        }

        /// <summary>
        /// Process a HTTP Url into string
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string ProcessUrl(string url)
        {
            Encoding enc;
            try
            {
                if (Response.ContentEncoding.Length > 0)
                    enc = Encoding.GetEncoding(Response.ContentEncoding);
                else
                    enc = Encoding.GetEncoding(1252);
            }
            catch
            {
                // *** Invalid encoding passed
                enc = Encoding.GetEncoding(1252);
            }

            using (StreamReader stream = new StreamReader(GetUrlStream(url), enc))
                return stream.ReadToEnd();
        }

        /// <summary>
        /// Return a the result from an HTTP Url into a string.
        /// </summary>
        /// <param name="Url">Url to retrieve.</param>
        /// <returns></returns>
        public byte[] GetUrlBytes(string url)
        {
            var oHttpResponse = GetUrlStream(url);

            if (oHttpResponse == null)
                return null;


            int b;
            var result = new List<byte>();
            while ((b = oHttpResponse.ReadByte()) > -1)
                result.Add(Convert.ToByte(b));

            oHttpResponse.Close();

            return result.ToArray();
        }

        #endregion

        public HttpWebClient()
        {
            //Recreate the stream
            _postedData = PostedData;
        }


        public delegate void OnReceiveDataHandler(object sender, OnReceiveDataEventArgs e);
        public class OnReceiveDataEventArgs
        {
            public long CurrentByteCount = 0;
            public long TotalBytes = 0;
            public int NumberOfReads = 0;
            public char[] CurrentChunk;
            public bool Done = false;
            public bool Cancel = false;
        }
    }
}
