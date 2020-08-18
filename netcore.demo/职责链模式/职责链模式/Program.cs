using System;

namespace 职责链模式
{
    class Program
    {
        static void Main_Bak(string[] args)
        {
            ConcreteHandler1 concreteHandler1 = new ConcreteHandler1();
            ConcreteHandler2 concreteHandler2 = new ConcreteHandler2();
            ConcreteHandler3 concreteHandler3 = new ConcreteHandler3();
            HandlerContext context = new HandlerContext
            {
                Id = 001,
                Request = 28
            };
            
            concreteHandler1.SetSuccessor(concreteHandler2);
            concreteHandler1.SetNext(context);
            concreteHandler2.SetSuccessor(concreteHandler3);
            concreteHandler2.SetNext(context);
            Console.WriteLine("Hello World!");
        }
    }

    public class HandlerContext
    {
        public int Id { get; set; }
        public int Request { get; set; }
    }
    public abstract class Handler
    {
        protected Handler _successor;
        public void SetSuccessor(Handler successor)
        {
            this._successor = successor;
        }

        public abstract void HandleRequest(HandlerContext context);



        public void SetNext(HandlerContext context)
        {
            if (_successor != null)
            {
                this._successor.HandleRequest(context);
            }
        }
    }

    public class ConcreteHandler1 : Handler
    {
        public override void HandleRequest(HandlerContext context)
        {
            if(context.Request >= 0 && context.Request < 10)
            {
                Console.WriteLine($" {this.GetType().Name} handled request { context.Request}");

            }
            else
            {
                base.SetNext(context);
            }
        }
    }
    public class ConcreteHandler2 : Handler
    {
        public override void HandleRequest(HandlerContext context)
        {
            if (context.Request >= 10 && context.Request < 20)
            {
                Console.WriteLine($" {this.GetType().Name} handled request { context.Request}");

            }
            else
            {
                base.SetNext(context);
            }
        }
    }
    public class ConcreteHandler3 : Handler
    {
        public override void HandleRequest(HandlerContext context)
        {
            if (context.Request >= 20 && context.Request < 30)
            {
                Console.WriteLine($" {this.GetType().Name} handled request { context.Request}");

            }
            else
            {
                base.SetNext(context);
            }
        }
    }
}
