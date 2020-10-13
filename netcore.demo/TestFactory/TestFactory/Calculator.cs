using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TestFactory
{
    public delegate int CalculateHandler(params int[] items);
    public class Calculator
    {
        public int Add(params int[] items)
        {
            int result = 0;
            foreach (var item in items)
            {
                result += item;
            }
            return result;
        }
    }

    public class CalculateHandlerFactory : IFactory<CalculateHandler>
    {
        public CalculateHandler Create()
        {
            return (new Calculator()).Add;
        }
    }
}
