using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

using InfoControl.Runtime;


namespace InfoControl
{

    public static partial class ObjectExtensions
    {
        public static MemoryStream SerializeToXmlInStream<T>(this T graph)
        {
            return SerializationHelper.SerializeToXmlInStream(graph);
        }
        public static string SerializeToXml<T>(this T graph)
        {
            return SerializationHelper.SerializeToXml(graph);
        }
#if !CompactFramework
        public static string SerializeToString<T>(this T graph)
        {
            return SerializationHelper.SerializeToString(graph);
        }

        public static string SerializeToJson<T>(this T graph)
        {
            return SerializationHelper.SerializeToJson(graph);
        }

        public static string SerializeToWcfJson<T>(this T graph)
        {
            return SerializationHelper.SerializeToWcfJson(graph);
        }

        public static byte[] Serialize<T>(this T graph)
        {
            return SerializationHelper.Serialize(graph);
        }
#endif
    }
}
