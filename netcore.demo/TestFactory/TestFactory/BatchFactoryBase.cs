using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    public class BatchFactoryBase<TCollection, TItem>:FactoryBase<TCollection> where TCollection : ProductCollection,new() where TItem:IProduct,new()
    {
        protected int quantity;

        public virtual int Quantity { set { this.quantity = value; } }

        public override TCollection Create()
        {
            if (quantity <= 0) throw new ArgumentException("quantity");
            TCollection collection = new TCollection();
            for (int i = 0; i < quantity; i++)
            {
                collection.Insert(new TItem());
            }
            return collection;
        }
    }
}
