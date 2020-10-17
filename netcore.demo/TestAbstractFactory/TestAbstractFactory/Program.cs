using System;

namespace TestAbstractFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            TestFactory tf = new TestFactory();
            tf.Test();
            //IAbstractFactory factory = new ConcreteFactory2();
            //IProductA productA = factory.CreateProductA();

            //IProductB productB = factory.CreateProductB();

            //IAbstractFactory factory = new Test().AssemblyFactory();
            //IProductA productA = factory.Create<IProductA>();
            //IProductB productB = factory.Create<IProductB>();

            //IAbstractFactoryWithTypeMapper factory = new ConcreteFactoryX();
            //AssemblyMechanism.Assembly(factory);
            //IProductB productB = factory.Create<IProductB>();

            //factory = new ConcreteFactoryY();
            //AssemblyMechanism.Assembly(factory);
            //IProductA productA = factory.Create<IProductA>();
            Console.Read();
        }
    }

    public delegate void ObjectCreateHandler<T>(T newProduct);
    public interface IProduct { }
    public interface IFactory
    {
        IProduct Create();
    }

    public interface IFactoryWithNotofier : IFactory
    {
        void Create(ObjectCreateHandler<IProduct> callback);
    }

    public class TestFactory
    {
        class ConcreteProduct : IProduct { }
        class ConcreteFactory : IFactoryWithNotofier
        {
            public void Create(ObjectCreateHandler<IProduct> callback)
            {
                IProduct product = Create();
                callback(product);
            }

            public IProduct Create()
            {
                return new ConcreteProduct();
            }
        }

        class Subscribe
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
            IFactoryWithNotofier factory = new ConcreteFactory();
            Subscribe subscribe = new Subscribe();
            ObjectCreateHandler<IProduct> callback = new ObjectCreateHandler<IProduct>(subscribe.SetProduct);
            Console.WriteLine($"委托执行前 {subscribe.GetProduct()}");
            factory.Create(callback);
            Console.WriteLine($"委托执行后 {subscribe.GetProduct()}");
        }
    }
}
