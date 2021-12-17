using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubDemo.DataEntity
{
    internal class Event : IObjectWithKey
    {
        public string id=Guid.NewGuid().ToString();
        public string ID => id;
        private Article article;

        public Event(Article article)
        {
            this.article = article;
        }
        public virtual string Key => ID;

        internal Article Article { get => article;}
    }
}
