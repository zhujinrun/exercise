using System;
using System.Collections.Generic;
using System.Text;

namespace Builder_PatternDemo.Models
{
    public class ConcreteBuilder2 : Builder
    {
        Product product = new Product();
        public override void BuildPartA()
        {
            product.Add("X部件");
        }

        public override void BuildPartB()
        {
            product.Add("Y部件");
        }

        public override Product GetResult()
        {
            return product;
        }
    }
}
