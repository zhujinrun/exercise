using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractDataBase.Data.Template
{
    public static class DataBaseFactory
    {
        public static DataBase Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
            string providerName = ConfigurationManager.ConnectionStrings[name].ProviderName;
            switch (providerName)
            {
                case "System.Data.SqlClient":
                    return new SqlServerDatabase(name);
                case "System.Data.OracleClient":
                    return new OracleDatabase(name);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
