using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ObserverDesign
{
    class Program
    {
        static void Main(string[] args)
        {
            A a = new A();
            B b = new B();
            C c = new C();
            X x = new X();
            x.instanceA = a;
            x.instanceB = b;
            x.instanceC = c;
            x.SetData(123);

            Console.WriteLine("Hello World!");
        }
    }

    public class DictionaryEventArgs<TKey, TValue> : EventArgs
    {
        private TKey key;
        private TValue value;

        public DictionaryEventArgs(TKey key,TValue value)
        {
            this.key = key;
            this.value = value;
        }

        public TKey Key { get => key; }
        public TValue Value { get => value; }
    }
    public interface IObserverableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        EventHandler<DictionaryEventArgs<TKey,TValue>> NewItemAdded { get; set; }
    }

    public class ObserverableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IObserverableDictionary<TKey, TValue>
    {
        protected EventHandler<DictionaryEventArgs<TKey, TValue>> newItemAdded;

        public EventHandler<DictionaryEventArgs<TKey, TValue>> NewItemAdded { get => newItemAdded; set => newItemAdded = value; }

        public new void Add(TKey key,TValue value)
        {
            base.Add(key, value);
            if (NewItemAdded != null)
            {
                NewItemAdded(this, new DictionaryEventArgs<TKey, TValue>(key, value));
            }
        }
    }
    public class TestObserver
    {
        string key = "hello";
        string value = "world";

        public void Validate(object sender,DictionaryEventArgs<string,string> args)
        {

        }

        public void Test()
        {
            IObserverableDictionary<string, string> dictionary = new ObserverableDictionary<string, string>();
            dictionary.NewItemAdded += this.Validate;
            dictionary.Add(key, value);

        }
    }
    public interface IObserver<T>
    {
        void Update(SubjectBase<T> subject);
    }

    public abstract class SubjectBase<T>
    {
        protected IList<IObserver<T>> observers = new List<IObserver<T>>();
        protected T state;
        public virtual T State { get => state; }

        public static SubjectBase<T> operator +(SubjectBase<T> subject,IObserver<T> observer)
        {
            subject.observers.Add(observer);
            return subject;
        }

        public static SubjectBase<T> operator -(SubjectBase<T> subject, IObserver<T> observer)
        {
            subject.observers.Remove(observer);
            return subject;
        }

        public virtual void Notify()
        {
            foreach (var observer in observers)
            {
                observer.Update(this);
            }
        }

        public virtual void Update( T state)
        {
            this.state = state;
            Notify();
        }
    }

    public class Observer<T> : IObserver<T>
    {
        public T State;
        public void Update(SubjectBase<T> subject)
        {
            this.State = subject.State;
        }
    }

    public class TestObserver
    {
        public void TestMulticst()
        {
            SubjectBase<int> subject = new SubjctA<int>();
        }
    }

    public class SubjctA<T> : SubjectBase<int>
    {
    }
    public class SubjctB<T> : SubjectBase<int>
    {
    }

    public class A
    {
        public int data;
        public void Update(int data)
        {
            this.data = data;
        }
    }
    public class B
    {
        public int Count;
        public void NotifyCount(int data)
        {
            this.Count = data;
        }
    }

    public class C
    {
        public int N;
        public void Set(int data)
        {
            this.N = data;
        }
    }

    public class X
    {
        private int data;

        public A instanceA;
        public B instanceB;
        public C instanceC;

        public void SetData(int data)
        {
            this.data = data;
            instanceA.Update(data);
            instanceB.NotifyCount(data);
            instanceC.Set(data);
        }

    }
}
