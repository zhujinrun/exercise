using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
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
}
