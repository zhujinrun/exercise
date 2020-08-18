using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace 职责链模式
{
    public class Test
    {
        static void Main(string[] args)
        {
            Director larry = new Director();
            VicePredident sam = new VicePredident();
            President tammy = new President();
            BigBigBong bbb = new BigBigBong();
            
            larry.SetSuccessor(sam);
            sam.SetSuccessor(tammy);
            tammy.SetSuccessor(bbb);

            Purchase p = new Purchase(2034, 350.00, "Supplies");
            larry.ProcessRequest(p);

            p = new Purchase(2035, 32590.10, "Project X");
            larry.ProcessRequest(p);

            p = new Purchase(2036, 2400, "Project Y");
            larry.ProcessRequest(p);


        }
    }

    public class Purchase
    {
        private int _number;
        private double _amount;
        private string _purpose;

        public Purchase(int number,double amount,string purpose)
        {
            Number = number;
            Amount = amount;
            Purpose = purpose;
        }

        public int Number { get => _number; set => _number = value; }
        public double Amount { get => _amount; set => _amount = value; }
        public string Purpose { get => _purpose; set => _purpose = value; }
    }
    public abstract class Approver
    {
        protected Approver _successor;
        public void SetSuccessor(Approver successor)
        {
            _successor = successor;
        }

        public abstract void ProcessRequest(Purchase purchase);
    }

    public class Director : Approver
    {
        public override void ProcessRequest(Purchase purchase)
        {
            if(purchase.Amount < 1000.0)
            {
                Console.WriteLine("{0} approved request# {1}", this.GetType().Name, purchase.Number);
            }
            else if (_successor != null)
            {
                _successor.ProcessRequest(purchase);
            }
        }
    }

    public class VicePredident : Approver
    {
        public override void ProcessRequest(Purchase purchase)
        {
            if (purchase.Amount < 2500.0)
            {
                Console.WriteLine("{0} approved request# {1}", this.GetType().Name, purchase.Number);
            }
            else if (_successor != null)
            {
                _successor.ProcessRequest(purchase);
            }
        }
    }

    public class President : Approver
    {
        public override void ProcessRequest(Purchase purchase)
        {
            if (purchase.Amount < 10000.0)
            {
                Console.WriteLine("{0} approved request# {1}", this.GetType().Name, purchase.Number);
            }
            else if (_successor != null)
            {
                _successor.ProcessRequest(purchase);
            }
        }
    }

    public class BigBigBong : Approver
    {
        public override void ProcessRequest(Purchase purchase)
        {
            if (purchase.Amount >= 10000.0)
            {
                Console.WriteLine("{0} approved request# {1}", this.GetType().Name, purchase.Number);
            }
            else if (_successor != null)
            {
                _successor.ProcessRequest(purchase);
            }
        }
    }
}
