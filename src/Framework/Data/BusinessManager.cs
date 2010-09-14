using System;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Linq.Mapping;
#if !CompactFramework

#endif

namespace InfoControl.Data
{
    public class BusinessManager : IDataAccessor, IDisposable
    {
        private readonly DataManager _dataManager;

        public BusinessManager(IDataAccessor container)
        {
            _dataManager = (container != null) ? container.DataManager : new DataManager(false);
        }

        #region IDataAccessor Members

        public DataManager DataManager
        {
            get { return _dataManager; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (!DataManager.KeepConnected)
                DataManager.Dispose();
        }

        #endregion
    }

#if !CompactFramework
    public class BusinessManager<TDataContext> : BusinessManager where TDataContext : DataContext
    {
        private TDataContext _dbContext;

        public BusinessManager(IDataAccessor container)
            : base(container)
        {
        }

        public TDataContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = DataManager.CreateDataContext<TDataContext>();

                return _dbContext;
            }
        }

        public TEntity Get<TEntity>(Int32 id)where TEntity : class
        {
            var item = Activator.CreateInstance<TEntity>();
            var table = DbContext.GetTable<TEntity>();

            System.Reflection.PropertyInfo[] currentProps = item.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo prop in currentProps)
            {
                if ((prop.GetCustomAttributes(typeof(AssociationAttribute), true).Length == 0) &&
                    (prop.GetCustomAttributes(typeof(ColumnAttribute), true).Length == 1))
                {
                    var attr = prop.GetCustomAttributes(typeof(ColumnAttribute), true)[0] as ColumnAttribute;
                    if (attr.IsPrimaryKey)
                    {
                        //item.GetType().GetProperty(prop.Name).SetValue(item, id, null);

                        var exp = Expression.Parameter(typeof(TEntity), null);
                        var left = Expression.Property(exp, prop.Name);
                        var right = Expression.Constant(id);

                        try
                        {
                            return table.First(Expression.Lambda<Func<TEntity, bool>>(
                                Expression.Equal(left, right), 
                                new ParameterExpression[] { exp }));
                        }
                        catch
                        {
                            return default(TEntity);
                        }
                    }
                }
            }

            return default(TEntity);
        }

        //public void Insert(TEntity item)
        //{
        //    TDataContext context = DataManager.CreateContext<TDataContext>();
        //    Table<TEntity> table = context.GetTable<TEntity>();
        //    table.Add(item);
        //    context.SubmitChanges();
        //}

        //public void Delete(TEntity item)
        //{
        //    TDataContext context = DataManager.CreateContext<TDataContext>();
        //    Table<TEntity> table = context.GetTable<TEntity>();
        //    table.Attach(item);
        //    table.Remove(item);
        //    context.SubmitChanges();
        //}

        //public void Update(TEntity original_item, TEntity item)
        //{
        //    TDataContext context = DataManager.CreateContext<TDataContext>();
        //    Table<TEntity> table = context.GetTable<TEntity>();
        //    table.Attach(original_item);
        //    original_item.CopyPropertiesFrom(item);
        //    context.SubmitChanges();
        //}

        //public T
    }

    //public class BusinessManager<TDataContext, T> : BusinessManager<TDataContext>
    //    where T : class
    //{
    //    public BusinessManager(IDataAccessor container) : base(container) { }

    //    public Table<T> GetAll()
    //    {
    //        return (DbContext as DataContext).GetTable<T>();
    //    }

    //    public void Save(T entity)
    //    {
    //        if (entity.IsNew())
    //            GetAll().InsertOnSubmit(entity);
    //        else
    //            (DbContext as DataContext).SubmitChanges();
    //    }

    //    public void Update(T originalEntity, T entity)
    //    {
    //        DbContext.Refresh(RefreshMode.OverwriteCurrentValues, originalEntity);
    //        originalEntity.CopyPropertiesFrom(entity);
    //        DbContext.SubmitChanges();
    //    }

    //    public void Delete(T entity)
    //    {
    //        DbContext.GetTable<T>().InsertOnSubmit(entity);
    //        DbContext.SubmitChanges();
    //    }
    //}

#endif
}