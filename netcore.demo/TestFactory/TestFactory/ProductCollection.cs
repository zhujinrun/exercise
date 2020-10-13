using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    public class ProductCollection
    {
        private IList<IProduct> data = new List<IProduct>();
        public void Insert(IProduct item)
        {
            data.Add(item);
        }
        public void Insert(IProduct[] items)
        {
            if (items == null || items.Length == 0) return;
            foreach (IProduct item in items)
            {
                data.Add(item);
            }
        }
        public void Remove(IProduct product)
        {
            data.Remove(product);
        }
        public void Clear() { data.Clear(); }
        public IProduct[] Data
        {
            get
            {
                if (data == null || data.Count == 0) return null;
                IProduct[] result = new IProduct[data.Count];
                data.CopyTo(result, 0);
                return result;
            }
        }
        public int Count { get { return data.Count; } }
        public static ProductCollection operator +(ProductCollection collection,IProduct[] items)
        {
            ProductCollection result = new ProductCollection();
            if(!(collection==null || collection.Count == 0))
            {
                result.Insert(collection.Data);
            }
            if(!(items==null || items.Length==0))
            {
                result.Insert(items);
            }
            return result;
        }
        public static ProductCollection operator +(ProductCollection source, ProductCollection target)
        {
            ProductCollection result = new ProductCollection();
            if (!(source == null || source.Count == 0))
            {
                result.Insert(source.Data);
            }
            if (!(target == null || target.Count == 0))
            {
                result.Insert(target.Data);
            }
            return result;
        }
    }
}
