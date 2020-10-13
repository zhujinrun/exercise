using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    /// <summary>
    /// 具体工厂类型B
    /// </summary>
    public class FactoryB : IFactory
    {
        public IProduct Create()
        {
            return new ProductB();
        }
    }
}
