using System;
using System.Collections.Generic;
using System.Text;

namespace InfoControl.Generics
{
    public class Activator
    {
        /// <summary>
        /// Create a type generic in runtime with dynamic types
        /// </summary>
        /// <param name="invariantGenericType"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static object CreateGenericType(string invariantGenericType, params string[] types)
        {

            string typesList = "[";
            foreach (string type in types)
            {
                typesList += "[" + type + "]";
            }
            typesList += "]";

            string genericType = invariantGenericType + "`" + types.Length.ToString() + typesList;


            Type typ = Type.GetType(genericType);

            //object dictInstance = System.Activator.CreateInstance(typ);

            return (System.Activator.CreateInstance(typ));
        }
    }
}
