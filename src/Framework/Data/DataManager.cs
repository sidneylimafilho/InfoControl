using System;
using System.Reflection;
using System.Configuration;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Data.Common;


#if !CompactFramework
#if LinqCTP
using System.Data.Linq;
#else
using System.Linq;
using System.Linq.Expressions;
using System.Data.Linq;
#endif
#else
using System.Data.SqlServerCe;
#endif


using InfoControl.Configuration;
using InfoControl.Web.Auditing;

namespace InfoControl.Data
{
    /// <summary>
    /// Summary description for DataManager.
    /// </summary>
#if !CompactFramework
    [ToolboxItem(true)]
#endif
    public partial class DataManager : Component
    {
        #region Variables
        private DbConnection _connection;
        private string _providerName;
        private string _connectionString;
        private DbCommand _command;
        private DbCommandBuilder _commandBuilder;
        private MethodInfo _getParameterName;
        private DataParameterCollection _parameters;
        private DbTransaction _transaction;
        private int _transactionDepth = 0;
        private bool _keepConnected = true;
        private string _connectionStringName;
        private DbDataReader dr = null;
        private Hashtable items;
        private Hashtable dataContexts = new Hashtable();
        private static Hashtable _CacheCommands;
        #endregion

        #region Properties

        /// <summary>
        /// Indica se mantem a conexão aberta ou não
        /// </summary>
#if !CompactFramework
        [Browsable(false)]
#endif
        public string ConnectionStringName
        {
            get { return _connectionStringName ?? AppConfig.DataAccess.ConnectionStringName; }
            set { _connectionStringName = value; }
        }


        /// <summary>
        /// Indica qual será o provedor de informações
        /// </summary>
        public string ProviderName
        {
            get { return _providerName ?? (_providerName = AppConfig.ConnectionStrings[ConnectionStringName].ProviderName); }
            set { _providerName = value; }
        }

#if CompactFramework
        /// <summary>
        /// Factory que cria os objetos da base de dados
        /// </summary>
        public SqlServerCeFactory DataSource
        {
            get
            {
                if (_dataSource == null)
                {
                    _dataSource = new SqlServerCeFactory();
                }
                return (_dataSource);
            }
        }
        private SqlServerCeFactory _dataSource;
#else
        /// <summary>
        /// Factory que cria os objetos da base de dados
        /// </summary>
        public DbProviderFactory DataSource { get { return _dataSource ?? (_dataSource = DbProviderFactories.GetFactory(ProviderName)); } }
        private DbProviderFactory _dataSource;
#endif
        public string ConnectionString
        {
            get
            {
                if (!String.IsNullOrEmpty(_connectionString)) return _connectionString;

                _connectionString = AppConfig.ConnectionStrings[ConnectionStringName].ConnectionString;

                if (String.IsNullOrEmpty(AppConfig.DataAccess.TenantConnectionStringName)) return _connectionString;

                var settings = AppConfig.ConnectionStrings[AppConfig.DataAccess.TenantConnectionStringName];

                if (settings == null) return _connectionString;
                if (String.IsNullOrEmpty(settings.ConnectionString)) return _connectionString;

                var connStr = settings.ConnectionString;

                // update datasource to tenant
                _dataSource = DbProviderFactories.GetFactory(settings.ProviderName);

                using (var conn = _dataSource.CreateConnection())
                {
                    conn.ConnectionString = _connectionString;
                    conn.Open();

                    var cmd = conn.CreateCommand();
                    cmd.CommandText = AppConfig.DataAccess.TenantGetQuery.Replace("@ID", GetTenantId());
                    var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    if (reader.Read())
                    {
                        for (var i = 0; i < reader.FieldCount - 1; i++)
                            connStr = connStr.Replace("{" + reader.GetName(i) + "}", Convert.ToString(reader.GetValue(i)));
                    }
                    else
                    {
                        // rollback datasource to tenant
                        _dataSource = DbProviderFactories.GetFactory(AppConfig.ConnectionStrings[ConnectionStringName].ProviderName);
                        return _connectionString;
                    }
                }

                return _connectionString = connStr;
            }
            set { _connectionString = value; }
        }


        public Hashtable CacheCommands
        {
            get
            {
                return _CacheCommands;
            }
        }

        public Hashtable Items { get { return items ?? (items = new Hashtable()); } }

        /// <summary>
        /// Retorna a transação atual com a base de dados
        /// </summary>
        public DbTransaction Transaction { get { return _transaction; } }

        /// <summary>
        /// Retorna a conexão com a base de dados
        /// </summary>
#if !CompactFramework
        [Browsable(false)]
#endif
        public DbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = DataSource.CreateConnection();
                    _connection.ConnectionString = ConnectionString;
                    _transaction = null;
                }

                return (_connection);
            }
        }


        /// <summary>
        /// Retorna um objeto Command associado com a conexão
        /// </summary>
