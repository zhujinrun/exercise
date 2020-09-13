using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StateDesign
{
    class Program
    {
        static void Test(string[] args)
        {
            StopLigth stopLigth = new StopLigth();
            Console.WriteLine(stopLigth.ChangeColor());
            Console.WriteLine(stopLigth.ChangeColor());
            Console.WriteLine(stopLigth.ChangeColor());
            Console.WriteLine(stopLigth.ChangeColor());


            Connection connection = new Connection();
            connection.State = new OpenState();
            try
            {
                connection.Open();
                
            }
            catch { }
            
            try
            {
                connection.Close();
            }
            catch { }

            connection.State = new CloseState();
            try
            {
                connection.Query();
            }
            catch { }
            try
            {
                connection.Open();
            }
            catch { }
            Console.Read();
        }
    }
 
    public interface IState
    {
        void Open();
        void Close();
        void Query();
    }

    public abstract class ContextBase
    {
        private IState state;
        public virtual IState State
        {
            get { return state; }
            set { state = value; }
        }

        public virtual void Open()
        {
            state.Open();
        }
        public virtual void Query()
        {
            state.Query();
        }
        public virtual void Close()
        {
            state.Close();
        }
    }

    public class OpenState : IState
    {
        public void Close()
        {
            
        }

        public void Open()
        {
            throw new NotSupportedException();
        }

        public void Query()
        {
           
        }
    }
    public class CloseState : IState
    {
        public void Close()
        {
            throw new NotSupportedException();
        }

        public void Open()
        {
            
        }

        public void Query()
        {
            throw new NotSupportedException();
        }
    }
    public class Connection : ContextBase
    {

    }
    public enum Color { Red,Yellow,Green}
    public class StopLigth
    {
        private Color current = Color.Green;

        public Color ChangeColor()
        {
            if(current== Color.Green)
            {
                current = Color.Yellow;
            }
            else if(current == Color.Yellow)
            {
                current = Color.Red;
            }
            else if(current == Color.Red)
            {
                current = Color.Green;
            }
            return current;
        }
    }
}
namespace StateDesignTwo{

    class Program
    {
        static void Main(string[] args)
        {
            ObjectWithName objA = new ObjectWithName();
            IStateProvider providerA = new RestrictedStateProvider();
            ObjectWithNameAssembler.Assembly(objA,providerA);
            objA.Name = new string('1', 5);
            Console.WriteLine(objA.Name);

            objA.Name = new string('1', 20);
            Console.WriteLine(objA.Name);


            Target obj = new Target();
            obj.x = 1;
            obj.y = 2;
            obj.message = "hello";
            string graph = JsonConvert.SerializeObject(obj);

            try
            {
                Connection connection = new Connection();

                connection.Open().Query().Close().Query().Close();
            }
            catch
            {

            }
            Console.Read();
        }
    }

    public class GenericEventArgs : EventArgs
    {
        private string value;
        public GenericEventArgs(string value)
        {
            this.value = value;
        }
        public string Value
        {
            get => value;
            set => this.value = value;
        }
    }

    public class ObjectWithName
    {
        private string name;
        public virtual string Name
        {
            get => name;
            set
            {
                GenericEventArgs args = new GenericEventArgs(value);
                if(BeforeModifyName != null)
                {
                    BeforeModifyName(this,args);
                }
                name = args.Value;
            }
        }
        public event EventHandler<GenericEventArgs> BeforeModifyName;
    }

    public interface IStateProvider
    {
        void Handle(object sender, GenericEventArgs genericEventArgs);
    }

    public static class ObjectWithNameAssembler
    {
        public static void Assembly(ObjectWithName target,IStateProvider stateProvider)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if(stateProvider == null)
            {
                throw new ArgumentNullException("stateProvider");
            }
            target.BeforeModifyName += stateProvider.Handle;
        }
    }

    public class UnlimitedStateProvider : IStateProvider
    {
        public void Handle(object sender, GenericEventArgs genericEventArgs)
        {
            
        }
    }
    public class RestrictedStateProvider : IStateProvider
    {
        public void Handle(object sender,GenericEventArgs args)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            if(args.Value.Length > 10)
            {
                args.Value = args.Value.Substring(0, 10);
                if(args.Value.IndexOf("X")>= 0)
                {
                    args.Value = args.Value.Replace("X", "Y");
                }
            }
        }
    }
    public interface IState
    {
        void Open(ContextBase context);
        void Close(ContextBase context);
        void Query(ContextBase context);
    }

    public abstract class ContextBase
    {
        private IState state;
        public virtual IState State
        {
            get { return state; }
            set { state = value; }
        }

        public virtual ContextBase Open()
        {
            state.Open(this);
            return this;
        }
        public virtual ContextBase Query()
        {
            state.Query(this);
            return this;
        }
        public virtual ContextBase Close()
        {
            state.Close(this);
            return this;
        }
    }

    public class OpenState : IState
    {
        public void Close(ContextBase context)
        {
            context.State = new CloseState();
        }

        public void Open(ContextBase context)
        {
            throw new NotSupportedException();
        }

        public void Query(ContextBase context)
        {
            
        }
    }

    public class CloseState : IState
    {
        public void Close(ContextBase context)
        {
            throw new NotSupportedException();
        }

        public void Open(ContextBase context)
        {
            context.State = new OpenState();
        }

        public void Query(ContextBase context)
        {
            throw new NotSupportedException();
        }
    }

   public class Connection : ContextBase
    {
        public Connection()
        {
            State = new CloseState();
        }

    }
    [Serializable]
    public class Target : ISerializable
    {
        public int x;
        public int y;
        public string message;
        public Target()
        {

        }
        protected Target(SerializationInfo info, StreamingContext context)
        {
            x = info.GetInt32("x");
            message = info.GetString("message");
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("x", x);
            info.AddValue("message", message);
        }
    }
}