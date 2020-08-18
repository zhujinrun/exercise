using System;
using System.Collections.Generic;
using System.Text;

namespace Builder_PatternDemo.Models
{
    public class Product
    {
        IList<string> parts = new List<string>();
        
        public void Add(string part)
        {
            parts.Add(part);
        }
        public void Show()
        {
            Console.WriteLine("\n 产品 创建=======");
            foreach (var item in parts)
            {
                Console.WriteLine(item);
            }
        }
    }
}
