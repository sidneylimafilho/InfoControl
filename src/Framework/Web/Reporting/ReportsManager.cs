using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Data.Linq;
using System.Web;
using System.Web.UI;

using InfoControl.Data;
using InfoControl.Configuration;

namespace InfoControl.Web.Reporting
{
    [DataObject(true)]
    public partial class ReportsManager : BusinessManager<DataClasses.ReportsDataContext>
    {

        StringBuilder query;


        public ReportsManager(IDataAccessor container)
            : base(container)
        {

        }

        public IQueryable<DataClasses.Report> RetrieveAllReports()
        {
            return DbContext.Reports.OrderBy(rep => rep.Name);
        }

        public DataClasses.Report GetReport(Int32 reportId)
        {
            return RetrieveAllReports().Where(r => r.ReportId == reportId).FirstOrDefault();
        }

        #region ReportTablesSchema
        public IQueryable<DataClasses.ReportTablesSchema> RetrieveAllTablesSchema()
        {
            return DbContext.ReportTablesSchemas.OrderBy(table => table.Name);
        }

        public DataClasses.ReportTablesSchema GetTableSchema(int tableId)
        {
            return RetrieveAllTablesSchema().Where(t => t.ReportTablesSchemaId == tableId).FirstOrDefault();
        }

        public void Insert(DataClasses.ReportTablesSchema table)
        {
            DbContext.ReportTablesSchemas.InsertOnSubmit(table);
            DbContext.SubmitChanges();
        }

        public void Insert(DataClasses.ReportSort sort)
        {
            DbContext.ReportSorts.InsertOnSubmit(sort);
            DbContext.SubmitChanges();
        }

        public void Insert(DataClasses.ReportFilter filter)
        {
            DbContext.ReportFilters.InsertOnSubmit(filter);
            DbContext.SubmitChanges();
        }

        public void Insert(DataClasses.ReportColumn column)
        {
            DbContext.ReportColumns.InsertOnSubmit(column);
            DbContext.SubmitChanges();
        }

