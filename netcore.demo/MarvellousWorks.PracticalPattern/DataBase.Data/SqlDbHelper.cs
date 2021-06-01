using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractDataBase.Data
{
    public class SqlDbHelper
    {
        private const string Name = "LocalSQL";
        private const string SystemTableNameRoot = "Table";

        private readonly static string connectionString;

        static SqlDbHelper()
        {
            //connectionString = ConfigurationManager.ConnectionStrings[Name].ConnectionString;
        }

        public static DataSet ExecuteDataSet(string connectionString,string commandText,string[]tableNames,params IDataParameter[] parameters)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = commandText;
                if(parameters!=null&& parameters.Length > 0)
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                }
                return DoExecuteDataSet(command, tableNames);
            }
        }
        private static DataSet DoExecuteDataSet(SqlCommand command,string[] tableNames)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (command.Connection == null)
                throw new ArgumentNullException("command.connection");
            using(SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                if(tableNames != null)
                {
                    for (int i = 0; i < tableNames.Length; i++)
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

        public static DataSet ExecuteDataSet(string commandText,string[] tableNames,params SqlParameter[] paramters)
        {
            if (string.IsNullOrWhiteSpace(commandText)) throw new ArgumentNullException("commandText");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = commandText;
                if(paramters!=null && paramters.Length > 0)
                {
                    foreach (SqlParameter parameter in paramters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                return DoExecuteDataSet(command, tableNames);
            }
        }

        public static DataSet ExecuteDataSet(string commandText)
        {
            return ExecuteDataSet(commandText, null);
        }
    }
}
