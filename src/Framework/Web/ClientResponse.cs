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
    public class ClientResponse
    {
        public ClientResponse() { }
        public ClientResponse(Func<object> data) {
            try
            {
                Data = data();
            }
            catch (Exception ex)
            {
                Errors = ex.Message;
            }
        }

        [DataMember]
        public string Script { get; set; }

        [DataMember]
        public string Errors { get; set; }

        [DataMember]
        public string Warnings { get; set; }

        [DataMember]
        public object Data { get; set; }

    }
}