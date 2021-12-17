using PubSubDemo.BusinessEntity;
using PubSubDemo.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubDemo.Implement
{
    /// <summary>
    /// 通知发送对象
    /// </summary>
    internal class Distributor
    {
        private NotificationStore notificationStore;
        public Distributor(NotificationStore notificationStore)
        {
            this.notificationStore = notificationStore;
            notificationStore.NewNotificationOccured += OnNewNotificationOccured;
        }

        private void OnNewNotificationOccured(object sender, NotificationEventArgs args)
        {
            Article article = args.Notification.Event.Article;
            ISubscriber subscriber = args.Notification.Subscriber;
            subscriber.Enqueue(article);
        }
    }
}
