using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BookDesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {

            IObjectBuilder builder = ConfigurationBroker.GetConfigurationObject<IObjectBuilder>();

            TypeCreator typeCreator = new TypeCreator();
            typeCreator.BuildUp<TestInjector>().Invoke();

            typeCreator.BuildUp<TestInjectorHasPars>( new object[] { "lilei", 12 }).Invoke();
            typeCreator.BuildUp<TestInjector>("BookDesignPatterns.TestInjector").Invoke();
            typeCreator.BuildUp<TestInjector>(typeof(TestInjector).Namespace+"."+nameof(TestInjector)).Invoke();
        }
    }
    public class GenericContextText
    {
        class WorkItem
        {
            private GenericContext context;
            private const string Key = "id";

            private static IList<string> works = new List<string>();
            public string Id { get { return context[Key] as string ; } }

            public void Start()
            {
                context = new GenericContext();
                context[Key] = Guid.NewGuid().ToString();
                works.Add(Id);
            }
            public static IList<string> Work { get { return works; } }

            public void Test()
            {
                Thread[] threads = new Thread[3];
                for (int i = 0; i < threads.Length; i++)
                {
                    threads[i] = new Thread(new ThreadStart(new WorkItem().Start));
                    threads[i].Start();
                }
                Thread.Sleep(1000);
            }
        }
    }
    public class GenericContextTest : System.Web.UI.Page
    {
        private const string Key = "id";
        private GenericContext context = new GenericContext();
        protected void Page_Load(object sender,EventArgs e)
        {
            context[Key] = Guid.NewGuid().ToString();
            Console.WriteLine(context[Key] as string);
            Console.WriteLine(context[Key] as string);
            Console.WriteLine(context[Key] as string);
        }
    }
    public class GenericContext
    {
        class NameBasedDictionary : Dictionary<string, object> { }
        [ThreadStatic]
        private static NameBasedDictionary threadCache;

        private static readonly bool isWeb = CheckWhetherIsWeb();
        private const string ContextKey = "BookDesignPatterns.context.web";

        public GenericContext()
        {
            if(isWeb&& (HttpContext.Current.Items[ContextKey]==null))
            {
                HttpContext.Current.Items[ContextKey] = new NameBasedDictionary();
            }
        }

        public object this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name)) return null;
                NameBasedDictionary cache = GetCache();
                if (cache.Count < 0) return null;
                object result;
                if (cache.TryGetValue(name, out result))
                    return result;
                else
                    return null;
            }
            set
            {
                if (string.IsNullOrEmpty(name)) return;
                NameBasedDictionary cache = GetCache();
                object temp;
                if (cache.TryGetValue(name, out temp))
                {
                    cache[name]=value;
                }
                else
                {
                    cache.Add(name, value);
                }
            }
        }

        private NameBasedDictionary GetCache()
        {
            NameBasedDictionary cache;
            if (isWeb)
            {
                cache = (NameBasedDictionary)HttpContext.Current.Items[ContextKey];
            }
            else
            {
                cache = threadCache;
            }
            if(cache==null) 
                cache = new NameBasedDictionary();
            {
                if (isWeb)
                {
                    HttpContext.Current.Items[ContextKey] = cache;
                }
                else
                {
                    threadCache = cache;
                }
            }
            return cache;
        }

        private static bool CheckWhetherIsWeb()
        {
            bool result = false;
            AppDomain domain = AppDomain.CurrentDomain;
            try
            {
                if (domain.ShadowCopyFiles)
                {
                    result = (HttpContext.Current.GetType() != null);
                }
            }
            catch(Exception ex)
            {

            }
            return result;
        }
    }
    #region 反射 泛型缓存 配置
    public interface IConfigurationSource
    {
        void Load();
    }
    public static class ConfigurationBroker
    {
        private static readonly GenericCache<Type, object> cache;
        static ConfigurationBroker()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cache = new GenericCache<Type, object>();
            foreach (var item in config.SectionGroups)
            {
                if (typeof(IConfigurationSource).IsAssignableFrom(item.GetType()))
                {
                    ((IConfigurationSource)item).Load();
                }
            }

        }

        public static void Add(Type type, object item)
        {
            if (type == null || item == null)
                throw new NullReferenceException();
            cache.Add(type, item);
        }

        public static void Add(KeyValuePair<Type, object> item)
        {
            Add(item.Key, item.Value);
        }

        public static void Add(object item)
        {
            Add(item.GetType(), item);
        }
        public static T GetConfigurationObject<T>() where T : class
        {
            if (cache.Count <= 0)
                return null;
            object result;
            if (!cache.TryGetValue(typeof(T), out result))
                return null;
            else return (T)result;
        }
    }
    /// <summary>
    /// 字典
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class GenericCache<TKey, TValue>
    {
        private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        private ReaderWriterLock rwLock = new ReaderWriterLock();

        private readonly TimeSpan lockTimeOut = TimeSpan.FromMilliseconds(100);

        public void Add(TKey key, TValue value)
        {
            bool isExisting = false;
            rwLock.AcquireWriterLock(lockTimeOut);
            try
            {
                if (!dictionary.ContainsKey(key))
                    dictionary.Add(key, value);
                else
                    isExisting = true;
            }
            catch
            {

            }
            finally
            {
                rwLock.ReleaseWriterLock();
                if (isExisting)
                {
                    throw new IndexOutOfRangeException();
                }
            }

        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            rwLock.AcquireReaderLock(lockTimeOut);
            value = default(TValue);
            bool result = false;
            try
            {
                result = dictionary.TryGetValue(key, out value);
            }
            catch
            {

            }
            finally
            {
                rwLock.ReleaseReaderLock();
            }
            return result;
        }

        public void Clear()
        {
            if (dictionary.Count > 0)
            {
                rwLock.AcquireWriterLock(lockTimeOut);
                try
                {
                    dictionary.Clear();
                }
                finally
                {
                    rwLock.ReleaseWriterLock();
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            if (dictionary.Count <= 0)
            {
                return false;
            }
            bool result = false;
            rwLock.AcquireReaderLock(lockTimeOut);
            try
            {
                result = dictionary.ContainsKey(key);
            }
            finally
            {
                rwLock.ReleaseReaderLock();
            }
            return result;
        }
        public int Count
        {
            get { return dictionary.Count; }
        }
    }

    public class TestInjectorHasPars
    {
        private string name;
        private int age;
        public TestInjectorHasPars(string name, int age)
        {
            this.name = name;
            this.age = age;
        }
        public void Invoke()
        {
            Console.WriteLine(name + " " + age);
        }
    }
    public class TestInjector
    {
        public void Invoke()
        {
            Console.WriteLine("正确调用");
        }
    }
    public class TypeCreator : IObjectBuilder
    {
        public T BuildUp<T>(object[] args)
        {
            object result = Activator.CreateInstance(typeof(T), args);
            return (T)result;
        }

        public T BuildUp<T>() where T : new()
        {
            return Activator.CreateInstance<T>();
        }

        public T BuildUp<T>(string typeName)
        {
            return (T)Activator.CreateInstance(Type.GetType(typeName));
        }

        public T BuildUp<T>(string typeName, object[] args)
        {
            return (T)Activator.CreateInstance(Type.GetType(typeName), args);
        }
    }

    public interface IObjectBuilder
    {
        T BuildUp<T>(object[] args);
        T BuildUp<T>() where T : new();
        T BuildUp<T>(string typeName);
        T BuildUp<T>(string typeName, object[] args);
    } 
    #endregion
}
