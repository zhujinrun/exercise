using AbstractDataBase.Data.Template;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 改进后的DataBase
{
    public class DbProviderRegistry
    {
        private const string GroupName = "marvellousWorks.practicalPattern.shwoCase";
        private const string SectionName = "dbProviderMappings";
        private static NameValueCollection collection;
        static DbProviderRegistry()
        {
            collection = (NameValueCollection)ConfigurationManager.GetSection(GroupName + "/" + SectionName);
        }
        public string GetDbType(string providerName)
        {
            return collection[providerName];
        }
    }

    public static class DataBaseFactoryNew
    {
        private static DbProviderRegistry registry = new DbProviderRegistry();
        public static DataBase Create(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            string providerName = ConfigurationManager.ConnectionStrings[name].ProviderName;
            Type type = Type.GetType(registry.GetDbType(providerName));
            return (DataBase)Activator.CreateInstance(type, name);
        }
    }
}
