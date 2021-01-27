using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.API.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class IocFilter :Attribute, IFilterFactory
    {
        private readonly Type _type;
        public IocFilter(Type type)
        {
            _type = type;
        }
        public bool IsReusable => true;

        public IFilterMetadata CreateInstance (IServiceProvider serviceProvider)
        {
          return (IFilterMetadata) serviceProvider.GetService(_type);
        }
    }
}
