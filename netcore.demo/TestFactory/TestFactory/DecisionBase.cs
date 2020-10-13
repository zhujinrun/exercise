using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    public abstract class DecisionBase
    {
        protected IBatchFactory factory;
        protected int quantity;
        public DecisionBase(IBatchFactory factory,int quantity)
        {
            this.factory = factory;
            this.quantity = quantity;
        }

        public virtual IBatchFactory Factory
        {
            get { return factory; }
        }
        public virtual int Quantity { get { return quantity; } }
            
    }
}
