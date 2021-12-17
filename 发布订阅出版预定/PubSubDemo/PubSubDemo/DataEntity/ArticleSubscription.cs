﻿using PubSubDemo.BusinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubDemo.DataEntity
{
    internal class ArticleSubscription : IObjectWithKey
    {
        private Article article;

        public virtual string Key => $"{((IObjectWithKey)article).Key}{subscriber.GetHashCode().ToString()}";

        internal Article Article { get => article; }
        internal ISubscriber Subscriber { get => subscriber;}

        private ISubscriber subscriber;
        public ArticleSubscription(Article article,ISubscriber subscriber)
        {
            this.article=article;
            this.subscriber=subscriber;
        }
    }
}
