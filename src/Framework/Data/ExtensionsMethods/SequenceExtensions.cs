using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace InfoControl.Data
{
    public static class SequenceExtensions
    {
        private static List<SortClause> CreateSortClauseList(string sortExpression, string defaultColumn)
        {
            var list = new List<SortClause>();

            if (string.IsNullOrEmpty(sortExpression))
            {
                if (string.IsNullOrEmpty(defaultColumn))
                    return list;

                sortExpression = defaultColumn;
            }

            string[] sortColumnList = sortExpression.Trim().Split(new[] { ',' });

            foreach (string sortColumn in sortColumnList)
            {
                string[] textArray = sortColumn.Trim().Split(new[] { ' ' });

                var direction = SortDirection.Ascending;
                if (textArray.Length > 1 && textArray[1].ToUpper() == "DESC")
                    direction = SortDirection.Descending;

                list.Insert(0, new SortClause(textArray[0], direction));
            }
            return list;
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> source, int startRowIndex, int maximumRows)
        {
            if (startRowIndex > 0)
            {
                source = source.Skip(startRowIndex);
            }
            if (maximumRows > 0)
            {
                source = source.Take(maximumRows);
            }
            return source;
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string sortExpression)
        {
            return Sort(source, sortExpression, "");
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string sortExpression, string defaultSortExpression)
        {
            List<SortClause> list = CreateSortClauseList(sortExpression, defaultSortExpression);

            foreach (SortClause clause in list)
            {
                //
                // Determines what the infer type and create a variable with name "x"            
                //
                ParameterExpression param = Expression.Parameter(typeof(T), "x");

                //
                // Get the parameter of variable 
                // EX: x.Name
                //
                MemberExpression column = Expression.Property(param, clause.SortColumn);

                //
                // Create LambdaExpression
                // EX: x => x.Name
                //
                LambdaExpression keySelector = Expression.Lambda(column, new[] { param });

                //
                // Get the OrderBy method and Invoke
                //
                MethodInfo methodInfo = null;
                if (clause.SortDirection == SortDirection.Ascending)
                {
                    methodInfo = FindQueryableMethod("OrderBy", new[] { typeof(T), column.Type });
                }
                else
                {
                    methodInfo = FindQueryableMethod("OrderByDescending", new[] { typeof(T), column.Type });
                }

                MethodCallExpression method = Expression.Call(null, methodInfo, new[] { source.Expression, Expression.Quote(keySelector) });
                source = source.Provider.CreateQuery<T>(method);
            }

            return source;
        }

        private static MethodInfo FindQueryableMethod(string name, params Type[] typeArgs)
        {
            MethodInfo info = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static).ToLookup(m => m.Name)[name].FirstOrDefault();

            if (info == null)
            {
                throw new InvalidOperationException(string.Format("There is no method '{0}' on class System.Linq.Queryable that matches the specified arguments", name));
            }
            if (typeArgs != null)
            {
                return info.MakeGenericMethod(typeArgs);
            }
            return info;
        }

        public static IQueryable<T> SortAndPage<T>(this IQueryable<T> source, string sortExpression, int startRowIndex, int maximumRows, string defaultSortExpression)
        {
            return Page(Sort(source, sortExpression, defaultSortExpression), startRowIndex, maximumRows);
        }

        #region Nested type: SortClause

        private class SortClause
        {
            private readonly string _sortColumn;
            private readonly SortDirection _sortDirection;

            public SortClause(string sortColumn, SortDirection sortDirection)
            {
                _sortColumn = sortColumn;
                _sortDirection = sortDirection;
            }

            public string SortColumn
            {
                get { return _sortColumn; }
            }

            public SortDirection SortDirection
            {
                get { return _sortDirection; }
            }
        }

        #endregion

        #region Nested type: SortDirection

        private enum SortDirection
        {
            Ascending,
            Descending
        }

        #endregion
    }
}