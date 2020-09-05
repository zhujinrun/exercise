using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReadBookDemo
{
    class Program
    {
        static void Main(string[] args)
        {        
            #region 测试二

            ITimerProvider timerProvider1 = new Assembler().Create<ITimerProvider>();
            // Client client = new Client(timerProvider1);
            Client client = new Client();
            IObjectWithTimeProvider objectWithTimeProvider = new ImClient();
            objectWithTimeProvider.TimerProvider = timerProvider1;
            AttClient attClient = new AttClient();
            var getattClientValue = client.TimerProvider;

            client.TimerProvider = timerProvider1;

            Adaptee adaptee = new Adaptee();
            Target target = adaptee;
            Console.WriteLine(adaptee.Code + "   " + target.Data);

            List<Target> targets = new List<Target>();
            targets.Add(adaptee);
            targets.Add(adaptee);
            Console.WriteLine(adaptee.Code + "   " + targets[1].Data);


            ErrorEntity entity = new ErrorEntity();
            entity += "Hello";
            entity += 1;
            entity += 2;
            entity += " world";

            foreach (var item in entity.Codes)
            {
                Console.WriteLine(item);
            }
            foreach (var item in entity.Messages)
            {
                Console.WriteLine(item);
            }

            RawIterator rawIterator = new RawIterator();
            var rawIteratorv1 = rawIterator.GetEnumerator();
            var rawIteratorv2 = rawIterator.GetRange(1, 2);
            var rawIteratorv3 = rawIterator.Greeting;
            Console.WriteLine("*********************************");
            while (rawIteratorv1.MoveNext())
            {
                Console.WriteLine(rawIteratorv1.Current);
            }
            Console.WriteLine("*********************************");
            var v2 = rawIteratorv2.GetEnumerator();
            while (v2.MoveNext())
            {
                Console.WriteLine(v2.Current);
            }
            Console.WriteLine("*********************************");
            var v3 = rawIteratorv3.GetEnumerator();

            while (v3.MoveNext())
            {
                Console.WriteLine(v3.Current);
            }
            #endregion
            #region 测试

            Dashboard dashboard = new Dashboard();
            Predicate<float> predicate = a => a > 55;
            float resultDas = dashboard[predicate];
            Staff staff = new Staff();
            Employee employee = staff["John", "Doe"];
            string exptected = "Vice President";
            Console.WriteLine(employee.Title == exptected);

            Console.WriteLine(new MultiColumnCollection().Data.Tables[0].Rows[0][0]);
            Console.WriteLine(new MultiColumnCollection().Data.Tables[0].Rows[0][1]);
            IAttributeBuilder builder = new AttributeBuilder();
            Director director = new Director();
            director.BuildUp(builder);
            var resultBuilder = builder.Log[0] == "A";
            string typeName = typeof(Test).AssemblyQualifiedName;
            var result = new RawGenericFactory<ITest>().Create(typeName);

            AsyncInvoker AsyncInvoker = new AsyncInvoker();
            Thread.Sleep(3000);
            Console.WriteLine("method" == AsyncInvoker.OutPut[0]);
            #endregion
        }
    }
}
#region 示例代码

#region 依赖注入

[AttributeUsage(AttributeTargets.Class,AllowMultiple =true)]
public sealed class DecoratorAttribute : Attribute
{
    public readonly object Injector;
    private Type type;

    public DecoratorAttribute(Type type) 
    {
        if (type == null) throw new ArgumentNullException("type");
        this.type = type;
        Injector = (new Assembler().Create(this.type));
    }
    public Type Type { get { return type; } }

}

public static class AttributeHelper
{
    public static T Injector<T>(object target) where T:class
    {
        if (target == null) throw new ArgumentNullException("target");

        Type targetType = target.GetType();
        object[] attributes = targetType.GetCustomAttributes(typeof(DecoratorAttribute),false);
        if ((attributes == null) || (attributes.Length <= 0)) return null;
        foreach (var attribute in (DecoratorAttribute[])attributes)
        {
            if(attribute.Type == typeof(T))
            {
                return (T)attribute.Injector;
            }
        }
        return default(T);
    }
}

[Decorator(typeof(ITimerProvider))]
public class AttClient
{
    public int GetYear()
    {
        ITimerProvider timerProvider = AttributeHelper.Injector<ITimerProvider>(this);
        return timerProvider.CurrentDate.Year;
    }
}
public class Assembler
{
    private static Dictionary<Type, Type> dictionary = new Dictionary<Type, Type>();

