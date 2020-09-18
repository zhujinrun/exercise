using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace Publish_SubscriberDesign
{
    public class Test
    {
        private ArticleStore articleStore;
        private Article articleX;
        private Article articleY;
        private Article articleZ;
        private ArticleSubscriptionStore articleSubscriptionStore;
        private EventStore eventStore;
        private NotificationStore notificationStore;
        private Publisher publisher;
        private Subscriber subscriberA;
        private Subscriber subscriberB;
        private NotificationGenerator generator;
        private Distributor distributor;

        public void AssemblyPubSub()
        {
            publisher = new Publisher();
            publisher.ArticleStore = articleStore;
            publisher.ArticleSubscriptionStore = articleSubscriptionStore;
            publisher.EventStore = eventStore;
            subscriberA = new Subscriber();
            subscriberB = new Subscriber();
            generator = new NotificationGenerator(eventStore, notificationStore, articleSubscriptionStore);
            distributor = new Distributor(notificationStore);
            publisher.Subscribe(articleX, subscriberA);
            publisher.Subscribe(articleY, subscriberA);
            publisher.Subscribe(articleY, subscriberB);
            publisher.Subscribe(articleZ, subscriberB);
        }

        public void PublishEvent()
        {
            publisher.Publish(new Article("X", "1"));
            publisher.Publish(new Article("Y", "3"));
            publisher.Publish(new Article("Z", "4"));
            publisher.Publish(new Article("X", "2"));
        }

        public void InitPersisence()
        {
            articleStore = new ArticleStore();
            articleX = new Article("X", string.Empty);
            articleY = new Article("Y", string.Empty);
            articleZ = new Article("Z", string.Empty);

            articleStore.Save(articleX);
            articleStore.Save(articleY);
            articleStore.Save(articleZ);

            articleSubscriptionStore = new ArticleSubscriptionStore();
            eventStore = new EventStore();
            notificationStore = new NotificationStore();
        }

        public void Invoke()
        {
            InitPersisence();
            AssemblyPubSub();
            PublishEvent();

            Console.WriteLine($"subscriberA.Queue.Count = {subscriberA.Queue.Count}");
            Console.WriteLine($"{subscriberA.Dequeue("X")}");
            Console.WriteLine($"{subscriberA.Dequeue("X")}");
            Console.WriteLine($"{subscriberA.Dequeue("Y")}");
            Console.WriteLine($"subscriberA.Queue.Count = {subscriberA.Queue.Count}");
            Console.WriteLine($"{subscriberA.Dequeue("Y")}");
            Console.WriteLine($"{subscriberA.Dequeue("Z")}");
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            Test test = new Test();
            test.Invoke();
            Console.Read();
        }


    }

    #region 数据实体模型
    /// <summary>
    /// 持久层检索对象的抽象
    /// </summary>
    public interface IObjectWithKey
    {
        string Key { get; }
    }

    /// <summary>
    /// 订阅发布内容对象 各种数据类型都可以 string dataset stream xmldocument 
    /// </summary>
    public class Article : IObjectWithKey
    {
        private string category;
        public string Category
        {
            get => category;
            set => category = value;
        }

        private string content;
        public string Content
        {
            get => content;
            set => content = value;
        }
        public Article(string category, string content)
        {
            this.category = category;
            this.content = content;
        }

        public virtual string Key { get => category; }

    }
    /// <summary>
    /// publisher抛出的订阅事件
    /// </summary>
    public class Event : IObjectWithKey
    {
        public virtual string Key => ID;
        public string id = Guid.NewGuid().ToString();
        public string ID
        {
            get { return id; }
        }

        private Article article;
        public Article Article
        {
            get => article;
        }
        public Event(Article article)
        {
            this.article = article;
        }
    }
    /// <summary>
    /// 订阅情况
    /// </summary>
    public class ArticleSubscription : IObjectWithKey
    {
        private Article article;
        public Article Article { get => article; }
        private ISubscriber subscriber;
        public ISubscriber Subscriber { get => subscriber; }
        public ArticleSubscription(Article article, ISubscriber subscriber)
        {
            this.article = article;
            this.subscriber = subscriber;
        }

        public virtual string Key { get => ((IObjectWithKey)article).Key + subscriber.GetHashCode().ToString(); }
    }
    /// <summary>
    /// 通知
    /// </summary>
    public class Notification : IObjectWithKey
    {
        private Event e;
        public Event Event { get => e; }
        private ISubscriber subscriber;
        public ISubscriber Subscriber { get => subscriber; }

        public Notification(Event e, ISubscriber subscriber)
        {
            this.e = e;
            this.subscriber = subscriber;
        }
        public string Key { get => e.ID + subscriber.GetHashCode().ToString(); }
    }
    #endregion

    #region 业务实体
    /// <summary>
    /// 抽象订阅接口
    /// </summary>
    public interface ISubscriber
    {
        void Enque(Article article);
        string Peek(string category);
        string Dequeue(string category);
    }
    /// <summary>
    /// 抽象订阅对象
    /// </summary>
    public abstract class SubscribeBase : ISubscriber
    {
        protected IDictionary<string, Queue<String>> queue = new Dictionary<string, Queue<string>>();

        public virtual string Dequeue(string category)
        {
            if (!queue.ContainsKey(category)) return null;
            if (queue[category].Count == 0) return null;
            return queue[category].Dequeue();
        }

        public virtual void Enque(Article article)
        {
            if (article == null) throw new ArgumentNullException("article");
            string category = article.Category;
            if (!queue.ContainsKey(category))
            {
                queue.Add(category, new Queue<string>());
            }
            queue[category].Enqueue(article.Content);
        }

        public virtual string Peek(string category)
        {
            if (!queue.ContainsKey(category)) return null;
            if (queue[category].Count == 0) return null;
            return queue[category].Peek();
        }

    }

    /// <summary>
    /// 抽象发布接口
    /// </summary>
    public interface IPublisher
    {
        void Subscribe(Article article, ISubscriber subscriber);   //订阅
        void UnSubscribe(Article article, ISubscriber subscriber);
        IEnumerator<Article> Articles { get; }
        void Publish(Article article);
    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class PublisherBase : IPublisher
    {
        protected IKeyObjectStore<Article> articleStore;
        protected IKeyObjectStore<ArticleSubscription> subscriptionStore;
        protected IKeyObjectStore<Event> eventStore;

        public virtual void Subscribe(Article article, ISubscriber subscriber)
        {
            Extensions.CustomException(article); Extensions.CustomException(subscriber);
            ArticleSubscription subscription = new ArticleSubscription(article, subscriber);
            string key = ((IObjectWithKey)subscription).Key;
            if (subscriptionStore.Select(key) == null)
            {
                subscriptionStore.Save(subscription);
            }
        }

        public virtual void UnSubscribe(Article article, ISubscriber subscriber)
        {
            Extensions.CustomException(article); Extensions.CustomException(subscriber);
            ArticleSubscription subscription = new ArticleSubscription(article, subscriber);

            subscriptionStore.Remove(((IObjectWithKey)subscription).Key);
        }
        public virtual IEnumerator<Article> Articles
        {
            get
            {
                foreach (Article article in articleStore)
                {
                    yield return article;
                }
            }
        }

        public virtual void Publish(Article article)
        {
            Extensions.CustomException(article);
            Event e = new Event(article);
            eventStore.Save(e);
        }
    }


    public interface IKeyObjectStore<T> where T : class, IObjectWithKey
    {
        void Save(T target);
        T Select(string key);
        void Remove(string key);
        IEnumerator GetEnumerator();
    }
    #endregion

    public class Extensions
    {
        public static void CustomException(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
        }
    }

    #region 实体对象
    public class KeyObjectStore<T> : IKeyObjectStore<T> where T : class, IObjectWithKey
    {
        protected IDictionary<string, T> data = new Dictionary<string, T>();

        public IEnumerator GetEnumerator()
        {
            foreach (var item in data.Values)
            {
                yield return item;
            }
        }

        public virtual void Remove(string key)
        {
            data.Remove(key);
        }

        public virtual void Save(T target)
        {
            Extensions.CustomException(target);
            data.Add(target.Key, target);
        }

        public virtual T Select(string key)
        {
            Extensions.CustomException(key);
            T result;
            if (data.TryGetValue(key, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }

    public class ExtEventArgs : EventArgs
    {
        private Event e;
        public Event Event { get => e; }
        public ExtEventArgs(Event e)
        {
            this.e = e;
        }
    }
    public class EventStore : KeyObjectStore<Event>
    {
        public EventHandler<ExtEventArgs> NewEventOccured;
        public override void Save(Event target)
        {
            base.Save(target);
            if (NewEventOccured != null)
            {
                NewEventOccured(this, new ExtEventArgs(target));
            }
        }
    }
    public class NotificationEventArgs : EventArgs
    {
        private Notification notification;
        public Notification Notification { get => notification; }

        public NotificationEventArgs(Notification notification)
        {
            this.notification = notification;
        }
    }

    public class NotificationStore : KeyObjectStore<Notification>
    {
        public event EventHandler<NotificationEventArgs> NewNotificationOccured;

        public override void Save(Notification target)
        {
            base.Save(target);
            if (NewNotificationOccured != null)
                NewNotificationOccured(this, new NotificationEventArgs(target));
        }
    }

    public class ArticleStore : KeyObjectStore<Article>
    {

    }
    public class ArticleSubscriptionStore : KeyObjectStore<ArticleSubscription>
    {

    }

    public class Publisher : PublisherBase
    {
        public ArticleStore ArticleStore { set => articleStore = value; }
        public ArticleSubscriptionStore ArticleSubscriptionStore { set { subscriptionStore = value; } }

        public EventStore EventStore { set { eventStore = value; } }
    }

    public class Subscriber : SubscribeBase
    {
        public IDictionary<string, Queue<string>> Queue
        {
            get { return queue; }

        }
    }

    public class NotificationGenerator
    {
        private EventStore eventStore;
        private NotificationStore notificationSource;
        private ArticleSubscriptionStore articleSubscriptionStore;

        public NotificationGenerator(EventStore eventStore, NotificationStore notificationSource, ArticleSubscriptionStore articleSubscriptionStore)
        {
            this.eventStore = eventStore;
            this.notificationSource = notificationSource;
            this.articleSubscriptionStore = articleSubscriptionStore;

            eventStore.NewEventOccured += OnNewEventOccured;
        }

        public void OnNewEventOccured(object sender, ExtEventArgs args)
        {
            Event e = args.Event;
            string articleKey = e.Article.Key;
            foreach (ArticleSubscription item in articleSubscriptionStore)
            {
                string subScriptionArticleKey = ((IObjectWithKey)item).Key;
                if (string.Equals(articleKey, subScriptionArticleKey))
                {
                    Notification notification = new Notification(e, item.Subscriber);
                    notificationSource.Save(notification);
                }
            }
        }

    }

    public class Distributor
    {
        private NotificationStore notificationStore;
        public Distributor(NotificationStore notificationStore)
        {
            this.notificationStore = notificationStore;

            notificationStore.NewNotificationOccured += OnNewNotificationOccured;
        }

        public void OnNewNotificationOccured(object sender, NotificationEventArgs args)
        {
            Article article = args.Notification.Event.Article;
            ISubscriber subscriber = args.Notification.Subscriber;
            subscriber.Enque(article);
        }
    }

    #endregion
}
