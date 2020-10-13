using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    public class ProductDirector:DirectorBase
    {
        public ProductDirector()
        {
            base.Insert(new ProductADecision());
            base.Insert(new ProductBDecision());
        }
    }
}
