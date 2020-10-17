using System;
using System.Collections.Generic;
using System.Text;

namespace TestAbstractFactory
{
    public interface IAbstractFactory
    {
        //IProductA CreateProductA();
        //IProductB CreateProductB();

        T Create<T>() where T : class;

    }
    public interface IAbstractFactoryNew
    {
        T Create<T>();
    }
}
