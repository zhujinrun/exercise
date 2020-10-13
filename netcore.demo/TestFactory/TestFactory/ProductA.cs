using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    /// <summary>
    /// 具体产品类型A
    /// </summary>
    public class ProductA : IProduct
    {
        public string Name => "A";
    }
}
