using Demo_.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_
{
    public class ExceptionMVC : DeAbstractMVC
    {
        private AbstractMVC _mvc;
        public ExceptionMVC(AbstractMVC mvc) : base(mvc)
        {
            _mvc = mvc;
        }
        public override void Action()
        {
            Console.WriteLine("aciton执行前 监视错误日志");
            _mvc.Action();
            Console.WriteLine("aciton执行后 监视错误日志");
        }
    }
}
