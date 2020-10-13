using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    //public class BatchProductAFactory : BatchProductFactoryBase<ProductA>
    //{
    //}

    public class BatchProductAFactory : BatchFactoryBase<ProductCollection, ProductA> { }
}
