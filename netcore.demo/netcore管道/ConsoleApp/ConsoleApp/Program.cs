using System;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ConsoleApp
{
    internal class Program
    {
        #region MyRegion
        //   static void Main(string[] args)
        //   {
        //       var throwIfNegative = new ActionBlock<int>(n =>
        //       {
        //           Console.WriteLine("n={0}", n);
        //           if (n < 0)
        //           {
        //               throw new ArgumentOutOfRangeException();
        //           }
        //       });
        //       throwIfNegative.Completion.ContinueWith((task) =>
        //       {
        //           Console.WriteLine("The status of the completion task is '{0}'.",
        //task.Status);

        //       });
        //       throwIfNegative.Post(0);
        //       throwIfNegative.Post(-1);
        //       throwIfNegative.Post(1);
        //       throwIfNegative.Post(-2);
        //       try
        //       {
        //           throwIfNegative.Completion.Wait();

        //       }
        //       catch (AggregateException ae)
        //       {
        //           ae.Handle(e =>
        //           {
        //               Console.WriteLine("Encountered {0}: {1}",
        //                  e.GetType().Name, e.Message);
        //               return true;
        //           });
        //       }
        //   } 
        #endregion
        static void Main(string[] args)
        {
            var bufferBlock = new BufferBlock<int>();

            for (int i = 0; i < 3; i++)
            {
                bufferBlock.Post(i);
            }
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(bufferBlock.Receive());
            }
            var broadcastBlock = new BroadcastBlock<double>(null);
            broadcastBlock.Post(Math.PI);
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(broadcastBlock.Receive());
            }

            var writeOnceBlock = new WriteOnceBlock<string>(null);

            Parallel.Invoke(
                           () => writeOnceBlock.Post("Message 1"),
                           () => writeOnceBlock.Post("Message 2"),
                           () => writeOnceBlock.Post("Message 3")
                           );
            Console.WriteLine(writeOnceBlock.Receive());
            Console.WriteLine(writeOnceBlock.Receive());

            var actionBlock = new ActionBlock<int>(n => Console.WriteLine(n));
            for (int i = 0; i < 3; i++)
            {
                actionBlock.Post(i * 10);
            }
            actionBlock.Complete();
            actionBlock.Completion.Wait();
            Console.WriteLine(actionBlock.Completion.Status);

            var transformBlock = new TransformBlock<int, double>(n => Math.Sqrt(n));
            transformBlock.Post(10);
            transformBlock.Post(20);
            transformBlock.Post(30);

            // Read the output messages from the block.
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(transformBlock.Receive());
            }
            var transformManyBlock = new TransformManyBlock<string, char>(s => s.ToCharArray());

            transformManyBlock.Post("Hello");
            transformManyBlock.Post("World");

            for (int i = 0; i < ("Hello"+"World").Length; i++)
            {
                Console.WriteLine(transformManyBlock.Receive());
            }

            var batchBlock = new BatchBlock<int>(10);
            for (int i = 0; i < 13; i++)
            {
                batchBlock.Post(i);
            }
            batchBlock.Complete();

            Console.WriteLine("The sum of the elements in batch 1 is {0}.",
   batchBlock.Receive().Sum());

            Console.WriteLine("The sum of the elements in batch 2 is {0}.",
               batchBlock.Receive().Sum());

            var joinBlock = new JoinBlock<int, int, char>();
            joinBlock.Target1.Post(3);
            joinBlock.Target1.Post(6);
            joinBlock.Target2.Post(5);
            joinBlock.Target2.Post(4);

            joinBlock.Target3.Post('+');
            joinBlock.Target3.Post('-');

            for (int i = 0; i < 2; i++)
            {
                var data = joinBlock.Receive();
                switch (data.Item3)
                {
                    case '+':
                        Console.WriteLine("{0} + {1} = {2}",
                           data.Item1, data.Item2, data.Item1 + data.Item2);
                        break;
                    case '-':
                        Console.WriteLine("{0} - {1} = {2}",
                           data.Item1, data.Item2, data.Item1 - data.Item2);
                        break;
                    default:
                        Console.WriteLine("Unknown operator '{0}'.", data.Item3);
                        break;
                }
            }

            Func<int, int> DoWork = n =>
             {
                 if (n < 0)
                 {
                     throw new ArgumentOutOfRangeException();
                 }
                 return n;
             };
            var batchedJoinBlock = new BatchedJoinBlock<int, Exception>(7);

            foreach (int i in new int[] { 5, 6, -7, -22, 13, 55, 0 })
            {
                try
                {
                    // Post the result of the worker to the
                    // first target of the block.
                    batchedJoinBlock.Target1.Post(DoWork(i));
                }
                catch (ArgumentOutOfRangeException e)
                {
                    // If an error occurred, post the Exception to the
                    // second target of the block.
                    batchedJoinBlock.Target2.Post(e);
                }
            }
            var results = batchedJoinBlock.Receive();
            foreach (var item in results.Item1)
            {
                Console.WriteLine(item);
            }
            foreach (var item in results.Item2)
            {
                Console.WriteLine(item.Message);
            }

            Channel<string> channel = Channel.CreateBounded<string>(100);

            Task.Run(async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    await channel.Writer.WriteAsync($"{i}");
                }
            });

            Task.Run(async () => { 
            while(await channel.Reader.WaitToReadAsync())
                {
                    if(channel.Reader.TryRead(out var msg))
                    {
                        Console.WriteLine($"读取值为: {msg}");
                    }
                }
            
            });
            Console.Read();

        }



    }
}
