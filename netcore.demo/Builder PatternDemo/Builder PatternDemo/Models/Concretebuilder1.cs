using System;
using System.Collections.Generic;
using System.Text;

namespace Builder_PatternDemo.Models
{
    public class Concretebuilder1 : Builder
    {
        Product product = new Product();
        public override void BuildPartA()
        {
            product.Add("部件A");
        }

        public override void BuildPartB()
        {
            product.Add("部件B");
        }

        public override Product GetResult()
        {
            return product;
        }
    }
}
