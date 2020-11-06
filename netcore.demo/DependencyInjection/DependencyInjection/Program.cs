using System;
using System.Collections.Generic;
using System.Timers;

namespace DependencyInjection
{
    class Program
    {
        static void Main(string[] args)
        {
            ITimeProvider timeProvider = (new Assembler().Create<ITimeProvider>)();
            Client client = new Client(timeProvider);
            ClientForProp clientForProp = new ClientForProp();
            clientForProp.TimeProvider = timeProvider;
            Console.WriteLine(client.GetYear());

            ClientAttr clientAttr = new ClientAttr();
            Console.WriteLine(clientAttr.GetYear());
            Console.Read();
        }
    }
    public interface ITimeProvider
    {
      public DateTime CurrentDate { get; }
    }
    public class TimeProvider: ITimeProvider
    {
        public DateTime CurrentDate => DateTime.Now;
    }
    public class Client
    {
        private ITimeProvider timeProvider;
        public Client(ITimeProvider timeProvider)
        {
            this.timeProvider = timeProvider;
        }
        public int GetYear()
        {
            
            return timeProvider.CurrentDate.Year;
        }
    }

    public class ClientForProp
    {
        private ITimeProvider timeProvider;
        public ITimeProvider TimeProvider { get => timeProvider; set => timeProvider = value; }
    }

    public class Assembler
    {
        private static Dictionary<Type, Type> dictionary = new Dictionary<Type, Type>();
        static Assembler()
        {
            dictionary.Add(typeof(ITimeProvider), typeof(TimeProvider));
        }
        public object Create(Type type)
        {
            if (type == null || !dictionary.ContainsKey(type))
                throw new NullReferenceException("");
            Type targetType = dictionary[type];
            return Activator.CreateInstance(targetType);
        }
        public T Create<T>()
        {
            return (T)Create(typeof(T));
        }
    }
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =true)]
    sealed class DecoratorAttribute : Attribute
    {
        public readonly object INjector;
        private Type type;
        public DecoratorAttribute(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            this.type = type;
            INjector = (new Assembler()).Create(this.type);
        }
        public Type Type { get => this.type; }
    }
    static class AttributeHelper
    {
        public static T Injector<T>(object target) where T:class
        {
            if (target == null) throw new ArgumentNullException("target");
            Type targetType = target.GetType();
            object[] attributes = targetType.GetCustomAttributes(typeof(DecoratorAttribute), false);
            if (attributes == null || attributes.Length <= 0) return null;
            foreach (DecoratorAttribute attribute in (DecoratorAttribute[])attributes)
            {
                if(attribute.Type == typeof(T))
                {
                    return (T)attribute.INjector;
                }
            }

            return null;
        }
    }
    [Decorator(typeof(ITimeProvider))]
    public class ClientAttr
    {
        public int GetYear()
        {
            ITimeProvider timeProvider = AttributeHelper.Injector<ITimeProvider>(this);
            return timeProvider.CurrentDate.Year;
        }
    }
}
