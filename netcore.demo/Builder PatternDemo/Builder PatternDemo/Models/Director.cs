using System;
using System.Collections.Generic;
using System.Text;

namespace Builder_PatternDemo.Models
{
    public class Director
    {
        public void Construct(Builder builder)
        {
            builder.BuildPartA();

            builder.BuildPartB();
        }
    }
}
