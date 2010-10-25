using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;

namespace InfoControl
{
    /// <summary>
    /// Represents a object that can be identificable
    /// </summary>
    [DataContract]
    public class JsonObject : Hashtable
    {
        new public JsonObject this[object key]
        {
            get
            {
                if (!ContainsKey(key))
                    throw new KeyNotFoundException(key.ToString());

                return base[key] as JsonObject;
            }
            set
            {
                base[key] = value;
            }
        }
    }
}