#if !CompactFramework
        [Browsable(false)]
#endif
        public DbCommand Command
        {
            get
            {
                if (_command == null)
                {
                    _command = Connection.CreateCommand();
                }
                return (_command);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private DbCommandBuilder CommandBuilder
        {
            get
            {
                InitCommandBuilder();
                return _commandBuilder;
            }
        }

        private void InitCommandBuilder()
        {
            if (_commandBuilder == null)
            {
                _commandBuilder = DataSource.CreateCommandBuilder();
                _getParameterName = _commandBuilder.GetType().GetMethod("GetParameterName", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(string) }, null);
            }
        }


        /// <summary>
        /// Indica se mantem a conexão aberta ou não
        /// </summary>
#if !CompactFramework
        [Browsable(false)]
#endif
        public bool KeepConnected
        {
            get { return (_keepConnected); }
            set { _keepConnected = value; }
        }

        /// <summary>
        /// Retorna uma coleção de parametros
        /// </summary>
#if !CompactFramework
        [Browsable(false)]
#endif
        public DataParameterCollection Parameters
        {
            get
            {
                if (_parameters == null)
                    _parameters = new DataParameterCollection(Command);
                return (_parameters);
            }
        }

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Classe que empacota todas as operações com a base de dados
        /// </summary>
        /// <param name="container"></param>
        public DataManager(IContainer container)
        {
            //
            // Required for Windows.Forms Class Composition Designer support
            //
            container.Add(this);

            //
            // Required for Windows.Forms Class Composition Designer support
            //
            InitializeComponent();

            Initialize();

        }

        /// <summary>
        /// Classe que empacota todas as operações com a base de dados
        /// </summary>
        public DataManager()
        {

            //
            // Required for Windows.Forms Class Composition Designer support
            //
            InitializeComponent();

            Initialize();


        }

        static DataManager()
        {
            _CacheCommands = Hashtable.Synchronized(new Hashtable());
        }

        /// <summary>
        /// Classe que empacota todas as operações com a base de dados
        /// </summary>
        /// <param name="commitRequired"></param>
        public DataManager(bool commitRequired)
        {

            //
            // Required for Windows.Forms Class Composition Designer support
            //
            InitializeComponent();

            Initialize();

            this.KeepConnected = commitRequired;


        }

        void Initialize()
        {

            Executing = new ResolveEventHandler(OnExecuting);
            Executed = new ResolveEventHandler(OnExecuted);
        }



        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if ((this._connection != null) && (this._connection.State != ConnectionState.Closed) && !KeepConnected)
                {
                    this._connection.Close();
                    this._connection.Dispose();
                }
            }
            catch (Exception)
            {
                this._connection = null;
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }




        #endregion

        #region Utilities

        private string GetTenantId()
        {
            if (!String.IsNullOrEmpty(AppConfig.DataAccess.TenantCookieName))
                if (System.Web.HttpContext.Current != null)
                {
                    var cookie = System.Web.HttpContext.Current.Request.Cookies[AppConfig.DataAccess.TenantCookieName];
                    if (cookie != null)
                        return cookie.Value;
                }

            return AppConfig.DataAccess.TenantDefaultID;
        }


        /// <summary>
        /// Realiza todo o processo necessário para estabelecer uma conexão com a base de dados.
        /// </summary>
        public DbConnection GetOpenConnection()
        {
            return (GetOpenConnection(Connection));
        }

        /// <summary>
        /// Realiza todo o processo necessário para estabelecer uma conexão com a base de dados.
        /// </summary>
        public DbConnection GetOpenConnection(DbConnection conn)
        {
            Trace.TraceWarning("DataManager.GetOpenConnection()");
            if (conn.State != ConnectionState.Open)
                conn.Open();

            if (KeepConnected)
            {
                if (_transaction == null)
                    BeginTransaction();

                if (_transaction.Connection == null)
                    BeginTransaction();
            }

            Trace.TraceWarning("DataManager.GetOpenConnection()");
            return (conn);

        }


        private void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
            _transactionDepth++;
        }

        /// <summary>
        /// Commita a transação na base de dados
        /// </summary>
        public void Commit()
        {
            Trace.TraceWarning("DataManager.Commit()");
            if (_transaction != null)
            {
                if (_transactionDepth == 1)
                {
                    CheckOpenedDataReader();

                    //System.Web.HttpContext.Current.Trace.Warn("_transaction.Commit();");
                    _transaction.Commit();
                    _transactionDepth--;

                    _transaction = null;


                }
                else
                {
#if !CompactFramework
                    if (_transaction is System.Data.SqlClient.SqlTransaction)
                    {
                        (_transaction as System.Data.SqlClient.SqlTransaction).Save(_transactionDepth.ToString());
                    }
#endif
                }
            }

            CloseConnection();
            Trace.TraceWarning("DataManager.Commit()");
        }


        /// <summary>
        /// Desfaz a transação na base de dados
        /// </summary>
        public void Rollback()
        {
            Trace.TraceWarning("Begin Rollback");
            if (_transaction != null)
            {
                if (_transaction.Connection != null)
                {
                    CheckOpenedDataReader();

                    _transaction.Rollback();
                    _transactionDepth--;

                    _transaction = null;
                }
            }

            CloseConnection();
            Trace.TraceWarning("End Rollback");
        }

        /// <summary>
        /// Indica se a conexão está aberta ou não
        /// </summary>
        /// <returns>Verdadeiro: Conexão Aberta; Falso: Conexão Fechada</returns>
        private bool IsOpenConnection()
        {
            return (_connection.State == ConnectionState.Open);
        }

        /// <summary>
        /// Verifica se precisa fechar ou não a conexão
        /// </summary>
        private void VerifyKeepConnected()
        {
            if (!KeepConnected)
            {
                CloseConnection();
            }
        }

        internal void CloseConnection()
        {
            //
            // Only disconnect if dont logging, else Logging Module will disconnect
            //
            if (!AuditModule.IsAuditing)
            {
                if (_transaction != null)
                    Rollback();

                if (_connection != null && _connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                    Trace.TraceWarning("CloseConnection");
                }

                _connection = null;

                foreach (DataContext ctx in dataContexts.Values)
                    if (ctx.Connection != null && ctx.Connection.State == ConnectionState.Open)
                        ctx.Connection.Close();

                dataContexts = new Hashtable();
            }
        }

        /// <summary>
        /// Prepara um objeto command associado com o objeto connection
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <param name="commandType">Tipo de commando a ser executado</param>
        /// <returns>Objeto command preparado e pronto para uso.</returns>
        private DbCommand PrepareCommand(DbConnection connection, string commandText, System.Data.CommandType commandType)
        {
            connection = GetOpenConnection(connection);

            if (_command == null)
            {
                _command = connection.CreateCommand();
            }

            _command.Connection = connection;
            _command.CommandText = commandText;
            //System.Web.HttpContext.Current.Trace.Warn(commandText);
            _command.CommandType = commandType;

            if (_transaction != null)
                _command.Transaction = _transaction;

            return (_command);

        }

        /// <summary>
        /// Cria e prepara um objeto command pronto para uso
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <param name="commandType">Tipo de commando a ser executado</param>
        /// <returns>Objeto command preparado e pronto para uso.</returns>
        private DbCommand PrepareCommand(string commandText, System.Data.CommandType commandType)
        {
            return (PrepareCommand(Connection, commandText, commandType));
        }


        /// <summary>
        /// Prepara um objeto command associado com o objeto connection
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <param name="commandType">Tipo de commando a ser executado</param>
        /// <returns>Objeto command preparado e pronto para uso.</returns>
        private void AssignCommand(DbCommand command)
        {
            _command.CommandText = command.CommandText;
            _command.CommandTimeout = command.CommandTimeout;
            _command.CommandType = command.CommandType;
            _command.Connection = GetOpenConnection(Connection);

            if (_transaction != null)
                _command.Transaction = _transaction;
        }

        /// <summary>
        /// Cria um objeto command com uma conexão não gerenciada
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        private DbCommand CreateCommand(string commandText, System.Data.CommandType commandType)
        {
            DbConnection connection = GetOpenConnection(_dataSource.CreateConnection());
            DbCommand command = connection.CreateCommand();

            command.CommandText = commandText;
            command.CommandType = commandType;

            foreach (DbParameter param in _command.Parameters)
                command.Parameters.Add((new DataParameter(param)).Clone());


            return (command);
        }

        /// <summary>
        /// Checks if the had a DataReader is open
        /// </summary>
        private void CheckOpenedDataReader()
        {
            dr = FindLiveReader();

            //
            // Closes a DataReader opened
            //            
            if (dr != null)
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
            }
        }

        /// <summary>
        /// Finds a IDataReader that uses the current connection
        /// </summary>
        /// <returns></returns>
        public DbDataReader FindLiveReader()
        {
            try
            {
                System.Reflection.BindingFlags all = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
                object _intConnection = _connection.GetType().GetField("_innerConnection", all).GetValue(_connection);
                System.Reflection.MethodInfo method = _intConnection.GetType().GetMethod("FindLiveReader", all);
                return (DbDataReader)method.Invoke(_intConnection, all, null, new object[] { null }, null);
            }
            catch
            {
                return null;
            }
        }

        private string GetParameterName(string name)
        {
            InitCommandBuilder();
            object obj = _getParameterName.Invoke(_commandBuilder, new object[] { name });
            return obj.ToString();
        }


        #endregion

        #region Methods

        #region ExecuteCached

        public DataReader ExecuteCachedQuery(string cacheKey, params object[] args)
        {

            OnExecuting(this, new ResolveEventArgs("DataManager.ExecuteCachedQuery"));
            if (CacheCommands[cacheKey] != null)
            {
                try
                {
                    //
                    // Attach parameters
                    //
                    for (int i = 0; i < args.Length; i++)
                        Parameters.Add("@p" + i, args[i]);

                    AssignCommand(CacheCommands[cacheKey] as DbCommand);
                    dr = _command.ExecuteReader();
                    OnExecuted(this, new ResolveEventArgs("DataManager.ExecuteCachedQuery"));
                    return new DataReader(dr);
                }
                finally
                {
                    _command.Parameters.Clear();
                    _parameters.Clear();
                }
            }
            else
            {
                throw new ArgumentNullException("The cached command not exists!");
            }

        }


        //public DataReader ExecuteReaderAndCache<T>(string cacheKey, IQueryable<T> query)
        //{
        //    try
        //    {
        //        OnExecuting(this, new ResolveEventArgs());
        //        if (CacheCommands[cacheKey] != null)
        //        {
        //            AssignCommand(CacheCommands[cacheKey] as DbCommand);
        //            dr = _command.ExecuteReader();
        //            return new DataReader(dr);
        //        }
        //        else
        //        {
        //            CacheCommands[cacheKey] = CurrentContext.GetCommand(query);
        //            return query.ToDataReader();
        //        }
        //    }
        //    finally
        //    {
        //        _command.Parameters.Clear();
        //        OnExecuted(this, new ResolveEventArgs());
        //    }
        //}

        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// Executa uma instrução SQL e retorna o numero de registros afetados.
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <param name="commandType">Tipo de commando a ser executado</param>

        /// <returns>Quantidade de registros afetados</returns>
        public int ExecuteNonQuery(string commandText, System.Data.CommandType commandType)
        {
            int result;
            try
            {
                OnExecuting(this, new ResolveEventArgs("ExecuteNonQuery"));
                result = PrepareCommand(commandText, commandType).ExecuteNonQuery();
                OnExecuted(this, new ResolveEventArgs("ExecuteNonQuery"));
            }
            finally
            {
                _command.Parameters.Clear();
                VerifyKeepConnected();
            }
            return (result);
        }

        /// <summary>
        /// Executa uma instrução SQL e retorna o numero de registros afetados.
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <returns>Quantidade de registros afetados</returns>
        public int ExecuteNonQuery(string commandText)
        {
            return (ExecuteNonQuery(commandText, CommandType.Text));
        }
        #endregion

        #region ExecuteDataSet
        /// <summary>
        /// Executa uma instrução SQL e retorna um DataSet gen�rico com uma tabela sendo o resultado da query
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <param name="commandType">Tipo de commando a ser executado</param>

        /// <returns>DataSet gen�rico com uma tabela sendo o resultado da query</returns>
        public System.Data.DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            DataSet ds;

            OnExecuting(this, new ResolveEventArgs("ExecuteDataSet"));

            DbDataAdapter da = DataSource.CreateDataAdapter();
            da.SelectCommand = PrepareCommand(commandText, commandType);
            ds = new DataSet();
            da.Fill(ds);

            OnExecuted(this, new ResolveEventArgs("ExecuteDataSet"));

            _parameters = null;
            _command = null;
            VerifyKeepConnected();

            return (ds);
        }

        /// <summary>
        /// Executa uma instrução SQL e retorna um DataSet gen�rico com uma tabela sendo o resultado da query
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <returns>DataSet gen�rico com uma tabela sendo o resultado da query</returns>
        public System.Data.DataSet ExecuteDataSet(string commandText)
        {
            return (ExecuteDataSet(commandText, CommandType.Text));
        }

        #endregion

        #region ExecuteTable
        /// <summary>
        ///		Executa uma instrução SQL contra o objeto connection e retorna um objeto <c>DataReader</c>
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <param name="commandType">Tipo de commando a ser executado</param>
        /// <returns>Retorna um objeto DataReader</returns>
        public DataTable ExecuteDataTable(string commandText, System.Data.CommandType commandType)
        {
            return (ExecuteDataSet(commandText, commandType).Tables[0]);
        }

        /// <summary>
        ///		Executa uma instrução SQL contra o objeto connection e retorna um objeto <c>DataReader</c>
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <returns>Retorna um objeto DataReader</returns>
        public DataTable ExecuteDataTable(string commandText)
        {
            return (ExecuteDataTable(commandText, CommandType.Text));
        }

        #endregion

        #region RetrieveDataRow
        /// <summary>
        /// Executa uma instrução SQL e retorna a primeira linha de um DataSet gen�rico com uma tabela sendo o resultado da query
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <param name="commandType">Tipo de commando a ser executado</param>
        /// <returns>DataSet gen�rico com uma tabela sendo o resultado da query</returns>
        public System.Data.DataRow ExecuteDataRow(string commandText, System.Data.CommandType commandType)
        {
            DataTable table = ExecuteDataTable(commandText, commandType);
            return table.Rows.Count > 0 ? table.Rows[0] : (DataRow)null;
        }

        /// <summary>
        /// Executa uma instrução SQL e retorna a primeira linha de um DataSet gen�rico com uma tabela sendo o resultado da query
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <returns>DataSet gen�rico com uma tabela sendo o resultado da query</returns>
        public System.Data.DataRow ExecuteDataRow(string commandText)
        {
            return (ExecuteDataRow(commandText, CommandType.Text));
        }
        #endregion

        #region ExecuteTableReader
        /// <summary>
        ///		Executa uma instrução SQL contra o objeto connection e retorna um objeto <c>DataReader</c>
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <param name="commandType">Tipo de commando a ser executado</param>
        /// <returns>Retorna um objeto DataReader</returns>
        public DataTableReader ExecuteTableReader(string commandText, System.Data.CommandType commandType)
        {
            return (ExecuteDataTable(commandText, commandType).CreateDataReader());
        }

        /// <summary>
        ///		Executa uma instrução SQL contra o objeto connection e retorna um objeto <c>DataReader</c>
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <returns>Retorna um objeto DataReader</returns>
        public DataTableReader ExecuteTableReader(string commandText)
        {
            return (ExecuteTableReader(commandText, CommandType.Text));
        }

        #endregion

        #region ExecuteReader
        /// <summary>
        ///		Executa uma instrução SQL contra o objeto connection e retorna um objeto <c>DataReader</c>
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <param name="commandType">Tipo de commando a ser executado</param>
        /// <returns>Retorna um objeto DataReader</returns>
        public DataReader ExecuteReader(string commandText, System.Data.CommandType commandType)
        {
            try
            {
                CheckOpenedDataReader();

                OnExecuting(this, new ResolveEventArgs("ExecuteReader"));
                PrepareCommand(commandText, commandType);
                dr = _command.ExecuteReader(KeepConnected ? CommandBehavior.Default : CommandBehavior.CloseConnection);
                OnExecuted(this, new ResolveEventArgs("ExecuteReader"));
            }
            finally
            {
                if (_command != null)
                    _command.Parameters.Clear();
            }


            return (new DataReader(dr));
        }

        /// <summary>
        ///		Executa uma instrução SQL contra o objeto connection e retorna um objeto <c>DataReader</c>
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <returns>Retorna um objeto DataReader</returns>
        public DataReader ExecuteReader(string commandText)
        {
            return (ExecuteReader(commandText, CommandType.Text));
        }

        #endregion

        #region ExecuteReaderAndCount
        /// <summary>
        ///		Executa uma instrução SQL contra o objeto connection e retorna um objeto <c>DataReader</c>
        ///		<para>
        ///			Caso j� haja um DataReader para a conexão atual cria-se outro 
        ///			objeto connection e re-executa a instrução
        ///		</para>
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <param name="commandType">Tipo de commando a ser executado</param>
        /// <returns>Retorna um objeto DataReader</returns>
        public DataReader ExecuteReaderAndCount(string commandText, System.Data.CommandType commandType)
        {
            commandText = "select count(*) from (" + commandText + ") as _asdaip348h8fa9w8349324awidfaosjr0243; " + commandText;

            PrepareCommand(commandText, commandType);
            IDataReader dr = ExecuteReader(commandText, commandType);

            // pega o count e move para o resultset
            dr.Read();
            int recordAffected = dr.GetInt32(0);
            dr.NextResult();

            return (new DataReader(dr, recordAffected));
        }

        /// <summary>
        ///		Executa uma instrução SQL contra o objeto connection e retorna um objeto <c>DataReader</c>
        ///		<para>
        ///			Caso j� haja um DataReader para a conexão atual cria-se outro 
        ///			objeto connection e re-executa a instrução
        ///		</para>
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <returns>Retorna um objeto DataReader</returns>
        public DataReader ExecuteReaderAndCount(string commandText)
        {
            return (ExecuteReaderAndCount(commandText, CommandType.Text));
        }

        #endregion

        #region ExecuteScalar
        /// <summary>
        /// Executa uma instrução SQL e retorna o conte�do da primeira coluna do primeiro registro, os demais são automaticamente ignorados
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <param name="commandType">Tipo de commando a ser executado</param>
        /// <returns>Retorna o conte�do da primeira coluna do primeiro registro, os demais são automaticamente ignorados</returns>
        public object ExecuteScalar(string commandText, System.Data.CommandType commandType)
        {
            object result;
            try
            {
                OnExecuting(this, new ResolveEventArgs("DataManager.ExecuteScalar"));
                result = PrepareCommand(commandText, commandType).ExecuteScalar();
                OnExecuted(this, new ResolveEventArgs("DataManager.ExecuteScalar"));
            }
            finally
            {
                _command.Parameters.Clear();
            }

            return (result);
        }

        /// <summary>
        /// Executa uma instrução SQL e retorna o conte�do da primeira coluna do primeiro registro, os demais são automaticamente ignorados
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <returns>Retorna o conte�do da primeira coluna do primeiro registro, os demais são automaticamente ignorados</returns>
        public object ExecuteScalar(string commandText)
        {
            return (ExecuteScalar(commandText, CommandType.Text));
        }

        /// <summary>
        /// Executa uma instrução SQL e retorna o conte�do da primeira coluna do primeiro registro, os demais são automaticamente ignorados
        /// </summary>
        /// <param name="commandText">String representando a query SQL a ser executada</param>
        /// <returns>Retorna o conte�do da primeira coluna do primeiro registro, os demais são automaticamente ignorados</returns>
        public T ExecuteScalar<T>(string commandText)
        {
            return ((T)ExecuteScalar(commandText, CommandType.Text));
        }
        #endregion

        #region  ExecuteTable<Table>
        public Table ExecuteTable<Table>(string commandText, CommandType commandType) where Table : DataTable
        {
            Table dt;
            try
            {
                OnExecuting(this, new ResolveEventArgs("DataManager.ExecuteTable<Table>"));

                // Cria o objeto DataTable que retornar�
                dt = Activator.CreateInstance<Table>();

                // Prepara o command
                DbDataAdapter da = DataSource.CreateDataAdapter();
                da.SelectCommand = PrepareCommand(commandText, commandType);

                // Executa e preenche o DataTable
                DataSet ds = new DataSet();

                da.Fill(ds);
                dt.Merge(ds.Tables[0]);

                // Limpa os parametros e dispara o evento 
                OnExecuted(this, new ResolveEventArgs("DataManager.ExecuteTable<Table>"));
            }
            finally
            {
                _command.Parameters.Clear();
            }
            return (dt);
        }

        public Table ExecuteTable<Table>(string commandText) where Table : DataTable
        {
            return (ExecuteTable<Table>(commandText, CommandType.Text));
        }


        public Hashtable[] ExecuteHashtable(string commandText)
        {
            var list = new List<Hashtable>();
            var reader = ExecuteReader(commandText);
            while (reader.Read())
            {
                var hashTbl = new Hashtable();
                for (var i = 0; i < reader.FieldCount; i++)
                    hashTbl[reader.GetName(i)] = reader[i];

                list.Add(hashTbl);
            }
            return list.ToArray();

        }
        #endregion

        #region  ExecuteRow<Row>
        public Row ExecuteRow<Row>(string commandText, CommandType commandType) where Row : DataRow
        {
            DataTable dt;
            try
            {
                OnExecuting(this, new ResolveEventArgs("ExecuteRow<Row>"));

                Type rowType = typeof(Row);
                dt = (DataTable)rowType.Assembly.CreateInstance(rowType.FullName.Replace("Row", "DataTable"));

                // Prepara o command
                DbDataAdapter da = DataSource.CreateDataAdapter();
                da.SelectCommand = PrepareCommand(commandText, commandType);

                // Executa e preenche o DataTable
                DataSet ds = new DataSet();

                da.Fill(ds);
                dt.Merge(ds.Tables[0]);

                // Limpa os parametros e dispara o evento 
                OnExecuted(this, new ResolveEventArgs("ExecuteRow<Row>"));
            }
            finally
            {
                _command.Parameters.Clear();
            }

            return (dt.Rows.Count > 0 ? dt.Rows[0] as Row : null);
        }

        public Row ExecuteRow<Row>(string commandText) where Row : DataRow
        {
            return (ExecuteRow<Row>(commandText, CommandType.Text));
        }
        #endregion

