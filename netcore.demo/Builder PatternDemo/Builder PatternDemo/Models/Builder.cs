using System;
using System.Collections.Generic;
using System.Text;

namespace Builder_PatternDemo.Models
{
    public abstract class Builder
    {
        public abstract void BuildPartA();
        public abstract void BuildPartB();

        public abstract Product GetResult();

    }
}
