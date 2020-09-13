using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;

namespace CommandDesign
{
    class Program
    {
        static void Main(string[] args)
        {
            //    Receiver receiver = new Receiver();
            //    ICommand command1 = new SetAddressCommand();
            //    command1.Receiver = receiver;
            //    ICommand command2 = new SetNameCommand();
            //    command2.Receiver = receiver;
            //    Console.WriteLine($"name={receiver.Name}");
            //    Console.WriteLine($"name={receiver.Address}");
            //    Console.WriteLine("..............");
            //    Invoker invoker = new Invoker();
            //    invoker.AddCommand(command1);
            //    invoker.AddCommand(command2);
            //    invoker.Run();
            //    Console.WriteLine($"name={receiver.Name}");
            //    Console.WriteLine($"name={receiver.Address}");

            //    KolAsset kolAsset = new KolAsset();
            //    kolAsset.AssetType = new List<AssetType> {AssetType.Youtube,AssetType.FaceBook };

            //    Invoker1 invoker1 = new Invoker1();
            //    invoker1.AddHandler(new Receiver1().A);
            //    invoker1.AddHandler(new Receiver2().B);
            //    invoker1.AddHandler(Receiver3.C);
            //    invoker1.Run();

            //CommandTest2.DemoCommand demoCommand = new CommandTest2.DemoCommand();
            //demoCommand.Execute();
            //Console.WriteLine(demoCommand.log.Count);
            //Console.WriteLine(demoCommand.log[0]);

            //CommandTest2.DemoCommand demoCommand2 = new CommandTest2.DemoCommand();
            //demoCommand2.AsyncExecute();
            //Console.WriteLine(demoCommand2.log.Count);
            //Console.WriteLine(demoCommand2.log[0]);
            //Thread.Sleep(2000);

            //CommandTest3.FederateCommand command = new CommandTest3.FederateCommand(new CommandTest3.CommandA(),
            //    new CommandTest3.CommandB(),new CommandTest3.CommandC());
            //command.A();
            //command.B();
            //command.C();
            //command.Execute();

            CommandTest4.ICommand command = new CommandTest4.CommandCompsite();
            command.Add(new CommandTest4.CommandA());
            command.Add(new CommandTest4.CommandB());

            CommandTest4.ICommand subCommand = new CommandTest4.CommandCompsite();
            subCommand.Add(new CommandTest4.CommandB());

            command.Add(subCommand);
            command.Execute();

            Console.WriteLine("Hello World!");
        }
    }

    public class CommandTest5
    {
        public delegate void VoidHandler();

        public class CommandQueue : Queue<VoidHandler>
        {

        }
        public class Invoker
        {
            private CommandQueue queue;
            public Invoker(CommandQueue queue)
            {
                this.queue = queue;
            }

            public void Run()
            {
                while(queue.Count > 0)
                {
                    VoidHandler handler = queue.Dequeue();
                    handler();
                }
            }
        }
    }
    public class CommandTest4
    {
        public interface ICommand
        {
            void Execute();
            void Add(ICommand command);
        }

        public class CommandCompsite : ICommand
        {
            protected IList<ICommand> commands = new List<ICommand>();
            public void Add(ICommand command)
            {
                commands.Add(command);
            }

            public virtual void Execute()
            {
                foreach (var command in commands)
                {
                    command.Execute();
                }
            }
        }
        public abstract class CommandLeaf : ICommand
        {
            public void Add(ICommand command)
            {
                throw new NotSupportedException("Leaf command onject");
            }

            public abstract void Execute();
        }

        public class TestCommand
        {
           public static IList<string> log = new List<string>();
           
        }
        public class CommandA : CommandLeaf
        {
            public override void Execute()
            {
                TestCommand.log.Add("A");
            }
        }
        public class CommandB : CommandLeaf
        {
            public override void Execute()
            {
                TestCommand.log.Add("B");
            }
        }
    }
    public class CommandTest3
    {
        public interface ICommand
        {
            void Execute();
        }

        public interface IFederateCommand: ICommand
        {
            void A();
            void B();
            void C();
        }

