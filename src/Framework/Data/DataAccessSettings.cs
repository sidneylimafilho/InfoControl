﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.42.
// 
namespace InfoControl.Data {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="InfoControl.Data", IsNullable=false)]
    public partial class dataAccessSettings {
        
        private provider[] providersField;
        
        private source[] sourcesField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public provider[] providers {
            get {
                return this.providersField;
            }
            set {
                this.providersField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public source[] sources {
            get {
                return this.sourcesField;
            }
            set {
                this.sourcesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="InfoControl.Data")]
    public partial class provider {
        
        private string nameField;
        
        private string connectionTypeField;
        
        private string dataAdapterTypeField;
        
        private string commandBuilderTypeField;
        
        private string parameterPrefixField;
        
        private string assemblyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string connectionType {
            get {
                return this.connectionTypeField;
            }
            set {
                this.connectionTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataAdapterType {
            get {
                return this.dataAdapterTypeField;
            }
            set {
                this.dataAdapterTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string commandBuilderType {
            get {
                return this.commandBuilderTypeField;
            }
            set {
                this.commandBuilderTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string parameterPrefix {
            get {
                return this.parameterPrefixField;
            }
            set {
                this.parameterPrefixField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string assembly {
            get {
                return this.assemblyField;
            }
            set {
                this.assemblyField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="InfoControl.Data")]
    public partial class source {
        
        private string nameField;
        
        private bool isDefaultField;
        
        private bool isDefaultFieldSpecified;
        
        private string providerField;
        
        private string connectionStringField;
        
        private bool readOnlyField;
        
        private bool readOnlyFieldSpecified;
        
        private bool autoCommitField;
        
        private bool autoCommitFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isDefault {
            get {
                return this.isDefaultField;
            }
            set {
                this.isDefaultField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isDefaultSpecified {
            get {
                return this.isDefaultFieldSpecified;
            }
            set {
                this.isDefaultFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string provider {
            get {
                return this.providerField;
            }
            set {
                this.providerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string connectionString {
            get {
                return this.connectionStringField;
            }
            set {
                this.connectionStringField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool readOnly {
            get {
                return this.readOnlyField;
            }
            set {
                this.readOnlyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool readOnlySpecified {
            get {
                return this.readOnlyFieldSpecified;
            }
            set {
                this.readOnlyFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool autoCommit {
            get {
                return this.autoCommitField;
            }
            set {
                this.autoCommitField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool autoCommitSpecified {
            get {
                return this.autoCommitFieldSpecified;
            }
            set {
                this.autoCommitFieldSpecified = value;
            }
        }
    }
}
