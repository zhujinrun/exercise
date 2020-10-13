using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    /// <summary>
    /// 具体工厂类型A
    /// </summary>
    public class FactoryA : IFactory
    {
        public IProduct Create()
        {
            return new ProductA();
        }
    }
}