    static Assembler()
    {
        dictionary.Add(typeof(ITimerProvider), typeof(TimerProvider));
    }
    public object Create(Type type)
    {
        if (type == null || !dictionary.ContainsKey(type))
        {
            throw new NullReferenceException();
        }
        Type targtType = dictionary[type];
        return Activator.CreateInstance(targtType);
    }
    public T Create<T>()
    {
        return (T)Create(typeof(T));
    }
}
public interface ITimerProvider
{
    DateTime CurrentDate { get; }
}
public class TimerProvider : ITimerProvider
{
    public DateTime CurrentDate => DateTime.Now;
}
public class Client
{
    public int GetYear()
    {
        ITimerProvider timeProvider = new TimerProvider();
        return timeProvider.CurrentDate.Year;
    }
    private ITimerProvider timerProvider;
    //public Client(ITimerProvider timerProvider)
    //{
    //    this.timerProvider = timerProvider;
    //}  //测试属性注入 屏蔽
    public ITimerProvider TimerProvider
    {
        get
        {
            return this.timerProvider;
        }
        set
        {
            this.timerProvider = value;
        }
    }

} 
public interface IObjectWithTimeProvider
{
    ITimerProvider TimerProvider { get; set; }
}

public class ImClient : IObjectWithTimeProvider
{
    private ITimerProvider timerProvider;
    public ITimerProvider TimerProvider { get { return this.timerProvider; } set { this.timerProvider = value; } }


}
#endregion
public class Adaptee
{
    public int Code { get { return new Random().Next(); } }

}
public class Target
{
    private int data;
    public Target(int data)
    {
        this.data = data;
    }

    public int Data { get { return data; } }
    public static implicit operator Target(Adaptee adaptee)
    {
        return new Target(adaptee.Code);
    }
}
public class ErrorEntity
{
    private IList<string> messages = new List<string>();
    private IList<int> codes = new List<int>();

    public IList<string> Messages { get => messages; set => messages = value; }
    public IList<int> Codes { get => codes; set => codes = value; }

    public static ErrorEntity operator +(ErrorEntity entity, string message)
    {
        entity.Messages.Add(message);
        return entity;
    }

    public static ErrorEntity operator +(ErrorEntity entity, int code)
    {
        entity.Codes.Add(code);
        return entity;
    }

}
public class ObjectWithName
{
    private string name;
    public ObjectWithName(String name)
    {
        this.name = name;
    }
    public override string ToString()
    {
        return name;
    }
}

public class BinaryTreeNode : ObjectWithName
{
    private string name;
    public BinaryTreeNode(string name) : base(name)
    {

    }
    public BinaryTreeNode Left = null;
    public BinaryTreeNode Right = null;
    public IEnumerator GetEnumerator()
    {
        yield return this;
        if (Left != null)
        {
            foreach (var item in Left)
            {
                yield return item;
            }
        }
        if (Right != null)
        {
            foreach (var item in Right)
            {
                yield return item;
            }
        }
    }

}

public class CompositeIterator
{
    private IDictionary<object, IEnumerator> items = new Dictionary<object, IEnumerator>();

    public void Add(object data)
    {
        items.Add(data, GetEnumerator(data));
    }

    private IEnumerator GetEnumerator()
    {
        if (items != null && items.Count > 0)
        {
            foreach (var item in items.Values)
            {
                while (item.MoveNext())
                    yield return item.Current;
            }
        }
    }

    public static IEnumerator GetEnumerator(object data)
    {
        if (data == null) throw new NullReferenceException();
        Type type = data.GetType();

        if (type.IsAssignableFrom(typeof(Stack)) || type.IsAssignableFrom(typeof(Stack<ObjectWithName>)))
        {
            return DynamicInvokeEnumerator(data);
        }
        if (type.IsAssignableFrom(typeof(Queue)) || type.IsAssignableFrom(typeof(Queue<ObjectWithName>)))
        {
            return ((ObjectWithName[])data).GetEnumerator();
        }
        if (type.IsAssignableFrom(typeof(BinaryTreeNode)))
        {
            return ((BinaryTreeNode)data).GetEnumerator();
        }
        throw new NotSupportedException();
    }

    private static IEnumerator DynamicInvokeEnumerator(object data)
    {
        if (data == null) throw new NullReferenceException();
        Type type = data.GetType();
        return (IEnumerator)type.InvokeMember("GetEnumerator", System.Reflection.BindingFlags.InvokeMethod, null, data, null);
    }
}
public class RawIterator
{
    private int[] data = new int[] { 0, 1, 2 };
    public IEnumerator GetEnumerator()
    {
        foreach (var item in data)
        {
            yield return item;
        }
    }
    public IEnumerable GetRange(int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            yield return data[i];
        }
    }
    public IEnumerable<string> Greeting
    {
        get
        {
            yield return "Hello";
            yield return " ";
            yield return " World!";
        }
    }
}
public class Dashboard
{
    float[] temps = new float[10] { 56.1F, 56.5F, 56.2F, 56.7F, 56.2F, 52.2F, 55.2F, 53.9F, 52.0F, 56.6F };

