using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Net.Http.Headers;

namespace 模板方法
{
    class Program
    {
        static void Main(string[] args)
        {
            TestThird();
            Console.Read();
        }

        public static void TestThird()
        {
            TemplateList<int> list = new TemplateList<int>();
            list.Add(2);
            list.Add(3);
            int i = 3;
            foreach (var data in list)
            {
                Console.WriteLine(data);
            }
        }
        public static void Test()
        {
            IAbstract i1 = new ArrayData();
            Console.WriteLine(Math.Abs(i1.Average - 2.2) <= 0.001);
            IAbstract i2 = new ListData();
            Console.WriteLine(Math.Abs(i1.Average - i2.Average) <= 0.001);
        }
        public static void TestNext()
        {
            ITransform transform = new DataBroker();
            string data = "1X2X";
            Console.WriteLine(transform.Transform(data));
            ISetter setter = new DataBroker();
            data = "H:123";
            Console.WriteLine(setter.Append(data));
        }
        public static void TestSecond()
        {
            Counter counter = new Counter();
            counter.Changed += (object sender, CounterEventArgs args) =>
              {
                  Console.WriteLine(args.Value);
              };
            counter.Add();
        }
    }

    #region 模板方法一
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
        public virtual double Average { get { return Total / Quantity; } }
    }

    public class ArrayData : AbstractBase
    {
        protected double[] data = new double[3] { 1.1, 2.2, 3.3 };

        public override int Quantity
        {
            get
            {
                return data.Length;
            }
        }

        public override double Total
        {
            get
            {
                double count = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    count += data[i];
                }
                return count;
            }
        }
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
        public override int Quantity
        {
            get
            {
                return data.Count;
            }
        }

        public override double Total
        {
            get
            {
                double count = 0;
                foreach (var item in data)
                {
                    count += item;
                }
                return count;
            }
        }
    }
    #endregion

    #region 模板方法二
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


        public string Transform(string data)
        {
            if (Parse(data))
                data = Replace(data);
            return data;
        }
    }
    public abstract class SetterBase : ISetter
    {
        public virtual string Append(string data)
        {
            if (!CheckHeader(data)) data = "H:" + data;
            if (!CheckTailer(data)) data = data + ":T";
            return data;
        }

        public abstract bool CheckHeader(string data);


        public abstract bool CheckTailer(string data);

    }

    public class DataBroker : ITransform, ISetter
    {
        class InternalTransform : TransformBase
        {
            public override bool Parse(string data)
            {
                return data.Contains("X");
            }

            public override string Replace(string data)
            {
                return data.Replace("X", "Y");
            }
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
        #region ISetter Members
        private ISetter setter = new InternalSetter();
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
        #endregion

        #region ITransform Members
        private ITransform transform = new InternalTransform();
        public bool Parse(string data)
        {
            return transform.Parse(data);
        }

        public string Replace(string data)
        {
            return transform.Replace(data);
        }

        public string Transform(string data)
        {
            return transform.Transform(data);
        }
        #endregion
    }
    #endregion

    #region 模板方法三
    public class CounterEventArgs : EventArgs
    {
        private int value;
        public CounterEventArgs(int value) { this.value = value; }
        public int Value { get { return this.value; } }
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

    #region 模板方法四
    public class TemplateList<T>
    {
        private class Node
        {
            public T Data;
            public Node Next;
            public Node(T data)
            {
                this.Data = data;
                Next = null;
            }
        }
        private Node head = null;
        public void Add(T data)
        {
            Node node = new Node(data);
            node.Next = head;
            head = node;
        }
        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }
    #endregion
}
