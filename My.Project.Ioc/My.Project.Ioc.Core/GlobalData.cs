using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace My.Project.Ioc.Core
{
    public static class GlobalData
    {
        public static ConcurrentDictionary<string, Type> TypeDic = new ConcurrentDictionary<string, Type>();
        static GlobalData()
        {
            string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            AllFxAssemblies = Directory.GetFiles(rootPath, "*.dll")
                .Where(x => new FileInfo(x).Name.Contains(FXASSEMBLY_PATTERN))
                .Select(x => Assembly.LoadFrom(x))
                .Where(x => !x.IsDynamic)
                .ToList();

            AllFxAssemblies.ForEach(aAssembly =>
            {
                try
                {
                    AllFxTypes.AddRange(aAssembly.GetTypes());

                    {
                        aAssembly.GetTypes().ToList().ForEach(a => {
                            if (!TypeDic.ContainsKey(a.Name))
                            {
                                TypeDic.TryAdd(a.Name, a);
                            }
                        
                        });
                    }
                }
                catch
                {

                }
            });
        }

        /// <summary>
        /// 解决方案程序集匹配名
        /// </summary>
        public const string FXASSEMBLY_PATTERN = "My.Project.Ioc";

        /// <summary>
        /// 解决方案所有程序集
        /// </summary>
        public static readonly List<Assembly> AllFxAssemblies;

        /// <summary>
        /// 解决方案所有自定义类
        /// </summary>
        public static readonly List<Type> AllFxTypes = new List<Type>();
    }
}
