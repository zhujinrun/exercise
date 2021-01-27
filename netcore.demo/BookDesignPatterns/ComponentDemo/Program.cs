using System;
using System.Collections.Generic;

namespace ComponentDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ComponentFactory factory = new ComponentFactory();
            Component corporate = factory.Create<Composite>("corporate");
            factory.Create<Leaf>(corporate, "president");
            factory.Create<Leaf>(corporate, "vice president");

            Component sales = factory.Create<Composite>(corporate, "sales");
            Component market = factory.Create<Composite>(corporate, "market");
            factory.Create<Leaf>(sales, "joe");
            factory.Create<Leaf>(sales, "bob");
            factory.Create<Leaf>(sales, "judi");
            Component branch = factory.Create<Composite>(corporate, "branch");
            factory.Create<Leaf>(branch, "manager");
            factory.Create<Leaf>(branch, "peter");
            IList<string> names = new List<string>(corporate.GetNameList());
            foreach (var item in names)
            {
                Console.WriteLine(item);
            }
            Console.Read();
        }
    }

    public abstract class Component
    {
        protected IList<Component> children;

        protected string name;
        public virtual string Name { get { return name; } set { name = value; } }
        public virtual void Add(Component child)
        {
            children.Add(child);
        }
        public virtual void Remove(Component child)
        {
            children.Remove(child);
        }

        public virtual Component this[int index] { get { return children[index]; } }

        public virtual IEnumerable<string> GetNameList()
        {
            yield return name;
            if((children!=null) && (children.Count > 0))
                foreach (Component child in children)
                {
                    foreach (string item in child.GetNameList())
                    {
                        yield return item;
                    }
                }
        }
    }

    public class Leaf : Component
    {
        public override void Add(Component child)
        {
            throw new NotSupportedException();
        }
        public override void Remove(Component child)
        {
            throw new NotSupportedException();
        }

        public override Component this[int index] { get { throw new NotSupportedException(); } }
    }

    public class Composite : Component
    {
        public Composite()
        {
            base.children = new List<Component>();
        }
    }


    public class ComponentFactory
    {
        public Component Create<T>(string name) where T : Component, new()
        {
            T instance = new T();
            instance.Name = name;
            return instance;
        }

        public Component Create<T>(Component parent,string name) where T:Component,new()
        {
            if (parent == null) throw new ArgumentNullException("parent");

            if (!(parent is Composite)) throw new Exception("non-somposite type");

            Component instance = Create<T>(name);
            parent.Add(instance);
            return instance;
        }
    }
}
