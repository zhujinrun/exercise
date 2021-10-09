using System;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            (_ = (Action<string>)((string str) => { Console.Write(str); }))("你见过这个东西吗 ");

            var bs = new
            {             
                Print = (Action<string>)((string str) => { Console.WriteLine(str); })
            };
            bs.Print(" Jack");
            Console.Read();
        }
    }
}
