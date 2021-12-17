using PubSubDemo.BusinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubDemo.DataEntity
{
    internal class Notification : IObjectWithKey
    {
        private Event e;
        public Event Event { get => e; }
        internal ISubscriber Subscriber { get => subscriber; }

        private ISubscriber subscriber;
        public Notification(Event e,ISubscriber subscriber)
        {
            this.e = e;
            this.subscriber = subscriber;
        }
        public string Key { get=>$"{e.ID}{subscriber.GetHashCode().ToString()}"; }
    }
}
