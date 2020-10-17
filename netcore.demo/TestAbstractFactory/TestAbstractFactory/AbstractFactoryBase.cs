using System;
using System.Collections.Generic;
using System.Text;

namespace TestAbstractFactory
{
    #region IAbstractFactory
    public abstract class AbstractFactoryBase : IAbstractFactory
    {
        protected IDictionary<Type, Type> mapper;
        public AbstractFactoryBase(IDictionary<Type, Type> mapper)
        {
            this.mapper = mapper;
        }
        public virtual T Create<T>() where T : class
        {
            if (mapper == null || mapper.Count == 0 || !mapper.ContainsKey(typeof(T)))
                throw new ArgumentException("T");
            Type targetType = mapper[typeof(T)];
            return (T)Activator.CreateInstance(targetType);
        }
    }

    public class ConcreteFactory : AbstractFactoryBase
    {
        public ConcreteFactory(IDictionary<Type, Type> mapper) : base(mapper) { }
    }

    public class Test
    {
        public IAbstractFactory AssemblyFactory()
        {
            IDictionary<Type, Type> dictionary = new Dictionary<Type, Type>();
            dictionary.Add(typeof(IProductA), typeof(ProductA1));
            dictionary.Add(typeof(IProductB), typeof(ProductB1));
            return new ConcreteFactory(dictionary);
        }
    }
    #endregion

    #region IAbstractFactoryNew
    public interface IAbstractFactoryWithTypeMapper : IAbstractFactoryNew
    {
        TypeMapperBase Mapper { get; set; }
    }

    public abstract class AbstractFactoryBaseNew : IAbstractFactoryWithTypeMapper
    {
        protected TypeMapperBase mapper;

        public virtual TypeMapperBase Mapper { get => mapper; set => mapper = value; }

        public T Create<T>()
        {
            Type targetType = mapper[typeof(T)];
            return (T)Activator.CreateInstance(targetType);
        }
    } 
    #endregion
}
