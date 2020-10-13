using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    public abstract class FactoryBase<T> : IFactory<T> where T : new()
    {
        public virtual T Create()
        {
            return new T();
        }
    }
}
