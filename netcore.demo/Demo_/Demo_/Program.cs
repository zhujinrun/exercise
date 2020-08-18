using Demo_.Abstract;
using System;

namespace Demo_
{
    class Program
    {
        static void Main(string[] args)
        {
            
            AbstractMVC conc = new ConcAbstractMVC();
            conc = new AuthMVC(conc);
            conc = new ViewMVC(conc);
            conc = new ExceptionMVC(conc);
            conc.Action();

            Parent child = new ChildP();
            child.Paly();
            ChildP child2 = new ChildP();
            child2.Paly();
            Parent newChild = new NewChild();
            newChild.Paly();
            NewChild newChild2 = new NewChild();
            newChild2.Paly();
            Parent VirChild = new VirChild();
            VirChild.Paly();
            VirChild VirChild2 = new VirChild();
            VirChild2.Paly();

            Console.Read();
        }
    }

    public class Parent
    {
        public virtual void Paly()
        {
            Console.WriteLine("parent virtual");
        }
    }

    public class VirChild: Parent
    {

    }
    public class ChildP : Parent
    {
        public override void Paly()
        {
            Console.WriteLine("overrid paly");
        }
    }

    public class NewChild : Parent
    {
        public new void Paly()
        {
            Console.WriteLine("new paly");
        }
    }  
}
