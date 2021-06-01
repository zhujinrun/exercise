using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractDataBase.Data.Template
{
    public class SqlServerDatabase : DataBase
    {
        public SqlServerDatabase(string name) : base(name) { }
        protected override string ParameterPrefix => "@";
    }
}
