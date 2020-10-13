using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    public interface IBatchFactory
    {
        ProductCollection Create(int quantity);
    }
}
