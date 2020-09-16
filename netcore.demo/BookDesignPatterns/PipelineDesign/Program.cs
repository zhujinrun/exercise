using System;
using System.Collections.Generic;

namespace PipelineDesign
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public interface IMessage { }
    public interface IFilter<T> where T : IMessage
    {
        T Handle(T message);

        PipelineBase<T> Pipeline { get; set; }
    }
    public abstract class FilterBase<T> : IFilter<T> where T : IMessage
    {
        private PipelineBase<T> pipeline;

        public virtual PipelineBase<T> Pipeline { get => pipeline; set => pipeline = value; }

        public abstract T Handle(T message);
    }

    public interface IDataSource<T> where T : IMessage
    {
        T Read();
    }

    public interface IDataSink<T> where T : IMessage
    {
        void Write(T message);
    }

    public abstract class PipelineBase<T> where T : IMessage
    {
        protected IList<IFilter<T>> filters = new List<IFilter<T>>();

        protected T message;
        protected IDataSink<T> dataSink;
        protected IDataSource<T> dataSource;

        public virtual void Process()
        {
            if (dataSource == null)
                throw new ArgumentNullException("data source");

            if (dataSink == null)
                throw new ArgumentNullException("data sink");
            if (message == null)
                throw new ArgumentNullException("message");

            foreach (IFilter<T> filter in filters)
            {
                message = filter.Handle(message);
            }
        }

        public virtual void Add(IFilter<T> t)
        {
            if (t == null)
                throw new ArgumentNullException("t");
            t.Pipeline = this;
            filters.Add(t);
        }
        public virtual void Remove(IFilter<T> t)
        {
            if (t == null)
                throw new ArgumentNullException("t");
            if (!filters.Contains(t))
            {
                return;
            }
            else
            {
                t.Pipeline = null;
                filters.Remove(t);
            }
        }
        public virtual T Message
        {
            get { return message; }
            set { message = value; }
        }
        public IDataSource<T> DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        public IDataSink<T> DataSink
        {
            get { return dataSink; }
            set { dataSink = value; }
        }
    }
    public class Message : IMessage
    {
        public string data;
    }
    public class AppendAFiler : FilterBase<Message>
    {
        public override Message Handle(Message message)
        {
            message.data += "A";

            return message;
        }
    }
    public class AppendBFiler : FilterBase<Message>
    {
        public override Message Handle(Message message)
        {
            message.data += "B";

            return message;
        }
    }

    public class DataSource : IDataSource<Message>
    {
        public virtual Message Read()
        {
            Message message = new Message();
            message.data = Environment.MachineName;
            return message;
        }

    }

    public class DataSink : IDataSink<Message>
    {
        public string Content;
        
        public virtual Message Read()
        {
            Message message = new Message();
            message.data = Environment.MachineName;
            return message;
        }

        public void Write(Message message)
        {
            Content = message.data;
        }
    }
}
