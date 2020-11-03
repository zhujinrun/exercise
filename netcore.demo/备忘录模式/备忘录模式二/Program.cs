using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace 备忘录模式二
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
    public interface IState { }
    public interface IPersistenceStore<T> where T:IState
    {
        void Store(string originatorID, int version, T target);
        T Find(string originatorID, int version);
    }

    public abstract class OriginatorBase<T> where T : IState
    {
        protected T state;
        protected string key;
        public OriginatorBase() { key = new Guid().ToString(); }
        protected IPersistenceStore<T> store;
        public virtual void SaveCheckpoint(int version)
        {
            store.Store(key, version, state);
        }

        public virtual void Undo(int version)
        {
            state = store.Find(key, version);
        }
    }

    public class MementoPersistenceStore<T> : IPersistenceStore<T> where T : IState
    {
        private static IDictionary<KeyValuePair<string, int>, string> store = new Dictionary<KeyValuePair<string, int>, string>();

        public T Find(string originatorID, int version)
        {
            KeyValuePair<string, int> key = new KeyValuePair<string, int>(originatorID, version);
            string value;
            if (!store.TryGetValue(key, out value)) throw new NullReferenceException();
            return JsonConvert.DeserializeObject<T>(value);
        }

        public void Store(string originatorID, int version, T target)
        {
            if (target == null) throw new ArgumentNullException("target");
            KeyValuePair<string, int> key = new KeyValuePair<string, int>(originatorID, version);
            string value = JsonConvert.SerializeObject(target);
            if (store.ContainsKey(key))
            {
                store[key] = value;
            }
            else
            {
                store.Add(key, value);
            }
        }
    }

    [Serializable]
    public struct Position : IState
    {
        public int X;
        public int Y;
    }
    public class Originator : OriginatorBase<Position>
    {
        public Originator()
        {
            store = new MementoPersistenceStore<Position>();
        }
    }
}
