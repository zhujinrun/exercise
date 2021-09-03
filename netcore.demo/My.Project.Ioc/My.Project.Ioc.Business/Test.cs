using My.Project.Ioc.Core.IService;
using My.Project.Ioc.IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Project.Ioc.Business
{
    public class Test : ITest, ITransientDependency
    {
        public string GetValue()
        {
            return "Success";
        }
    }
}
