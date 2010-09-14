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
#if !CompactFramework
    public abstract class ConfigurationSectionHandler : IConfigurationSectionHandler
#else
    public abstract class ConfigurationSectionHandler 
#endif
    {
        
        private Type _configType = null;
        private string _schemaResourceName = string.Empty;
        private string _schemaNamespace = string.Empty;

        /// <summary>
        /// Classe abstrata que encapsula todas as operações necessárias
        /// para criar um objeto de configuração e XML
        /// </summary>
        /// <param name="configType">Classe gerada a partir do arquivo XSD atraves da ferramenta 'GenClassesFromXSD.bat'</param>
        public ConfigurationSectionHandler(Type configType)
        {
            if (configType == null)
                throw new ArgumentException("A classe de serialização deve ser criada usando a rotina 'XSD.exe'");

            _configType = configType;
            _schemaNamespace = configType.Namespace;

        }

        /// <summary>
        /// A partir de um XML ele deserializa e monta o objeto 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            return (SerializationHelper.DeserializeFromXml(section.OuterXml, _configType));
        }

        /// <summary>
        /// Monta as configurações de acordo com um arquivo XML
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public object GetConfig(string filename)
        {
            return GetConfig(filename, _configType);
        }

        /// <summary>
        /// Monta as configurações de acordo com um arquivo XML
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="configType">Classe gerada a partir do arquivo XSD atraves da ferramenta 'GenClassesFromXSD.bat'</param>
        /// <returns></returns>
        public object GetConfig(string filename, Type configType)
        {
            // Carrega o arquivo de configuração
            System.Xml.XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            return (XmlDeserializeConfig(_configType, doc.OuterXml));
        }

        /// <summary>
        /// Monta as configurações de acordo com uma string em formato XML
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private object XmlDeserializeConfig(Type configType, string xml)
        {
            XmlSerializer ser = new XmlSerializer(configType);
            return (ser.Deserialize(new XmlTextReader(new StringReader(xml))));
        }

    }

    public abstract class ConfigurationSectionHandler<T> : ConfigurationSectionHandler where T : ConfigurationElement
    {
        public ConfigurationSectionHandler()
            : base(typeof(T))
        {
        }
    }
}
