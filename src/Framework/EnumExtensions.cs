using System;
using System.Collections.Generic;
using System.Text;

namespace InfoControl
{
    public static partial class EnumExtensions
    {
        public static T ToObject<T>(this Enum graph, object value)
        {
            return (T) Enum.ToObject(typeof(T), value);
        }       
    }
}
