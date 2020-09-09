using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace BuilderDesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            IBuilderP<Product> builderU = new ProductBulder();

            Product product = builderU.BuildUp();
            Console.WriteLine(product.Count);
            Console.WriteLine(product.Items.Count);

            product = builderU.TearDown();
            Console.WriteLine(product.Count);
            Console.WriteLine(product.Items.Count);


            IBuilder builder = new ConcreteBuilder();
            Car car = builder.BuildUp();


            Console.WriteLine("Hello World!");
        }
        private static IDictionary<Type, IList<BuildStepAttribute>> cache = new Dictionary<Type, IList<BuildStepAttribute>>();
        protected virtual IList<BuildStepAttribute> DiscoveryBuildSteps()
        {
            return null;
        }
    }
    public class Product
    {
        public int Count;
        public IList<int> Items;
    }
    public interface IBuilderP<T>
    {
        T BuildUp();
        T TearDown();
    }
    public class ProductBulder : IBuilderP<Product>
    {
        private Product product = new Product();

        private Random random = new Random();   
        public Product BuildUp()
        {
            product.Count = 0;
            product.Items = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                product.Items.Add(random.Next());
                product.Count++;
            }
            return product;
        }

        public Product TearDown()
        {
            while(product.Count > 0)
            {
                int val = product.Items[0];
                product.Items.Remove(val);
                product.Count--;
            }
            return product;
        }
    }
    public interface IBuilder<T> where T:class, new()
    {
        T BuildUp();
    }

    public class Builder<T> : IBuilder<T> where T : class, new()
    {
        public virtual T BuildUp()
        {
            IList<BuildStepAttribute> buildStepAttributeList = DiscoveryBuildSteps();

            if (buildStepAttributeList == null || buildStepAttributeList.Count == 0)
                return new T();
            T target = new T();
            foreach (var item in buildStepAttributeList)
            {
                for (int i = 0; i < item.Times; i++)
                {
                    item.Handler.Invoke(target, null);
                    
                }
            }
            return target;
        }

        protected virtual IList<BuildStepAttribute> DiscoveryBuildSteps()
        {
            IList<MethodInfo> methodInfos = AttributeHelper.GetMethodsWithCustomAttribute<BuildStepAttribute>(typeof(T));
            if (methodInfos == null || methodInfos.Count == 0)
                throw null;
            BuildStepAttribute[] attributes = new BuildStepAttribute[methodInfos.Count];

            for (int i = 0; i < methodInfos.Count; i++)
            {
                BuildStepAttribute attribute = AttributeHelper.GetMethodCustomAttribute<BuildStepAttribute>(methodInfos[i]);
                attribute.Handler = methodInfos[i];
                attributes[i] = attribute;
            }
            Array.Sort<BuildStepAttribute>(attributes);
            return new List<BuildStepAttribute>(attributes);
        }
    }
    public class TestBuilder
    {
        public class Car
        {
            public IList<string> Log = new List<string>();
            [BuildStep(2)]
            public void AddWheel()
            {
                Log.Add("wheel");
            }
            [BuildStep(1)]
            public void AddEngine()
            {
                Log.Add("engine");
            }
            [BuildStep(2,3)]
            public void AddBody()
            {
                Log.Add("body");
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class BuildStepAttribute : Attribute, IComparable
    {
        private int sequence;
        private int times;
        private MethodInfo handler;
        public BuildStepAttribute(int sequence, int times)
        {
            this.sequence = sequence;
            this.times = times;
        }

        public BuildStepAttribute(int sequence) : this(sequence, 1) { }

        public MethodInfo Handler
        {
            get { return handler; }
            set { this.handler = value; }
        }

        public int Sequence { get { return sequence; } }
        public int Times => times;
        public int CompareTo(object target)
        {
            if (target == null || target.GetType() != typeof(BuildStepAttribute))
                throw new ArgumentException("target");
            return this.sequence - ((BuildStepAttribute)target).sequence;
        }
    }
    public class AttributeHelper
    {
        /// <summary>
        /// 获取某个类型包括指定属性的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<T> GetCustomAttributes<T> (Type type) where T : Attribute
        {
            if (type == null)
                throw new ArgumentNullException("type");
            T[] attributes = (T[])(type.GetCustomAttributes(typeof(T), false));
            return (attributes.Length == 0) ? null : new List<T>(attributes);
        }
        /// <summary>
        /// 获取某个类型包括属性的所有方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<MethodInfo> GetMethodsWithCustomAttribute<T>(Type type) where T : Attribute
        {
            if (type == null)
                throw new ArgumentNullException("type");
            MethodInfo[] methodInfos = type.GetMethods();
            if (methodInfos == null || methodInfos.Length == 0)
                return null;
            IList<MethodInfo> result = new List<MethodInfo>();
            foreach (var item in methodInfos)
            {
                if (item.IsDefined(typeof(T), false))
                {
                    result.Add(item);
                }
            }
            return result.Count==0 ? null:result;
        }
        /// <summary>
        /// 获取某个方法指定类型属性的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        public static IList<T> GetMethodCustomAttributes<T>(MethodInfo method) where T : Attribute
        {
            if (method == null) throw new ArgumentNullException("method");
            T[] attributes = (T[])method.GetCustomAttributes(typeof(T),false);
            return attributes.Length == 0 ? null : new List<T>(attributes);
        }

        /// <summary>
        /// 获取某个方法指定类型的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        public static T GetMethodCustomAttribute<T>(MethodInfo method) where T : Attribute
        {
            if (method == null) throw new ArgumentNullException("method");
            IList<T> attributes = GetMethodCustomAttributes<T>(method);

            return attributes == null ? null : attributes[0];
        }  
    }
    public class Car
    {
        public void AddWheel() { Console.WriteLine("add wheel"); }
        public void AddEngine() { Console.WriteLine("add engine"); }
        public void AddBody() { Console.WriteLine("add body"); }
    }
    public interface IBuilder
    {
        Car BuildUp();
    }

    public delegate void BuilderStepHandler();
    public abstract class BuilderBase : IBuilder
    {
        protected IList<BuilderStepHandler> steps=new List<BuilderStepHandler>();
        protected Car car = new Car();
        public virtual Car BuildUp()
        {
            foreach (var step in steps)
            {
                step();
            }
            return car;
        }
    }

    public class ConcreteBuilder: BuilderBase
    {
        public ConcreteBuilder() : base()
        {
            steps.Add(car.AddBody);
            steps.Add(car.AddEngine);
            steps.Add(car.AddWheel);
        }
    }
}
