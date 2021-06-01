using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using System.Data;

namespace AbstractDataBase.Data
{
    public static class OracleHelper
    {
        private const string SystemTableRoot = "Table";
        private static DataSet DoExecuteDataSet(OracleCommand command,string[] tableNames)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (command.Connection == null) throw new ArgumentNullException("command.Connection");
            using(OracleDataAdapter adapter = new OracleDataAdapter(command))
            {
                if (tableNames != null)
                {
                    for (int i = 0; i < tableNames.Length; i++)
                    {
                        string systemTableName = (i == 0) ? SystemTableRoot:SystemTableRoot + i;
                        adapter.TableMappings.Add(systemTableName, tableNames[i]);
                        
                    }
  
                }
                DataSet result = new DataSet();
                adapter.Fill(result);
                return result;
            }
        }

        public static DataSet ExecuteDataSet(string connectionString, string commandText, string[] tableNames, params IDataParameter[] parameters)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                OracleCommand command = new OracleCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = commandText;
                if (parameters != null && parameters.Length > 0)
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                }
                return DoExecuteDataSet(command, tableNames);
            }
        }
        public static DataSet ExecuteDataSet(string connectionString,string commandText)
        {
            return ExecuteDataSet(connectionString, commandText, null);
        }
    }
}
