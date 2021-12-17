using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubDemo.DataEntity
{
    /// <summary>
    /// 预定发布内容对象
    /// </summary>
    internal class Article : IObjectWithKey
    {
        private string category;
        private string content;
        public Article(string category,string content)
        {
            this.Category = category;
            this.Content = content;
        }
        public virtual string Key { get =>  category; }
        public string Category { get => category; set => category = value; }
        public string Content { get => content; set => content = value; }
    }
}
