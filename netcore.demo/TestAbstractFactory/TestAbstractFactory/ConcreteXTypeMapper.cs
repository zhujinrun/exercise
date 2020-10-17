using System;
using System.Collections.Generic;
using System.Text;

namespace TestAbstractFactory
{
    public class ConcreteXTypeMapper:TypeMapperBase
    {
        public ConcreteXTypeMapper()
        {
            base.Add(typeof(IProductA), typeof(ProductA2X));
            base.Add(typeof(IProductB), typeof(ProductB2));
        }
    }

    public class ConcreteYTypeMapper : TypeMapperBase
    {
        public ConcreteYTypeMapper()
        {
            base.Add(typeof(IProductA), typeof(ProductA2Y));
            base.Add(typeof(IProductB), typeof(ProductB2));
        }
    }

    public class ConcreteFactoryX: AbstractFactoryBaseNew { }
    public class ConcreteFactoryY : AbstractFactoryBaseNew { }
}
