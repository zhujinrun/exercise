using System;

namespace ResponsibilityDesign
{
    class Program
    {
        static void Main(string[] args)
        {
            IHandler handler1 = new InternalHandler();
            IHandler handler2 = new DiscountHandler();
            IHandler handler3 = new MailHandler();
            IHandler handler4 = new RegularHandler();

            handler1.Successor = handler3;
            handler3.Successor = handler2;
            handler2.Successor = handler4;
            IHandler head = handler4;

            Request request = new Request(20, PurchaseType.Mail);
            head.HandleRequest(request);
            Console.WriteLine(request.Price);
            handler1.Successor = handler1.Successor;
            request = new Request(20, PurchaseType.Discount);
            Console.WriteLine(request.Price);
        }
    }

    public enum PurchaseType
    {
        Internal,
        Discount,
        Regular,
        Mail
    }

    public class Request
    {
        private double price;
        private PurchaseType type;
        public Request(double price ,PurchaseType type)
        {
            this.price = price;
            this.type = type;
        }
        public double Price { get { return price; } set { price = value; } }
        public PurchaseType Type { get { return type; } set { type = value; } }
    }

    public interface IHandler
    {
        void HandleRequest(Request request);
        IHandler Successor { get; set; }
        PurchaseType Type { get; set; }
    }

    public abstract class HandlerBase : IHandler
    {
        protected IHandler successor;
        protected PurchaseType type;
        public HandlerBase(PurchaseType type,IHandler successor)
        {
            this.type = type;
            this.successor = successor;
        }
        public HandlerBase(PurchaseType type) : this(type, null) { }
        public IHandler Successor { get => successor; set => successor = value; }
        public PurchaseType Type { get => type; set => type=value; }
        public abstract void Process(Request request);

        public virtual void HandleRequest(Request request)
        {
            if (request == null) return;
            if (request.Type == Type) Process(request);
            else if (Successor != null) successor.HandleRequest(request);
        }
    }

    public class InternalHandler : HandlerBase
    {
        public InternalHandler() : base(PurchaseType.Internal) { }

        public override void Process(Request request)
        {
            request.Price *= 0.6;
        }
    }
    public class MailHandler : HandlerBase
    {
        public MailHandler() : base(PurchaseType.Mail) { }
        public override void Process(Request request)
        {
            request.Price *= 1.3;
        }
    }
    public class DiscountHandler : HandlerBase
    {
        public DiscountHandler() : base(PurchaseType.Discount) { }
        public override void Process(Request request)
        {
            request.Price *= 0.9;
        }
    }
    public class RegularHandler : HandlerBase
    {
        public RegularHandler() : base(PurchaseType.Regular) { }
        public override void Process(Request request)
        {
            
        }
    }
}
