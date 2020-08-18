using System;
using System.Security.Cryptography.X509Certificates;

namespace AbstractFactory
{
    class Program
    {
        static void Main(string[] args)
        {

            AbstractFactory factory1 = new ConcrectFactory1();
            

            Client clientAB = new Client(factory1);
            clientAB.Run();

            AbstractFactory factory2 = new ConcrectFactory2();
           

            Client clientXY = new Client(factory2);
            clientXY.Run();

            Console.Read();
        }
    }
    public abstract class AbstractFactory
    {
        public abstract AbstractProduct1 CreateProduct1();
        public abstract AbstractProduct2 CreateProduct2();
    }

    public class ConcrectFactory1 : AbstractFactory
    {
        public override AbstractProduct1 CreateProduct1()
        {
            return new ProductA();
        }

        public override AbstractProduct2 CreateProduct2()
        {
            return new ProductB();
        }
    }

    public class ConcrectFactory2 : AbstractFactory
    {
        public override AbstractProduct1 CreateProduct1()
        {
            return new ProductX();
        }

        public override AbstractProduct2 CreateProduct2()
        {
            return new ProductY();
        }
    }
    public class Client{

        public AbstractProduct1 _abstractProduct1;

        public AbstractProduct2 _abstractProduct2;

        public Client(AbstractFactory factory)
        {
                _abstractProduct1 = factory.CreateProduct1();
                _abstractProduct2 = factory.CreateProduct2();
        }

        public void Run()
        {
            _abstractProduct2.Interact(_abstractProduct1);
        }

    }

    public class ProductA: AbstractProduct1
    {

    }
    public class ProductB : AbstractProduct2
    {
        public override void Interact(AbstractProduct1 a)
        {
            Console.WriteLine(this.GetType().Name + " Interact with " + a.GetType().Name);
        }
    }

    public class ProductX : AbstractProduct1
    {

    }
    public class ProductY : AbstractProduct2
    {
        public override void Interact(AbstractProduct1 a)
        {
            Console.WriteLine(this.GetType().Name + " Interact with " + a.GetType().Name);
        }
    }

    public abstract class AbstractProduct1
    {

    }

    public abstract class AbstractProduct2
    {
        public abstract void Interact(AbstractProduct1 a);
    }


}
