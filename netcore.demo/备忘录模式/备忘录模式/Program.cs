using System;
using System.Collections.Generic;

namespace 备忘录模式
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            Console.Read();
        }
       
        public static void Test() 
        {
            #region 版本一
            //Originator originator = new Originator();
            //Console.WriteLine(originator.Current.Y);
            //Console.WriteLine(originator.Current.X);

            //IMemento<Position> m1 = originator.Memento;
            //originator.IncreaseY();
            //originator.DecreaseX();
            //Console.WriteLine(originator.Current.Y);
            //Console.WriteLine(originator.Current.X);
            //originator.Memento = m1;
            //Console.WriteLine(originator.Current.Y);
            //Console.WriteLine(originator.Current.X); 
            #endregion


            Originator originator = new Originator();
            originator.SaveCheckpoint();
            originator.DecreaseX();
            originator.IncreaseY();
            Console.WriteLine(originator.Current.X);
            Console.WriteLine(originator.Current.Y);
            originator.Undo();
            Console.WriteLine(originator.Current.X);
            Console.WriteLine(originator.Current.Y);
        }
    }
    /// <summary>
    /// 为了便于定义抽象状态类型所定义的接口
    /// </summary>
    public interface IState { }
    /// <summary>
    /// 抽象备忘录接口对象
    /// </summary>
    /// <typeparam name="T">实现了IState接口类型</typeparam>

    public interface IMemento<T> where T : IState
    {
        T State { get; set; }
    }
    /// <summary>
    /// 抽象备忘录接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MementoBase<T> : IMemento<T> where T : IState
    {
        protected T state;
        public virtual T State { get => state; set => state = value; }
    }
    /// <summary>
    /// 抽象的原发器对象接口
    /// </summary>
    /// <typeparam name="T">IState类型</typeparam>
    /// <typeparam name="M">备忘录</typeparam>
    public interface IOriginator<T, M> where T : IState where M : IMemento<T>, new()
    {
        IMemento<T> Memento { get; set; }
    }

    #region OriginatorBase 版本一
    //public abstract class OriginatorBase<T, M> where T : IState where M : IMemento<T>, new()
    //{
    //    /// <summary>
    //    /// 原发器对象的状态
    //    /// </summary>
    //    protected T state;
    //    /// <summary>
    //    /// 把状态保存到备忘录，或者从备忘录恢复之前的状态
    //    /// </summary>
    //    public virtual IMemento<T> Memento
    //    {
    //        get
    //        {
    //            M m = new M();
    //            m.State = this.state;
    //            return m;
    //        }
    //        set
    //        {
    //            if (value == null) throw new ArgumentException();
    //            this.state = value.State;
    //        }
    //    }
    //} 
    #endregion

    public abstract class OriginatorBase<T> where T : IState
    {
        protected T state;
        protected class InternalMemento<T> : IMemento<T> where T:IState
        {
            private T state;
            public T State
            {
                get { return state; }
                set { state = value; }
            }
        }
        protected virtual IMemento<T> CreateMemento()
        {
            IMemento<T> m = new InternalMemento<T>();
            m.State = this.state;
            return m;
        }
        private IMemento<T> m;
        public virtual void SaveCheckpoint() 
        {
            //m = CreateMemento();
            stack.Push(CreateMemento());
        }
        public virtual void Undo()
        {
            //if (m == null) return;
            if (stack.Count == 0) return;
            IMemento<T> m = stack.Pop();
            this.state = m.State;
            state = m.State;
        }
        private Stack<IMemento<T>> stack = new Stack<IMemento<T>>();
    }
    /// <summary>
    /// 具体状态类型
    /// </summary>
    public struct Position : IState
    {
        public int X;
        public int Y;
    }
    /// <summary>
    /// 具体备忘录类型
    /// </summary>
    public class Memento : MementoBase<Position>
    {

    }
    #region 版本一
    ///// <summary>
    ///// 具体原发器类型
    ///// </summary>
    //public class Originator : OriginatorBase<Position, Memento>
    //{
    //    public void Update(int x) { base.state.X = x; }
    //    public void DecreaseX() { base.state.X--; }
    //    public void IncreaseY() { base.state.Y++; }
    //    public Position Current { get { return base.state; } }
    //} 
    #endregion

    /// <summary>
    /// 具体原发器类型
    /// </summary>
    public class Originator : OriginatorBase<Position>
    {
        public void Update(int x) { base.state.X = x; }
        public void DecreaseX() { base.state.X--; }
        public void IncreaseY() { base.state.Y++; }
        public Position Current { get { return base.state; } }
    }
}
