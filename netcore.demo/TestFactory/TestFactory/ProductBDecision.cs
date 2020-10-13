using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    public class ProductBDecision:DecisionBase
    {
        public ProductBDecision():base(new BatchProductBFactory(), 3) { }
    }
}
