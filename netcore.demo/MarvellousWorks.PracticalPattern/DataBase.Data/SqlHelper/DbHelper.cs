using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractDataBase.Data.SqlHelper
{
    public static class DbHelper
    {
        public static DataSet ExecuteDataSet(string name,string commandText,string[] tableNames,params IDataParameter[] parameters)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");
            string connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            string providerName = ConfigurationManager.ConnectionStrings[name].ProviderName;
            switch (providerName)
            {
                case "System.Data.SqlClient":
                    return SqlDbHelper.ExecuteDataSet(connectionString, commandText, tableNames, parameters);
                case "System.Data.OracleString":
                    return OracleHelper.ExecuteDataSet(connectionString, commandText, tableNames, parameters);
                default:
                    throw new NotSupportedException();
            }
        }

        public static DataSet ExecuteDataSet(string name,string commandText)
        {
            return ExecuteDataSet(name, commandText, null);
        }
    }
}
