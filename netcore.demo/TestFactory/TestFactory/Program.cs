using System;

namespace TestFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            IProduct[] products = client.Produce();
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine(products[i].Name);
            }
            for (int i = 2; i < 5; i++)
            {
                Console.WriteLine(products[i].Name);
            }
        }
    }
}
