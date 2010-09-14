using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace InfoControl
{
    /// <summary>
    /// Represents a object that can be identificable
    /// </summary>
    [DataContract]
    public class Recognizable
    {
        [DataMember]
        public object Name { get; private set; }

        [DataMember]
        public object Id { get; private set; }

        [DataMember]
        public object[] Remarks { get; private set; }

        public Recognizable(object id, object name, params object[] remarks)
        {
            Id = id;
            Name = name;
            Remarks = remarks;
        }
    }
}
