using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

#if !CompactFramework
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
#endif

namespace InfoControl.Runtime
{
    /// <summary>
    /// Provides methods for serialization and De-serialization from objects in Binary Format
    /// </summary>
    public static class SerializationHelper
    {
        #region Serialization
#if !CompactFramework
        public static byte[] Serialize(this object graph)
        {
            byte[] buf;
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            try
            {
                formatter.Serialize(stream, graph);
                buf = stream.ToArray();
            }
            finally
            {
                stream.Close();
            }

            return buf;
        }
        public static string SerializeToString(this object graph)
        {
            return Convert.ToBase64String(Serialize(graph));
        }
#endif
        public static string SerializeToXml(this object graph)
        {
            StringWriter writer = new StringWriter();
            string ret = "[failure]";
            try
            {
                XmlSerializer serializer = new XmlSerializer(graph.GetType());
                serializer.Serialize(writer, graph);
                ret = writer.ToString();
            }
            finally
            {
                writer.Close();
            }

            return ret;
        }
        public static MemoryStream SerializeToXmlInStream(this object graph)
        {
            var stream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(graph.GetType());
            serializer.Serialize(stream, graph);
            stream.Position = 0;
            return stream;
        }

        public static string SerializeToJson(this object graph)
        {
            return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(graph);
        }

        public static string SerializeToWcfJson(this object graph)
        {
            using (var stream = new MemoryStream())
            {
                new DataContractJsonSerializer(graph.GetType()).WriteObject(stream, graph);
                stream.Position = 0;
                return new StreamReader(stream).ReadToEnd();
            }
        }


        #endregion

        #region Deserialization
        #region Untyped
#if !CompactFramework
        public static object Deserialize(this string binary)
        {
            return Deserialize(Convert.FromBase64String(binary));
        }
        public static object Deserialize(this byte[] binary)
        {
            object obj;
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(binary);
            try
            {
                obj = formatter.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }

            return obj;
        }
#endif
        public static object DeserializeFromXml(this string xml, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(xml);
            MemoryStream stream = new MemoryStream(bytes);
            XmlTextReader reader = new XmlTextReader(stream);
            object obj;
            try
            {
                obj = serializer.Deserialize(reader);
            }
            finally
            {
                reader.Close();
            }

            return obj;
        }
        public static object DeserializeFromJson(this string json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            return ser.DeserializeObject(json);
        }

        public static T DeserializeFromWcfJson<T>(this string json)
        {
            using (var stream = new MemoryStream())
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)));
        }

        #endregion
        #region Typed
#if !CompactFramework
        public static T Deserialize<T>(this string binary)
        {
            return Deserialize<T>(Convert.FromBase64String(binary));
        }
        public static T Deserialize<T>(this byte[] binary)
        {
            return (T)Deserialize(binary);
        }
#endif
        public static T DeserializeFromXml<T>(this string xml)
        {
            return (T)DeserializeFromXml(xml, typeof(T));
        }
        public static T DeserializeFromJson<T>(this string json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            return ser.Deserialize<T>(json);
        }
        #endregion

        #endregion
    }
}
