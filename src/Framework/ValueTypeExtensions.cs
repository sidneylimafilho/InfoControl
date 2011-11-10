using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data.Linq;
using System.Collections;

namespace InfoControl
{
    public static partial class ValueTypeExtensions
    {
        #region DateTime

        public static DateTime Sql2005MinValue(this DateTime date)
        {
            return new DateTime(1753, 1, 1);
        }

        public static DateTime NextUtilDay(this DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday)
                date = date.AddDays(2);

            if (date.DayOfWeek == DayOfWeek.Sunday)
                date = date.AddDays(1);

            return date;
        }

        public static DateTime NextUtilDay(this DateTime? date)
        {
            return NextUtilDay(date.Value);
        }

        public static string ToLocalDateString(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Convert the DateTime to RFC1123 format as <example>"Sun, 09 Mar 2008 16:05:07 GMT"</example>
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToRFC1123(this DateTime date)
        {
            return date.ToString("r");
        }

        #endregion

        #region Object

        /// <summary>
        /// Enables you to get a string representation of the object using string 
        /// formatting with property names, rather than index based values.
        /// </summary>
        /// <param name="anObject">The object being extended.</param>
        /// <param name="aFormat">The formatting string, like "Hi, my name 
        /// is {FirstName} {LastName}".</param>
        /// <returns>A formatted string with the values from the object replaced 
        /// in the format string.</returns>
        /// <remarks>To embed a pair of {} on the string, simply double them: 
        /// "I am a {{Literal}}".</remarks>
        public static string ToString(this object anObject, string aFormat)
        {
            return anObject.ToString(aFormat, null);
        }

        /// <summary>
        /// Enables you to get a string representation of the object using string 
        /// formatting with property names, rather than index based values.
        /// </summary>
        /// <param name="anObject">The object being extended.</param>
        /// <param name="aFormat">The formatting string, like "Hi, my name 
        /// <param name="formatProvider">format is {FirstName} {LastName}".</param>
        /// <returns>A formatted string with the values from the object replaced 
        /// in the format string.</returns>
        /// <remarks>To embed a pair of {} on the string, simply double them: 
        /// "I am a {{Literal}}".</remarks>
        public static string ToString(this object anObject, string aFormat, IFormatProvider formatProvider)
        {
            if (anObject != null)
            {
                var sb = new StringBuilder();
                Type type = anObject.GetType();
                var reg = new Regex(@"({)([^}]+)(})", RegexOptions.IgnoreCase);
                MatchCollection mc = reg.Matches(aFormat);
                int startIndex = 0;
                foreach (Match m in mc)
                {
                    Group g = m.Groups[2]; //it's second in the match between { and }
                    int length = g.Index - startIndex - 1;
                    sb.Append(aFormat.Substring(startIndex, length));

                    string toGet;
                    string toFormat = String.Empty;
                    int formatIndex = g.Value.IndexOf(":"); //formatting would be to the right of a :
                    if (formatIndex == -1) //no formatting, no worries
                    {
                        toGet = g.Value;
                    }
                    else //pickup the formatting
                    {
                        toGet = g.Value.Substring(0, formatIndex);
                        toFormat = g.Value.Substring(formatIndex + 1);
                    }

                    Type retrievedType = null;
                    object retrievedObject = null;

                    //first try properties
                    PropertyInfo retrievedProperty = type.GetProperty(toGet);
                    if (retrievedProperty != null)
                    {
                        retrievedObject = retrievedProperty.GetValue(anObject, null);
                        retrievedType = retrievedProperty.PropertyType;
                    }
                    else
                    {
                        retrievedProperty = type.GetProperty("Item", BindingFlags.Public | BindingFlags.Instance, null, null, new[] { toGet.GetType() }, null);
                        if (retrievedProperty != null)
                        {
                            retrievedObject = retrievedProperty.GetValue(anObject, new object[] { toGet });
                            retrievedType = retrievedProperty.PropertyType;
                        }
                        else
                        {
                            //
                            // Try Fields
                            //                
                            FieldInfo retrievedField = type.GetField(toGet);
                            if (retrievedField != null)
                            {
                                retrievedType = retrievedField.FieldType;
                                retrievedObject = retrievedField.GetValue(anObject);
                            }
                        }
                    }

                    if (retrievedType != null) //Cool, we found something
                    {
                        object[] additional = null;
                        if (toFormat != String.Empty)
                            additional = new object[] { toFormat, formatProvider };
                        sb.Append(retrievedType.InvokeMember("ToString",
                                                             BindingFlags.Public | BindingFlags.NonPublic |
                                                             BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                                                             , null, retrievedObject, additional));
                    }
                    else //didn't find a property with that name, so be gracious and put it back
                        sb.Append("{" + g.Value + "}");

                    startIndex = g.Index + g.Length + 1;
                }
                if (startIndex < aFormat.Length) //include the rest (end) of the string
                {
                    sb.Append(aFormat.Substring(startIndex));
                }
                return sb.ToString();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this object container, string propName)
        {
            PropertyInfo info = container.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
            if (info != null)
            {
                return info.GetValue(container, null);
            }
            info = container.GetType().GetProperty("Item", BindingFlags.Public | BindingFlags.Instance, null, null, new[] { propName.GetType() }, null);
            if (info == null)
            {
                throw new ArgumentException("The object not the Indexed Accessor");
            }
            return info.GetValue(container, new object[] { propName });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this object container, string propName)
        {
            return (T)container.GetPropertyValue(propName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(this object container, string propName, object value)
        {
            PropertyDescriptor prop = TypeDescriptor.GetProperties(container).Find(propName, true);
            if (prop != null)
            {
                prop.SetValue(container, value);
            }
            else
            {
                PropertyInfo info = container.GetType().GetProperty("Item", BindingFlags.Public | BindingFlags.Instance, null, null, new[] { propName.GetType() }, null);
                if (info == null)
                {
                    throw new ArgumentException("The object not the Indexed Accessor");
                }
                info.SetValue(container, value, new object[] { propName });
            }
        }

        /// <summary>
        /// Copy the properties from a Entity to another entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="current"></param>
        public static void CopyPropertiesFrom<T>(this T original, object current)
        {
            if (current != null)
            {
                PropertyInfo[] currentProps = current.GetType().GetProperties();
                foreach (PropertyInfo prop in currentProps)
                    if ((prop.GetCustomAttributes(typeof(AssociationAttribute), true).Length == 0))
                    {
                        object newValue = prop.GetValue(current, null);
                        var propOriginal = original.GetType().GetProperty(prop.Name);
                        if (propOriginal.GetSetMethod() != null)
                            propOriginal.SetValue(original, newValue, null);
                    }
            }
        }

        /// <summary>
        /// Copy the properties from a Entity to another entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        //public static bool IsNew<T>(this T original)
        //{
        //    PropertyInfo[] currentProps = original.GetType().GetProperties();
        //    foreach (PropertyInfo prop in currentProps)
        //    {
        //        object[] attributes = prop.GetCustomAttributes(typeof(ColumnAttribute), true);

        //        if (attributes.Length == 1)
        //            if (((ColumnAttribute)attributes[0]).IsPrimaryKey)
        //                return true;
        //    }

        //    return false;
        //}

        /// <summary>
        /// Remove lazy loading pointers that create connections with database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        public static T Detach<T>(this T original)
        {
            if (original != null)
            {
                FieldInfo[] currentFields = original.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo fieldInfo in currentFields)
                {
                    if (fieldInfo.FieldType.Name.Contains("EntityRef"))
                        fieldInfo.SetValue(original, null);

                    if (fieldInfo.FieldType.Name.Contains("EntitySet"))
                    {
                        var list = fieldInfo.GetValue(original);
                        list.GetType().GetMethod("Load").Invoke(list, null);
                    }
                }
            }
            return original;
        }

        public static IQueryable<T> Detach<T>(this IQueryable<T> original)
        {
            return original.Select(t => t.Detach());
        }

        /// <summary>
        /// Load the properties from a Entity 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        public static T LoadAll<T>(this T original) where T : class
        {
            if (original != null)
            {
                PropertyInfo[] currentProps = original.GetType().GetProperties();
                foreach (PropertyInfo prop in currentProps)
                    prop.GetValue(original, null);
            }
            return original;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <returns></returns>
        public static T Duplicate<T>(this T original)
        {
            var entity = Activator.CreateInstance<T>();
            entity.CopyPropertiesFrom(original);
            return entity;
        }


        #endregion

        #region Float

        public static string AtFull(this decimal wvalor)
        {
            string[] wunidade = { "", " e um", " e dois", " e três", " e quatro", " e cinco", " e seis", " e sete", " e oito", " e nove" };
            string[] wdezes = { "", " e onze", " e doze", " e treze", " e quatorze", " e quinze", " e dezesseis", " e dezessete", " e dezoito", " e dezenove" };
            string[] wdezenas = { "", " e dez", " e vinte", " e trinta", " e quarenta", " e cinquenta", " e sessenta", " e setenta", " e oitenta", " e noventa" };
            string[] wcentenas = { "", " e cento", " e duzentos", " e trezentos", " e quatrocentos", " e quinhentos", " e seiscentos", " e setecentos", " e oitocentos", " e novecentos" };
            string[] wplural = { " bilhões", " milhões", " mil", "" };
            string[] wsingular = { " bilhão", " milhão", " mil", "" };
            string wextenso = "";
            string wfracao;

            string wnumero = wvalor.ToString("F").Replace(",", "").Replace(".", "");
            wnumero = wnumero.PadLeft(14, '0');
            if (Int64.Parse(wnumero.Substring(0, 12)) > 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    wfracao = wnumero.Substring(i * 3, 3);
                    if (int.Parse(wfracao) != 0)
                    {
                        if (int.Parse(wfracao.Substring(0, 3)) == 100) wextenso += " e cem";
                        else
                        {
                            wextenso += wcentenas[int.Parse(wfracao.Substring(0, 1))];
                            if (int.Parse(wfracao.Substring(1, 2)) > 10 && int.Parse(wfracao.Substring(1, 2)) < 20) wextenso += wdezes[int.Parse(wfracao.Substring(2, 1))];
                            else
                            {
                                wextenso += wdezenas[int.Parse(wfracao.Substring(1, 1))];
                                wextenso += wunidade[int.Parse(wfracao.Substring(2, 1))];
                            }
                        }
                        if (int.Parse(wfracao) > 1) wextenso += wplural[i];
                        else wextenso += wsingular[i];
                    }
                }
                if (Int64.Parse(wnumero.Substring(0, 12)) > 1) wextenso += " reais";
                else wextenso += " real";
            }
            wfracao = wnumero.Substring(12, 2);
            if (int.Parse(wfracao) > 0)
            {
                if (int.Parse(wfracao.Substring(0, 2)) > 10 && int.Parse(wfracao.Substring(0, 2)) < 20) wextenso = wextenso + wdezes[int.Parse(wfracao.Substring(1, 1))];
                else
                {
                    wextenso += wdezenas[int.Parse(wfracao.Substring(0, 1))];
                    wextenso += wunidade[int.Parse(wfracao.Substring(1, 1))];
                }
                if (int.Parse(wfracao) > 1) wextenso += " centavos";
                else wextenso += " centavo";
            }
            if (wextenso != "") wextenso = wextenso.Substring(3, 1).ToUpper() + wextenso.Substring(4);
            else wextenso = "Nada";
            return wextenso;
        }

        #endregion

        #region Conversions
        //public static int? ToInt(this object original)
        //{
        //    return ToInt(original, null);
        //}
        //public static int? ToInt(this object original, int? defaultValue)
        //{
        //    if (!String.IsNullOrEmpty(Convert.ToString(original)))
        //        return Convert.ToInt32(original);

        //    return defaultValue;
        //}

        #endregion
    }
}