using System;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;


using InfoControl.Runtime;


namespace InfoControl.Configuration
{
    /// <summary>
    /// Classe abstrata que encapsula todas as operações necessárias
    /// para criar um objeto de configuração e XML
    /// </summary>
    public class ConfigLoader<T>
    {
        /// <summary>
        /// Monta as configurações de acordo com um arquivo XML
        /// </summary>
        /// <param name="filename"></param>        
        /// <returns></returns>
        public T Load(string filename)
        {
            // Carrega o arquivo de configuração
            System.Xml.XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            return doc.OuterXml.DeserializeFromXml<T>();
        }
    }
}