        public class FederateCommand : IFederateCommand
        {
            protected ICommand commandA;
            protected ICommand commandB;
            protected ICommand commandC;
            public FederateCommand(ICommand commandA, ICommand commandB, ICommand commandC)
            {
                this.commandA = commandA;
                this.commandB = commandB;
                this.commandC = commandC;
            }
            public virtual void A()
            {
                commandA.Execute();
            }

            public virtual void B()
            {
                commandB.Execute();
            }

            public virtual void C()
            {
                commandC.Execute();
            }

            public virtual void Execute()
            {
                commandA.Execute();
                commandB.Execute();
                commandC.Execute();
            }
        }

        public class CommandA : ICommand
        {
            public void Execute()
            {
                Console.WriteLine("A");
            }
        }
        public class CommandB : ICommand
        {
            public void Execute()
            {
                Console.WriteLine("B");
            }
        }
        public class CommandC : ICommand
        {
            public void Execute()
            {
                Console.WriteLine("C");
            }
        }
    }
    public class CommandTest2
    {
        public interface ICommand
        {
            void Execute();
        }
        public interface IAsyncCommand : ICommand
        {
            event EventHandler Completed;
            event AsyncCallback AsyncCompleted;
            void AsyncExecute();
        }

        public abstract class CommandBase : IAsyncCommand
        {
            public event EventHandler Completed;
            public event AsyncCallback AsyncCompleted;
            private bool isAsync = false;
            public virtual void AsyncExecute()
            {
                if(AsyncCompleted != null && Completed != null)
                {
                    isAsync = true;

                    Completed.BeginInvoke(this, EventArgs.Empty,AsyncCompleted,null); //netcore平台已经不支持BeginInvoke这种异步模式
                }
                Execute();
                isAsync = false;
            }

            public virtual void Execute()
            {
                if(Completed!=null&& !isAsync)
                {
                    Completed(this, EventArgs.Empty);
                }
            }
        }

        public class DemoCommand: CommandBase
        {
            public DemoCommand()
            {
                Completed += this.OnCompleted;
                AsyncCompleted += new AsyncCallback(this.OnAsyncCompleted);
            }

            private void OnAsyncCompleted(IAsyncResult ar)
            {
                log.Add("OnAsyncCompleted");
            }

            private void OnCompleted(object sender, EventArgs e)
            {
                log.Add("OnCompleted");
            }
            public List<string> log = new List<string>();
        }


    }
    public class CommandTest1
    {
        public delegate void VoidHandler();
        public class Receiver1 { public void A() { } }
        public class Receiver2 { public void B() { } }
        public class Receiver3 { public static void C() { } }

        public class Invoker1
        {
            IList<VoidHandler> handlers = new List<VoidHandler>();

            public void AddHandler(VoidHandler handler)
            {
                handlers.Add(handler);
            }

            public void Run()
            {
                foreach (var hander in handlers)
                {
                    hander();
                }
            }
        }

        public class Receiver
        {
            private string name = string.Empty;
            public string Name { get { return name; } }
            private string address = string.Empty;
            public string Address { get { return address; } }

            public void SetName() { name = "name"; }
            public void SetAddress() { address = "address"; }
        }

        public interface ICommand
        {
            void Execute();
            Receiver Receiver { set; }
        }

        public abstract class CommandBase : ICommand
        {
            protected Receiver recevier;
            public Receiver Receiver { set => recevier = value; }

            public abstract void Execute();
        }

        public class SetNameCommand : CommandBase
        {
            public override void Execute()
            {
                recevier.SetName();
            }
        }
        public class SetAddressCommand : CommandBase
        {
            public override void Execute()
            {
                recevier.SetAddress();
            }
        }

        public class Invoker
        {
            private IList<ICommand> commands = new List<ICommand>();
            public void AddCommand(ICommand command)
            {
                commands.Add(command);
            }
            public void Run()
            {
                foreach (var item in commands)
                {
                    item.Execute();
                }
            }
        }
        public class KolAsset
        {
            public List<int> AssetID { get; set; }

            [IgnoreDataMember]
            public List<AssetType> AssetType
            {
                get => Array.ConvertAll<int, AssetType>(AssetID.ToArray(), x => (AssetType)x).ToList();
                set => AssetID = Array.ConvertAll<AssetType, int>(value.ToArray(), a => (int)a).ToList();
            }

        }
        public enum AssetType
        {
            Youtube = 1,
            FaceBook = 2
        }
    }  
}