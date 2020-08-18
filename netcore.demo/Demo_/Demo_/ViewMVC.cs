using Demo_.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_
{
    public class ViewMVC : DeAbstractMVC
    {
        private AbstractMVC _mvc;
        public ViewMVC(AbstractMVC mvc) : base(mvc)
        {
            _mvc = mvc;
        }
        public override void Action()
        {
            _mvc.Action();
            Console.WriteLine("动作执行后：返回view视图");
        }
    }
}
