using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    public class Client
    {
        private DirectorBase director = new ProductDirector();
        public IProduct[] Produce()
        {
            ProductCollection collection = new ProductCollection();
            foreach (DecisionBase decision in director.Decisions)
            {
                collection += decision.Factory.Create(decision.Quantity);
            }
            return collection.Data;
        }
    }
}
