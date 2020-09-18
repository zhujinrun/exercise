using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;

namespace TemplateDesign
{
    class Program
    {
        static void Main(string[] args)
        {
            //ITransform transfrom = new DataBroker();
            //string data = "1X2X";
            //Console.WriteLine(transfrom.Transform(data));

            //ISetter setter = new DataBroker();
            //data = "H:123";
            //Console.WriteLine(setter.Append(data));

            //Counter counter = new Counter();
            //counter.Changed += (o, a) => {
            //    Console.WriteLine(a.Value);

            //};
            //counter.Add();
            TemplateList<int> list = new TemplateList<int>();
            list.Add(2);
            list.Add(3);
            int i = 3;
            foreach (var data in list)
            {
                Console.WriteLine(data);
            }
        }
    }
    #region 模板四
    public class TemplateList<T>
    {
        private class Node
        {
            public T data;
            public Node next;
            public Node(T data)
            {
                this.data = data;
                next = null;
            }
        }

        private Node head = null;

        public void Add(T data)
        {
            Node node = new Node(data);
            node.next = head;
            head = node;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;
            while (current != null)
            {
                yield return current.data;
                current = current.next;
            }
        }
    }
    #endregion
    #region 模板三
    public class CounterEventArgs : EventArgs
    {
        private int value;
        public CounterEventArgs(int value)
        {
            this.value = value;
        }
        public int Value => this.value;
    }
    public class Counter
    {
        private int value = 0;
        public event EventHandler<CounterEventArgs> Changed;

        public void Add()
        {
            value++;
            Changed(this, new CounterEventArgs(value));
        }
    }
    #endregion

    #region 模板一
    public interface IAbstract
    {
        int Quantity { get; }
        double Total { get; }
        double Average { get; }
    }

    public abstract class AbstractBase : IAbstract
    {
        public abstract int Quantity { get; }
        public abstract double Total { get; }
        public virtual double Average { get => Total / Quantity; }
    }

    public class ArrayData : AbstractBase
    {
        protected double[] data = new double[3] { 1.1, 2.2, 3.3 };

        public override int Quantity => data.Length;
        public override double Total => data.Sum();

    }

    public class ListData : AbstractBase
    {
        protected IList<double> data = new List<double>();

        public ListData()
        {
            data.Add(1.1);
            data.Add(2.2);
            data.Add(3.3);
        }

        public override int Quantity => data.Count;

        public override double Total => data.Sum();
    }
    #endregion

    #region 模板二
    public interface ITransform
    {
        string Transform(string data);
        bool Parse(string data);
        string Replace(string data);
    }

    public interface ISetter
    {
        string Append(string data);
        bool CheckHeader(string data);
        bool CheckTailer(string data);
    }

    public abstract class TransformBase : ITransform
    {

        public abstract bool Parse(string data);
        public abstract string Replace(string data);
        public virtual string Transform(string data)
        {
            if (Parse(data))
            {
                data = Replace(data);
            }
            return data;
        }
    }
    public abstract class SetterBase : ISetter
    {
        public virtual string Append(string data)
        {
            if (!CheckHeader(data)) data = $"H:{data}";
            if (CheckTailer(data)) data = $"{data}:T";
            return data;
        }
        public abstract bool CheckHeader(string data);
        public abstract bool CheckTailer(string data);
    }

    public class DataBroker : ITransform, ISetter
    {
        class InternalTransform : TransformBase
        {
            public override bool Parse(string data) { return data.Contains("X"); }
            public override string Replace(string data) { return data.Replace("X", "Y"); }
        }
        class InternalSetter : SetterBase
        {
            public override bool CheckHeader(string data)
            {
                return data.StartsWith("H:");
            }
            public override bool CheckTailer(string data)
            {
                return data.EndsWith(":T");
            }
        }

        private ISetter setter = new InternalSetter();
        private ITransform transform = new InternalTransform();

        public string Transform(string data)
        {
            return transform.Transform(data);
        }

        public bool Parse(string data)
        {
            return transform.Parse(data);
        }

        public string Replace(string data)
        {
            return transform.Replace(data);
        }

        public string Append(string data)
        {
            return setter.Append(data);
        }

        public bool CheckHeader(string data)
        {
            return setter.CheckHeader(data);
        }

        public bool CheckTailer(string data)
        {
            return setter.CheckTailer(data);
        }
    } 
    #endregion
}
