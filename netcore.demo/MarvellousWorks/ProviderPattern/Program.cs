using System;

namespace ProviderPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write(Message.Insert(new MessageModel("插入", DateTime.Now)));
            Console.Write("<br />");
            Console.Write(Message.Get()[0].Message + " " + Message.Get()[0].PublishTime.ToString());
            Console.ReadKey();
        }
    }
}
