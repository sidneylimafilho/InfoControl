using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Reflection;
using InfoControl.Data;


#if !CompactFramework
#if LinqCTP
using System.Data.Linq;
#else

#endif
#endif

namespace InfoControl
{
    public static class DataContextExtensions
    {
        /// <summary>
        /// Detach Entity 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="current"></param>
        //public static void Detach<T>(object current)
        //{
        //    T detached = new T();
        //    T clean = new T();            
        //    detached.CopyValuesFrom(current);
        //    current.CopyPropertiesFrom(detached);
        //}
        /// <summary>
        /// Transforms a <see cref="T:System.Data.Linq.Table<T>" /> into a SQL Query 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        //public static string ToSql<T>(this Table<T> source)
        //{
        //    return source.Context.GetQueryText(source);
        //}
        private static IDataReader GetReader(IEnumerable source)
        {
            return GetReader(source.GetEnumerator());
        }

        private static IDataReader GetReader(IEnumerator source)
        {
            BindingFlags all = BindingFlags.NonPublic | BindingFlags.Instance;
            //
            // Utilize reflection to get IDataReader underlying a IQureryable
            // 
            object reader = source;
            var fieldInfo = reader.GetType().GetField("session", all);
            if (fieldInfo != null)
            {
                reader = fieldInfo.GetValue(reader);
                reader = reader.GetType().GetField("dataReader", all).GetValue(reader);
                return (reader as IDataReader);
            }

            return null;
        }

        /// <summary>
        /// Create a DataReader to read the data and close a underlying IDataReader imediate after read all
        /// </summa
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataReader AsDataReader<T>(this IEnumerable<T> source)
        {
            return new DataReader(GetReader(source));
        }

        /// <summary>
        /// Create a DataReader to read the data and close a underlying IDataReader imediate after read all
        /// </summa
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ObjectReader<T> AsObjectReader<T>(this IEnumerable<T> source)
        {
            return new ObjectReader<T>(GetReader(source));
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            return ToDataTable(source, false);
        }

        /// <summary>
        /// Create a DataReader to read the data and close a underlying IDataReader imediate after read all
        /// </summa
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source, bool allProperties)
        {
            var table = new DataTable();
            IEnumerator<T> enu = source.GetEnumerator();
            IDataReader reader = GetReader(enu);
            using (enu)
                if (allProperties)
                {
                    bool loadedSchema = false;
                    PropertyInfo[] currentProps = null;

                    while (enu.MoveNext())
                    {
                        //
                        // Create the DataTableSchema
                        //
                        if (!loadedSchema)
                        {
                            currentProps = enu.Current.GetType().GetProperties();
                            foreach (PropertyInfo prop in currentProps)
                                if (!prop.PropertyType.FullName.Contains("EntitySet") && !prop.PropertyType.FullName.Contains("EntityRef") && prop.GetCustomAttributes(typeof(AssociationAttribute), true).Length == 0)
                                    table.Columns.Add(new DataColumn
                                                      {
                                                          ColumnName = prop.Name,
                                                          DataType = typeof(object)
                                                      });

                            loadedSchema = true;
                        }

                        //
                        // hidrates the DataTable
                        //
                        DataRow row = table.NewRow();

                        foreach (PropertyInfo prop in currentProps)
                            if (!prop.PropertyType.FullName.Contains("EntitySet") && !prop.PropertyType.FullName.Contains("EntityRef") && prop.GetCustomAttributes(typeof(AssociationAttribute), true).Length == 0)
                                row[prop.Name] = prop.GetValue(enu.Current, null);

                        table.Rows.Add(row);
                    }
                    if (reader != null)
                        reader.Close();
                }
                else
                    table.Load(reader);

            return table;
        }
    }
}