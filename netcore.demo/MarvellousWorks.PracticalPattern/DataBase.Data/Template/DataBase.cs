using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractDataBase.Data.Template
{
    public abstract class DataBase
    {
        protected string name;
        protected string connectionString;
        protected DbProviderFactory factory;

        private const string SystemTableNameRoot = "Table";

        public DataBase(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            factory = DbProviderFactories.GetFactory(settings.ProviderName);
            connectionString = settings.ConnectionString;
            this.name = name;
        }

        protected abstract string ParameterPrefix { get; }

        private DataSet DoExecuteDataSet(DbCommand command,string[] tableNames)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (command.Connection == null) throw new ArgumentNullException("command.connection");
            using(DbDataAdapter adapter = factory.CreateDataAdapter())
            {
                adapter.SelectCommand = command;
                if (tableNames != null)
                {
                    for(int i = 0; i < tableNames.Length; i++)
                    {
                        string systemTableName = (i == 0) ? SystemTableNameRoot : SystemTableNameRoot + i;
                        adapter.TableMappings.Add(systemTableName, tableNames[i]);
                    }   
                }
                DataSet result = new DataSet();
                adapter.Fill(result);
                return result;
            }
        }
        protected virtual DbCommand CreateCommand(string commandText,params IDataParameter[] parameters)
        {
            DbCommand command = factory.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
            command.CommandText = commandText;
            if(parameters!=null && parameters.Length > 0)
            {
                foreach (var item in parameters)
                {
                    if (!item.ParameterName.StartsWith(ParameterPrefix))
                        item.ParameterName += ParameterPrefix;
                    command.Parameters.Add(item);
                }
            }
            return command;
        }
        public virtual DataSet ExecuteDataSet(string commandText,string[] tableNames,params IDataParameter[] parameters)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");

            using(DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = this.connectionString;
                DbCommand command = CreateCommand(commandText, parameters);
                command.Connection = connection;
                return DoExecuteDataSet(command, tableNames);
            }
        }
        public virtual DataSet ExecuteDataSet(string commandText)
        {
            return ExecuteDataSet(commandText, null);
        }
    }
}
