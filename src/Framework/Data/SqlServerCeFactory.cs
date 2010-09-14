using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Collections.Generic;
using System.Text;

namespace InfoControl.Data
{
    public class SqlServerCeFactory
    {
        public SqlServerCeFactory()
        {

        }

        public SqlCeConnection CreateConnection()
        {
            return new SqlCeConnection();
        }

        public SqlCeCommand CreateCommand()
        {
            return new SqlCeCommand();
        }

        public SqlCeDataAdapter CreateDataAdapter()
        {
            return new SqlCeDataAdapter();
        }

        public SqlCeCommandBuilder CreateCommandBuilder()
        {
            return new SqlCeCommandBuilder();
        }

    }


    
    public class SqlCeCommandBuilder : System.Data.Common.DbCommandBuilder
    {
        public SqlCeCommandBuilder()
        {

        }

        protected override string GetParameterPlaceholder(int parameterOrdinal)
        {
            return ("@p" + parameterOrdinal.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        protected override string GetParameterName(int parameterOrdinal)
        {
            return ("@" + parameterOrdinal.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        protected override string GetParameterName(string parameterName)
        {
            return "@" + parameterName;
        }

        protected override void ApplyParameterInfo(DbParameter param, DataRow row, StatementType statementType, bool whereClause)
        {
            
        }

        protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
        {
            
        }

 





    }
}