        public void Insert(DataClasses.Report report)
        {
            DbContext.Reports.InsertOnSubmit(report);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="original_x"></param>
        /// <param name="x"></param>
        public void Update(DataClasses.ReportTablesSchema original_x, DataClasses.ReportTablesSchema x)
        {
            //DbContext.ReportTablesSchemas.Attach(original_x);
            original_x.CopyPropertiesFrom(x);
            DbContext.SubmitChanges();
            DataManager.Commit();
        }

        public void Delete(DataClasses.ReportTablesSchema table)
        {
            DbContext.ReportTablesSchemas.DeleteAllOnSubmit(DbContext.ReportTablesSchemas.Where(t => t.ReportTablesSchemaId == table.ReportTablesSchemaId));
            DbContext.SubmitChanges();
            DataManager.Commit();
        }
        #endregion

        #region ReportColumnsSchema
        public IQueryable<DataClasses.ReportColumnsSchema> RetrieveAllColumnsSchema()
        {
            return DbContext.ReportColumnsSchemas.OrderBy(col => col.Name);
        }

        public IQueryable<DataClasses.ReportColumnsSchema> RetrieveColumnsSchema(int tableId)
        {
            return RetrieveAllColumnsSchema().Where(col => col.ReportTablesSchemaId == tableId);
        }

        public DataClasses.ReportColumnsSchema GetColumnSchema(int tableId, int columnId)
        {
            return RetrieveAllColumnsSchema().Where(col => col.ReportTablesSchemaId == tableId && col.ReportColumnsSchemaId == columnId).FirstOrDefault();
        }

        public void Insert(DataClasses.ReportColumnsSchema column)
        {
            DbContext.ReportColumnsSchemas.InsertOnSubmit(column);
            DbContext.SubmitChanges();
            DataManager.Commit();
        }

        public void Update(DataClasses.ReportColumnsSchema original_x, DataClasses.ReportColumnsSchema x)
        {
            //DbContext.ReportColumnsSchemas.Attach(original_x);
            original_x.CopyPropertiesFrom(x);
            DbContext.SubmitChanges();
            DataManager.Commit();
        }

        public void Delete(DataClasses.ReportColumnsSchema column)
        {
            DbContext.ReportColumnsSchemas.DeleteAllOnSubmit(RetrieveAllColumnsSchema().Where(t => t.ReportColumnsSchemaId == column.ReportColumnsSchemaId));

            DbContext.SubmitChanges();
            DataManager.Commit();
        }
        #endregion

        #region ReportFilterType
        public DataClasses.ReportFilterType GetFilterType(int filterId)
        {

            return RetrieveFilterTypes().Where(f => f.ReportFilterTypeId == filterId).FirstOrDefault();
        }

        public IQueryable<DataClasses.ReportDataType> RetrieveDataTypes()
        {
            return DbContext.ReportDataTypes;
        }

        public IQueryable<DataClasses.ReportFilterType> RetrieveFilterTypes()
        {
            return DbContext.ReportFilterTypes.OrderBy(filter => filter.Name);
        }

        public IQueryable<DataClasses.ReportFilterType> RetrieveFilterTypes(int dataTypeId)
        {
            return RetrieveFilterTypes().Where(f => f.ReportDataTypeId == dataTypeId);
        }

        public DataReader RetrieveFilterListItems(DataClasses.ReportColumnsSchema column)
        {
            string query = @"SELECT " + column.PrimaryKey + ", " + column.PrimaryLabelColumn + " FROM " + column.PrimaryTable;
            return DataManager.ExecuteReader(query);
        }

        public DataClasses.ReportFilterType RetrieveFilterType(string filterName)
        {
            return RetrieveFilterTypes().Where(t => t.Name == filterName).FirstOrDefault();
        }
        #endregion

        public DataTable ExecuteReport(DataClasses.ReportSettings settings)
        {
            string query = BuildQuery(settings);
            DataTable table = DataManager.ExecuteDataTable(query);
            return table;
        }

        private string BuildQuery(DataClasses.ReportSettings settings)
        {
            query = new StringBuilder();
            query.Append("SELECT ");
            BuildColumns(settings);
            BuildFrom(settings);

            //
            // WHERE
            //
            if (settings.Filters.Count > 0)
            {
                BuildFilter(settings);
            }


            //
            // GROUP BY
            //
            BuildGroupBy(settings);

            //
            // ORDER BY
            //
            if (settings.SortedColumns.Count > 0)
            {
                BuildSort(settings);
            }

            return query.ToString();
        }
        private void BuildColumns(DataClasses.ReportSettings settings)
        {


            foreach (DataClasses.ReportColumn column in settings.MatrixRows)
                AddColumnInQuery(column);

            foreach (DataClasses.ReportColumn column in settings.Columns)
                AddColumnInQuery(column);

            query.Append("Count(*) as Qtd, ");

            // Remove trailing ", "
            query.Remove(query.Length - 2, 2);
        }
        private void AddColumnInQuery(DataClasses.ReportColumn column)
        {
            DataClasses.ReportColumnsSchema columnSchema = null;
           // DataClasses.ReportDataFunction function;
            //if (column.ReportDataFunctionId == null)
            //{
            columnSchema = GetColumnSchema(column.ReportTablesSchemaId, column.ReportColumnsSchemaId);
            query.Append(columnSchema.Source + " as [" + columnSchema.Name + "], ");
            //}
            //else
            //{
            //    function = null;
            //    query.Append(function.SqlText.Replace("[=Column=]", columnSchema.Source) + ", ");
            //}
        }
        private void BuildSort(DataClasses.ReportSettings settings)
        {
            query.Append(" ORDER BY   ");
            foreach (DataClasses.ReportSort sort in settings.SortedColumns)
            {
                DataClasses.ReportColumnsSchema columnSchema = GetColumnSchema(sort.ReportTablesSchemaId, sort.ReportColumnsSchemaId);
                if (!columnSchema.Source.Contains(")"))
                    query.Append(columnSchema.Source + ((bool)sort.Ascending ? ", " : " DESC, "));
            }
            // Remove trailing ", "
            query.Remove(query.Length - 2, 2);
        }
        private void BuildFrom(DataClasses.ReportSettings settings)
        {
            DataClasses.ReportTablesSchema table = GetTableSchema(settings.Report.ReportTablesSchemaId.Value);
            query.Append(" FROM " + table.SqlText + " ");
        }
        private void BuildFilter(DataClasses.ReportSettings settings)
        {
            DataClasses.ReportFilterType filterType;
            string filterSql = "";
            DataClasses.ReportColumnsSchema columnSchema;
            query.Append(" WHERE ");


            foreach (DataClasses.ReportFilter filter in settings.Filters)
            {
                 //
                // Get filter type and column
                //
                filterType = GetFilterType(filter.ReportFilterTypeId);
                columnSchema = GetColumnSchema(filter.ReportTablesSchemaId, filter.ReportColumnsSchemaId);
                if (filter.ReportColumnsSchemaId > 0 && columnSchema.Source.Contains(")"))
                    continue;

                string parameterName = "@p" + Math.Abs(columnSchema.Source.GetHashCode());
               

                filterSql = filterType.SqlText;

                if (filterType.Name.ToUpper() == "LIST")
                {
                    //
                    // LIST filter needs a clause IN, then in first time 
                    // create parameter and append clause 'where', after 
                    // concatenate the values
                    //
                    
                    if (DataManager.Parameters.IndexOf(parameterName) == -1)
                    {
                        filterSql = filterSql.Replace("[=Column=]", columnSchema.ForeignKey)
                                             .Replace("[=Parameter=]", parameterName.Replace("@p", ""));
                        query.Append(filterSql + " AND ");
                        DataManager.Parameters.Add(parameterName, 0);
                    }

                    query.Replace(parameterName, filter.Value + ", " + parameterName.Replace("@p", ""));
                }
                else
                {
                    //
                    // String filter type
                    //
                    if ((DataType)filterType.ReportDataTypeId == DataType.String)
                        filter.Value = filterType.ReportFilterTypeId == 1 ? (filter.Value + "%") :
                                       filterType.ReportFilterTypeId == 2 ? ("%" + filter.Value + "%") :
                                       filterType.ReportFilterTypeId == 3 ? ("%" + filter.Value) : "";

                    DataManager.Parameters.Add(parameterName, filter.Value);
                    filterSql = filterSql.Replace("[=Column=]", filter.ReportColumnsSchemaId > 0 ? columnSchema.Source : filter.Name);
                    filterSql = filterSql.Replace("[=Parameter=]", parameterName);
                    query.Append(filterSql + " AND ");
                }
            }

            // Remove trailing " AND "
            query = query.Remove(query.Length - 5, 5);
        }
        private void BuildGroupBy(DataClasses.ReportSettings settings)
        {
            if (settings.MatrixRows.Count > 0 || settings.Columns.Count > 0)
            {
                query.Append(" GROUP BY    ");
                DataClasses.ReportColumnsSchema columnSchema;
                foreach (DataClasses.ReportColumn column in settings.MatrixRows)
                {
                    //if (column.ReportDataFunctionId == null)
                    //{
                    columnSchema = GetColumnSchema(column.ReportTablesSchemaId, column.ReportColumnsSchemaId);
                    if (!columnSchema.Source.Contains(")"))
                        query.Append(columnSchema.Source + ", ");
                    // }
                }

                foreach (DataClasses.ReportColumn column in settings.Columns)
                {
                    //if (column.ReportDataFunctionId == null)
                    //{
                    columnSchema = GetColumnSchema(column.ReportTablesSchemaId, column.ReportColumnsSchemaId);
                    if (!columnSchema.Source.Contains(")"))
                        query.Append(columnSchema.Source + ", ");
                    //}
                }

                // Remove trailing ", "
                query.Remove(query.Length - 2, 2);
            }
        }

    }

    public enum DataType
    {
        String = 1,
        Int32,
        DateTime,
        ForeignKey
    }
}
