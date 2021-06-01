using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptAttribute.Attributing
{
    public interface IAttributeBuilder
    {
        IList<string> Log { get;  }
        void BuildPartA();
        void BuildPartB();
        void BuildPartC();

    }
    [Director(3, "BuildPartA")]
    [Director(2, "BuildPartB")]
    [Director(1, "BuildPartC")]
    public class AttributeBuilder : IAttributeBuilder
    {
        private IList<string> log = new List<string>();
        public IList<string> Log => log;
        public void BuildPartA() => log.Add("a");
        public void BuildPartB() => log.Add("b");
        public void BuildPartC() => log.Add("c");     
    }

    [AttributeUsage(AttributeTargets.Class,AllowMultiple =true)]
    public sealed class DirectorAttribute : Attribute, IComparable<DirectorAttribute>
    {
        private int priority;
        private string method;
        public DirectorAttribute(int priority,string method)
        {
            this.priority = priority;
            this.method = method;
        }
        public int Priority { get => priority; set => priority = value; }
        public string Method { get => method; set => method = value; }

        public int CompareTo(DirectorAttribute other)
        {
            return other.priority - this.priority;
        }
    }

    public class Director
    {
        public void BuildUp(IAttributeBuilder builder)
        {
            var type = builder.GetType();
            if (type.IsDefined(typeof(DirectorAttribute), false))
            {
                DirectorAttribute[] attributes = (DirectorAttribute[])type.GetCustomAttributes(typeof(DirectorAttribute), false);
                if (attributes.Length < 1) return;
                Array.Sort<DirectorAttribute>(attributes);
                foreach (var attribute in attributes)
                {
                    InvokeBuildPartMethod(builder, attribute);
                }
            }
        }

        private void InvokeBuildPartMethod(IAttributeBuilder builder,DirectorAttribute attribute)
        {
            switch (attribute.Method)
            {
                case "BuildPartA":builder.BuildPartA();break;
                case "BuildPartB": builder.BuildPartB(); break;
                case "BuildPartC": builder.BuildPartC(); break;
            }
        }
    }
}
