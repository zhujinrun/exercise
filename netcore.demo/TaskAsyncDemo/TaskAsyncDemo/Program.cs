using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAsyncDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> listTasks = new List<Task>();
            for (int i = 0; i < 20000; i++)
            {
                int j = i;

                if (listTasks.Count(t=>t.Status!=TaskStatus.RanToCompletion) >= 5)
                {
                    Task.WaitAny(listTasks.ToArray());
                    listTasks = listTasks.Where(t => t.Status != TaskStatus.RanToCompletion).ToList();
                }
                else
                {
                    listTasks.Add(Task.Run(() => {
                        Thread.Sleep(3000);
                        Console.WriteLine(j);
                        Console.WriteLine("ManagedThreadId=" + Thread.CurrentThread.ManagedThreadId);

                    }));
                }
            }
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
