using System.Net;
using System.Net.Mail;
using InfoControl.Web.Configuration;
using System;
using System.IO;
using System.Security.Policy;
using InfoControl.Net;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace InfoControl.Web
{
    [DataContract]
    public class ClientRequest<T>
    {
        [DataMember]
        public Hashtable Parameters { get; set; }

        [DataMember]
        public T FormData { get; set; }

    }   
}