using System;
using System.Collections.Generic;
using System.Text;

namespace InfoControl
{
    public static partial class EnumExtensions
    {
        public static T ToEnum<T>(this int graph)
        {
            return (T)Enum.ToObject(typeof(T), graph);
        }
    }
}
