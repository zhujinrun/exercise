using System;
using System.Drawing;
using System.Runtime.Versioning;

namespace DecoratorDesign
{

    public interface ITarget { void Request(); }
    public interface IAdaptee { void SpeciifiedRequest(); }
    public abstract class GenericAdapteeBase<T> : ITarget where T : IAdaptee,new()
    {
        public virtual void Request()
        {
            new T().SpeciifiedRequest();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            IText text = new TextObject();
            //text = new BoldDecorator(text);
            //text = new ColorDecorator(text);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text.Content);
            Console.Read();
        }
    }
    public class BoldDecorator : DecoratorBase
    {
        public BoldDecorator(IText target) : base(target)
        {
            base.state = new BoldState();
        }
        public override string Content
        {
            get
            {
                if (((BoldState)State).IsBold)
                    return $"<b>{target.Content}</b>";
                else
                    return target.Content;
            }
        }
    }
    public class ColorDecorator : DecoratorBase
    {
        public ColorDecorator(IText target) : base(target)
        {
            base.state = new ColorState();
        }
        public override string Content
        {
            get
            {
                string colorName = (State as ColorState).Color.Name;
                return $"<{colorName}>{target.Content}</{colorName}>";

            }
        }
    }
    public class BoldState : IState
    {
        public bool IsBold;
        public bool Equals(IState newState)
        {
            if (newState == null) return false;
            return ((BoldState)newState).IsBold == IsBold;
        }
    }
    public class ColorState : IState
    {
        public Color Color = Color.Black;
        public bool Equals(IState newState)
        {
            if (newState == null) return false;
            return ((ColorState)newState).Color == Color;
        }
    }
    public interface IState
    {
        bool Equals(IState newState);
    }
    public class TextObject : IText
    {
        public string Content => "hello";
    }

    public class BlockAllDecorator : DecoratorBase
    {
        public BlockAllDecorator(IText target) : base(target) { }
        public override string Content => string.Empty;

    }


    public interface IText
    {
        string Content { get;  }
    }

    public interface IDecorator : IText
    {
        IState State { get; set; }
        void Refresh<T>(IState newState) where T : IDecorator;
    }
    public abstract class DecoratorBase : IDecorator
    {
        protected IText target;
        public DecoratorBase(IText target)
        {
            this.target = target;
        }
        public abstract string Content { get; }
        protected IState state;
        public virtual IState State { get { return this.state; } set { this.state = value; } }

        public virtual void Refresh<T>(IState newState)where T : IDecorator
        {
            if(this.GetType() == typeof(T))
            {
                if (newState == null) State = null;
                if(State!=null && !State.Equals(newState))
                {
                    State = newState;
                }
                return;
            }
            if (target != null)
            {
                ((IDecorator)target).Refresh<T>(newState);
            }
        }
    }
}