    public float this[Predicate<float> predicate]
    {
        get
        {
            float[] matches = Array.FindAll<float>(temps, predicate);
            return matches[0];
        }
    }
}
public struct Employee
{
    public string FirstName;
    public string FamilyName;
    public string Title;
    public Employee(DataRow row)
    {
        this.FirstName = row["FirstName"] as string;
        this.FamilyName = row["FamilyName"] as string;
        this.Title = row["Title"] as string;
    }
}
public class Staff
{
    static DataTable data = new DataTable();
    static Staff()
    {
        data.Columns.Add("FirstName");
        data.Columns.Add("FamilyName");
        data.Columns.Add("Title");

        data.PrimaryKey = new DataColumn[] { data.Columns[0], data.Columns[1] };
        data.Rows.Add("Jane", "Done", "SalesManager");
        data.Rows.Add("John", "Doe", "Vice President");
        data.Rows.Add("Rupert", "Muck", "President");
        data.Rows.Add("John", "Simth", "Logistics Engineer");
    }

    public Employee this[string firstName, string familyName]
    {
        get
        {
            DataRow row = data.Rows.Find(new object[] { firstName, familyName });
            return new Employee(row);
        }
    }
}
public class MultiColumnCollection
{
    private static DataSet data = new DataSet();

    static MultiColumnCollection()
    {
        data.Tables.Add("tableTest");
        data.Tables[0].Columns.Add("name", typeof(string));
        data.Tables[0].Columns.Add("gender", typeof(int));
        data.Tables[0].Rows.Add(new object[] { "lilei", 4 });

    }
    public DataSet Data { get { return data; } }

}
public class SingleColumnCollection
{
    private static string[] countries = new string[] {
        "china","chile","uk"
        };

    public string this[int index]
    {
        get
        {
            return countries[index];
        }
    }
    public string[] this[string name]
    {
        get
        {
            if (countries == null || countries.Count() <= 0)
            {
                return null;
            }
            return Array.FindAll<string>(countries, o => o.StartsWith(name));
        }
    }
}
public class Director
{
    public void BuildUp(IAttributeBuilder builder)
    {
        object[] attributes = builder.GetType().GetCustomAttributes(typeof(DirectorAttribute), false);
        if (attributes.Length <= 0)
        {
            return;
        }
        DirectorAttribute[] directors = new DirectorAttribute[attributes.Length];

        for (int i = 0; i < attributes.Length; i++)
        {
            directors[i] = (DirectorAttribute)attributes[i];
        }
        Array.Sort<DirectorAttribute>(directors);
        foreach (var item in directors)
        {
            InvokePartMethod(builder, item);
        }
    }

    private void InvokePartMethod(IAttributeBuilder builder, DirectorAttribute item)
    {
        switch (item.Method)
        {
            case "BuildPartA":
                builder.BuildPartA();
                break;
            case "BuildPartB":
                builder.BuildPartB();
                break;
            case "BuildPartC":
                builder.BuildPartC();
                break;

        }
    }
}
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DirectorAttribute : Attribute, IComparable<DirectorAttribute>
{
    private int priority;
    private string method;
    public DirectorAttribute(int priority, string method)
    {
        this.Priority = priority;
        this.Method = method;
    }

    public int Priority { get => priority; set => priority = value; }
    public string Method { get => method; set => method = value; }

    public int CompareTo(DirectorAttribute other)
    {
        return other.priority - this.priority;
    }
}

public interface IAttributeBuilder
{
    IList<string> Log { get; }
    void BuildPartA();
    void BuildPartB();
    void BuildPartC();

}
[Director(1, "BuildPartB")]
[Director(2, "BuildPartA")]
[Director(3, "BuildPartC")]
public class AttributeBuilder : IAttributeBuilder
{
    private IList<string> log = new List<string>();
    public IList<string> Log
    {
        get
        {
            return log;
        }
    }
    public void BuildPartA()
    {
        log.Add("A");
    }

    public void BuildPartB()
    {
        log.Add("B");
    }

    public void BuildPartC()
    {
        log.Add("C");
    }
}
public interface IOrganization { }

public abstract class UserBase<TKey, TOrganization> where TOrganization : class, IOrganization, new()
{
    public abstract TOrganization Organization { get; }
    public abstract void Promotion(TOrganization newOrganization);
    delegate TOrganization OriganizationChangedHandler();
}
public interface ITest
{

}
public class Test : ITest
{

}
public class RawGenericFactory<T>
{
    public T Create(string typeName)
    {
        return (T)(Activator.CreateInstance(Type.GetType(typeName)));
    }
}
public class AsyncInvoker
{
    private IList<string> output = new List<string>();

    public AsyncInvoker()
    {
        Timer slowTimer = new Timer(new TimerCallback(OnTimerInterval), "slow", 2500, 2500);
        Timer fastTimer = new Timer(new TimerCallback(OnTimerInterval), "fast", 2000, 2000);

        output.Add("fast");

    }

    private void OnTimerInterval(object state)
    {
        output.Add(state as string);
    }
    public IList<string> OutPut
    {
        get
        {
            return output;
        }
    }
}

#endregion}