#if !CompactFramework
        /// <summary> Create a DataContext and manage connection
        /// </summary>
        /// <typeparam name="T">System.Data.Linq.DataContext</typeparam>
        /// <returns></returns>
        public T CreateDataContext<T>() where T : DataContext
        {
            try
            {
                OnExecuting(this, new ResolveEventArgs("DataManager.CreateDataContext<T>"));


                if (dataContexts[typeof(T)] == null)
                    dataContexts[typeof(T)] = Activator.CreateInstance(typeof(T), GetOpenConnection());

                T _currentContext = (T)dataContexts[typeof(T)];
                _currentContext.Transaction = Transaction;

                _currentContext.Log = new DataContextTraceListener();

                return _currentContext;
            }
            finally
            {
                OnExecuted(this, new ResolveEventArgs("DataManager.CreateDataContext<T>"));
            }

        }
#endif

        #region Insert
        /// <summary>
        /// Insert a row in dtabase
        /// </summary>
        /// <param name="row"></param>        
        public int InsertRow<T>(T row) where T : DataRow
        {
            try
            {
                return BuildInsertCommand(row).ExecuteNonQuery();
            }
            finally
            {
                _command.Parameters.Clear();
            }
        }
        private DbCommand BuildInsertCommand(DataRow row)
        {
            StringBuilder plSQL = new StringBuilder();
            StringBuilder columnNames = new StringBuilder();
            StringBuilder parameterNames = new StringBuilder();


            foreach (DataColumn column in row.Table.Columns)
            {
                if (!column.AutoIncrement)
                {
                    columnNames.Append(column.ColumnName + ",");
                    parameterNames.Append(GetParameterName(column.ColumnName) + ",");
                    Parameters.Add(GetParameterName(column.ColumnName), row[column.ColumnName]);
                }
            }

            //
            // Trim , at end
            // 
            columnNames = columnNames.Remove(columnNames.Length - 1, 1);
            parameterNames = parameterNames.Remove(parameterNames.Length - 1, 1);

            //
            // Build a query
            //
            plSQL.Append(" INSERT INTO  " + row.Table.TableName);
            plSQL.Append(" (" + columnNames.ToString() + ") ");
            plSQL.Append(" VALUES ");
            plSQL.Append(" (" + parameterNames.ToString() + ") ");


            // TODO: Cacheable command
            return PrepareCommand(plSQL.ToString(), CommandType.Text);
        }
        #endregion

        #region Update
        public int UpdateRow<T>(T row) where T : DataRow
        {
            return UpdateRow(null, row);
        }

        public int UpdateRow<T>(T currentRow, T row) where T : DataRow
        {
            try
            {
                return BuildUpdateCommand(currentRow, row).ExecuteNonQuery();
            }
            finally
            {
                _command.Parameters.Clear();
            }
        }
        private DbCommand BuildUpdateCommand(DataRow currentRow, DataRow row)
        {
            StringBuilder plSQL = new StringBuilder();
            StringBuilder columnNames = new StringBuilder();
            StringBuilder whereColumnNames = new StringBuilder();

            foreach (DataColumn column in row.Table.Columns)
            {
                columnNames.Append(column.ColumnName);
                columnNames.Append(" = ");
                columnNames.Append(GetParameterName(column.ColumnName) + ",");
                Parameters.Add(GetParameterName(column.ColumnName), row[column.ColumnName]);
            }

            //
            // Concurrency Optimist
            //
            if (currentRow != null)
            {
                foreach (DataColumn column in row.Table.Columns)
                {
                    whereColumnNames.Append(column.ColumnName);
                    whereColumnNames.Append(" = ");
                    whereColumnNames.Append(GetParameterName("OLD" + column.ColumnName) + ",");
                    Parameters.Add(GetParameterName("OLD" + column.ColumnName), currentRow[column.ColumnName]);
                }
            }
            else
            {
                foreach (DataColumn column in row.Table.PrimaryKey)
                {
                    whereColumnNames.Append(column.ColumnName);
                    whereColumnNames.Append(" = ");
                    whereColumnNames.Append(GetParameterName("OLD" + column.ColumnName) + " AND ");
                    Parameters.Add(GetParameterName("OLD" + column.ColumnName), row[column.ColumnName]);
                }
            }

            //
            // trim 
            //
            columnNames = columnNames.Remove(columnNames.Length - 1, 1);
            whereColumnNames = whereColumnNames.Remove(whereColumnNames.Length - 4, 4);

            //
            // Build a query
            //
            plSQL.Append("UPDATE " + row.Table.TableName + " SET ");
            plSQL.Append(columnNames.ToString());
            plSQL.Append(" WHERE ");
            plSQL.Append(whereColumnNames.ToString());

            return PrepareCommand(plSQL.ToString(), CommandType.Text);

        }
        #endregion

        #region Delete
        public Int32 DeleteRow<T>(T row) where T : DataRow
        {
            try
            {
                return BuildDeleteCommand(row).ExecuteNonQuery();
            }
            finally
            {
                _command.Parameters.Clear();
            }
        }
        private DbCommand BuildDeleteCommand(DataRow row)
        {
            StringBuilder plSQL = new StringBuilder();
            StringBuilder whereColumnNames = new StringBuilder();

            foreach (DataColumn column in row.Table.PrimaryKey)
            {
                whereColumnNames.Append(column.ColumnName);
                whereColumnNames.Append(" = ");
                whereColumnNames.Append(GetParameterName(column.ColumnName) + ",");
                Parameters.Add(GetParameterName(column.ColumnName), row[column.ColumnName]);
            }

            //
            // trim 
            //            
            whereColumnNames = whereColumnNames.Remove(whereColumnNames.Length - 1, 1);

            //
            // Build a query
            //
            plSQL.Append("DELETE FROM " + row.Table.TableName);
            plSQL.Append(" WHERE ");
            plSQL.Append(whereColumnNames.ToString());

            return PrepareCommand(plSQL.ToString(), CommandType.Text);
        }
        #endregion

        #region Select
        public T Search<T>(T row) where T : DataRow
        {
            return ExecuteRow<T>(BuildSelectCommand<T>(row));
        }
        private string BuildSelectCommand<T>(T row) where T : DataRow
        {
            StringBuilder plSQL = new StringBuilder();
            StringBuilder whereColumnNames = new StringBuilder();

            ArrayList primaryKeys = new ArrayList();
            foreach (DataColumn column in row.Table.PrimaryKey)
                primaryKeys.Add(column);


            foreach (DataColumn column in row.Table.Columns)
            {
                if ((row[column.ColumnName] != null) && (Convert.ToString(row[column.ColumnName]) != "") && (Convert.ToString(row[column.ColumnName]) != "0") && (Convert.ToString(row[column.ColumnName]) != "-1") && !primaryKeys.Contains(column))
                {
                    whereColumnNames.Append(column.ColumnName);
                    whereColumnNames.Append(" = ");
                    whereColumnNames.Append(GetParameterName(column.ColumnName) + " AND ");
                    Parameters.Add(GetParameterName(column.ColumnName), row[column.ColumnName]);
                }
            }

            //
            // trim 
            //            
            whereColumnNames = whereColumnNames.Remove(whereColumnNames.Length - 4, 4);

            //
            // Build a query
            //
            plSQL.Append("SELECT * FROM " + row.Table.TableName);
            plSQL.Append(" WHERE ");
            plSQL.Append(whereColumnNames.ToString());

            return plSQL.ToString();
        }

        public T SearchByKey<T>(T row) where T : DataRow
        {
            return row = ExecuteRow<T>(BuildSelectByKeyCommand<T>(row));
        }
        private string BuildSelectByKeyCommand<T>(T row) where T : DataRow
        {
            StringBuilder plSQL = new StringBuilder();
            StringBuilder whereColumnNames = new StringBuilder();

            foreach (DataColumn column in row.Table.PrimaryKey)
            {
                whereColumnNames.Append(column.ColumnName);
                whereColumnNames.Append(" = ");
                whereColumnNames.Append(GetParameterName(column.ColumnName) + " AND ");
                Parameters.Add(GetParameterName(column.ColumnName), row[column.ColumnName]);
            }

            //
            // trim 
            //            
            whereColumnNames = whereColumnNames.Remove(whereColumnNames.Length - 4, 4);

            //
            // Build a query
            //
            plSQL.Append("SELECT * FROM " + row.Table.TableName);
            plSQL.Append(" WHERE ");
            plSQL.Append(whereColumnNames.ToString());

            return plSQL.ToString();
        }
        #endregion


        #endregion

        #region Events
        /// <summary>
        /// Dispara o evento notificando que já foi executado a operação pelo Helper
        /// </summary>
        public event ResolveEventHandler Executing;
        public Assembly OnExecuting(object sender, System.ResolveEventArgs e)
        {
            Trace.TraceInformation("Begin " + e.Name);
            return null;
        }

        public event ResolveEventHandler Executed;
        public Assembly OnExecuted(object sender, System.ResolveEventArgs e)
        {
            Trace.TraceInformation("End " + e.Name);
            if (!(e.Name.Contains("Reader") || e.Name.Contains("DataContext")) && !KeepConnected)
                CloseConnection();

            return null;
        }
        #endregion


    }

    internal class DataContextTraceListener : System.IO.TextWriter
    {
        public override Encoding Encoding
        {
            get { return System.Text.Encoding.Default; }
        }

        public override void Write(string format, object arg0)
        {
            Trace.TraceWarning(String.Format(format, arg0));
        }
        public override void Write(string format, object arg0, object arg1)
        {
            Trace.TraceWarning(String.Format(format, arg0, arg1));
        }
        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            Trace.TraceWarning(String.Format(format, arg0, arg1, arg2));
        }
        public override void Write(string value)
        {
            Trace.TraceWarning(value);
        }
        public override void Write(string format, params object[] arg)
        {
            Trace.TraceWarning(String.Format(format, arg));
        }

        public override void WriteLine(string format, object arg0)
        {
            Trace.TraceWarning(String.Format(format, arg0));
        }
        public override void WriteLine(string format, object arg0, object arg1)
        {
            Trace.TraceWarning(String.Format(format, arg0, arg1));
        }
        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            Trace.TraceWarning(String.Format(format, arg0, arg1, arg2));
        }
        public override void WriteLine(string value)
        {
            Trace.TraceWarning(value);
        }
        public override void WriteLine(string format, params object[] arg)
        {
            Trace.TraceWarning(String.Format(format, arg));
        }


    }
}

