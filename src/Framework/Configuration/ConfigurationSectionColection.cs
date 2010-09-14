using System;
using System.Configuration;
using System.Collections;
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
    public class ConfigurationElementCollection<T> : ConfigurationElementCollection where T : ConfigurationElement
#else
    public class ConfigurationElementCollection<T> 
#endif
    {
        public ConfigurationElementCollection() : base() { }

        public T this[int index]
        {
            get
            {
                return (T)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }

        }

        protected override System.Configuration.ConfigurationElement CreateNewElement()
        {
            return Activator.CreateInstance<T>();
        }

        protected override object GetElementKey(System.Configuration.ConfigurationElement element)
        {
            foreach (PropertyInformation pInfo in element.ElementInformation.Properties)
            {
                if (pInfo.IsKey)
                    return pInfo.Value;
            }
            return null;
        }

        public void Add(T entity)
        {
            this.BaseAdd(entity);
        }

        public void Clear()
        {
            base.BaseClear();
        }

        internal bool IsRemoved(string key)
        {
            return base.BaseIsRemoved(key);
        }

        public void Remove(string key)
        {
            base.BaseRemove(key);
        }

        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return base.Properties;
            }
        }
    }


}
