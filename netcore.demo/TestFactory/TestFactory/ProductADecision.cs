using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    public class ProductADecision:DecisionBase
    {
        public ProductADecision():base(new BatchProductAFactory(), 2) { }
    }
}
