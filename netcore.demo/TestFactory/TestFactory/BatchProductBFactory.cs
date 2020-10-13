using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    //public class BatchProductBFactory:BatchProductFactoryBase<ProductB>
    //{
    //}

    public class BatchProductBFactory : BatchFactoryBase<ProductCollection, ProductB>
    {
    }
}
