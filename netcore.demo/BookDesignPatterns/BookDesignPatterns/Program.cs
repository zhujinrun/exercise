using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
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

            ClientProduct clientProduct = new ClientProduct();
            IProduct[] prodcuts = clientProduct.Produce();


            IFactory factory = new Assember().Create<IFactory>();
            Client client = new Client(factory);


            IProduct product = client.GetProduct();

            IObjectBuilder builder = ConfigurationBroker.GetConfigurationObject<IObjectBuilder>();

            TypeCreator typeCreator = new TypeCreator();
            typeCreator.BuildUp<TestInjector>().Invoke();

            typeCreator.BuildUp<TestInjectorHasPars>(new object[] { "lilei", 12 }).Invoke();
            typeCreator.BuildUp<TestInjector>("BookDesignPatterns.TestInjector").Invoke();
            typeCreator.BuildUp<TestInjector>(typeof(TestInjector).Namespace + "." + nameof(TestInjector)).Invoke();

            IFactory<CalculateHandler> eventFactory = new CalculateHandlerFactory();
            CalculateHandler handler = eventFactory.Create();
            Console.WriteLine(handler(1, 2, 3));

            IAbstractFactory factoryNew = AssemblyFactory();
            IProductA pa = factoryNew.Createe<IProductA>();
            IProductB pb = factoryNew.Createe<IProductB>();


            IAbstractFactoryWithTypeMapper factoryWithTypeMapper = new ContrectFactoryX();
            AssemblyMechanism.Assembly(factoryWithTypeMapper);
            IProductXB xb = factoryWithTypeMapper.Createe<IProductXB>();


        }
        static IAbstractFactory AssemblyFactory()
        {
            IDictionary<Type, Type> dictionary = new Dictionary<Type, Type>();
            dictionary.Add(typeof(IProductA), typeof(ProductA1));
            dictionary.Add(typeof(IProductB), typeof(ProductB1));

            return new ConcreteFactory(dictionary);
        }
    }
   
    public class TestFactory
    {
        class ConcreteProduct : IProduct { public string Name => throw new NotImplementedException(); };
        class ConcreteFactory : IFactoryWithNotifier
        {
            public void Create(ObjectCreateHandler<IProduct> callback)
            {
                IProduct product = new ConcreteProduct();
                callback(product);
            }

            public IProduct Create()
            {
                return new ConcreteProduct();
            }
        }
        public class Subscribe
        {
            private IProduct product;
            public void SetProduct(IProduct product)
            {
                this.product = product;
            }
            public IProduct GetProduct()
            {
                return this.product;
            }
        }
        public void Test()
        {
            IFactoryWithNotifier factoryWithNotifier = new ConcreteFactory();
            Subscribe subscribe = new Subscribe();
            ObjectCreateHandler<IProduct> callback = new ObjectCreateHandler<IProduct>(subscribe.SetProduct);
            subscribe.GetProduct();
            factoryWithNotifier.Create(callback);
            subscribe.GetProduct();
        }
    }
    #region 抽象工厂
    public delegate void ObjectCreateHandler<T>(T newProduct);

    public interface IFactoryHandler
    {
        IProduct Create();
    }
    public interface IFactoryWithNotifier: IFactoryHandler
    {
        void Create(ObjectCreateHandler<IProduct> callback);
    }
    public interface IProductA { }
    public interface IProductB { }
    public interface IProductXA { }
    public class IProductXA2 : IProductXA { }
    public interface IProductXB { }
    public class IProductXB1 : IProductXB { }

    public interface IProductYA { }
    public class IProductYA2 : IProductYA { }
    public interface IProductYB { }
    public class IProductYB1 : IProductYB { }

    public interface IAbstractFactory
    {
        //IProductA CreateProductA();
        //IProductB CreateProductB();

        T Createe<T>() where T : class;
    }
    public abstract class TypeMapperBase:Dictionary<Type,Type>
    {

    }
    public class TypeMapperDictionary: Dictionary<Type, TypeMapperBase>
    {

    }
    public interface IAbstractFactoryWithTypeMapper: IAbstractFactory
    {
        TypeMapperBase Mapper { get; set; }
    }
    public abstract class AbstractFactoryMapperBase : IAbstractFactoryWithTypeMapper
    {
        protected TypeMapperBase mapper;
        public virtual TypeMapperBase Mapper 
        {
            get {
                return mapper;
            }
            set {
                mapper = value;
            }
        }

        public virtual T Createe<T>() where T : class
        {
            Type target = mapper[typeof(T)];
            return (T)Activator.CreateInstance(target);
        }
    }
    public class ContrectXTypeMapper:TypeMapperBase
    {
        public ContrectXTypeMapper()
        {
            base.Add(typeof(IProductXA), typeof(IProductXA2));
            base.Add(typeof(IProductXB), typeof(IProductXB1));
        }
    }
    public class ContrectYTypeMapper : TypeMapperBase
    {
        public ContrectYTypeMapper()
        {
            base.Add(typeof(IProductYA), typeof(IProductYA2));
            base.Add(typeof(IProductYB), typeof(IProductYB1));
        }
    }

    public class ContrectFactoryX : AbstractFactoryMapperBase { }
    public class ContrectFactoryY : AbstractFactoryMapperBase { }

    public static class AssemblyMechanism
    {
        private static TypeMapperDictionary dictionary =new TypeMapperDictionary();

        static AssemblyMechanism()
        {
            dictionary.Add(typeof(ContrectFactoryX), new ContrectXTypeMapper());
            dictionary.Add(typeof(ContrectFactoryY), new ContrectYTypeMapper());
        }
        public static void Assembly(IAbstractFactoryWithTypeMapper factory)
        {
            TypeMapperBase mapper = dictionary[factory.GetType()];
            factory.Mapper = mapper;
        }

    }
    public abstract class AbstractFacrotyBase : IAbstractFactory
    {
        protected IDictionary<Type, Type> mapper;
        public AbstractFacrotyBase(IDictionary<Type, Type> mapper)
        {
            this.mapper = mapper;
        }
        public virtual T Createe<T>() where T : class
        {
            if (mapper == null || mapper.Count == 0 || !mapper.ContainsKey(typeof(T)))
            {
                throw new ArgumentException("T");
            }
            Type target = mapper[typeof(T)];
            return (T)Activator.CreateInstance(target);

        }
    }

    public class ConcreteFactory : AbstractFacrotyBase
    {
        public ConcreteFactory(IDictionary<Type, Type> mapper) : base(mapper)
        {

        }
       
    }

    public class ProductA1 : IProductA { }
    public class ProductA2X : IProductA { }
    public class ProductA2Y : IProductA { }
    public class ProductB1 : IProductB { }
    public class ProductB2X : IProductB { }
    public class ProductB2Y : IProductB { }

    public class CreateFactory1 : IAbstractFactory
    {
        public T Createe<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public IProductA CreateProductA()
        {
            throw new NotImplementedException();
        }

        public IProductB CreateProductB()
        {
            throw new NotImplementedException();
        }
    }

    public class ConcreteFactoryT : IAbstractFactory
    {
        private Type typeA;
        private Type typeB;
        public ConcreteFactoryT(Type typeA, Type typeB)
        {

            this.typeA = typeA;
            this.typeB = typeB;
        }

        public T Createe<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public virtual IProductA CreateProductA()
        {
            return (IProductA)Activator.CreateInstance(typeA);
        }

        public virtual IProductB CreateProductB()
        {
            return (IProductB)Activator.CreateInstance(typeB);
        }
    }
    #endregion

    public delegate int CalculateHandler(params int[] items);

    public class Calculator
    {
        public int Add(params int[] items)
        {
            int result = 0;
            foreach (var item in items)
            {
                result += item;
            }
            return result;
        }
    }

    public class CalculateHandlerFactory : IFactory<CalculateHandler>
    {
        public CalculateHandler Create()
        {
            return (new Calculator()).Add;
        }
    }
    public interface IFactory<T>
    {
        T Create();
    }
    public abstract class FactoryBase<T> : IFactory<T> where T : new()
    {
        public virtual T Create()
        {
            return new T();
        }
    }
    public class ProductAFactory : FactoryBase<ProductA> { }
    public class ProductBFactory : FactoryBase<ProductB> { }

    public abstract class BatchFactoryBase<TCollection, TITem> : FactoryBase<TCollection> where TCollection : ProductCollection, new() where TITem : IProduct, new()
    {
        protected int quantity;
        public virtual int Quantity
        {
            set
            {
                this.quantity = value;
            }
        }
        public override TCollection Create()
        {
            if (quantity <= 0) throw new ArgumentException("quantity");
            TCollection collection = new TCollection();
            for (int i = 0; i < quantity; i++)
            {
                collection.Insert(new TITem());
            }
            return collection;
        }
    }
    public class BatchProdcutAFactory : BatchFactoryBase<ProductCollection, ProductA>
    {

    }
    public class BatchProdcutBFactory : BatchFactoryBase<ProductCollection, ProductB>
    {

    }
    public class ProductADecision : DecisionBase
    {
        public ProductADecision() : base(new BatchProductAFactory(), 2) { }

    }
    public class ProductBDecision : DecisionBase
    {
        public ProductBDecision() : base(new BatchProductBFactory(), 3) { }

    }
    public class ProductDirector : DirectorBase
    {
        public ProductDirector()
        {
            base.Insert(new ProductADecision());
            base.Insert(new ProductBDecision());
        }
    }
    public class ClientProduct
    {
        private DirectorBase director = new ProductDirector();
        public IProduct[] Produce()
        {
            ProductCollection collection = new ProductCollection();
            foreach (var item in director.Decisions)
            {
                collection += item.Factory.Create(item.Quantity);
            }
            return collection.Data;
        }
    }
    public interface IBatchFactory
    {
        ProductCollection Create(int quantity);
    }
    public class BatchProductFactoryBase<T> : IBatchFactory where T : IProduct, new()
    {
        public virtual ProductCollection Create(int quantity)
        {
            if (quantity <= 0) throw new ArgumentException();

            ProductCollection collection = new ProductCollection();
            for (int i = 0; i < quantity; i++)
            {
                collection.Insert(new T());
            }
            return collection;
        }
    }
    public class BatchProductAFactory : BatchProductFactoryBase<ProductA>
    {

    }
    public class BatchProductBFactory : BatchProductFactoryBase<ProductB>
    {

    }
    public abstract class DecisionBase
    {
        protected IBatchFactory factory;
        protected int quantity;
        public DecisionBase(IBatchFactory factory, int quantity)
        {
            this.factory = factory;
            this.quantity = quantity;
        }
        public virtual IBatchFactory Factory
        {
            get
            {
                return factory;
            }
        }
        public virtual int Quantity
        {
            get { return quantity; }
        }
    }
    public abstract class DirectorBase
    {
        protected IList<DecisionBase> decisions = new List<DecisionBase>();

        protected virtual void Insert(DecisionBase decision)
        {
            if (decision == null || decision.Factory == null || decision.Quantity < 0)
            {
                throw new ArgumentException("decision");

            }
            decisions.Add(decision);
        }
        public virtual IEnumerable<DecisionBase> Decisions
        {
            get
            {
                return decisions;
            }
        }
    }
    public class ProductCollection
    {
        private IList<IProduct> data = new List<IProduct>();

        public void Insert(IProduct product)
        {
            data.Add(product);
        }
        public void Insert(params IProduct[] product)
        {
            if (product == null || product.Length == 0)
            {
                return;
            }
            Array.ForEach<IProduct>(product, a => { data.Add(a); });
        }
        public void Remove(IProduct product)
        {
            data.Remove(product);
        }
        public void Clear()
        {
            data.Clear();
        }

        public IProduct[] Data
        {
            get
            {
                if (data == null || data.Count <= 0)
                {
                    return null;
                }
                IProduct[] products = new IProduct[data.Count];
                data.CopyTo(products, 0);
                return products;
            }
        }
        public int Count
        {
            get { return data.Count; }
        }
        public static ProductCollection operator +(ProductCollection collection, IProduct[] products)
        {
            ProductCollection result = new ProductCollection();
            if (!(collection == null || collection.Count == 0)) result.Insert(collection.Data);
            if (!(products == null || products.Length == 0)) result.Insert(products);
            return result;
        }

        public static ProductCollection operator +(ProductCollection source, ProductCollection target)
        {
            ProductCollection result = new ProductCollection();
            if (!(source == null || source.Count == 0)) result.Insert(source.Data);
            if (!(target == null || target.Count == 0)) result.Insert(target.Data);
            return result;
        }
    }
    public class Client
    {
        private IFactory factory;
        public Client(IFactory factory)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            this.factory = factory;
        }

        public IProduct GetProduct()
        {
            return factory.Create();
        }
    }
    public class Assember
    {
        private const string SectionName = "BookDesignPatterns";
        private const string FactoryName = "IFactory";

        private static Dictionary<Type, Type> dictionary = new Dictionary<Type, Type>();
        static Assember()
        {
            NameValueCollection nameValueCollection = (NameValueCollection)ConfigurationManager.GetSection(SectionName);
            for (int i = 0; i < nameValueCollection.Count; i++)
            {
                string target = nameValueCollection.GetKey(i);
                string source = nameValueCollection[i];

                dictionary.Add(Type.GetType(target), Type.GetType(source));
            }

        }

        public object Create(Type type)
        {
            if (type == null || !dictionary.ContainsKey(type))
            {
                throw new NullReferenceException();
            }
            Type targetTyep = dictionary[type];
            return Activator.CreateInstance(targetTyep);
        }

        public T Create<T>()
        {
            return (T)Create(typeof(T));
        }
    }
    public interface IProduct
    {
        string Name { get; }
    }

    public class ProductA : IProduct
    {
        public string Name => "A";
    }
    public class ProductB : IProduct
    {
        public string Name => "B";
    }

    public interface IFactory
    {
        IProduct Create();
    }

    public class FactoryA : IFactory
    {
        public IProduct Create()
        {
            return new ProductA();
        }
    }
    public class FactoryB : IFactory
    {
        public IProduct Create()
        {
            return new ProductB();
        }
    }
    public class GenericContextText
    {
        class WorkItem
        {
            private GenericContext context;
            private const string Key = "id";

            private static IList<string> works = new List<string>();
            public string Id { get { return context[Key] as string; } }

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
        protected void Page_Load(object sender, EventArgs e)
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
            if (isWeb && (HttpContext.Current.Items[ContextKey] == null))
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
                    cache[name] = value;
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
            if (cache == null)
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
            catch
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
